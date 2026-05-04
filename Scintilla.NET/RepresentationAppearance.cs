using System;

namespace ScintillaNET;

/// <summary>
/// Visual appearance of representation characters. These are usually control character mnemonics and invalid bytes but can also be custom text associated with a character using <see cref="Scintilla.SetRepresentation"/>.
/// </summary>
[Flags]
public enum RepresentationAppearance : uint
{
    /// <summary>
    /// Draw the representation text with no decorations.
    /// </summary>
    Plain = SciApi.SC_REPRESENTATION_PLAIN,
    
    /// <summary>
    /// Draw the representation text inverted in a rounded rectangle. This is the default appearance.
    /// </summary>
    Blob = SciApi.SC_REPRESENTATION_BLOB,

    /// <summary>
    /// Draw the representation in the colour set with <see cref="SciApi.SCI_SETREPRESENTATIONCOLOUR"/> instead of in the colour of the style of the text being represented.
    /// </summary>
    Colour = SciApi.SC_REPRESENTATION_COLOUR,
}
