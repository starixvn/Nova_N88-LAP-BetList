USE bodb_licensee
GO

/*
	 Creator: 20181002@Amanda
	 Task: Integrate Pragmatic product - [Redmine: #92318]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20181002@Amanda : Add product PP for Athena00 [redmine:#92318]
*/

DECLARE @LicenseeId INT
		,@ResellerID INT

SELECT @LicenseeiD = LicenseeId
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName ='Athena000'	

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller  WITH (NOLOCK)
WHERE LicenseeId = @LicenseeId
	 AND ResellerID <> 3 -- startlit
 
SELECT @LicenseeId LicenseeID ,	 @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeId,
	@ProductCode  	= 'PP',
	@EffectiveDate	= NULL

GO