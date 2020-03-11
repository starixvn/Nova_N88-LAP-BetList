USE bodb_licensee
GO
/*
	 Created: 20181113@Amanda [redmine:#104708]
	 Task: [BetDeal] New Licensee Launch (Zelianta)
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 - 20181113@Amanda: Update effective date for licensee and active system 'Accounting' for BetDeal [redmine:#104708]
	 
	 Developer's Note
		- Licensee Name: BetDeal        
		- Licensee Type: Zelianta licensee
		- Launch product: Sportsbook, Number Game, Virtual Sports (1&2),  Cashout
		- Terms: Min fee: EUR 5,000, Waiver: 6 months
		- Launch date :  12 Nov, 2018

*/

-- Lauch Licensee
EXEC dbo.DBA_Lrf_LaunchLicensee 
	@LicenseeName = 'BetDeal', 
    @AffectedDate = '2018-11-12'
GO

-- Lauch Product
-- 1.
EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'BetDeal',
    @ProductCode = 'SB', 
    @EffectiveDate = '2018-11-12' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'BetDeal', 
    @ProductCode = 'NG', 
    @EffectiveDate = '2018-11-12' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'BetDeal', 
    @ProductCode = 'VS', 
    @EffectiveDate = '2018-11-12' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'BetDeal', 
    @ProductCode = 'VS2', 
    @EffectiveDate = '2018-11-12' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'BetDeal', 
    @ProductCode = 'CO', 
    @EffectiveDate = '2018-11-12'
GO

-- Active Licensee Sys
EXEC dbo.DBA_Lrf_ActiveLicenseeSystem 
	@LicenseeName = 'BetDeal', 
    @SystemisActive = 'Accounting'
GO
