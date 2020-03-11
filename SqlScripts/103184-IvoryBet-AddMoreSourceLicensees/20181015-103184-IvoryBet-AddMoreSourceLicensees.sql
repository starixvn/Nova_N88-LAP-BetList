use bodb_licensee
GO 

/*

	Creator: 20181015@Amanda
	Task: Add more data for Ivorybet - [Redmine: #103184]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20181015@Amanda : Add more data for Ivorybet [redmine:#103184]

*/

DECLARE @licenseeId INT
SELECT @licenseeId = LicenseeID FROM dbo.Licensees WITH (NOLOCK) WHERE LicenseeName = 'Ivorybet'

select @licenseeId

INSERT INTO LicenseeSourcedSetting  
(	SiteName,
	LicenseeId,
	SourcedLicenseeId,
	SourcedId,
	RoleId,
	SourcedCurrencyId,
	IsActive,
	EffectiveDate, 
	SourcedSiteName)
SELECT 'Ivorybet', @licenseeId, c.LicenseeID, c.CustID , c.RoleId, c.Currency , 1, '2018-10-15', l.LicenseeName
FROM dbo.LicenseeCust c WITH (NOLOCK) 
INNER JOIN dbo.Licensees l WITH (NOLOCK) ON l.LicenseeID = c.LicenseeID
WHERE l.LicenseeName IN ('Lucky88','MrCat','Happy1668')
AND c.Currency <> 20
AND c.Currency IN (13,4,2,51,15)
AND c.RoleID = 2

GO