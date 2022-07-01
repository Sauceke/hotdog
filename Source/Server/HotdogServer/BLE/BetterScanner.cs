using System;
using System.Runtime.InteropServices;
using System.Threading;

// Based on the following SO answers:
// https://stackoverflow.com/a/37328965
// https://stackoverflow.com/a/65091813
class BetterScanner
{
    /// <summary>
    /// The BLUETOOTH_FIND_RADIO_PARAMS structure facilitates enumerating installed Bluetooth radios.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private struct BLUETOOTH_FIND_RADIO_PARAM
    {
        internal UInt32 dwSize;
        internal void Initialize()
        {
            this.dwSize = (UInt32)Marshal.SizeOf(typeof(BLUETOOTH_FIND_RADIO_PARAM));
        }
    }

    /// <summary>
    /// Closes an open object handle.
    /// </summary>
    /// <param name="handle">[In] A valid handle to an open object.</param>
    /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
    [DllImport("Kernel32.dll", SetLastError = true)]
    static extern bool CloseHandle(IntPtr handle);

    /// <summary>
    /// Finds the first bluetooth radio present in device manager
    /// </summary>
    /// <param name="pbtfrp">Pointer to a BLUETOOTH_FIND_RADIO_PARAMS structure</param>
    /// <param name="phRadio">Pointer to where the first enumerated radio handle will be returned. When no longer needed, this handle must be closed via CloseHandle.</param>
    /// <returns>In addition to the handle indicated by phRadio, calling this function will also create a HBLUETOOTH_RADIO_FIND handle for use with the BluetoothFindNextRadio function.
    /// When this handle is no longer needed, it must be closed via the BluetoothFindRadioClose.
    /// Returns NULL upon failure. Call the GetLastError function for more information on the error. The following table describe common errors:</returns>
    [DllImport("irprops.cpl", SetLastError = true)]
    static extern IntPtr BluetoothFindFirstRadio(ref BLUETOOTH_FIND_RADIO_PARAM pbtfrp, out IntPtr phRadio);

    [StructLayout(LayoutKind.Sequential)]
    private struct LE_SCAN_REQUEST
    {
        internal uint unknown1;
        internal int scanType;
        internal uint unknown2;
        internal ushort scanInterval;
        internal ushort scanWindow;
        internal uint unknown3_0;
        internal uint unknown3_1;
    }

    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
    static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode,
    ref LE_SCAN_REQUEST lpInBuffer, uint nInBufferSize,
    IntPtr lpOutBuffer, uint nOutBufferSize,
    out uint lpBytesReturned, IntPtr lpOverlapped);

    /// <summary>
    /// Starts scanning for LE devices.
    /// Example: BetterScanner.StartScanner(0, 29, 29)
    /// </summary>
    /// <param name="scanType">0 = Passive, 1 = Active</param>
    /// <param name="scanInterval">Interval in 0.625 ms units</param>
    /// <param name="scanWindow">Window in 0.625 ms units</param>
    public static void StartScanner(int scanType, ushort scanInterval, ushort scanWindow)
    {
        var thread = new Thread(() =>
        {
            BLUETOOTH_FIND_RADIO_PARAM param = new BLUETOOTH_FIND_RADIO_PARAM();
            param.Initialize();
            IntPtr handle;
            BluetoothFindFirstRadio(ref param, out handle);
            uint outsize;
            LE_SCAN_REQUEST req = new LE_SCAN_REQUEST { scanType = scanType, scanInterval = scanInterval, scanWindow = scanWindow };
            bool a = DeviceIoControl(handle, 0x41118c, ref req, 24, IntPtr.Zero, 0, out outsize, IntPtr.Zero);
            Console.WriteLine(a);
        });
        thread.Start();
    }
}
