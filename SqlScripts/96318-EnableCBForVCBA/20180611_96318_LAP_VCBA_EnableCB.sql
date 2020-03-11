USE SiteConfig
GO

/*
	 Creator: 20180611@Amanda
	 Task: VCBA - Enable CB[Redmine: #96318]
	 Database: DBCUST.SITECONFIG

	 Revisions:
	 - 20180611@Amanda : VCBA - Enable ColossusBetsWinLoss71 report for LAP [redmine:#96318]
*/

UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","CasinoProgressiveWins68","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = 'VCBA'