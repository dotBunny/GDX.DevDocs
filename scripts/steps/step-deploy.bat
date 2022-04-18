echo [1m[36m^> Deploy Files[0m

robocopy /E /PURGE %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\_site %~dp0..\..\..\Documentation\docs /XF CNAME