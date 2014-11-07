﻿CREATE VIEW [dbo].[LicenseAll]
AS
SELECT L1.*
	,CASE 
		WHEN L2.Id IS NULL
			THEN 1
		ELSE 0
		END AS IsLast
FROM [dbo].[License] AS L1
LEFT JOIN [dbo].[License] L2 ON L1.Id = L2.Parent
WHERE L1.STATUS <> 4 -- no drafts