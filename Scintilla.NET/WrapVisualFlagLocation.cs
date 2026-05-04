using System;

namespace ScintillaNET;

/// <summary>
/// Additional location options for line wrapping visual indicators.
/// </summary>
[Flags]
public enum WrapVisualFlagLocation : uint
{
    /// <summary>
    /// Wrap indicators are drawn near the border. This is the default.
    /// </summary>
    Default = SciApi.SC_WRAPVISUALFLAGLOC_DEFAULT,

    /// <summary>
    /// Wrap indicators are drawn at the end of sublines near the text.
    /// </summary>
    EndByText = SciApi.SC_WRAPVISUALFLAGLOC_END_BY_TEXT,

    /// <summary>
    /// Wrap indicators are drawn at the beginning of sublines near the text.
    /// </summary>
    StartByText = SciApi.SC_WRAPVISUALFLAGLOC_START_BY_TEXT,
}
