USE bodb_licensee
GO
/*
	Created: 20181022@Amanda
	Task: gbkj - Set up new licensee - [Redmine: #103492]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20181022@Amanda: Create new licensee on LRF [redmine:#103492]
	
	Developer's note:
		- Site Name: gbkj -- Boping API
		- Product: Sportsbook, CashOut, Number Game, Virtual Sports (VS1 & VS2), BA

*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'gbkj'
	,@SuperWildCard  	 	= 'gbkjSuper%'
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