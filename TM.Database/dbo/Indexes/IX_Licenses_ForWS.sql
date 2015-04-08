CREATE NONCLUSTERED INDEX [IX_Licenses_ForWS] ON [dbo].[License]
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
