USE bodb_licensee
GO

/*
	 Creator: 20181204@Amanda
	 Task:  Enable CashOut - [Redmine: #105836]
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 - 20181204@Amanda : LRF - Enable CashOut for Bodog [redmine:#105836]
*/

DECLARE @LicenseeID INT,
		@ResellerID INT

-- Bodog
SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'Bodog' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID, @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'CO',
	@EffectiveDate	= '2018-12-04'
	
GO