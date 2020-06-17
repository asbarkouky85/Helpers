ALTER PROCEDURE [dbo].[CompareStructures](
 @db1 varchar(100),
 @db2 varchar(100),
 @idDefinition varchar(70),
 @execute bit=0
)
AS
BEGIN
	SET NOCOUNT ON;

	declare @q nvarchar(max);
	declare @i int=1;
	declare @out nvarchar(max);
	declare @q2 nvarchar(max);

	declare @tables table(
		name varchar(70)
	);

	declare @columns table(
		TABLE_NAME varchar(100),
		COLUMN_NAME varchar(100),
		DATA_TYPE varchar(100),
		NUMERIC_PRECISION  int,
		NUMERIC_SCALE int,
		CHARACTER_MAXIMUM_LENGTH int,
		ORDINAL_POSITION int,
		IS_NULLABLE varchar(100));

	declare @constraints table(
		Name varchar(100),
		ChildTable varchar(100),
		ChildColumn varchar(100),
		ParentTable varchar(100),
		ParentColumn varchar(100),
		UpdateRule varchar(100),
		DeleteRule varchar(100),
		OldKey varchar(100)
	);

	declare @other table(
		name varchar(100),
		SrcCreate datetime,
		SrcUpdate datetime,
		IsNew bit,
		[Type] varchar(20),
		[Definition] nvarchar (max)
	);

	set @q=N'
	select SCHEMA_NAME
	from '+@db1+'.INFORMATION_SCHEMA.SCHEMATA Src
	where 
		SCHEMA_OWNER=''dbo'' AND 
		SCHEMA_NAME NOT IN (
			select SCHEMA_NAME
			from '+@db2+'.INFORMATION_SCHEMA.SCHEMATA 
			where SCHEMA_OWNER=''dbo''
		)';

	insert @tables
	exec sp_executesql @q;

	set @i=1;
	while(@i<=(select count(*) from @tables))
		begin
			select @out='CREATE SCHEMA '+name+';'
			from (
				select 
					row_number() over (order by name) i,
					*
				from @tables 
			) RWS
			where i=@i;
			set @i=@i+1;
	
			print @out;
			print 'GO';
		
		end;

	select @q='
		SELECT 
			TABLE_SCHEMA+''.''+TABLE_NAME name 
		FROM '+ @db1+'.INFORMATION_SCHEMA.TABLES 
		WHERE 
			TABLE_SCHEMA+''.''+TABLE_NAME NOT IN (
				SELECT TABLE_SCHEMA+''.''+TABLE_NAME 
				FROM '+@db2+'.INFORMATION_SCHEMA.TABLES
			) AND TABLE_NAME !=''sysdiagrams'';';

	delete from @tables;
	insert @tables
	exec sp_executesql @q;

	set @i=1;

	if((select count(*) from @tables)=0)
		print '-- NO ADDED TABLES';
	else
		print '-- ADDED TABLES :';

	while(@i<=(select count(*) from @tables))
	begin
		select @out='CREATE TABLE '+@db2+'.'+name+' ('+@idDefinition+' PRIMARY KEY);'
		from (
			select 
				row_number() over (order by name) i,
				*
			from @tables 
		) RWS
		where i=@i;
		set @i=@i+1;
	
		print @out;
		if(@execute=1)
		begin
			exec sp_executesql @out;
			print ' --EXECUTED';
		end;
	end;

	if((select count(*) from @tables)>0)
		RETURN;

	print '';

	select @q='
		SELECT 
			TABLE_SCHEMA+''.''+TABLE_NAME TABLE_NAME,
			COLUMN_NAME,
			DATA_TYPE,
			NUMERIC_PRECISION,
			NUMERIC_SCALE,
			CHARACTER_MAXIMUM_LENGTH,
			ORDINAL_POSITION,
			IS_NULLABLE
		FROM 
			'+@db1+'.INFORMATION_SCHEMA.columns as t1
		WHERE 
		NOT EXISTS
		(
			SELECT *
			FROM 
				'+@db2+'.INFORMATION_SCHEMA.columns as t2 
			WHERE t2.COLUMN_NAME=t1.COLUMN_NAME AND t2.TABLE_NAME=t1.TABLE_NAME AND t2.TABLE_SCHEMA=t1.TABLE_SCHEMA
		) AND 
		TABLE_SCHEMA+''.''+TABLE_NAME IN 
		(
			SELECT TABLE_SCHEMA+''.''+TABLE_NAME 
			FROM 
				'+@db2+'.INFORMATION_SCHEMA.TABLES
		)
		ORDER BY TABLE_NAME,t1.ORDINAL_POSITION';
	
	insert @columns
	exec sp_executesql @q;

	if((select count(*) from @columns)=0)
		print '-- NO ADDED COLUMNS';
	else
		print '-- ADDED COLUMNS :';

	set @i=1;
	while(@i<=(select count(*) from @columns))
	begin
		select @out='ALTER TABLE '+@db2+'.'+TABLE_NAME+' ADD ['+COLUMN_NAME+'] '+DATA_TYPE+
			(
				CASE
					WHEN DATA_TYPE='decimal' THEN '('+CONVERT(VARCHAR,NUMERIC_PRECISION)+','+CONVERT(VARCHAR(3),NUMERIC_SCALE)+')'
					WHEN DATA_TYPE like '%varchar' THEN 
						'('+(
						CASE 
							WHEN (CHARACTER_MAXIMUM_LENGTH=-1) THEN 'max' 
							ELSE CONVERT(VARCHAR,CHARACTER_MAXIMUM_LENGTH) 
						END)+
						')'
				
				ELSE '' END
			)
			+
			(
				CASE WHEN (IS_NULLABLE='yes') THEN ' NULL' ELSE ' NOT NULL' END
			)+
			';'
		from (
			select 
				row_number() over (order by TABLE_NAME,ORDINAL_POSITION) i,
				*
			from @columns 
		) RWS
		where i=@i;
		set @i=@i+1;

		print @out;
		if(@execute=1)
		begin
			exec sp_executesql @out;
			print ' --EXECUTED';
		end;

	
	end;

	print '';

	select @q='
		SELECT 
			TABLE_SCHEMA+''.''+TABLE_NAME TABLE_NAME,
			COLUMN_NAME,DATA_TYPE,
			NUMERIC_PRECISION,
			NUMERIC_SCALE,
			CHARACTER_MAXIMUM_LENGTH,
			ORDINAL_POSITION,
			IS_NULLABLE
			
		FROM 
			'+@db1+'.INFORMATION_SCHEMA.columns as t1
		WHERE 
		NOT EXISTS
		(
			SELECT *
			FROM 
				'+@db2+'.INFORMATION_SCHEMA.columns as t2 
			WHERE 
			t2.COLUMN_NAME=t1.COLUMN_NAME AND t2.TABLE_NAME=t1.TABLE_NAME AND t2.TABLE_SCHEMA=t1.TABLE_SCHEMA AND
			t2.DATA_TYPE=t1.DATA_TYPE AND t2.IS_NULLABLE=t1.IS_NULLABLE AND
	
			(t2.NUMERIC_PRECISION=t1.NUMERIC_PRECISION OR t2.NUMERIC_PRECISION IS NULL) AND 
			(t2.NUMERIC_SCALE=t1.NUMERIC_SCALE OR t2.NUMERIC_SCALE IS NULL) AND
			(t2.CHARACTER_MAXIMUM_LENGTH=t1.CHARACTER_MAXIMUM_LENGTH OR t2.CHARACTER_MAXIMUM_LENGTH IS NULL)
	
		) AND 
		TABLE_SCHEMA+''.''+TABLE_NAME+''.''+COLUMN_NAME IN 
		(
			SELECT TABLE_SCHEMA+''.''+TABLE_NAME+''.''+COLUMN_NAME 
			FROM 
				'+@db2+'.INFORMATION_SCHEMA.COLUMNS
		)
		ORDER BY TABLE_NAME';

	delete from @columns;
	insert @columns
	exec sp_executesql @q;

	if((select count(*) from @columns)=0)
		print '-- NO MODIFIED COLUMNS';
	else
		print '-- MODIFIED COLUMNS :';

	set @i=1;
	while(@i<=(select count(*) from @columns))
	begin
		select @out='ALTER TABLE '+@db2+'.'+TABLE_NAME+' ALTER COLUMN ['+COLUMN_NAME+'] '+DATA_TYPE+
			(
				CASE
					WHEN DATA_TYPE='decimal' THEN '('+CONVERT(VARCHAR,NUMERIC_PRECISION)+','+CONVERT(VARCHAR(3),NUMERIC_SCALE)+')'
					WHEN DATA_TYPE like '%varchar' THEN 
						'('+(
						CASE 
							WHEN (CHARACTER_MAXIMUM_LENGTH=-1) THEN 'max' 
							ELSE CONVERT(VARCHAR,CHARACTER_MAXIMUM_LENGTH) 
						END)+
						')'
				
				ELSE '' END
			)
			+
			(
				CASE WHEN (IS_NULLABLE='yes') THEN ' NULL' ELSE ' NOT NULL' END
			)+
			';'
		from (
			select 
				row_number() over (order by TABLE_NAME,COLUMN_NAME) i,
				*
			from @columns 
		) RWS
		where i=@i;
		set @i=@i+1;

		print @out;
		if(@execute=1)
		begin
			exec sp_executesql @out;
			print ' --EXECUTED';
		end;
	end;

	print '';

	select @q='
		SELECT 
			Name,
			ChildTable,
			ChildColumn,
			ParentTable,
			ParentColumn,
			UpdateRule,
			DeleteRule,
			null
		FROM(
			SELECT 
				t1.CONSTRAINT_NAME as Name,
				t1.TABLE_SCHEMA+''.''+t1.TABLE_NAME as ChildTable,
				t3.COLUMN_NAME as ChildColumn,
				t5.TABLE_SCHEMA+''.''+t5.TABLE_NAME as ParentTable,
				t5.COLUMN_NAME as ParentColumn,
				t4.UPDATE_RULE as UpdateRule,
				t4.DELETE_RULE as DeleteRule
			FROM 
				'+@db1+'.INFORMATION_SCHEMA.TABLE_CONSTRAINTS as t1
				JOIN '+@db1+'.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t3 ON t1.CONSTRAINT_NAME=t3.CONSTRAINT_NAME and t1.CONSTRAINT_SCHEMA=t3.CONSTRAINT_SCHEMA
				JOIN '+@db1+'.INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as t4 ON t1.CONSTRAINT_NAME=t4.CONSTRAINT_NAME and t1.CONSTRAINT_SCHEMA=t4.CONSTRAINT_SCHEMA
				JOIN '+@db1+'.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t5 ON t4.UNIQUE_CONSTRAINT_NAME=t5.CONSTRAINT_NAME and t4.CONSTRAINT_SCHEMA=t5.CONSTRAINT_SCHEMA

			WHERE NOT EXISTS
			(
				SELECT 1
				FROM '+@db2+'.INFORMATION_SCHEMA.TABLE_CONSTRAINTS as t11
					JOIN '+@db2+'.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t31 ON t11.CONSTRAINT_NAME=t31.CONSTRAINT_NAME and t11.CONSTRAINT_SCHEMA=t31.CONSTRAINT_SCHEMA
					JOIN '+@db2+'.INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as t41 ON t11.CONSTRAINT_NAME=t41.CONSTRAINT_NAME and t11.CONSTRAINT_SCHEMA=t41.CONSTRAINT_SCHEMA
					JOIN '+@db2+'.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t51 ON t41.UNIQUE_CONSTRAINT_NAME=t51.CONSTRAINT_NAME and t41.CONSTRAINT_SCHEMA=t51.CONSTRAINT_SCHEMA
				WHERE 
					t1.TABLE_SCHEMA+''.''+t1.TABLE_NAME=t11.TABLE_SCHEMA+''.''+t11.TABLE_NAME AND
					t5.TABLE_SCHEMA+''.''+t5.TABLE_NAME=t51.TABLE_SCHEMA+''.''+t51.TABLE_NAME AND
					t3.COLUMN_NAME=t31.COLUMN_NAME AND
					t5.COLUMN_NAME=t51.COLUMN_NAME
					
			)
			AND t1.TABLE_NAME IN
			(
				SELECT TABLE_NAME
				FROM 
					'+@db2+'.INFORMATION_SCHEMA.TABLES
			)
			AND t5.TABLE_NAME IN
			(
				SELECT TABLE_NAME
				FROM 
					'+@db2+'.INFORMATION_SCHEMA.TABLES
			)
		) as tx;'

	insert @constraints
	exec sp_executesql @q;

	if((select count(*) from @constraints)=0)
		print '-- NO ADDED CONSTRAINTS';
	else
		print '-- ADDED CONSTRAINTS :';

	set @i=1;
	while(@i<=(select count(*) from @constraints))
	begin
		select @out='ALTER TABLE '+@db2+'.'+ChildTable+' ADD CONSTRAINT '+Name+' FOREIGN KEY ('+ChildColumn+') REFERENCES '+ParentTable+' ('+ParentColumn+') 
		ON UPDATE '+UpdateRule+' ON DELETE '+DeleteRule+';'
		from (
			select 
				row_number() over (order by ChildTable,ChildColumn) i,
				*
			from @constraints 
		) RWS
		where i=@i;
		set @i=@i+1;

		print @out;
		if(@execute=1)
		begin
			exec sp_executesql @out;
			print ' --EXECUTED';
		end;
	end;

	print '';

	delete from @constraints;
	select @q='SELECT 
				t1.CONSTRAINT_NAME as Name,
				t1.TABLE_SCHEMA+''.''+t1.TABLE_NAME as ChildTable,
				t3.COLUMN_NAME as ChildColumn,
				t5.TABLE_SCHEMA+''.''+t5.TABLE_NAME as ParentTable,
				t5.COLUMN_NAME as ParentColumn,
				t4.UPDATE_RULE as UpdateRule,
				t4.DELETE_RULE as DeleteRule,
				sc2.Name as OldKey
			FROM 
				'+@db1+'.INFORMATION_SCHEMA.TABLE_CONSTRAINTS as t1
				JOIN '+@db1+'.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t3 ON t1.CONSTRAINT_NAME=t3.CONSTRAINT_NAME and t1.CONSTRAINT_SCHEMA=t3.CONSTRAINT_SCHEMA
				JOIN '+@db1+'.INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as t4 ON t1.CONSTRAINT_NAME=t4.CONSTRAINT_NAME and t1.CONSTRAINT_SCHEMA=t4.CONSTRAINT_SCHEMA
				JOIN '+@db1+'.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t5 ON t4.UNIQUE_CONSTRAINT_NAME=t5.CONSTRAINT_NAME and t4.CONSTRAINT_SCHEMA=t5.CONSTRAINT_SCHEMA

				JOIN
				(
					SELECT 
						t11.CONSTRAINT_NAME as Name,
						t11.TABLE_SCHEMA+''.''+t11.TABLE_NAME as ChildTable,
						t31.COLUMN_NAME as ChildColumn,
						t51.TABLE_SCHEMA+''.''+t51.TABLE_NAME as ParentTable,
						t51.COLUMN_NAME as ParentColumn,
						t41.UPDATE_RULE as UpdateRule,
						t41.DELETE_RULE as DeleteRule
					FROM '+@db2+'.INFORMATION_SCHEMA.TABLE_CONSTRAINTS as t11
						JOIN '+@db2+'.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t31 ON t11.CONSTRAINT_NAME=t31.CONSTRAINT_NAME and t11.CONSTRAINT_SCHEMA=t31.CONSTRAINT_SCHEMA
						JOIN '+@db2+'.INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as t41 ON t11.CONSTRAINT_NAME=t41.CONSTRAINT_NAME and t11.CONSTRAINT_SCHEMA=t41.CONSTRAINT_SCHEMA
						JOIN '+@db2+'.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t51 ON t41.UNIQUE_CONSTRAINT_NAME=t51.CONSTRAINT_NAME and t41.CONSTRAINT_SCHEMA=t51.CONSTRAINT_SCHEMA
				) sc2 ON	t1.TABLE_SCHEMA+''.''+t1.TABLE_NAME=sc2.ChildTable AND
							t5.TABLE_SCHEMA+''.''+t5.TABLE_NAME=sc2.ParentTable AND
							t3.COLUMN_NAME=sc2.ChildColumn AND
							t5.COLUMN_NAME=sc2.ParentColumn
			WHERE sc2.UpdateRule <> t4.UPDATE_RULE OR sc2.DeleteRule <> t4.DELETE_RULE AND 
			t1.TABLE_NAME IN
			(
				SELECT TABLE_NAME
				FROM 
					'+@db2+'.INFORMATION_SCHEMA.TABLES
			) AND 
			t5.TABLE_NAME IN
			(
				SELECT TABLE_NAME
				FROM 
					'+@db2+'.INFORMATION_SCHEMA.TABLES
			)';

	insert @constraints
	exec sp_executesql @q;

	if((select count(*) from @constraints)=0)
		print '-- NO MODIFIED CONSTRAINTS';
	else
		print '-- MODIFIED CONSTRAINTS :';

	set @i=1;
	while(@i<=(select count(*) from @constraints))
	begin
		select @out='
		ALTER TABLE '+@db2+'.'+ChildTable+' DROP CONSTRAINT '+OldKey+';
		ALTER TABLE '+@db2+'.'+ChildTable+' ADD CONSTRAINT '+Name+' FOREIGN KEY ('+ChildColumn+') REFERENCES '+ParentTable+' ('+ParentColumn+') 
		ON UPDATE '+UpdateRule+' ON DELETE '+DeleteRule+';'
		from (
			select 
				row_number() over (order by ChildTable,ChildColumn) i,
				*
			from @constraints 
		) RWS
		where i=@i;
		set @i=@i+1;

		print @out;
		if(@execute=1)
		begin
			exec sp_executesql @out;
			print ' --EXECUTED';
		end;
	end;

	print '';


	select @q='select 
		srcTriggers.name,
		srcTriggers.create_date as SrcCreate,
		srcTriggers.modify_date as SrcUpdate,
		CASE 
			WHEN (tarTriggers.modify_date is NULL) THEN 1 ELSE 0 END IsNew,
		''TRIGGER'' as [Type],
		Mods.[definition]
	from 
		'+@db1+'.sys.triggers srcTriggers
		join '+@db1+'.sys.objects srcTables on srcTables.object_id = srcTriggers.parent_id
		join '+@db1+'.sys.schemas srcSchemas on srcSchemas.schema_id = srcTables.schema_id
		LEFT join '+@db1+'.sys.sql_modules Mods on Mods.object_id = srcTriggers.object_id
		LEFT join(
			SELECT 
				tarSchemas.name SchemaName,
				tarTables.name TableName,
				_tarTriggers.name TriggerName,
				_tarTriggers.modify_date
			from '+@db2+'.sys.triggers _tarTriggers
				join '+@db2+'.sys.objects tarTables on tarTables.object_id=_tarTriggers.parent_id
				join '+@db2+'.sys.schemas tarSchemas on tarSchemas.schema_id=tarTables.schema_id
		) tarTriggers on 
			srcTables.name = tarTriggers.TableName AND
			srcSchemas.name = tarTriggers.SchemaName AND
			srcTriggers.name = tarTriggers.TriggerName
	where
		tarTriggers.modify_date<srcTriggers.modify_date OR
		tarTriggers.modify_date IS NULL;';

	insert @other
	exec sp_executesql @q;

	select @q='
		select 
			Src.ROUTINE_SCHEMA+''.''+Src.SPECIFIC_NAME as name,
			Src.CREATED as SrcCreate,
			Src.LAST_ALTERED as SrcUpdate,
			CASE 
				WHEN (Tar.CREATED is NULL) THEN 1 ELSE 0 END IsNew,
			Src.ROUTINE_TYPE as [Type],
			Src.ROUTINE_DEFINITION
		from 
			'+@db1+'.INFORMATION_SCHEMA.ROUTINES Src
			LEFT join '+@db2+'.INFORMATION_SCHEMA.ROUTINES Tar on 
				Src.SPECIFIC_NAME=Tar.SPECIFIC_NAME AND
				Src.ROUTINE_SCHEMA=Tar.ROUTINE_SCHEMA
		where
			Tar.LAST_ALTERED<Src.LAST_ALTERED OR
			Tar.CREATED IS NULL
		order by Src.ROUTINE_TYPE;';

	insert @other
	exec sp_executesql @q;

	select @q='
		select 
			Src.name,
			Src.create_date as SrcCreate,
			Src.modify_date as SrcUpdate,
			CASE 
				WHEN (Tar.modify_date is NULL) THEN 1 ELSE 0 END IsNew,
			''SEQUENCE'' as [Type],
			Mods.[definition]
		from 
			'+@db1+'.sys.sequences Src
			LEFT join '+@db1+'.sys.sql_modules Mods on Mods.object_id=Src.object_id
			LEFT join '+@db2+'.sys.sequences Tar on Src.name=Tar.name
		where
			Tar.modify_date<Src.modify_date OR
			Tar.modify_date IS NULL;'

	insert @other
	exec sp_executesql @q;

	if((select count(*) from @other)=0)
		print '-- NO MODIFIED TRIGGERS, SEQUENCES, FUNCTIONS OR PROCEDURES';
	else
		BEGIN
			print 'GO';
			print '';
			print '-- OTHER MODIFIED DATA:';
			
		END

	set @i=1;

	while(@i<=(select count(*) from @other))
	begin
		select @out='-- '+[Type]+'  '+char(9)+char(9)+(
			CASE 
				WHEN (IsNew=1) THEN concat(' CREATED ',char(9),SrcCreate)
				ELSE concat(' MODIFIED ',char(9),SrcUpdate) 
			END)+char(9)+char(9)+name,
			@q2=CASE 
					WHEN (IsNew=0) THEN REPLACE(RWS.[Definition],'CREATE ','ALTER ')
					ELSE RWS.[Definition] 
				END
		from (
			select 
				row_number() over (order by [Type]) i,
				*
			from @other 
		) RWS

		where i=@i
		order by i;
		set @i=@i+1;

		print @out;
		print @q2;
		print 'GO'
	end;
END;