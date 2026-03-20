using System;

namespace ScintillaNET;

/// <summary>
/// Specifies how <see cref="SciApi.SCI_ENSUREVISIBLEENFORCEPOLICY"/> makes a line visible. Works similar to <see cref="CaretPolicy"/>.
/// </summary>
[Flags]
public enum VisiblePolicy : uint
{
    /// <summary>
    /// No special handling while making a line visible using <see cref="SciApi.SCI_ENSUREVISIBLEENFORCEPOLICY"/>.
    /// </summary>
    None = 0,

    /// <summary>
    /// If set, we can define a slop value: visibleSlop. This value defines an unwanted zone (UZ) where the line is... unwanted. This zone is defined as a number of lines near the horizontal margins. By keeping the line away from the edges, it is seen within its context. This makes it likely that the current line is seen with some of the lines following it, which are often dependent on that line.
    /// </summary>
    Slop = SciApi.VISIBLE_SLOP,

    /// <summary>
    /// If set, the policy set by <see cref="Slop"/> is enforced... strictly. The line is centred on the display if visibleSlop is not set, and cannot go in the UZ if visibleSlop is set.
    /// </summary>
    Strict = SciApi.VISIBLE_STRICT,
}
