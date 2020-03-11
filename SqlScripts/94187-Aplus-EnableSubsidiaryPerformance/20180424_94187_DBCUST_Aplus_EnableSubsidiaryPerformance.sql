USE SiteConfig
GO

/*
	 Creator: 20180424@Amanda
	 Task:  Aplus - Enable Subsidiary Performance report - [Redmine: #94187]
	 Database: DBCUST.SITECONFIG

	 Revisions:
	 -20180424@Amanda : LAP - Open Subsidiary Performance for Aplus [redmine:#94187]
*/

UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoBalance63","MicrogamingWinloss67","CasinoProgressiveWins68","ColossusBetsWinLoss71","ExchangeWinloss77"]'
WHERE SiteName = 'Aplus'