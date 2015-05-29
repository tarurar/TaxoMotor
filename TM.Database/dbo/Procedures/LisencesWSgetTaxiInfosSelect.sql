CREATE PROCEDURE [dbo].[LisencesWSgetTaxiInfosSelect]
    @Flag INT = 1 -- флаг хранимки
	,@LicenseNum NVARCHAR(64) = NULL --RegNumber
	,@LicenseDate DATETIME = NULL-- CreationDate
	,@Name NVARCHAR(32) = NULL-- ShortName
	,@OgrnNum  NVARCHAR(255) = NULL--Ogrn
	,@OgrnDate DATETIME = NULL--OgrnDate
	,@Brand NVARCHAR(64) = NULL-- TaxiBrand
	,@Model NVARCHAR(64) = NULL --TaxiModel
	,@RegNum NVARCHAR(24) = NULL--TaxiStateNumber
	,@SortOrder NVARCHAR(300) = NULL
	,@PageNumber INT = 1
	,@Count INT = 10
AS


DECLARE 	
	@OFFSET INT
	,@FETCH INT
	,@RegNumberInt INT

	
SELECT 
	@RegNumberInt = CAST(RIGHT(@LicenseNum, 5) AS INT)
	,@SortOrder = REPLACE(ISNULL(@SortOrder, N'LicenseNum'), N';', N', ')
    ,@Name = '%' + REPLACE(@Name, ' ', '%') + '%'

SET @OFFSET = CASE WHEN @PageNumber = 1 THEN 0 ELSE ((@PageNumber - 1) * @Count) + 1 END
SET	@FETCH = CASE WHEN @PageNumber = 1 THEN @Count ELSE @OFFSET + @Count - 1 END

SET @SortOrder = REPLACE(@SortOrder, N'RegNum',N'L1.TaxiStateNumber')
SET @SortOrder = REPLACE(@SortOrder, N'LicenseNum',N'L1.RegNumberInt')
SET @SortOrder = REPLACE(@SortOrder, N'LicenseDate',N'L1.CreationDate')
SET @SortOrder = REPLACE(@SortOrder, N'Name',N'L1.ShortName')
SET @SortOrder = REPLACE(@SortOrder, N'OgrnNum',N'L1.Ogrn')
SET @SortOrder = REPLACE(@SortOrder, N'OgrnDate',N'L1.OgrnDate')
SET @SortOrder = REPLACE(@SortOrder, N'Brand',N'L1.TaxiBrand')
SET @SortOrder = REPLACE(@SortOrder, N'Model',N'L1.TaxiModel')
SET @SortOrder = REPLACE(@SortOrder, N'Condition',N'L1.Status')

DECLARE @sqlcmd NVARCHAR(max)

SET @sqlcmd = N'SELECT
	L1.Id,
    ROW_NUMBER() OVER (ORDER BY '+ @SortOrder + N') AS NN
INTO #ids
FROM 
	(
        SELECT
	        L1.Id,
            L1.TaxiStateNumber,
            L1.RegNumberInt,
            L1.CreationDate,
            L1.ShortName,
            L1.Ogrn,
            L1.OgrnDate,
            L1.TaxiBrand,
            L1.TaxiModel,
            L1.Status            
        FROM 
	        License AS L1 (NOLOCK)
            LEFT OUTER JOIN License (NOLOCK) AS chld
                ON chld.Parent = L1.Id
        WHERE
            L1.Status <> 4 AND chld.Id IS NULL
	        AND (@RegNumberInt IS NULL OR L1.RegNumberInt = @RegNumberInt)
	        AND (@LicenseDate IS NULL OR @LicenseDate = L1.CreationDate)
	        AND (@Name IS NULL OR L1.ShortName LIKE @Name)
	        AND (@OgrnNum IS NULL OR L1.Ogrn = @OgrnNum)
	        AND (@OgrnDate IS NULL OR L1.OgrnDate = @OgrnDate)
	        AND (@Brand IS NULL OR L1.TaxiBrand = @Brand)
	        AND (@Model IS NULL OR L1.TaxiModel = @Model)
	        AND (@RegNum IS NULL OR L1.TaxiStateNumber = @RegNum)
        ) AS L1

SELECT 
	L1.Signature as ''TaxiInfo''
FROM
	License AS L1 (NOLOCK)
    INNER JOIN #ids
        ON #ids.Id = L1.Id
            AND #ids.NN BETWEEN @OFFSET AND @FETCH
ORDER BY #ids.NN
FOR XML PATH(''''), ROOT (''ArrayOfTaxiInfo'')

SELECT
	COUNT(*)
FROM 
	#ids

DROP TABLE #ids'

DECLARE @ParmDefinition NVARCHAR(max) = N'@RegNumberInt INT, @LicenseDate DATETIME, @Name NVARCHAR(32), @OgrnNum  NVARCHAR(255), @OgrnDate DATETIME, @Brand NVARCHAR(64), @Model NVARCHAR(64), @RegNum NVARCHAR(24), @OFFSET INT, @FETCH INT';


IF @Flag = 1
    EXECUTE sp_executesql 
	    @sqlcmd, 
	    @ParmDefinition,
	    @RegNumberInt = @RegNumberInt, 
	    @LicenseDate = @LicenseDate ,
	    @Name = @Name,
	    @OgrnNum = @OgrnNum,
	    @OgrnDate = @OgrnDate,
	    @Brand = @Brand,
	    @Model = @Model,
	    @RegNum = @RegNum, 
	    @OFFSET = @OFFSET,
	    @FETCH = @FETCH

IF @Flag = 2
    SELECT TOP 1
        L1.Signature,
        L1.Parent
    FROM 
        License AS L1 (NOLOCK)
        LEFT OUTER JOIN License (NOLOCK) AS chld
            ON chld.Parent = L1.Id
    WHERE chld.Id IS NULL AND L1.Status <> 4 AND L1.RegNumberInt = @RegNumberInt    