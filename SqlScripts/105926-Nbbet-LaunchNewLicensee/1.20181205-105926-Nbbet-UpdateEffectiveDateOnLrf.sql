USE bodb_licensee
GO
/*
	 Created: 20181205@Amanda [redmine:#105926]
	 Task: [Nbbet] New Licensee Launch (Zelianta)
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 - 20181205@Amanda: Update effective date for licensee and active system 'Accounting' for Nbbet [redmine:#105926]
	 
	 Developer's Note
		- Licensee Name: Nbbet        
		- Licensee Type: Zelianta licensee
		- Launch product: SB, NG, VSx2 , Cash Out
		- Terms: Min fee: EUR 5,000, Waiver: 6 months
		- Launch date :  3 Dec 2018

*/

-- Lauch Licensee
EXEC dbo.DBA_Lrf_LaunchLicensee 
	@LicenseeName = 'Nbbet', 
    @AffectedDate = '2018-12-03'
GO

-- Lauch Product
-- 1.
EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Nbbet',
    @ProductCode = 'SB', 
    @EffectiveDate = '2018-12-03' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Nbbet', 
    @ProductCode = 'NG', 
    @EffectiveDate = '2018-12-03' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Nbbet', 
    @ProductCode = 'VS', 
    @EffectiveDate = '2018-12-03' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Nbbet', 
    @ProductCode = 'VS2', 
    @EffectiveDate = '2018-12-03' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Nbbet', 
    @ProductCode = 'CO', 
    @EffectiveDate = '2018-12-03'
GO

-- Active Licensee Sys
EXEC dbo.DBA_Lrf_ActiveLicenseeSystem 
	@LicenseeName = 'Nbbet', 
    @SystemisActive = 'Accounting'
GO
