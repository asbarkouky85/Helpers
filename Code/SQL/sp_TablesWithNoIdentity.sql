CREATE PROCEDURE dbo.TablesWithNoIdentity
	
AS
	BEGIN
		select 
			TABLE_SCHEMA+'.'+ TABLE_NAME 
		from INFORMATION_SCHEMA.TABLES 
		where 
			TABLE_NAME not in(
				select 
					TABLE_NAME
				from INFORMATION_SCHEMA.COLUMNS
				where 
					COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1
			)
		order by TABLE_NAME;
	END
GO
