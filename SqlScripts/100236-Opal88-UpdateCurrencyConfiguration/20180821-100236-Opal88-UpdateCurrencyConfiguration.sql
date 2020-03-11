USE bodb_Licensee
GO

/*
	Creator: 20180821@Amanda
	Task: Update currency configuration for Opal88 [Redmine: #100236]
	Database: DBLIC.bodb_Licensee

	Revisions:
	- 20180821@Amanda: Update currency configuration for Opal88 [Redmine: #100236]
*/

-- end date for currencies to be removed is today Aug 21, 2018
UPDATE dbo.LicenseeSourcedSetting WITH (ROWLOCK, UPDLOCK)
SET EndDate = '2018-08-21 00:00:00'
WHERE SiteName = 'Opal88'
	  AND EndDate > '2018-08-21 00:00:00'
	  AND SourcedCurrencyId NOT IN (2,4,13,15)
GO