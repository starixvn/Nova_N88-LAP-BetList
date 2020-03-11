USE bodb_licensee
GO

/*
	 Creator: 20180809@Amanda [redmine:#99678]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20180809@Amanda: Clean up terminated licensees from LRF Licensee List  [redmine:#99678]
	 
	 Developer's Note
	 - AK318, Wuming, Holey, WM8888
*/

-- Deactive System
UPDATE dbo.SystemLicensee WITH (ROWLOCK, UPDLOCK)
SET IsActive = 0
WHERE LicenseeID IN (
			SELECT LicenseeID
			FROM dbo.Licensees WITH (NOLOCK)
			WHERE LicenseeName IN ('AK318', 'Wuming', 'Holey', 'WM8888'))
	  AND IsActive = 1

-- Deactive pull service
DELETE bodb_PlaytechCS.dbo.LicIntegrationSetting
WHERE LicenseeName IN ('AK318', 'Wuming', 'Holey', 'WM8888')
	
-- Deactivate in Licensees table
UPDATE dbo.Licensees WITH (ROWLOCK, UPDLOCK)
SET IsActive = 0
WHERE LicenseeName IN ('AK318', 'Wuming', 'Holey', 'WM8888') AND IsActive = 1
GO