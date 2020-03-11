USE bodb_Licensee
GO

/*
	Creator: 20180709@Amanda
	Task: LAP - Get data from sourced licensee following valid date range [Redmine: #97913]
	Database: DBLIC.bodb_Licensee

	Revisions:
	- 20180709@Amanda: Suspend sourced licensee Macaubet of Opal88 in the source table from July 10 2018 to Jul 27 2018 [Redmine: #97913]
*/


-- Update sourced licensee(Macaubet)end date of Opal88
IF NOT EXISTS (SELECT 1 FROM dbo.LicenseeSourcedSetting WITH (NOLOCK) WHERE SiteName = 'Opal88' AND SourcedSiteName = 'Macaubet' AND EffectiveDate = '2018-02-01 00:00:00' AND EndDate = '2018-07-09 00:00:00')
BEGIN
	UPDATE dbo.LicenseeSourcedSetting WITH (ROWLOCK, UPDLOCK)
	SET EndDate = '2018-07-09 00:00:00'
	WHERE SiteName = 'Opal88' AND SourcedSiteName = 'Macaubet' AND EffectiveDate = '2018-02-01 00:00:00' AND EndDate = '2050-01-01 00:00:00'
END

-- Insert new records of Macaubet for Opal88 with new date range
IF NOT EXISTS (SELECT 1 FROM dbo.LicenseeSourcedSetting WITH (NOLOCK) WHERE SiteName = 'Opal88' AND SourcedSiteName = 'Macaubet' AND EffectiveDate = '2018-07-28 00:00:00' AND EndDate = '2050-01-01 00:00:00')
BEGIN
	INSERT INTO dbo.LicenseeSourcedSetting (SiteName, LicenseeId, SourcedLicenseeId, SourcedId, RoleId, SourcedCurrencyId, IsActive, EffectiveDate, SourcedSiteName, EndDate)
	SELECT SiteName, LicenseeId, SourcedLicenseeId, SourcedId, RoleId, SourcedCurrencyId, IsActive, '2018-07-28 00:00:00', SourcedSiteName, '2050-01-01 00:00:00'
	FROM dbo.LicenseeSourcedSetting l WITH (NOLOCK)
	WHERE l.SiteName = 'Opal88' AND l.SourcedSiteName = 'Macaubet' AND EndDate = '2018-07-09 00:00:00' AND EffectiveDate = '2018-02-01 00:00:00'
END

GO
