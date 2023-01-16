﻿using System;
using System.Drawing;
using Scintilla.NET.Abstractions;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.NeedShown" /> event.
/// </summary>
public class NeedShownEventArgs : EventArgs
{
    private readonly IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla;
    private readonly int bytePosition;
    private readonly int byteLength;
    private int? position;
    private int? length;

    /// <summary>
    /// Gets the length of the text that needs to be shown.
    /// </summary>
    /// <returns>The length of text starting at <see cref="Position" /> that needs to be shown.</returns>
    public int Length
    {
        get
        {
            if (length == null)
            {
                var endBytePosition = (bytePosition + byteLength);
                var endPosition = scintilla.Lines.ByteToCharPosition(endBytePosition);
                length = (endPosition - Position);
            }

            return (int)length;
        }
    }

    /// <summary>
    /// Gets the zero-based document position where text needs to be shown.
    /// </summary>
    /// <returns>The zero-based document position where the range of text to be shown starts.</returns>
    public int Position
    {
        get
        {
            if (position == null)
                position = scintilla.Lines.ByteToCharPosition(bytePosition);

            return (int)position;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NeedShownEventArgs" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="bytePosition">The zero-based byte position within the document where text needs to be shown.</param>
    /// <param name="byteLength">The length in bytes of the text that needs to be shown.</param>
    public NeedShownEventArgs(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla, int bytePosition, int byteLength)
    {
        this.scintilla = scintilla;
        this.bytePosition = bytePosition;
        this.byteLength = byteLength;
    }
}