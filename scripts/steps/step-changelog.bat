:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

set TARGET=%~dp0..\..\..\Package\.docfx\changelog.md

echo --- > %TARGET%
echo _disableBreadcrumb: true >> %TARGET%
echo _disableContribution: true >> %TARGET%
echo --- >> %TARGET%
type %~dp0..\..\..\Package\CHANGELOG.md >> %TARGET%