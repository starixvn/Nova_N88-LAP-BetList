USE bodb_licensee
GO

/*
	 Creator: 20180424@Amanda
	 Task:  Aplus - Enable Subsidiary Performance report - [Redmine: #94187]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180424@Amanda : LRF - Open Subsidiary Performance for Aplus [redmine:#94187]
*/

DECLARE @LicenseeID INT, @FunctionID INT

SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'Aplus' -- Aplus

SELECT @FunctionID = FunctionId
FROM dbo.Functions
WHERE FunctionKey = 'SubsidiaryPerformance73'

SELECT @FunctionID, @LicenseeID

INSERT INTO LicenseeFunctions (LicenseeId, FunctionId)
VALUES (@LicenseeID, @FunctionID)

INSERT INTO LicenseeUserFunctions(UserId, FunctionId, LicenseeId)
SELECT lu.UserID, @FunctionID, @LicenseeID
FROM LicenseeUser lu
WHERE lu.LicenseeID = @LicenseeID	
	AND EXISTS (	SELECT 1 
					FROM dbo.Users u WITH (NOLOCK) 
					WHERE u.Username IN ( 'administration', 'admin')
						AND lu.UserID = u.UserID
				)
	AND NOT EXISTS (	SELECT 1 
						FROM LicenseeUserFunctions luf WITH (NOLOCK) 
						WHERE luf.LicenseeId = @LicenseeID AND luf.UserId = lu.UserID AND luf.FunctionId = @FunctionID
					)