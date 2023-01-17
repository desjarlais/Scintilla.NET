using System;
using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.InsertCheck" /> event.
/// </summary>
public class InsertCheckEventArgs : InsertCheckEventArgsBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InsertCheckEventArgs" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="bytePosition">The zero-based byte position within the document where text is being inserted.</param>
    /// <param name="byteLength">The length in bytes of the inserted text.</param>
    /// <param name="text">A pointer to the text being inserted.</param>
    public InsertCheckEventArgs(
        IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection,
            SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap,
            Color> scintilla, int bytePosition, int byteLength, IntPtr text) : base(scintilla, bytePosition, byteLength,
        text)
    {
    }
}