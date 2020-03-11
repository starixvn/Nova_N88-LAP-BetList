USE bodb_licensee
GO
/*
	 Created: 20181029@Amanda [redmine:#103860]
	 Task: Launch new licensees
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 - 20181029@Amanda: Update effective date for licensee and active system 'Accounting' for ANGbet [redmine:#103860]
	 
	 Developer's Note
		- Licensee Name: ANGbet        
		- Licensee Type: Boping API licensee
		- Launch product: SB, NG, VS, VS2, Keno, Lottery, Cash Out, BA
		- Launch date :   Oct 25, 2018
*/

-- Lauch Licensee
EXEC dbo.DBA_Lrf_LaunchLicensee 
	@LicenseeName = 'ANGbet', 
    @AffectedDate = '2018-10-25'
GO

-- Lauch Product
-- 1.
EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'ANGbet',
    @ProductCode = 'SB', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'ANGbet', 
    @ProductCode = 'NG', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'ANGbet', 
    @ProductCode = 'VS', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'ANGbet', 
    @ProductCode = 'VS2', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'ANGbet', 
    @ProductCode = 'KN', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'ANGbet', 
    @ProductCode = 'LT', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'ANGbet', 
    @ProductCode = 'CO', 
    @EffectiveDate = '2018-10-25'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'ANGbet', 
    @ProductCode = 'BA', 
    @EffectiveDate = '2018-10-25'
GO

-- Active Licensee Sys
EXEC dbo.DBA_Lrf_ActiveLicenseeSystem 
	@LicenseeName = 'ANGbet', 
    @SystemisActive = 'Accounting'
GO
