USE SiteConfig
GO

/*
	 Creator: 20180919@Amanda
	 Task: Remove products from licensee product list [Redmine: #98222]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20180919@Amanda: LAP - Remove SpecialBA54 Permission for Bobo88 [redmine:#98222]

*/

-- Add SpecialBA54 to exclusion
IF EXISTS (SELECT 1 FROM dbo.LicVendorProfile WITH (NOLOCK) WHERE (Exclusions IS NULL OR Exclusions = '') AND SiteName = 'Bobo88')
BEGIN
	UPDATE dbo.LicVendorProfile WITH(UPDLOCK, ROWLOCK)
	SET Exclusions = '["SpecialBA54"]'
	WHERE SiteName = 'Bobo88'
END
ELSE IF NOT EXISTS (SELECT 1 FROM dbo.LicVendorProfile WITH (NOLOCK) WHERE Exclusions LIKE '%SpecialBA54%' AND SiteName = 'Bobo88')
BEGIN
	UPDATE dbo.LicVendorProfile WITH(UPDLOCK, ROWLOCK)
	SET Exclusions = LEFT(Exclusions, LEN(Exclusions) - 1) + ',"SpecialBA54"]'
	WHERE SiteName = 'Bobo88'
END

-- Remove permission from users and groups
DELETE dbo.UserFuncPermission
WHERE FunctionKey = 'SpecialBA54' AND SiteName = 'Bobo88'

DELETE dbo.GroupFuncPermission
WHERE FunctionKey = 'SpecialBA54' AND SiteName = 'Bobo88'

GO