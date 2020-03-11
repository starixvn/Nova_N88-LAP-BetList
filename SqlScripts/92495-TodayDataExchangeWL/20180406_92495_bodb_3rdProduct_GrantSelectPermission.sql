USE [bodb_3rdProduct]
GO

/*
 Created: 20180406@Amanda
 Task:  LAP - Support today's data for Exchange report - [Redmine: #92495]
 Database: DBLIC.bodb_3rdProduct

 Revisions:
	- 20180406@Amanda : Grant permision for bodbLicenseeNet
*/
GRANT SELECT ON [dbo].[Product3rd_SummarizedDateHistory] TO [bodbLicenseeNet] AS [dbo]
GO