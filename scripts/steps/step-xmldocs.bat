:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

:: Build content
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"^
 /p:Configuration=Debug^
 /p:GenerateDocumentation=true^
 /p:WarningLevel=0^
 /p:DefineConstants="GDX_ADDRESSABLES;GDX_BURST;GDX_MATHEMATICS;GDX_PLATFORMS;GDX_VISUALSCRIPTING"^
 /p:DocumentationFile="%~dp0..\..\..\Projects\000_Development\Assets\com.dotbunny.gdx\.docfx\GDX.xml"^
  %~dp0..\..\..\Projects\000_Development\GDX.csproj

%~dp0..\..\tools\XmlDocForBolt\XmlDocForBolt.exe %~dp0..\..\..\Projects\000_Development\Assets\com.dotbunny.gdx\.docfx\GDX.xml