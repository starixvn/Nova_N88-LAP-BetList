USE bodb_licensee
GO
/*
	Created: 20181128@Amanda
	Task: Mbet - Set up new licensee - [Redmine: #105472]
	Database: DBCUST.bodb_licensee

	Revisions:
	 -20181128@Amanda: Create new licensee on LRF [redmine:#105472]
	
	Developer's note:
		- Site Name: Mbet -- Boping API
		- Product: Sportsbook, Number Game, Virtual Sports, New Virtual Sports, GD, GG Fishing, Cash Out and BA
*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'Mbet'
	,@SuperWildCard  	 	= 'MbetSuper%'
	,@ResellerName   	  	= 'Boping'
	,@IsSpecial      	 	= 0
	,@ProductCode	  	 	= 'SB,NG,VS,VS2,GD,FW,CO,BA'
	,@SystemisActive 	  	= ''
	,@IsAllowedDW 		   	= 0
	,@IsOfferLas		    = 0
	,@IsAllowMemberComm     = 1
	,@IsAllowWLbyLeague	    = 0
	,@IsDeposit				= 1 
	,@IsPullService			= 0
	
EXEC DBPTCS.bodb_PlaytechCS.[dbo].[DBA_EnableGoldDeluxeService] @LicenseeName = 'Mbet', @SettingName ='GoldDeluxe_VersionKey'
GO