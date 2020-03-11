USE SiteConfig
GO

/*
	 Creator: 20180716@Amanda
	 Task: Map new subsidary with agents - [Redmine: #98292]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180716@Amanda : Map new subsidary with Sunriseb2bRMB12 & Sunriseb2bUUS15[redmine:#98292]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

-- Sunriseb2b
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID)
SELECT CustID, Site, currency, 'yg'
FROM CUSTDB.dbo.Customer WITH (NOLOCK)
WHERE (custid = 29536603 AND Username = 'Sunriseb2bRMB12') OR (custid = 29740145 AND Username = 'Sunriseb2bUUS15')

INSERT INTO Operator (AgentID, SiteName, Currency, OperatorID)
SELECT AgentId, SiteName, currency, OperatorID FROM @Operator

GO