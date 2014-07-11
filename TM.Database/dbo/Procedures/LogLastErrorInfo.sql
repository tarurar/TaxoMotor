CREATE PROCEDURE [dbo].[LogLastErrorInfo] @USERCONTEXT NVARCHAR(MAX),
	@SYSTEMCONTEXT NVARCHAR(MAX)
AS
INSERT INTO [dbo].[ErrorData] (
	[Number],
	[Severity],
	[State],
	[Procedure],
	[Message],
	[Line],
	[UserContext],
	[SystemContext]
	)
SELECT ERROR_NUMBER(),
	ERROR_SEVERITY(),
	ERROR_STATE(),
	ERROR_PROCEDURE(),
	ERROR_MESSAGE(),
	ERROR_LINE(),
	@USERCONTEXT,
	@SYSTEMCONTEXT;

RETURN 0
