USE SiteConfig
GO

/*
	 Creator: 20180920@Amanda [redmine:#101856]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 - 20180920@Amanda: 12bet - Import LRF users' function permission and currency permission into LAP [redmine:#101856]
*/

CREATE TABLE #lrfUserFunction(UserSEQ INT, FunctionKey VARCHAR(100))
CREATE TABLE #lapUser(UserSEQ INT, UserId VARCHAR(50))

-- Get licenseeId
	DECLARE @licenseeId INT

	SELECT @licenseeId = l.LicenseeId
	FROM DBLIC.bodb_licensee.dbo.Licensees l WITH (NOLOCK)
	WHERE l.LicenseeName = '12bet'
-- End Get LicenseeId

-- Get LAP users which do not have group 
	INSERT INTO #lapUser(UserSEQ, UserId)
	SELECT u.UserSEQ, u.UserID
	FROM dbo.Users u WITH (NOLOCK)
	WHERE u.SiteName = '12bet' 
			AND NOT EXISTS (
				SELECT 1 
				FROM dbo.AuthorGroup ag WITH (NOLOCK) 
				WHERE ag.UserSEQ = u.UserSEQ
			)
-- End Get LAP users which do not have group 

-- Get LRF users and their permissions
	INSERT INTO #lrfUserFunction (UserSEQ, FunctionKey)
	SELECT lu.UserSEQ, f.FunctionKey
	FROM DBLIC.bodb_licensee.dbo.Users u WITH (NOLOCK)
		INNER JOIN #lapUser lu ON u.UserName = lu.UserID
		INNER JOIN DBLIC.bodb_licensee.dbo.LicenseeUserFunctions luf WITH (NOLOCK) ON luf.UserId = u.UserId
		INNER JOIN DBLIC.bodb_licensee.dbo.Functions f WITH (NOLOCK) ON f.FunctionId = luf.FunctionId
	WHERE luf.LicenseeId = @licenseeId 
			AND f.isActive = 1 
			AND f.Environment = 'LIVE' 
			AND f.ParentId <> 6
			AND f.ParentId > 0
-- End Get LRF users and their permissions

-- Insert LAP permission
	DECLARE @groupID INT

	SELECT @groupID = g.SEQ
	FROM dbo.UserGroup g WITH (NOLOCK)
	WHERE g.GroupName = 'Reporting' and g.SiteName = '12bet'

	MERGE dbo.UserFuncPermission AS dest
	USING ( 
		SELECT luf.UserSEQ, gf.FunctionKey, gf.SubFunctionKeys
		FROM #lrfUserFunction luf
		INNER JOIN dbo.GroupFuncPermission gf WITH (NOLOCK) ON luf.FunctionKey = gf.FunctionKey
		WHERE gf.UserGroupSEQ = @groupID AND gf.SiteName = '12bet') AS src
	ON (dest.UserSEQ = src.UserSEQ AND dest.FunctionKey = src.FunctionKey)
	WHEN NOT MATCHED BY TARGET THEN
	INSERT (UserSEQ, FunctionKey, SiteName, SystemType, SubFunctionKeys, IsAllow)
	VALUES (src.UserSEQ, src.FunctionKey, '12bet', 1, src.SubFunctionKeys, 1);
	
-- End Insert LAP permission

-- Insert LAP currency
	DECLARE @Currency VARCHAR(100)

	SELECT @Currency = ac.Currency
	FROM dbo.Users u WITH (NOLOCK)
		INNER JOIN dbo.AuthorCurrency ac WITH (NOLOCK) ON u.UserSEQ = ac.UserSEQ
	WHERE u.UserID = 'Administrator' and u.SiteName = '12bet'
		
	MERGE dbo.AuthorCurrency AS dest
	USING #lapUser AS src
	ON (dest.UserSEQ = src.UserSEQ)
	WHEN NOT MATCHED BY TARGET THEN
	INSERT (UserSEQ, Currency)
	VALUES (src.UserSEQ, @Currency)
	WHEN MATCHED AND dest.Currency <> @Currency THEN
	UPDATE SET dest.Currency = @Currency;
	
-- End Insert LAP currency

DROP TABLE #lrfUserFunction
DROP TABLE #lapUser
GO