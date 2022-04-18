:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

echo [1m[36m^> XML Documentation for GDX[0m

:: Build content
"%MSBUILD%"^
 /p:Configuration=Debug^
 /p:GenerateDocumentation=true^
 /p:WarningLevel=0^
 /p:DefineConstants=%DEFINES%^
 /p:DocumentationFile="%~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\GDX.xml"^
  %~dp0..\..\..\Projects\GDX_Development\GDX.csproj

%~dp0..\..\tools\XmlDocForBolt\XmlDocForBolt.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\GDX.xml

echo [1m[36m^> XML Documentation for GDX.Editor[0m

:: Build XML for editor
"%MSBUILD%"^
 /p:Configuration=Debug^
 /p:GenerateDocumentation=true^
 /p:WarningLevel=0^
 /p:DefineConstants=%DEFINES%^
 /p:DocumentationFile="%~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\GDX.Editor.xml"^
  %~dp0..\..\..\Projects\GDX_Development\GDX.Editor.csproj

  %~dp0..\..\tools\XmlDocForBolt\XmlDocForBolt.exe %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\GDX.Editor.xml