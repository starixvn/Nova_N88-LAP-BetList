USE bodb_licensee
GO

/*
	 Creator: 20181120@Amanda
	 Task:  Enable Pragmatic Play - [Redmine: #105037]
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 -20181120@Amanda : LRF - Enable Pragmatic Play for Lucky88 [redmine:#105037]
*/

DECLARE @LicenseeID INT,
		@ResellerID INT

-- Lucky88
SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'Lucky88' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID, @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'PP',
	@EffectiveDate	= '2018-11-20'
	
GO