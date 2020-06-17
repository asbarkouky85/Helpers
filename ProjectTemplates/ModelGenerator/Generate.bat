@echo off
set connection_parameters=Server=.;Integrated Security=true;

:main
echo.
echo Database Models Generator for CSharp, Enter Database Information
echo ----------------------------------------------------------------

set database_name=<projectName>

set context_name=<projectName>Context
if not defined schema set schema=dbo

echo.
echo Building Models Using "%connection_parameters%Database=%database_name%;"
echo.
dotnet ef dbcontext scaffold -f "%connection_parameters%Database=%database_name%;" Microsoft.EntityFrameworkCore.SqlServer -o Entities --data-annotations -f -c "%context_name%"
echo.
echo Model Build Complete for Database %database_name%
echo.
PAUSE


