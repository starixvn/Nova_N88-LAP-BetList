USE bodb_licensee
GO
/*
	Created: 20181102@Amanda
	Task: Reopen account "Justin" for all licensees - [Redmine: #104212]
	Database: DBCUST.bodb_licensee

	Revisions:
	 -20181102@Amanda: Reopen account "Justin" for all licensees [redmine:#104212]
*/

DECLARE @userId INT = 64

DECLARE @licenseeId TABLE (LicenseeId INT)

INSERT INTO @licenseeId (LicenseeId)
SELECT l.LicenseeID 
FROM dbo.Licensees l WITH (NOLOCK)
LEFT JOIN dbo.LicenseeUser lu WITH (NOLOCK) ON l.LicenseeID = lu.LicenseeID AND lu.UserID = @userId
WHERE l.IsActive = 1 AND lu.LicenseeID IS NULL

-- Add Licensees
INSERT INTO dbo.LicenseeUser(LicenseeID, UserID, isActive, PageAccess)
SELECT LicenseeID, @userId, 1, '' 
FROM @licenseeId

-- Add Function for user
;WITH CTE_Function AS (
	SELECT luf.FunctionId, luf.LicenseeId 
	FROM dbo.LicenseeUserFunctions luf WITH (NOLOCK)
	INNER JOIN @licenseeId li ON luf.LicenseeId = li.LicenseeId
	WHERE luf.UserId = @userId
)
INSERT INTO dbo.LicenseeUserFunctions(UserId, LicenseeId, FunctionId)
SELECT @userId, l.LicenseeID, f.FunctionId
FROM @licenseeId l
INNER JOIN dbo.LicenseeFunctions lf WITH (NOLOCK) ON l.LicenseeID = lf.LicenseeId
INNER JOIN dbo.Functions f WITH (NOLOCK) ON lf.FunctionId = f.FunctionId AND f.IsActive = 1 AND f.Environment = 'LIVE' AND f.ParentId <> 6
LEFT JOIN CTE_Function cte ON cte.LicenseeId = l.LicenseeId AND cte.FunctionId = f.FunctionId 
WHERE cte.FunctionId IS NULL
GROUP BY l.LicenseeID, f.FunctionId

-- Add Currency for user
DECLARE @Currency AS TABLE (LicenseeID INT, CurrencyID INT)

INSERT INTO @Currency (LicenseeID , CurrencyID)
SELECT DISTINCT l.LicenseeId, lc.Currency
FROM dbo.LicenseeCust lc WITH (NOLOCK)
INNER JOIN @licenseeId l ON lc.LicenseeID = l.LicenseeId 

;WITH CTE_Function AS (
	SELECT cu.CurrencyID, cu.LicenseeId FROM dbo.CurrencyUser cu WITH (NOLOCK) WHERE cu.usrID = @userId AND cu.SystemID = 1
)
INSERT INTO dbo.CurrencyUser (
	usrID
	, LicenseeID
	, CurrencyID
	, SystemID
	, isActive
	)
SELECT @userId
	, c.LicenseeID
	, c.CurrencyID
	, 1
	, 1
FROM @Currency c
WHERE NOT EXISTS (SELECT 1 FROM CTE_Function cte WHERE cte.CurrencyID = c.CurrencyID AND cte.LicenseeId = c.LicenseeId)

GO