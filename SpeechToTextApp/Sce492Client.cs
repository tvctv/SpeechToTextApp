using System;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextApp
{
    public class Sce492Client : IDisposable
    {
        private readonly AppSettings _settings;
        private UdpClient? _udp;
        private TcpClient? _tcp;
        private NetworkStream? _tcpStream;
        private SerialPort? _serial;

        public bool Connected { get; private set; }

        public Sce492Client(AppSettings settings)
        {
            _settings = settings;
        }

        public async Task ConnectAsync()
        {
            Disconnect();
            if (!_settings.SceEnabled) return;

            if (!string.IsNullOrWhiteSpace(_settings.SerialPortName))
            {
                _serial = new SerialPort(_settings.SerialPortName, _settings.SerialBaud);
                _serial.Open();
                Connected = _serial.IsOpen;
                return;
            }

            if (_settings.SceUseUdp)
            {
                _udp = new UdpClient();
                _udp.Connect(_settings.SceHost, _settings.ScePort);
                Connected = true;
            }
            else
            {
                _tcp = new TcpClient();
                await _tcp.ConnectAsync(_settings.SceHost, _settings.ScePort);
                _tcpStream = _tcp.GetStream();
                Connected = true;
            }
        }

        public void Disconnect()
        {
            Connected = false;
            try { _udp?.Close(); } catch { }
            try { _tcpStream?.Close(); } catch { }
            try { _tcp?.Close(); } catch { }
            try { if (_serial?.IsOpen == true) _serial.Close(); } catch { }

            _udp = null; _tcp = null; _tcpStream = null; _serial = null;
        }

        public async Task SendCaptionAsync(string text)
        {
            if (!_settings.SceEnabled) return;
            var payload = Encoding.UTF8.GetBytes(text + "\r\n");

            if (_serial?.IsOpen == true)
            {
                _serial.Write(text + "\r\n");
                return;
            }

            if (_settings.SceUseUdp && _udp != null)
            {
                await _udp.SendAsync(payload, payload.Length);
            }
            else if (_tcpStream != null)
            {
                await _tcpStream.WriteAsync(payload, 0, payload.Length);
                await _tcpStream.FlushAsync();
            }
        }

        public void Dispose() => Disconnect();
    }
}