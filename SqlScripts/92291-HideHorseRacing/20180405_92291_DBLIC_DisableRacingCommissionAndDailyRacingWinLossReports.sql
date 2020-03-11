USE bodb_licensee
GO

/*
 Created: 20180405@Joseph
 Task: Hide Horse Racing in the backend - [Redmine #92291]
 Database: DBLIC.bodb_licensee
 Revisions:
 -20180405@Joseph: Disable two reports: Racing Commission and Daily Racing Commission
 
 Parameter's Explaination:

 Developer's Notes:
*/

UPDATE f set IsActive = 0
FROM [bodb_licensee].[dbo].[Functions] f
WHERE FunctionId in (34,35)