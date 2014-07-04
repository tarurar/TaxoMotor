CREATE FUNCTION [dbo].[fn_GetCoordinateV5RequestMessageId]
(
	@messageXML xml
)
RETURNS VARCHAR(36)
WITH SCHEMABINDING
AS
BEGIN
	DECLARE @RETVALUE VARCHAR(36);

	SET @RETVALUE = @messageXML.value('
		/CoordinateMessage[1]/ServiceHeader[1]/MessageId[1]', 'varchar(36)');

	RETURN @RETVALUE;
END