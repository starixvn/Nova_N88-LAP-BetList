USE SiteConfig
GO

/*
	 Creator: 20180507@Amanda
	 Task: Mybet88 - Enable Cash out WL report for LAP [Redmine: #94658]
	 Database: DBCUST.SITECONFIG

	 Revisions:
	 -20180507@Amanda : Mybet88 - Enable Cash out WL report for LAP [redmine:#94658]
*/

UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","CasinoProgressiveWins68","WeeklyLiveMatches69","ColossusBetsWinLoss71","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = 'Mybet88'