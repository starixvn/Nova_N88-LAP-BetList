USE SiteConfig
GO

/*
	 Creator: 20180605@Amanda
	 Task: Map new subsidary with agents - [Redmine: #96453]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180605@Amanda : Map new subsidary with Bobo88UUS13 & Bobo88RM04 [redmine:#96453]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

-- Bobo88
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, Site, currency, 'fwkklive'
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('Bobo88UUS13', 'Bobo88RM04')

INSERT INTO Operator 
SELECT * FROM @Operator
 
GO