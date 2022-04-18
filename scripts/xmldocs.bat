:: Turn off output to CLI
@echo off
echo [1;4;96mGenerate XML Documentation[0m

:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.
call %~dp0steps\step-set.bat
call %~dp0steps\step-xmldocs.bat