--Replace <TABLE> with table name

ALTER trigger [dbo].[<TABLE>_Chain_TR]
    ON [dbo].[<TABLE>]
    AFTER INSERT, UPDATE 
	
	AS
		DECLARE @i int=1;
        DECLARE @id BIGINT;
		DECLARE @name NVARCHAR(max);
        
        DECLARE @chain NVARCHAR(max);
		DECLARE @nameChain NVARCHAR(max);


		DECLARE @parentId bigint;

		while(@i<=(select count(*) from inserted))
		begin
			 SELECT 
				@id=Id,
				@name=Name,
				@parentId=ParentId
			FROM (
				select ROW_NUMBER() over (order by Id) i,*
				from inserted
			) TX
			WHERE i=@i;

			SET @i=@i+1;

			IF(@parentId IS NULL)
				SELECT 
					@chain='|'+CONVERT(NVARCHAR(25),@id)+'|',
					@nameChain=@name;
				
			ELSE
				SELECT 
					@chain=Chain+CONVERT(NVARCHAR(25),@id)+'|',
					@nameChain=NameChain+' / '+@name
				FROM [dbo].[<TABLE>] WHERE Id=@parentId;

			UPDATE [dbo].[<TABLE>] 
			SET 
				Chain=@chain,
				NameChain=@nameChain
			WHERE Id=@id;
		end
