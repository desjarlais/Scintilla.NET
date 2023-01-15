#region License
/*
MIT License

Copyright(c) 2022 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System.Text;

namespace Scintilla.NET.Abstractions;

public static class HelpersGeneral
{
    #region Fields

    private static bool registeredFormats;
    private static uint CF_HTML;
    private static uint CF_RTF;
    private static uint CF_LINESELECT;
    private static uint CF_VSLINETAG;

    #endregion Fields

    #region Methods

    public static long CopyTo(this Stream source, Stream destination)
    {
        byte[] buffer = new byte[2048];
        int bytesRead;
        long totalBytes = 0;
        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
        {
            destination.Write(buffer, 0, bytesRead);
            totalBytes += bytesRead;
        }
        return totalBytes;
    }

    public static int Clamp(int value, int min, int max)
    {
        if (value < min)
            return min;

        if (value > max)
            return max;

        return value;
    }

    public static int ClampMin(int value, int min)
    {
        if (value < min)
            return min;

        return value;
    }
    
    #endregion Methods

    #region Types

    private struct StyleData
    {
        public bool Used;
        public string FontName;
        public int FontIndex; // RTF Only
        public float SizeF;
        public int Weight;
        public int Italic;
        public int Underline;
        public int BackColor;
        public int BackColorIndex; // RTF Only
        public int ForeColor;
        public int ForeColorIndex; // RTF Only
        public int Case; // HTML only
        public int Visible; // HTML only
    }

    #endregion Types

    #region Miscellaneous
    public static unsafe byte[] GetBytes(string text, Encoding encoding, bool zeroTerminated)
    {
        if (string.IsNullOrEmpty(text))
            return (zeroTerminated ? new byte[] { 0 } : new byte[0]);

        int count = encoding.GetByteCount(text);
        byte[] buffer = new byte[count + (zeroTerminated ? 1 : 0)];

        fixed (byte* bp = buffer)
        fixed (char* ch = text)
        {
            encoding.GetBytes(ch, text.Length, bp, count);
        }

        if (zeroTerminated)
            buffer[buffer.Length - 1] = 0;

        return buffer;
    }

    public static unsafe byte[] GetBytes(char[] text, int length, Encoding encoding, bool zeroTerminated)
    {
        fixed (char* cp = text)
        {
            var count = encoding.GetByteCount(cp, length);
            var buffer = new byte[count + (zeroTerminated ? 1 : 0)];
            fixed (byte* bp = buffer)
                encoding.GetBytes(cp, length, bp, buffer.Length);

            if (zeroTerminated)
                buffer[buffer.Length - 1] = 0;

            return buffer;
        }
    }

    public static unsafe byte[] ByteToCharStyles(byte* styles, byte* text, int length, Encoding encoding)
    {
        // This is used by annotations and margins to get all the styles in one call.
        // It converts an array of styles where each element corresponds to a BYTE
        // to an array of styles where each element corresponds to a CHARACTER.

        var bytePos = 0; // Position within text BYTES and style BYTES (should be the same)
        var charPos = 0; // Position within style CHARACTERS
        var decoder = encoding.GetDecoder();
        var result = new byte[encoding.GetCharCount(text, length)];

        while (bytePos < length)
        {
            if (decoder.GetCharCount(text + bytePos, 1, false) > 0)
                result[charPos++] = *(styles + bytePos); // New char

            bytePos++;
        }

        return result;
    }

    public static unsafe byte[] CharToByteStyles(byte[] styles, byte* text, int length, Encoding encoding)
    {
        // This is used by annotations and margins to style all the text in one call.
        // It converts an array of styles where each element corresponds to a CHARACTER
        // to an array of styles where each element corresponds to a BYTE.

        var bytePos = 0; // Position within text BYTES and style BYTES (should be the same)
        var charPos = 0; // Position within style CHARACTERS
        var decoder = encoding.GetDecoder();
        var result = new byte[length];

        while (bytePos < length && charPos < styles.Length)
        {
            result[bytePos] = styles[charPos];
            if (decoder.GetCharCount(text + bytePos, 1, false) > 0)
                charPos++; // Move a char

            bytePos++;
        }

        return result;
    }

    /// <summary>
    /// Gets the number of CHARACTERS in a BYTE range.
    /// </summary>
    public static unsafe int GetCharCount(IntPtr text, int length, Encoding encoding)
    {
        if (text == IntPtr.Zero || length == 0)
            return 0;

        // Never use SCI_COUNTCHARACTERS. It counts CRLF as 1 char!
        var count = encoding.GetCharCount((byte*)text, length);
        return count;
    }

    public static unsafe string GetString(IntPtr bytes, int length, Encoding encoding)
    {
        var ptr = (sbyte*)bytes;
        var str = new string(ptr, 0, length, encoding);

        return str;
    }
    #endregion
}