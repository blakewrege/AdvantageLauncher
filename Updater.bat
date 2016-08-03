set arg1=%1
cd %~dp0
IF EXIST Releases Powershell.exe  -ExecutionPolicy Bypass "Remove-Item .\Releases -Force -Recurse"
mkdir Releases
Powershell.exe  -ExecutionPolicy Bypass "(New-Object System.Net.WebClient).DownloadFile('https://github.com/gigglesbw4/AdvantageLauncher/archive/master.zip"', 'C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\Releases\currentversion.zip'); "
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
if "%arg1%"=="yes" (
GOTO YES
 )
if %arg1 NEQ "yes" (
GOTO NO
)
:YES
Taskkill /IM AdvantageLauncher.exe /F
start /MIN Powershell.exe -ExecutionPolicy Bypass -windowstyle hidden "Copy-Item  -Path 'C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\Releases\currentversion\AdvantageLauncher-master\*' -Destination 'C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\' -Recurse -force; start cmd.exe ' /c echo Updating is complete & pause '"
exit 0
:NO
start /MIN Powershell.exe -ExecutionPolicy Bypass -windowstyle hidden "Copy-Item  -Path 'C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\Releases\currentversion\AdvantageLauncher-master\*' -Destination 'C:\Program Files (x86)\Deltek\Advantage\9.1\AdvantageLauncher\' -Recurse -force;"
exit 0