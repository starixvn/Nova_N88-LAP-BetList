USE SiteConfig
GO

/*
	 Creator: 20190423@Joseph
	 Task: Remove Daily Betlist and Cancelled Bets functions [Redmine: #111554]
	 Database: DBCUST.SiteConfig

	 Revisions:
	 - 20190423@Joseph: Remove Daily Betlist and Cancelled Bets functions [Redmine: #111554]
*/

      
-- Remove permission from users and groups
DELETE dbo.UserFuncPermission
WHERE FunctionKey in ('CancelledBets36', 'DailyBetList37')

DELETE dbo.GroupFuncPermission
WHERE FunctionKey in ('CancelledBets36', 'DailyBetList37')

GO