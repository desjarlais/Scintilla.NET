using System;

namespace ScintillaNET
{
    /// <summary>
    /// Bit-flags for whether Scintilla should keep track of document change history and in which ways it should display the difference.
    /// </summary>
    [Flags]
    public enum ChangeHistory : int
    {
        /// <summary>
        /// The default: change history turned off.
        /// </summary>
        Disabled = NativeMethods.SC_CHANGE_HISTORY_DISABLED,

        /// <summary>
        /// Track changes to the document.
        /// </summary>
        Enabled = NativeMethods.SC_CHANGE_HISTORY_ENABLED,

        /// <summary>
        /// Display changes in the margin using the SC_MARKNUM_HISTORY markers.
        /// </summary>
        Markers = NativeMethods.SC_CHANGE_HISTORY_MARKERS,

        /// <summary>
        /// Display changes in the text using the INDICATOR_HISTORY indicators.
        /// </summary>
        Indicators = NativeMethods.SC_CHANGE_HISTORY_INDICATORS,
    }
}
