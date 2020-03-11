USE bodb_licensee
GO

/*
	 Creator: 20181106@Amanda
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20181106@Amanda: Update month of deduction for JIBET, Callmeboss, Citybk [redmine:#100071-#101912-#93713]
*/


DECLARE @MinFeeID INT, @AMF MONEY, @PartyId INT, @MinFee INT, @BaseCurrencyId INT
DECLARE @Today DATE

SET @Today = GETDATE()

------------- JIBET ----------------
SET @MinFee = 5000
SET @BaseCurrencyId = 6

SELECT @PartyId = PartyID
FROM dbo.OAS_Party WITH (NOLOCK)
WHERE PartyName = 'JIBET'

SELECT @MinFeeID = MinFeeId
FROM dbo.LA_MinFee WITH (NOLOCK)
WHERE PartyId = @PartyId AND ProductId = 0 AND FeeTypeId = 1 AND CurrencyId = @BaseCurrencyId AND [Priority] = 1
	AND FromDate <= @Today AND ToDate >= @Today

SELECT @PartyId, @MinFeeID

IF ISNULL(@PartyId, 0) <> 0 AND ISNULL(@MinFeeID, 0) <> 0
BEGIN
	-- Caculate Month of Deduction
	SELECT @AMF = SUM(ISNULL(d.Income, 0))
	FROM dbo.LA_InvoiceHeader h WITH (NOLOCK)
		INNER JOIN dbo.LA_InvoiceStatement d WITH (NOLOCK) ON h.InvoiceHeaderId = d.InvoiceHeaderId
	 WHERE h.InvoiceType = 7 AND d.InvItemTypeId = 3 AND h.StatusId = 0 AND h.Environment = 'PRO'			
		AND h.PartyId = @PartyId
		AND h.CurrencyId = @BaseCurrencyId

	IF ISNULL(@AMF, 0) <> 0
	BEGIN
		UPDATE dbo.LA_MinFee
		SET MonthOfDeduction = CEILING(@AMF / @Minfee)
		WHERE MinFeeId = @MinFeeID
	END
END

----------- Callmeboss ----------------
SELECT @MinFeeID = 0, @AMF = 0, @PartyId = 0, @MinFee = 5000, @BaseCurrencyId = 6

SELECT @PartyId = PartyID
FROM dbo.OAS_Party WITH (NOLOCK)
WHERE PartyName = 'Callmeboss'

SELECT @MinFeeID = MinFeeId
FROM dbo.LA_MinFee WITH (NOLOCK)
WHERE PartyId = @PartyId AND ProductId = 0 AND FeeTypeId = 1 AND CurrencyId = @BaseCurrencyId AND [Priority] = 1
	AND FromDate <= @Today AND ToDate >= @Today

SELECT @PartyId, @MinFeeID

IF ISNULL(@PartyId, 0) <> 0 AND ISNULL(@MinFeeID, 0) <> 0
BEGIN
	-- Caculate Month of Deduction
	SELECT @AMF = SUM(ISNULL(d.Income, 0))
	FROM dbo.LA_InvoiceHeader h WITH (NOLOCK)
		INNER JOIN dbo.LA_InvoiceStatement d WITH (NOLOCK) ON h.InvoiceHeaderId = d.InvoiceHeaderId
	 WHERE h.InvoiceType = 7 AND d.InvItemTypeId = 3 AND h.StatusId = 0 AND h.Environment = 'PRO'			
		AND h.PartyId = @PartyId
		AND h.CurrencyId = @BaseCurrencyId

	IF ISNULL(@AMF, 0) <> 0
	BEGIN
		UPDATE dbo.LA_MinFee
		SET MonthOfDeduction = CEILING(@AMF / @Minfee)
		WHERE MinFeeId = @MinFeeID
	END
END

----------- Citybk ----------------
SELECT @MinFeeID = 0, @AMF = 0, @PartyId = 0, @MinFee = 5000, @BaseCurrencyId = 6

SELECT @PartyId = PartyID
FROM dbo.OAS_Party WITH (NOLOCK)
WHERE PartyName = 'Citybk'

SELECT @MinFeeID = MinFeeId
FROM dbo.LA_MinFee WITH (NOLOCK)
WHERE PartyId = @PartyId AND ProductId = 0 AND FeeTypeId = 1 AND CurrencyId = @BaseCurrencyId AND [Priority] = 1
	AND FromDate <= @Today AND ToDate >= @Today

SELECT @PartyId, @MinFeeID

IF ISNULL(@PartyId, 0) <> 0 AND ISNULL(@MinFeeID, 0) <> 0
BEGIN
	-- Caculate Month of Deduction
	SELECT @AMF = SUM(ISNULL(d.Income, 0))
	FROM dbo.LA_InvoiceHeader h WITH (NOLOCK)
		INNER JOIN dbo.LA_InvoiceStatement d WITH (NOLOCK) ON h.InvoiceHeaderId = d.InvoiceHeaderId
	 WHERE h.InvoiceType = 7 AND d.InvItemTypeId = 3 AND h.StatusId = 0 AND h.Environment = 'PRO'			
		AND h.PartyId = @PartyId
		AND h.CurrencyId = @BaseCurrencyId

	IF ISNULL(@AMF, 0) <> 0
	BEGIN
		UPDATE dbo.LA_MinFee
		SET MonthOfDeduction = CEILING(@AMF / @Minfee)
		WHERE MinFeeId = @MinFeeID
	END
END

GO