USE SiteConfig
GO

/*
	 Creator: 20181009@Amanda
	 Task: Map agent accounts with subsidiaries - [Redmine: #102834]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20181009@Amanda : Map new subsidary for Bobo88[redmine:#102834]
*/

DECLARE @Operator AS TABLE (Id INT IDENTITY (1,1), AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

DECLARE @Agents AS TABLE (AgentId INT, UserName VARCHAR(50), OperatorID VARCHAR(50))
INSERT INTO @Agents (AgentId, UserName, OperatorID)
VALUES (29470760, 'Bobo88UUS16', 'R7'),
(29470958, 'Bobo88RM05', 'R7')

-- Bobo88
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT c.CustID, 'Bobo88', c.currency, a.OperatorID
FROM CUSTDB.dbo.Customer c WITH (NOLOCK)
INNER JOIN @Agents a ON c.custid = a.AgentId AND c.username = a.UserName

DECLARE @i INT = 1
WHILE (@i <= (select MAX(Id) FROM @Operator))
BEGIN
	DECLARE @AgentID INT
	SELECT @AgentID = AgentId FROM @Operator WHERE Id = @i
	
	IF NOT EXISTS (SELECT 1 FROM dbo.Operator WITH (NOLOCK) WHERE AgentID = @AgentID)
	BEGIN
		INSERT INTO dbo.Operator (AgentID, SiteName, Currency, OperatorID)
		SELECT AgentId, SiteName, currency, OperatorID FROM @Operator WHERE Id = @i
	END
	ELSE IF EXISTS (SELECT 1 FROM dbo.Operator WITH (NOLOCK) WHERE AgentID = @AgentID AND ISNULL(OperatorID,'') = '' )
	BEGIN
		UPDATE dbo.Operator WITH (ROWLOCK, UPDLOCK)
		SET OperatorID = 'R7'
		WHERE AgentID = @AgentID
	END
		
	SET @i = @i + 1
END
 
GO