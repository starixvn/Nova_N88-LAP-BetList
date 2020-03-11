USE bodb_licensee
GO

/*
	 Creator: 20181009@Amanda
	 Task: Scripts prepared for the launching package - [Redmine: #102748]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20181009@Amanda : LRF - Launch Betting ticket center function and remove unused function [redmine:#102748]
*/

-- Open Betting ticket center function on PRO
	UPDATE dbo.Functions WITH (ROWLOCK, UPDLOCK)
	SET Environment = 'LIVE'
	WHERE FunctionId = 79

-- Delete unused key
	DECLARE @DeletedFunction TABLE (Id INT IDENTITY(1,1), FunctionId INT, FunctionKey NVARCHAR(100))

	INSERT INTO @DeletedFunction(FunctionId, FunctionKey)
	SELECT FunctionId, FunctionKey
	FROM dbo.Functions WITH (NOLOCK)
	WHERE FunctionKey IN (
				'RacingCommission34', 
				'DailyRacingWinLoss35', 
				'Balance42', 
				'WinLoss44', 
				'TopCustomer45', 
				'DATAWAREHOUSE5'
		)

	-- DELETE PERMISSION
	DELETE lf
	FROM dbo.LicenseeFunctions lf WITH (NOLOCK)
	INNER JOIN @DeletedFunction df ON lf.FunctionId = df.FunctionId
	
	DELETE lf
	FROM dbo.LicenseeUserFunctions lf WITH (NOLOCK)
	INNER JOIN @DeletedFunction df ON lf.FunctionId = df.FunctionId
	
	-- DELETE FUNCTION KEYS
	DELETE dbo.Functions
	WHERE FunctionId IN (SELECT FunctionId FROM @DeletedFunction)
GO