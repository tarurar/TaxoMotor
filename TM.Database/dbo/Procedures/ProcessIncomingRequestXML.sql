CREATE PROCEDURE [dbo].[ProcessIncomingRequestXML]
	@REQUESTBODY XML,
	@SUCCESS BIT OUT
AS
	SET NOCOUNT ON;

	DECLARE @REQUEST_ID INT;

	SET @SUCCESS = 0;
	SET @REQUEST_ID = NULL;
	
	BEGIN TRANSACTION;

	-- ЗАЯВКА
	BEGIN TRY
		EXEC [dbo].[InsertRequestForXMLBody] N'/CoordinateMessage', @REQUESTBODY, @REQUEST_ID OUTPUT;
		SET @SUCCESS = 1;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		
		EXEC [dbo].[LogLastErrorInfo] '', 'Processing incoming request from XML body into the set of records in RDBMS';

		THROW;
	END CATCH;

	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;

RETURN 0