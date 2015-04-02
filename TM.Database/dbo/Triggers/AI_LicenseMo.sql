CREATE TRIGGER [dbo].[AI_LicenseMO] ON [dbo].[LicenseMO]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Id INT;
    DECLARE @Parent INT;
    DECLARE @RootParent INT;
    DECLARE @RegNumber NVARCHAR(64);

    SELECT @Id = i.Id
        ,@RegNumber = i.RegNumber
        ,@Parent = i.Parent
        ,@RootParent = i.RootParent
    FROM INSERTED i;

    IF (@RegNumber IS NULL)
    BEGIN
        UPDATE [dbo].[LicenseMO]
        SET [RegNumber] = (
                SELECT TOP 1 [l1].[RegNumberInt] + 1
                FROM [dbo].[LicenseMO] l1
                WHERE NOT EXISTS (
                        SELECT NULL
                        FROM [dbo].[LicenseMO] l2
                        WHERE [l2].[RegNumberInt] = [l1].[RegNumberInt] + 1
                        )
                    AND ([l1].[RegNumberInt] IS NOT NULL)
                ORDER BY [l1].[RegNumberInt]
                )
        WHERE Id = @Id;
    END

    -- первое условие - признак корневой записи
    -- второе условие - если RootParent еще никто до нас не проставил
    IF (
            @Parent IS NULL
            AND @RootParent IS NULL
            )
    BEGIN
        UPDATE [dbo].[LicenseMO]
        SET [RootParent] = @Id
        WHERE Id = @Id;
    END

    UPDATE LicenseMO
    SET Signature = REPLACE(
            (
            SELECT   
                LicenseMO.RegNumber AS 'License/regnumber',
                LicenseMO.BlankSeries AS 'License/blankseries',
                LicenseMO.BlankNo AS 'License/blankno',
                LicenseMO.OrgName AS 'License/orgname',
                LicenseMO.Ogrn AS 'License/ogrn',
                LicenseMO.Inn AS 'License/inn',
                LicenseMO.Status AS 'License/status',
                LicenseMO.Lfb AS 'License/lfb',
                LicenseMO.JuridicalAddress AS 'License/juridicaladdress',
                LicenseMO.PhoneNumber AS 'License/phonenumber',
                LicenseMO.TaxiBrand AS 'License/taxibrand',
                LicenseMO.TaxiModel AS 'License/taximodel',
                LicenseMO.TaxiStateNumber AS 'License/taxistatenumber',
                LicenseMO.TaxiYear AS 'License/taxiyear',
                LicenseMO.OutputDate AS 'License/outputdate',
                LicenseMO.CreationDate AS 'License/creationdate',
                LicenseMO.TillDate AS 'License/tilldate',
                LicenseMO.TillSuspensionDate AS 'License/tillsuspensiondate',
                LicenseMO.SuspensionReason AS 'License/suspensionreason',
                LicenseMO.ChangeReason AS 'License/changereason',
                LicenseMO.ShortName AS 'License/shortname',
                LicenseMO.OgrnDate AS 'License/ogrndate',
                LicenseMO.PostalCode AS 'License/postalcode',
                LicenseMO.Locality AS 'License/locality',
                LicenseMO.Region AS 'License/region',
                LicenseMO.Street AS 'License/street',
                LicenseMO.House AS 'License/house',
                LicenseMO.Structure AS 'License/structure',
                LicenseMO.EMail AS 'License/email',
                LicenseMO.TaxiColor AS 'License/taxocolor',
                LicenseMO.TaxiNumberColor AS 'License/taxinumbercolor',
                LicenseMO.ChangeDate AS 'License/changedate',
                LicenseMO.Brand AS 'License/brand',
                LicenseMO.OgrnNum AS 'License/ogrnnum',
                LicenseMO.OgrnName AS 'License/ogrnname',
                LicenseMO.GRAddress AS 'License/graaddress',
                LicenseMO.InnDate AS 'License/inndate',
                LicenseMO.InnName AS 'License/innname',
                LicenseMO.InnNum AS 'License/innnum',
                LicenseMO.Gps AS 'License/gps',
                LicenseMO.Taxometr AS 'License/taxometr',
                LicenseMO.TODate AS 'License/todate',
                LicenseMO.STSNumber AS 'License/stsnumber',
                LicenseMO.OwnType AS 'License/owntype',
                LicenseMO.MO AS 'License/mo',
                LicenseMO.Fax AS 'License/fax'
            FROM 
                LicenseMO
            WHERE LicenseMO.Id = @Id
            FOR XML PATH('Data'), ROOT ('Envelope')
            ), '<Envelope', '<Envelope xmlns="urn:envelope"')
    WHERE Id = @Id;
END
