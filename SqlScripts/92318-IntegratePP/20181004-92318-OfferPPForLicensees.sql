USE bodb_licensee
GO

/*
	 Creator: 20181004@Amanda
	 Task: Integrate Pragmatic product - [Redmine: #92318]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20181004@Amanda : Enable product PP for licensees [redmine:#92318]
*/
DECLARE @Licensee TABLE (LicenseeId INT, LicenseeName VARCHAR(50))
DECLARE @ProductId INT = 32 -- PP

INSERT INTO @Licensee(LicenseeId, LicenseeName)
SELECT l.LicenseeID, l.LicenseeName
FROM dbo.Licensees l WITH (NOLOCK)
WHERE l.LicenseeName IN (
					'Aoncash',
					'Macaubet',
					'ITCbet',
					'9bet',
					'M8bola',
					'Q8bola',
					'NBCbet',
					'BE7',
					'Bolanation',
					'Sbcbet',
					'UNObet'
				)

INSERT INTO dbo.LicenseeProduct3rd(LicenseeID, ProductID, Comm, Winloss)   
SELECT LicenseeId, @ProductId, 0, 1  
FROM @Licensee

GO