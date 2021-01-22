:: Working Folder
set ScriptDirectory=%~dp0

:: Define path to docfx executable.
set DocFXExecutable=%ScriptDirectory%..\..\Tools\docfx\docfx.exe

:: Define absolute path to project folder
set ProjectFolder=%ScriptDirectory%..\docfx_project

:: Force full regeneration
rmdir %ProjectFolder%\obj /s /q
rmdir %ProjectFolder%\_site /s /q

:: Generate & Serve
%DocFXExecutable% %ProjectFolder%\docfx.json --serve