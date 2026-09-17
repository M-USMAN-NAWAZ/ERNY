#!/bin/bash
set -euo pipefail

PROJECT_PATH=""
TEAM_ID="${APPLE_TEAM_ID:-}"
CONFIGURATION="Release"
DESTINATION="generic/platform=iOS"
BUILD_ROOT="$HOME/BlingARBuilds"
LOG_DIR="$BUILD_ROOT/Logs"
LOG_FILE="$LOG_DIR/mac-xcode-build.log"

mkdir -p "$LOG_DIR" "$BUILD_ROOT/DerivedData"
exec > >(tee "$LOG_FILE") 2>&1

echo "Mac build started: $(date)"
echo "Log file: $LOG_FILE"

while [[ $# -gt 0 ]]; do
  case "$1" in
    --project)
      PROJECT_PATH="$2"
      shift 2
      ;;
    --team-id)
      TEAM_ID="$2"
      shift 2
      ;;
    --configuration)
      CONFIGURATION="$2"
      shift 2
      ;;
    *)
      echo "Unknown argument: $1"
      exit 2
      ;;
  esac
done

if [[ -z "$PROJECT_PATH" ]]; then
  echo "Missing --project /path/to/exported-xcode-project"
  exit 2
fi

if [[ -f "$HOME/blingar-build.env" ]]; then
  # Optional local Mac config. Example: export APPLE_TEAM_ID=ABCDE12345
  source "$HOME/blingar-build.env"
  TEAM_ID="${TEAM_ID:-${APPLE_TEAM_ID:-}}"
fi

if [[ ! -d "$PROJECT_PATH" ]]; then
  echo "Project folder not found: $PROJECT_PATH"
  exit 1
fi

cd "$PROJECT_PATH"

echo "Project path: $PROJECT_PATH"
echo "Configuration: $CONFIGURATION"
echo "Destination: $DESTINATION"

if ! command -v xcodebuild >/dev/null 2>&1; then
  echo "xcodebuild was not found. Install Xcode and select it with xcode-select."
  exit 1
fi

echo "Xcode path: $(xcode-select -p)"
xcodebuild -version

PROJECT_FILE="$PROJECT_PATH/Unity-iPhone.xcodeproj/project.pbxproj"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [[ -f "$PROJECT_FILE" ]]; then
  python3 "$SCRIPT_DIR/clean-ios-linker-flags.py" "$PROJECT_FILE"
else
  echo "Unity-iPhone.xcodeproj/project.pbxproj not found."
  exit 1
fi

if [[ -f "Podfile" ]]; then
  if ! command -v pod >/dev/null 2>&1; then
    echo "Podfile exists, but CocoaPods is not installed or not in PATH."
    exit 1
  fi

  pod install
fi

BUILD_ARGS=(
  -scheme "Unity-iPhone"
  -configuration "$CONFIGURATION"
  -destination "$DESTINATION"
  -derivedDataPath "$BUILD_ROOT/DerivedData"
)

if [[ -d "Unity-iPhone.xcworkspace" ]]; then
  BUILD_ARGS=(-workspace "Unity-iPhone.xcworkspace" "${BUILD_ARGS[@]}")
else
  BUILD_ARGS=(-project "Unity-iPhone.xcodeproj" "${BUILD_ARGS[@]}")
fi

if [[ -n "$TEAM_ID" ]]; then
  echo "Using Apple development team: $TEAM_ID"
  BUILD_ARGS+=("DEVELOPMENT_TEAM=$TEAM_ID" "-allowProvisioningUpdates")
else
  echo "No APPLE_TEAM_ID provided. Using signing settings already inside the Xcode project."
fi

echo "Running xcodebuild..."
xcodebuild "${BUILD_ARGS[@]}" build
echo "Mac build completed: $(date)"
