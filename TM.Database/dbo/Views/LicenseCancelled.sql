CREATE VIEW [dbo].[LicenseCancelled]
AS
SELECT L1.*
FROM [dbo].[License] AS L1
LEFT JOIN [dbo].[License] L2 ON L1.Id = L2.Parent
WHERE L2.Id IS NULL                     -- last record
	AND L1.STATUS = 3                   -- cancelled
