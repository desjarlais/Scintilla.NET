namespace ScintillaNET;

/// <summary>
/// The rendering technology used in a <see cref="Scintilla" /> control.
/// </summary>
public enum Technology
{
    /// <summary>
    /// Renders text using GDI. This is the default.
    /// </summary>
    Default = NativeMethods.SC_TECHNOLOGY_DEFAULT,

    /// <summary>
    /// Renders text using Direct2D/DirectWrite. Since Direct2D buffers drawing,
    /// Scintilla's buffering can be turned off with <see cref="Scintilla.BufferedDraw" />.
    /// </summary>
    DirectWrite = NativeMethods.SC_TECHNOLOGY_DIRECTWRITE,

    /// <summary>
    /// Request that the frame is retained after being presented which may prevent drawing failures on some cards and drivers.
    /// </summary>
    DirectWriteRetain = NativeMethods.SC_TECHNOLOGY_DIRECTWRITERETAIN,

    /// <summary>
    /// Use DirectWrite to draw into a GDI DC. This mode may work for remote access sessions.
    /// </summary>
    DirectWriteDc = NativeMethods.SC_TECHNOLOGY_DIRECTWRITEDC,

    /// <summary>
    /// Renders text using Windows 8 DirectWrite 1.1 features in a lower level way that manages graphics state more explicitly.
    /// Technically DirectWrite 1.1 is available on some Windows 7 systems, but it's difficult to detect and not worth the effort. This technology is only available on Windows 8 and later.
    /// </summary>
    DirectWrite1 = NativeMethods.SC_TECHNOLOGY_DIRECT_WRITE_1,
}
