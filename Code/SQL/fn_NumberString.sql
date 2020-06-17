
CREATE function [dbo].[NumberString](
	@val bigint,
	@minLength int
)
returns varchar(15)
begin
	declare @res varchar(15)=convert(varchar(15),@val);

	if(len(@val)<@minLength)
		select @res= RIGHT('00000000000000'+@res,@minLength)
	return @res;
end;
GO


