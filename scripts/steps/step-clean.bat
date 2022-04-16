:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

rmdir %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\obj /s /q
rmdir %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\_site /s /q

del /s /f /q %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\templates\gdx\partials\footer.tmpl.partial
del /s /f /q %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\changelog.md