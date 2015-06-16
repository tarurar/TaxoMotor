CREATE NONCLUSTERED INDEX [IX_License_ID_ParentID] ON [dbo].[License]
(
	[Parent] ASC,
	[Id] ASC
)