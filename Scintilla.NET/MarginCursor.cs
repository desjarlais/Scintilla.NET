namespace ScintillaNET;

/// <summary>
/// The display of a cursor when over a margin.
/// </summary>
public enum MarginCursor
{
    /// <summary>
    /// A normal arrow.
    /// </summary>
    Arrow = SciApi.SC_CURSORARROW,

    /// <summary>
    /// A reversed arrow.
    /// </summary>
    ReverseArrow = SciApi.SC_CURSORREVERSEARROW,
}
