CREATE TABLE [dbo].[AppLog] (
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AppLog_Id] DEFAULT(NEWSEQUENTIALID())
	,[EventDateTime] [datetime] NOT NULL CONSTRAINT [DF_AppLog_EventDateTime] DEFAULT (getdate())
	,[ApplicationName] [nvarchar](500) NOT NULL
	,[Direction] [int] NOT NULL
	,[Message] [nvarchar](max) NOT NULL
	,CONSTRAINT [PK_AppLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]