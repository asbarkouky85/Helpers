--<[TABLE_NAME]>	: Replace with table name
--<[COLUMN_NAME]>	: Replace with column name
--<[PATTERN]>		: Replace with pattern
--<[EXTRA_DATA]>	: optional extra data to pattern

CREATE TRIGGER <[TABLE_NAME]>_TRG
   ON  <[TABLE_NAME]>
   AFTER INSERT
AS 
BEGIN

	DECLARE @i INT=1;
	DECLARE @id BIGINT;
	DECLARE @extraData BIGINT=<[EXTRA_DATA]>;
	DECLARE @pattern VARCHAR(255)=<[PATTERN]>;
	DECLARE @minLent INT=<[LENGTH]>;
	DECLARE @seq VARCHAR(50);

	WHILE(@i<=(SELECT Count(*) FROM inserted))
	BEGIN
		SELECT 
			@id=Id
		FROM (
			SELECT 
				ROW_NUMBER() OVER (ORDER BY Id) i,
				Id
			FROM inserted
		) INS
		WHERE i = @i;
		
		EXEC dbo.GenerateSequenceNumber '<[TABLE_NAME]>' , '<[COLUMN_NAME]>' , @pattern , @minLent , @extraData , @seq OUTPUT
		UPDATE <[TABLE_NAME]> SET <[COLUMN_NAME]> = @seq WHERE Id = @id;
		SET @i = @i + 1;
	END;
END;
GO


