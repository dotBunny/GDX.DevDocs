:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

echo [1m[36m^> DocFX Meta Extraction[0m
%~dp0..\..\..\External\docfx\docfx.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\docfx.json --metadata

echo [1m[36m^> Fix Namespaces[0m
%~dp0..\..\tools\DocFxNestedNamespaces\DocFxNestedNamespaces.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\api\toc.yml

echo [1m[36m^> DocFX Build/Serve Documentation[0m
%~dp0..\..\..\External\docfx\docfx.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\docfx.json --serve