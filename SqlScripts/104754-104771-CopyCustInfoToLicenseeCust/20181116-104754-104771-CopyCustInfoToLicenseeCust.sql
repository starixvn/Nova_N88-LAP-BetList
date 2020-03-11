USE bodb_Licensee
GO

/*
	Creator: 20181116@Amanda
	Task: Create Agent Id for subsidiary [Redmine: #104754 & #104771]
	Database: DBCUST.bodb_Licensee

	Revisions:
	- 20181116@Amanda: LRF - Copy data from CustInfo to LicenseeCust [Redmine: #104754 & #104771]
*/

-- #104754 - Callmeboss
EXEC [dbo].[DBA_CreateLicenseeAgent]
	@LicenseeName = 'Callmeboss',
	@SuperWildCard = 'CallmebossSuper%'


-- #104754 - IGPtech
EXEC [dbo].[DBA_CreateLicenseeAgent]
	@LicenseeName = 'IGPtech',
	@SuperWildCard = 'IGPtechSuper%',
	@Currency = 20 -- UUS

GO