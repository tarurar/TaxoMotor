INSERT INTO [dbo].[ReportSession] ([ReportId], [Date]) VALUES(@ReportId, @Date); 
SELECT SCOPE_IDENTITY();