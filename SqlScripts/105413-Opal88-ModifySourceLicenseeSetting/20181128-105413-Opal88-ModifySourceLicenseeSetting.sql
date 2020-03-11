USE bodb_Licensee
GO

/*
	Creator: 20181128@Amanda
	Task: Opal88- Modify sources of data [Redmine: #105413]
	Database: DBCUST.bodb_Licensee

	Revisions:
	- 20181128@Amanda: Stop getting data from Mayayule from 28 Nov till 10 Dec, and resume on 11 Dec  [Redmine: #105413]
*/

-- Update sourced licensees end date
UPDATE dbo.LicenseeSourcedSetting WITH (ROWLOCK, UPDLOCK)
SET EndDate = '2018-11-27 00:00:00'
WHERE SiteName = 'Opal88' AND SourcedSiteName = 'Mayayule' AND EndDate > '2018-11-27'

-- Insert new records with new date
INSERT INTO dbo.LicenseeSourcedSetting (SiteName, LicenseeId, SourcedLicenseeId, SourcedId, RoleId, SourcedCurrencyId, IsActive, EffectiveDate, SourcedSiteName, EndDate)
SELECT SiteName, LicenseeId, SourcedLicenseeId, SourcedId, RoleId, SourcedCurrencyId, IsActive, '2018-12-11 00:00:00', SourcedSiteName, '2050-01-01 00:00:00'
FROM dbo.LicenseeSourcedSetting l WITH (NOLOCK)
WHERE l.SiteName = 'Opal88' AND l.SourcedSiteName = 'Mayayule' AND EndDate = '2018-11-27 00:00:00'
AND NOT EXISTS (
			SELECT 1
			FROM dbo.LicenseeSourcedSetting l2 WITH (NOLOCK)
			WHERE l2.LicenseeID = l.LicenseeID 
					AND l2.SourcedId = l.SourcedId 
					AND l2.SourcedLicenseeID = l.SourcedLicenseeID 
					AND l2.SourcedCurrencyID = l.SourcedCurrencyID
					AND l2.EffectiveDate = '2018-12-11 00:00:00'
		)

GO