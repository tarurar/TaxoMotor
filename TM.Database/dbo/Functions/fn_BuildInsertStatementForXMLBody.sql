﻿ CREATE FUNCTION [dbo].[fn_BuildInsertStatementForXMLBody] (
	@TARGETTABLESCHEMA NVARCHAR(255)
	,@TARGETTABLENAME NVARCHAR(255)
	,@VALUES tt_KeyValueMap READONLY
	,@SOURCEXMLPATH NVARCHAR(MAX)
	,@SOURCEXMLPARAMNAME VARCHAR(255)
	,@IDOUTPARAMNAME VARCHAR(255)
	)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @FNAME NVARCHAR(128);
	DECLARE @EXPRESSIONLINE NVARCHAR(MAX);
	DECLARE @FIELDLIST NVARCHAR(MAX);
	DECLARE @SELECT NVARCHAR(MAX);
	DECLARE @STATEMENT NVARCHAR(MAX);
	DECLARE @TARGETTABLEFULLNAME NVARCHAR(255);

	--INIT
	SET @FIELDLIST = '';
	SET @SELECT = '';
	SET @STATEMENT = '';
	SET @TARGETTABLEFULLNAME = @TARGETTABLESCHEMA + '.' + @TARGETTABLENAME;

	DECLARE FIELDS CURSOR
	FOR
	SELECT [T].[FieldName]
		,CASE 
			WHEN [T].[VALUE] IS NULL
				THEN CASE 
						WHEN [T].[HASKEY] = 1
							THEN 'NULL'
						ELSE [T].[STATEMENT]
						END
			ELSE [T].[VALUE]
			END AS [EXPRESSION]
	FROM (
		SELECT [F].[FIELDNAME]
			,CASE [F].[DATATYPE]
				WHEN 'DATETIME'
					THEN 'CAST(NULLIF('
				ELSE ''
				END + 'T.REF.value(''' + [F].[FIELDNAME] + '[1]'', ''' + [F].[DATATYPE] + CASE [F].[DATATYPE]
				WHEN 'NVARCHAR'
					THEN '(' + CAST([F].[LENGTH] AS NVARCHAR(255)) + ')'
				WHEN 'VARCHAR'
					THEN '(' + CAST([F].[LENGTH] AS NVARCHAR(255)) + ')'
				WHEN 'CHAR'
					THEN '(' + CAST([F].[LENGTH] AS NVARCHAR(255)) + ')'
				WHEN 'VARBINARY'
					THEN '(MAX)'
				ELSE ''
				END + ''')' + CASE [F].[DATATYPE]
				WHEN 'DATETIME'
					THEN ', '''') AS datetime)'
				ELSE ''
				END AS [STATEMENT]
			,CASE 
				WHEN [V].[VALUE_STR] IS NOT NULL
					THEN '''' + [V].[VALUE_STR] + ''''
				WHEN [V].[VALUE_INT] IS NOT NULL
					THEN CAST([V].[VALUE_INT] AS NVARCHAR(255))
				WHEN [V].[VALUE_BIN] IS NOT NULL
					THEN '''' + CAST([V].[VALUE_BIN] AS NVARCHAR(MAX)) + ''''
				WHEN [V].[VALUE_DAT] IS NOT NULL
					THEN CAST([V].[VALUE_DAT] AS NVARCHAR(255))
				WHEN [V].[VALUE_XML] IS NOT NULL
					THEN '''' + CAST([V].[VALUE_XML] AS NVARCHAR(MAX)) + ''''
				WHEN [V].[VALUE_BOL] IS NOT NULL
					THEN CAST([V].[VALUE_BOL] AS NVARCHAR(255))
				ELSE NULL
				END AS [VALUE]
			,CASE 
				WHEN [V].[KEY] IS NULL
					THEN 0
				ELSE 1
				END AS HASKEY
		FROM [DBO].[fn_GetFieldsForTable](@TARGETTABLESCHEMA, @TARGETTABLENAME) F
		LEFT JOIN @VALUES V ON [F].[FIELDNAME] = [V].[KEY]
		WHERE COLUMNPROPERTY(OBJECT_ID(@TARGETTABLEFULLNAME), [F].[FIELDNAME], 'IsIdentity') != 1
			AND COLUMNPROPERTY(OBJECT_ID(@TARGETTABLEFULLNAME), [F].[FIELDNAME], 'IsComputed') != 1
		) T

	OPEN FIELDS;

	FETCH NEXT
	FROM FIELDS
	INTO @FNAME
		,@EXPRESSIONLINE;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @FIELDLIST = @FIELDLIST + '[' + @FNAME + ']';
		SET @SELECT = @SELECT + @EXPRESSIONLINE;

		FETCH NEXT
		FROM FIELDS
		INTO @FNAME
			,@EXPRESSIONLINE

		IF @@FETCH_STATUS = 0
		BEGIN
			SET @FIELDLIST = @FIELDLIST + ',';
			SET @SELECT = @SELECT + ',';
		END
	END

	CLOSE FIELDS;

	DEALLOCATE FIELDS;

	-- RETURN IF FIELD LIST IS EMPTY
	IF (
			@FIELDLIST IS NULL
			OR @FIELDLIST = ''
			)
		RETURN '';

	SET @STATEMENT = 'SET NOCOUNT ON;' + 'INSERT INTO [' + @TARGETTABLESCHEMA + '].[' + @TARGETTABLENAME + '] (' + @FIELDLIST + ')' + 'SELECT ' + @SELECT + ' ' + 'FROM ' + CASE LEFT(@SOURCEXMLPARAMNAME, 1)
			WHEN '@'
				THEN @SOURCEXMLPARAMNAME
			ELSE '@' + @SOURCEXMLPARAMNAME
			END + '.nodes(''' + @SOURCEXMLPATH + ''') T(REF);' + 'SELECT ' + CASE LEFT(@IDOUTPARAMNAME, 1)
			WHEN '@'
				THEN @IDOUTPARAMNAME
			ELSE '@' + @IDOUTPARAMNAME
			END + ' = SCOPE_IDENTITY();';

	RETURN @STATEMENT;
END

