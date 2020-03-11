USE bodb_licensee
GO
/*
	 Created: 20181001@Amanda [redmine:#102375]
	 Task: [WINCLUB88] New Licensee Launch (Boping API)
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20181001@Amanda: Update effective date for licensee and active system 'Accounting' for WINCLUB88 [redmine:#102375]
	 
	 Developer's Note
		- Licensee Name: WINCLUB88        
		- Licensee Type: Boping API licensee
		- Launch product: Sportsbook, Virtual Sports x2, Number Game, BA, Cashout
		- Terms: no min fee, no waive setting
		- Launch date :  Sep 25, 2018
		- Enable on:
			  » NAP Report
			  » LRF
			  » LA
*/

-- Lauch Licensee
EXEC dbo.DBA_Lrf_LaunchLicensee 
	@LicenseeName = 'WINCLUB88', 
    @AffectedDate = '2018-09-25'
GO

-- Lauch Product
-- 1.
EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'WINCLUB88', 
    @ProductCode = 'SB', 
    @EffectiveDate = '2018-09-25' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'WINCLUB88', 
    @ProductCode = 'NG', 
    @EffectiveDate = '2018-09-25' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'WINCLUB88', 
    @ProductCode = 'VS', 
    @EffectiveDate = '2018-09-25' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'WINCLUB88', 
    @ProductCode = 'VS2', 
    @EffectiveDate = '2018-09-25' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'WINCLUB88', 
    @ProductCode = 'BA', 
    @EffectiveDate = '2018-09-25' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'WINCLUB88', 
    @ProductCode = 'CO', 
    @EffectiveDate = '2018-09-25'
GO

-- Active Licensee Sys
EXEC dbo.DBA_Lrf_ActiveLicenseeSystem 
	@LicenseeName = 'WINCLUB88', 
    @SystemisActive = 'Accounting'
GO
