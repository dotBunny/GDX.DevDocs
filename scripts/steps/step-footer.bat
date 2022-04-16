:: These scripts are setup to be pathed to the workspace used internally for GDX development.
:: They almost certainly will not have the correct paths for anyone else.

:: Get release information
IF /i %RELEASE% == "" goto DevelopmentBuild

echo ^> Release Build
set RELEASE_LINK=https://github.com/dotBunny/GDX/releases/tag/%RELEASE%
goto EndRelease

:DevelopmentBuild
echo ^> Development Build
pushd %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\

for /f %%a in ('git rev-parse HEAD') do (
   set COMMIT_LONG=%%a
)
for /f %%a in ('git rev-parse --short HEAD') do (
   set COMMIT_SHORT=%%a
) 

set RELEASE=%COMMIT_SHORT%
set RELEASE_LINK=https://github.com/dotBunny/GDX/commit/%COMMIT_LONG%

popd

:EndRelease
echo ^> Built against %RELEASE_LINK%

:: Get Timestamp
for /f "tokens=1-7 delims=:/-, " %%i in ('echo exit^|cmd /q /k"prompt $d $t"') do (
   for /f "tokens=2-4 delims=/-,() skip=1" %%a in ('echo.^|date') do (
      set dow=%%i
      set %%a=%%j
      set %%b=%%k
      set %%c=%%l
      set hh=%%m
      set min=%%n
      set ss=%%o
   )
)

:: Make sure folder exists
md %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\templates\gdx\partials\

:: Generate file
(
echo ^<footer^>
echo  ^<div class="grad-bottom"^>^</div^>
echo  ^<div class="footer"^>
echo    ^<div class="container"^>
echo      ^<span class="pull-right"^>
echo        ^<a href="#top"^>{{__global.backToTop}}^</a^>
echo      ^</span^>
echo      {{{_appFooter}}}
echo      {{^^_appFooter}}^<span^>Built on %yy%-%mm%-%dd% against ^<a href="%RELEASE_LINK%"^>^<strong^>%RELEASE%^</strong^>^</a^>.^<br /^>Generated by ^<strong^>DocFX^</strong^>^</span^>{{/_appFooter}}
echo    ^</div^>
echo  ^</div^>
echo ^</footer^>
) > %~dp0..\..\..\Projects\GDX_Development\Packages\com.dotbunny.gdx\.docfx\templates\gdx\partials\footer.tmpl.partial