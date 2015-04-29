--=================================================================
--  Автор       achernenko
--  Дата        28.04.2015
--  Описание    Вставка в таблицу лога
--=================================================================
ALTER PROCEDURE [dbo].[AppLogInsert]	
    @ApplicationName NVARCHAR(500),
    @Direction NCHAR(1),
    @Message NVARCHAR(MAX),
    @MessageID UNIQUEIDENTIFIER
AS
INSERT INTO AppLog
        (ApplicationName,
         Direction,
         Message,
         MessageID)
VALUES
        (@ApplicationName,
         @Direction,
         @Message,
         @MessageID)