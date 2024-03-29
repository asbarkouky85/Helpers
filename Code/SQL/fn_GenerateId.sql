CREATE FUNCTION [dbo].[GenerateId]
(
	@time Datetime,
	@rowNum int
)
RETURNS bigint
AS
BEGIN
	DECLARE @id bigint;
	DECLARE @idString varchar(15);
	DECLARE @secs int;
	select @secs=(DATEPART(hour,@time)*3600)+(DATEPART(MINUTE,@time)*60)+DATEPART(SECOND,@time);
	if(@rowNum>999)
	SET @secs=@secs+1;
	select @idString=CONCAT(
		(YEAR(@time)-2000),
		RIGHT('000'+CAST(DATEPART(dayofyear,@time) as varchar(5)),3),
		RIGHT('00000'+CAST(@secs as varchar(5)),5),
		RIGHT('000'+CAST(@rowNum as varchar(5)),3)
	)
	RETURN CONVERT(bigint,@idString);
END