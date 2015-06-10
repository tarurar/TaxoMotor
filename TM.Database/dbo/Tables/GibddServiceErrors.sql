USE [TM.Data]
GO

/****** Object:  Table [dbo].[GibddServiceErrors]    Script Date: 08.06.2015 14:19:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GibddServiceErrors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ErrorDate] [datetime] NULL,
	[Exception] [nvarchar](max) NULL,
 CONSTRAINT [PK_GibddServiceErrors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

