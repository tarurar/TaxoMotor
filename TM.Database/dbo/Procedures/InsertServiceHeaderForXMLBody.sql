﻿CREATE PROCEDURE [dbo].[InsertServiceHeaderForXMLBody]
	@SOURCEXQUERY NVARCHAR(MAX),
	@XMLBODY XML,
	@RECORDID INT OUTPUT
AS
	DECLARE @VALUES TT_KEYVALUEMAP

	SET @RECORDID = NULL;

	EXEC [dbo].[InsertTMRecordForXMLBody] N'DBO', N'SERVICEHEADER', @VALUES, @SOURCEXQUERY, @XMLBODY, @RECORDID OUTPUT;
RETURN 0
