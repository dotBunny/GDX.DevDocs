:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

:: Build content

%~dp0..\..\..\External\docfx\docfx.exe %~dp0..\..\..\Package\.docfx\docfx.json --metadata

%~dp0..\..\tools\DocFxNestedNamespaces\DocFxNestedNamespaces.exe %~dp0..\..\..\Package\.docfx\api\toc.yml

%~dp0..\..\..\External\docfx\docfx.exe %~dp0..\..\..\Package\.docfx\docfx.json --build