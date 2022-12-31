using System.Runtime.InteropServices;

namespace NetMauiApp;

public partial class MainPage : ContentPage
{
    [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern uint midiOutGetNumDevs();

    [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int midiOutGetDevCaps(uint uDeviceID, ref MIDIOUTCAPS pmoc, uint cbmoc);

    [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int midiOutOpen(out IntPtr phmo, uint uDeviceID, IntPtr dwCallback, IntPtr dwInstance, uint fdwOpen);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MIDIOUTCAPS
    {
        public short wMid;
        public short wPid;
        public int vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        public short wTechnology;
        public short wVoices;
        public short wNotes;
        public short wChannelMask;
        public uint dwSupport;
    }

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
        var midiOutCaps = new MIDIOUTCAPS();

        for (uint i = 0; i < midiOutGetNumDevs(); i++)
        {
            midiOutGetDevCaps(i, ref midiOutCaps, (uint)Marshal.SizeOf(midiOutCaps));

            if (midiOutCaps.szPname == "Microsoft GS Wavetable Synth")
            {
                var hMidiOut = IntPtr.Zero;

                Console.WriteLine("Opening the device...");
                var result = midiOutOpen(out hMidiOut, 0, IntPtr.Zero, IntPtr.Zero, 0);
                if (result != 0)
                    throw new Exception($"midiOutOpen failed with {result}.");

                Console.WriteLine("Opened.");
                break;
            }
        }
    }
}

