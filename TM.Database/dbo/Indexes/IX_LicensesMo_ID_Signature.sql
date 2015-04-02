CREATE NONCLUSTERED INDEX [IX_LicensesMo_ID_Signature] ON [dbo].[LicenseMo]
(
	id
)
INCLUDE ([Signature])