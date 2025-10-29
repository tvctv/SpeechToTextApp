using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Whisper.net;

namespace SpeechToTextApp
{
    public class WhisperService : IDisposable
    {
        private readonly AppSettings _settings;
        private WhisperFactory? _factory;
        private WhisperProcessor? _processor;
        private CancellationTokenSource? _cts;
        private readonly ConcurrentQueue<float> _sampleQueue = new();
        private Task? _worker;
        private int _threads = Math.Max(1, Environment.ProcessorCount / 2);

        public event EventHandler<string>? CaptionFinalized;

        public WhisperService(AppSettings settings)
        {
            _settings = settings;
            DetectOptimalSettings();
        }

        private void DetectOptimalSettings()
        {
            if (Environment.ProcessorCount >= 12 && _settings.Model != ModelSize.MediumEn)
                _settings.Model = ModelSize.MediumEn;
        }

        public async Task InitializeAsync()
        {
            var modelPath = _settings.ResolveModelPath();
            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"Model not found at {modelPath}");

            _factory = WhisperFactory.FromPath(modelPath);
            _processor = _factory.CreateBuilder()
                .WithLanguage("en")
                .WithThreads(_threads)
                .Build();
        }

        public void PushAudio(byte[] pcm16LeMono16k)
        {
            for (int i = 0; i < pcm16LeMono16k.Length; i += 2)
            {
                short sample = (short)(pcm16LeMono16k[i] | (pcm16LeMono16k[i + 1] << 8));
                _sampleQueue.Enqueue(sample / 32768f);
            }
        }

        public void Start()
        {
            if (_processor == null) throw new InvalidOperationException("Call InitializeAsync first.");
            Stop();
            _cts = new CancellationTokenSource();
            _worker = Task.Run(() => WorkerLoopAsync(_cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
            try { _worker?.Wait(1000); } catch { }
            _cts = null; _worker = null;
        }

        private async Task WorkerLoopAsync(CancellationToken ct)
        {
            var window = new System.Collections.Generic.List<float>(16000 * 5);
            while (!ct.IsCancellationRequested)
            {
                while (window.Count < 16000 * 5 && _sampleQueue.TryDequeue(out var s))
                {
                    window.Add(s);
                }

                if (window.Count >= 16000 * 2)
                {
                    var arr = window.ToArray();
                    window.Clear();

                    if (_processor != null)
                    {
                        var result = await _processor.ProcessAsync(arr, ct);
                        var text = string.Concat(result.Segments.Select(s => s.Text)).Trim();
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            CaptionFinalized?.Invoke(this, text);
                        }
                    }
                }
                else
                {
                    await Task.Delay(50, ct);
                }
            }
        }

        public void Dispose()
        {
            Stop();
            _processor?.Dispose();
            _factory?.Dispose();
        }
    }
}