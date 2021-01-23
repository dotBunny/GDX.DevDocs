:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.
call %~dp0steps\clean.bat
call %~dp0steps\changelog.bat
call %~dp0steps\footer.bat
call %~dp0steps\build.bat