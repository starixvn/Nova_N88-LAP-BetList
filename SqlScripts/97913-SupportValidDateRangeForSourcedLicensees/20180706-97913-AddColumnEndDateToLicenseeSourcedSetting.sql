USE bodb_licensee
GO
/*
	Creator: 20180706@Harper
	Task: LAP - Get data from sourced licensee following valid date range [Redmine: #97913]
	Database: DBLIC.bodb_Licensee

	Revisions:
	- 20180706@Harper: Add new column EndDate to LicenseeSourcedSetting [Redmine: #97913]
*/


-- Add New Column
ALTER TABLE dbo.LicenseeSourcedSetting ADD EndDate SMALLDATETIME NOT NULL DEFAULT ('2050-01-01')

-- Change cluster index from unique to non-unique
DROP INDEX PK_LicenseeSourcedSetting ON dbo.LicenseeSourcedSetting

-- Create new Clusterde index
CREATE CLUSTERED INDEX [PK_LicenseeSourcedSetting] ON [dbo].[LicenseeSourcedSetting]
(
 [LicenseeID] ASC,
 [SourcedId] ASC
)ON [licensee_table]

GO