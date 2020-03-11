USE bodb_licensee
GO
/*
	 Created: 20181029@Amanda [redmine:#103860]
	 Task: Launch new licensees
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 - 20181029@Amanda: Update effective date for licensee and active system 'Accounting' for SSSGaming [redmine:#103860]
	 
	 Developer's Note
		- Licensee Name: SSSGaming        
		- Licensee Type: Boping API licensee
		- Launch product: SB, NG, VS, VS2, Cash Out, BA
		- Launch date :   Oct 25, 2018
*/

-- Lauch Licensee
EXEC dbo.DBA_Lrf_LaunchLicensee 
	@LicenseeName = 'SSSGaming', 
    @AffectedDate = '2018-10-25'
GO

-- Lauch Product
-- 1.
EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'SSSGaming',
    @ProductCode = 'SB', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'SSSGaming', 
    @ProductCode = 'NG', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'SSSGaming', 
    @ProductCode = 'VS', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'SSSGaming', 
    @ProductCode = 'VS2', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'SSSGaming', 
    @ProductCode = 'CO', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'SSSGaming', 
    @ProductCode = 'BA', 
    @EffectiveDate = '2018-10-25'
GO

-- Active Licensee Sys
EXEC dbo.DBA_Lrf_ActiveLicenseeSystem 
	@LicenseeName = 'SSSGaming', 
    @SystemisActive = 'Accounting'
GO
