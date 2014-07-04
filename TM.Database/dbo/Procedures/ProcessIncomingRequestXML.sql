CREATE PROCEDURE [DBO].[PROCESSINCOMINGREQUESTXML]
	@REQUESTBODY XML,
	@SUCCESS BIT OUT
AS
	SET NOCOUNT ON;

	DECLARE @DECLARANT_ACCOUNT_ID INT;
	DECLARE @DECLARANT_CONTACT_ID INT;
	DECLARE @LAST_TAXI_ID INT;
	DECLARE @SERVICE_PROPS_ID INT;

	SET @SUCCESS = 0;
	SET @DECLARANT_ACCOUNT_ID = NULL;
	SET @DECLARANT_CONTACT_ID = NULL;
	SET @LAST_TAXI_ID = NULL;
	SET @SERVICE_PROPS_ID = NULL;
	
	-- ЗАЯВИТЕЛЬ - ЮРИДИЧЕСКОЕ ЛИЦО
	EXEC [dbo].[InsertRequestAccountForXMLBody] N'/CoordinateMessage/Message/Declarant[@xsi:type="RequestAccount"]', @REQUESTBODY, @DECLARANT_ACCOUNT_ID OUTPUT;
	-- ЗАЯВИТЕЛЬ - ФИЗИЧЕСКОЕ ЛИЦО
	EXEC [dbo].[InsertRequestContactForXMLBody] N'/CoordinateMessage/Message/Declarant[@xsi:type="RequestContact"]', @REQUESTBODY, @DECLARANT_CONTACT_ID OUTPUT;


	-- TEST: СПИСОК АВТОМАШИН ТАКСИ И SERVICE PROPERTIES
	DECLARE @VALUES TT_KEYVALUEMAP;
	DECLARE @MESSAGEID VARCHAR(36);
	SET @MESSAGEID = [dbo].[fn_GetCoordinateV5RequestMessageId] (@REQUESTBODY);
	INSERT INTO @VALUES([KEY], [VALUE_STR]) VALUES(N'MessageId', @MESSAGEID);
	EXEC [dbo].[InsertRecordForXMLBody] N'DBO', N'SERVICEPROPERTIES', @VALUES, N'/CoordinateMessage/Message/CustomAttributes/ServiceProperties', @REQUESTBODY, @SERVICE_PROPS_ID OUTPUT;
	
	INSERT INTO @VALUES([KEY], [VALUE_INT]) VALUES(N'ServiceProperties', @SERVICE_PROPS_ID);
	EXEC [dbo].[InsertRecordForXMLBody] N'DBO', N'TAXI_INFO', @VALUES, N'/CoordinateMessage/Message/CustomAttributes/ServiceProperties/taxi_infolist/taxi_info', @REQUESTBODY, @LAST_TAXI_ID OUTPUT;
	-- **************************************************
RETURN 0