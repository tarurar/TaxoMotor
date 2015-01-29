CREATE TABLE [dbo].[ReportData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Indicator] nvarchar(255),
	[IntValue1] INT,
	[IntValue2] INT,
	[IntValue3] INT,
	[StrValue1] nvarchar(255),
	[StrValue2] nvarchar(255),
	[StrValue3] nvarchar(255),
	[NumericValue1] numeric(18, 2),
	[NumericValue2] numeric(18, 2),
	[NumericValue3] numeric(18, 2),
	[ReportSessionId] INT NOT NULL,
	CONSTRAINT [FK_ReportData_ReportSession] FOREIGN KEY ([ReportSessionId]) REFERENCES [dbo].[ReportSession]([Id])
)
