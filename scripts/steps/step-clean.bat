:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

rmdir %~dp0..\..\..\Package\.docfx\obj /s /q
rmdir %~dp0..\..\..\Package\.docfx\_site /s /q

del /s /f /q %~dp0..\..\..\Package\.docfx\templates\gdx\partials\footer.tmpl.partial
del /s /f /q %~dp0..\..\..\Package\.docfx\changelog.md