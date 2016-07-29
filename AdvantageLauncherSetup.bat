::Author: Blake Wrege
cd %~dp0
if not exist "C:\Program Files (x86)\Deltek\Advantage\9.1\Advantag.ico" (
   echo does not exist
   start explorer "\\teifs2\_Software Installation Points\Computer Software\Deltek Advantage 9.1"
   sleep 1
   start cmd.exe " /c echo PLEASE INSTALL ADVANTAGE & pause "
   @call "\\teifs2\IT\Michigan Office\Batch Files\pcinfo.bat"
   exit 0
)
IF EXIST "C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher" exit 0
Taskkill /IM AdvantageLauncher.exe /F
mkdir "C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher"
cd "C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher"
IF EXIST "C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\Releases" Powershell.exe  -ExecutionPolicy Bypass "Remove-Item .\Releases -Force -Recurse"
mkdir "Releases"
Powershell.exe  -ExecutionPolicy Bypass "(New-Object System.Net.WebClient).DownloadFile('https://github.com/gigglesbw4/AdvantageLauncher/archive/master.zip"', 'Releases\currentversion.zip'); "
::Create unzip
set SOURCEDIR="C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\Releases\currentversion.zip"
set OUTPUTDIR="C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\Releases\currentversion"
echo ZipFile=%SOURCEDIR% > _unzip.vbs
echo ExtractTo=%OUTPUTDIR% >> _unzip.vbs
echo Set fso = CreateObject("Scripting.FileSystemObject") >> _unzip.vbs
echo fso.CreateFolder(ExtractTo) >> _unzip.vbs
echo set objShell = CreateObject("Shell.Application")	>> _unzip.vbs
echo set FilesInZip=objShell.NameSpace(ZipFile).items	>> _unzip.vbs
echo objShell.NameSpace(ExtractTo).CopyHere(FilesInZip)	>> _unzip.vbs
echo Set fso = Nothing	>> _unzip.vbs
echo Set objShell = Nothing	>> _unzip.vbs
CScript  _unzip.vbs
del _unzip.vbs
::Finished unzip
copy "C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\Releases\currentversion\AdvantageLauncher-master\AdvantageLauncher - Shortcut.lnk" "%USERPROFILE%/Desktop/"
del "C:\Users\Public\Desktop\Timekeeper.lnk"
del "C:\Users\Public\Desktop\Expensekeeper.lnk"
del "C:\Users\Public\Desktop\Advantage.lnk"
start Powershell.exe -ExecutionPolicy Bypass "Copy-Item  -Path Releases\currentversion\AdvantageLauncher-master\* -Destination . -Recurse -force; start cmd.exe ' /c echo INSTALLATION COMPLETE YOU MAY NOW DELETE THIS SCRIPT & pause '"
exit 0