--IF OBJECT_ID(N'dbo.LisencesWSgetTaxiLicenseCommonSelect') IS NOT NULL DROP PROCEDURE dbo.LisencesWSgetTaxiLicenseCommonSelect
--GO
--SET ANSI_NULLS ON
--SET QUOTED_IDENTIFIER ON
--GO
--=================================================================
--  Автор       achernenko
--  Дата        06.03.2015
--  Описание    Получение данных для сервиса TaxiLisenses метода getTaxiLicenseСommon
--=================================================================
CREATE PROCEDURE dbo.LisencesWSgetTaxiLicenseCommonSelect
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
    ,@RegNumberInt NVARCHAR(64)	
    
SET	@RegNumberInt = CAST(CAST(@LicenseNum AS INT) AS NVARCHAR(64))
SET @OFFSET = CASE WHEN @PageNumber = 1 THEN 0 ELSE (@PageNumber - 1) * @Count END
SET	@FETCH = @OFFSET + @Count
SET @LicenseNum = RIGHT('00000'+@LicenseNum, 5)
    

SELECT 
    --CAST(License.Signature AS VARBINARY(MAX)) AS 'InfoTaxiData',
    --CAST(License.Signature AS VARCHAR(MAX)) AS 'InfoTaxiData',
    License.Signature AS 'InfoTaxiData',
    License.Id AS 'ID',
    x.Status AS 'Status'
FROM
    License
    INNER JOIN 
        (
            SELECT 
                License.Id,
                CASE WHEN child.Id IS NULL THEN 0 ELSE 1 END AS [Status],
                ROW_NUMBER() OVER (ORDER BY License.Status) AS NN
            FROM
                License
                LEFT OUTER JOIN License AS child
                    ON License.Id = child.Parent
            WHERE
                (NULLIF(@Condition, 1) IS NULL OR (@Condition = 0 AND child.Id IS NULL))
                AND (License.Status <> 4 AND (NULLIF(@Status, 5) IS NULL OR License.Status = @Status))
                AND (@LicenseNum IS NULL OR (License.RegNumber = @LicenseNum OR License.RegNumber = @RegNumberInt))
                AND (@LicenseDate_Begin IS NULL OR License.CreationDate >= @LicenseDate_Begin)
                AND (@LicenseDate_End IS NULL OR License.CreationDate <= @LicenseDate_End)
                AND (@Date_Begin IS NULL OR License.ChangeDate >= @Date_Begin)
                AND (@Date_End IS NULL OR License.ChangeDate <= @Date_End)
                AND (@TillDate_Begin IS NULL OR License.TillDate >= @TillDate_Begin)
                AND (@TillDate_End IS NULL OR License.TillDate <= @TillDate_End)
                AND (NULLIF(@MO, 2) IS NULL OR License.MO = @MO)
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
        ) AS X
            ON License.id = x.id
WHERE X.NN BETWEEN @OFFSET AND @FETCH
FOR XML PATH('InfoTaxiArray'), ROOT('ArrayOfInfoTaxiArray'), BINARY BASE64
    

SELECT 
    COUNT(*) AS CountRow
FROM
    License
    LEFT OUTER JOIN License AS child
        ON License.Id = child.Parent
WHERE
    (NULLIF(@Condition, 1) IS NULL OR (@Condition = 0 AND child.Id IS NULL))
    AND (License.Status <> 4 AND (NULLIF(@Status, 5) IS NULL OR License.Status = @Status))
    AND (@LicenseNum IS NULL OR (License.RegNumber = @LicenseNum OR License.RegNumber = @RegNumberInt))
    AND (@LicenseDate_Begin IS NULL OR License.CreationDate >= @LicenseDate_Begin)
    AND (@LicenseDate_End IS NULL OR License.CreationDate <= @LicenseDate_End)
    AND (@Date_Begin IS NULL OR License.ChangeDate >= @Date_Begin)
    AND (@Date_End IS NULL OR License.ChangeDate <= @Date_End)
    AND (@TillDate_Begin IS NULL OR License.TillDate >= @TillDate_Begin)
    AND (@TillDate_End IS NULL OR License.TillDate <= @TillDate_End)
    AND (NULLIF(@MO, 2) IS NULL OR License.MO = @MO)
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
