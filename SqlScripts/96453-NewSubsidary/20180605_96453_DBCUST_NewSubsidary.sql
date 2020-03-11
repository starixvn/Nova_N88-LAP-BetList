USE SiteConfig
GO

/*
	 Creator: 20180605@Amanda
	 Task:  Map new subsidary with agents - [Redmine: #96453]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20180605@Amanda : Map new subsidary with agents [redmine:#96453]
*/

DECLARE @Operator AS TABLE (AgentId INT, SiteName VARCHAR(50), currency INT, OperatorID VARCHAR(50))

-- Sunriseb2b
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, Site, currency, 'hp'   
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('Sunriseb2bUUS07', 'Sunriseb2bRMB05')
 
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, Site, currency, 'hk'   
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('Sunriseb2bUUS08', 'Sunriseb2bRMB06')
 
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, Site, currency, 'ha'   
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('Sunriseb2bUUS09', 'Sunriseb2bRMB07')
 
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, Site, currency, 'zy'   
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('Sunriseb2bUUS10', 'Sunriseb2bRMB08')
 
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, Site, currency, 'ld'   
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('Sunriseb2bUUS11', 'Sunriseb2bRMB09')
 
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, Site, currency, 'hc'   
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('Sunriseb2bUUS12', 'Sunriseb2bRMB10')

-- Aplus
INSERT INTO @Operator (AgentId, SiteName, currency, OperatorID )
SELECT CustID, Site, currency, 'BF'
FROM CUSTDB.dbo.Customer WITH (NOLOCK) where Username IN ('AplusUUS06', 'AplusRMB06')

INSERT INTO Operator 
SELECT * FROM @Operator
 
GO