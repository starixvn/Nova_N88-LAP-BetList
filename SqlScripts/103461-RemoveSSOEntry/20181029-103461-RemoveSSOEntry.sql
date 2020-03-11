USE [bodb_Licensee]
GO

/*
	 Creator: 20181029@Joseph
	 Task: LAP - Remove SSO Entry
	 Database: DBCUST.bodb_Licensee

	 Revisions:
	 -20181029@Joseph: LRF - Remove internal login SP [redmine:#103461]
*/

EXEC sp_rename 'Lrf_InternalLogin', 'DEL_Lrf_InternalLogin', 'Object'

GO