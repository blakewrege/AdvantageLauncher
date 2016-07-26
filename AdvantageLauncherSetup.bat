Taskkill /IM AdvantageLauncher.exe /F
cd %~dp0
if not exist "C:\Program Files (x86)\Deltek\Advantage\9.1\Advantag.ico" (
   echo does not exist
   start explorer "\\teifs2\_Software Installation Points\Computer Software\Deltek Advantage 9.1"
   sleep 1
   start cmd.exe " /c echo PLEASE INSTALL ADVANTAGE & pause "
   @call "\\teifs2\IT\Michigan Office\Batch Files\pcinfo.bat"
   exit
) 
IF NOT EXIST "C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher" mkdir "C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher"
cd "C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher"
Powershell.exe  -ExecutionPolicy Bypass "Remove-Item .\Releases -Force -Recurse"
mkdir "Releases"
Powershell.exe  -ExecutionPolicy Bypass "(New-Object System.Net.WebClient).DownloadFile('https://github.com/gigglesbw4/AdvantageLauncher/archive/master.zip"', 'Releases\currentversion.zip'); "
Powershell.exe  -ExecutionPolicy Bypass "Add-Type -AssemblyName System.IO.Compression.FileSystem; [System.IO.Compression.ZipFile]::ExtractToDirectory('Releases\currentversion.zip', 'Releases\currentversion'); "
copy "C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\Releases\currentversion\AdvantageLauncher-master\AdvantageLauncher - Shortcut.lnk" "%USERPROFILE%/Desktop/"
rm "C:\Users\Public\Desktop\Timekeeper.lnk"
rm "C:\Users\Public\Desktop\Expensekeeper.lnk"
rm "C:\Users\Public\Desktop\Advantage.lnk"
start Powershell.exe -ExecutionPolicy Bypass "Copy-Item  -Path Releases\currentversion\AdvantageLauncher-master\* -Destination . -Recurse -force; start cmd.exe ' /c echo INSTALLATION COMPLETE YOU MAY NOW DELETE THIS SCRIPT & pause '"