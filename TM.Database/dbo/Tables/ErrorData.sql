CREATE TABLE [dbo].[ErrorData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Date] DATETIME,
	[Number] INT,
	[Severity] INT,
	[State] INT,
	[Procedure] NVARCHAR(128),
	[Line] INT,
	[Message] NVARCHAR(4000),
	[SystemContext] NVARCHAR(MAX),
	[UserContext] NVARCHAR(MAX)
)
