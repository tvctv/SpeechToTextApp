using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeechToTextApp
{
    public partial class MainForm : Form
    {
        private readonly AppSettings _settings;
        private readonly ProfanityFilter _filter = new();
        private readonly AudioCapture _audio = new();
        private WhisperService? _whisper;
        private Sce492Client? _sce;
        private readonly System.Collections.Generic.LinkedList<string> _lines = new();
        private bool _running = false;

        public MainForm()
        {
            InitializeComponent();
            _settings = AppSettings.Load();
            UpdateUi();
        }

        private async Task EnsureInitializedAsync()
        {
            if (_whisper == null)
            {
                _whisper = new WhisperService(_settings);
                await _whisper.InitializeAsync();
                _whisper.CaptionFinalized += (s, text) => OnCaption(text);
            }
            if (_sce == null)
            {
                _sce = new Sce492Client(_settings);
                await _sce.ConnectAsync();
            }
        }

        private void OnCaption(string text)
        {
            if (InvokeRequired) { BeginInvoke(new Action(() => OnCaption(text))); return; }

            if (_settings.ProfanityFilterEnabled)
                text = _filter.Clean(text);

            _lines.AddLast(text);
            while (_lines.Count > 3) _lines.RemoveFirst();

            txtCaption.Text = string.Join(Environment.NewLine, _lines);

            if (_sce != null && _settings.SceEnabled)
                Task.Run(() => _sce.SendCaptionAsync(text));
        }

        private async void btnToggle_Click(object sender, EventArgs e)
        {
            if (_running)
            {
                _audio.Stop();
                _whisper?.Stop();
                _running = false;
                UpdateUi();
                return;
            }

            try
            {
                await EnsureInitializedAsync();

                _audio.AudioDataAvailable += (s, buf) => _whisper?.PushAudio(buf);
                _audio.Start(_settings.AudioDeviceId);
                _whisper!.Start();
                _running = true;
                UpdateUi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUi()
        {
            btnToggle.Text = _running ? "Stop Captions" : "Start Captions";
            lblStatus.Text = _running ? "Running" : "Stopped";
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new SettingsForm(_settings);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _sce?.Dispose();
                _sce = null;
            }
        }
    }
}