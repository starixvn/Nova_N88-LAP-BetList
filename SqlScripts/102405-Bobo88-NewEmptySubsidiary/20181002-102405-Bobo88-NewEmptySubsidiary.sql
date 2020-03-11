USE SiteConfig
GO

/*
	 Creator: 20181002@Amanda
	 Task: Bobo88 - Create subsidiary agent accounts UUS - [Redmine: #102405]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20181002@Amanda : Add new operators [redmine:#102405]
*/

-- Bobo88
INSERT INTO dbo.Operator(AgentId, SiteName, currency)
VALUES	(32607965, 'Bobo88', 20),
		(32607976, 'Bobo88', 20),
		(32607983, 'Bobo88', 20),
		(32607992, 'Bobo88', 20),
		(32607996, 'Bobo88', 20)

GO