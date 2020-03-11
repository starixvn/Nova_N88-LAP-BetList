USE bodb_LicenseeReportArchive
GO

/*
	 Creator: 20180820@Amanda
	 Task: Create new table dbo.rptDep_AuditTickets - [Redmine: #99777]
	 Database: DBLIC.bodb_LicenseeReport

	 Revisions:
	 -20180820@Amanda: Create new table dbo.rptDep_AuditTickets [redmine:#99777]
*/
GO
CREATE TABLE [dbo].[rptDep_AuditTickets]
(
	[PlayerNo]			INT				NOT NULL
   ,[PlayerName]		VARCHAR(100)
   ,[PlayerType]		VARCHAR(10)	 
   ,[TransactionDate]	DATETIME
   ,[TransactionId]		BIGINT			NOT NULL	
   ,[SettlementDate]	DATETIME		NOT NULL
   ,[Brand]				VARCHAR(50)
   ,[BrandUrl]			VARCHAR(100)
   ,[GameCode]			VARCHAR(10)
   ,[GameName]			VARCHAR(100)
   ,[GameType]			VARCHAR(50)		NOT NULL
   ,[CurrencyCode]		VARCHAR(10)
   ,[BetType]			VARCHAR(100)
   ,[BetAmount]		MONEY
   ,[DebitAmount]		MONEY
   ,[CreditAmount]		MONEY
   ,[NetAmount]		MONEY
   ,[WinLostDate]	     DATE NOT NULL
	CONSTRAINT [PK_rptDep_AuditTickets] PRIMARY KEY NONCLUSTERED
	(
		[TransactionId] ASC
	) ON  [bodb_LicenseeReportArchive_table] -- local: Licensee_Staging
)

CREATE CLUSTERED INDEX [CIX_playerNo] ON [dbo].[rptDep_AuditTickets]
(   
	[WinLostDate],
	[Brand] ASC,
	[CurrencyCode] ASC,
	[GameType] ASC
) ON [bodb_LicenseeReportArchive_table] -- local :Licensee_Staging

GO
