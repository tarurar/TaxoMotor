INSERT INTO [dbo].[LicenseMigrationTicket] (
	[Status]
	,[StartDate]
	,[LicenseId]
	)
OUTPUT INSERTED.[Id]
	,INSERTED.[Status]
	,INSERTED.[LicenseId]
SELECT @Status
	,GETDATE()
	,l.[Id]
FROM [dbo].[License] l
LEFT JOIN [dbo].[LicenseMigrationTicket] m ON m.[LicenseId] = l.[Id]
WHERE m.[Title] IS NULL
	AND l.[Id] = @ItemId
	AND l.[Status] <> 4
ORDER BY l.[Parent] ASC