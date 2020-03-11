USE bodb_licensee
GO

/*
	 Creator: 20180918@Amanda
	 Task: Remove products from licensee product list [Redmine: #98222]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180918@Amanda : LRF - Delete setting of BA from Bobo88's product setting [redmine:#98222]
	 
	 Developer's Note:
	 -Licensees: Bobo88
*/

DECLARE @LicenseeId INT
DECLARE @ProductId INT

-- get licenseeId
SELECT @LicenseeId = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'Bobo88'

-- get product Id
SELECT @ProductId = Productid
FROM dbo.Product3rd WITH (NOLOCK)
WHERE ProductPrefix = 'BA'

-- delete setting
DELETE lp
FROM dbo.LicenseeProduct3rd lp
WHERE ProductID = @ProductId AND lp.LicenseeID = @LicenseeId
	  
-- remove BA Special permission
DECLARE @FunctionId INT

SELECT @FunctionId = FunctionId
FROM dbo.Functions WITH (NOLOCK)
WHERE FunctionKey = 'SpecialBA54'

DELETE lf
FROM dbo.LicenseeFunctions lf WITH (NOLOCK)
WHERE FunctionId = @FunctionId AND lf.LicenseeId = @LicenseeId
		
DELETE luf
FROM dbo.LicenseeUserFunctions luf WITH (NOLOCK)
WHERE FunctionId = @FunctionId AND luf.LicenseeID = @LicenseeId

GO
