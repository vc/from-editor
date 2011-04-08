@echo on
set FILE_VERSION=%BIN_PATH%FRom.exe
set NSIS="c:\Program Files\NSIS\makensis.exe"

echo * BIN_PATH: %BIN_PATH%
echo * DEPLOY_PATH: %DEPLOY_PATH%
echo * FILE_VERSION: %FILE_VERSION%
 
IF NOT EXIST "%FILE_VERSION%" (
echo * version file not found
GOTO :EOF
)

cscript %DEPLOY_PATH%get_version.js ^
	//Nologo ^
	%FILE_VERSION% 1>temp_version_file

echo * save version to variable Version
set /P VERSION= 0<temp_version_file
echo * delete temp_version_file
del temp_version_file
echo * Version of application is %VERSION%
set temp_var=^
	/v3 ^
	/DPRODUCT_VERSION="%VERSION%" ^
	/DSOURCE_DIR="%BIN_PATH%\" ^
	%DEPLOY_PATH%FRomEditor.nsi
echo * Running: %NSIS% %temp_var%
%NSIS% %temp_var%