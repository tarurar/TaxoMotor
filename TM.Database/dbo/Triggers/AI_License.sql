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
		SET [RegNumber] = (
				SELECT TOP 1 [l1].[RegNumberInt] + 1
				FROM [dbo].[License] l1
				WHERE NOT EXISTS (
						SELECT NULL
						FROM [dbo].[License] l2
						WHERE [l2].[RegNumberInt] = [l1].[RegNumberInt] + 1
						)
					AND ([l1].[RegNumberInt] IS NOT NULL)
				ORDER BY [l1].[RegNumberInt]
				)
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

