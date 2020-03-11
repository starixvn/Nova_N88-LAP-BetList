USE bodb_licensee
GO

/*
	 Creator: 20180507@Amanda
	 Task:  Mybet88 - Enable Cashout[Redmine: #94658]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180507@Amanda : LAP - Enable Cashout for Mybet88 [redmine:#94658]
*/

DECLARE @LicenseeID INT,
		@ResellerID INT

-- Mybet88

SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'Mybet88' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID ,	 @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'CO',
	@EffectiveDate	= ''