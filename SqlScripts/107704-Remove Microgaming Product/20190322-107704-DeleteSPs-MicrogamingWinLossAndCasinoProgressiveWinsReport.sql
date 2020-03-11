USE [bodb02]
GO

/*
	 Creator: 20190322@Amanda
	 Task: LAP - Remove Microgaming Product - [Redmine: #107704]
	 Database: DBEXT.bodb02

	 Revisions:
	 -20190322@Amanda : LRF - Remove SP of Microgaming Win Loss and Casino Progressive Wins report [redmine:#107704]
*/

-- Microgaming Win Loss
EXEC sp_rename 'LRF_Rpt_MicroGamingWinloss', 'DEL_LRF_Rpt_MicroGamingWinloss'

GO

-- Casino Progressive Wins
EXEC sp_rename 'Lrf_Rpt_CasinoProgressiveWins', 'DEL_Lrf_Rpt_CasinoProgressiveWins'

GO