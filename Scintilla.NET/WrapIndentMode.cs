namespace ScintillaNET;

/// <summary>
/// Indenting behavior of wrapped sublines.
/// </summary>
public enum WrapIndentMode
{
    /// <summary>
    /// Wrapped sublines aligned to left of window plus the amount set by <see cref="ScintillaNET.Scintilla.WrapStartIndent" />.
    /// This is the default.
    /// </summary>
    Fixed = SciApi.SC_WRAPINDENT_FIXED,

    /// <summary>
    /// Wrapped sublines are aligned to first subline indent.
    /// </summary>
    Same = SciApi.SC_WRAPINDENT_SAME,

    /// <summary>
    /// Wrapped sublines are aligned to first subline indent plus one more level of indentation.
    /// </summary>
    Indent = SciApi.SC_WRAPINDENT_INDENT,

    /// <summary>
    /// Wrapped sublines are aligned to first subline indent plus two more levels of indentation.
    /// </summary>
    DeepIndent = SciApi.SC_WRAPINDENT_DEEPINDENT,
}
