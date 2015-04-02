CREATE NONCLUSTERED INDEX [IX_LicensesMo_ForWS] ON [dbo].[LicenseMo]
(
	[Status],
	[MO],
	[RegNumber],
	[CreationDate],
	[ShortName],
	[OgrnNum],
	[OgrnDate],
	[TaxiBrand],
	[TaxiModel],
	[TaxiStateNumber],
	[Id]
)