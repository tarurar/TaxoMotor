--=================================================================
--  Автор       achernenko
--  Дата        06.03.2015
--  Описание    Получение данных для сервиса TaxiLisenses метода getTaxiLicenseСommon
--=================================================================
CREATE PROCEDURE [dbo].[LisencesWSgetTaxiLicenseCommonSelect] --255
    @PageNumber INT = 1 
    ,@Count INT = 10
    ,@LicenseNum NVARCHAR(64) = NULL -- RegNumber
    ,@LicenseDate_Begin DATETIME = NULL -- CreationDate
    ,@LicenseDate_End DATETIME = NULL --CreationDate
    ,@Date_Begin DATETIME = NULL -- ChangeDate
    ,@Date_End DATETIME = NULL -- ChangeDate
    ,@TillDate_Begin DATETIME = NULL -- TillDate
    ,@TillDate_End DATETIME = NULL -- TillDate
    ,@MO INT = NULL -- MO
    ,@Name NVARCHAR(32) = NULL -- ShortName
    ,@OgrnNum NVARCHAR(255) = NULL -- Ogrn
    ,@INN NVARCHAR(255) = NULL -- INN
    ,@Lfb NVARCHAR(128) = NULL -- Lfb
    ,@Brand NVARCHAR(64) = NULL -- TaxiBrand
    ,@Model NVARCHAR(64) = NULL -- TaxiModel
    ,@RegNum NVARCHAR(24) = NULL -- TaxiStateNumber
    ,@TaxiColor NVARCHAR(24) = NULL -- TaxiColor
    ,@TaxiNumberColor NVARCHAR(64) = NULL -- TaxiNumberColor
    ,@TaxiYear INT = NULL -- TaxiYear    
    ,@Status INT = NULL -- Status
    ,@Condition INT = NULL -- Не имеет дочерних
AS


DECLARE 	
    @OFFSET INT
    ,@FETCH INT
    ,@RegNumberInt INT
    
SET	@RegNumberInt = CAST(RIGHT(@LicenseNum, 5) AS INT)
SET @OFFSET = CASE WHEN @PageNumber = 1 THEN 0 ELSE (@PageNumber - 1) * @Count END
SET	@FETCH = @OFFSET + @Count

SELECT
    *
    ,ROW_NUMBER() OVER (ORDER BY R.MO, R.ID) AS NN
INTO #ids
FROM
    (
        SELECT
            License.Id, 
            0 AS MO,
            CASE WHEN child.Id IS NULL THEN 0 ELSE 1 END AS [Status]
        FROM 
            License (NOLOCK)
            LEFT OUTER JOIN License AS child (NOLOCK)
                ON child.Parent = License.Id
                    AND child.Status <> 4
        WHERE
            (NULLIF(@Condition, 1) IS NULL OR (@Condition = 0 AND child.Id IS NULL))
            AND (License.Status <> 4 AND (NULLIF(@Status, 5) IS NULL OR License.Status = @Status))
            AND (@RegNumberInt IS NULL OR License.RegNumberInt = @RegNumberInt)
            AND (@LicenseDate_Begin IS NULL OR License.CreationDate >= @LicenseDate_Begin)
            AND (@LicenseDate_End IS NULL OR License.CreationDate <= @LicenseDate_End)
            AND (@Date_Begin IS NULL OR License.ChangeDate >= @Date_Begin)
            AND (@Date_End IS NULL OR License.ChangeDate <= @Date_End)
            AND (@TillDate_Begin IS NULL OR License.TillDate >= @TillDate_Begin)
            AND (@TillDate_End IS NULL OR License.TillDate <= @TillDate_End)
            AND (NULLIF(@MO, 2) IS NULL OR @MO = 0)
            AND (@Name IS NULL OR License.ShortName = @Name)
            AND (@OgrnNum IS NULL OR License.Ogrn = @OgrnNum)
            AND (@INN IS NULL OR License.Inn = @INN)
            AND (@Lfb IS NULL OR License.Lfb = @Lfb)
            AND (@Brand IS NULL OR License.TaxiBrand = @Brand)
            AND (@Model IS NULL OR License.TaxiModel = @Model)
            AND (@RegNum IS NULL OR License.TaxiStateNumber = @RegNum)
            AND (@TaxiColor IS NULL OR License.TaxiColor = @TaxiColor)
            AND (@TaxiNumberColor IS NULL 
                OR (License.TaxiNumberColor = N'1' AND @TaxiNumberColor = N'1') 
                OR (ISNULL(License.TaxiNumberColor,N'') <> N'1' AND @TaxiNumberColor <> N'1')
                )
            AND (@TaxiYear IS NULL OR License.TaxiYear = @TaxiYear)
        UNION ALL
        SELECT
            LicenseMO.Id,
            1 AS MO,
            CASE WHEN child.Id IS NULL THEN 0 ELSE 1 END AS [Status]
        FROM 
            LicenseMO (NOLOCK)
            LEFT OUTER JOIN LicenseMO AS child (NOLOCK)
                ON child.Parent = LicenseMO.Id
                    AND child.Status <> 4
        WHERE
            (NULLIF(@Condition, 1) IS NULL OR (@Condition = 0 AND child.Id IS NULL))
            AND (LicenseMO.Status <> 4 AND (NULLIF(@Status, 5) IS NULL OR LicenseMO.Status = @Status))
            AND (@RegNumberInt IS NULL OR LicenseMO.RegNumberInt = @RegNumberInt)
            AND (@LicenseDate_Begin IS NULL OR LicenseMO.CreationDate >= @LicenseDate_Begin)
            AND (@LicenseDate_End IS NULL OR LicenseMO.CreationDate <= @LicenseDate_End)
            AND (@Date_Begin IS NULL OR LicenseMO.ChangeDate >= @Date_Begin)
            AND (@Date_End IS NULL OR LicenseMO.ChangeDate <= @Date_End)
            AND (@TillDate_Begin IS NULL OR LicenseMO.TillDate >= @TillDate_Begin)
            AND (@TillDate_End IS NULL OR LicenseMO.TillDate <= @TillDate_End)
            AND (NULLIF(@MO, 2) IS NULL OR @MO = 1)
            AND (@Name IS NULL OR LicenseMO.ShortName = @Name)
            AND (@OgrnNum IS NULL OR LicenseMO.Ogrn = @OgrnNum)
            AND (@INN IS NULL OR LicenseMO.Inn = @INN)
            AND (@Lfb IS NULL OR LicenseMO.Lfb = @Lfb)
            AND (@Brand IS NULL OR LicenseMO.TaxiBrand = @Brand)
            AND (@Model IS NULL OR LicenseMO.TaxiModel = @Model)
            AND (@RegNum IS NULL OR LicenseMO.TaxiStateNumber = @RegNum)
            AND (@TaxiColor IS NULL OR LicenseMO.TaxiColor = @TaxiColor)
            AND (@TaxiNumberColor IS NULL 
                OR (LicenseMO.TaxiNumberColor = N'1' AND @TaxiNumberColor = N'1') 
                OR (ISNULL(LicenseMO.TaxiNumberColor,N'') <> N'1' AND @TaxiNumberColor <> N'1')
                )
            AND (@TaxiYear IS NULL OR LicenseMO.TaxiYear = @TaxiYear)
    ) AS R

SELECT
    X.Signature AS 'InfoTaxiData',
    X.ID AS 'ID',
    x.Status AS 'Status'
FROM 
    (
        SELECT
            License.Signature,
            'MO=0' + CAST(License.Id AS NVARCHAR(MAX)) AS ID,
            #ids.Status
        FROM 
            License (NOLOCK)
            INNER JOIN #ids
                ON License.Id = #ids.Id
                    AND #ids.MO = 0
                    AND #ids.NN BETWEEN @OFFSET AND @FETCH
        UNION ALL
        SELECT
            LicenseMO.Signature,
            'MO=1' + CAST(LicenseMO.Id AS NVARCHAR(MAX)) AS ID,
            #ids.Status
        FROM 
            LicenseMO (NOLOCK)
            INNER JOIN #ids
                ON LicenseMO.Id = #ids.Id
                    AND #ids.MO = 1
                    AND #ids.NN BETWEEN @OFFSET AND @FETCH
    ) AS X
FOR XML PATH('InfoTaxiArray'), ROOT('ArrayOfInfoTaxiArray')

SELECT 
    COUNT(*) AS CountRow
FROM
    #ids

DROP TABLE #ids