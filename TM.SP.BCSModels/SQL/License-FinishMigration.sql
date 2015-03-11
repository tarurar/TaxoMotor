UPDATE [dbo].[LicenseMigrationTicket]
SET [Status] = @Status
	,[FinishDate] = GETDATE()
	,[ErrorInfo] = @ErrorInfo
	,[StackTrace] = @StackTrace
WHERE [Id] = @Id