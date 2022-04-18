:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

echo [1m[36m^> Create Code Of Conduct[0m

set TARGET=%~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\manual\contributing\code-of-conduct.md

echo --- > %TARGET%
echo _disableContribution: true >> %TARGET%
echo --- >> %TARGET%
type %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\CODE_OF_CONDUCT.md >> %TARGET%