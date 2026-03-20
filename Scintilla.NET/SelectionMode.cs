namespace ScintillaNET;

/// <summary>
/// Text selection mode.
/// </summary>
public enum SelectionMode
{
    /// <summary>
    /// Stream selection mode.
    /// </summary>
    Stream = SciApi.SC_SEL_STREAM,

    /// <summary>
    /// Rectangular selection mode.
    /// </summary>
    Rectangle = SciApi.SC_SEL_RECTANGLE,

    /// <summary>
    /// Lines selection mode.
    /// </summary>
    Lines = SciApi.SC_SEL_LINES,

    /// <summary>
    /// Thin rectangular selection mode.
    /// </summary>
    Thin = SciApi.SC_SEL_THIN,
}
