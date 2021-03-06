﻿CREATE PROCEDURE [dbo].[InsertRequestContactForXMLBody]
	@SOURCEXQUERY NVARCHAR(MAX),
	@XMLBODY XML,
	@RECORDID INT OUTPUT
AS
	DECLARE @VALUES tt_KeyValueMap;
	DECLARE @REGADDRESS_ID INT;
	DECLARE @REGADDRESS_XQUERY NVARCHAR(MAX);
	DECLARE @FACTADDRESS_ID INT;
	DECLARE @FACTADDRESS_XQUERY NVARCHAR(MAX);
	DECLARE @BIRTHADDRESS_ID INT;
	DECLARE @BIRTHADDRESS_XQUERY NVARCHAR(MAX);

	SET @REGADDRESS_XQUERY = CONCAT(@SOURCEXQUERY, N'/RegAddress');
	SET @FACTADDRESS_XQUERY = CONCAT(@SOURCEXQUERY, N'/FactAddress');
	SET @BIRTHADDRESS_XQUERY = CONCAT(@SOURCEXQUERY, N'/BirthAddress');
	SET @RECORDID = NULL;

	-- АДРЕС РЕГИСТРАЦИИ
	EXEC [dbo].[InsertAddressForXMLBody] N'RegAddress', @REGADDRESS_XQUERY, @XMLBODY, @REGADDRESS_ID OUTPUT;
	-- ФАКТИЧЕСКИЙ АДРЕС
	EXEC [dbo].[InsertAddressForXMLBody] N'FactAddress', @FACTADDRESS_XQUERY, @XMLBODY, @FACTADDRESS_ID OUTPUT;
	-- АДРЕС РОЖДЕНИЯ
	EXEC [dbo].[InsertAddressForXMLBody] N'BirthAddress', @BIRTHADDRESS_XQUERY, @XMLBODY, @BIRTHADDRESS_ID OUTPUT;

	INSERT INTO @VALUES ([KEY], [VALUE_INT]) VALUES (N'RegAddress', @REGADDRESS_ID);
	INSERT INTO @VALUES ([KEY], [VALUE_INT]) VALUES (N'FactAddress', @FACTADDRESS_ID);
	INSERT INTO @VALUES ([KEY], [VALUE_INT]) VALUES (N'BirthAddress', @BIRTHADDRESS_ID);
	EXEC [dbo].[InsertTMRecordForXMLBody] N'DBO', N'REQUESTCONTACT', @VALUES, @SOURCEXQUERY, @XMLBODY, @RECORDID OUTPUT;
RETURN 0
