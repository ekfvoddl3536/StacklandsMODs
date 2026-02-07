@ECHO OFF
SETLOCAL ENABLEDELAYEDEXPANSION

SET "CURRENT_DIR=%~dp0"

REM Create gamedir symbolic link
SET "LINK_NAME=%CURRENT_DIR%.gamedir"

IF NOT EXIST "%LINK_NAME%" (
    ECHO INFO: A path is required to create a symbolic link to the game's installed directory.
    ECHO.
    
    SET /P GAME_ROOT_DIR="Enter the path to the game root directory: "
    
    REM Remove trailing backslash
    IF "%GAME_ROOT_DIR:~-1%"=="\" (
        SET "GAME_ROOT_DIR=%GAME_ROOT_DIR:~0,-1%"
    )
    
    SET "TARGET_PATH=%GAME_ROOT_DIR%\Stacklands_Data\Managed"
    if NOT EXIST "%TARGET_PATH%\" (
        ECHO "ERROR: Path not found: '%TARGET_PATH%'"
        PAUSE
        EXIT /b 0
        goto :EOF
    )
    
    ECHO Creating '.gamedir/' symbolic link...
    MKLINK /J "%LINK_NAME%" "%TARGET_PATH%"
    
    if !ERRORLEVEL! NEQ 0 (
        ECHO ERROR: Fail
        PAUSE
        EXIT /b 0
        goto :EOF
    )
) ELSE (
    ECHO INFO: Found '.gamedir/'
    ECHO INFO: Skipping...
    ECHO.
)

REM Create appdir symbolic link
SET "APPDIR_LINK=%CURRENT_DIR%.appdir"

IF NOT EXIST "%APPDIR_LINK%" (
    ECHO Creating '.appdir/' symbolic link...
    MKLINK /J "%APPDIR_LINK%" "%LOCALAPPDATA%Low\sokpop\Stacklands"
    
    if !ERRORLEVEL! NEQ 0 (
        ECHO ERROR: Fail
        PAUSE
        EXIT /b 0
        goto :EOF
    )
) ELSE (
    ECHO INFO: Found '.appdir/'
    ECHO INFO: Skipping...
    ECHO.
)

ECHO INFO: SUCCESS
PAUSE