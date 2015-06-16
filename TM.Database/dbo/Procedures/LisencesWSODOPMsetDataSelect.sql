--=================================================================
--  Автор       achernenko
--  Дата        25.03.2015
--  Описание    Посыл данных в сервис ODOPM
--=================================================================
CREATE PROCEDURE [dbo].[LisencesWSODOPMsetDataSelect]
    @Flag INT,
    @CatalogName NVARCHAR(MAX) = NULL,
    @LicenseId INT = NULL,
    @GUID_OD NVARCHAR(64) = NULL
AS

-- Получение данных для посыла в серивис ОДОПМ
IF @Flag = 1
BEGIN
    SELECT
        par.RootParent AS ID,
        CAST(
            (
                SELECT
                    ISNULL(l.RootParent, l.id) AS 'id',
                    @CatalogName AS 'catalog/@name',
                    CASE 
                        WHEN NULLIF(l.GUID_OD,'') IS NULL 
                            THEN N'ADDED' 
                            ELSE N'MODIFIED' 
                        END 
                    AS 'catalog/item/@action', 
                    N'nameHierarchy' AS 'catalog/item/categories/category/@nameHier',
                    188 AS 'catalog/item/categories/category',
                    (
                        SELECT
                            CASE WHEN NULLIF(l.GUID_OD,N'') IS NOT NULL
                            THEN
                                  (
                                        SELECT 
                                        -1 AS '@field_id',
                                        N'INTEGER' AS '@type',
                                        N'true' AS '@pk',
                                        0 AS 'values/value/@occurrence',
                                        l.GUID_OD AS 'values/value'
                                        FOR XML PATH('attribute'), TYPE
                                    )  
                            END,             
                            (
                                SELECT 
                                -2 AS '@field_id',
                                N'INTEGER' AS '@type',
                                N'true' AS '@pk',
                                0 AS 'values/value/@occurrence',
                                ISNULL(l.RootParent, l.id) AS 'values/value'
                                FOR XML PATH('attribute'), TYPE
                            ),
                            (
                                SELECT 
                                4036 AS '@field_id',
                                N'STRING' AS '@type',
                                N'false' AS '@pk',
                                0 AS 'values/value/@occurrence',
                                RIGHT('00000' + l.RegNumber, 5) AS 'values/value'
                                FOR XML PATH('attribute'), TYPE
                            ),
                            (
                                SELECT 
                                4037 AS '@field_id',
                                N'STRING' AS '@type',
                                N'false' AS '@pk',
                                0 AS 'values/value/@occurrence',
                                UPPER(ISNULL(l.TaxiStateNumber, '')) AS 'values/value'
                                FOR XML PATH('attribute'), TYPE
                            ),
                            (
                                SELECT 
                                4038 AS '@field_id',
                                N'STRING' AS '@type',
                                N'false' AS '@pk',
                                0 AS 'values/value/@occurrence',
                                REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(RTRIM(LTRIM(REPLACE(REPLACE(REPLACE(REPLACE(ISNULL(l.ShortName, ''),' "',' «'),'"','»'),'   ',' '),'  ',' '))),'A','А'),'C','С'),'c','с'),'M','м'),'O','О'),'P','Р'),'E','Е'),'X','Х'),'B','В'),'“','«'),'”','»') AS 'values/value'
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
                                l.BlankSeries + N' № ' +  -- для прода раскомментированть!
                                l.BlankNo AS 'values/value'
                                FOR XML PATH('attribute'), TYPE
                            ),
                            (
                                SELECT 
                                4042 AS '@field_id',
                                N'STRING' AS '@type',
                                N'false' AS '@pk',
                                0 AS 'values/value/@occurrence',
                                REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(RTRIM(LTRIM(ISNULL(l.TaxiBrand,'') + ' ' + ISNULL(l.TaxiModel, ''))),',','.'),'   ',' '),'  ',' '),'`',''),'\','/'),'*',''),'’',''),'&','S'),' "',' «'),'"','»'),'?',''),'|','/'),'~',''),'?',''),'?','') AS 'values/value'
                                FOR XML PATH('attribute'), TYPE
                            ),
                            (
                                SELECT 
                                13379 AS '@field_id',
                                N'STRING' AS '@type',
                                N'false' AS '@pk',
                                0 AS 'values/value/@occurrence',
                                ISNULL(CAST(l.TaxiYear AS VARCHAR(4)),'')  AS 'values/value'
                                FOR XML PATH('attribute'), TYPE
                            ),
                            (
                                SELECT 
                                13380 AS '@field_id',
                                N'STRING' AS '@type',
                                N'false' AS '@pk',
                                0 AS 'values/value/@occurrence',
                                CASE
                                    WHEN l.Status < 2 THEN 'Действующее'
                                    WHEN l.Status = 2 THEN 'Приостановлено'
                                    WHEN l.Status = 3 THEN 'Аннулировано'
                                END AS 'values/value'
                                FOR XML PATH('attribute'), TYPE
                            )                            
                        FOR XML PATH('data'), TYPE
                    ) AS 'catalog/item'
                FROM License l
                WHERE l.Id = par.Id
                FOR XML PATH('message')
            ) AS VARCHAR(MAX)) AS XMLText
    FROM 
	    License AS par
        LEFT OUTER JOIN License AS chd
            ON chd.Parent = par.Id
    WHERE 
        par.Status <> 4
        AND par.Date_OD IS NULL 
        AND chd.Id IS NULL
END

-- Обновление строк ответом из ОДОПМ
IF @Flag = 2
    UPDATE License 
    SET 
        DATE_OD = ISNULL(DATE_OD, GETDATE()),
        GUID_OD = @GUID_OD
    WHERE License.RootParent = @LicenseId