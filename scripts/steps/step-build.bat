:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

:: Build content
echo [1m[36m^> DocFX Meta Extraction[0m
%~dp0..\..\..\External\docfx\docfx.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\docfx.json --metadata

echo [1m[36m^> Update API TOC[0m
%~dp0..\..\tools\DocFxNestedNamespaces\DocFxNestedNamespaces.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\api\toc.yml

echo [1m[36m^> DocFX Build Documentation[0m
%~dp0..\..\..\External\docfx\docfx.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\docfx.json --build