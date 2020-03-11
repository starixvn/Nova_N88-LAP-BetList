USE bodb_Licensee
GO

/*
	Creator: 20181204@Amanda
	Task: Add EUR currency For GamingSoft [Redmine: #105794]
	Database: DBCUST.bodb_Licensee

	Revisions:
	- 20181204@Amanda: LRF - Copy data from CustInfo to LicenseeCust [Redmine: #105794]
*/

-- Gamingsoft
EXEC [dbo].[DBA_CreateLicenseeAgent]
	@LicenseeName = 'Gamingsoft',
	@SuperWildCard = 'GamingsoftSuper%',
	@Currency = 6 -- EUR

GO