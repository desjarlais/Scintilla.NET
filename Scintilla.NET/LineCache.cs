namespace ScintillaNET;

/// <summary>
/// Line layout cache mode.
/// </summary>
public enum LineCache
{
    /// <summary>
    /// No lines are cached.
    /// </summary>
    None = SciApi.SC_CACHE_NONE,

    /// <summary>
    /// One line is cached. This is the default.
    /// </summary>
    Caret = SciApi.SC_CACHE_CARET,

    /// <summary>
    /// Visible lines plus the line containing the caret.
    /// </summary>
    Page = SciApi.SC_CACHE_PAGE,

    /// <summary>
    /// All lines in the document.
    /// </summary>
    Document = SciApi.SC_CACHE_DOCUMENT,
}
