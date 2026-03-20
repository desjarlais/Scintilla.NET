namespace ScintillaNET;

/// <summary>
/// Display options for fold text tags.
/// </summary>
public enum FoldDisplayText
{
    /// <summary>
    /// Do not display the text tags. This is the default.
    /// </summary>
    Hidden = SciApi.SC_FOLDDISPLAYTEXT_HIDDEN,

    /// <summary>
    /// Display the text tags.
    /// </summary>
    Standard = SciApi.SC_FOLDDISPLAYTEXT_STANDARD,

    /// <summary>
    /// Display the text tags with a box drawn around them.
    /// </summary>
    Boxed = SciApi.SC_FOLDDISPLAYTEXT_BOXED,
}
