DELETE
FROM [dbo].[LicenseMigrationTicket]
WHERE LicenseId IN (
		SELECT Id
		FROM [dbo].[License]
		WHERE [Status] = @Status
			AND [TaxiId] = @TaxiId
		);

DELETE
FROM [dbo].[License]
WHERE [Status] = @Status
	AND [TaxiId] = @TaxiId;