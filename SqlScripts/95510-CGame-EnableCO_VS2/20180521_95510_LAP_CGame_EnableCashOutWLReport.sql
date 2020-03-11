USE SiteConfig
GO

/*
	 Creator: 20180521@Amanda
	 Task: CGame - Enable VS2, Cash Out[Redmine: #95510]
	 Database: DBCUST.SITECONFIG

	 Revisions:
	 -20180521@Amanda : CGame - Enable Cash out WL report for LAP [redmine:#95510]
*/

UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","CasinoProgressiveWins68","ColossusBetsWinLoss71","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = 'CGame'