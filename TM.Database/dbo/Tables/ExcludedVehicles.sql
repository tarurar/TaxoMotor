CREATE TABLE [dbo].[ExcludeVehicles](
	[ID] [uniqueidentifier] NOT NULL,
	[owner] [nvarchar](100) NULL,
	[licenseId] [nvarchar](250) NULL,
	[catalogNumber] [nvarchar](100) NULL,
	[licencePlateNumber] [nvarchar](100) NULL,
	[uploadDate] [datetime] NULL,
	[cancelDate] [datetime] NULL,
	[cancelComment] [nvarchar](max) NULL,
 CONSTRAINT [PK_ExcludeVehicles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

