#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_PATH="$ROOT_DIR/Vjezba.Model/Vjezba.Model.csproj"
APP_URL="http://localhost:5199"
LOG_FILE="$(mktemp -t vjezba-run.XXXXXX.log)"

cleanup() {
  if [[ -n "${DOTNET_PID:-}" ]] && kill -0 "$DOTNET_PID" 2>/dev/null; then
    kill "$DOTNET_PID"
    wait "$DOTNET_PID" 2>/dev/null || true
  fi

  if [[ -n "${TAIL_PID:-}" ]] && kill -0 "$TAIL_PID" 2>/dev/null; then
    kill "$TAIL_PID" 2>/dev/null || true
  fi

  rm -f "$LOG_FILE"
}

trap cleanup EXIT INT TERM

echo "Starting ASP.NET app..."
dotnet run --project "$PROJECT_PATH" >"$LOG_FILE" 2>&1 &
DOTNET_PID=$!

echo "Waiting for server to be ready..."
for _ in {1..60}; do
  if grep -q "Now listening on" "$LOG_FILE"; then
    break
  fi

  if ! kill -0 "$DOTNET_PID" 2>/dev/null; then
    echo "App stopped unexpectedly during startup."
    cat "$LOG_FILE"
    exit 1
  fi

  sleep 1
done

if ! grep -q "Now listening on" "$LOG_FILE"; then
  echo "Timed out waiting for app startup."
  cat "$LOG_FILE"
  exit 1
fi

echo "Opening $APP_URL"
open "$APP_URL"

echo "App is running. Press Ctrl+C to stop."
tail -n +1 -f "$LOG_FILE" &
TAIL_PID=$!

wait "$DOTNET_PID"
