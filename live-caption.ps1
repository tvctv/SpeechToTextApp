# live-caption.ps1
# Simple interactive caption sender for Link Electronics SCE-492

$ip   = "172.16.73.62"   # ← Change if your encoder has a different IP
$port = 10001            # TCP tunnel port (from your Lantronix config)

Write-Host ("Connecting to SCE-492 at {0}:{1} ..." -f $ip, $port)
$client = New-Object System.Net.Sockets.TcpClient($ip, $port)
$stream = $client.GetStream()
$writer = New-Object System.IO.StreamWriter($stream, [System.Text.Encoding]::ASCII)
$writer.NewLine  = "`r`n"
$writer.AutoFlush = $true

# Send Ctrl+B (0x02) to unlock the tunnel if 'Flush Start Character' is enabled
#$writer.BaseStream.WriteByte(0x02)
$writer.Flush()
Write-Host "Connected! (Ctrl+B sent)"
Write-Host "Type your caption lines below. Press Ctrl+C to exit.`n"

# Start realtime caption mode
$writer.WriteLine("!R1")

# Loop: read a line, send it as a caption block
while ($true) {
    $line = Read-Host "Caption"
    if ([string]::IsNullOrWhiteSpace($line)) { continue }

    # Optional placeholder profanity filter
    $line = $line -replace '\*', ''

    # Send text and end command
    $writer.WriteLine("!T$line")
    $writer.WriteLine("!E")
}
