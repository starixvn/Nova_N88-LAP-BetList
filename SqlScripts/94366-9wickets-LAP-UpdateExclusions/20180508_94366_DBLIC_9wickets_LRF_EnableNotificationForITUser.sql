USE bodb_licensee
GO

/*
	 Creator: 20180508@Amanda
	 Task: 9wickets - LRF - Enable Result Amendment/Recalculation report for IT account[redmine:#94366]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180508@Amanda : 9wickets - LRF - Enable Result Amendment/Recalculation report for IT account[redmine:#94366]
*/

DECLARE @FunctionID INT,
		@LicenseeID INT

SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = '9wickets' -- 9wickets

SELECT @FunctionID = FunctionId
FROM dbo.Functions
WHERE FunctionKey = 'ResultAmendment_Recalculation75'

IF NOT EXISTS (SELECT 1 FROM dbo.LicenseeFunctions WHERE LicenseeId = @LicenseeID AND FunctionId = @FunctionID)
BEGIN
	INSERT INTO dbo.LicenseeFunctions (LicenseeId, FunctionId)
	VALUES(@LicenseeID, @FunctionID)
END

INSERT INTO dbo.LicenseeUserFunctions(UserId, FunctionId, LicenseeId)
SELECT lu.userid, @FunctionID, lu.LicenseeID
FROM dbo.LicenseeUser lu
INNER JOIN dbo.Users u WITH (NOLOCK) ON lu.UserID = u.UserID AND u.isInternal = 0 AND lu.isActive = 1
WHERE	lu.LicenseeID = @LicenseeID	
		AND NOT EXISTS (	
						SELECT 1 
						FROM LicenseeUserFunctions luf WITH (NOLOCK) 
						WHERE luf.LicenseeId = @LicenseeID AND luf.UserId = lu.UserID AND luf.FunctionId = @FunctionID
						)

GO
					