USE bodb_licensee
GO

/*
	Created: 20180613@Amanda
	Task: Hide Top Customer, Win Loss report - [Redmine #96893]
	Database: DBLIC.bodb_licensee
 
	Revisions:
	- 20180613@Amanda: Hide Top Customer, Win Loss report - [Redmine #96893]
*/

UPDATE dbo.Functions WITH(UPDLOCK, ROWLOCK)
SET IsActive = 0
WHERE FunctionId in (44,45)

GO