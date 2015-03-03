--SET QUOTED_IDENTIFIER ON
--SET ANSI_NULLS ON
--GO
--IF OBJECT_ID('dbo.LisencesWSgetTaxiInfosSelect') IS NOT NULL DROP PROCEDURE dbo.LisencesWSgetTaxiInfosSelect
--GO
--=================================================================
--  Автор       achernenko
--  Дата        03.03.2015
--  Описание    Получение выборки для сервиса TaxiServices.TaxiLicenses.TaxiLicenses метод getTaxiInfos
--				в виде XML
--=================================================================
CREATE PROCEDURE dbo.LisencesWSgetTaxiInfosSelect
	@LicenseNum		NVARCHAR(64) = NULL --RegNumber
	,@LicenseDate	DATETIME = NULL-- CreationDate
	,@Name			NVARCHAR(32) = NULL-- ShortName
	,@OgrnNum		NVARCHAR(255) = NULL--Ogrn
	,@OgrnDate		DATETIME = NULL--OgrnDate
	,@Brand			NVARCHAR(64) = NULL-- TaxiBrand
	,@Model			NVARCHAR(64) = NULL --TaxiModel
	,@RegNum		NVARCHAR(24) = NULL--TaxiStateNumber
	,@SortOrder		NVARCHAR(30) = NULL
	,@PageNumber	INT = 1
	,@Count			INT = 10
AS

DECLARE 	
	@OFFSET INT = 0
	,@FETCH INT
	,@RegNumberInt NVARCHAR(64)	
	
SELECT 
	@RegNumberInt = CAST(CAST(@LicenseNum AS INT) AS NVARCHAR(64))
	,@FETCH = @Count
	,@OFFSET = CASE WHEN @PageNumber = 1 THEN 0 ELSE (@PageNumber - 1) * @Count END

SELECT 
	License.Signature as 'TaxiInfo'
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
		
		ORDER BY 
			CASE @SortOrder
					WHEN 'LicenseNum' THEN L1.RegNumber
					WHEN 'Name' THEN L1.ShortName
					WHEN 'OgrnNum' THEN L1.Ogrn
					WHEN 'Brand' THEN L1.TaxiBrand
					WHEN 'TaxiModel' THEN L1.TaxiModel
					WHEN 'RegNum' THEN L1.TaxiStateNumber
					WHEN NULL THEN NULL 
			END,
			CASE @SortOrder			
					WHEN 'LicenseDate' THEN L1.CreationDate            
					WHEN 'OgrnDate' THEN L1.OgrnDate
					WHEN NULL THEN NULL 
			END	
		OFFSET @OFFSET ROWS FETCH NEXT @FETCH ROWS ONLY
	) AS X
		ON License.Id = X.Id
FOR XML PATH(''), ROOT ('ArrayOfTaxiInfo')
GO