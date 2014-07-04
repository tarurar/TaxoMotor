CREATE TABLE [dbo].[ServiceHeader] (
	[FromOrgCode] NVARCHAR(255) NULL,
	[ToOrgCode] NVARCHAR(255) NULL,
	[MessageId] VARCHAR(36) NOT NULL,
	[RelatesTo] NVARCHAR(255) NULL,
	[ServiceNumber] NVARCHAR(255) NULL,
	[RequestDateTime] DATETIME NOT NULL,
	[Id] INT NOT NULL IDENTITY (1, 1),
	[Request] INT NOT NULL
	);