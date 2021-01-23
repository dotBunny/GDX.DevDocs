:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

:: Copy over most recent changelog
copy /Y %~dp0..\..\..\Projects\000_Development\Assets\com.dotbunny.gdx\CHANGELOG.md %~dp0..\..\..\Projects\000_Development\Assets\com.dotbunny.gdx\.docfx\changelog.md 
