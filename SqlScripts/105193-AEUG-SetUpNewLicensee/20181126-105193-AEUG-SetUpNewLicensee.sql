USE bodb_licensee
GO
/*
	Created: 20181126@Amanda
	Task: AEUG - Set up new licensee - [Redmine: #105193]
	Database: DBCUST.bodb_licensee

	Revisions:
	 -20181126@Amanda: Create new licensee on LRF [redmine:#105193]
	
	Developer's note:
		- Site Name: AEUG -- Boping API
		- Product:  Sportsbook, Number Game, Virtual Sports, New Virtual Sports, Cash Out, and BA
*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'AEUG'
	,@SuperWildCard  	 	= 'AEUGSuper%'
	,@ResellerName   	  	= 'Boping'
	,@IsSpecial      	 	= 0
	,@ProductCode	  	 	= 'SB,NG,VS,VS2,CO,BA'
	,@SystemisActive 	  	= ''
	,@IsAllowedDW 		   	= 0
	,@IsOfferLas		    = 0
	,@IsAllowMemberComm     = 1
	,@IsAllowWLbyLeague	    = 0
	,@IsDeposit				= 1 
	,@IsPullService			= 0
GO