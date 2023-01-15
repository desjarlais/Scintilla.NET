using Scintilla.NET.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using Scintilla.NET.Abstractions.Collections;
using Scintilla.NET.Abstractions.UtilityClasses;
using static Scintilla.NET.Abstractions.ScintillaConstants;
using static Scintilla.NET.Abstractions.ScintillaApiStructs;

namespace ScintillaNET;
// TODO Revisit this following Scintilla v3.7.0 because is said to be better about character handling

/// <summary>
/// An immutable collection of lines of text in a <see cref="Scintilla" /> control.
/// </summary>
public class LineCollection : LineCollectionBase<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color>
{
    #region Fields

    // The 'step' is a break in the continuity of our line starts. It allows us
    // to delay the updating of every line start when text is inserted/deleted.
    private int stepLine;
    private int stepLength;

    #endregion Fields

    #region Methods

    private bool LineContainsMultibyteChar(int index)
    {
        var perLine = perLineData[index];
        if (perLine.ContainsMultibyte == ContainsMultibyte.Unkown)
        {
            perLine.ContainsMultibyte =
                (scintilla.DirectMessage(SCI_LINELENGTH, new IntPtr(index)).ToInt32() == CharLineLength(index))
                    ? ContainsMultibyte.No
                    : ContainsMultibyte.Yes;

            perLineData[index] = perLine;
        }

        return (perLine.ContainsMultibyte == ContainsMultibyte.Yes);
    }

    /// <summary>
    /// Returns the line index containing the CHARACTER position.
    /// </summary>
    internal int LineFromCharPosition(int pos)
    {
        Debug.Assert(pos >= 0);

        // Iterative binary search
        // http://en.wikipedia.org/wiki/Binary_search_algorithm
        // System.Collections.Generic.ArraySortHelper.InternalBinarySearch

        var low = 0;
        var high = Count - 1;

        while (low <= high)
        {
            var mid = low + ((high - low) / 2);
            var start = CharPositionFromLine(mid);

            if (pos == start)
                return mid;
            else if (start < pos)
                low = mid + 1;
            else
                high = mid - 1;
        }

        // After while exit, 'low' will point to the index where 'pos' should be
        // inserted (if we were creating a new line start). The line containing
        // 'pos' then would be 'low - 1'.
        return low - 1;
    }

    /// <summary>
    /// Tracks a new line with the given CHARACTER length.
    /// </summary>
    private void InsertPerLine(int index, int length = 0)
    {
        MoveStep(index);

        PerLine data;
        var lineStart = 0;

        // Add the new line length to the existing line start
        data = perLineData[index];
        lineStart = data.Start;
        data.Start += length;
        perLineData[index] = data;

        // Insert the new line
        data = new PerLine { Start = lineStart };
        perLineData.Insert(index, data);

        // Move the step
        stepLength += length;
        stepLine++;
    }

    private void MoveStep(int line)
    {
        if (stepLength == 0)
        {
            stepLine = line;
        }
        else if (stepLine < line)
        {
            PerLine data;
            while (stepLine < line)
            {
                stepLine++;
                data = perLineData[stepLine];
                data.Start += stepLength;
                perLineData[stepLine] = data;
            }
        }
        else if (stepLine > line)
        {
            PerLine data;
            while (stepLine > line)
            {
                data = perLineData[stepLine];
                data.Start -= stepLength;
                perLineData[stepLine] = data;
                stepLine--;
            }
        }
    }

    public override void scintilla_SCNotification(object sender, SCNotificationEventArgs e)
    {
        var scn = e.SCNotification;
        switch (scn.nmhdr.code)
        {
            case SCN_MODIFIED:
                ScnModified(scn);
                break;
        }
    }
    #endregion Methods

    #region Properties
    
    /// <summary>
    /// Gets the <see cref="Line" /> at the specified zero-based index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="Line" /> to get.</param>
    /// <returns>The <see cref="Line" /> at the specified index.</returns>
    public override Line this[int index]
    {
        get
        {
            index = Helpers.Clamp(index, 0, Count - 1);
            return new Line(scintilla, index);
        }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LineCollection" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this collection.</param>
    public LineCollection(IScintillaApi<MarkerCollection, StyleCollection, IndicatorCollection, LineCollection, MarginCollection, SelectionCollection, SCNotificationEventArgs, Marker, Style, Indicator, Line, Margin, Selection, Bitmap, Color> scintilla) : base(scintilla)
    {
        this.scintilla.SCNotification += scintilla_SCNotification;

        this.perLineData = new GapBuffer<PerLine>
        {
            new() { Start = 0 },
            new() { Start = 0 }, // Terminal
        };
    }

    #endregion Constructors
}