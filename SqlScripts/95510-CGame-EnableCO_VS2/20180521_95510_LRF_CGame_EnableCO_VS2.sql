USE bodb_licensee
GO

/*
	 Creator: 20180521@Amanda
	 Task:  CGame - Enable VS2, Cash Out[Redmine: #95510]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180521@Amanda : LRF - CGame - Enable VS2, Cash Out[Redmine: #95510]
*/

DECLARE @LicenseeID INT,
		@ResellerID INT

-- CGame

SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'CGame' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID ,	 @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'CO,VS2',
	@EffectiveDate	= ''