CREATE FUNCTION [dbo].[fn_BuildAddressSingleString] (@Id INT)
RETURNS NVARCHAR(1024)
AS
BEGIN
	DECLARE @retVal NVARCHAR(1024);

	SELECT @retVal = (
			CASE 
				WHEN [Country] IS NOT NULL
					THEN [Country] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [PostalCode] IS NOT NULL
					THEN [PostalCode] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [Locality] IS NOT NULL
					THEN [Locality] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [Region] IS NOT NULL
					THEN [Region] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [City] IS NOT NULL
					THEN [City] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [Town] IS NOT NULL
					THEN [Town] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [Street] IS NOT NULL
					THEN [Street] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [House] IS NOT NULL
					THEN N'д. ' + [House] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [Building] IS NOT NULL
					THEN N'корп. ' + [Building] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [Structure] IS NOT NULL
					THEN N'стр. ' + [Structure] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [Facility] IS NOT NULL
					THEN N'соор. ' + [Facility] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [Ownership] IS NOT NULL
					THEN N'влад. ' + [Ownership] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [Flat] IS NOT NULL
					THEN N'кв. ' + [Flat] + ' '
				ELSE ''
				END
			) + (
			CASE 
				WHEN [POBox] IS NOT NULL
					THEN N'а/я ' + [POBox] + ' '
				ELSE ''
				END
			)
	FROM [dbo].[Address]
	WHERE Id = @Id;

	RETURN ltrim(rtrim(@retVal));
END