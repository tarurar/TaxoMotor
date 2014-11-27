CREATE TRIGGER [dbo].[AI_License] ON [dbo].[License]
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Id INT;
	DECLARE @Parent INT;
	DECLARE @RootParent INT;
	DECLARE @RegNumber NVARCHAR(64);

	SELECT @Id = i.Id
		,@RegNumber = i.RegNumber
		,@Parent = i.Parent
		,@RootParent = i.RootParent
	FROM INSERTED i;

	IF (@RegNumber IS NULL)
	BEGIN
		UPDATE [dbo].[License]
		SET [RegNumber] = CAST((
					SELECT TOP 1 IntRegNumber
					FROM (
						SELECT COALESCE(CAST(RegNumber AS INT) + 1, 1) AS IntRegNumber
						FROM [dbo].[License]
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

	-- первое условие - признак корневой записи
	-- второе условие - если RootParent еще никто до нас не проставил
	IF (
			@Parent IS NULL
			AND @RootParent IS NULL
			)
	BEGIN
		UPDATE [dbo].[License]
		SET [RootParent] = @Id
		WHERE Id = @Id;
	END
END
GO

