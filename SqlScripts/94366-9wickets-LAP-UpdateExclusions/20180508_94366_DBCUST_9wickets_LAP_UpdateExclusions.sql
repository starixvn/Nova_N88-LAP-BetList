USE SiteConfig
GO

/*
	 Creator: 20180508@Amanda
	 Task: 9wickets - Update Exclusions on LAP - Licensee only offer products: SB, OneRNG, Keno, Lottery [redmine:#94366]
	 Database: DBCUST.SITECONFIG

	 Revisions:
	 -20180508@Amanda : 9wickets - Update Exclusions on LAP [redmine:#94366]
*/

UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","ColossusBetsWinLoss71","CashOutWinLoss72","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = '9wickets'

GO