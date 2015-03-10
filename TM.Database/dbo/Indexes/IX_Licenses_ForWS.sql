CREATE NONCLUSTERED INDEX [IX_Licenses_ForWS] ON [dbo].[License]
(
	Status,
	MO,
	RegNumber,
	CreationDate,
	ShortName,
	OgrnNum,
	OgrnDate,
	TaxiBrand,
	TaxiModel,
	TaxiStateNumber,
	Id
)