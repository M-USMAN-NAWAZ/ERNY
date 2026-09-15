param(
    [string]$MacUser = "elytramacminim42",
    [string]$MacHost = "192.168.100.251",
    [string]$MacBuildRoot = "/Users/elytramacminim42/BlingARBuilds",
    [string]$IOSProjectPath = "C:\Usman\iOSBuilds\Erny-iOS",
    [string]$AppleTeamId = "",
    [string]$SshExe = "C:\Windows\System32\OpenSSH\ssh.exe",
    [string]$ScpExe = "C:\Windows\System32\OpenSSH\scp.exe"
)

$ErrorActionPreference = "Stop"

function Invoke-CheckedCommand {
    param(
        [string]$FilePath,
        [string[]]$Arguments,
        [string]$FailureMessage
    )

    & $FilePath @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw $FailureMessage
    }
}

function Test-MacSshConnection {
    param(
        [string]$HostName,
        [int]$Port = 22,
        [int]$TimeoutMilliseconds = 5000
    )

    $client = [System.Net.Sockets.TcpClient]::new()
    try {
        $connectTask = $client.ConnectAsync($HostName, $Port)
        if (-not $connectTask.Wait($TimeoutMilliseconds)) {
            return $false
        }

        return $client.Connected
    }
    catch {
        return $false
    }
    finally {
        $client.Dispose()
    }
}

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$buildScript = Join-Path $scriptRoot "Build-iOS.ps1"
$macScript = Join-Path $scriptRoot "mac-build-ios-xcode.sh"
$cleanerScript = Join-Path $scriptRoot "clean-ios-linker-flags.py"

if (-not (Test-Path -LiteralPath $buildScript)) {
    throw "Could not find Windows iOS build script: $buildScript"
}

if (-not (Test-Path -LiteralPath $macScript)) {
    throw "Could not find Mac build script: $macScript"
}

if (-not (Test-Path -LiteralPath $cleanerScript)) {
    throw "Could not find linker flag cleaner script: $cleanerScript"
}

if (-not (Test-Path -LiteralPath $SshExe)) {
    throw "Could not find ssh.exe: $SshExe"
}

if (-not (Test-Path -LiteralPath $ScpExe)) {
    throw "Could not find scp.exe: $ScpExe"
}

$remote = "$MacUser@$MacHost"
$remoteProjectRoot = "$MacBuildRoot/Incoming"
$remoteProjectPath = "$remoteProjectRoot/Erny-iOS"
$remoteScriptsRoot = "$remoteProjectPath/_BuildToolsForMac"

Write-Host "Preflight: Checking SSH connection to $remote..."
if (-not (Test-MacSshConnection -HostName $MacHost)) {
    throw @"
Could not reach Mac mini SSH at $MacHost:22.

Check these on the Mac mini:
1. Mac mini is awake and connected to the same Ethernet/Wi-Fi network.
2. Remote Login is enabled in System Settings > General > Sharing.
3. The IP address is still $MacHost. If it changed, run this script with -MacHost NEW_IP.
4. Firewall allows SSH / Remote Login.
"@
}

Write-Host "Step 1/4: Exporting Unity iOS Xcode project on Windows..."
& $buildScript
if ($LASTEXITCODE -ne 0) {
    throw "Unity iOS export failed"
}

if (-not (Test-Path -LiteralPath $IOSProjectPath)) {
    throw "Expected exported iOS project was not found: $IOSProjectPath"
}

Write-Host "Step 2/4: Preparing Mac build folders..."
Invoke-CheckedCommand -FilePath $SshExe -Arguments @(
    $remote,
    "mkdir -p '$remoteProjectRoot' '$MacBuildRoot/Logs' '$MacBuildRoot/DerivedData' && rm -rf '$remoteProjectPath'"
) -FailureMessage "Could not prepare folders on Mac mini"

Write-Host "Step 3/4: Sending Xcode project and Mac scripts to Mac mini..."

$iosProjectFullPath = (Resolve-Path -LiteralPath $IOSProjectPath).Path
$iosProjectParent = Split-Path -Parent $iosProjectFullPath
$iosProjectFolder = Split-Path -Leaf $iosProjectFullPath
$localBundledScriptsRoot = Join-Path $iosProjectFullPath "_BuildToolsForMac"

New-Item -ItemType Directory -Force -Path $localBundledScriptsRoot | Out-Null
Copy-Item -LiteralPath $macScript -Destination (Join-Path $localBundledScriptsRoot "mac-build-ios-xcode.sh") -Force
Copy-Item -LiteralPath $cleanerScript -Destination (Join-Path $localBundledScriptsRoot "clean-ios-linker-flags.py") -Force

Push-Location $iosProjectParent
try {
    Invoke-CheckedCommand -FilePath $ScpExe -Arguments @(
        "-r",
        $iosProjectFolder,
        "${remote}:$remoteProjectRoot/"
    ) -FailureMessage "Could not send iOS Xcode project to Mac mini"
}
finally {
    Pop-Location
}

Write-Host "Step 4/4: Running Xcode build on Mac mini..."
$teamArgument = ""
if (-not [string]::IsNullOrWhiteSpace($AppleTeamId)) {
    $teamArgument = " --team-id '$AppleTeamId'"
}

Invoke-CheckedCommand -FilePath $SshExe -Arguments @(
    $remote,
    "perl -pi -e 's/\r$//' '$remoteScriptsRoot/mac-build-ios-xcode.sh' '$remoteScriptsRoot/clean-ios-linker-flags.py' && chmod +x '$remoteScriptsRoot/mac-build-ios-xcode.sh' && '$remoteScriptsRoot/mac-build-ios-xcode.sh' --project '$remoteProjectPath'$teamArgument"
) -FailureMessage "Mac mini Xcode build failed. Check Mac log: $MacBuildRoot/Logs/mac-xcode-build.log"

Write-Host ""
Write-Host "iOS Xcode build finished on Mac mini."
Write-Host "Mac project path: $remoteProjectPath"
