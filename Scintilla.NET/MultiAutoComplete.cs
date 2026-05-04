namespace ScintillaNET;

/// <summary>
/// Indicates whether the auto-completed text should be inserted into just the main selection or every selection.
/// </summary>
public enum MultiAutoComplete
{
    /// <summary>
    /// Auto-complete will insert text into the main selection only. This is the default.
    /// </summary>
    Once = SciApi.SC_MULTIAUTOC_ONCE,

    /// <summary>
    /// Auto-complete will insert text into every selection (caret).
    /// </summary>
    Each = SciApi.SC_MULTIAUTOC_EACH,
}
