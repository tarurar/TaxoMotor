CREATE TABLE [dbo].[File] (
	[Id] NVARCHAR(255) NULL,
	[FileName] NVARCHAR(255) NULL,
	[MimeType] NVARCHAR(255) NULL,
	[FileContent] VARBINARY(max) NULL,
	[IsFileInStore] BIT NULL,
	[FileIdInStore] NVARCHAR(255) NULL,
	[StoreName] NVARCHAR(255) NULL,
	[FileChecksum] VARBINARY(max) NULL,
	[Id_Auto] INT NOT NULL IDENTITY (1, 1),
	[MessageId] VARCHAR(36) NOT NULL,
	[ServiceDocument] INT NOT NULL
	);