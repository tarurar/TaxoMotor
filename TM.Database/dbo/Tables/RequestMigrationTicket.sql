CREATE TABLE [dbo].[RequestMigrationTicket]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Title] AS [MessageId],
	[MessageId] VARCHAR(36) NOT NULL,
	[Status] INT NOT NULL DEFAULT 0, 
    [StartDate] DATETIME NULL, 
    [FinishDate] DATETIME NULL, 
    [ErrorInfo] NVARCHAR(MAX) NULL, 
    [StackTrace] NVARCHAR(MAX) NULL,
	[RequestId] INT NOT NULL,
	CONSTRAINT [FK_RequestMigrationTicket_Request] FOREIGN KEY ([RequestId]) REFERENCES [dbo].[Request]([Id])
)
