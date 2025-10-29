using System;
using System.IO;
using System.Text.Json;

namespace SpeechToTextApp
{
    public enum ModelSize { SmallEn, MediumEn }

    public class AppSettings
    {
        public string SceHost { get; set; } = "127.0.0.1";
        public int ScePort { get; set; } = 5000;
        public bool SceUseUdp { get; set; } = true;
        public bool SceEnabled { get; set; } = true;
        public string? SerialPortName { get; set; } = null;
        public int SerialBaud { get; set; } = 9600;

        public string AudioDeviceId { get; set; } = ""; // NAudio device friendly name
        public ModelSize Model { get; set; } = ModelSize.SmallEn;

        public string ModelDir { get; set; } = "models"; // where .bin lives
        public string SmallModelFile { get; set; } = "ggml-small.en.bin";
        public string MediumModelFile { get; set; } = "ggml-medium.en.bin";

        public bool ProfanityFilterEnabled { get; set; } = true;

        public static string ConfigPath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SpeechToTextApp", "config.json");

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    var json = File.ReadAllText(ConfigPath);
                    return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch { }
            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(ConfigPath)!;
                Directory.CreateDirectory(dir);
                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch { }
        }

        public string ResolveModelPath()
        {
            var file = Model == ModelSize.SmallEn ? SmallModelFile : MediumModelFile;
            return Path.Combine(ModelDir, file);
        }
    }
}