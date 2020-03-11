USE [bodb_Licensee]
GO

/*
	 Creator: 20181010@Amanda
	 Task: LAP - Several updates for reporting and bet list section - [Redmine: #102748]
	 Database: DBLIC.bodb_Licensee

	 Revisions:
	 -20181010@Amanda : LRF - Remove SP of Data WareHouse report [redmine:#102748]
*/

-- Ware house Win Loss
EXEC sp_rename 'Lrf_rpt_DWWinloss', 'DEL_Lrf_rpt_DWWinloss', 'Object'

GO
-- Top Customer
EXEC sp_rename 'Lrf_rpt_DWTopCustomerSel', 'DEL_Lrf_rpt_DWTopCustomerSel', 'Object'

GO