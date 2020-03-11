USE bodb_licensee
GO

/*
	 Creator: 20180611@Amanda
	 Task:  VCBA - Enable CB and Colossus Bets Win Loss report - [Redmine: #96318]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180611@Amanda : LRF - Enable CB and Colossus Bets Win Loss report for VCBA [redmine:#96318]
*/

DECLARE @LicenseeID INT,
		@ResellerID INT

-- VCBA

SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'VCBA' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID, @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'CB',
	@EffectiveDate	= ''
	
GO