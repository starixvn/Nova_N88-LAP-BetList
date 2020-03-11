USE bodb_licensee
GO
/*
	Created: 20180926@Amanda
	Task: BetDeal - Set up new licensee - [Redmine: #102159]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20180926@Amanda: Create new licensee on LRF [redmine:#102159]
	
	Developer's note:
		- Site Name: BetDeal  	 
		- Zelianta licensee
		- Product:  Sportsbook, Number Game, Virtual Sports, Virtual Sports 2  and Cashout

*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'BetDeal'
	,@SuperWildCard  	 	= 'BetDealSuper%'
	,@ResellerName   	  	= 'BetDeal' -- Zelianta
	,@IsSpecial      	 	= 0
	,@ProductCode	  	 	= 'SB,NG,VS,VS2,CO'
	,@SystemisActive 	  	= ''
	,@IsAllowedDW 		   	= 0
	,@IsOfferLas		    = 0
	,@IsAllowMemberComm     = 1
	,@IsAllowWLbyLeague	    = 0
	,@IsDeposit				= 1 
	,@IsPullService			= 0
GO