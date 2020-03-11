USE bodb_licensee
GO

/*
	Created: 20180926@Amanda
	Task: Launch new licensee Callmeboss - [Redmine: #101912]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20180926@Amanda: Update effective date for licensee and active system 'Accounting' [redmine:#101912]
	
	Developer's note:
		- Site Name: Callmeboss  	 
		- Launch date :  Sep 20, 2018
		- Product: SB, Cash Out, Number Game, VS, VS2, BA

*/

-- Lauch Licensee
EXEC dbo.DBA_Lrf_LaunchLicensee 
	@LicenseeName = 'Callmeboss', 
    @AffectedDate = '2018-09-20'
GO

-- Lauch Product
-- 1.
EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Callmeboss', 
    @ProductCode = 'SB', 
    @EffectiveDate = '2018-09-20' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Callmeboss', 
    @ProductCode = 'CO', 
    @EffectiveDate = '2018-09-20' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Callmeboss', 
    @ProductCode = 'NG', 
    @EffectiveDate = '2018-09-20' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Callmeboss', 
    @ProductCode = 'VS', 
    @EffectiveDate = '2018-09-20' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Callmeboss', 
    @ProductCode = 'VS2', 
    @EffectiveDate = '2018-09-20' 
GO

EXEC dbo.DBA_Lrf_LaunchProduct 
	@LicenseeName = 'Callmeboss', 
    @ProductCode = 'BA' ,
    @EffectiveDate = '2018-09-20' 
GO

-- Active Licensee Sys
EXEC dbo.DBA_Lrf_ActiveLicenseeSystem 
	@LicenseeName = 'Callmeboss', 
    @SystemisActive = 'Accounting'
GO
