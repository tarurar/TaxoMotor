CREATE TABLE [dbo].[OpfBase] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [CodeAsguf] NVARCHAR (50)  NULL,
    [CodeMO]    NVARCHAR (50)  NULL,
    [NameAsguf] NVARCHAR (255) NULL,
    [NameMo]    NVARCHAR (255) NULL,
    CONSTRAINT [PK_OpfBase] PRIMARY KEY CLUSTERED ([Id] ASC)
);