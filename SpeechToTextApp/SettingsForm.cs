using System;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace SpeechToTextApp
{
    public partial class SettingsForm : Form
    {
        private readonly AppSettings _settings;

        public SettingsForm(AppSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            LoadValues();
        }

        private void LoadValues()
        {
            txtHost.Text = _settings.SceHost;
            numPort.Value = _settings.ScePort;
            chkUdp.Checked = _settings.SceUseUdp;
            chkEnableSce.Checked = _settings.SceEnabled;

            cmbSerial.Items.Clear();
            cmbSerial.Items.Add("(none)");
            foreach (var port in SerialPort.GetPortNames())
                cmbSerial.Items.Add(port);
            cmbSerial.SelectedItem = string.IsNullOrWhiteSpace(_settings.SerialPortName) ? "(none)" : _settings.SerialPortName;
            numBaud.Value = _settings.SerialBaud;

            cmbAudio.Items.Clear();
            var devs = AudioCapture.ListInputDevices();
            foreach (var d in devs) cmbAudio.Items.Add($"{d.id}|{d.name}");
            if (!string.IsNullOrEmpty(_settings.AudioDeviceId))
            {
                var match = devs.FirstOrDefault(x => x.id == _settings.AudioDeviceId);
                if (!string.IsNullOrEmpty(match.id))
                {
                    cmbAudio.SelectedItem = $"{match.id}|{match.name}";
                }
            }
            if (cmbAudio.SelectedIndex < 0 && cmbAudio.Items.Count > 0) cmbAudio.SelectedIndex = 0;

            cmbModel.Items.Clear();
            cmbModel.Items.Add("Small (English)");
            cmbModel.Items.Add("Medium (English)");
            cmbModel.SelectedIndex = _settings.Model == ModelSize.SmallEn ? 0 : 1;

            txtModelDir.Text = _settings.ModelDir;
            txtSmall.Text = _settings.SmallModelFile;
            txtMedium.Text = _settings.MediumModelFile;

            chkProfanity.Checked = _settings.ProfanityFilterEnabled;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _settings.SceHost = txtHost.Text.Trim();
            _settings.ScePort = (int)numPort.Value;
            _settings.SceUseUdp = chkUdp.Checked;
            _settings.SceEnabled = chkEnableSce.Checked;

            var serialSel = cmbSerial.SelectedItem?.ToString() ?? "(none)";
            _settings.SerialPortName = serialSel == "(none)" ? null : serialSel;
            _settings.SerialBaud = (int)numBaud.Value;

            var audioSel = cmbAudio.SelectedItem?.ToString() ?? "";
            _settings.AudioDeviceId = audioSel.Split('|')[0];

            _settings.Model = cmbModel.SelectedIndex == 0 ? ModelSize.SmallEn : ModelSize.MediumEn;

            _settings.ModelDir = txtModelDir.Text.Trim();
            _settings.SmallModelFile = txtSmall.Text.Trim();
            _settings.MediumModelFile = txtMedium.Text.Trim();

            _settings.ProfanityFilterEnabled = chkProfanity.Checked;

            _settings.Save();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}