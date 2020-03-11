USE bodb_licensee
GO
/*
	Created: 20181010@Amanda
	Task: HJTK - Set up new licensee - [Redmine: #102932]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20181010@Amanda: Create new licensee on LRF [redmine:#102932]
	
	Developer's note:
		- Site Name: HJTK -- Boping
		- Product: Sportsbook, CashOut, Number Game, Virtual Sports (VS1 & VS2), BA

*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'HJTK'
	,@SuperWildCard  	 	= 'HJTKSuper%'
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