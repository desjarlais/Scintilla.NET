using System;

namespace ScintillaNET;

/// <summary>
/// Specifies the change that triggered a <see cref="Scintilla.UpdateUI" /> event.
/// </summary>
/// <remarks>This enumeration has a FlagsAttribute attribute that allows a bitwise combination of its member values.</remarks>
[Flags]
public enum UpdateChange : uint
{
    /// <summary>
    /// Value without any changes.
    /// </summary>
    None = SciApi.SC_UPDATE_NONE,

    /// <summary>
    /// Contents, styling or markers may have been changed.
    /// </summary>
    Content = SciApi.SC_UPDATE_CONTENT,

    /// <summary>
    /// Selection may have been changed.
    /// </summary>
    Selection = SciApi.SC_UPDATE_SELECTION,

    /// <summary>
    /// May have scrolled vertically.
    /// </summary>
    VScroll = SciApi.SC_UPDATE_V_SCROLL,

    /// <summary>
    /// May have scrolled horizontally.
    /// </summary>
    HScroll = SciApi.SC_UPDATE_H_SCROLL,
}
