USE bodb_licensee
GO
/*
	Created: 20181128@Amanda
	Task: 18Luck - Set up new licensee - [Redmine: #105425]
	Database: DBCUST.bodb_licensee

	Revisions:
	 -Created@Amanda: Create new licensee on LRF [redmine:#105425]
	
	Developer's note:
		- Site Name: 18Luck -- Zelianta API
		- Product: Sportsbook, CashOut, Number Game, Virtual Sports (VS1 & VS2), BA
*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= '18Luck'
	,@SuperWildCard  	 	= '18LuckSuper%'
	,@ResellerName   	  	= '18Luck'
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