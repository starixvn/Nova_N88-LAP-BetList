USE bodb_licensee
GO
/*
	 Created: 20181107@Amanda [redmine:#104369]
	 Task: Launch new licensees
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 - 20181107@Amanda: Update effective date for licensee and active system 'Accounting' for HJTK [redmine:#104369]
	 
	 Developer's Note
		- Licensee Name: HJTK        
		- Licensee Type: Boping API licensee
		- Launch product: SB, VS, VS2, NG, BA, Cash Out
		- Launch date :   5 Nov, 2018

*/

-- Lauch Licensee
EXEC dbo.DBA_Lrf_LaunchLicensee 
	@LicenseeName = 'HJTK', 
    @AffectedDate = '2018-11-05'
GO

-- Lauch Product
-- 1.
EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'HJTK',
    @ProductCode = 'SB', 
    @EffectiveDate = '2018-11-05'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'HJTK', 
    @ProductCode = 'VS', 
    @EffectiveDate = '2018-11-05'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'HJTK', 
    @ProductCode = 'VS2', 
    @EffectiveDate = '2018-11-05'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'HJTK', 
    @ProductCode = 'NG', 
    @EffectiveDate = '2018-11-05'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'HJTK', 
    @ProductCode = 'BA', 
    @EffectiveDate = '2018-11-05'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'HJTK', 
    @ProductCode = 'CO', 
    @EffectiveDate = '2018-11-05'
GO

-- Active Licensee Sys
EXEC dbo.DBA_Lrf_ActiveLicenseeSystem 
	@LicenseeName = 'HJTK', 
    @SystemisActive = 'Accounting'
GO
