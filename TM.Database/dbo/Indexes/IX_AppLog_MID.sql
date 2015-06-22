CREATE NONCLUSTERED INDEX [IX_AppLog_MID] ON [dbo].[AppLog]
(
	[Direction] ASC,
	[MessageID] ASC,
	[ID] ASC
)