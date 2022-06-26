using System;
using System.Threading;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace HotdogServer
{
    internal class HotdogBLEMonitor
    {
        public event EventHandler<DepthReadingEventArgs> NewReading;

        private BluetoothLEAdvertisementWatcher bleWatcher;

        public void Open()
        {
            bleWatcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Passive
            };
            bleWatcher.AdvertisementFilter = new BluetoothLEAdvertisementFilter
            {
                Advertisement = new BluetoothLEAdvertisement
                {
                    LocalName = "hotdog"
                }
            };
            bleWatcher.Received += OnAdvertisementReceived;
            bleWatcher.Stopped += (s, a) => { };
            try
            {
                bleWatcher.Start();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error: could not start BLE watcher.");
                Console.Error.WriteLine(e);
                return;
            }
            BetterScanner.StartScanner(0, 10, 10);
            new Thread(() =>
            {
                while (true)
                {
                    BluetoothLEAdvertisementWatcher w = new BluetoothLEAdvertisementWatcher();
                    w.ScanningMode = BluetoothLEScanningMode.Passive;
                    w.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(100);
                    w.Received += (s, a) => { };
                    w.Start();
                    Thread.Sleep(2000);
                    w.Stop();
                }
            }).Start();
        }

        public void Close()
        {
            bleWatcher.Stop();
        }

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender,
            BluetoothLEAdvertisementReceivedEventArgs args)
        {
            foreach (var section in args.Advertisement.DataSections)
            {
                if (section.DataType != 0x16)
                {
                    continue;
                }
                var dataReader = DataReader.FromBuffer(section.Data);
                int level = int.Parse(dataReader.ReadString(section.Data.Length).Substring(2));
                NewReading?.Invoke(this, new DepthReadingEventArgs { Depth = level / 100f });
            }
        }
    }
}
