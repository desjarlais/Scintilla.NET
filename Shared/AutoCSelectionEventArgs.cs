using System;
using System.Drawing;
using Scintilla.NET.Abstractions;
using Scintilla.NET.Abstractions.Enumerations;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.AutoCSelection" /> event.
/// </summary>
public class AutoCSelectionEventArgs : AutoCSelectionEventArgsBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoCSelectionEventArgs" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that generated this event.</param>
    /// <param name="bytePosition">The zero-based byte position within the document of the word being completed.</param>
    /// <param name="text">A pointer to the selected auto-completion text.</param>
    /// <param name="ch">The character that caused the completion.</param>
    /// <param name="listCompletionMethod">A value indicating the way in which the completion occurred.</param>
    public AutoCSelectionEventArgs(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla, int bytePosition, IntPtr text, int ch, ListCompletionMethod listCompletionMethod) : base(scintilla, bytePosition, text, ch, listCompletionMethod)
    {
    }
}