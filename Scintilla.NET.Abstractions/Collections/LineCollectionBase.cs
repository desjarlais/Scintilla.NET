using System.Collections;
using System.Diagnostics;
using Scintilla.NET.Abstractions.UtilityClasses;
using static Scintilla.NET.Abstractions.ScintillaConstants;
using static Scintilla.NET.Abstractions.ScintillaApiStructs;

namespace Scintilla.NET.Abstractions.Collections;
// TODO Revisit this following Scintilla v3.7.0 because is said to be better about character handling

/// <summary>
/// An immutable collection of lines of text in a <see cref="Scintilla" /> control.
/// </summary>
public abstract class LineCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> : IEnumerable<TLine>
    where TMarkers : MarkerCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TStyles : StyleCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TIndicators :IndicatorCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TLines : LineCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TMargins : MarginCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TSelections : SelectionCollectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>, IEnumerable
    where TEventArgs : System.EventArgs
    where TMarker: MarkerBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TStyle : StyleBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TIndicator : IndicatorBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TLine : LineBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TMargin : MarginBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TSelection : SelectionBase<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor>
    where TBitmap: class
    where TColor: struct
{
    #region Fields

    /// <summary>
    /// A reference to the Scintilla control interface.
    /// </summary>
    protected readonly IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla;
    protected GapBuffer<PerLine> perLineData;

    // The 'step' is a break in the continuity of our line starts. It allows us
    // to delay the updating of every line start when text is inserted/deleted.
    protected int stepLine;
    protected int stepLength;

    #endregion Fields

    #region Methods

    /// <summary>
    /// Adjust the number of CHARACTERS in a line.
    /// </summary>
    public virtual void AdjustLineLength(int index, int delta)
    {
        MoveStep(index);
        stepLength += delta;

        // Invalidate multibyte flag
        var perLine = perLineData[index];
        perLine.ContainsMultibyte = ContainsMultibyte.Unkown;
        perLineData[index] = perLine;
    }

    /// <summary>
    /// Converts a BYTE offset to a CHARACTER offset.
    /// </summary>
    public virtual int ByteToCharPosition(int pos)
    {
        Debug.Assert(pos >= 0);
        Debug.Assert(pos <= scintilla.DirectMessage(SCI_GETLENGTH).ToInt32());

        var line = scintilla.DirectMessage(SCI_LINEFROMPOSITION, new IntPtr(pos)).ToInt32();
        var byteStart = scintilla.DirectMessage(SCI_POSITIONFROMLINE, new IntPtr(line)).ToInt32();
        var count = CharPositionFromLine(line) + GetCharCount(byteStart, pos - byteStart);

        return count;
    }

    /// <summary>
    /// Returns the number of CHARACTERS in a line.
    /// </summary>
    public virtual int CharLineLength(int index)
    {
        Debug.Assert(index >= 0);
        Debug.Assert(index < Count);

        // A line's length is calculated by subtracting its start offset from
        // the start of the line following. We keep a terminal (faux) line at
        // the end of the list so we can calculate the length of the last line.

        if (index + 1 <= stepLine)
            return perLineData[index + 1].Start - perLineData[index].Start;
        else if (index <= stepLine)
            return (perLineData[index + 1].Start + stepLength) - perLineData[index].Start;
        else
            return (perLineData[index + 1].Start + stepLength) - (perLineData[index].Start + stepLength);
    }

    /// <summary>
    /// Returns the CHARACTER offset where the line begins.
    /// </summary>
    public virtual int CharPositionFromLine(int index)
    {
        Debug.Assert(index >= 0);
        Debug.Assert(index < perLineData.Count); // Allow query of terminal line start

        var start = perLineData[index].Start;
        if (index > stepLine)
            start += stepLength;

        return start;
    }

    public virtual int CharToBytePosition(int pos)
    {
        Debug.Assert(pos >= 0);
        Debug.Assert(pos <= TextLength);

        // Adjust to the nearest line start
        var line = LineFromCharPosition(pos);
        var bytePos = scintilla.DirectMessage(SCI_POSITIONFROMLINE, new IntPtr(line)).ToInt32();
        pos -= CharPositionFromLine(line);

        // Optimization when the line contains NO multibyte characters
        if (!LineContainsMultibyteChar(line))
            return (bytePos + pos);

        while (pos > 0)
        {
            // Move char-by-char
            bytePos = scintilla.DirectMessage(SCI_POSITIONRELATIVE, new IntPtr(bytePos), new IntPtr(1)).ToInt32();
            pos--;
        }

        return bytePos;
    }

    public virtual void DeletePerLine(int index)
    {
        Debug.Assert(index != 0);

        MoveStep(index);

        // Subtract the line length
        stepLength -= CharLineLength(index);

        // Remove the line
        perLineData.RemoveAt(index);

        // Move the step to the line before the one removed
        stepLine--;
    }

    #region DebugMethods
#if DEBUG

    /// <summary>
    /// Dumps the line buffer to a string.
    /// </summary>
    /// <returns>A string representing the line buffer.</returns>
    public string Dump()
    {
        using (var writer = new StringWriter())
        {
            scintilla.Lines.Dump(writer);
            return writer.ToString();
        }
    }

    /// <summary>
    /// Dumps the line buffer to the specified TextWriter.
    /// </summary>
    /// <param name="writer">The writer to use for dumping the line buffer.</param>
    public unsafe void Dump(TextWriter writer)
    {
        var totalChars = 0;

        for (int i = 0; i < perLineData.Count; i++)
        {
            var error = totalChars == CharPositionFromLine(i) ? null : "*";
            if (i == perLineData.Count - 1)
            {
                writer.WriteLine("{0}[{1}] {2} (terminal)", error, i, CharPositionFromLine(i));
            }
            else
            {
                var len = scintilla.DirectMessage(SCI_GETLINE, new IntPtr(i)).ToInt32();
                var bytes = new byte[len];

                fixed (byte* ptr = bytes)
                    scintilla.DirectMessage(SCI_GETLINE, new IntPtr(i), new IntPtr(ptr));

                var str = scintilla.Encoding.GetString(bytes);
                var containsMultibyte = "U";
                if (perLineData[i].ContainsMultibyte == ContainsMultibyte.Yes)
                    containsMultibyte = "Y";
                else if (perLineData[i].ContainsMultibyte == ContainsMultibyte.No)
                    containsMultibyte = "N";

                writer.WriteLine("{0}[{1}] {2}:{3}:{4} {5}", error, i, CharPositionFromLine(i), str.Length, containsMultibyte, str.Replace("\r", "\\r").Replace("\n", "\\n"));
                totalChars += str.Length;
            }
        }
    }

#endif
    #endregion

    /// <summary>
    /// Gets the number of CHARACTERS int a BYTE range.
    /// </summary>
    public virtual int GetCharCount(int pos, int length)
    {
        var ptr = scintilla.DirectMessage(SCI_GETRANGEPOINTER, new IntPtr(pos), new IntPtr(length));
        return HelpersGeneral.GetCharCount(ptr, length, scintilla.Encoding);
    }

    /// <summary>
    /// Provides an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An object that contains all <see cref="LineBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> objects within the <see cref="LineCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" />.</returns>
    public IEnumerator<TLine> GetEnumerator()
    {
        int count = Count;
        for (int i = 0; i < count; i++)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public virtual bool LineContainsMultibyteChar(int index)
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
    public virtual int LineFromCharPosition(int pos)
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
    public virtual void InsertPerLine(int index, int length = 0)
    {
        MoveStep(index);

        PerLine data;

        // Add the new line length to the existing line start
        data = perLineData[index];
        var lineStart = data.Start;
        data.Start += length;
        perLineData[index] = data;

        // Insert the new line
        data = new PerLine { Start = lineStart };
        perLineData.Insert(index, data);

        // Move the step
        stepLength += length;
        stepLine++;
    }

    public virtual void MoveStep(int line)
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

    public virtual void RebuildLineData()
    {
        stepLine = 0;
        stepLength = 0;

        perLineData = new GapBuffer<PerLine>();
        perLineData.Add(new PerLine { Start = 0 });
        perLineData.Add(new PerLine { Start = 0 }); // Terminal

        // Fake an insert notification
        var scn = new SCNotification();
        var adjustedLines = scintilla.DirectMessage(SCI_GETLINECOUNT).ToInt32() - 1;
        scn.linesAdded = new IntPtr(adjustedLines);
        scn.position = IntPtr.Zero;
        scn.length = scintilla.DirectMessage(SCI_GETLENGTH);
        scn.text = scintilla.DirectMessage(SCI_GETRANGEPOINTER, scn.position, scn.length);
        TrackInsertText(scn);
    }

    public abstract void scintilla_SCNotification(object sender, TEventArgs e);

    public virtual void ScnModified(SCNotification scn)
    {
        if ((scn.modificationType & SC_MOD_DELETETEXT) > 0)
        {
            TrackDeleteText(scn);
        }

        if ((scn.modificationType & SC_MOD_INSERTTEXT) > 0)
        {
            TrackInsertText(scn);
        }
    }

    public virtual void TrackDeleteText(SCNotification scn)
    {
        var startLine = scintilla.DirectMessage(SCI_LINEFROMPOSITION, scn.position).ToInt32();
        if (scn.linesAdded == IntPtr.Zero)
        {
            // That was easy
            var delta = HelpersGeneral.GetCharCount(scn.text, scn.length.ToInt32(), scintilla.Encoding);
            AdjustLineLength(startLine, delta * -1);
        }
        else
        {
            // Adjust the existing line
            var lineByteStart = scintilla.DirectMessage(SCI_POSITIONFROMLINE, new IntPtr(startLine)).ToInt32();
            var lineByteLength = scintilla.DirectMessage(SCI_LINELENGTH, new IntPtr(startLine)).ToInt32();
            AdjustLineLength(startLine, GetCharCount(lineByteStart, lineByteLength) - CharLineLength(startLine));

            var linesRemoved = scn.linesAdded.ToInt32() * -1;
            for (int i = 0; i < linesRemoved; i++)
            {
                // Deleted line
                DeletePerLine(startLine + 1);
            }
        }
    }

    public virtual void TrackInsertText(SCNotification scn)
    {
        var startLine = scintilla.DirectMessage(SCI_LINEFROMPOSITION, scn.position).ToInt32();
        if (scn.linesAdded == IntPtr.Zero)
        {
            // That was easy
            var delta = GetCharCount(scn.position.ToInt32(), scn.length.ToInt32());
            AdjustLineLength(startLine, delta);
        }
        else
        {
            // Adjust existing line
            var lineByteStart = scintilla.DirectMessage(SCI_POSITIONFROMLINE, new IntPtr(startLine)).ToInt32();
            var lineByteLength = scintilla.DirectMessage(SCI_LINELENGTH, new IntPtr(startLine)).ToInt32();
            AdjustLineLength(startLine, GetCharCount(lineByteStart, lineByteLength) - CharLineLength(startLine));

            for (int i = 1; i <= scn.linesAdded.ToInt32(); i++)
            {
                var line = startLine + i;

                // Insert new line
                lineByteStart += lineByteLength;
                lineByteLength = scintilla.DirectMessage(SCI_LINELENGTH, new IntPtr(line)).ToInt32();
                InsertPerLine(line, GetCharCount(lineByteStart, lineByteLength));
            }
        }
    }

    #endregion Methods

    #region Properties

    /// <summary>
    /// Gets a value indicating whether all the document lines are visible (not hidden).
    /// </summary>
    /// <returns>true if all the lines are visible; otherwise, false.</returns>
    public virtual bool AllLinesVisible
    {
        get
        {
            return (scintilla.DirectMessage(SCI_GETALLLINESVISIBLE) != IntPtr.Zero);
        }
    }

    /// <summary>
    /// Gets the number of lines.
    /// </summary>
    /// <returns>The number of lines in the <see cref="LineCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" />.</returns>
    public virtual int Count
    {
        get
        {
            // Subtract the terminal line
            return (perLineData.Count - 1);
        }
    }

    /// <summary>
    /// Gets the number of CHARACTERS in the document.
    /// </summary>
    public virtual int TextLength
    {
        get
        {
            // Where the terminal line begins
            return CharPositionFromLine(perLineData.Count - 1);
        }
    }

    /// <summary>
    /// Gets the <see cref="LineBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> at the specified zero-based index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="LineBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> to get.</param>
    /// <returns>The <see cref="LineBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> at the specified index.</returns>
    public abstract TLine this[int index] { get; }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LineCollectionBase{TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor}" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this collection.</param>
    public LineCollectionBase(IScintillaApi<TMarkers, TStyles, TIndicators, TLines, TMargins, TSelections, TEventArgs, TMarker, TStyle, TIndicator, TLine, TMargin, TSelection, TBitmap, TColor> scintilla)
    {
        this.scintilla = scintilla;
        this.scintilla.SCNotification += scintilla_SCNotification;

        this.perLineData = new GapBuffer<PerLine>();
        this.perLineData.Add(new PerLine { Start = 0 });
        this.perLineData.Add(new PerLine { Start = 0 }); // Terminal
    }

    #endregion Constructors

    #region Types

    /// <summary>
    /// Stuff we track for each line.
    /// </summary>
    public struct PerLine
    {
        /// <summary>
        /// The CHARACTER position where the line begins.
        /// </summary>
        public int Start;

        /// <summary>
        /// 1 if the line contains multibyte (Unicode) characters; -1 if not; 0 if undetermined.
        /// </summary>
        /// <remarks>Using an enum instead of Nullable because it uses less memory per line...</remarks>
        public ContainsMultibyte ContainsMultibyte;
    }

    public enum ContainsMultibyte
    {
        No = -1,
        Unkown,
        Yes
    }

    #endregion Types
}