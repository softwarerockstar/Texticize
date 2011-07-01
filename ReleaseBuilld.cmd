@ECHO OFF

:::::: THESE ARE VARIABLES THAT CAN BE MODIFIED AS NEEDED ::::::
SET FRAMEWORK_VER=4.0.30319
SET BASE_FOLDER=.\SoftwareRockstar.Texticize
SET LOGFILE=ReleaseBuild.log
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

IF EXIST %LOGFILE% del %LOGFILE%

ECHO Building Texticize...
"C:\WINDOWS\Microsoft.NET\Framework\v%FRAMEWORK_VER%\msbuild.exe" "%BASE_FOLDER%\SoftwareRockstar.Texticize.csproj" /t:Build /p:Configuration=Release >> %LOGFILE%

FINDSTR /R /N /I [1-9].error %LOGFILE%
IF %ERRORLEVEL% == 0 GOTO END

ECHO Creating ZIP
zip.exe -0 -j "%BASE_FOLDER%\bin\Release\Texticize.zip" "%BASE_FOLDER%\bin\Release\SoftwareRockstar.Texticize.dll" "%BASE_FOLDER%\bin\Release\Readme.txt" >> %LOGFILE%


FINDSTR /R /N /I [1-9].error %LOGFILE%
IF %ERRORLEVEL% == 0 GOTO END

ECHO.
ECHO Build completed successfully

:END
ECHO.
ECHO.

PAUSE

