USE bodb_licensee
GO
/*
	Created: 20181024@Amanda
	Task: Dachen - Set up new licensee - [Redmine: #103731]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20181024@Amanda: Create new licensee on LRF [redmine:#103731]
	
	Developer's note:
		- Site Name: Dachen -- Boping API
		- Product: Sportsbook, Number Game, Virtual Sports (1&2) and BA

*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'Dachen'
	,@SuperWildCard  	 	= 'DachenSuper%'
	,@ResellerName   	  	= 'Boping'
	,@IsSpecial      	 	= 0
	,@ProductCode	  	 	= 'SB,NG,VS,VS2,BA'
	,@SystemisActive 	  	= ''
	,@IsAllowedDW 		   	= 0
	,@IsOfferLas		    = 0
	,@IsAllowMemberComm     = 1
	,@IsAllowWLbyLeague	    = 0
	,@IsDeposit				= 1 
	,@IsPullService			= 0
GO