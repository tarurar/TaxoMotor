CREATE TABLE [dbo].[AppLog] (
    [ID]              UNIQUEIDENTIFIER CONSTRAINT [DF_AppLog_Id] DEFAULT (newsequentialid()) NOT NULL,
    [EventDateTime]   DATETIME         CONSTRAINT [DF_AppLog_EventDateTime] DEFAULT (getdate()) NOT NULL,
    [ApplicationName] NVARCHAR (500)   NOT NULL,
    [Direction]       NCHAR (1)        NOT NULL,
    [Message]         NVARCHAR (MAX)   NOT NULL,
    [MessageID]       UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_AppLog] PRIMARY KEY CLUSTERED ([ID] ASC)
);