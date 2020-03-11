USE SiteConfig
GO

/*
	 Creator: 20180627@Amanda [redmine:#97435]
	 Database: DBCUST.SITECONFIG

	 Revisions:
	 - 20180627@Amanda: Update Exclusions on LAP for Licensees - BETSUN, Monaco, Haoying, HOT88  [redmine:#97435]
	 
	 Developer's Note
	 - BETSUN: SB,CO,NG,VS,VS2,BA - 94881
	 - Monaco: SB,CO,NG,VS,VS2 - 95566
	 - Haoying: SB,CO,NG,VS,VS2,BA - 95010
	 - HOT88: SB,CO,NG,VS,VS2 - 94976
*/

-- BETSUN, Monaco, Haoying, HOT88
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","CasinoProgressiveWins68","ColossusBetsWinLoss71","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName IN ('BETSUN', 'Monaco', 'Haoying', 'HOT88')

GO