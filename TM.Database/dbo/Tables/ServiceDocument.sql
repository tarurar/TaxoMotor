CREATE TABLE [dbo].[ServiceDocument] (
	[Id] NVARCHAR(255) NULL,
	[DocCode] NVARCHAR(255) NULL,
	[DocSubType] NVARCHAR(255) NULL,
	[DocPerson] NVARCHAR(255) NULL,
	[DocSerie] NVARCHAR(255) NULL,
	[DocNumber] NVARCHAR(255) NULL,
	[DocDate] DATETIME NULL,
	[ValidityPeriod] NVARCHAR(255) NULL,
	[WhoSign] NVARCHAR(255) NULL,
	[ListCount] INT NULL,
	[CopyCount] INT NULL,
	[DivisionCode] NVARCHAR(255) NULL,
	[Signature] VARBINARY(max) NULL,
	[Id_Auto] INT NOT NULL IDENTITY (1, 1),
	[MessageId] VARCHAR(36) NOT NULL,
	[Service] INT NOT NULL
	);