@ECHO OFF
SETLOCAL ENABLEDELAYEDEXPANSION

REM Move directory

CD /d "%~dp0"
CD ..

SET DEST_PATH="%LOCALAPPDATA%Low\sokpop\Stacklands\Mods"

SET P_REBUILD=0
SET P_CONFIG="Release"
SET P_PROJNAME=""

:_PARSE_
REM Drop first argument
SHIFT

IF '%~0'=='' GOTO :_MAIN_

IF /I '%~0'=='/q' (
    SET P_REBUILD=1
    GOTO :_PARSE_
)
IF /I '%~0'=='/d' (
    SET "P_CONFIG=Debug"'
    GOTO :_PARSE_
)
IF '!P_PROJNAME!' NEQ '' (
    SET P_PROJNAME=%~0
    GOTO :_PARSE_
)

CALL :_PRINT_ "  Invalid Arguments! '%~0'"
GOTO :EOF




:_MAIN_
CALL :_TITLE_ "STEP 1. Prepare for build"

CALL :_MD_ "%CD%\Build\Artifacts"

CALL :_MD_ "%DEST_PATH%"

CALL :_PRINT_ "  - OK"

ECHO.
ECHO.
CALL :_TITLE_ "STEP 2. Build Projects"

REM Query directories

FOR /f "tokens=*" %%D IN ('DIR "%CD%\Source" /AD /B') DO CALL :_BUILD_ "%%D"

ECHO.
CALL :_TITLE_ "STEP 3. Complete"
CALL :_PRINT_ "Done!"

PAUSE
EXIT /b 0
GOTO :EOF





:_BUILD_ arg1_name
REM If, '0SuperComicLib.Stacklands', skip
IF /I "%~1"=="0SuperComicLib.Stacklands" GOTO :EOF

IF '!P_PROJNAME!' NEQ '' IF '!P_PROJNAME!' NEQ '%~1' (
    CALL :_PRINT_ "  - Skip '%~1'"
    GOTO :EOF
)

CALL :_PRINT_ "  - Build '%~1'"

CALL :_TRY_CLEAN_ %~1

REM build...

dotnet restore "%CD%\Source\%~1\%~1.csproj"
IF !ERRORLEVEL! NEQ 0 GOTO :_ERROR_

dotnet build "%CD%\Source\%~1\%~1.csproj" ^
--no-restore ^
--configuration !P_CONFIG! ^
-nowarn:1701,1702,IDE1006,IDE0290,CS1591 ^
-p:Unsafe=true ^
-o "%CD%\Build\Artifacts\%~1"
IF !ERRORLEVEL! NEQ 0 GOTO :_ERROR_

REM copy resources...

IF EXIST "%CD%\Source\%~1\Resources\" (
    XCOPY /Y /S "%CD%\Source\%~1\Resources\*" "%CD%\Build\Artifacts\%~1\"
)

REM Even if there are problems during this process, copy to Artifacts for easy manual intervention, and then copy back to the Mods folder.

XCOPY /Y /S "%CD%\Build\Artifacts\%~1\*" "%DEST_PATH%\%~1\"

REM Copy LICENSE

IF NOT EXIST "%DEST_PATH%\%~1\LICENSE.txt" (
    XCOPY /Y "%CD%\LICENSE.txt" "%DEST_PATH%\"
)

GOTO :EOF

REM ====== SUB-ROUTINE ====== 
REM VOID _PRINT_(str)
REM =========================
:_PRINT_ arg1_str
SET "TEMP_STR=%~1"
ECHO !TEMP_STR!
GOTO :EOF

REM ====== SUB-ROUTINE ====== 
REM VOID _RD_(path)
REM     Remove Directory
REM =========================
:_RD_ arg1_path
SET "TEMP_STR=%~1"
IF EXIST "!TEMP_STR!\" ( 
    RD /S /Q "!TEMP_STR!"
)
GOTO :EOF

REM ====== SUB-ROUTINE ====== 
REM VOID _MD_(path)
REM     Make Directory
REM =========================
:_MD_ arg1_path
SET "TEMP_STR=%~1"
IF NOT EXIST "!TEMP_STR!\" ( 
    MD "!TEMP_STR!"
)
GOTO :EOF

REM ====== SUB-ROUTINE ====== 
REM VOID _TRY_CLEAN(name)
REM =========================
:_TRY_CLEAN_ arg1_name
IF !P_REBUILD!==1 (
    CALL :_PRINT_ "  CLEAN-UP: %1"
    
    REM clean-up, rebuild
    
    CALL :_RD_ "%CD%\Source\%1\bin"
    CALL :_RD_ "%CD%\Source\%1\obj"
    
    CALL :_RD_ "%CD%\Build\Artifacts\%1"
       
    CALL :_RD_ "%DEST_PATH%\%1"
)
CALL :_MD_ "%CD%\Build\Artifacts\%1"
CALL :_MD_ "%DEST_PATH%\%1"
GOTO :EOF

REM ====== SUB-ROUTINE ====== 
REM VOID _TITLE_(str)
REM =========================
:_TITLE_ arg1_str
SET "TEMP_STR=%~1"
TITLE !TEMP_STR!
GOTO :EOF

REM ====== SUB-ROUTINE ====== 
REM VOID _ERROR_()
REM =========================
:_ERROR_
EXIT /B !ERRORLEVEL!
GOTO :EOF
