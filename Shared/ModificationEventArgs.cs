using System;
using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.Enumerations;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.Insert" /> and <see cref="Scintilla.Delete" /> events.
/// </summary>
public class ModificationEventArgs : BeforeModificationEventArgsBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    /// <summary>
    /// Gets the number of lines added or removed.
    /// </summary>
    /// <returns>The number of lines added to the document when text is inserted, or the number of lines removed from the document when text is deleted.</returns>
    /// <remarks>When lines are deleted the return value will be negative.</remarks>
    public int LinesAdded { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModificationEventArgs" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="source">The source of the modification.</param>
    /// <param name="bytePosition">The zero-based byte position within the document where text was modified.</param>
    /// <param name="byteLength">The length in bytes of the inserted or deleted text.</param>
    /// <param name="text">>A pointer to the text inserted or deleted.</param>
    /// <param name="linesAdded">The number of lines added or removed (delta).</param>
    public ModificationEventArgs(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla, ModificationSource source, int bytePosition, int byteLength, IntPtr text, int linesAdded) : base(scintilla, source, bytePosition, byteLength, text)
    {
        LinesAdded = linesAdded;
    }
}