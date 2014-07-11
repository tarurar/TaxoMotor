CREATE TABLE [dbo].[Service] (
	[Title] AS [RegNum],
	[RegNum] NVARCHAR(255) NULL,
	[RegDate] DATETIME NULL,
	[ServiceNumber] NVARCHAR(255) NULL,
	[ServiceTypeCode] NVARCHAR(255) NULL,
	[ServicePrice] DECIMAL(38, 0) NULL,
	[PrepareTargetDate] DATETIME NULL,
	[OutputTargetDate] DATETIME NULL,
	[Copies] INT NULL,
	[PrepareFactDate] DATETIME NULL,
	[OutputFactDate] DATETIME NULL,
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[MessageId] VARCHAR(36) NOT NULL
	);