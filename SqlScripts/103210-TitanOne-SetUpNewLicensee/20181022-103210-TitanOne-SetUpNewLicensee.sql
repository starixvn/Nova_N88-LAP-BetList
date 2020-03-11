USE bodb_licensee
GO
/*
	Created: 20181022@Amanda
	Task: TitanOne - Set up new licensee - [Redmine: #103210]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20181022@Amanda: Create new licensee on LRF [redmine:#103210]
	
	Developer's note:
		- Site Name: TitanOne -- Zelianta
		- Product: Sportsbook, CashOut, Number Game, Virtual Sports (VS1 & VS2), BA
*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'TitanOne'
	,@SuperWildCard  	 	= 'TitanOneSuper%'
	,@ResellerName   	  	= 'TitanOne'
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