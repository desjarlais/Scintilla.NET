using System;

namespace ScintillaNET;

/// <summary>
/// Additional display options for folds.
/// </summary>
[Flags]
public enum FoldFlags : uint
{
    /// <summary>
    /// Default value.
    /// </summary>
    None = SciApi.SC_FOLDFLAG_NONE,

    /// <summary>
    /// A line is drawn above if expanded.
    /// </summary>
    LineBeforeExpanded = SciApi.SC_FOLDFLAG_LINEBEFORE_EXPANDED,

    /// <summary>
    /// A line is drawn above if not expanded.
    /// </summary>
    LineBeforeContracted = SciApi.SC_FOLDFLAG_LINEBEFORE_CONTRACTED,

    /// <summary>
    /// A line is drawn below if expanded.
    /// </summary>
    LineAfterExpanded = SciApi.SC_FOLDFLAG_LINEAFTER_EXPANDED,

    /// <summary>
    /// A line is drawn below if not expanded.
    /// </summary>
    LineAfterContracted = SciApi.SC_FOLDFLAG_LINEAFTER_CONTRACTED,

    /// <summary>
    /// Displays the hexadecimal fold levels in the margin to aid with debugging.
    /// This feature may change in the future.
    /// </summary>
    LevelNumbers = SciApi.SC_FOLDFLAG_LEVELNUMBERS,

    /// <summary>
    /// Displays the hexadecimal line state in the margin to aid with debugging. This flag
    /// cannot be used at the same time as the <see cref="LevelNumbers" /> flag.
    /// </summary>
    LineState = SciApi.SC_FOLDFLAG_LINESTATE,
}
