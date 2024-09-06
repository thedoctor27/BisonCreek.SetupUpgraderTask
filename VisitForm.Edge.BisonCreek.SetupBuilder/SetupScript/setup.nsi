; NSIS installer script for VisitForm Edge

!include "MUI2.nsh"
!include "nsDialogs.nsh"
!include "LogicLib.nsh"

; For environment variable code
!include "WinMessages.nsh"
!define env_hklm 'HKLM "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"'

Icon "vf.ico"
!define MUI_COMPONENTSPAGE_NODESC

!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "logo.bmp"
!define MUI_ICON "vf.ico"


!define MUI_FINISHPAGE_LINK "nsis.sourceforge.net"
!define MUI_FINISHPAGE_LINK_LOCATION "http://nsis.sourceforge.net"

Name "VisitForm Edge ${SERVICENAME} v${VERSION}"
OutFile "visitform-edge-${SERVICENAME}-${VERSION}-install.exe"

InstallDir "$PROGRAMFILES64\VisitForm\${SERVICENAME}"

Var ExistingVersion

;--------------------------------
; Installer pages
;!insertmacro MUI_PAGE_WELCOME

;Page custom DependencyPage
;!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH


;--------------------------------
; Uninstaller pages
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

;--------------------------------
; Languages
!insertmacro MUI_LANGUAGE "English"

;--------------------------------
; Installer sections

InstType "Standard"

Section "Server" SecInstall
	;SectionIn RO
	SectionIn 1 2
	SetOutPath "$INSTDIR"
	
	; Check if the application is already installed by reading the version from the registry
	ReadRegStr $ExistingVersion HKLM "Software\VisitForm\${SERVICENAME}" "Version"
  
	; If the registry key exists, inform the user and offer to upgrade or cancel installation
	StrCmp $ExistingVersion "" NotInstalled Installed

	Installed:
		Goto DoInstall

	DoInstall:
		SimpleSC::StopService "${SERVICENAME}" 1 30

		Delete "$INSTDIR\*.*"

		SetOutPath "$INSTDIR"
		; wecopy only th files notthe folders
		File /r "${PUBLISHPATH}\*.*"


		WriteRegStr HKLM "Software\VisitForm\${SERVICENAME}" "Version" "${VERSION}"

		SimpleSC::StartService "${SERVICENAME}" 30
	
		WriteUninstaller "$INSTDIR\Uninstall.exe"
		Goto EndInstall

	NotInstalled:
		; New installation (proceed with installation)
		SetOutPath "$INSTDIR"
		File /r "${PUBLISHPATH}\*"

		SetOutPath "$INSTDIR\docs"

		CreateDirectory "$INSTDIR\docs\test"

		WriteRegStr HKLM "Software\VisitForm\${SERVICENAME}" "Version" "${VERSION}"


		SimpleSC::InstallService "${SERVICENAME}" "${SERVICEDISPLAYNAME}" 16 2 "$INSTDIR\${SERVICEBIN}.exe" "" "" ""
		SimpleSC::SetServiceDescription "${SERVICENAME}" "${SERVICEDISPLAYNAME}"
		SimpleSC::SetServiceFailure "${SERVICENAME}" "0" "" "" "1" "60000" "1" "60000" "1" "60000" 
		SimpleSC::SetServiceDelayedAutoStartInfo "${SERVICENAME}" "1"
		SimpleSC::StartService "${SERVICENAME}" 30

		WriteUninstaller "$INSTDIR\Uninstall.exe"
		Goto EndInstall


EndInstall:

SectionEnd


Section "Uninstall"

	;nsExec::Exec '"$SYSDIR\cmd.exe" /c Netsh.exe advfirewall firewall delete rule "VisitFormMqtt"'
	;nsExec::Exec '"$SYSDIR\cmd.exe" /c setx -m MOSQUITTO_DIR ""'
	;nsExec::Exec '"$INSTDIR\broker\mosquitto.exe" uninstall'

	SimpleSC::StopService "${ServiceName}" 1 30
	SimpleSc::RemoveService "${ServiceName}"



	Delete "$INSTDIR\Uninstall.exe"
	RMDir /r "$INSTDIR"
	DeleteRegKey HKLM "Software\VisitForm\${SERVICENAME}"

SectionEnd

LangString DESC_SecInstall ${LANG_ENGLISH} "The main installation."
LangString DESC_SecService ${LANG_ENGLISH} "Install VisitForm Edge as a Windows service?"
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${SecInstall} $(DESC_SecInstall)
	!insertmacro MUI_DESCRIPTION_TEXT ${SecService} $(DESC_SecService)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

