USE SiteConfig
GO

/*
	 Creator: 20180508@Amanda
	 Task: Remove Casino Progressive Wins report from '9wickets','Citybk','HOT88','Haoying','BETSUN','Spondemo','Arlocash','11bet','Opal88' [Redmine: #95197]
	 Database: DBCUST.SITECONFIG

	 Revisions:
	 -20180508@Amanda : LAP - Update exclusions to remove Casino Progressive Wins report from '9wickets','Citybk','HOT88','Haoying','BETSUN','Spondemo','Arlocash','11bet','Opal88' [Redmine: #95197]
*/

-- 9wickets
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","CasinoProgressiveWins68","ColossusBetsWinLoss71","CashOutWinLoss72","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = '9wickets'

-- Citybk
GO
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","GoldDeluxeWinloss65","MicrogamingWinloss67","CasinoProgressiveWins68","ColossusBetsWinLoss71","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = 'Citybk'

-- HOT88
GO
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["New_Bonus_System","CasinoProgressiveWins68"]'
WHERE SiteName = 'HOT88'

-- Haoying
GO
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["New_Bonus_System","CasinoProgressiveWins68"]'
WHERE SiteName = 'Haoying'

-- BETSUN
GO
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["CasinoProgressiveWins68"]'
WHERE SiteName = 'BETSUN'


-- Spondemo
GO
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AGCasinoBalance63","CasinoProgressiveWins68","RacingCommission34","DailyRacingWinLoss35","New_Bonus_System"]'
WHERE SiteName = 'Spondemo'


-- Arlocash
GO
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","MicrogamingWinloss67","CasinoProgressiveWins68","ColossusBetsWinLoss71","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = 'Arlocash'

-- 11bet
GO
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","RacingCommission34","DailyRacingWinLoss35","WinLossByLeague59","AGCasinoWinLoss62","AGCasinoBalance63","GoldDeluxeWinloss65","MicrogamingWinloss67","CasinoProgressiveWins68","ColossusBetsWinLoss71","SubsidiaryPerformance73","ExchangeWinloss77"]'
WHERE SiteName = '11bet'

-- Opal88
GO
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET Exclusions = '["AffiliateItems","HandicapOverUnderLive7","MoneyLine8","OddEven1x2DND9","CorrectScore10","TotalGoal11","MixParlay12","Outright13","HTFT14","FGLG15","CleanSheet16","DoubleChance17","HomeDrawAwayNoBet18","BONteamtoscore19","Towintonil20","3-WayHandicap21","HandicapOverUnderLive24","1X225","LastBetsMonitoring26","SettledMatchCount28","CustomerWinLoss29","CustomerPendingFunds30","MatchWinLoss31","WinLossByProducts32","WinLossByBetType33","RacingCommission34","New_Bonus_System","CasinoProgressiveWins68"]'
WHERE SiteName = 'Opal88'

GO