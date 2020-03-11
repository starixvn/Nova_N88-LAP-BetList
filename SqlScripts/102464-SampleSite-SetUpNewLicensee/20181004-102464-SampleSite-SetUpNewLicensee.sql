USE bodb_licensee
GO
/*
	Created: 20181004@Amanda
	Task: SampleSite - Set up new licensee - [Redmine: #102464]
	Database: DBLIC.bodb_licensee

	Revisions:
	 -20181004@Amanda: Create new licensee on LRF [redmine:#102464]
	
	Developer's note:
		- Site Name: SampleSite -- Boping
		- Product: Sportsbook, Number Game, Virtual Sports X 2, BA and Cashout

*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'SampleSite'
	,@SuperWildCard  	 	= 'SampleSiteSuper%'
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