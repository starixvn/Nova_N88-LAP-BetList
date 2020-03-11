USE SiteConfig
GO

/*
	 Creator: 20180705@Amanda
	 Task: Map new subsidary with agents - [Redmine: #97863]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180705@Amanda : Map new subsidary with 	Sunriseb2bUUS14 & Sunriseb2bINH02 & Sunriseb2bTB02 [redmine:#97863]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

-- Sunriseb2b
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID)
SELECT CustID, Site, currency, 'sb'
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('Sunriseb2bUUS14', 'Sunriseb2bINH02', 'Sunriseb2bTB02')

INSERT INTO Operator (AgentID, SiteName, Currency, OperatorID)
SELECT AgentId, SiteName, currency, OperatorID FROM @Operator
 
GO