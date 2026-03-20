namespace ScintillaNET;

/// <summary>
/// Specifies the behavior of pasting into multiple selections.
/// </summary>
public enum MultiPaste
{
    /// <summary>
    /// Pasting into multiple selections only pastes to the main selection. This is the default.
    /// </summary>
    Once = SciApi.SC_MULTIPASTE_ONCE,

    /// <summary>
    /// Pasting into multiple selections pastes into each selection.
    /// </summary>
    Each = SciApi.SC_MULTIPASTE_EACH,
}
