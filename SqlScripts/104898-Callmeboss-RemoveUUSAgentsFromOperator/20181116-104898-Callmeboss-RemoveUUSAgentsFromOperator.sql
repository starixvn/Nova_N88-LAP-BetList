USE SiteConfig
GO

/*
	 Creator: 20181116@Amanda
	 Task:  Remove UUS agents from Operator table [Redmine: #104898]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 -20181116@Amanda : Remove UUS agents from Operator table [Redmine: #104898]
*/

DELETE o
FROM dbo.Operator o WITH (ROWLOCK)
WHERE SiteName = 'Callmeboss' 
		AND Currency = 20 -- UUS
		AND ISNULL(OperatorID,'') = ''

GO