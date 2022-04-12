:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

set TARGET=%~dp0..\..\..\Package\.docfx\manual\security.md

echo --- > %TARGET%
echo _disableContribution: true >> %TARGET%
echo --- >> %TARGET%
type %~dp0..\..\..\Package\SECURITY.md >> %TARGET%