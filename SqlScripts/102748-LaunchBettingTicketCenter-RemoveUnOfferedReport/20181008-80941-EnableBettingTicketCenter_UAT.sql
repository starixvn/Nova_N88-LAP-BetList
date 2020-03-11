USE bodb_licensee
GO

/*
	 Creator: 20180622@Amanda
	 Task: Add function Betting ticket center - [Redmine: #80941]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180622@Amanda : LRF - Add function Betting ticket center on UAT [redmine:#80941]
*/

-- Add new function
SET IDENTITY_INSERT Functions ON

INSERT INTO dbo.Functions 
(
	FunctionId,
	Name,
	Description,
	ParentId,
	Url,
	IsMenu,
	IsActive,
	DisplayOrder,
	IsNew,
	FunctionKey,
	Environment,
	IsFunction
)
VALUES (79, 'Betting Ticket Center', '', 3, 'BettingTicketCenter', 1, 1, 54, 1, 'BTC79', 'UAT', 1)
  
SET IDENTITY_INSERT Functions OFF

-- Add permission

DECLARE @FunctionID INT = 79

INSERT INTO LicenseeFunctions(LicenseeId, FunctionId)
SELECT LicenseeId, @FunctionID
FROM dbo.Licensees WITH (NOLOCK)
WHERE IsActive = 1; 

INSERT INTO dbo.LicenseeUserFunctions(UserId,FunctionId,LicenseeId)
SELECT l.UserID,@FunctionID,l.LicenseeId
FROM dbo.LicenseeUser l WITH (NOLOCK)
WHERE l.isActive = 1 and l.UserId IN( SELECT UserId from Users WITH (NOLOCK) WHERE Username in('pgmAlex', 'pgmAnn', 'pgmPhuc', 'pgmJoseph'));

GO