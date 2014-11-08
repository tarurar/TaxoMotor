CREATE TRIGGER [AI_License] ON [dbo].[License]
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Id INT;

	SELECT @Id = i.Id
	FROM INSERTED i;

	UPDATE [dbo].[License]
	SET [RegNumber] = CAST((
				SELECT TOP 1 IntRegNumber
				FROM (
					SELECT CAST(RegNumber AS INT) + 1 AS IntRegNumber
					FROM [dbo].[License]
					WHERE RegNumber IS NOT NULL
					) ii
				WHERE IntRegNumber NOT IN (
						SELECT CAST(RegNumber AS INT)
						FROM [dbo].[License]
						WHERE RegNumber IS NOT NULL
						)
				ORDER BY IntRegNumber
				) AS NVARCHAR(64))
	WHERE Id = @Id;
END