USE bodb_Licensee
GO

/*
	Creator: 20181127@Amanda
	Task: Create agent accounts for subsidiaries [Redmine: #105335]
	Database: DBCUST.bodb_Licensee

	Revisions:
	- 20181127@Amanda: LRF - Copy data from CustInfo to LicenseeCust [Redmine: #105335]
*/

-- AEUG
EXEC [dbo].[DBA_CreateLicenseeAgent]
	@LicenseeName = 'AEUG',
	@SuperWildCard = 'AEUGSuper%'


-- Bobo88
EXEC [dbo].[DBA_CreateLicenseeAgent]
	@LicenseeName = 'Bobo88',
	@SuperWildCard = 'Bobo88Super%'

GO