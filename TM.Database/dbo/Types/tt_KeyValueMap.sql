CREATE TYPE [dbo].[tt_KeyValueMap] AS TABLE (
    [KEY]       NVARCHAR (255)  NOT NULL UNIQUE,
    [VALUE_STR] NVARCHAR (255)  NULL,
    [VALUE_INT] INT             NULL,
    [VALUE_BIN] VARBINARY (MAX) NULL,
    [VALUE_BOL] BIT             NULL,
    [VALUE_DAT] DATETIME        NULL,
    [VALUE_XML] XML             NULL);