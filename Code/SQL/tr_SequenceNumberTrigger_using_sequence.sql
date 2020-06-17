--<[TABLE_NAME]> 	: table name
--<[COLUMN_NAME]>	: column name
--<[SEQUENCE_NAME]>	: sequence name

CREATE SEQUENCE <[SEQUENCE_NAME]>
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE;

Alter TRIGGER <[TABLE_NAME]>_TRG
   ON  <[TABLE_NAME]>
   AFTER INSERT
AS 
BEGIN

	DECLARE @nowYear int=YEAR(GETDATE());
	DECLARE @nowMonth int=MONTH(GETDATE());
	DECLARE @lastYear int;
	DECLARE @lastMonth int;
	DECLARE @inc int;
	DECLARE @seq varchar(50);

	SELECT @lastYear= YEAR ( MAX (CreatedOn) ) , @lastMonth = MONTH ( MAX ( CreatedOn ) ) FROM <[TABLE_NAME]>;

	if(@lastYear!=@nowYear AND @lastMonth!=@nowMonth)
		exec [dbo].[ResetSequence] '<[SEQUENCE_NAME]>';
	
	SELECT @inc=NEXT VALUE FOR <[SEQUENCE_NAME]>;

	SELECT @seq=CONCAT('WR'+RIGHT(@nowYear,2),RIGHT(@nowMonth,2),RIGHT('00000'+CAST(@inc as varchar(5)),5));

    UPDATE <[TABLE_NAME]> SET <[COLUMN_NAME]> = @seq , CreatedOn = GETDATE() WHERE Id = (Select Id FROM inserted);

END;