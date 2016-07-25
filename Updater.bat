Taskkill /IM AdvantageLauncher.exe /F
cd %~dp0
IF EXIST Releases Powershell.exe  -ExecutionPolicy Bypass "Remove-Item .\Releases -Force -Recurse"
mkdir Releases
Powershell.exe  -ExecutionPolicy Bypass "(New-Object System.Net.WebClient).DownloadFile('https://github.com/gigglesbw4/AdvantageLauncher/archive/master.zip"', 'Releases\currentversion.zip'); "
Powershell.exe  -ExecutionPolicy Bypass "Add-Type -AssemblyName System.IO.Compression.FileSystem; [System.IO.Compression.ZipFile]::ExtractToDirectory('Releases\currentversion.zip', 'Releases\currentversion'); "
start Powershell.exe -ExecutionPolicy Bypass "Copy-Item  -Path Releases\currentversion\AdvantageLauncher-master\* -Destination . -Recurse -force; start cmd.exe ' /c echo Updating is complete & pause '"

