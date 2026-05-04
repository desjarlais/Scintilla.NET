using System;
using System.Runtime.InteropServices;
using Windows.Win32;

namespace ScintillaNET;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static partial class NativeMethods
{
    #region Constants

    public const uint WM_REFLECT = PInvoke.WM_USER + 0x1C00;

    #endregion Constants

    #region Functions

    [DllImport("User32.dll", SetLastError = true)]
    public static extern IntPtr MB_GetString(uint hInst);

    public static string GetMessageBoxString(uint msgId) =>
        Marshal.PtrToStringUni(MB_GetString(msgId));

    #endregion Functions

    #region Structures

    // http://www.openrce.org/articles/full_view/23
    // It's worth noting that this structure (and the 64-bit version below) represents the ILoader
    // class virtual function table (vtable), NOT the ILoader interface defined in ILexer.h.
    // In this case they are identical because the ILoader class contains only functions.
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ILoaderVTable32
    {
        public ReleaseDelegate Release;
        public AddDataDelegate AddData;
        public ConvertToDocumentDelegate ConvertToDocument;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ReleaseDelegate(IntPtr self);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int AddDataDelegate(IntPtr self, byte* data, int length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ConvertToDocumentDelegate(IntPtr self);
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ILoaderVTable64
    {
        public ReleaseDelegate Release;
        public AddDataDelegate AddData;
        public ConvertToDocumentDelegate ConvertToDocument;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ReleaseDelegate(IntPtr self);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int AddDataDelegate(IntPtr self, byte* data, int length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr ConvertToDocumentDelegate(IntPtr self);
    }

    #endregion Structures
}
