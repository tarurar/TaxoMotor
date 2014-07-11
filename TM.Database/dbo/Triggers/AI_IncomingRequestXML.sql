﻿CREATE TRIGGER [AI_IncomingRequestXML]
	ON [dbo].[IncomingRequestXML]
	AFTER INSERT
	AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @ID INT, @REQUESTBODY XML, @MESSAGEID VARCHAR(36);
		DECLARE @ERR_CONTEXT VARCHAR(255);
		DECLARE @SUCCESS BIT;

		DECLARE C_INS CURSOR FOR
		SELECT ID, REQUESTBODY, BODYMESSAGEID FROM INSERTED;

		OPEN C_INS;

		FETCH NEXT FROM C_INS INTO @ID, @REQUESTBODY, @MESSAGEID;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRY
				EXEC [dbo].[ProcessIncomingRequestXML] @REQUESTBODY, @SUCCESS OUTPUT;
				IF @SUCCESS = 1
					UPDATE [dbo].[IncomingRequestXML] SET [OUTDATE] = GETDATE() WHERE ID = @ID;
			END TRY
			BEGIN CATCH
				SET @ERR_CONTEXT = CONCAT('INCOMING REQUEST MESSAGE ID: ', @MESSAGEID);
				EXEC [dbo].[LogLastErrorInfo] @ERR_CONTEXT, 'TRIGGER AFTER INSERT [dbo].[IncomingRequestXML]';
			END CATCH
			
			FETCH NEXT FROM C_INS INTO @ID, @REQUESTBODY, @MESSAGEID;
		END

		CLOSE C_INS;

		DEALLOCATE C_INS;
	END
