USE bodb_Licensee
GO

/*
	Creator: 20181116@Amanda
	Task: Suspend source licensee for Opal88 [Redmine: #104861]
	Database: DBCUST.bodb_Licensee

	Revisions:
	- 20181116@Amanda: Suspend source licensee for Opal88 - Big Gaming, Macaubet (all currencies) [Redmine: #104861]
*/

-- end date for setting to be removed is Nov 15, 2018
UPDATE dbo.LicenseeSourcedSetting WITH (ROWLOCK, UPDLOCK)
SET EndDate = '2018-11-15 00:00:00'
WHERE SiteName = 'Opal88'
	  AND EndDate > '2018-11-15 00:00:00'
	  AND SourcedSiteName IN ('Macaubet', 'BigGaming')
GO