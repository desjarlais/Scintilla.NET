using System;

namespace ScintillaNET;

/// <summary>
/// Document options that can be used in <see cref="Scintilla.CreateDocument"/>.
/// </summary>
[Flags]
public enum DocumentOption : uint
{
    /// <summary>
    /// Standard behaviour.
    /// </summary>
    Default = SciApi.SC_DOCUMENTOPTION_DEFAULT,

    /// <summary>
    /// Stop allocation of memory for styles and treat all text as style 0.
    /// </summary>
    StylesNone = SciApi.SC_DOCUMENTOPTION_STYLES_NONE,

    /// <summary>
    /// Allow document to be larger than 2 GB.
    /// </summary>
    TextLarge = SciApi.SC_DOCUMENTOPTION_TEXT_LARGE,
}
