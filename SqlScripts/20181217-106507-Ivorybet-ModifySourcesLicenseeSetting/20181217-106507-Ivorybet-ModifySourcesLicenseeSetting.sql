USE bodb_Licensee
GO

/*
	Creator: 20181217@Joseph
	Task: Ivorybet - Modify sources of data for Ivorybet [Redmine: #106507]
	Database: DBCUST.bodb_Licensee

	Revisions:
	- 20181217@Joseph: Stop getting data from some licensees from 18 Dec 2018 [Redmine: #106507]
*/

-- Update sourced licensees end date
UPDATE dbo.LicenseeSourcedSetting WITH (ROWLOCK, UPDLOCK)
SET EndDate = '2018-12-17 00:00:00'
WHERE SiteName = 'Ivorybet' AND SourcedSiteName IN ('Dawoo','Happy1668','Huangma','IBO','Abcasino','MrCat','AG','ibet') AND EndDate > '2018-12-17'

GO
