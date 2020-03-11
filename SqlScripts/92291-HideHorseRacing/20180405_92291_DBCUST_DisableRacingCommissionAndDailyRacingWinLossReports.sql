USE SiteConfig;
GO

 /*
 Creator: 2018035@Aex
 Task:  Description - [Redmine: #92291]
 Database: DBCUST.SITECONFIG

 Revisions:
 -20180201@Alex : Hide report Racing commission and daily racing win loss

 Parameter's Explaination:

 Developer's Notes:
  - Notes
*/

-- Add daily racing winloss and racing commisison key to all licensees
UPDATE [LicVendorProfile] WITH(UPDLOCK, ROWLOCK)
  SET
      Exclusions = LEFT(Exclusions, LEN(Exclusions) - 1)+',"RacingCommission34","DailyRacingWinLoss35"]'
WHERE(Exclusions NOT LIKE '%DailyRacingWinLoss35%'
      OR Exclusions NOT LIKE '%RacingCommission34%')
     AND Exclusions != ''
     AND Exclusions IS NOT NULL;
UPDATE [LicVendorProfile] WITH(UPDLOCK, ROWLOCK)
  SET
      Exclusions = '["RacingCommission34","DailyRacingWinLoss35"]'
WHERE Exclusions IS NULL
      OR Exclusions = '';