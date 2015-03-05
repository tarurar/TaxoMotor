--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--=================================================================
--  Автор       achernenko
--  Дата        03.03.2015
--  Описание    Получение выборки для сервиса TaxiServices.TaxiLicenses.TaxiLicenses метод getTaxiInfos
--				в виде XML
--=================================================================
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
	@OFFSET INT = 0
	,@FETCH INT
	,@RegNumberInt NVARCHAR(64)	
	
SELECT 
	@RegNumberInt = CAST(CAST(@LicenseNum AS INT) AS NVARCHAR(64))
	,@FETCH = @Count
	,@OFFSET = CASE WHEN @PageNumber = 1 THEN 0 ELSE (@PageNumber - 1) * @Count END
	,@SortOrder = REPLACE(ISNULL(@SortOrder, N'LicenseNum'), N';', N', ')

SET @SortOrder = REPLACE(@SortOrder, N'RegNum',N'L1.TaxiStateNumber')
SET @SortOrder = REPLACE(@SortOrder, N'LicenseNum',N'L1.RegNumber')
SET @SortOrder = REPLACE(@SortOrder, N'LicenseDate',N'L1.CreationDate')
SET @SortOrder = REPLACE(@SortOrder, N'Name',N'L1.ShortName')
SET @SortOrder = REPLACE(@SortOrder, N'OgrnNum',N'L1.Ogrn')
SET @SortOrder = REPLACE(@SortOrder, N'OgrnDate',N'L1.OgrnDate')
SET @SortOrder = REPLACE(@SortOrder, N'Brand',N'L1.TaxiBrand')
SET @SortOrder = REPLACE(@SortOrder, N'Model',N'L1.TaxiModel')
SET @SortOrder = REPLACE(@SortOrder, N'Condition',N'L1.Status')


SELECT 
	@LicenseNum = RIGHT('00000'+@LicenseNum, 5)



DECLARE @sqlcmd NVARCHAR(max) = N'
SELECT 
	License.Signature as ''TaxiInfo''
FROM
	License
	INNER JOIN 
		(
			SELECT
				L1.Id
			FROM 
				License L1
				LEFT JOIN License L2 
					ON L1.Id = L2.Parent
			WHERE 
				L2.Id IS NULL
				AND L1.Status <> 4 
				AND (L1.MO IS NULL OR L1.MO = 0)
				AND (@LicenseNum IS NULL OR (L1.RegNumber = @LicenseNum OR L1.RegNumber = @RegNumberInt))
				AND (@LicenseDate IS NULL OR @LicenseDate = L1.CreationDate)
				AND (@Name IS NULL OR L1.ShortName = @Name)
				AND (@OgrnNum IS NULL OR L1.OgrnNum = @OgrnNum)
				AND (@OgrnDate IS NULL OR L1.OgrnDate = @OgrnDate)
				AND (@Brand IS NULL OR L1.TaxiBrand = @Brand)
				AND (@Model IS NULL OR L1.TaxiModel = @Model)
				AND (@RegNum IS NULL OR L1.TaxiStateNumber = @RegNum)
		
			ORDER BY '+ @SortOrder + N'
			OFFSET @OFFSET ROWS FETCH NEXT @FETCH ROWS ONLY
		) AS X
			ON License.Id = X.Id
FOR XML PATH(''''), ROOT (''ArrayOfTaxiInfo'')'

DECLARE @ParmDefinition NVARCHAR(max) = N'@LicenseNum NVARCHAR(64), @RegNumberInt NVARCHAR(64), @LicenseDate DATETIME, @Name NVARCHAR(32), @OgrnNum  NVARCHAR(255), @OgrnDate DATETIME, @Brand NVARCHAR(64), @Model NVARCHAR(64), @RegNum NVARCHAR(24), @OFFSET INT, @FETCH INT';

EXECUTE sp_executesql 
	@sqlcmd, 
	@ParmDefinition, 
	@LicenseNum = @LicenseNum, 
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
	AND (@LicenseNum IS NULL OR (L1.RegNumber = @LicenseNum OR L1.RegNumber = @RegNumberInt))
	AND (@LicenseDate IS NULL OR @LicenseDate = L1.CreationDate)
	AND (@Name IS NULL OR L1.ShortName = @Name)
	AND (@OgrnNum IS NULL OR L1.OgrnNum = @OgrnNum)
	AND (@OgrnDate IS NULL OR L1.OgrnDate = @OgrnDate)
	AND (@Brand IS NULL OR L1.TaxiBrand = @Brand)
	AND (@Model IS NULL OR L1.TaxiModel = @Model)
	AND (@RegNum IS NULL OR L1.TaxiStateNumber = @RegNum)
