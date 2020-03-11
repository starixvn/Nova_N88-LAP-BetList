USE SiteConfig
GO

/*
	 Creator: 20180907@Amanda
	 Task: Map new subsidary with agents - [Redmine: #101142]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180907@Amanda : Map new subsidary with Bobo88TB02[redmine:#101142]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

DECLARE @Agents AS TABLE (AgentId INT, UserName VARCHAR(50), OperatorID VARCHAR(50))
INSERT INTO @Agents (AgentId, UserName, OperatorID)
VALUES (29470756, 'Bobo88UUS14', 'fwnb'),
(29470758, 'Bobo88UUS15', 'fwjlh'),
(29469103, 'Bobo88RMB09', 'fwnb'),
(29469105, 'Bobo88RMB10', 'fwjlh')

-- Bobo88
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT c.CustID, 'Bobo88', c.currency, a.OperatorID
FROM CUSTDB.dbo.Customer c WITH (NOLOCK)
INNER JOIN @Agents a ON c.custid = a.AgentId AND c.username = a.UserName

INSERT INTO Operator (AgentID, SiteName, Currency, OperatorID)
SELECT AgentId, SiteName, currency, OperatorID FROM @Operator
 
GO