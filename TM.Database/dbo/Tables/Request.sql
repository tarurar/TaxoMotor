CREATE TABLE [dbo].[Request] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Title] AS [MessageId],
	[MessageId] VARCHAR(36) NOT NULL,
	[DeclarantRequestAccount] INT NULL,
	[DeclarantRequestContact] INT NULL,
	[TrusteeRequestContact] INT NULL,
	[ServiceProperties] INT NOT NULL,
	[Service] INT NOT NULL,
	[ServiceHeader] INT NOT NULL, 
    CONSTRAINT [FK_Request_RequestAccountDeclarant] FOREIGN KEY ([DeclarantRequestAccount]) REFERENCES [dbo].[RequestAccount]([Id]),
	CONSTRAINT [FK_Request_RequestContactDeclarant] FOREIGN KEY ([DeclarantRequestContact]) REFERENCES [dbo].[RequestContact]([Id_Auto]),
	CONSTRAINT [FK_Request_RequestContactTrustee] FOREIGN KEY ([TrusteeRequestContact]) REFERENCES [dbo].[RequestContact]([Id_Auto]),
	CONSTRAINT [FK_Request_ServiceProperties] FOREIGN KEY ([ServiceProperties]) REFERENCES [dbo].[ServiceProperties]([Id]),
	CONSTRAINT [FK_Request_Service] FOREIGN KEY ([Service]) REFERENCES [dbo].[Service]([Id]),
	CONSTRAINT [FK_Request_ServiceHeader] FOREIGN KEY ([ServiceHeader]) REFERENCES [dbo].[ServiceHeader]([Id])
	);