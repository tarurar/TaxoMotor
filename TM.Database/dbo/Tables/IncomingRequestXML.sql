CREATE TABLE [dbo].[IncomingRequestXML]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [RequestBody] XML NOT NULL, 
    [InDate] DATETIME NOT NULL, 
    [Source] NVARCHAR(50) NULL,
	[OutDate] DATETIME NULL, 
    [ProcessLastAttemptDate] DATETIME NULL, 
    [Processed] AS CAST(CASE ISNULL([OutDate], 0) WHEN 0 THEN 0 ELSE 1 END AS BIT) PERSISTED, 
    [BodyMessageId] AS [dbo].[fn_GetCoordinateV5RequestMessageId](RequestBody) PERSISTED NOT NULL,
	CONSTRAINT IncomingRequestXML_UniqueMessageId UNIQUE(BodyMessageId)
)
