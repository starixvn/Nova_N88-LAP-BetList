USE bodb_licensee
GO
/*
	Created: 20181022@Amanda
	Task: skykey - Set up new licensee - [Redmine: #103566]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20181022@Amanda: Create new licensee on LRF [redmine:#103566]
	
	Developer's note:
		- Site Name: skykey -- ONEworks
		- Product: Sportsbook, CashOut, Number Game, Virtual Sports (VS1 & VS2)

*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'skykey'
	,@SuperWildCard  	 	= 'skykeySuper%'
	,@ResellerName   	  	= 'ONEworks'
	,@IsSpecial      	 	= 0
	,@ProductCode	  	 	= 'SB,CO,NG,VS,VS2'
	,@SystemisActive 	  	= ''
	,@IsAllowedDW 		   	= 0
	,@IsOfferLas		    = 0
	,@IsAllowMemberComm     = 1
	,@IsAllowWLbyLeague	    = 0
	,@IsDeposit				= 1 
	,@IsPullService			= 0
GO