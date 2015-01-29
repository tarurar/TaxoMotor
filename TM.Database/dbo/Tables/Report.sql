﻿CREATE TABLE [dbo].[Report]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Guid] UNIQUEIDENTIFIER NOT NULL,
	[Title] nvarchar(255) NOT NULL,
	[Maintained] BIT NOT NULL
)
