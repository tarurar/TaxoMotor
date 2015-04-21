-- There is more examples of using query functions on msdn site
-- 1. https://msdn.microsoft.com/en-us/library/cc645858(v=sql.110).aspx
-- 2. https://msdn.microsoft.com/en-us/library/bb510627(v=sql.110).aspx


-- ex1: for updates returns only one record with values after update
DECLARE @from_lsn BINARY (10)
	,@to_lsn BINARY (10)

SET @from_lsn = sys.fn_cdc_get_min_lsn('dbo_RequestAccount')
SET @to_lsn = sys.fn_cdc_get_max_lsn()

SELECT *
FROM cdc.fn_cdc_get_all_changes_dbo_RequestAccount(@from_lsn, @to_lsn, N'all')
GO

-- ex2: for updates returns two records with values before and after update
DECLARE @from_lsn BINARY (10)
	,@to_lsn BINARY (10)

SET @from_lsn = sys.fn_cdc_get_min_lsn('dbo_RequestAccount')
SET @to_lsn = sys.fn_cdc_get_max_lsn()

SELECT *
FROM cdc.fn_cdc_get_all_changes_dbo_RequestAccount(@from_lsn, @to_lsn, N'all update old')
GO

-- ex3
SELECT LSN.tran_begin_time
	,LSN.tran_end_time
	,CASE CDCTable.__$operation
		WHEN 1
			THEN 'Delete'
		WHEN 2
			THEN 'Insert'
		WHEN 3
			THEN 'Update - Before'
		WHEN 4
			THEN 'Update - After'
		END AS 'OperationChangeType'
	,CDCTable.*
FROM cdc.dbo_RequestAccount_CT AS CDCTable
INNER JOIN cdc.lsn_time_mapping AS LSN ON CDCTable.__$start_lsn = LSN.start_lsn