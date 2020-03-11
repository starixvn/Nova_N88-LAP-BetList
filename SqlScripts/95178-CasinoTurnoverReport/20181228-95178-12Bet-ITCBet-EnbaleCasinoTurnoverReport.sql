USE bodb_licensee
GO

/*
	 Creator: 20181228@Joseph
	 Task: 12Bet & ICTBet - Enable Casino Turnover Report [Redmine: #95178]
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 - 20181228@Joseph: LRF - Enable BCasino Turnover Report for 12Bet & ICTBet[Redmine: #95178]
*/

DECLARE @LicenseeID INT, @FunctionID INT

SELECT @FunctionID = FunctionId
FROM dbo.Functions
WHERE FunctionKey = 'CasinoTurnover80'

-- 12bet
SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = '12Bet'

SELECT @FunctionID, @LicenseeID

INSERT INTO LicenseeFunctions (LicenseeId, FunctionId)
VALUES (@LicenseeID, @FunctionID)

INSERT INTO dbo.LicenseeUserFunctions(UserId,FunctionId,LicenseeId)
SELECT l.UserID,@FunctionID,l.LicenseeId
FROM dbo.LicenseeUser l WITH (NOLOCK)
WHERE l.isActive = 1 AND LicenseeId = @LicenseeID and l.UserId IN( SELECT UserId from Users WITH (NOLOCK) WHERE Username in('pgmAlex', 'pgmAnn', 'pgmPhuc', 'pgmJoseph'));

-- ITCBet
SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'ITCBet'

SELECT @FunctionID, @LicenseeID

INSERT INTO LicenseeFunctions (LicenseeId, FunctionId)
VALUES (@LicenseeID, @FunctionID)

INSERT INTO dbo.LicenseeUserFunctions(UserId,FunctionId,LicenseeId)
SELECT l.UserID,@FunctionID,l.LicenseeId
FROM dbo.LicenseeUser l WITH (NOLOCK)
WHERE l.isActive = 1 AND LicenseeId = @LicenseeID and l.UserId IN( SELECT UserId from Users WITH (NOLOCK) WHERE Username in('pgmAlex', 'pgmAnn', 'pgmPhuc', 'pgmJoseph'));

GO