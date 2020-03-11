USE SiteConfig
GO

/*
	 Creator: 20180725@Amanda
	 Task: Map new subsidary with agents - [Redmine: #98825]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180725@Amanda : Map new subsidary with Sunriseb2bRMB13 & Sunriseb2bUUS16[redmine:#98825]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

-- Sunriseb2b
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID)
SELECT CustID, 'Sunriseb2b', currency, 'ph'
FROM CUSTDB.dbo.Customer WITH (NOLOCK)
WHERE (custid = 29740072 AND Username = 'Sunriseb2bRMB13') OR (custid = 29740147 AND Username = 'Sunriseb2bUUS16')

INSERT INTO Operator (AgentID, SiteName, Currency, OperatorID)
SELECT AgentId, SiteName, currency, OperatorID FROM @Operator

GO