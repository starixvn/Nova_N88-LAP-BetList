USE bodb_licensee
GO

/*
	 Creator: 20181116@Amanda
	 Task:  Enable Pragmatic Play - [Redmine: #104856]
	 Database: DBCUST.bodb_licensee

	 Revisions:
	 -20181116@Amanda : LRF - Enable Pragmatic Play for Liga, ZZun88, ISC888 [redmine:#104856]
*/

DECLARE @LicenseeID INT,
		@ResellerID INT

-- Liga
SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'Liga' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID, @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'PP',
	@EffectiveDate	= '2018-11-15'
	
-- ZZun88
SELECT @LicenseeID = NULL, @ResellerID = NULL

SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'ZZun88' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID, @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'PP',
	@EffectiveDate	= '2018-11-15'
	
-- ISC888
SELECT @LicenseeID = NULL, @ResellerID = NULL

SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'ISC888' 

SELECT @ResellerID = ResellerID
FROM dbo.LicenseeReseller WITH (NOLOCK)
WHERE LicenseeID = @LicenseeID AND ResellerID <> 3 

SELECT @LicenseeID LicenseeID, @ResellerID ResellerID

EXEC [dbo].[DBA_Lrf_OfferProduct]
	@ResellerID		= @ResellerID,
	@LicenseeId  	= @LicenseeID,
	@ProductCode  	= 'PP',
	@EffectiveDate	= '2018-11-15'
	
GO