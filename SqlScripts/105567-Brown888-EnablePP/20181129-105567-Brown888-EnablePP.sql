USE bodb_licensee
GO

/*
	 Creator: 20181129@Amanda
	 Task:  Enable Pragmatic Play - [Redmine: #105567]
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 - 20181129@Amanda : LRF - Enable Pragmatic Play for Brown888 [redmine:#105567]
*/

DECLARE @LicenseeID INT,
		@ResellerID INT

-- Brown888
SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'Brown888' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID, @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'PP',
	@EffectiveDate	= '2018-12-03'
	
GO