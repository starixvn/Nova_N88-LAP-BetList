USE SiteConfig
GO

/*
	 Creator: 20180712@Amanda
	 Task: Map new subsidary with agents - [Redmine: #98179]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180712@Amanda : Map new subsidary with Sunriseb2bRMB11 & Sunriseb2bUUS13[redmine:#98179]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

-- Sunriseb2b
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID)
SELECT CustID, Site, currency, 'dh'
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('Sunriseb2bRMB11', 'Sunriseb2bUUS13')

INSERT INTO Operator (AgentID, SiteName, Currency, OperatorID)
SELECT AgentId, SiteName, currency, OperatorID FROM @Operator
 
GO