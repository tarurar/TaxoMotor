--SET QUOTED_IDENTIFIER ON
--SET ANSI_NULLS ON
--GO
--=================================================================
--  Автор       achernenko
--  Дата        25.03.2015
--  Описание    Посыл данных в сервис ODOPM
--=================================================================
CREATE PROCEDURE dbo.LisencesWSODOPMsetDataSelect
    @Flag INT,
    @CatalogName NVARCHAR(MAX) = NULL,
    @LicenseId INT = NULL,
    @GUID_OD NVARCHAR(64) = NULL
AS

-- Получение данных для посыла в серивис ОДОПМ
IF @Flag = 1
BEGIN

    DECLARE @lid INT 

    SELECT TOP 1
        @lid = l.Id
    FROM License l
    WHERE l.DATE_OD is null and l.Status < 4  and (l.MO is null or l.MO = 0)

    SELECT @lid AS LicenseID

    SELECT
        0 AS 'id',
        @CatalogName AS 'catalog/@name',
        CASE WHEN NULLIF(l.GUID_OD,'') IS NULL THEN N'ADDED' ELSE N'MODIFIED' END AS 'catalog/item/@action', 
        N'nameHierarchy' AS 'catalog/item/categories/category/@nameHier',
        188 AS 'catalog/item/categories/category',
        (
            SELECT 
                (
                    SELECT 
                    -2 AS '@field_id',
                    N'INTEGER' AS '@type',
                    N'true' AS '@pk',
                    ISNULL(l.RootParent, l.Id) AS 'values/value/@occurrence',
                    ISNULL(NULLIF(l.GUID_OD,N''), N'-2') AS 'values/value'
                    FOR XML PATH('attribute'), TYPE
                ),
                (
                    SELECT 
                    4036 AS '@field_id',
                    N'STRING' AS '@type',
                    N'false' AS '@pk',
                    0 AS 'values/value/@occurrence',
                    l.RegNumber AS 'values/value'
                    FOR XML PATH('attribute'), TYPE
                ),
                (
                    SELECT 
                    4037 AS '@field_id',
                    N'STRING' AS '@type',
                    N'false' AS '@pk',
                    0 AS 'values/value/@occurrence',
                    ISNULL(l.TaxiStateNumber, '') AS 'values/value'
                    FOR XML PATH('attribute'), TYPE
                ),
                (
                    SELECT 
                    4038 AS '@field_id',
                    N'STRING' AS '@type',
                    N'false' AS '@pk',
                    0 AS 'values/value/@occurrence',
                    ISNULL(l.ShortName, '') AS 'values/value'
                    FOR XML PATH('attribute'), TYPE
                ),
                (
                    SELECT 
                    4039 AS '@field_id',
                    N'STRING' AS '@type',
                    N'false' AS '@pk',
                    0 AS 'values/value/@occurrence',
                    ISNULL(l.INN, '') AS 'values/value'
                    FOR XML PATH('attribute'), TYPE
                ),
                (
                    SELECT 
                    4040 AS '@field_id',
                    N'STRING' AS '@type',
                    N'false' AS '@pk',
                    0 AS 'values/value/@occurrence',
                    ISNULL(l.Ogrn, '') AS 'values/value'
                    FOR XML PATH('attribute'), TYPE
                ),
                (
                    SELECT 
                    4041 AS '@field_id',
                    N'STRING' AS '@type',
                    N'false' AS '@pk',
                    0 AS 'values/value/@occurrence',
                    l.BlankSeries + N' № ' + l.BlankNo AS 'values/value'
                    FOR XML PATH('attribute'), TYPE
                ),
                (
                    SELECT 
                    4042 AS '@field_id',
                    N'STRING' AS '@type',
                    N'false' AS '@pk',
                    0 AS 'values/value/@occurrence',
                    RTRIM(LTRIM(ISNULL(l.TaxiBrand,'') + ' ' + ISNULL(l.TaxiModel, ''))) AS 'values/value'
                    FOR XML PATH('attribute'), TYPE
                )
            FOR XML PATH('data'), TYPE
        ) AS 'catalog/item'
    FROM License l
    WHERE l.Id = @lid
    FOR XML PATH('message')
END

-- Обновление строки с ИД из ответа ОДОПМ
IF @Flag = 2
    UPDATE License 
    SET 
        DATE_OD = GETDATE(),
        GUID_OD = ISNULL(@GUID_OD, GUID_OD)
    WHERE License.Id = @LicenseId