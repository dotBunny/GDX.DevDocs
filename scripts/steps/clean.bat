:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

rmdir %~dp0..\..\..\Projects\000_Development\Assets\com.dotbunny.gdx\.docfx\obj /s /q
rmdir %~dp0..\..\..\Projects\000_Development\Assets\com.dotbunny.gdx\.docfx\_site /s /q

del /s /f /q %~dp0..\..\..\Projects\000_Development\Assets\com.dotbunny.gdx\.docfx\gdx-data.yml
del /s /f /q %~dp0..\..\..\Projects\000_Development\Assets\com.dotbunny.gdx\.docfx\changelog.md