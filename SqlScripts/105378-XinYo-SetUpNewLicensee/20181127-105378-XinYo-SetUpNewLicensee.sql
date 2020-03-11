USE bodb_licensee
GO
/*
	Created: 20181127@Amanda
	Task: XinYo - Set up new licensee - [Redmine: #105378]
	Database: DBCUST.bodb_licensee

	Revisions:
	 -20181127@Amanda: Create new licensee on LRF [redmine:#105378]
	
	Developer's note:
		- Site Name: XinYo -- Zelianta API
		- Product: Sportsbook, CashOut, Number Game, Virtual Sports (VS1 & VS2), BA
*/

-- Create new licensee
EXEC [DBA_Lrf_CreateNewLicensee]
	@LicenseeName   		= 'XinYo'
	,@SuperWildCard  	 	= 'XinYoSuper%'
	,@ResellerName   	  	= 'XinYo'
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