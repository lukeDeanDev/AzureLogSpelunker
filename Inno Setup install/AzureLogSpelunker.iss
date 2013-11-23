;Inno Setup script for Inno Setup.  http://www.jrsoftware.org or http://www.innosetup.com

#define MyAppName "Azure Log Spelunker"
#define MyAppShortName "AzureLogSpelunker"
#define MyAppPublisher "Luke Dean"
#define MyAppURL "https://github.com/SageLukeDean/AzureLogSpelunker/"
#define MyAppExeName "AzureLogSpelunker.exe"
#define MySourceDir "..\AzureLogSpelunker\bin\Release\"
#define MyLicenseFile "..\LICENSE"
#define MyAppVersion GetFileVersion(MySourceDir + MyAppExeName)

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{952F94F9-5FAC-4BCC-B3DF-8B435562F00C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={userappdata}\{#MyAppShortName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile={#MyLicenseFile}
OutputDir=..\Releases
OutputBaseFilename={#MyAppName} Setup-{#MyAppVersion}
Compression=lzma
SolidCompression=yes
PrivilegesRequired=lowest
;SetupIconFile=
;WizardSmallImageFile=
;WizardImageFile=
;WizardImageStretch=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
;Source: "{#MySourceDir}*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MySourceDir}AzureLogSpelunker.exe"; DestDir: "{app}"
Source: "{#MySourceDir}AzureLogSpelunker.exe.config"; DestDir: "{app}"
Source: "{#MySourceDir}x64\SQLite.Interop.dll"; DestDir: "{app}\x64"
Source: "{#MySourceDir}x86\SQLite.Interop.dll"; DestDir: "{app}\x86"
Source: "{#MySourceDir}Microsoft.Data.Edm.dll"; DestDir: "{app}"
;Source: "{#MySourceDir}Microsoft.Data.Edm.xml"; DestDir: "{app}"
Source: "{#MySourceDir}Microsoft.Data.OData.dll"; DestDir: "{app}"
;Source: "{#MySourceDir}Microsoft.Data.OData.xml"; DestDir: "{app}"
Source: "{#MySourceDir}Microsoft.WindowsAzure.Storage.dll"; DestDir: "{app}"
;Source: "{#MySourceDir}Microsoft.WindowsAzure.Storage.xml"; DestDir: "{app}"
Source: "{#MySourceDir}System.Data.SQLite.dll"; DestDir: "{app}"
Source: "{#MySourceDir}System.Data.SQLite.Linq.dll"; DestDir: "{app}"
;Source: "{#MySourceDir}System.Data.SQLite.xml"; DestDir: "{app}"
Source: "{#MySourceDir}System.Spatial.dll"; DestDir: "{app}"
;Source: "{#MySourceDir}System.Spatial.xml"; DestDir: "{app}"
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
