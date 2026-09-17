# Local Builds

This project builds locally from a clean git copy inside:

`C:\Usman\BuildAgents`

## How it works

1. The build script reads the current repo's `origin` remote.
2. It clones or updates a clean copy in `C:\Usman\BuildAgents`.
3. It finds the Unity editor path from `ProjectSettings\ProjectVersion.txt`.
4. It runs Unity in batch mode with the matching method in `BuildScript`.
5. If the first Unity run imports the clean clone but does not produce output, the script retries once.

## Android one-click build

Double-click:

`BuildTools\Build-Android.bat`

The final APK is created at:

`C:\Usman\APKs\APKs\Erny.apk`

The Android log is created at:

`C:\Usman\BuildAgents\Logs\android-build.log`

## iOS one-click build

Double-click:

`BuildTools\Build-iOS.bat`

The final iOS Xcode project is exported at:

`C:\Usman\iOSBuilds\Erny-iOS`

The iOS log is created at:

`C:\Usman\BuildAgents\Logs\ios-build.log`

Important: this exports the Unity iOS/Xcode project. Creating a signed `.ipa` still requires opening/archive-signing the Xcode project on macOS with the correct Apple signing setup.

## iOS build on Mac mini

Double-click:

`BuildTools\Build-iOS-And-Send-To-Mac.bat`

This does three things:

1. Exports the Unity iOS Xcode project on Windows.
2. Sends the exported project to the Mac mini through SSH/SCP.
3. Runs `xcodebuild build` on the Mac mini.

Default Mac target:

`elytramacminim42@192.168.100.251`

Default Mac project path:

`/Users/elytramacminim42/BlingARBuilds/Incoming/Erny-iOS`

The Mac build script cleans bad `UnityFramework` linker flags before building. It removes `OTHER_LDFLAGS` entries that start with `-ld` or end with `-ISystem`.

For generic signing, create this optional file on the Mac mini:

`/Users/elytramacminim42/blingar-build.env`

Example content:

`export APPLE_TEAM_ID=ABCDE12345`

If `APPLE_TEAM_ID` is not set, the Mac build uses whatever signing settings already exist in the exported Xcode project.

## Optional Unity path override

If Unity is installed somewhere else, set:

`UNITY_EDITOR_PATH`

to the full path of `Unity.exe` before running the script.
