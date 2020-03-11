USE SiteConfig
GO

/*
	 Creator: 20180731@Joseph
	 Task: Map new subsidary with agents - [Redmine: #99192]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180731@Joseph : Map new subsidary with Sunriseb2bUUS17 & Sunriseb2bRMB14 [redmine:#99192]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

-- Sunriseb2b
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID)
SELECT CustID, Site, currency, 'x'
FROM CUSTDB.dbo.Customer WITH (NOLOCK)
WHERE (custid = 29740149 AND Username = 'Sunriseb2bUUS17') OR (custid = 29740078 AND Username = 'Sunriseb2bRMB14')

INSERT INTO Operator (AgentID, SiteName, Currency, OperatorID)
SELECT AgentId, SiteName, currency, OperatorID FROM @Operator

GO