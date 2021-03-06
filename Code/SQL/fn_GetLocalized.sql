CREATE FUNCTION [dbo].[GetLocalized]
(
	-- Add the parameters for the function here
	@EntityType  varchar(50),
	@EntityId bigint,
	@LocaleId int,
	@PropertyName varchar(50),
	@default nvarchar(max)
)
RETURNS nvarchar(max)
AS
BEGIN
	DECLARE @ret nvarchar(max)=NULL;

	select 
		TOP 1 @ret=Value
	from [dbo].[Localizables] 
	where 
		EntityId=@EntityId AND 
		LocaleId=@LocaleId AND 
		EntityType=@EntityType AND 
		ColumnName=@PropertyName;

	if @ret IS null
		return @default;
	
	return @ret;
END
