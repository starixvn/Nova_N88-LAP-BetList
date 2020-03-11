USE bodb_licensee
GO
/*
	Created: 20181109@Amanda
	Task: laba360 - Set up new licensee - [Redmine: #104524]
	Database: DBCUST.bodb_licensee

	Revisions:
	 -20181109@Amanda: Create new licensee on LRF [redmine:#104524]
	
	Developer's note:
		- Site Name: laba360 -- Boping API
		- Product: Sportsbook, Cashout, Number Game, VS1 & VS2, BA

*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'laba360'
	,@SuperWildCard  	 	= 'laba360Super%'
	,@ResellerName   	  	= 'Boping'
	,@IsSpecial      	 	= 0
	,@ProductCode	  	 	= 'SB,CO,NG,VS,VS2,BA'
	,@SystemisActive 	  	= ''
	,@IsAllowedDW 		   	= 0
	,@IsOfferLas		    = 0
	,@IsAllowMemberComm     = 1
	,@IsAllowWLbyLeague	    = 0
	,@IsDeposit				= 1 
	,@IsPullService			= 0
GO