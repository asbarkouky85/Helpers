@echo off
set connection_parameters=Server=.;Integrated Security=true;

:main
echo.
echo Database Models Generator for CSharp, Enter Database Information
echo ----------------------------------------------------------------

set database_name=<projectName>
set /p schema="Enter Schema : "
set /p folder_name="Enter Folder Name : "

set context_name=%folder_name%Context
if not defined schema set schema=dbo

echo.
echo Building Models Using "%connection_parameters%Database=%database_name%;" with Schema "%schema%"
echo Selected Folder : %folder_name%
echo.
dotnet ef dbcontext scaffold -f "%connection_parameters%Database=%database_name%;" Microsoft.EntityFrameworkCore.SqlServer -o %folder_name% --data-annotations -f --schema=%schema% -c "%context_name%"
echo.
echo Model Build Complete for Database %database_name%
echo.
goto main
PAUSE


