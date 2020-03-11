USE SiteConfig
GO

/*
	 Creator: 20180914@Amanda [redmine:#85460]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 - 20180914@Amanda: Shutdown affiliate for a few licensees [redmine:#85460]
	 
	 Developer's Note
	 - Remove AffiliateUrl and add Affiliate to Exclusions for all Site which do not support Affiliate
*/

-- Remove Affiliate Url
UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
SET AffiliateUrl = NULL
WHERE SiteName NOT IN ('itcbet','macaubet','aoncash','spondemo','athena000')

-- Add Affiliate to exclusion
UPDATE dbo.LicVendorProfile WITH(UPDLOCK, ROWLOCK)
SET Exclusions = '["AffiliateItems"]'
WHERE (Exclusions IS NULL OR Exclusions = '') AND SiteName NOT IN ('itcbet','macaubet','aoncash','spondemo','athena000')

UPDATE dbo.LicVendorProfile WITH(UPDLOCK, ROWLOCK)
SET Exclusions = '["AffiliateItems",' + RIGHT(Exclusions, LEN(Exclusions) - 1)
WHERE Exclusions NOT LIKE '%AffiliateItems%'
      AND Exclusions <> ''
      AND Exclusions IS NOT NULL
      AND SiteName NOT IN ('itcbet','macaubet','aoncash','spondemo','athena000')

GO