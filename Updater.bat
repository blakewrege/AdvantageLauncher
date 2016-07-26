set arg1=%1
cd %~dp0
IF EXIST Releases Powershell.exe  -ExecutionPolicy Bypass "Remove-Item .\Releases -Force -Recurse"
mkdir Releases
Powershell.exe  -ExecutionPolicy Bypass "(New-Object System.Net.WebClient).DownloadFile('https://github.com/gigglesbw4/AdvantageLauncher/archive/master.zip"', 'Releases\currentversion.zip'); "
Powershell.exe  -ExecutionPolicy Bypass "Add-Type -AssemblyName System.IO.Compression.FileSystem; [System.IO.Compression.ZipFile]::ExtractToDirectory('Releases\currentversion.zip', 'Releases\currentversion'); "
Taskkill /IM AdvantageLauncher.exe /F
if "%arg1%"=="yes" (
GOTO YES
 )
if %arg1 NEQ "yes" (
GOTO NO
)
:YES
start /MIN Powershell.exe -ExecutionPolicy Bypass -windowstyle hidden "Copy-Item  -Path Releases\currentversion\AdvantageLauncher-master\* -Destination . -Recurse -force; start cmd.exe ' /c echo Updating is complete & pause '"
exit 0
:NO
start /MIN Powershell.exe -ExecutionPolicy Bypass -windowstyle hidden "Copy-Item  -Path Releases\currentversion\AdvantageLauncher-master\versioninfo.html -Destination . -Recurse -force;"
exit 0