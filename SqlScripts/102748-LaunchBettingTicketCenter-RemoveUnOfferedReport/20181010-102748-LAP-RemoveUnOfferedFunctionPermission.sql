USE SiteConfig
GO

/*
	 Creator: 20181009@Amanda
	 Task: Scripts prepared for the launching package - [Redmine: #102748]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20181009@Amanda : LAP - Remove Permission of Functions which are deleted [redmine:#102748]
*/

-- Delete unused key
	DECLARE @DeletedFunction TABLE (Id INT IDENTITY(1,1), FunctionKey NVARCHAR(100))

	INSERT INTO @DeletedFunction(FunctionKey)
	VALUES	('RacingCommission34'),
			('DailyRacingWinLoss35'),
			('Balance42'), 
			('WinLoss44'), 
			('TopCustomer45')

	-- DELETE PERMISSION
	DELETE gf
	FROM dbo.GroupFuncPermission gf WITH (NOLOCK)
	INNER JOIN @DeletedFunction df ON gf.FunctionKey = df.FunctionKey
	
	DELETE uf
	FROM dbo.UserFuncPermission uf WITH (NOLOCK)
	INNER JOIN @DeletedFunction df ON uf.FunctionKey = df.FunctionKey
GO