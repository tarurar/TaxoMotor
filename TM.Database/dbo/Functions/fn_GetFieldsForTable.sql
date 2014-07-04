﻿CREATE FUNCTION [dbo].[fn_GetFieldsForTable] (
	@tableSchema NVARCHAR(255),
	@tableName NVARCHAR(255)
	)
RETURNS @returntable TABLE (
	[FieldName] NVARCHAR(128),
	[DataType] NVARCHAR(128),
	[Length] INT
	)
AS
BEGIN
	INSERT @returntable
	SELECT 
		[COLUMN_NAME],
		[DATA_TYPE],
		[CHARACTER_MAXIMUM_LENGTH]
	FROM [INFORMATION_SCHEMA].[COLUMNS]
	WHERE [TABLE_NAME] = @tableName
		AND [TABLE_SCHEMA] = @tableSchema
	ORDER BY [ORDINAL_POSITION]

	RETURN
END
