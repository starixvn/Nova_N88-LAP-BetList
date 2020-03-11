USE [bodb_LicenseeReportArchive]
/*
	 Creator: 20180823@Amanda
	 Task: Create new table dbo.rptDep_AuditTickets - [Redmine: #99777]
	 Database: DBLIC.bodb_LicenseeReport

	 Revisions:
	 -20180823@Amanda: Grant select permission on dbo.rptDep_AuditTickets [redmine:#99777]
*/
GO
GRANT SELECT ON [dbo].[rptDep_AuditTickets] TO [bodbLicenseeNet]
GO
GRANT SELECT ON [dbo].[rptDep_AuditTickets] TO [dot_member]
GO
