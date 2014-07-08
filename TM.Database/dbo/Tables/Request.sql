CREATE TABLE [dbo].[Request] (
	[Id] INT NOT NULL IDENTITY(1, 1),
	[MessageId] VARCHAR(36) NOT NULL,
	[DeclarantRequestAccount] INT NULL,
	[DeclarantRequestContact] INT NULL,
	[TrusteeRequestContact] INT NULL,
	[ServiceProperties] INT NOT NULL,
	[Service] INT NOT NULL,
	[ServiceHeader] INT NOT NULL
	);