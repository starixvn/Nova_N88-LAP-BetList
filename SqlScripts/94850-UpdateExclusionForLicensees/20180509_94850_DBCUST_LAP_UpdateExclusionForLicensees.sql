USE SiteConfig
GO

/*
	 Creator: 20180509@Amanda [redmine:#94850]
	 Database: DBCUST.SITECONFIG

	 Revisions:
	 - 20180509@Amanda: Update Exclusions on LAP for Licensees - jinmangguo, 24TECH, YBTech, Yibo, zt828, Xinbo, WBS, Arlocash, 9wickets [redmine:#94850]
	 
	 Developer's Note
	 - jinmangguo: SB,CO,NG,VS,VS2 - 92621
	 - 24TECH: SB,CO,NG,VS,VS2,BA - 93635
	 - YBTech: SB,CO,NG,VS,VS2,BA - 93666
	 - Yibo: SB,CO,NG,VS,VS2 - 93717
	 - zt828: SB,CO,NG,VS,VS2 - 91878
	 - Xinbo: SB,CO,NG,VS,VS2,BA - 94276
	 - WBS: SB,CO,NG,VS,VS2,BA - 92962
	 - Arlocash: SB,CO,NG,VS,VS2,GD,CS,VG,AB,KN,LT,FW,BA - 92892
	 - 9wickets: SB,CS,KN,LT - 94366
*/

-- jinmangguo, Yibo, zt828
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","CasinoProgressiveWins68","ColossusBetsWinLoss71","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName IN ('jinmangguo', 'Yibo', 'zt828')

GO
-- 24TECH, YBTech, Xinbo, WBS
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","CasinoProgressiveWins68","ColossusBetsWinLoss71","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName IN ('24TECH', 'YBTech', 'Xinbo', 'WBS')

GO
-- Arlocash
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","MicrogamingWinloss67","ColossusBetsWinLoss71","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = 'Arlocash'

GO
-- 9wickets
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","ColossusBetsWinLoss71","CashOutWinLoss72","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = '9wickets'

GO
