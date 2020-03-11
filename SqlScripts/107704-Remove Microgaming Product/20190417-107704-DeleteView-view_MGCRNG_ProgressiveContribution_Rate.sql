USE [bodb_LicenseeReport]
GO

/*
	 Creator: 20190322@Amanda
	 Task: LAP - Remove Microgaming Product - [Redmine: #107704]
	 Database: DBLIC.bodb_LicenseeReport

	 Revisions:
	 -20190417@Amanda : LRF - Remove view view_MGCRNG_ProgressiveContribution_Rate [redmine:#107704]
*/

-- view_MGCRNG_ProgressiveContribution_Rate
EXEC sp_rename 'view_MGCRNG_ProgressiveContribution_Rate', 'DEL_view_MGCRNG_ProgressiveContribution_Rate'

GO