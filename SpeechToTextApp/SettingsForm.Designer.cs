namespace SpeechToTextApp
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtHost = new System.Windows.Forms.TextBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.chkUdp = new System.Windows.Forms.CheckBox();
            this.chkEnableSce = new System.Windows.Forms.CheckBox();
            this.cmbSerial = new System.Windows.Forms.ComboBox();
            this.numBaud = new System.Windows.Forms.NumericUpDown();
            this.cmbAudio = new System.Windows.Forms.ComboBox();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.txtModelDir = new System.Windows.Forms.TextBox();
            this.txtSmall = new System.Windows.Forms.TextBox();
            this.txtMedium = new System.Windows.Forms.TextBox();
            this.chkProfanity = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBaud)).BeginInit();
            this.SuspendLayout();
            // 
            this.txtHost.SetBounds(20, 20, 250, 24);
            this.numPort.SetBounds(280, 20, 80, 24);
            this.chkUdp.SetBounds(370, 20, 120, 24);
            this.chkEnableSce.SetBounds(20, 50, 200, 24);
            this.cmbSerial.SetBounds(20, 80, 150, 24);
            this.numBaud.SetBounds(180, 80, 100, 24);
            this.cmbAudio.SetBounds(20, 120, 300, 24);
            this.cmbModel.SetBounds(330, 120, 160, 24);
            this.txtModelDir.SetBounds(20, 160, 300, 24);
            this.txtSmall.SetBounds(20, 190, 300, 24);
            this.txtMedium.SetBounds(20, 220, 300, 24);
            this.chkProfanity.SetBounds(20, 250, 200, 24);
            this.btnSave.SetBounds(20, 290, 120, 30);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.ClientSize = new System.Drawing.Size(520, 340);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.txtHost, this.numPort, this.chkUdp, this.chkEnableSce,
                this.cmbSerial, this.numBaud, this.cmbAudio, this.cmbModel,
                this.txtModelDir, this.txtSmall, this.txtMedium, this.chkProfanity,
                this.btnSave
            });
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBaud)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.CheckBox chkUdp;
        private System.Windows.Forms.CheckBox chkEnableSce;
        private System.Windows.Forms.ComboBox cmbSerial;
        private System.Windows.Forms.NumericUpDown numBaud;
        private System.Windows.Forms.ComboBox cmbAudio;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.TextBox txtModelDir;
        private System.Windows.Forms.TextBox txtSmall;
        private System.Windows.Forms.TextBox txtMedium;
        private System.Windows.Forms.CheckBox chkProfanity;
        private System.Windows.Forms.Button btnSave;
    }
}