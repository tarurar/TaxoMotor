CREATE TABLE [dbo].[RequestCounter]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[Title] as [Id],
	[Year] INT NOT NULL,
	[ServiceCode] NVARCHAR(10) NOT NULL,
	[CounterValue] INT NOT NULL
)
