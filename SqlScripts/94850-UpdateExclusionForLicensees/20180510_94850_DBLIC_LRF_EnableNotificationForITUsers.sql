USE bodb_licensee
GO

/*
	 Creator: 20180510@Amanda
	 Task: LRF - Enable Result Amendment/Recalculation report for IT account[redmine:#94850]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20180510@Amanda : LRF - Enable Result Amendment/Recalculation report for IT account[redmine:#94850]
	 
	 Developer's Note:
	 - Licensees - jinmangguo, 24TECH, YBTech, Yibo, zt828, Xinbo, WBS, Arlocash
*/

DECLARE @LicenseeIds TABLE (LicenseeId INT)
INSERT INTO @LicenseeIds
SELECT LicenseeID 
FROM dbo.Licensees
WHERE LicenseeName IN ('jinmangguo', '24TECH', 'YBTech', 'Yibo', 'zt828', 'Xinbo', 'WBS', 'Arlocash')

DECLARE @FunctionID INT,
		@LicenseeID INT

SELECT @FunctionID = FunctionId
FROM dbo.Functions
WHERE FunctionKey = 'ResultAmendment_Recalculation75'

INSERT INTO dbo.LicenseeFunctions (LicenseeId, FunctionId)
SELECT l.LicenseeId, @FunctionID
FROM @LicenseeIds l
WHERE NOT EXISTS (SELECT 1 FROM dbo.LicenseeFunctions f WHERE f.LicenseeId = l.LicenseeId AND FunctionId = @FunctionID)

INSERT INTO dbo.LicenseeUserFunctions(UserId, FunctionId, LicenseeId)
SELECT lu.userid, @FunctionID, lu.LicenseeID
FROM dbo.LicenseeUser lu WITH (NOLOCK)
INNER JOIN @LicenseeIds l ON lu.LicenseeID = l.LicenseeId
INNER JOIN dbo.Users u WITH (NOLOCK) ON lu.UserID = u.UserID AND u.isInternal = 0 AND lu.isActive = 1
WHERE NOT EXISTS (	
						SELECT 1 
						FROM LicenseeUserFunctions luf WITH (NOLOCK) 
						WHERE luf.LicenseeId = lu.LicenseeID AND luf.UserId = lu.UserID AND luf.FunctionId = @FunctionID
					)

GO