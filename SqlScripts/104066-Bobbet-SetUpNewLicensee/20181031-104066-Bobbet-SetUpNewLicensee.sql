USE bodb_licensee
GO
/*
	Created: 20181031@Amanda
	Task: Bobbet - Set up new licensee - [Redmine: #104066]
	Database: DBCUST.bodb_licensee

	Revisions:
	 -20181031@Amanda: Create new licensee on LRF [redmine:#104066]
	
	Developer's note:
		- Site Name: Bobbet -- Boping API
		- Product: Sportsbook, Number Game, Virtual Sports x2, Cash out and BA

*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'Bobbet'
	,@SuperWildCard  	 	= 'BobbetSuper%'
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