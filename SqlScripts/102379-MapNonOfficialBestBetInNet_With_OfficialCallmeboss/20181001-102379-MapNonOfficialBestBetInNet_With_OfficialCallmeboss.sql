USE bodb_licensee
GO

/*
	 Creator: 20180920@Amanda [redmine:#102379]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20180920@Amanda: LA system - Map non-official licensee BestBetInNet with official licensee Callmeboss [redmine:#102379]
*/

DECLARE @OfficalID INT, @UnOfficalID INT

SELECT @UnOfficalID = PartyID FROM dbo.OAS_Party p WITH (NOLOCK) WHERE p.PartyName = 'BestBetInNet'

SELECT @OfficalID = PartyID FROM dbo.OAS_Party p WITH (NOLOCK) WHERE p.PartyName = 'Callmeboss'

-- 1. Update Setup Name of Party BestBetInNet
UPDATE dbo.OAS_Party 
SET SetupName = 'BestBetInNet'
WHERE PartyID = @OfficalID

-- 2. Update PartyID of Invoice Setup Fee of BestBetInNet to Callmeboss
UPDATE dbo.LA_InvoiceHeader
SET PartyId = @OfficalID
WHERE PartyId = @UnOfficalID

UPDATE dbo.LA_InvoiceStatement
SET RefPartyId = @OfficalID
WHERE RefPartyId = @UnOfficalID

UPDATE dbo.LA_InvoiceSetting
SET FromPartyId = @OfficalID
WHERE FromPartyId = @UnOfficalID AND InvTypeId = 7 -- Setup Fee

-- 3. Delete Non-offical Party BestBetInNet
DELETE dbo.OAS_Party
WHERE PartyID = @UnOfficalID

GO