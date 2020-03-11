USE bodb_licensee
GO
/*
	 Created: 20181008@Amanda [redmine:#102801]
	 Task: [Gameroom] New Licensee Launch (Zelianta)
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20181008@Amanda: Update effective date for licensee and active system 'Accounting' for Gameroom [redmine:#102801]
	 
	 Developer's Note
		- Licensee Name: Gameroom        
		- Licensee Type: Zelianta licensee
		- Launch product: SB, Cash Out, Number Game, VS, VS2, BA
		- Terms: no min fee, no waive setting
		- Launch date :  Oct 1, 2018

*/

-- Lauch Licensee
EXEC dbo.DBA_Lrf_LaunchLicensee 
	@LicenseeName = 'Gameroom', 
    @AffectedDate = '2018-10-01'
GO

-- Lauch Product
-- 1.
EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Gameroom',
    @ProductCode = 'SB', 
    @EffectiveDate = '2018-10-01' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Gameroom', 
    @ProductCode = 'NG', 
    @EffectiveDate = '2018-10-01' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Gameroom', 
    @ProductCode = 'VS', 
    @EffectiveDate = '2018-10-01' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Gameroom', 
    @ProductCode = 'VS2', 
    @EffectiveDate = '2018-10-01' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Gameroom', 
    @ProductCode = 'BA', 
    @EffectiveDate = '2018-10-01' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Gameroom', 
    @ProductCode = 'CO', 
    @EffectiveDate = '2018-10-01'
GO

-- Active Licensee Sys
EXEC dbo.DBA_Lrf_ActiveLicenseeSystem 
	@LicenseeName = 'Gameroom', 
    @SystemisActive = 'Accounting'
GO
