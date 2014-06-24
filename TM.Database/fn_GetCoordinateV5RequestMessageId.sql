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
		declare namespace s="http://schemas.xmlsoap.org/soap/envelope/";
		declare namespace h="http://asguf.mos.ru/rkis_gu/coordinate/v5/";
		declare default element namespace "http://asguf.mos.ru/rkis_gu/coordinate/v5/";
		/s:Envelope[1]/s:Header[1]/h:ServiceHeader[1]/MessageId[1]', 'varchar(36)');

	RETURN @RETVALUE;
END