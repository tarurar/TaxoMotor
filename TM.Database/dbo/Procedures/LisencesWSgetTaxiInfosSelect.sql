CREATE PROCEDURE [dbo].[LisencesWSgetTaxiInfosSelect]
	@LicenseNum NVARCHAR(64) = NULL --RegNumber
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

SET @OFFSET = CASE WHEN @PageNumber = 1 THEN 0 ELSE ((@PageNumber - 1) * @Count) + 1 END
SET	@FETCH = @OFFSET + @Count

SET @SortOrder = REPLACE(@SortOrder, N'RegNum',N'L1.TaxiStateNumber')
SET @SortOrder = REPLACE(@SortOrder, N'LicenseNum',N'L1.RegNumberInt')
SET @SortOrder = REPLACE(@SortOrder, N'LicenseDate',N'L1.CreationDate')
SET @SortOrder = REPLACE(@SortOrder, N'Name',N'L1.ShortName')
SET @SortOrder = REPLACE(@SortOrder, N'OgrnNum',N'L1.Ogrn')
SET @SortOrder = REPLACE(@SortOrder, N'OgrnDate',N'L1.OgrnDate')
SET @SortOrder = REPLACE(@SortOrder, N'Brand',N'L1.TaxiBrand')
SET @SortOrder = REPLACE(@SortOrder, N'Model',N'L1.TaxiModel')
SET @SortOrder = REPLACE(@SortOrder, N'Condition',N'L1.Status')


DECLARE @sqlcmd NVARCHAR(max) = N'
SELECT 
	L1.Signature as ''TaxiInfo''
FROM
	License AS L1
	INNER JOIN 
		(
			SELECT
				L1.Id,
				ROW_NUMBER() OVER (ORDER BY '+ @SortOrder + N') AS NN
			FROM 
				License L1
				LEFT JOIN License L2 
					ON L1.Id = L2.Parent
			WHERE 
				L2.Id IS NULL
				AND L1.Status <> 4 
				AND (L1.MO IS NULL OR L1.MO = 0)
				AND (@RegNumberInt IS NULL OR L1.RegNumberInt = @RegNumberInt)
				AND (@LicenseDate IS NULL OR @LicenseDate = L1.CreationDate)
				AND (@Name IS NULL OR L1.ShortName = @Name)
				AND (@OgrnNum IS NULL OR L1.OgrnNum = @OgrnNum)
				AND (@OgrnDate IS NULL OR L1.OgrnDate = @OgrnDate)
				AND (@Brand IS NULL OR L1.TaxiBrand = @Brand)
				AND (@Model IS NULL OR L1.TaxiModel = @Model)
				AND (@RegNum IS NULL OR L1.TaxiStateNumber = @RegNum)
		) AS X
			ON L1.Id = X.Id
WHERE 
	X.NN BETWEEN @OFFSET AND @FETCH
ORDER BY '+ @SortOrder + N'
FOR XML PATH(''''), ROOT (''ArrayOfTaxiInfo'')'

DECLARE @ParmDefinition NVARCHAR(max) = N'@RegNumberInt INT, @LicenseDate DATETIME, @Name NVARCHAR(32), @OgrnNum  NVARCHAR(255), @OgrnDate DATETIME, @Brand NVARCHAR(64), @Model NVARCHAR(64), @RegNum NVARCHAR(24), @OFFSET INT, @FETCH INT';

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

SELECT
	COUNT(*) as TotalRow
FROM 
	License L1
	LEFT JOIN License L2 
		ON L1.Id = L2.Parent
WHERE 
	L2.Id IS NULL
	AND L1.Status <> 4 
	AND (L1.MO IS NULL OR L1.MO = 0)
	AND (@RegNumberInt IS NULL OR L1.RegNumberInt = @RegNumberInt)
	AND (@LicenseDate IS NULL OR @LicenseDate = L1.CreationDate)
	AND (@Name IS NULL OR L1.ShortName = @Name)
	AND (@OgrnNum IS NULL OR L1.OgrnNum = @OgrnNum)
	AND (@OgrnDate IS NULL OR L1.OgrnDate = @OgrnDate)
	AND (@Brand IS NULL OR L1.TaxiBrand = @Brand)
	AND (@Model IS NULL OR L1.TaxiModel = @Model)
	AND (@RegNum IS NULL OR L1.TaxiStateNumber = @RegNum)