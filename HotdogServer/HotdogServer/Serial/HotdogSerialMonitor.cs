using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace HotdogServer
{
    internal class HotdogSerialMonitor
    {
        public event EventHandler<DepthReadingEventArgs> NewReading;

        private List<SerialPort> ports = new List<SerialPort>();

        public void Open()
        {
            Close();
            foreach (string name in SerialPort.GetPortNames())
            {
                // GetPortNames has a bug in win10, port names sometimes have garbage characters
                // at the end
                string fixedName = Regex.Replace(name, @"[^a-zA-Z0-9]*$", "");
                try
                {
                    var port = new SerialPort(fixedName, 9600, Parity.None, 8, StopBits.One);
                    port.DtrEnable = true;
                    port.DataReceived += (s, a) => ProcessData(port);
                    port.Open();
                    ports.Add(port);
                }
                catch (IOException) { }
            }
        }

        public void Close()
        {
            ports.ForEach(port => port.Close());
            ports.Clear();
        }

        private void ProcessData(SerialPort port)
        {
            string line = port.ReadLine();
            if (line == "")
            {
                return;
            }
            if (line.StartsWith("L0"))
            {
                ports.ForEach(p => p.Close());
                SerialPort hotdogPort = new SerialPort(port.PortName, port.BaudRate, port.Parity,
                    port.DataBits, port.StopBits);
                hotdogPort.DtrEnable = true;
                hotdogPort.DataReceived += (s, a) => ProcessHotdogData(hotdogPort);
                hotdogPort.Open();
                Console.WriteLine($"Listening to Hotdog serial device on port {port.PortName}");
            }
        }

        private void ProcessHotdogData(SerialPort port)
        {
            string line = port.ReadLine();
            if (line.Length < 3)
            {
                return;
            }
            if (!int.TryParse(line.Substring(2), out int depth))
            {
                return;
            }
            NewReading?.Invoke(this, new DepthReadingEventArgs { Depth = depth / 100f });
        }
    }
}
