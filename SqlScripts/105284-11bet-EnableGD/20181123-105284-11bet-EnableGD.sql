USE bodb_licensee
GO

/*
	 Creator: 20181123@Amanda
	 Task:  11bet - Enable GD[Redmine: #105284]
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 -20181123@Amanda : LAP - Enable GD for 11bet [redmine:#105284]
*/

DECLARE @LicenseeID INT,
		@ResellerID INT

-- 11bet

SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = '11bet' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID ,	 @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'GD',
	@EffectiveDate	= '2018-11-22',
	@IsPullService	= 1