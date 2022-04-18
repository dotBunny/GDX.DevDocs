echo [1m[36m^> Setup Environment[0m

:: Setup Environment
set RELEASE=""

:: Find MSBuild
"C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > msbuild.loc
set /p MSBUILD= < msbuild.loc
del msbuild.loc
echo Using MSBuild found @ %MSBUILD%

:: Set ScriptDefines
set DEFINES="GDX_LICENSED;GDX_ADDRESSABLES;GDX_BURST;GDX_MATHEMATICS;GDX_PLATFORMS;GDX_VISUALSCRIPTING"