#nullable enable
namespace ScintillaNET;

/// <summary>
/// Provides optional early configuration for locating native satellite libraries (Scintilla.dll, Lexilla.dll).
/// Must be configured BEFORE the <see cref="Scintilla"/> type is first touched; otherwise changes have no effect.
/// </summary>
public static class ScintillaNativeLibrary
{
    /// <summary>
    /// Optional absolute directory path probed first for native satellite libraries.
    /// If set, this directory is checked before all default probing locations.
    /// Example Usage: ScintillaNativeLibrary.SatelliteDirectory = @"C:\MyApp\binaries";
    /// </summary>
    public static string? SatelliteDirectory { get; set; }

    /// <summary>
    /// Verify the digital signature of native `Scintilla.dll` before loading it. Enabled by default.
    /// </summary>
    public static bool VerifyScintillaDll { get; set; } = true;

    /// <summary>
    /// Verify the digital signature of native `Lexilla.dll` before loading it. Enabled by default.
    /// </summary>
    public static bool VerifyLexillaDll { get; set; } = true;
}
