; -- BetterFormat Installer Script --

[Setup]
AppName=BetterFormat
AppVersion=1.0
AppPublisher=Cline
DefaultDirName={autopf}\BetterFormat
DefaultGroupName=BetterFormat
OutputBaseFilename=BetterFormat-Setup
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin

[Files]
; IMPORTANT: Change the source path below to your actual 'publish' directory path
Source: "Z:\Project\BetterFormat\BetterFormat\bin\x64\Release\net8.0-windows10.0.19041.0\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Registry]
Root: HKCR; Subkey: "Drive\shell\BetterFormat"; ValueType: string; ValueName: ""; ValueData: "BetterFormat"; Flags: uninsdeletekey
Root: HKCR; Subkey: "Drive\shell\BetterFormat"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\BetterFormat.exe"",0"
Root: HKCR; Subkey: "Drive\shell\BetterFormat\command"; ValueType: string; ValueName: ""; ValueData: """{app}\BetterFormat.exe"" ""%1"""

[Icons]
Name: "{group}\BetterFormat"; Filename: "{app}\BetterFormat.exe"
Name: "{group}\{cm:UninstallProgram,BetterFormat}"; Filename: "{uninstallexe}"
