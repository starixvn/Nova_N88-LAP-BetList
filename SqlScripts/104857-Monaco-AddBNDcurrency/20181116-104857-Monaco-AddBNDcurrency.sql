USE bodb_Licensee
GO

/*
	Creator: 20181116@Amanda
	Task: [Monaco] Add BND currency [Redmine: #104857]
	Database: DBCUST.bodb_Licensee

	Revisions:
	- 20181116@Amanda: LRF - Add BND currency for Monaco [Redmine: #104857]
*/

EXEC [dbo].[DBA_CreateLicenseeAgent]
	@LicenseeName = 'Monaco',
	@SuperWildCard = 'MonacoSuper%',
	@Currency = 46 -- BND

GO