-- This script is intended to enable Change Data Capture for certain tables

-- REMEMBER 
-- 1. Change data capture cannot function properly when the Database Engine service or the SQL Server Agent 
--    service is running under the NETWORK SERVICE account. This can result in error 22832.
--    (https://msdn.microsoft.com/en-us/library/cc645937(v=sql.110).aspx)
-- 2. There is a need to turn on SQL Server Agent

USE [TM.Data];
GO

-- enabling cdc on database
EXEC sys.sp_cdc_enable_db
GO

-- enabling cdc on particular tables
DECLARE @isTableTracked BIT;

SELECT @isTableTracked = [is_tracked_by_cdc]
FROM sys.tables
WHERE [name] = 'RequestAccount'

IF (@isTableTracked = 0)
	EXEC sys.sp_cdc_enable_table @source_schema = N'dbo'
		,@source_name = N'RequestAccount'
		,@role_name = NULL
		,@supports_net_changes = 0

SELECT @isTableTracked = [is_tracked_by_cdc]
FROM [sys].[tables]
WHERE [name] = 'RequestContact'

IF (@isTableTracked = 0)
	EXEC sys.sp_cdc_enable_table @source_schema = N'dbo'
		,@source_name = N'RequestContact'
		,@role_name = NULL
		,@supports_net_changes = 0

SELECT @isTableTracked = [is_tracked_by_cdc]
FROM [sys].[tables]
WHERE [name] = 'RequestAddress'

IF (@isTableTracked = 0)
	EXEC sys.sp_cdc_enable_table @source_schema = N'dbo'
		,@source_name = N'Address'
		,@role_name = NULL
		,@supports_net_changes = 0

-- retention period in minutes, max value = 52494800 (100 years)
EXEC sys.sp_cdc_change_job @job_type = N'cleanup'
	,@retention = 52494800;