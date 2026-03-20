namespace ScintillaNET;

/// <summary>
/// IME interaction mode.
/// </summary>
public enum ImeInteraction
{
    /// <summary>
    /// Windowed IME mode. More similar to other applications.
    /// </summary>
    Windowed = SciApi.SC_IME_WINDOWED,

    /// <summary>
    /// Inline IME mode. Some Scintilla features such as rectangular and multi selection might work better.
    /// </summary>
    Inline = SciApi.SC_IME_INLINE,
}
