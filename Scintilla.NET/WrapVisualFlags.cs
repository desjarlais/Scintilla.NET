using System;

namespace ScintillaNET;

/// <summary>
/// The visual indicator used on a wrapped line.
/// </summary>
[Flags]
public enum WrapVisualFlags : uint
{
    /// <summary>
    /// No visual indicator is displayed. This the default.
    /// </summary>
    None = SciApi.SC_WRAPVISUALFLAG_NONE,

    /// <summary>
    /// A visual indicator is displayed at th end of a wrapped subline.
    /// </summary>
    End = SciApi.SC_WRAPVISUALFLAG_END,

    /// <summary>
    /// A visual indicator is displayed at the beginning of a subline.
    /// The subline is indented by 1 pixel to make room for the display.
    /// </summary>
    Start = SciApi.SC_WRAPVISUALFLAG_START,

    /// <summary>
    /// A visual indicator is displayed in the number margin.
    /// </summary>
    Margin = SciApi.SC_WRAPVISUALFLAG_MARGIN,
}
