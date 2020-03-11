USE SiteConfig
GO

/*
	 Creator: 20180927@Amanda
	 Task: Map new subsidary with agents - [Redmine: #102253]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180927@Amanda : Map new subsidary with Bobo88US$05[redmine:#102253]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

-- Bobo88
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, 'Bobo88', currency, 'fwbn'
FROM CUSTDB.dbo.Customer WITH (NOLOCK) 
WHERE Username = 'Bobo88US$05' and custid = 29470864

INSERT INTO Operator (AgentID, SiteName, Currency, OperatorID)
SELECT AgentId, SiteName, currency, OperatorID FROM @Operator
 
GO