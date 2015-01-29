CREATE TABLE [dbo].[ReportSession]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[ReportId] INT NOT NULL,
	[Date] DATETIME NOT NULL,
	CONSTRAINT [FK_ReportSession_Report] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[Report]([Id])
)
