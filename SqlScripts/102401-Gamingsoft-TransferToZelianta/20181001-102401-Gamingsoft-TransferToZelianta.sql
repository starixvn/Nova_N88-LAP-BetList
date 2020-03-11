USE bodb_licensee
GO
/*
	 Creator: 20181001@Amanda [redmine:#102401]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20181001@Amanda: Gamingsoft- Transfer to Zelianta [redmine:#102401]
	 
		End date with ONEWorks : 31/12/2017
		Effective date with Zelianta : 01/01/2018
		SB : 15%
		NG: 15%
		VS, VS2 : 15%
		RNG Casino : 7%
*/

-- SB(15%)
DECLARE @ChargeId INT
EXEC dbo.OAS_ChargeInfoIns 
	  @ProductName		= 'SB'
	, @FromPartyName	= 'Gamingsoft'
	, @ToPartyName		= 'Zelianta'	
	, @Fromdate			= '2018-01-01'
	, @Todate			= '2050-01-01'
	, @Rate				= 0.15
	, @System			= 'OA'
	, @Remark			= 'Transfer to Zelianta at 2018-01-01; Redmine: #102401'
	, @ChargeId			= @ChargeId	OUTPUT
SELECT @ChargeId
GO

-- Number Game(15%)
DECLARE @ChargeId INT
EXEC dbo.OAS_ChargeInfoIns 
	  @ProductName		= 'NG'
	, @FromPartyName	= 'Gamingsoft'
	, @ToPartyName		= 'Zelianta'	
	, @Fromdate			= '2018-01-01'
	, @Todate			= '2050-01-01'
	, @Rate				= 0.15		
	, @System			= 'OA'
	, @Remark			= 'Transfer to Zelianta at 2018-01-01; Redmine: #102401'
	, @ChargeId			= @ChargeId	OUTPUT
SELECT @ChargeId
GO

-- VS(15%)
DECLARE @ChargeId INT
EXEC dbo.OAS_ChargeInfoIns 
	  @ProductName		= 'VS'
	, @FromPartyName	= 'Gamingsoft'
	, @ToPartyName		= 'Zelianta'	
	, @Fromdate			= '2018-01-01'
	, @Todate			= '2050-01-01'
	, @Rate				= 0.15		
	, @System			= 'OA'
	, @Remark			= 'Transfer to Zelianta at 2018-01-01; Redmine: #102401'
	, @ChargeId			= @ChargeId	OUTPUT
SELECT @ChargeId
GO

-- VS2 (15%)
DECLARE @ChargeID INT
EXEC dbo.OAS_ChargeInfoIns 
	  @ProductName		= 'VS2'
    , @FromPartyName	= 'Gamingsoft'
    , @ToPartyName		= 'Zelianta'
    , @FromDate			= '2018-01-01'
    , @ToDate			= '2050-01-01'	 
    , @Rate				= 0.15
    , @System			= 'OA'
	, @Remark			= 'Transfer to Zelianta at 2018-01-01; Redmine: #102401'
    , @ChargeId			= @ChargeID OUTPUT 

SELECT @ChargeID
GO

-- CS
DECLARE @ChargeID INT
EXEC dbo.OAS_ChargeInfoIns 
	  @ProductName		= 'CS'
    , @FromPartyName	= 'Gamingsoft'
    , @ToPartyName		= 'Zelianta'
    , @FromDate			= '2018-01-01'
    , @ToDate			= '2050-01-01'	 
    , @Rate				= 0.07
    , @System			= 'OA'
	, @Remark			= 'Transfer to Zelianta at 2018-01-01; Redmine: #102401'
    , @ChargeId			= @ChargeID OUTPUT 

SELECT @ChargeID
GO

-- Deactive ONEWorks User
DECLARE @PartyID INT
SELECT @PartyID = PartyID
FROM dbo.OAS_Party WITH (NOLOCK)
WHERE PartyName = 'Gamingsoft'

DECLARE @DeactiveUser AS TABLE(id INT IDENTITY (1,1), UserName VARCHAR(100), Userid INT)
	
INSERT INTO @DeactiveUser (UserName, Userid)
SELECT Username, UserID 
FROM dbo.LA_User WITH (NOLOCK)
WHERE Username IN ( -- For ONEWorks
                    'twCarrie',
                    'twTeresa',
                    'twTony'
                )

DECLARE @j INT = 1
WHILE (@j <= (select MAX(id) FROM @DeactiveUser))
BEGIN
	DECLARE @UserID INT, @UserName VARCHAR(100)
	SELECT @UserID = Userid, @UserName = UserName
	FROM @DeactiveUser WHERE id = @j
	
	SELECT 'Deactive UserName = ' + @UserName + ' with PartyName = Gamingsoft';
	
	DELETE dbo.LA_UserParty
	WHERE PartyID = @PartyID AND UserID = @UserID
		
	SET @j = @j + 1
END

-- Insert Permission to Zelianta User 
DECLARE @LicenseeName VARCHAR(20)
SELECT @LicenseeName = 'Gamingsoft'

DECLARE @User AS TABLE(id INT IDENTITY (1,1), UserName VARCHAR(100), Userid INT)
	
INSERT INTO @User (UserName, Userid)
SELECT Username, UserID 
FROM dbo.LA_User WITH (NOLOCK)
WHERE Username IN ( -- For Zelianta
                    'twDavid',
                    'twJack',
                    'starix102'
                )

DECLARE @i INT = 1
WHILE (@i <= (select MAX(id) FROM @User))
BEGIN
	DECLARE @sql VARCHAR( 300) =''
	SELECT @sql = 'EXEC dbo.LA_UserPartyIns 
		  @UserName		= '''+ UserName +'''
		, @PartyName	= '''+ @LicenseeName +''''
	FROM @User WHERE id = @i
	EXECUTE (@sql)
	
	SELECT @sql
	SET @i = @i + 1
END

GO	