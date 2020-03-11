USE SiteConfig
GO

/*
	 Creator: 20180919@Amanda
	 Task: Remove products from licensee product list [Redmine: #98222]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20180919@Amanda: Remove Cash out Win Loss permission [redmine:#98222]
	 
	 Developer's Note
	 - Licensees: 'Ty777', 'Sunriseb2b', 'zt828', 'Bobo88'
*/

-- Add CashOutWinLoss72 to exclusion
UPDATE dbo.LicVendorProfile WITH(UPDLOCK, ROWLOCK)
SET Exclusions = '["CashOutWinLoss72"]'
WHERE (Exclusions IS NULL OR Exclusions = '') AND SiteName IN ('Ty777', 'Sunriseb2b', 'zt828', 'Bobo88')

UPDATE dbo.LicVendorProfile WITH(UPDLOCK, ROWLOCK)
SET Exclusions = LEFT(Exclusions, LEN(Exclusions) - 1) + ',"CashOutWinLoss72"]'
WHERE Exclusions NOT LIKE '%CashOutWinLoss72%'
      AND Exclusions <> ''
      AND Exclusions IS NOT NULL
      AND SiteName IN ('Ty777', 'Sunriseb2b', 'zt828', 'Bobo88')
      
-- Remove permission from users and groups
DELETE dbo.UserFuncPermission
WHERE FunctionKey = 'CashOutWinLoss72' AND SiteName IN ('Ty777', 'Sunriseb2b', 'zt828', 'Bobo88')

DELETE dbo.GroupFuncPermission
WHERE FunctionKey = 'CashOutWinLoss72' AND SiteName IN ('Ty777', 'Sunriseb2b', 'zt828', 'Bobo88')

GO