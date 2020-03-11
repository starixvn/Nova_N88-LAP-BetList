USE bodb_licensee
GO

/*
	 Creator: 20190108@Joseph
	 Task: Enable function Casino Turnover report for Production - [Redmine: #95178]
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 -20100108@Joseph : LAP - Enable function Casino Turnover report for Production - [Redmine: #95178]
*/

  UPDATE [bodb_licensee].[dbo].[Functions] 
  SET Environment = 'LIVE' 
  WHERE FunctionId = 80

  GO