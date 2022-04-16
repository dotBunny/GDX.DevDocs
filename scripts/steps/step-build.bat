:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

:: Build content

%~dp0..\..\..\External\docfx\docfx.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\docfx.json --metadata

%~dp0..\..\tools\DocFxNestedNamespaces\DocFxNestedNamespaces.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\api\toc.yml

%~dp0..\..\..\External\docfx\docfx.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\docfx.json --build