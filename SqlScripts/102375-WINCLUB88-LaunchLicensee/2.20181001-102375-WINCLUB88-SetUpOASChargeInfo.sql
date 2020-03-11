USE bodb_licensee
GO

/*
	 Created: 20181001@Amanda [redmine:#102375]
	 Task: [WINCLUB88] New Licensee Launch (Boping API)
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20181001@Amanda: Set up OAS charge info for WINCLUB88 [redmine:#102375]
	 
	 Developer's Note
		- Licensee Name: WINCLUB88        
		- Licensee Type: Boping API licensee
		- Launch product: Sportsbook, Virtual Sports x2, Number Game, BA, Cashout
		- Terms: no min fee, no waive setting
		- Launch date :  Sep 25, 2018
		- Enable on:
			  » NAP Report
			  » LRF
			  » LA
*/

-- SB
DECLARE @ChargeId INT
EXEC dbo.OAS_ChargeInfoIns 
	  @ProductName		= 'SB'
	, @FromPartyName	= 'WINCLUB88'
	, @ToPartyName		= 'Boping'	
	, @Fromdate			= '2018-09-25'
	, @Todate			= '2050-01-01'
	, @Rate				= 0.15
	, @System			= 'OA'
	, @Remark			= 'Launch at 2018-09-25; Redmine: #102375'
	, @ChargeId			= @ChargeId	OUTPUT
SELECT @ChargeId
GO

DECLARE @ChargeId INT
EXEC dbo.OAS_ChargeInfoIns 
	  @ProductName		= 'NG'
	, @FromPartyName	= 'WINCLUB88'
	, @ToPartyName		= 'Boping'	
	, @Fromdate			= '2018-09-25'
	, @Todate			= '2050-01-01'
	, @Rate				= 0.15		
	, @System			= 'OA'
	, @Remark			= 'Launch at 2018-09-25; Redmine: #102375'
	, @ChargeId			= @ChargeId	OUTPUT
SELECT @ChargeId
GO

DECLARE @ChargeId INT
EXEC dbo.OAS_ChargeInfoIns 
	  @ProductName		= 'VS'
	, @FromPartyName	= 'WINCLUB88'
	, @ToPartyName		= 'Boping'	
	, @Fromdate			= '2018-09-25'
	, @Todate			= '2050-01-01'
	, @Rate				= 0.15		
	, @System			= 'OA'
	, @Remark			= 'Launch at 2018-09-25; Redmine: #102375'
	, @ChargeId			= @ChargeId	OUTPUT
SELECT @ChargeId
GO

DECLARE @ChargeID INT
EXEC dbo.OAS_ChargeInfoIns 
	@ProductName	= 'VS2',
    @FromPartyName	= 'WINCLUB88', 
    @ToPartyName	= 'Boping', 
    @FromDate		= '2018-09-25', 
    @ToDate			= '2050-01-01', 	 
    @Rate			= 0.15, 
    @System			= 'OA', 
	@Remark			= 'Launch at 2018-09-25; Redmine: #102375',
    @ChargeId		= @ChargeID OUTPUT 

SELECT @ChargeID
GO

DECLARE @ChargeID INT
EXEC dbo.OAS_ChargeInfoIns 
		@ProductName = 'BA',
		@FromPartyName = 'WINCLUB88', 
		@ToPartyName = 'Boping', 
		@FromDate = '2018-09-25', 
		@ToDate = '2050-01-01', 		
		@Rate = 1, 
		@System = 'OA', 
		@Remark	= 'Launch at 2018-09-25; Redmine: #102375',		
		@ChargeId = @ChargeID OUTPUT

SELECT @ChargeID
GO

DECLARE @ChargeID INT
EXEC dbo.OAS_ChargeInfoIns 
		@ProductName = 'CO',
		@FromPartyName = 'WINCLUB88', 
		@ToPartyName = 'Boping', 
		@FromDate = '2018-09-25', 
		@ToDate = '2050-01-01', 
		@ChargeType = 5, 
		@Rate = 0.03, 
		@System = 'OA', 
		@Remark	= 'Launch at 2018-09-25; Redmine: #102375',		
		@ChargeId = @ChargeID OUTPUT

SELECT @ChargeID
GO

-- Insert Permission to User 
DECLARE @NewLicenseeName VARCHAR(20)
SELECT @NewLicenseeName = 'WINCLUB88'

DECLARE @User AS TABLE(id INT IDENTITY (1,1), UserName VARCHAR(100), Userid INT)
	
INSERT INTO @User (UserName, Userid)
SELECT Username, UserID 
FROM dbo.LA_User WITH (NOLOCK)
WHERE Username IN ( -- For Boping
                    'accChevon',
                    'accMeiteng',
                    'accMyleen',
                    'anthony',
                    'BYRON',
                    'ByronChia',
                    'davidTan',
                    'jackyting',
                    'Justin',
                    'martin',
                    'nsdbeng',
                    'nsdchuan',
                    'nsdtai',
                    'SEANLAU',
                    'twDavid',
                    'twJack',
                    'starixIT',
                    'starix6',
                    'starix29',
                    'starix101',
                    'starix102',
                    'starix106',
                    'celinesaw',
                    'twjett'
                )

DECLARE @i INT = 1
WHILE (@i <= (select MAX(id) FROM @User))
BEGIN
	DECLARE @sql VARCHAR( 300) =''
	SELECT @sql = 'EXEC dbo.LA_UserPartyIns 
		  @UserName		= '''+ UserName +'''
		, @PartyName	= '''+ @NewLicenseeName +''''
	FROM @User WHERE id = @i
	EXECUTE (@sql)
	
	SELECT @sql
	SET @i = @i + 1
END

;WITH cte_function 
AS
(
	SELECT DISTINCT FunctionID, PartyID
	FROM dbo.OAS_Party p WITH(NOLOCK), dbo.LA_Function f WITH(NOLOCK)
	WHERE PartyName = @NewLicenseeName
		AND NOT EXISTS 
					(SELECT 1 
					FROM dbo.LA_FunctionParty fp WITH (NOLOCK) 
					WHERE fp.FunctionID = f.FunctionID AND fp.PartyID = p.PartyID)
)
INSERT INTO LA_FunctionParty( FunctionID, PartyID)
SELECT FunctionID, PartyID
FROM cte_function
WHERE FunctionID NOT IN (			
		7	--  Racing		
		,17 --  Live Casino Win Loss	
		,19	--  RNG Casino Win Loss
		,20 --	SunSport Win Loss	
		,32 --  Keno Win Loss	
		,43 --	Gold Deluxe Win Loss
		,46 --	Microgaming Win Loss (GMT - 4)
		,48 -- 	Microgaming Win Loss (GMT + 0)
		,49 --	Bet Trade Win Loss 
		,53 --	Cock Fighting Win Loss		
		,55 --	Colossus Bet Win Loss
		,56 --	Superlive Win Loss	
		,57 --  Keno Winloss	
		,58 --  AG Casino Win Loss	
		,64 --  AllBet Winloss		
		,65 --  NG2 Win Loss
		,66 --  Fishing World Win Loss (GMT +8)	
		,70 --  Lottery Winloss
		,74 --  Macau Games WinLoss
		,75 --  Exchange WinLoss
		,77 --  PP Win Loss(GMT +0)
		)

GO
