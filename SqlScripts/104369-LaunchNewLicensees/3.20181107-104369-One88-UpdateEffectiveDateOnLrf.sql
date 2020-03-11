USE bodb_licensee
GO
/*
	 Created: 20181107@Amanda [redmine:#104369]
	 Task: Launch new licensees
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 - 20181107@Amanda: Update effective date for licensee and active system 'Accounting' for One88 [redmine:#104369]
	 
	 Developer's Note
		- Licensee Name: One88        
		- Licensee Type: Boping API licensee
		- Launch product: SB, NG, VS, VS2, RNG, Macau Games, Keno, Lottery, Allbet, Fishing World, Cash Out, BA
		- Launch date : 6 Nov, 2018
*/

-- Lauch Licensee
EXEC dbo.DBA_Lrf_LaunchLicensee 
	@LicenseeName = 'One88', 
    @AffectedDate = '2018-11-06'
GO

-- Lauch Product
-- 1.
EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88',
    @ProductCode = 'SB', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'NG', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'VS', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'VS2', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'CS', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'VB', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'KN', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'LT', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'AB', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'FW', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'CO', 
    @EffectiveDate = '2018-11-06'
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'One88', 
    @ProductCode = 'BA', 
    @EffectiveDate = '2018-11-06'
GO

-- Active Licensee Sys
EXEC dbo.DBA_Lrf_ActiveLicenseeSystem 
	@LicenseeName = 'One88', 
    @SystemisActive = 'Accounting'
GO
