USE bodb_licensee
GO

/*
	 Creator: 20180521@Amanda
	 Task: Remove Casino Progressive Wins report from 'Citybk', '11bet', '9wickets', 'Arlocash' [Redmine: #95197]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180521@Amanda : LRF - Remove Casino Progressive Wins report from 'Citybk', '11bet', '9wickets', 'Arlocash' [Redmine: #95197]
*/

DECLARE @FunctionID INT

DECLARE @Licensees AS TABLE (Id INT)

INSERT INTO @Licensees
SELECT LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName IN ('Citybk', '11bet', '9wickets', 'Arlocash')

-- CasinoProgressiveWins68
SELECT @FunctionID = FunctionId 
FROM dbo.Functions WITH (NOLOCK)
WHERE FunctionKey = 'CasinoProgressiveWins68'

-- Remove function
DELETE
FROM dbo.LicenseeFunctions
WHERE FunctionId = @FunctionID AND EXISTS (
		SELECT 1
		FROM @Licensees l
		WHERE l.Id = LicenseeID
)

DELETE
FROM dbo.LicenseeUserFunctions
WHERE FunctionId = @FunctionID AND EXISTS (
		SELECT 1
		FROM @Licensees l
		WHERE l.Id = LicenseeID
)

GO