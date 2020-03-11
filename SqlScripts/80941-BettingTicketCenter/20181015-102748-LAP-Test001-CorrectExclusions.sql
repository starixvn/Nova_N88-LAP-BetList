USE SiteConfig
GO

/*
	 Creator: 20181015@Amanda
	 Task: Scripts prepared for the launching package - [Redmine: #102748]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20181015@Amanda : LAP - Correct exclusions for site Test001 [redmine:#102748]
*/
	
	UPDATE dbo.LicVendorProfile WITH (ROWLOCK, UPDLOCK)
	SET Exclusions = '["AffiliateItems"]'
	WHERE vendorId = 'eifnfaj' AND siteName = 'Test001'
	
GO