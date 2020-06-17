@echo off

REM Obtained from batch call parameters
set type=%~1
set db_name=%~2
set target_folder=%~3

REM Database server credentials
set db_server=SRV-GENIAL
set db_user=app
set db_password=123456

REM Local data
set loc_db_server=.
set loc_db_user=%db_user%
set loc_db_password=%db_password%
set loc_backup_folder=C:\Work\Backups

REM Network credentials to logon on server
set net_db_server=%db_server%
set net_db_user=%db_user%
set net_db_password=%db_password%
set net_folder=\\SRV-GENIAL\Publish
set net_user=GENIAL\administrator
set net_password=Gabr1el18
set net_home=D:\Publish
set net_path_on_server=SQL Backups

REM FTP credentials
set ftp_db_server=%db_server%
set ftp_db_user=%db_user%
set ftp_db_password=%db_password%
set ftp_server=SRV-GENIAL
set ftp_user=abarkouky
set ftp_password=Gabr1el2018
set ftp_home=D:\Publish
set ftp_path_on_server=SQL Backups
set ftp_active=P

IF %type%==ftp (call :backup_ftp) ELSE ( 
	IF %type%==net (call :backup_net) ELSE (
		IF %type%==loc (call :backup_loc) ELSE echo invalid
	)
)
exit

:backup_net

if not exist m:/ (
	echo creating network drive on %net_folder% ...
	net use m: %net_folder% /U:%net_user% %net_password%
)

echo.
echo Backing up database %db_name% using NETWORK...
echo.
sqlcmd -U %net_db_user% -P %net_db_password% -S %net_db_server% -Q "BACKUP DATABASE [%db_name%] TO DISK = '%net_home%\%net_path_on_server%\%db_name%.bak' WITH FORMAT"
echo.
echo Backup success [%net_home%\%net_path_on_server%\%db_name%.bak]
copy "m:\%net_path_on_server%\%db_name%.bak" "%target_folder%\%db_name%.bak"
echo Backup copied %target_folder%\%db_name%.bak
echo.
echo -----------------------------------------------------------------------
echo.

if exist m:/ (
	echo deleting network drive on %net_folder% ...
	net use m: /delete
	echo.
)

exit /B 0

:backup_ftp

echo.
echo Backing up database %db_name% using FTP...
echo.
sqlcmd -U %ftp_db_user% -P %ftp_db_password% -S %ftp_db_server% -Q "BACKUP DATABASE [%db_name%] TO DISK = '%ftp_home%\%path_on_server%\%db_name%.bak' WITH FORMAT"
echo.
echo Backup success [%ftp_home%\%path_on_server%\%db_name%.bak]
toolset -c "ftp:%ftp_user%/%ftp_password%@%ftp_server%::%ftp_active%::/%path_on_server%/%db_name%.bak" "%target_folder%"
echo Backup copied %target_folder%\%db_name%.bak
echo.
echo -----------------------------------------------------------------------
echo.

exit /B 0

:backup_loc

echo.
echo Backing up database %db_name%  using LOCAL...
echo.
sqlcmd -U %loc_db_user% -P %loc_db_password% -S %loc_db_server% -Q "BACKUP DATABASE [%db_name%] TO DISK = '%loc_backup_folder%\%db_name%.bak' WITH FORMAT"
echo.
echo File saved to %loc_backup_folder%\%db_name%.bak
copy "%loc_backup_folder%\%db_name%.bak" "%target_folder%\%db_name%.bak"
echo File copied to %target_folder%\%db_name%.bak
echo.
echo -----------------------------------------------------------------------
echo.

exit /B 0