USE [bodb02]
GO

/*
	 Creator: 20181010@Amanda
	 Task: LAP - Several updates for reporting and bet list section - [Redmine: #102748]
	 Database: DBEXT.bodb02

	 Revisions:
	 -20181010@Amanda : LRF - Remove SP of Racing Commission and Balance [redmine:#102748]
*/

-- Daily Racing Win Loss
EXEC sp_rename 'Lrf_Rpt_CommByBettype_HR', 'DEL_Lrf_Rpt_CommByBettype_HR', 'Object'

GO

-- Balance
EXEC sp_rename 'Lrf_CustomerSelBalance', 'DEL_Lrf_CustomerSelBalance', 'Object'

GO