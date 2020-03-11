USE bodb_licensee
GO
/*
	Created: 20181004@Amanda
	Task: VWIN66 - Set up new licensee - [Redmine: #102421]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20181004@Amanda: Create new licensee on LRF [redmine:#102421]
	
	Developer's note:
		- Site Name: VWIN66 -- Zelianta
		- Product: Sportsbook, Number Game, Virtual Sports X 2, BA and Cashout

*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'VWIN66'
	,@SuperWildCard  	 	= 'VWIN66Super%'
	,@ResellerName   	  	= 'VWIN66'
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