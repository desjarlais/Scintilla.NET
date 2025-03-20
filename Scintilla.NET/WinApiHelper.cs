using System;
using System.Runtime.InteropServices;

namespace ScintillaNET;

internal static class WinApiHelpers
{
    internal static IntPtr SetWindowLongPtr(this IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
        if (Environment.Is64BitProcess)
        {
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
    }

    internal static IntPtr GetWindowLongPtr(this IntPtr hWnd, int nIndex)
    {
        return GetWindowLong(hWnd, nIndex);
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

    internal const long WS_EX_LAYOUTRTL = 0x00400000L;
    internal const int GWL_EXSTYLE = -20;

    [DllImport("kernel32.dll")]
    internal static extern void GetNativeSystemInfo(out SYSTEM_INFO lpSystemInfo);

    [DllImport("kernel32.dll")]
    internal static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

    [StructLayout(LayoutKind.Sequential)]
    internal struct SYSTEM_INFO
    {
        public ushort wProcessorArchitecture;
        public ushort wReserved;
        public uint dwPageSize;
        public IntPtr lpMinimumApplicationAddress;
        public IntPtr lpMaximumApplicationAddress;
        public IntPtr dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public ushort wProcessorLevel;
        public ushort wProcessorRevision;
    }

    internal const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
    internal const ushort PROCESSOR_ARCHITECTURE_ARM = 5;
    internal const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
    internal const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
    internal const ushort PROCESSOR_ARCHITECTURE_ARM64 = 12;
    internal const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

    internal static Architecture GetProcessArchitecture()
    {
        GetSystemInfo(out SYSTEM_INFO sysInfo);
        return sysInfo.wProcessorArchitecture switch {
            PROCESSOR_ARCHITECTURE_INTEL => Architecture.X86,
            PROCESSOR_ARCHITECTURE_AMD64 => Architecture.X64,
            PROCESSOR_ARCHITECTURE_ARM => Architecture.Arm,
            PROCESSOR_ARCHITECTURE_ARM64 => Architecture.Arm64,
            _ => throw new PlatformNotSupportedException("Unknown processor architecture ID: " + sysInfo.wProcessorArchitecture),
        };
    }
}
