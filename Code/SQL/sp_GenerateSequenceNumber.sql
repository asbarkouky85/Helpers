CREATE procedure [dbo].[GenerateSequenceNumber](
	@table varchar(255),
	@column varchar(255),
	@sPattern varchar(255),
	@minLength int,
	@param varchar(255)='',
	@out varchar(255) OUTPUT
)
AS
BEGIN
	
	declare @max bigint=0;
	declare @pattern varchar(255);
	declare @date datetime =GETDATE();
	declare @day varchar(2)=(select dbo.NumberString(DAY(@date),2));
	declare @dayOfYear varchar(3)=(select dbo.NumberString(DATEPART(dayofyear,@date),3));
	declare @year4 varchar(4)=convert(varchar(4),YEAR(@date));
	declare @year2 varchar(2)=(select RIGHT(@year4,2));
	declare @mon varchar(2)=(select dbo.NumberString(MONTH(@date),2));
	declare @client varchar(3)=(select dbo.NumberString(5,2))

	if(@param is NULL)
		set @param='';

	select @pattern=replace(@sPattern,'{YEAR4}',@year4);
	select @pattern=replace(@sPattern,'{YEAR2}',@year2);
	select @pattern=replace(@pattern,'{MONTH}',@mon);
	select @pattern=replace(@pattern,'{DAYOFYEAR}',@dayOfYear);
	select @pattern=replace(@pattern,'{DAY}',@day);
	select @pattern=replace(@pattern,'{PARAM}',@param);

	declare @q nvarchar(max)=N'
	select 
		@max= MAX(convert(bigint, replace('+@column+','''+@pattern+''',''''))) 
	from 
		'+@table+' where '+@column+' like '''+@pattern+'[0123456789]%''';
	
	exec sp_executesql @q,N'@max bigint OUTPUT',@max OUTPUT;
	
	if(@max is null)
		select @max=0;
	select @out=concat( @pattern,dbo.NumberString(@max+1,@minLength));
	--print @out;
	
END;
GO