USE SiteConfig
GO

/*
	 Creator: 20180904@Amanda
	 Task: Map new subsidary with agents - [Redmine: #100850]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180904@Amanda : Map new subsidary with Bobo88TB02[redmine:#100850]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

-- Bobo88
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, 'Bobo88', currency, 'fwbn'
FROM CUSTDB.dbo.Customer WITH (NOLOCK) 
WHERE Username = 'Bobo88TB02' and custid = 32265040

INSERT INTO Operator (AgentID, SiteName, Currency, OperatorID)
SELECT AgentId, SiteName, currency, OperatorID FROM @Operator
 
GO