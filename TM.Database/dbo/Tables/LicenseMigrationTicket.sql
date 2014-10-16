CREATE TABLE [dbo].[LicenseMigrationTicket]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Title] AS [LicenseId],
	[Status] INT NOT NULL DEFAULT 0, 
    [StartDate] DATETIME NULL, 
    [FinishDate] DATETIME NULL, 
    [ErrorInfo] NVARCHAR(MAX) NULL, 
    [StackTrace] NVARCHAR(MAX) NULL,
	[LicenseId] INT NOT NULL,
	CONSTRAINT [FK_LicenseMigrationTicket_License] FOREIGN KEY ([LicenseId]) REFERENCES [dbo].[License]([Id])
)
