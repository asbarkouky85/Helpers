create function [dbo].[CompareVersion](
	@v1 varchar(100),
	@v2 varchar(100)
)
returns int
AS
begin
declare @result int;

    select @result=case 
    when CONVERT(int, LEFT(@v1, CHARINDEX('.', @v1)-1)) < CONVERT(int, LEFT(@v2, CHARINDEX('.', @v2)-1)) then 2
    when CONVERT(int, LEFT(@v1, CHARINDEX('.', @v1)-1)) > CONVERT(int, LEFT(@v2, CHARINDEX('.', @v2)-1)) then 1
    when CONVERT(int, substring(@v1, CHARINDEX('.', @v1)+1, LEN(@v1))) < CONVERT(int, substring(@v2, CHARINDEX('.', @v2)+1, LEN(@v1))) then 2
    when CONVERT(int, substring(@v1, CHARINDEX('.', @v1)+1, LEN(@v1))) > CONVERT(int, substring(@v2, CHARINDEX('.', @v2)+1, LEN(@v1))) then 1
    else 0 end;
return @result
end;
GO


