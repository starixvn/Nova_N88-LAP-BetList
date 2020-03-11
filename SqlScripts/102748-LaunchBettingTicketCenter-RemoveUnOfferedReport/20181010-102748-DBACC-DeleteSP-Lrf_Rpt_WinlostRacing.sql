USE [bodb02]
GO

/*
	 Creator: 20181010@Amanda
	 Task: LAP - Several updates for reporting and bet list section - [Redmine: #102748]
	 Database: DBACC.bodb02

	 Revisions:
	 -20181010@Amanda : LRF - Remove SP of Daily Racing Win Loss [redmine:#102748]
*/

-- Daily Racing Win Loss
EXEC sp_rename 'Lrf_Rpt_WinlostRacing', 'DEL_Lrf_Rpt_WinlostRacing', 'Object'

GO