# SpeechToTextApp (patched)

- Pins Whisper.net to `1.9.0-preview1` (available on nuget.org).
- Removes stray `WasapiCapture` usage; uses `WaveInEvent` only (NAudio).
- Uses `Microsoft.NET.Sdk` with `UseWindowsForms=true` to eliminate SDK warning.

**Build steps**

1. Open `SpeechToTextApp.sln` in Visual Studio 2022.
2. Ensure **nuget.org** package source is enabled.
3. Right-click solution → Restore NuGet Packages.
4. Build → Rebuild Solution.
5. Run and configure models/audio device in Settings.

Place Whisper models (e.g., `ggml-small.en.bin`, `ggml-medium.en.bin`) inside a `models` folder next to the EXE, or change paths in Settings.