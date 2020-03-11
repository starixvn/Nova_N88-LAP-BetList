USE bodb_licensee
GO

/*
	 Creator: 20181205@Amanda
	 Task:  [AE88] Enable Pragmatic Play & Macau Game[VB] [Redmine: #105870]
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 -20181205@Amanda : LRF - Enable Pragmatic Play & Macau Game[VB] for AE88 [redmine:#105870]
*/

DECLARE @LicenseeID INT,
		@ResellerID INT

-- AE88
SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'AE88' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID, @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'PP,VB',
	@EffectiveDate	= '2018-12-05'
	
GO