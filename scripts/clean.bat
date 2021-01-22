:: Working Folder
set ScriptDirectory=%~dp0

:: Define absolute path to project folder
set ProjectFolder=%ScriptDirectory%..\docfx_project

:: Force full regeneration
rmdir %ProjectFolder%\obj /s /q
rmdir %ProjectFolder%\_site /s /q