CREATE NONCLUSTERED INDEX [IX_LicensesMO_ForWS] ON [dbo].[LicenseMO]
(
	[Status] ASC,
	[MO] ASC,
	[RegNumberInt] ASC,
	[CreationDate] ASC,
	[ShortName] ASC,
	[OgrnNum] ASC,
	[OgrnDate] ASC,
	[TaxiBrand] ASC,
	[TaxiModel] ASC,
	[TaxiStateNumber] ASC,
	[Id] ASC
)