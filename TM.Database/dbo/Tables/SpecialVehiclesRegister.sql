CREATE TABLE [dbo].[SpecialVehiclesRegister](
	[ID] [uniqueidentifier] NOT NULL,
	[License_id] [int] NULL,
	[RecordId] [uniqueidentifier] NULL,
	[PackageId] [nvarchar](256) NULL,
	[ProccessingStageCode] [nvarchar](100) NULL,
	[ProcessingStageDate] [datetime] NULL,
	[ProcessingStageInfo] [nvarchar](512) NULL,
	[RecordStatusCode] [nvarchar](15) NULL,
	[RecordStatusText] [nvarchar](512) NULL,
	[Exception] [nvarchar](max) NULL,
 CONSTRAINT [PK_SpecialVehiclesRegister] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[SpecialVehiclesRegister]  WITH CHECK ADD  CONSTRAINT [FK_SpecialVehiclesRegister_License] FOREIGN KEY([License_id])
REFERENCES [dbo].[License] ([Id])
GO

ALTER TABLE [dbo].[SpecialVehiclesRegister] CHECK CONSTRAINT [FK_SpecialVehiclesRegister_License]
GO

