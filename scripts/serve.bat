:: Working Folder
set ScriptDirectory=%~dp0

:: Define path to docfx executable.
set DocFXExecutable=%ScriptDirectory%..\..\Tools\docfx\docfx.exe

:: Define absolute path to project folder
set ProjectFolder=%ScriptDirectory%..\docfx_project

:: Generate & Serve
%DocFXExecutable% %ProjectFolder%\docfx.json --serve