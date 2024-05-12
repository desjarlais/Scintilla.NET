using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ScintillaNET;
// TODO Revisit this following Scintilla v3.7.0 because is said to be better about character handling

/// <summary>
/// An immutable collection of lines of text in a <see cref="Scintilla" /> control.
/// </summary>
public class LineCollection : IEnumerable<Line>
{
    #region Fields

    private readonly Scintilla scintilla;
    private GapBuffer<PerLine> perLineData;

    // The 'step' is a break in the continuity of our line starts. It allows us
    // to delay the updating of every line start when text is inserted/deleted.
    private int stepLine;
    private int stepLength;

    #endregion Fields

    #region Methods

    /// <summary>
    /// Adjust the number of CHARACTERS in a line.
    /// </summary>
    private void AdjustLineLength(int index, int delta)
    {
        MoveStep(index);
        this.stepLength += delta;

        // Invalidate multibyte flag
        PerLine perLine = this.perLineData[index];
        perLine.ContainsMultibyte = ContainsMultibyte.Unkown;
        this.perLineData[index] = perLine;
    }

    /// <summary>
    /// Converts a BYTE offset to a CHARACTER offset.
    /// </summary>
    internal int ByteToCharPosition(int pos)
    {
        Debug.Assert(pos >= 0);
        Debug.Assert(pos <= this.scintilla.DirectMessage(NativeMethods.SCI_GETLENGTH).ToInt32());

        int line = this.scintilla.DirectMessage(NativeMethods.SCI_LINEFROMPOSITION, new IntPtr(pos)).ToInt32();
        int byteStart = this.scintilla.DirectMessage(NativeMethods.SCI_POSITIONFROMLINE, new IntPtr(line)).ToInt32();
        int count = CharPositionFromLine(line) + GetCharCount(byteStart, pos - byteStart);

        return count;
    }

    /// <summary>
    /// Returns the number of CHARACTERS in a line.
    /// </summary>
    internal int CharLineLength(int index)
    {
        Debug.Assert(index >= 0);
        Debug.Assert(index < Count);

        // A line's length is calculated by subtracting its start offset from
        // the start of the line following. We keep a terminal (faux) line at
        // the end of the list so we can calculate the length of the last line.

        if (index + 1 <= this.stepLine)
            return this.perLineData[index + 1].Start - this.perLineData[index].Start;
        else if (index <= this.stepLine)
            return this.perLineData[index + 1].Start + this.stepLength - this.perLineData[index].Start;
        else
            return this.perLineData[index + 1].Start + this.stepLength - (this.perLineData[index].Start + this.stepLength);
    }

    /// <summary>
    /// Returns the CHARACTER offset where the line begins.
    /// </summary>
    internal int CharPositionFromLine(int index)
    {
        Debug.Assert(index >= 0);
        Debug.Assert(index < this.perLineData.Count); // Allow query of terminal line start

        int start = this.perLineData[index].Start;
        if (index > this.stepLine)
            start += this.stepLength;

        return start;
    }

    internal int CharToWideBytePosition(int pos)
    {
        Debug.Assert(pos >= 0);
        Debug.Assert(pos <= TextLength);

        // Adjust to the nearest line start
        int line = LineFromCharPosition(pos);
        int bytePos = this.scintilla.DirectMessage(NativeMethods.SCI_POSITIONFROMLINE, new IntPtr(line)).ToInt32();
        pos -= CharPositionFromLine(line);

        // Optimization when the line contains NO multibyte characters
        if (!LineContainsMultibyteChar(line))
            return bytePos + pos;

        int prevBytePos;
        while (pos > 0)
        {
            // hang onto the prev byte position so we can determine if we are single or multi byte
            prevBytePos = bytePos;
            bytePos = this.scintilla.DirectMessage(NativeMethods.SCI_POSITIONRELATIVE, new IntPtr(bytePos), new IntPtr(1)).ToInt32();

            if (bytePos - prevBytePos == 1)
            {
                // if the byte position is 1, we are single byte
                pos--;
            }
            else
            {
                // if the byte position > 1, we are multi byte
                pos -= 2;
            }
        }

        return bytePos;
    }

    internal int CharToBytePosition(int pos)
    {
        Debug.Assert(pos >= 0);
        Debug.Assert(pos <= TextLength);

        // Adjust to the nearest line start
        int line = LineFromCharPosition(pos);
        int bytePos = this.scintilla.DirectMessage(NativeMethods.SCI_POSITIONFROMLINE, new IntPtr(line)).ToInt32();
        pos -= CharPositionFromLine(line);

        // Optimization when the line contains NO multibyte characters
        if (!LineContainsMultibyteChar(line))
            return bytePos + pos;

        while (pos > 0)
        {
            // Move char-by-char
            bytePos = this.scintilla.DirectMessage(NativeMethods.SCI_POSITIONRELATIVE, new IntPtr(bytePos),
                new IntPtr(1)).ToInt32();
            pos--;
        }

        return bytePos;
    }

    private void DeletePerLine(int index)
    {
        Debug.Assert(index != 0);

        MoveStep(index);

        // Subtract the line length
        this.stepLength -= CharLineLength(index);

        // Remove the line
        this.perLineData.RemoveAt(index);

        // Move the step to the line before the one removed
        this.stepLine--;
    }

#if DEBUG

    /// <summary>
    /// Dumps the line buffer to a string.
    /// </summary>
    /// <returns>A string representing the line buffer.</returns>
    public string Dump()
    {
        using var writer = new StringWriter();
        this.scintilla.Lines.Dump(writer);
        return writer.ToString();
    }

    /// <summary>
    /// Dumps the line buffer to the specified TextWriter.
    /// </summary>
    /// <param name="writer">The writer to use for dumping the line buffer.</param>
    public unsafe void Dump(TextWriter writer)
    {
        int totalChars = 0;

        for (int i = 0; i < this.perLineData.Count; i++)
        {
            string error = totalChars == CharPositionFromLine(i) ? null : "*";
            if (i == this.perLineData.Count - 1)
            {
                writer.WriteLine("{0}[{1}] {2} (terminal)", error, i, CharPositionFromLine(i));
            }
            else
            {
                int len = this.scintilla.DirectMessage(NativeMethods.SCI_GETLINE, new IntPtr(i)).ToInt32();
                byte[] bytes = new byte[len];

                fixed (byte* ptr = bytes)
                    this.scintilla.DirectMessage(NativeMethods.SCI_GETLINE, new IntPtr(i), new IntPtr(ptr));

                string str = this.scintilla.Encoding.GetString(bytes);
                string containsMultibyte = "U";
                if (this.perLineData[i].ContainsMultibyte == ContainsMultibyte.Yes)
                    containsMultibyte = "Y";
                else if (this.perLineData[i].ContainsMultibyte == ContainsMultibyte.No)
                    containsMultibyte = "N";

                writer.WriteLine("{0}[{1}] {2}:{3}:{4} {5}", error, i, CharPositionFromLine(i), str.Length, containsMultibyte, str.Replace("\r", "\\r").Replace("\n", "\\n"));
                totalChars += str.Length;
            }
        }
    }

#endif

    /// <summary>
    /// Gets the number of CHARACTERS int a BYTE range.
    /// </summary>
    private int GetCharCount(int pos, int length)
    {
        IntPtr ptr = this.scintilla.DirectMessage(NativeMethods.SCI_GETRANGEPOINTER, new IntPtr(pos), new IntPtr(length));
        return GetCharCount(ptr, length, this.scintilla.Encoding);
    }

    /// <summary>
    /// Gets the number of CHARACTERS in a BYTE range.
    /// </summary>
    private static unsafe int GetCharCount(IntPtr text, int length, Encoding encoding)
    {
        if (text == IntPtr.Zero || length == 0)
            return 0;

        // Never use SCI_COUNTCHARACTERS. It counts CRLF as 1 char!
        int count = encoding.GetCharCount((byte*)text, length);
        return count;
    }

    /// <summary>
    /// Provides an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An object that contains all <see cref="Line" /> objects within the <see cref="LineCollection" />.</returns>
    public IEnumerator<Line> GetEnumerator()
    {
        int count = Count;
        for (int i = 0; i < count; i++)
            yield return this[i];

        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private bool LineContainsMultibyteChar(int index)
    {
        PerLine perLine = this.perLineData[index];
        if (perLine.ContainsMultibyte == ContainsMultibyte.Unkown)
        {
            perLine.ContainsMultibyte =
                (this.scintilla.DirectMessage(NativeMethods.SCI_LINELENGTH, new IntPtr(index)).ToInt32() == CharLineLength(index))
                    ? ContainsMultibyte.No
                    : ContainsMultibyte.Yes;

            this.perLineData[index] = perLine;
        }

        return perLine.ContainsMultibyte == ContainsMultibyte.Yes;
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

        int low = 0;
        int high = Count - 1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;
            int start = CharPositionFromLine(mid);

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
        int lineStart = 0;

        // Add the new line length to the existing line start
        data = this.perLineData[index];
        lineStart = data.Start;
        data.Start += length;
        this.perLineData[index] = data;

        // Insert the new line
        data = new PerLine { Start = lineStart };
        this.perLineData.Insert(index, data);

        // Move the step
        this.stepLength += length;
        this.stepLine++;
    }

    private void MoveStep(int line)
    {
        if (this.stepLength == 0)
        {
            this.stepLine = line;
        }
        else if (this.stepLine < line)
        {
            PerLine data;
            while (this.stepLine < line)
            {
                this.stepLine++;
                data = this.perLineData[this.stepLine];
                data.Start += this.stepLength;
                this.perLineData[this.stepLine] = data;
            }
        }
        else if (this.stepLine > line)
        {
            PerLine data;
            while (this.stepLine > line)
            {
                data = this.perLineData[this.stepLine];
                data.Start -= this.stepLength;
                this.perLineData[this.stepLine] = data;
                this.stepLine--;
            }
        }
    }

    internal void RebuildLineData()
    {
        this.stepLine = 0;
        this.stepLength = 0;

        this.perLineData =
        [
            new PerLine { Start = 0 },
            new PerLine { Start = 0 }, // Terminal
        ];

        // Fake an insert notification
        var scn = new NativeMethods.SCNotification();
        int adjustedLines = this.scintilla.DirectMessage(NativeMethods.SCI_GETLINECOUNT).ToInt32() - 1;
        scn.linesAdded = new IntPtr(adjustedLines);
        scn.position = IntPtr.Zero;
        scn.length = this.scintilla.DirectMessage(NativeMethods.SCI_GETLENGTH);
        scn.text = this.scintilla.DirectMessage(NativeMethods.SCI_GETRANGEPOINTER, scn.position, scn.length);
        TrackInsertText(scn);
    }

    private void scintilla_SCNotification(object sender, SCNotificationEventArgs e)
    {
        NativeMethods.SCNotification scn = e.SCNotification;
        switch (scn.nmhdr.code)
        {
            case NativeMethods.SCN_MODIFIED:
                ScnModified(scn);
                break;
        }
    }

    private void ScnModified(NativeMethods.SCNotification scn)
    {
        if ((scn.modificationType & NativeMethods.SC_MOD_DELETETEXT) > 0)
        {
            TrackDeleteText(scn);
        }

        if ((scn.modificationType & NativeMethods.SC_MOD_INSERTTEXT) > 0)
        {
            TrackInsertText(scn);
        }
    }

    private void TrackDeleteText(NativeMethods.SCNotification scn)
    {
        int startLine = this.scintilla.DirectMessage(NativeMethods.SCI_LINEFROMPOSITION, scn.position).ToInt32();
        if (scn.linesAdded == IntPtr.Zero)
        {
            // That was easy
            int delta = GetCharCount(scn.text, scn.length.ToInt32(), this.scintilla.Encoding);
            AdjustLineLength(startLine, delta * -1);
        }
        else
        {
            // Adjust the existing line
            int lineByteStart = this.scintilla.DirectMessage(NativeMethods.SCI_POSITIONFROMLINE, new IntPtr(startLine)).ToInt32();
            int lineByteLength = this.scintilla.DirectMessage(NativeMethods.SCI_LINELENGTH, new IntPtr(startLine)).ToInt32();
            AdjustLineLength(startLine, GetCharCount(lineByteStart, lineByteLength) - CharLineLength(startLine));

            int linesRemoved = scn.linesAdded.ToInt32() * -1;
            for (int i = 0; i < linesRemoved; i++)
            {
                // Deleted line
                DeletePerLine(startLine + 1);
            }
        }
    }

    private void TrackInsertText(NativeMethods.SCNotification scn)
    {
        int startLine = this.scintilla.DirectMessage(NativeMethods.SCI_LINEFROMPOSITION, scn.position).ToInt32();
        if (scn.linesAdded == IntPtr.Zero)
        {
            // That was easy
            int delta = GetCharCount(scn.position.ToInt32(), scn.length.ToInt32());
            AdjustLineLength(startLine, delta);
        }
        else
        {
            int lineByteStart = 0;
            int lineByteLength = 0;

            // Adjust existing line
            lineByteStart = this.scintilla.DirectMessage(NativeMethods.SCI_POSITIONFROMLINE, new IntPtr(startLine)).ToInt32();
            lineByteLength = this.scintilla.DirectMessage(NativeMethods.SCI_LINELENGTH, new IntPtr(startLine)).ToInt32();
            AdjustLineLength(startLine, GetCharCount(lineByteStart, lineByteLength) - CharLineLength(startLine));

            for (int i = 1; i <= scn.linesAdded.ToInt32(); i++)
            {
                int line = startLine + i;

                // Insert new line
                lineByteStart += lineByteLength;
                lineByteLength = this.scintilla.DirectMessage(NativeMethods.SCI_LINELENGTH, new IntPtr(line)).ToInt32();
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
    public bool AllLinesVisible
    {
        get
        {
            return this.scintilla.DirectMessage(NativeMethods.SCI_GETALLLINESVISIBLE) != IntPtr.Zero;
        }
    }

    /// <summary>
    /// Gets the number of lines.
    /// </summary>
    /// <returns>The number of lines in the <see cref="LineCollection" />.</returns>
    public int Count
    {
        get
        {
            // Subtract the terminal line
            return this.perLineData.Count - 1;
        }
    }

    /// <summary>
    /// Gets the number of CHARACTERS in the document.
    /// </summary>
    internal int TextLength
    {
        get
        {
            // Where the terminal line begins
            return CharPositionFromLine(this.perLineData.Count - 1);
        }
    }

    /// <summary>
    /// Gets the <see cref="Line" /> at the specified zero-based index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="Line" /> to get.</param>
    /// <returns>The <see cref="Line" /> at the specified index.</returns>
    public Line this[int index]
    {
        get
        {
            index = Helpers.Clamp(index, 0, Count - 1);
            return new Line(this.scintilla, index);
        }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LineCollection" /> class.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla" /> control that created this collection.</param>
    public LineCollection(Scintilla scintilla)
    {
        this.scintilla = scintilla;
        this.scintilla.SCNotification += scintilla_SCNotification;

        this.perLineData =
        [
            new PerLine { Start = 0 },
            new PerLine { Start = 0 }, // Terminal
        ];
    }

    #endregion Constructors

    #region Types

    /// <summary>
    /// Stuff we track for each line.
    /// </summary>
    private struct PerLine
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

    private enum ContainsMultibyte
    {
        No = -1,
        Unkown,
        Yes
    }

    #endregion Types
}