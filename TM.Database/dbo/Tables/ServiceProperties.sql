CREATE TABLE [dbo].[ServiceProperties] (
	[Title] AS [MessageId],
	[delete] INT NULL,
	CONSTRAINT [CK_ServiceProperties_delete] CHECK (([delete] IN (1, 2, 3, 4, 5))),
	[name] NVARCHAR(255) NULL,
	[other] NVARCHAR(255) NULL,
	[pr_pereoformlenie] BIT NULL,
	[pr_pereoformlenie_2] BIT NULL,
	[pr_pereoformlenie_3] BIT NULL,
	[pr_pereoformlenie_4] BIT NULL,
	[pr_pereoformlenie_5] BIT NULL,
	[pr_pereoformlenie_6] BIT NULL,
	[pr_pereoformlenie_7] BIT NULL,
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[MessageId] VARCHAR(36) NOT NULL
	);