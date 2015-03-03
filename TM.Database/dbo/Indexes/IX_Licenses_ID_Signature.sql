CREATE NONCLUSTERED INDEX [IX_Licenses_ID_Signature] ON [dbo].[License]
(
	id
)
INCLUDE ([Signature])