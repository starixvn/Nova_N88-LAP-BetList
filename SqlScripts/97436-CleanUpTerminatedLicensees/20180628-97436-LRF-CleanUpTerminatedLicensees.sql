USE bodb_licensee
GO

/*
	 Creator: 20180628@Amanda [redmine:#97436]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 - 20180628@Amanda: Clean up terminated licensees from LRF Licensee List  [redmine:#97436]
	 
	 Developer's Note
	 - YQ355, Longbao, Leon, 10bet, Dela: should be deactivated on Accounting as well
	 - AB688: has no data on SystemLicensee => no need update
*/

--1-- YQ355
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='YQ355',
	@SystemDeactivate   ='LRF,Accounting'
	
--2-- HL5888
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='HL5888',
	@SystemDeactivate   ='LRF'
	
--3-- DB5688
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='DB5688',
	@SystemDeactivate   ='LRF'

--4-- AthenaQA
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='AthenaQA',
	@SystemDeactivate   ='LRF'

--5-- AlogEU
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='AlogEU',
	@SystemDeactivate   ='LRF'

--6-- 67bet
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='67bet',
	@SystemDeactivate   ='LRF,OA,KPI'
	
--7-- Wawabet
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='Wawabet',
	@SystemDeactivate   ='LRF,OA'
	
--8-- Royalebet
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='Royalebet',
	@SystemDeactivate   ='LRF,OA,KPI'
	
--9-- Greenplay
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='Greenplay',
	@SystemDeactivate   ='LRF,OA'
	
--10-- TS
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='TS',
	@SystemDeactivate   ='LRF,OA'
	
--11-- Mirabet
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='Mirabet',
	@SystemDeactivate   ='LRF,OA'
	
--12-- Doctorgame
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='Doctorgame',
	@SystemDeactivate   ='LRF,OA,KPI'
	
--13-- Citibet
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='Citibet',
	@SystemDeactivate   ='LRF,OA'
	
--14-- asianbet
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='asianbet',
	@SystemDeactivate   ='LRF,OA,KPI'
	
--15-- CQ11
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='CQ11',
	@SystemDeactivate   ='LRF,OA'
	
--16-- 12betCO
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='12betCO',
	@SystemDeactivate   ='LRF,OA'
	
--17-- LZYQ
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='LZYQ',
	@SystemDeactivate   ='LRF,OA'
	
--18-- Longbao
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='Longbao',
	@SystemDeactivate   ='LRF,Accounting,OA,KPI'
	
--19-- Leon
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='Leon',
	@SystemDeactivate   ='LRF,Accounting,OA'
	
--20-- 10bet
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='10bet',
	@SystemDeactivate   ='LRF,Accounting,OA'
	
--21-- LZYQ
EXEC [dbo].[DBA_Lrf_ActiveLicenseeSystem]
	@LicenseeName		='Dela',
	@SystemDeactivate   ='LRF,Accounting,OA,OAS,KPI'
	
	
-- Deactivate on Licensees table
UPDATE dbo.Licensees WITH (ROWLOCK, UPDLOCK)
SET IsActive = 0
WHERE LicenseeName IN ('YQ355', 'HL5888', 'DB5688', 'AthenaQA', 
     'AlogEU', '67bet', 'Wawabet', 'Royalebet', 
     'Greenplay', 'TS', 'Mirabet', 'Doctorgame', 
     'Citibet', 'asianbet', 'CQ11', '12betCO', 
     'LZYQ', 'AB688', 'LongBao', 'Leon', '10bet', 'Dela')
GO