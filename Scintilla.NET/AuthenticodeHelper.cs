using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace ScintillaNET;

/// <summary>
/// Provides Authenticode signature verification for native DLLs.
/// </summary>
internal static class AuthenticodeHelper
{
    private static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new("00AAC56B-CD44-11d0-8CC2-00C04FC295EE");

    private const uint WTD_UI_NONE = 2;
    private const uint WTD_REVOKE_NONE = 0;
    private const uint WTD_CHOICE_FILE = 1;
    private const uint WTD_STATEACTION_VERIFY = 1;
    private const uint WTD_STATEACTION_CLOSE = 2;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct WINTRUST_FILE_INFO
    {
        public uint cbStruct;
        public string pcwszFilePath;
        public IntPtr hFile;
        public IntPtr pgKnownSubject;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WINTRUST_DATA
    {
        public uint cbStruct;
        public IntPtr pPolicyCallbackData;
        public IntPtr pSIPClientData;
        public uint dwUIChoice;
        public uint fdwRevocationChecks;
        public uint dwUnionChoice;
        public IntPtr pFile; // WINTRUST_FILE_INFO*
        public uint dwStateAction;
        public IntPtr hWVTStateData;
        public IntPtr pwszURLReference;
        public uint dwProvFlags;
        public uint dwUIContext;
        public IntPtr pSignatureSettings;
    }

    [DllImport("wintrust.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
    private static extern int WinVerifyTrust(IntPtr hwnd, ref Guid pgActionID, ref WINTRUST_DATA pWVTData);

    /// <summary>
    /// Verifies that a file has a valid Authenticode signature.
    /// </summary>
    /// <param name="filePath">The full path to the file to verify.</param>
    /// <returns><c>true</c> if the file has a valid Authenticode signature; otherwise, <c>false</c>.</returns>
    public static bool HasValidSignature(string filePath)
    {
        var fileInfo = new WINTRUST_FILE_INFO
        {
            cbStruct = (uint)Marshal.SizeOf<WINTRUST_FILE_INFO>(),
            pcwszFilePath = filePath,
            hFile = IntPtr.Zero,
            pgKnownSubject = IntPtr.Zero,
        };

        IntPtr fileInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf<WINTRUST_FILE_INFO>());
        try
        {
            Marshal.StructureToPtr(fileInfo, fileInfoPtr, false);

            var trustData = new WINTRUST_DATA
            {
                cbStruct = (uint)Marshal.SizeOf<WINTRUST_DATA>(),
                dwUIChoice = WTD_UI_NONE,
                fdwRevocationChecks = WTD_REVOKE_NONE,
                dwUnionChoice = WTD_CHOICE_FILE,
                pFile = fileInfoPtr,
                dwStateAction = WTD_STATEACTION_VERIFY,
                hWVTStateData = IntPtr.Zero,
                pwszURLReference = IntPtr.Zero,
                dwProvFlags = 0,
                dwUIContext = 0,
            };

            Guid actionId = WINTRUST_ACTION_GENERIC_VERIFY_V2;
            int result = WinVerifyTrust(IntPtr.Zero, ref actionId, ref trustData);

            // Close the state handle
            trustData.dwStateAction = WTD_STATEACTION_CLOSE;
            WinVerifyTrust(IntPtr.Zero, ref actionId, ref trustData);

            // 0 = success (signature is valid and trusted)
            return result == 0;
        }
        finally
        {
            Marshal.FreeHGlobal(fileInfoPtr);
        }
    }

    /// <summary>
    /// Verifies the Authenticode signature and optionally checks that the signing certificate
    /// subject contains the expected publisher name.
    /// </summary>
    /// <param name="filePath">The full path to the file to verify.</param>
    /// <param name="expectedSubject">
    /// The expected certificate Subject string (e.g., "CN=MyCompany, O=MyCompany, L=...") to match, or <c>null</c> to skip publisher validation.
    /// </param>
    /// <returns><c>true</c> if the signature is valid and the publisher matches (if specified); otherwise, <c>false</c>.</returns>
    public static bool VerifySignature(string filePath, string expectedSubject = null)
    {
        if (!HasValidSignature(filePath))
            return false;

        if (string.IsNullOrEmpty(expectedSubject))
            return true;

        // Optionally verify the signing certificate's subject
        try
        {
            using var cert = new X509Certificate2(filePath);
            return cert.Subject != null &&
                   string.Equals(cert.Subject, expectedSubject, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}
