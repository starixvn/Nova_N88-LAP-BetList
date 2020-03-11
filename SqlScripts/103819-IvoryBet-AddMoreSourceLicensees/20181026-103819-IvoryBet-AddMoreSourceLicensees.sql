use bodb_licensee
GO 

/*

	Creator: 20181026@Amanda
	Task: Add more data for IvoryBet - [Redmine: #103819]
	Database: DBCUST.bodb_licensee

	Revisions:
	 -20181026@Amanda : Add more data for IvoryBet [redmine:#103819]

*/

DECLARE @licenseeId INT
SELECT @licenseeId = LicenseeID FROM dbo.Licensees WITH (NOLOCK) WHERE LicenseeName = 'IvoryBet'

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
SELECT 'IvoryBet', @licenseeId, c.LicenseeID, c.CustID , c.RoleId, c.Currency , 1, '2018-10-26', l.LicenseeName
FROM dbo.LicenseeCust c WITH (NOLOCK) 
INNER JOIN dbo.Licensees l WITH (NOLOCK) ON l.LicenseeID = c.LicenseeID
WHERE l.LicenseeName = 'Dawoo'
AND c.Currency <> 20
AND c.Currency IN (13,4,2,51,15)
AND c.RoleID = 2

GO