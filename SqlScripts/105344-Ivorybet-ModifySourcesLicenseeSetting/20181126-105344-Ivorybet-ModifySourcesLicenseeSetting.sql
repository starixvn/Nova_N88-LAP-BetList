USE bodb_Licensee
GO

/*
	Creator: 20181126@Amanda
	Task: Ivorybet - Modify sources of data for Ivorybet [Redmine: #105344]
	Database: DBCUST.bodb_Licensee

	Revisions:
	- 20181126@Amanda: Stop getting data from these licensees from Nov 27-Nov 30 2018 and restart from Dec 1 [Redmine: #105344]
		1.  Huangma
		2.  IBO
		3.  MrCat
		4.  Happy1668
		5.  Dawoo 
*/

-- Update sourced licensees end date
UPDATE dbo.LicenseeSourcedSetting WITH (ROWLOCK, UPDLOCK)
SET EndDate = '2018-11-26 00:00:00'
WHERE SiteName = 'Ivorybet' AND SourcedSiteName IN ('Huangma', 'IBO', 'MrCat', 'Happy1668', 'Dawoo') AND EndDate > '2018-11-26'

-- Insert new records with new date
INSERT INTO dbo.LicenseeSourcedSetting (SiteName, LicenseeId, SourcedLicenseeId, SourcedId, RoleId, SourcedCurrencyId, IsActive, EffectiveDate, SourcedSiteName, EndDate)
SELECT SiteName, LicenseeId, SourcedLicenseeId, SourcedId, RoleId, SourcedCurrencyId, IsActive, '2018-12-01 00:00:00', SourcedSiteName, '2050-01-01 00:00:00'
FROM dbo.LicenseeSourcedSetting l WITH (NOLOCK)
WHERE l.SiteName = 'Ivorybet' AND l.SourcedSiteName IN ('Huangma', 'IBO', 'MrCat', 'Happy1668', 'Dawoo') AND EndDate = '2018-11-26 00:00:00'
AND NOT EXISTS (
			SELECT 1
			FROM dbo.LicenseeSourcedSetting l2 WITH (NOLOCK)
			WHERE l2.LicenseeID = l.LicenseeID 
					AND l2.SourcedId = l.SourcedId 
					AND l2.SourcedLicenseeID = l.SourcedLicenseeID 
					AND l2.SourcedCurrencyID = l.SourcedCurrencyID
					AND l2.EffectiveDate = '2018-12-01 00:00:00'
		)

GO
