:: Turn off output to CLI
@echo off
echo [1;4;96mClean/Serve Documentation[0m

:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.
call %~dp0steps\step-set.bat
call %~dp0steps\step-clean.bat
call %~dp0steps\step-changelog.bat
call %~dp0steps\step-securitypolicy.bat
call %~dp0steps\step-codeofconduct.bat
call %~dp0steps\step-license.bat
call %~dp0steps\step-footer.bat
call %~dp0steps\step-serve.bat