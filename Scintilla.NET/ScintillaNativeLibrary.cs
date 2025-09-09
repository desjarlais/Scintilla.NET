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
    public static string SatelliteDirectory { get; set; }

    /// <summary>
    /// Optional absolute path to Scintilla.dll.
    /// </summary>
    public static string ScintillaFullPath { get; set; }

    /// <summary>
    /// optional absolute path to Lexilla.dll.
    /// </summary>
    public static string LexillaFullPath { get; set; }
}
