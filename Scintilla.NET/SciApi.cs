using System;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

namespace ScintillaNET;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class SciApi
{
    public delegate IntPtr Scintilla_DirectFunction(IntPtr ptr, int iMessage, IntPtr wParam, IntPtr lParam);
    
    [StructLayout(LayoutKind.Sequential)]
    public struct Sci_CharacterRange
    {
        public int cpMin;
        public int cpMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Sci_CharacterRangeFull
    {
        public nint cpMin;
        public nint cpMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Sci_TextRange
    {
        public Sci_CharacterRange chrg;
        public IntPtr lpstrText;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Sci_TextRangeFull
    {
        public Sci_CharacterRangeFull chrg;
        public IntPtr lpstrText;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Sci_TextToFind
    {
        public Sci_CharacterRange chrg;
        public IntPtr lpstrText;
        public Sci_CharacterRange chrgText;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Sci_TextToFindFull
    {
        public Sci_CharacterRangeFull chrg;
        public IntPtr lpstrText;
        public Sci_CharacterRangeFull chrgText;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    struct Sci_RangeToFormat
    {
        IntPtr hdc;
        IntPtr hdcTarget;
        RECT rc;
        RECT rcPage;
        Sci_CharacterRange chrg;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    struct Sci_RangeToFormatFull
    {
        IntPtr hdc;
        IntPtr hdcTarget;
        RECT rc;
        RECT rcPage;
        Sci_CharacterRangeFull chrg;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Sci_NotifyHeader
    {
        // Compatible with Windows NMHDR.
        // hwndFrom is really an environment specific window handle or pointer
        // but most clients of Scintilla.h do not have this type visible.
        public IntPtr hwndFrom;
        public UIntPtr idFrom;
        public uint code;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SCNotification
    {
        public Sci_NotifyHeader nmhdr;

        /// <summary>
        /// Used by: SCN_STYLENEEDED, SCN_DOUBLECLICK, SCN_MODIFIED, SCN_MARGINCLICK, SCN_MARGINRIGHTCLICK, SCN_NEEDSHOWN, SCN_DWELLSTART, SCN_DWELLEND, SCN_CALLTIPCLICK, SCN_HOTSPOTCLICK, SCN_HOTSPOTDOUBLECLICK, SCN_HOTSPOTRELEASECLICK, SCN_INDICATORCLICK, SCN_INDICATORRELEASE, SCN_USERLISTSELECTION, SCN_AUTOCCOMPLETED, SCN_AUTOCSELECTION, SCN_AUTOCSELECTIONCHANGE
        /// </summary>
        public IntPtr position;

        /// <summary>
        /// Used by: SCN_CHARADDED, SCN_KEY, SCN_AUTOCCOMPLETED, SCN_AUTOCSELECTION, SCN_USERLISTSELECTION
        /// </summary>
        public int ch;

        /// <summary>
        /// Used by: SCN_KEY, SCN_DOUBLECLICK, SCN_HOTSPOTCLICK, SCN_HOTSPOTDOUBLECLICK, SCN_HOTSPOTRELEASECLICK, SCN_INDICATORCLICK, SCN_INDICATORRELEASE, SCN_MARGINCLICK, SCN_MARGINRIGHTCLICK
        /// </summary>
        public int modifiers;

        /// <summary>
        /// Used by: SCN_MODIFIED
        /// </summary>
        public int modificationType;

        /// <summary>
        /// Used by: SCN_MODIFIED, SCN_USERLISTSELECTION, SCN_URIDROPPED, SCN_AUTOCCOMPLETED, SCN_AUTOCSELECTION, SCN_AUTOCSELECTIONCHANGE
        /// </summary>
        public IntPtr text;

        /// <summary>
        /// Used by: SCN_MODIFIED
        /// </summary>
        public IntPtr length;

        /// <summary>
        /// Used by: SCN_MODIFIED
        /// </summary>
        public IntPtr linesAdded;

        /// <summary>
        /// Used by: SCN_MACRORECORD
        /// </summary>
        public int message;

        /// <summary>
        /// Used by: SCN_MACRORECORD
        /// </summary>
        public UIntPtr wParam;

        /// <summary>
        /// Used by: SCN_MACRORECORD
        /// </summary>
        public IntPtr lParam;

        /// <summary>
        /// Used by: SCN_MODIFIED
        /// </summary>
        public IntPtr line;

        /// <summary>
        /// Used by: SCN_MODIFIED
        /// </summary>
        public int foldLevelNow;

        /// <summary>
        /// Used by: SCN_MODIFIED
        /// </summary>
        public int foldLevelPrev;

        /// <summary>
        /// Used by: SCN_MARGINCLICK, SCN_MARGINRIGHTCLICK
        /// </summary>
        public int margin;

        /// <summary>
        /// Used by: SCN_USERLISTSELECTION, SCN_AUTOCSELECTIONCHANGE
        /// </summary>
        public int listType;

        /// <summary>
        /// Used by: SCN_DWELLSTART, SCN_DWELLEND
        /// </summary>
        public int x;

        /// <summary>
        /// Used by: SCN_DWELLSTART, SCN_DWELLEND
        /// </summary>
        public int y;

        /// <summary>
        /// Used by: SCN_MODIFIED with SC_MOD_CONTAINER
        /// </summary>
        public int token;

        /// <summary>
        /// Used by: SCN_MODIFIED with SC_MOD_CHANGEANNOTATION
        /// </summary>
        public IntPtr annotationLinesAdded;

        /// <summary>
        /// Used by: SCN_UPDATEUI
        /// </summary>
        public int updated;

        /// <summary>
        /// Used by: SCN_AUTOCSELECTION, SCN_AUTOCCOMPLETED, SCN_USERLISTSELECTION
        /// </summary>
        public int listCompletionMethod;

        /// <summary>
        /// Used by: SCN_CHARADDED
        /// </summary>
        public int characterSource;
    }

    public const int INVALID_POSITION = -1;
    
    /// <summary>
    /// Define start of Scintilla messages to be greater than all Windows edit (EM_*) messages
    /// as many EM_ messages can be used although that use is deprecated.
    /// </summary>
    public const int SCI_START = 2000;
    
    public const int SCI_OPTIONAL_START = 3000;
    
    public const int SCI_LEXER_START = 4000;
    
    /// <summary>
    /// <code>fun void AddText=2001(position length, string text)</code>
    /// Add text to the document at current position.
    /// </summary>
    public const int SCI_ADDTEXT = 2001;
    
    /// <summary>
    /// <code>fun void AddStyledText=2002(position length, cells c)</code>
    /// Add array of cells to document.
    /// </summary>
    public const int SCI_ADDSTYLEDTEXT = 2002;
    
    /// <summary>
    /// <code>fun void InsertText=2003(position pos, string text)</code>
    /// Insert string at a position.
    /// </summary>
    public const int SCI_INSERTTEXT = 2003;
    
    /// <summary>
    /// <code>fun void ChangeInsertion=2672(position length, string text)</code>
    /// Change the text that is being inserted in response to SC_MOD_INSERTCHECK
    /// </summary>
    public const int SCI_CHANGEINSERTION = 2672;
    
    /// <summary>
    /// <code>fun void ClearAll=2004(, )</code>
    /// Delete all text in the document.
    /// </summary>
    public const int SCI_CLEARALL = 2004;
    
    /// <summary>
    /// <code>fun void DeleteRange=2645(position start, position lengthDelete)</code>
    /// Delete a range of text in the document.
    /// </summary>
    public const int SCI_DELETERANGE = 2645;
    
    /// <summary>
    /// <code>fun void ClearDocumentStyle=2005(, )</code>
    /// Set all style bytes to 0, remove all folding information.
    /// </summary>
    public const int SCI_CLEARDOCUMENTSTYLE = 2005;
    
    /// <summary>
    /// <code>get position GetLength=2006(, )</code>
    /// Returns the number of bytes in the document.
    /// </summary>
    public const int SCI_GETLENGTH = 2006;
    
    /// <summary>
    /// <code>get int GetCharAt=2007(position pos, )</code>
    /// Returns the character byte at the position.
    /// </summary>
    public const int SCI_GETCHARAT = 2007;
    
    /// <summary>
    /// <code>get position GetCurrentPos=2008(, )</code>
    /// Returns the position of the caret.
    /// </summary>
    public const int SCI_GETCURRENTPOS = 2008;
    
    /// <summary>
    /// <code>get position GetAnchor=2009(, )</code>
    /// Returns the position of the opposite end of the selection to the caret.
    /// </summary>
    public const int SCI_GETANCHOR = 2009;
    
    /// <summary>
    /// <code>get int GetStyleAt=2010(position pos, )</code>
    /// Returns the style byte at the position.
    /// </summary>
    public const int SCI_GETSTYLEAT = 2010;
    
    /// <summary>
    /// <code>get int GetStyleIndexAt=2038(position pos, )</code>
    /// Returns the unsigned style byte at the position.
    /// </summary>
    public const int SCI_GETSTYLEINDEXAT = 2038;
    
    /// <summary>
    /// <code>fun void Redo=2011(, )</code>
    /// Redoes the next action on the undo history.
    /// </summary>
    public const int SCI_REDO = 2011;
    
    /// <summary>
    /// <code>set void SetUndoCollection=2012(bool collectUndo, )</code>
    /// Choose between collecting actions into the undo
    /// history and discarding them.
    /// </summary>
    public const int SCI_SETUNDOCOLLECTION = 2012;
    
    /// <summary>
    /// <code>fun void SelectAll=2013(, )</code>
    /// Select all the text in the document.
    /// </summary>
    public const int SCI_SELECTALL = 2013;
    
    /// <summary>
    /// <code>fun void SetSavePoint=2014(, )</code>
    /// Remember the current position in the undo history as the position
    /// at which the document was saved.
    /// </summary>
    public const int SCI_SETSAVEPOINT = 2014;
    
    /// <summary>
    /// <code>fun position GetStyledText=2015(, textrange tr)</code>
    /// Retrieve a buffer of cells.
    /// Returns the number of bytes in the buffer not including terminating NULs.
    /// </summary>
    public const int SCI_GETSTYLEDTEXT = 2015;
    
    /// <summary>
    /// <code>fun position GetStyledTextFull=2778(, textrangefull tr)</code>
    /// Retrieve a buffer of cells that can be past 2GB.
    /// Returns the number of bytes in the buffer not including terminating NULs.
    /// </summary>
    public const int SCI_GETSTYLEDTEXTFULL = 2778;
    
    /// <summary>
    /// <code>fun bool CanRedo=2016(, )</code>
    /// Are there any redoable actions in the undo history?
    /// </summary>
    public const int SCI_CANREDO = 2016;
    
    /// <summary>
    /// <code>fun line MarkerLineFromHandle=2017(int markerHandle, )</code>
    /// Retrieve the line number at which a particular marker is located.
    /// </summary>
    public const int SCI_MARKERLINEFROMHANDLE = 2017;
    
    /// <summary>
    /// <code>fun void MarkerDeleteHandle=2018(int markerHandle, )</code>
    /// Delete a marker.
    /// </summary>
    public const int SCI_MARKERDELETEHANDLE = 2018;
    
    /// <summary>
    /// <code>fun int MarkerHandleFromLine=2732(line line, int which)</code>
    /// Retrieve marker handles of a line
    /// </summary>
    public const int SCI_MARKERHANDLEFROMLINE = 2732;
    
    /// <summary>
    /// <code>fun int MarkerNumberFromLine=2733(line line, int which)</code>
    /// Retrieve marker number of a marker handle
    /// </summary>
    public const int SCI_MARKERNUMBERFROMLINE = 2733;
    
    /// <summary>
    /// <code>get bool GetUndoCollection=2019(, )</code>
    /// Is undo history being collected?
    /// </summary>
    public const int SCI_GETUNDOCOLLECTION = 2019;
    
    // WhiteSpace
    // ==========
    public const int SCWS_INVISIBLE = 0;
    public const int SCWS_VISIBLEALWAYS = 1;
    public const int SCWS_VISIBLEAFTERINDENT = 2;
    public const int SCWS_VISIBLEONLYININDENT = 3;
    
    /// <summary>
    /// <code>get WhiteSpace GetViewWS=2020(, )</code>
    /// Are white space characters currently visible?
    /// Returns one of SCWS_* constants.
    /// </summary>
    public const int SCI_GETVIEWWS = 2020;
    
    /// <summary>
    /// <code>set void SetViewWS=2021(WhiteSpace viewWS, )</code>
    /// Make white space characters invisible, always visible or visible outside indentation.
    /// </summary>
    public const int SCI_SETVIEWWS = 2021;
    
    // TabDrawMode
    // ===========
    public const int SCTD_LONGARROW = 0;
    public const int SCTD_STRIKEOUT = 1;
    
    /// <summary>
    /// <code>get TabDrawMode GetTabDrawMode=2698(, )</code>
    /// Retrieve the current tab draw mode.
    /// Returns one of SCTD_* constants.
    /// </summary>
    public const int SCI_GETTABDRAWMODE = 2698;
    
    /// <summary>
    /// <code>set void SetTabDrawMode=2699(TabDrawMode tabDrawMode, )</code>
    /// Set how tabs are drawn when visible.
    /// </summary>
    public const int SCI_SETTABDRAWMODE = 2699;
    
    /// <summary>
    /// <code>fun position PositionFromPoint=2022(int x, int y)</code>
    /// Find the position from a point within the window.
    /// </summary>
    public const int SCI_POSITIONFROMPOINT = 2022;
    
    /// <summary>
    /// <code>fun position PositionFromPointClose=2023(int x, int y)</code>
    /// Find the position from a point within the window but return
    /// INVALID_POSITION if not close to text.
    /// </summary>
    public const int SCI_POSITIONFROMPOINTCLOSE = 2023;
    
    /// <summary>
    /// <code>fun void GotoLine=2024(line line, )</code>
    /// Set caret to start of a line and ensure it is visible.
    /// </summary>
    public const int SCI_GOTOLINE = 2024;
    
    /// <summary>
    /// <code>fun void GotoPos=2025(position caret, )</code>
    /// Set caret to a position and ensure it is visible.
    /// </summary>
    public const int SCI_GOTOPOS = 2025;
    
    /// <summary>
    /// <code>set void SetAnchor=2026(position anchor, )</code>
    /// Set the selection anchor to a position. The anchor is the opposite
    /// end of the selection from the caret.
    /// </summary>
    public const int SCI_SETANCHOR = 2026;
    
    /// <summary>
    /// <code>fun position GetCurLine=2027(position length, stringresult text)</code>
    /// Retrieve the text of the line containing the caret.
    /// Returns the index of the caret on the line.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_GETCURLINE = 2027;
    
    /// <summary>
    /// <code>get position GetEndStyled=2028(, )</code>
    /// Retrieve the position of the last correctly styled character.
    /// </summary>
    public const int SCI_GETENDSTYLED = 2028;
    
    // EndOfLine
    // =========
    public const int SC_EOL_CRLF = 0;
    public const int SC_EOL_CR = 1;
    public const int SC_EOL_LF = 2;
    
    /// <summary>
    /// <code>fun void ConvertEOLs=2029(EndOfLine eolMode, )</code>
    /// Convert all line endings in the document to one mode.
    /// </summary>
    public const int SCI_CONVERTEOLS = 2029;
    
    /// <summary>
    /// <code>get EndOfLine GetEOLMode=2030(, )</code>
    /// Retrieve the current end of line mode - one of CRLF, CR, or LF.
    /// </summary>
    public const int SCI_GETEOLMODE = 2030;
    
    /// <summary>
    /// <code>set void SetEOLMode=2031(EndOfLine eolMode, )</code>
    /// Set the current end of line mode.
    /// </summary>
    public const int SCI_SETEOLMODE = 2031;
    
    /// <summary>
    /// <code>fun void StartStyling=2032(position start, int unused)</code>
    /// Set the current styling position to start.
    /// The unused parameter is no longer used and should be set to 0.
    /// </summary>
    public const int SCI_STARTSTYLING = 2032;
    
    /// <summary>
    /// <code>fun void SetStyling=2033(position length, int style)</code>
    /// Change style from current styling position for length characters to a style
    /// and move the current styling position to after this newly styled segment.
    /// </summary>
    public const int SCI_SETSTYLING = 2033;
    
    /// <summary>
    /// <code>get bool GetBufferedDraw=2034(, )</code>
    /// Is drawing done first into a buffer or direct to the screen?
    /// </summary>
    public const int SCI_GETBUFFEREDDRAW = 2034;
    
    /// <summary>
    /// <code>set void SetBufferedDraw=2035(bool buffered, )</code>
    /// If drawing is buffered then each line of text is drawn into a bitmap buffer
    /// before drawing it to the screen to avoid flicker.
    /// </summary>
    public const int SCI_SETBUFFEREDDRAW = 2035;
    
    /// <summary>
    /// <code>set void SetTabWidth=2036(int tabWidth, )</code>
    /// Change the visible size of a tab to be a multiple of the width of a space character.
    /// </summary>
    public const int SCI_SETTABWIDTH = 2036;
    
    /// <summary>
    /// <code>get int GetTabWidth=2121(, )</code>
    /// Retrieve the visible size of a tab.
    /// </summary>
    public const int SCI_GETTABWIDTH = 2121;
    
    /// <summary>
    /// <code>set void SetTabMinimumWidth=2724(int pixels, )</code>
    /// Set the minimum visual width of a tab.
    /// </summary>
    public const int SCI_SETTABMINIMUMWIDTH = 2724;
    
    /// <summary>
    /// <code>get int GetTabMinimumWidth=2725(, )</code>
    /// Get the minimum visual width of a tab.
    /// </summary>
    public const int SCI_GETTABMINIMUMWIDTH = 2725;
    
    /// <summary>
    /// <code>fun void ClearTabStops=2675(line line, )</code>
    /// Clear explicit tabstops on a line.
    /// </summary>
    public const int SCI_CLEARTABSTOPS = 2675;
    
    /// <summary>
    /// <code>fun void AddTabStop=2676(line line, int x)</code>
    /// Add an explicit tab stop for a line.
    /// </summary>
    public const int SCI_ADDTABSTOP = 2676;
    
    /// <summary>
    /// <code>fun int GetNextTabStop=2677(line line, int x)</code>
    /// Find the next explicit tab stop position on a line after a position.
    /// </summary>
    public const int SCI_GETNEXTTABSTOP = 2677;
    
    /// <summary>
    /// The SC_CP_UTF8 value can be used to enter Unicode mode.
    /// This is the same value as CP_UTF8 in Windows
    /// </summary>
    public const int SC_CP_UTF8 = 65001;
    
    /// <summary>
    /// <code>set void SetCodePage=2037(int codePage, )</code>
    /// Set the code page used to interpret the bytes of the document as characters.
    /// The SC_CP_UTF8 value can be used to enter Unicode mode.
    /// </summary>
    public const int SCI_SETCODEPAGE = 2037;
    
    /// <summary>
    /// <code>set void SetFontLocale=2760(, string localeName)</code>
    /// Set the locale for displaying text.
    /// </summary>
    public const int SCI_SETFONTLOCALE = 2760;
    
    /// <summary>
    /// <code>get int GetFontLocale=2761(, stringresult localeName)</code>
    /// Get the locale for displaying text.
    /// </summary>
    public const int SCI_GETFONTLOCALE = 2761;
    
    // IMEInteraction
    // ==============
    public const int SC_IME_WINDOWED = 0;
    public const int SC_IME_INLINE = 1;
    
    /// <summary>
    /// <code>get IMEInteraction GetIMEInteraction=2678(, )</code>
    /// Is the IME displayed in a window or inline?
    /// </summary>
    public const int SCI_GETIMEINTERACTION = 2678;
    
    /// <summary>
    /// <code>set void SetIMEInteraction=2679(IMEInteraction imeInteraction, )</code>
    /// Choose to display the IME in a window or inline.
    /// </summary>
    public const int SCI_SETIMEINTERACTION = 2679;
    
    // Alpha
    // =====
    public const int SC_ALPHA_TRANSPARENT = 0;
    public const int SC_ALPHA_OPAQUE = 255;
    public const int SC_ALPHA_NOALPHA = 256;
    
    // CursorShape
    // ===========
    public const int SC_CURSORNORMAL = -1;
    public const int SC_CURSORARROW = 2;
    public const int SC_CURSORWAIT = 4;
    public const int SC_CURSORREVERSEARROW = 7;
    
    public const int MARKER_MAX = 31;
    
    // MarkerSymbol
    // ============
    public const int SC_MARK_CIRCLE = 0;
    public const int SC_MARK_ROUNDRECT = 1;
    public const int SC_MARK_ARROW = 2;
    public const int SC_MARK_SMALLRECT = 3;
    public const int SC_MARK_SHORTARROW = 4;
    public const int SC_MARK_EMPTY = 5;
    public const int SC_MARK_ARROWDOWN = 6;
    public const int SC_MARK_MINUS = 7;
    public const int SC_MARK_PLUS = 8;
    /// <summary>Shapes used for outlining column.</summary>
    public const int SC_MARK_VLINE = 9;
    public const int SC_MARK_LCORNER = 10;
    public const int SC_MARK_TCORNER = 11;
    public const int SC_MARK_BOXPLUS = 12;
    public const int SC_MARK_BOXPLUSCONNECTED = 13;
    public const int SC_MARK_BOXMINUS = 14;
    public const int SC_MARK_BOXMINUSCONNECTED = 15;
    public const int SC_MARK_LCORNERCURVE = 16;
    public const int SC_MARK_TCORNERCURVE = 17;
    public const int SC_MARK_CIRCLEPLUS = 18;
    public const int SC_MARK_CIRCLEPLUSCONNECTED = 19;
    public const int SC_MARK_CIRCLEMINUS = 20;
    public const int SC_MARK_CIRCLEMINUSCONNECTED = 21;
    /// <summary>Invisible mark that only sets the line background colour.</summary>
    public const int SC_MARK_BACKGROUND = 22;
    public const int SC_MARK_DOTDOTDOT = 23;
    public const int SC_MARK_ARROWS = 24;
    public const int SC_MARK_PIXMAP = 25;
    public const int SC_MARK_FULLRECT = 26;
    public const int SC_MARK_LEFTRECT = 27;
    public const int SC_MARK_AVAILABLE = 28;
    public const int SC_MARK_UNDERLINE = 29;
    public const int SC_MARK_RGBAIMAGE = 30;
    public const int SC_MARK_BOOKMARK = 31;
    public const int SC_MARK_VERTICALBOOKMARK = 32;
    public const int SC_MARK_BAR = 33;
    public const int SC_MARK_CHARACTER = 10000;
    
    // MarkerOutline
    // =============
    /// <summary>Markers used for outlining and change history columns.</summary>
    public const int SC_MARKNUM_HISTORY_REVERTED_TO_ORIGIN = 21;
    public const int SC_MARKNUM_HISTORY_SAVED = 22;
    public const int SC_MARKNUM_HISTORY_MODIFIED = 23;
    public const int SC_MARKNUM_HISTORY_REVERTED_TO_MODIFIED = 24;
    public const int SC_MARKNUM_FOLDEREND = 25;
    public const int SC_MARKNUM_FOLDEROPENMID = 26;
    public const int SC_MARKNUM_FOLDERMIDTAIL = 27;
    public const int SC_MARKNUM_FOLDERTAIL = 28;
    public const int SC_MARKNUM_FOLDERSUB = 29;
    public const int SC_MARKNUM_FOLDER = 30;
    public const int SC_MARKNUM_FOLDEROPEN = 31;
    
    public const uint SC_MASK_HISTORY = 0x01E00000;
    
    /// <summary>SC_MASK_FOLDERS doesn't go in an enumeration as larger than max 32-bit positive integer</summary>
    public const uint SC_MASK_FOLDERS = 0xFE000000;
    
    /// <summary>
    /// <code>fun void MarkerDefine=2040(int markerNumber, MarkerSymbol markerSymbol)</code>
    /// Set the symbol used for a particular marker number.
    /// </summary>
    public const int SCI_MARKERDEFINE = 2040;
    
    /// <summary>
    /// <code>set void MarkerSetFore=2041(int markerNumber, colour fore)</code>
    /// Set the foreground colour used for a particular marker number.
    /// </summary>
    public const int SCI_MARKERSETFORE = 2041;
    
    /// <summary>
    /// <code>set void MarkerSetBack=2042(int markerNumber, colour back)</code>
    /// Set the background colour used for a particular marker number.
    /// </summary>
    public const int SCI_MARKERSETBACK = 2042;
    
    /// <summary>
    /// <code>set void MarkerSetBackSelected=2292(int markerNumber, colour back)</code>
    /// Set the background colour used for a particular marker number when its folding block is selected.
    /// </summary>
    public const int SCI_MARKERSETBACKSELECTED = 2292;
    
    /// <summary>
    /// <code>set void MarkerSetForeTranslucent=2294(int markerNumber, colouralpha fore)</code>
    /// Set the foreground colour used for a particular marker number.
    /// </summary>
    public const int SCI_MARKERSETFORETRANSLUCENT = 2294;
    
    /// <summary>
    /// <code>set void MarkerSetBackTranslucent=2295(int markerNumber, colouralpha back)</code>
    /// Set the background colour used for a particular marker number.
    /// </summary>
    public const int SCI_MARKERSETBACKTRANSLUCENT = 2295;
    
    /// <summary>
    /// <code>set void MarkerSetBackSelectedTranslucent=2296(int markerNumber, colouralpha back)</code>
    /// Set the background colour used for a particular marker number when its folding block is selected.
    /// </summary>
    public const int SCI_MARKERSETBACKSELECTEDTRANSLUCENT = 2296;
    
    /// <summary>
    /// <code>set void MarkerSetStrokeWidth=2297(int markerNumber, int hundredths)</code>
    /// Set the width of strokes used in .01 pixels so 50  = 1/2 pixel width.
    /// </summary>
    public const int SCI_MARKERSETSTROKEWIDTH = 2297;
    
    /// <summary>
    /// <code>fun void MarkerEnableHighlight=2293(bool enabled, )</code>
    /// Enable/disable highlight for current folding block (smallest one that contains the caret)
    /// </summary>
    public const int SCI_MARKERENABLEHIGHLIGHT = 2293;
    
    /// <summary>
    /// <code>fun int MarkerAdd=2043(line line, int markerNumber)</code>
    /// Add a marker to a line, returning an ID which can be used to find or delete the marker.
    /// </summary>
    public const int SCI_MARKERADD = 2043;
    
    /// <summary>
    /// <code>fun void MarkerDelete=2044(line line, int markerNumber)</code>
    /// Delete a marker from a line.
    /// </summary>
    public const int SCI_MARKERDELETE = 2044;
    
    /// <summary>
    /// <code>fun void MarkerDeleteAll=2045(int markerNumber, )</code>
    /// Delete all markers with a particular number from all lines.
    /// </summary>
    public const int SCI_MARKERDELETEALL = 2045;
    
    /// <summary>
    /// <code>fun int MarkerGet=2046(line line, )</code>
    /// Get a bit mask of all the markers set on a line.
    /// </summary>
    public const int SCI_MARKERGET = 2046;
    
    /// <summary>
    /// <code>fun line MarkerNext=2047(line lineStart, int markerMask)</code>
    /// Find the next line at or after lineStart that includes a marker in mask.
    /// Return -1 when no more lines.
    /// </summary>
    public const int SCI_MARKERNEXT = 2047;
    
    /// <summary>
    /// <code>fun line MarkerPrevious=2048(line lineStart, int markerMask)</code>
    /// Find the previous line before lineStart that includes a marker in mask.
    /// </summary>
    public const int SCI_MARKERPREVIOUS = 2048;
    
    /// <summary>
    /// <code>fun void MarkerDefinePixmap=2049(int markerNumber, string pixmap)</code>
    /// Define a marker from a pixmap.
    /// </summary>
    public const int SCI_MARKERDEFINEPIXMAP = 2049;
    
    /// <summary>
    /// <code>fun void MarkerAddSet=2466(line line, int markerSet)</code>
    /// Add a set of markers to a line.
    /// </summary>
    public const int SCI_MARKERADDSET = 2466;
    
    /// <summary>
    /// <code>set void MarkerSetAlpha=2476(int markerNumber, Alpha alpha)</code>
    /// Set the alpha used for a marker that is drawn in the text area, not the margin.
    /// </summary>
    public const int SCI_MARKERSETALPHA = 2476;
    
    /// <summary>
    /// <code>get Layer MarkerGetLayer=2734(int markerNumber, )</code>
    /// Get the layer used for a marker that is drawn in the text area, not the margin.
    /// </summary>
    public const int SCI_MARKERGETLAYER = 2734;
    
    /// <summary>
    /// <code>set void MarkerSetLayer=2735(int markerNumber, Layer layer)</code>
    /// Set the layer used for a marker that is drawn in the text area, not the margin.
    /// </summary>
    public const int SCI_MARKERSETLAYER = 2735;
    
    public const int SC_MAX_MARGIN = 4;
    
    // MarginType
    // ==========
    public const int SC_MARGIN_SYMBOL = 0;
    public const int SC_MARGIN_NUMBER = 1;
    public const int SC_MARGIN_BACK = 2;
    public const int SC_MARGIN_FORE = 3;
    public const int SC_MARGIN_TEXT = 4;
    public const int SC_MARGIN_RTEXT = 5;
    public const int SC_MARGIN_COLOUR = 6;
    
    /// <summary>
    /// <code>set void SetMarginTypeN=2240(int margin, MarginType marginType)</code>
    /// Set a margin to be either numeric or symbolic.
    /// </summary>
    public const int SCI_SETMARGINTYPEN = 2240;
    
    /// <summary>
    /// <code>get MarginType GetMarginTypeN=2241(int margin, )</code>
    /// Retrieve the type of a margin.
    /// </summary>
    public const int SCI_GETMARGINTYPEN = 2241;
    
    /// <summary>
    /// <code>set void SetMarginWidthN=2242(int margin, int pixelWidth)</code>
    /// Set the width of a margin to a width expressed in pixels.
    /// </summary>
    public const int SCI_SETMARGINWIDTHN = 2242;
    
    /// <summary>
    /// <code>get int GetMarginWidthN=2243(int margin, )</code>
    /// Retrieve the width of a margin in pixels.
    /// </summary>
    public const int SCI_GETMARGINWIDTHN = 2243;
    
    /// <summary>
    /// <code>set void SetMarginMaskN=2244(int margin, int mask)</code>
    /// Set a mask that determines which markers are displayed in a margin.
    /// </summary>
    public const int SCI_SETMARGINMASKN = 2244;
    
    /// <summary>
    /// <code>get int GetMarginMaskN=2245(int margin, )</code>
    /// Retrieve the marker mask of a margin.
    /// </summary>
    public const int SCI_GETMARGINMASKN = 2245;
    
    /// <summary>
    /// <code>set void SetMarginSensitiveN=2246(int margin, bool sensitive)</code>
    /// Make a margin sensitive or insensitive to mouse clicks.
    /// </summary>
    public const int SCI_SETMARGINSENSITIVEN = 2246;
    
    /// <summary>
    /// <code>get bool GetMarginSensitiveN=2247(int margin, )</code>
    /// Retrieve the mouse click sensitivity of a margin.
    /// </summary>
    public const int SCI_GETMARGINSENSITIVEN = 2247;
    
    /// <summary>
    /// <code>set void SetMarginCursorN=2248(int margin, CursorShape cursor)</code>
    /// Set the cursor shown when the mouse is inside a margin.
    /// </summary>
    public const int SCI_SETMARGINCURSORN = 2248;
    
    /// <summary>
    /// <code>get CursorShape GetMarginCursorN=2249(int margin, )</code>
    /// Retrieve the cursor shown in a margin.
    /// </summary>
    public const int SCI_GETMARGINCURSORN = 2249;
    
    /// <summary>
    /// <code>set void SetMarginBackN=2250(int margin, colour back)</code>
    /// Set the background colour of a margin. Only visible for SC_MARGIN_COLOUR.
    /// </summary>
    public const int SCI_SETMARGINBACKN = 2250;
    
    /// <summary>
    /// <code>get colour GetMarginBackN=2251(int margin, )</code>
    /// Retrieve the background colour of a margin
    /// </summary>
    public const int SCI_GETMARGINBACKN = 2251;
    
    /// <summary>
    /// <code>set void SetMargins=2252(int margins, )</code>
    /// Allocate a non-standard number of margins.
    /// </summary>
    public const int SCI_SETMARGINS = 2252;
    
    /// <summary>
    /// <code>get int GetMargins=2253(, )</code>
    /// How many margins are there?.
    /// </summary>
    public const int SCI_GETMARGINS = 2253;
    
    // StylesCommon
    // ============
    public const int STYLE_DEFAULT = 32;
    public const int STYLE_LINENUMBER = 33;
    public const int STYLE_BRACELIGHT = 34;
    public const int STYLE_BRACEBAD = 35;
    public const int STYLE_CONTROLCHAR = 36;
    public const int STYLE_INDENTGUIDE = 37;
    public const int STYLE_CALLTIP = 38;
    public const int STYLE_FOLDDISPLAYTEXT = 39;
    public const int STYLE_LASTPREDEFINED = 39;
    public const int STYLE_MAX = 255;
    
    // CharacterSet
    // ============
    public const int SC_CHARSET_ANSI = 0;
    public const int SC_CHARSET_DEFAULT = 1;
    public const int SC_CHARSET_BALTIC = 186;
    public const int SC_CHARSET_CHINESEBIG5 = 136;
    public const int SC_CHARSET_EASTEUROPE = 238;
    public const int SC_CHARSET_GB2312 = 134;
    public const int SC_CHARSET_GREEK = 161;
    public const int SC_CHARSET_HANGUL = 129;
    public const int SC_CHARSET_MAC = 77;
    public const int SC_CHARSET_OEM = 255;
    public const int SC_CHARSET_RUSSIAN = 204;
    public const int SC_CHARSET_OEM866 = 866;
    public const int SC_CHARSET_CYRILLIC = 1251;
    public const int SC_CHARSET_SHIFTJIS = 128;
    public const int SC_CHARSET_SYMBOL = 2;
    public const int SC_CHARSET_TURKISH = 162;
    public const int SC_CHARSET_JOHAB = 130;
    public const int SC_CHARSET_HEBREW = 177;
    public const int SC_CHARSET_ARABIC = 178;
    public const int SC_CHARSET_VIETNAMESE = 163;
    public const int SC_CHARSET_THAI = 222;
    public const int SC_CHARSET_8859_15 = 1000;
    
    /// <summary>
    /// <code>fun void StyleClearAll=2050(, )</code>
    /// Clear all the styles and make equivalent to the global default style.
    /// </summary>
    public const int SCI_STYLECLEARALL = 2050;
    
    /// <summary>
    /// <code>set void StyleSetFore=2051(int style, colour fore)</code>
    /// Set the foreground colour of a style.
    /// </summary>
    public const int SCI_STYLESETFORE = 2051;
    
    /// <summary>
    /// <code>set void StyleSetBack=2052(int style, colour back)</code>
    /// Set the background colour of a style.
    /// </summary>
    public const int SCI_STYLESETBACK = 2052;
    
    /// <summary>
    /// <code>set void StyleSetBold=2053(int style, bool bold)</code>
    /// Set a style to be bold or not.
    /// </summary>
    public const int SCI_STYLESETBOLD = 2053;
    
    /// <summary>
    /// <code>set void StyleSetItalic=2054(int style, bool italic)</code>
    /// Set a style to be italic or not.
    /// </summary>
    public const int SCI_STYLESETITALIC = 2054;
    
    /// <summary>
    /// <code>set void StyleSetSize=2055(int style, int sizePoints)</code>
    /// Set the size of characters of a style.
    /// </summary>
    public const int SCI_STYLESETSIZE = 2055;
    
    /// <summary>
    /// <code>set void StyleSetFont=2056(int style, string fontName)</code>
    /// Set the font of a style.
    /// </summary>
    public const int SCI_STYLESETFONT = 2056;
    
    /// <summary>
    /// <code>set void StyleSetEOLFilled=2057(int style, bool eolFilled)</code>
    /// Set a style to have its end of line filled or not.
    /// </summary>
    public const int SCI_STYLESETEOLFILLED = 2057;
    
    /// <summary>
    /// <code>fun void StyleResetDefault=2058(, )</code>
    /// Reset the default style to its state at startup
    /// </summary>
    public const int SCI_STYLERESETDEFAULT = 2058;
    
    /// <summary>
    /// <code>set void StyleSetUnderline=2059(int style, bool underline)</code>
    /// Set a style to be underlined or not.
    /// </summary>
    public const int SCI_STYLESETUNDERLINE = 2059;
    
    // CaseVisible
    // ===========
    public const int SC_CASE_MIXED = 0;
    public const int SC_CASE_UPPER = 1;
    public const int SC_CASE_LOWER = 2;
    public const int SC_CASE_CAMEL = 3;
    
    /// <summary>
    /// <code>get colour StyleGetFore=2481(int style, )</code>
    /// Get the foreground colour of a style.
    /// </summary>
    public const int SCI_STYLEGETFORE = 2481;
    
    /// <summary>
    /// <code>get colour StyleGetBack=2482(int style, )</code>
    /// Get the background colour of a style.
    /// </summary>
    public const int SCI_STYLEGETBACK = 2482;
    
    /// <summary>
    /// <code>get bool StyleGetBold=2483(int style, )</code>
    /// Get is a style bold or not.
    /// </summary>
    public const int SCI_STYLEGETBOLD = 2483;
    
    /// <summary>
    /// <code>get bool StyleGetItalic=2484(int style, )</code>
    /// Get is a style italic or not.
    /// </summary>
    public const int SCI_STYLEGETITALIC = 2484;
    
    /// <summary>
    /// <code>get int StyleGetSize=2485(int style, )</code>
    /// Get the size of characters of a style.
    /// </summary>
    public const int SCI_STYLEGETSIZE = 2485;
    
    /// <summary>
    /// <code>get int StyleGetFont=2486(int style, stringresult fontName)</code>
    /// Get the font of a style.
    /// Returns the length of the fontName
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_STYLEGETFONT = 2486;
    
    /// <summary>
    /// <code>get bool StyleGetEOLFilled=2487(int style, )</code>
    /// Get is a style to have its end of line filled or not.
    /// </summary>
    public const int SCI_STYLEGETEOLFILLED = 2487;
    
    /// <summary>
    /// <code>get bool StyleGetUnderline=2488(int style, )</code>
    /// Get is a style underlined or not.
    /// </summary>
    public const int SCI_STYLEGETUNDERLINE = 2488;
    
    /// <summary>
    /// <code>get CaseVisible StyleGetCase=2489(int style, )</code>
    /// Get is a style mixed case, or to force upper or lower case.
    /// </summary>
    public const int SCI_STYLEGETCASE = 2489;
    
    /// <summary>
    /// <code>get CharacterSet StyleGetCharacterSet=2490(int style, )</code>
    /// Get the character get of the font in a style.
    /// </summary>
    public const int SCI_STYLEGETCHARACTERSET = 2490;
    
    /// <summary>
    /// <code>get bool StyleGetVisible=2491(int style, )</code>
    /// Get is a style visible or not.
    /// </summary>
    public const int SCI_STYLEGETVISIBLE = 2491;
    
    /// <summary>
    /// <code>get bool StyleGetChangeable=2492(int style, )</code>
    /// Get is a style changeable or not (read only).
    /// Experimental feature, currently buggy.
    /// </summary>
    public const int SCI_STYLEGETCHANGEABLE = 2492;
    
    /// <summary>
    /// <code>get bool StyleGetHotSpot=2493(int style, )</code>
    /// Get is a style a hotspot or not.
    /// </summary>
    public const int SCI_STYLEGETHOTSPOT = 2493;
    
    /// <summary>
    /// <code>set void StyleSetCase=2060(int style, CaseVisible caseVisible)</code>
    /// Set a style to be mixed case, or to force upper or lower case.
    /// </summary>
    public const int SCI_STYLESETCASE = 2060;
    
    public const int SC_FONT_SIZE_MULTIPLIER = 100;
    
    /// <summary>
    /// <code>set void StyleSetSizeFractional=2061(int style, int sizeHundredthPoints)</code>
    /// Set the size of characters of a style. Size is in points multiplied by 100.
    /// </summary>
    public const int SCI_STYLESETSIZEFRACTIONAL = 2061;
    
    /// <summary>
    /// <code>get int StyleGetSizeFractional=2062(int style, )</code>
    /// Get the size of characters of a style in points multiplied by 100
    /// </summary>
    public const int SCI_STYLEGETSIZEFRACTIONAL = 2062;
    
    // FontWeight
    // ==========
    public const int SC_WEIGHT_NORMAL = 400;
    public const int SC_WEIGHT_SEMIBOLD = 600;
    public const int SC_WEIGHT_BOLD = 700;
    
    /// <summary>
    /// <code>set void StyleSetWeight=2063(int style, FontWeight weight)</code>
    /// Set the weight of characters of a style.
    /// </summary>
    public const int SCI_STYLESETWEIGHT = 2063;
    
    /// <summary>
    /// <code>get FontWeight StyleGetWeight=2064(int style, )</code>
    /// Get the weight of characters of a style.
    /// </summary>
    public const int SCI_STYLEGETWEIGHT = 2064;
    
    /// <summary>
    /// <code>set void StyleSetCharacterSet=2066(int style, CharacterSet characterSet)</code>
    /// Set the character set of the font in a style.
    /// </summary>
    public const int SCI_STYLESETCHARACTERSET = 2066;
    
    /// <summary>
    /// <code>set void StyleSetHotSpot=2409(int style, bool hotspot)</code>
    /// Set a style to be a hotspot or not.
    /// </summary>
    public const int SCI_STYLESETHOTSPOT = 2409;
    
    /// <summary>
    /// <code>set void StyleSetCheckMonospaced=2254(int style, bool checkMonospaced)</code>
    /// Indicate that a style may be monospaced over ASCII graphics characters which enables optimizations.
    /// </summary>
    public const int SCI_STYLESETCHECKMONOSPACED = 2254;
    
    /// <summary>
    /// <code>get bool StyleGetCheckMonospaced=2255(int style, )</code>
    /// Get whether a style may be monospaced.
    /// </summary>
    public const int SCI_STYLEGETCHECKMONOSPACED = 2255;
    
    // FontStretch
    // ===========
    public const int SC_STRETCH_ULTRA_CONDENSED = 1;
    public const int SC_STRETCH_EXTRA_CONDENSED = 2;
    public const int SC_STRETCH_CONDENSED = 3;
    public const int SC_STRETCH_SEMI_CONDENSED = 4;
    public const int SC_STRETCH_NORMAL = 5;
    public const int SC_STRETCH_SEMI_EXPANDED = 6;
    public const int SC_STRETCH_EXPANDED = 7;
    public const int SC_STRETCH_EXTRA_EXPANDED = 8;
    public const int SC_STRETCH_ULTRA_EXPANDED = 9;
    
    /// <summary>
    /// <code>set void StyleSetStretch=2258(int style, FontStretch stretch)</code>
    /// Set the stretch of characters of a style.
    /// </summary>
    public const int SCI_STYLESETSTRETCH = 2258;
    
    /// <summary>
    /// <code>get FontStretch StyleGetStretch=2259(int style, )</code>
    /// Get the stretch of characters of a style.
    /// </summary>
    public const int SCI_STYLEGETSTRETCH = 2259;
    
    /// <summary>
    /// <code>set void StyleSetInvisibleRepresentation=2256(int style, string representation)</code>
    /// Set the invisible representation for a style.
    /// </summary>
    public const int SCI_STYLESETINVISIBLEREPRESENTATION = 2256;
    
    /// <summary>
    /// <code>get int StyleGetInvisibleRepresentation=2257(int style, stringresult representation)</code>
    /// Get the invisible representation for a style.
    /// </summary>
    public const int SCI_STYLEGETINVISIBLEREPRESENTATION = 2257;
    
    // Element
    // =======
    public const int SC_ELEMENT_LIST = 0;
    public const int SC_ELEMENT_LIST_BACK = 1;
    public const int SC_ELEMENT_LIST_SELECTED = 2;
    public const int SC_ELEMENT_LIST_SELECTED_BACK = 3;
    public const int SC_ELEMENT_SELECTION_TEXT = 10;
    public const int SC_ELEMENT_SELECTION_BACK = 11;
    public const int SC_ELEMENT_SELECTION_ADDITIONAL_TEXT = 12;
    public const int SC_ELEMENT_SELECTION_ADDITIONAL_BACK = 13;
    public const int SC_ELEMENT_SELECTION_SECONDARY_TEXT = 14;
    public const int SC_ELEMENT_SELECTION_SECONDARY_BACK = 15;
    public const int SC_ELEMENT_SELECTION_INACTIVE_TEXT = 16;
    public const int SC_ELEMENT_SELECTION_INACTIVE_BACK = 17;
    public const int SC_ELEMENT_SELECTION_INACTIVE_ADDITIONAL_TEXT = 18;
    public const int SC_ELEMENT_SELECTION_INACTIVE_ADDITIONAL_BACK = 19;
    public const int SC_ELEMENT_CARET = 40;
    public const int SC_ELEMENT_CARET_ADDITIONAL = 41;
    public const int SC_ELEMENT_CARET_LINE_BACK = 50;
    public const int SC_ELEMENT_WHITE_SPACE = 60;
    public const int SC_ELEMENT_WHITE_SPACE_BACK = 61;
    public const int SC_ELEMENT_HOT_SPOT_ACTIVE = 70;
    public const int SC_ELEMENT_HOT_SPOT_ACTIVE_BACK = 71;
    public const int SC_ELEMENT_FOLD_LINE = 80;
    public const int SC_ELEMENT_HIDDEN_LINE = 81;
    
    /// <summary>
    /// <code>set void SetElementColour=2753(Element element, colouralpha colourElement)</code>
    /// Set the colour of an element. Translucency (alpha) may or may not be significant
    /// and this may depend on the platform. The alpha byte should commonly be 0xff for opaque.
    /// </summary>
    public const int SCI_SETELEMENTCOLOUR = 2753;
    
    /// <summary>
    /// <code>get colouralpha GetElementColour=2754(Element element, )</code>
    /// Get the colour of an element.
    /// </summary>
    public const int SCI_GETELEMENTCOLOUR = 2754;
    
    /// <summary>
    /// <code>fun void ResetElementColour=2755(Element element, )</code>
    /// Use the default or platform-defined colour for an element.
    /// </summary>
    public const int SCI_RESETELEMENTCOLOUR = 2755;
    
    /// <summary>
    /// <code>get bool GetElementIsSet=2756(Element element, )</code>
    /// Get whether an element has been set by SetElementColour.
    /// When false, a platform-defined or default colour is used.
    /// </summary>
    public const int SCI_GETELEMENTISSET = 2756;
    
    /// <summary>
    /// <code>get bool GetElementAllowsTranslucent=2757(Element element, )</code>
    /// Get whether an element supports translucency.
    /// </summary>
    public const int SCI_GETELEMENTALLOWSTRANSLUCENT = 2757;
    
    /// <summary>
    /// <code>get colouralpha GetElementBaseColour=2758(Element element, )</code>
    /// Get the colour of an element.
    /// </summary>
    public const int SCI_GETELEMENTBASECOLOUR = 2758;
    
    /// <summary>
    /// <code>fun void SetSelFore=2067(bool useSetting, colour fore)</code>
    /// Set the foreground colour of the main and additional selections and whether to use this setting.
    /// </summary>
    public const int SCI_SETSELFORE = 2067;
    
    /// <summary>
    /// <code>fun void SetSelBack=2068(bool useSetting, colour back)</code>
    /// Set the background colour of the main and additional selections and whether to use this setting.
    /// </summary>
    public const int SCI_SETSELBACK = 2068;
    
    /// <summary>
    /// <code>get Alpha GetSelAlpha=2477(, )</code>
    /// Get the alpha of the selection.
    /// </summary>
    public const int SCI_GETSELALPHA = 2477;
    
    /// <summary>
    /// <code>set void SetSelAlpha=2478(Alpha alpha, )</code>
    /// Set the alpha of the selection.
    /// </summary>
    public const int SCI_SETSELALPHA = 2478;
    
    /// <summary>
    /// <code>get bool GetSelEOLFilled=2479(, )</code>
    /// Is the selection end of line filled?
    /// </summary>
    public const int SCI_GETSELEOLFILLED = 2479;
    
    /// <summary>
    /// <code>set void SetSelEOLFilled=2480(bool filled, )</code>
    /// Set the selection to have its end of line filled or not.
    /// </summary>
    public const int SCI_SETSELEOLFILLED = 2480;
    
    // Layer
    // =====
    public const int SC_LAYER_BASE = 0;
    public const int SC_LAYER_UNDER_TEXT = 1;
    public const int SC_LAYER_OVER_TEXT = 2;
    
    /// <summary>
    /// <code>get Layer GetSelectionLayer=2762(, )</code>
    /// Get the layer for drawing selections
    /// </summary>
    public const int SCI_GETSELECTIONLAYER = 2762;
    
    /// <summary>
    /// <code>set void SetSelectionLayer=2763(Layer layer, )</code>
    /// Set the layer for drawing selections: either opaquely on base layer or translucently over text
    /// </summary>
    public const int SCI_SETSELECTIONLAYER = 2763;
    
    /// <summary>
    /// <code>get Layer GetCaretLineLayer=2764(, )</code>
    /// Get the layer of the background of the line containing the caret.
    /// </summary>
    public const int SCI_GETCARETLINELAYER = 2764;
    
    /// <summary>
    /// <code>set void SetCaretLineLayer=2765(Layer layer, )</code>
    /// Set the layer of the background of the line containing the caret.
    /// </summary>
    public const int SCI_SETCARETLINELAYER = 2765;
    
    /// <summary>
    /// <code>get bool GetCaretLineHighlightSubLine=2773(, )</code>
    /// Get only highlighting subline instead of whole line.
    /// </summary>
    public const int SCI_GETCARETLINEHIGHLIGHTSUBLINE = 2773;
    
    /// <summary>
    /// <code>set void SetCaretLineHighlightSubLine=2774(bool subLine, )</code>
    /// Set only highlighting subline instead of whole line.
    /// </summary>
    public const int SCI_SETCARETLINEHIGHLIGHTSUBLINE = 2774;
    
    /// <summary>
    /// <code>set void SetCaretFore=2069(colour fore, )</code>
    /// Set the foreground colour of the caret.
    /// </summary>
    public const int SCI_SETCARETFORE = 2069;
    
    /// <summary>
    /// <code>fun void AssignCmdKey=2070(keymod keyDefinition, int sciCommand)</code>
    /// When key+modifier combination keyDefinition is pressed perform sciCommand.
    /// </summary>
    public const int SCI_ASSIGNCMDKEY = 2070;
    
    /// <summary>
    /// <code>fun void ClearCmdKey=2071(keymod keyDefinition, )</code>
    /// When key+modifier combination keyDefinition is pressed do nothing.
    /// </summary>
    public const int SCI_CLEARCMDKEY = 2071;
    
    /// <summary>
    /// <code>fun void ClearAllCmdKeys=2072(, )</code>
    /// Drop all key mappings.
    /// </summary>
    public const int SCI_CLEARALLCMDKEYS = 2072;
    
    /// <summary>
    /// <code>fun void SetStylingEx=2073(position length, string styles)</code>
    /// Set the styles for a segment of the document.
    /// </summary>
    public const int SCI_SETSTYLINGEX = 2073;
    
    /// <summary>
    /// <code>set void StyleSetVisible=2074(int style, bool visible)</code>
    /// Set a style to be visible or not.
    /// </summary>
    public const int SCI_STYLESETVISIBLE = 2074;
    
    /// <summary>
    /// <code>get int GetCaretPeriod=2075(, )</code>
    /// Get the time in milliseconds that the caret is on and off.
    /// </summary>
    public const int SCI_GETCARETPERIOD = 2075;
    
    /// <summary>
    /// <code>set void SetCaretPeriod=2076(int periodMilliseconds, )</code>
    /// Get the time in milliseconds that the caret is on and off. 0 = steady on.
    /// </summary>
    public const int SCI_SETCARETPERIOD = 2076;
    
    /// <summary>
    /// <code>set void SetWordChars=2077(, string characters)</code>
    /// Set the set of characters making up words for when moving or selecting by word.
    /// First sets defaults like SetCharsDefault.
    /// </summary>
    public const int SCI_SETWORDCHARS = 2077;
    
    /// <summary>
    /// <code>get int GetWordChars=2646(, stringresult characters)</code>
    /// Get the set of characters making up words for when moving or selecting by word.
    /// Returns the number of characters
    /// </summary>
    public const int SCI_GETWORDCHARS = 2646;
    
    /// <summary>
    /// <code>set void SetCharacterCategoryOptimization=2720(int countCharacters, )</code>
    /// Set the number of characters to have directly indexed categories
    /// </summary>
    public const int SCI_SETCHARACTERCATEGORYOPTIMIZATION = 2720;
    
    /// <summary>
    /// <code>get int GetCharacterCategoryOptimization=2721(, )</code>
    /// Get the number of characters to have directly indexed categories
    /// </summary>
    public const int SCI_GETCHARACTERCATEGORYOPTIMIZATION = 2721;
    
    /// <summary>
    /// <code>fun void BeginUndoAction=2078(, )</code>
    /// Start a sequence of actions that is undone and redone as a unit.
    /// May be nested.
    /// </summary>
    public const int SCI_BEGINUNDOACTION = 2078;
    
    /// <summary>
    /// <code>fun void EndUndoAction=2079(, )</code>
    /// End a sequence of actions that is undone and redone as a unit.
    /// </summary>
    public const int SCI_ENDUNDOACTION = 2079;
    
    /// <summary>
    /// <code>get int GetUndoSequence=2799(, )</code>
    /// Is an undo sequence active?
    /// </summary>
    public const int SCI_GETUNDOSEQUENCE = 2799;
    
    /// <summary>
    /// <code>get int GetUndoActions=2790(, )</code>
    /// How many undo actions are in the history?
    /// </summary>
    public const int SCI_GETUNDOACTIONS = 2790;
    
    /// <summary>
    /// <code>set void SetUndoSavePoint=2791(int action, )</code>
    /// Set action as the save point
    /// </summary>
    public const int SCI_SETUNDOSAVEPOINT = 2791;
    
    /// <summary>
    /// <code>get int GetUndoSavePoint=2792(, )</code>
    /// Which action is the save point?
    /// </summary>
    public const int SCI_GETUNDOSAVEPOINT = 2792;
    
    /// <summary>
    /// <code>set void SetUndoDetach=2793(int action, )</code>
    /// Set action as the detach point
    /// </summary>
    public const int SCI_SETUNDODETACH = 2793;
    
    /// <summary>
    /// <code>get int GetUndoDetach=2794(, )</code>
    /// Which action is the detach point?
    /// </summary>
    public const int SCI_GETUNDODETACH = 2794;
    
    /// <summary>
    /// <code>set void SetUndoTentative=2795(int action, )</code>
    /// Set action as the tentative point
    /// </summary>
    public const int SCI_SETUNDOTENTATIVE = 2795;
    
    /// <summary>
    /// <code>get int GetUndoTentative=2796(, )</code>
    /// Which action is the tentative point?
    /// </summary>
    public const int SCI_GETUNDOTENTATIVE = 2796;
    
    /// <summary>
    /// <code>set void SetUndoCurrent=2797(int action, )</code>
    /// Set action as the current point
    /// </summary>
    public const int SCI_SETUNDOCURRENT = 2797;
    
    /// <summary>
    /// <code>get int GetUndoCurrent=2798(, )</code>
    /// Which action is the current point?
    /// </summary>
    public const int SCI_GETUNDOCURRENT = 2798;
    
    /// <summary>
    /// <code>fun void PushUndoActionType=2800(int type, position pos)</code>
    /// Push one action onto undo history with no text
    /// </summary>
    public const int SCI_PUSHUNDOACTIONTYPE = 2800;
    
    /// <summary>
    /// <code>fun void ChangeLastUndoActionText=2801(position length, string text)</code>
    /// Set the text and length of the most recently pushed action
    /// </summary>
    public const int SCI_CHANGELASTUNDOACTIONTEXT = 2801;
    
    /// <summary>
    /// <code>get int GetUndoActionType=2802(int action, )</code>
    /// What is the type of an action?
    /// </summary>
    public const int SCI_GETUNDOACTIONTYPE = 2802;
    
    /// <summary>
    /// <code>get position GetUndoActionPosition=2803(int action, )</code>
    /// What is the position of an action?
    /// </summary>
    public const int SCI_GETUNDOACTIONPOSITION = 2803;
    
    /// <summary>
    /// <code>get int GetUndoActionText=2804(int action, stringresult text)</code>
    /// What is the text of an action?
    /// </summary>
    public const int SCI_GETUNDOACTIONTEXT = 2804;
    
    // IndicatorStyle
    // ==============
    public const int INDIC_PLAIN = 0;
    public const int INDIC_SQUIGGLE = 1;
    public const int INDIC_TT = 2;
    public const int INDIC_DIAGONAL = 3;
    public const int INDIC_STRIKE = 4;
    public const int INDIC_HIDDEN = 5;
    public const int INDIC_BOX = 6;
    public const int INDIC_ROUNDBOX = 7;
    public const int INDIC_STRAIGHTBOX = 8;
    public const int INDIC_DASH = 9;
    public const int INDIC_DOTS = 10;
    public const int INDIC_SQUIGGLELOW = 11;
    public const int INDIC_DOTBOX = 12;
    public const int INDIC_SQUIGGLEPIXMAP = 13;
    public const int INDIC_COMPOSITIONTHICK = 14;
    public const int INDIC_COMPOSITIONTHIN = 15;
    public const int INDIC_FULLBOX = 16;
    public const int INDIC_TEXTFORE = 17;
    public const int INDIC_POINT = 18;
    public const int INDIC_POINTCHARACTER = 19;
    public const int INDIC_GRADIENT = 20;
    public const int INDIC_GRADIENTCENTRE = 21;
    public const int INDIC_POINT_TOP = 22;
    /// <summary>
    /// INDIC_CONTAINER, INDIC_IME, INDIC_IME_MAX, and INDIC_MAX are indicator numbers,
    /// not IndicatorStyles so should not really be in the INDIC_ enumeration.
    /// They are redeclared in IndicatorNumbers INDICATOR_.
    /// </summary>
    public const int INDIC_CONTAINER = 8;
    public const int INDIC_IME = 32;
    public const int INDIC_IME_MAX = 35;
    public const int INDIC_MAX = 35;
    
    // IndicatorNumbers
    // ================
    public const int INDICATOR_CONTAINER = 8;
    public const int INDICATOR_IME = 32;
    public const int INDICATOR_IME_MAX = 35;
    public const int INDICATOR_HISTORY_REVERTED_TO_ORIGIN_INSERTION = 36;
    public const int INDICATOR_HISTORY_REVERTED_TO_ORIGIN_DELETION = 37;
    public const int INDICATOR_HISTORY_SAVED_INSERTION = 38;
    public const int INDICATOR_HISTORY_SAVED_DELETION = 39;
    public const int INDICATOR_HISTORY_MODIFIED_INSERTION = 40;
    public const int INDICATOR_HISTORY_MODIFIED_DELETION = 41;
    public const int INDICATOR_HISTORY_REVERTED_TO_MODIFIED_INSERTION = 42;
    public const int INDICATOR_HISTORY_REVERTED_TO_MODIFIED_DELETION = 43;
    public const int INDICATOR_MAX = 43;
    
    /// <summary>
    /// <code>set void IndicSetStyle=2080(int indicator, IndicatorStyle indicatorStyle)</code>
    /// Set an indicator to plain, squiggle or TT.
    /// </summary>
    public const int SCI_INDICSETSTYLE = 2080;
    
    /// <summary>
    /// <code>get IndicatorStyle IndicGetStyle=2081(int indicator, )</code>
    /// Retrieve the style of an indicator.
    /// </summary>
    public const int SCI_INDICGETSTYLE = 2081;
    
    /// <summary>
    /// <code>set void IndicSetFore=2082(int indicator, colour fore)</code>
    /// Set the foreground colour of an indicator.
    /// </summary>
    public const int SCI_INDICSETFORE = 2082;
    
    /// <summary>
    /// <code>get colour IndicGetFore=2083(int indicator, )</code>
    /// Retrieve the foreground colour of an indicator.
    /// </summary>
    public const int SCI_INDICGETFORE = 2083;
    
    /// <summary>
    /// <code>set void IndicSetUnder=2510(int indicator, bool under)</code>
    /// Set an indicator to draw under text or over(default).
    /// </summary>
    public const int SCI_INDICSETUNDER = 2510;
    
    /// <summary>
    /// <code>get bool IndicGetUnder=2511(int indicator, )</code>
    /// Retrieve whether indicator drawn under or over text.
    /// </summary>
    public const int SCI_INDICGETUNDER = 2511;
    
    /// <summary>
    /// <code>set void IndicSetHoverStyle=2680(int indicator, IndicatorStyle indicatorStyle)</code>
    /// Set a hover indicator to plain, squiggle or TT.
    /// </summary>
    public const int SCI_INDICSETHOVERSTYLE = 2680;
    
    /// <summary>
    /// <code>get IndicatorStyle IndicGetHoverStyle=2681(int indicator, )</code>
    /// Retrieve the hover style of an indicator.
    /// </summary>
    public const int SCI_INDICGETHOVERSTYLE = 2681;
    
    /// <summary>
    /// <code>set void IndicSetHoverFore=2682(int indicator, colour fore)</code>
    /// Set the foreground hover colour of an indicator.
    /// </summary>
    public const int SCI_INDICSETHOVERFORE = 2682;
    
    /// <summary>
    /// <code>get colour IndicGetHoverFore=2683(int indicator, )</code>
    /// Retrieve the foreground hover colour of an indicator.
    /// </summary>
    public const int SCI_INDICGETHOVERFORE = 2683;
    
    // IndicValue
    // ==========
    public const uint SC_INDICVALUEBIT = 0x1000000;
    public const uint SC_INDICVALUEMASK = 0xFFFFFF;
    
    // IndicFlag
    // =========
    public const int SC_INDICFLAG_NONE = 0;
    public const int SC_INDICFLAG_VALUEFORE = 1;
    
    /// <summary>
    /// <code>set void IndicSetFlags=2684(int indicator, IndicFlag flags)</code>
    /// Set the attributes of an indicator.
    /// </summary>
    public const int SCI_INDICSETFLAGS = 2684;
    
    /// <summary>
    /// <code>get IndicFlag IndicGetFlags=2685(int indicator, )</code>
    /// Retrieve the attributes of an indicator.
    /// </summary>
    public const int SCI_INDICGETFLAGS = 2685;
    
    /// <summary>
    /// <code>set void IndicSetStrokeWidth=2751(int indicator, int hundredths)</code>
    /// Set the stroke width of an indicator in hundredths of a pixel.
    /// </summary>
    public const int SCI_INDICSETSTROKEWIDTH = 2751;
    
    /// <summary>
    /// <code>get int IndicGetStrokeWidth=2752(int indicator, )</code>
    /// Retrieve the stroke width of an indicator.
    /// </summary>
    public const int SCI_INDICGETSTROKEWIDTH = 2752;
    
    /// <summary>
    /// <code>fun void SetWhitespaceFore=2084(bool useSetting, colour fore)</code>
    /// Set the foreground colour of all whitespace and whether to use this setting.
    /// </summary>
    public const int SCI_SETWHITESPACEFORE = 2084;
    
    /// <summary>
    /// <code>fun void SetWhitespaceBack=2085(bool useSetting, colour back)</code>
    /// Set the background colour of all whitespace and whether to use this setting.
    /// </summary>
    public const int SCI_SETWHITESPACEBACK = 2085;
    
    /// <summary>
    /// <code>set void SetWhitespaceSize=2086(int size, )</code>
    /// Set the size of the dots used to mark space characters.
    /// </summary>
    public const int SCI_SETWHITESPACESIZE = 2086;
    
    /// <summary>
    /// <code>get int GetWhitespaceSize=2087(, )</code>
    /// Get the size of the dots used to mark space characters.
    /// </summary>
    public const int SCI_GETWHITESPACESIZE = 2087;
    
    /// <summary>
    /// <code>set void SetLineState=2092(line line, int state)</code>
    /// Used to hold extra styling information for each line.
    /// </summary>
    public const int SCI_SETLINESTATE = 2092;
    
    /// <summary>
    /// <code>get int GetLineState=2093(line line, )</code>
    /// Retrieve the extra styling information for a line.
    /// </summary>
    public const int SCI_GETLINESTATE = 2093;
    
    /// <summary>
    /// <code>get int GetMaxLineState=2094(, )</code>
    /// Retrieve the last line number that has line state.
    /// </summary>
    public const int SCI_GETMAXLINESTATE = 2094;
    
    /// <summary>
    /// <code>get bool GetCaretLineVisible=2095(, )</code>
    /// Is the background of the line containing the caret in a different colour?
    /// </summary>
    public const int SCI_GETCARETLINEVISIBLE = 2095;
    
    /// <summary>
    /// <code>set void SetCaretLineVisible=2096(bool show, )</code>
    /// Display the background of the line containing the caret in a different colour.
    /// </summary>
    public const int SCI_SETCARETLINEVISIBLE = 2096;
    
    /// <summary>
    /// <code>get colour GetCaretLineBack=2097(, )</code>
    /// Get the colour of the background of the line containing the caret.
    /// </summary>
    public const int SCI_GETCARETLINEBACK = 2097;
    
    /// <summary>
    /// <code>set void SetCaretLineBack=2098(colour back, )</code>
    /// Set the colour of the background of the line containing the caret.
    /// </summary>
    public const int SCI_SETCARETLINEBACK = 2098;
    
    /// <summary>
    /// <code>get int GetCaretLineFrame=2704(, )</code>
    /// Retrieve the caret line frame width.
    /// Width = 0 means this option is disabled.
    /// </summary>
    public const int SCI_GETCARETLINEFRAME = 2704;
    
    /// <summary>
    /// <code>set void SetCaretLineFrame=2705(int width, )</code>
    /// Display the caret line framed.
    /// Set width != 0 to enable this option and width = 0 to disable it.
    /// </summary>
    public const int SCI_SETCARETLINEFRAME = 2705;
    
    /// <summary>
    /// <code>set void StyleSetChangeable=2099(int style, bool changeable)</code>
    /// Set a style to be changeable or not (read only).
    /// Experimental feature, currently buggy.
    /// </summary>
    public const int SCI_STYLESETCHANGEABLE = 2099;
    
    /// <summary>
    /// <code>fun void AutoCShow=2100(position lengthEntered, string itemList)</code>
    /// Display a auto-completion list.
    /// The lengthEntered parameter indicates how many characters before
    /// the caret should be used to provide context.
    /// </summary>
    public const int SCI_AUTOCSHOW = 2100;
    
    /// <summary>
    /// <code>fun void AutoCCancel=2101(, )</code>
    /// Remove the auto-completion list from the screen.
    /// </summary>
    public const int SCI_AUTOCCANCEL = 2101;
    
    /// <summary>
    /// <code>fun bool AutoCActive=2102(, )</code>
    /// Is there an auto-completion list visible?
    /// </summary>
    public const int SCI_AUTOCACTIVE = 2102;
    
    /// <summary>
    /// <code>fun position AutoCPosStart=2103(, )</code>
    /// Retrieve the position of the caret when the auto-completion list was displayed.
    /// </summary>
    public const int SCI_AUTOCPOSSTART = 2103;
    
    /// <summary>
    /// <code>fun void AutoCComplete=2104(, )</code>
    /// User has selected an item so remove the list and insert the selection.
    /// </summary>
    public const int SCI_AUTOCCOMPLETE = 2104;
    
    /// <summary>
    /// <code>fun void AutoCStops=2105(, string characterSet)</code>
    /// Define a set of character that when typed cancel the auto-completion list.
    /// </summary>
    public const int SCI_AUTOCSTOPS = 2105;
    
    /// <summary>
    /// <code>set void AutoCSetSeparator=2106(int separatorCharacter, )</code>
    /// Change the separator character in the string setting up an auto-completion list.
    /// Default is space but can be changed if items contain space.
    /// </summary>
    public const int SCI_AUTOCSETSEPARATOR = 2106;
    
    /// <summary>
    /// <code>get int AutoCGetSeparator=2107(, )</code>
    /// Retrieve the auto-completion list separator character.
    /// </summary>
    public const int SCI_AUTOCGETSEPARATOR = 2107;
    
    /// <summary>
    /// <code>fun void AutoCSelect=2108(, string select)</code>
    /// Select the item in the auto-completion list that starts with a string.
    /// </summary>
    public const int SCI_AUTOCSELECT = 2108;
    
    /// <summary>
    /// <code>set void AutoCSetCancelAtStart=2110(bool cancel, )</code>
    /// Should the auto-completion list be cancelled if the user backspaces to a
    /// position before where the box was created.
    /// </summary>
    public const int SCI_AUTOCSETCANCELATSTART = 2110;
    
    /// <summary>
    /// <code>get bool AutoCGetCancelAtStart=2111(, )</code>
    /// Retrieve whether auto-completion cancelled by backspacing before start.
    /// </summary>
    public const int SCI_AUTOCGETCANCELATSTART = 2111;
    
    /// <summary>
    /// <code>set void AutoCSetFillUps=2112(, string characterSet)</code>
    /// Define a set of characters that when typed will cause the autocompletion to
    /// choose the selected item.
    /// </summary>
    public const int SCI_AUTOCSETFILLUPS = 2112;
    
    /// <summary>
    /// <code>set void AutoCSetChooseSingle=2113(bool chooseSingle, )</code>
    /// Should a single item auto-completion list automatically choose the item.
    /// </summary>
    public const int SCI_AUTOCSETCHOOSESINGLE = 2113;
    
    /// <summary>
    /// <code>get bool AutoCGetChooseSingle=2114(, )</code>
    /// Retrieve whether a single item auto-completion list automatically choose the item.
    /// </summary>
    public const int SCI_AUTOCGETCHOOSESINGLE = 2114;
    
    /// <summary>
    /// <code>set void AutoCSetIgnoreCase=2115(bool ignoreCase, )</code>
    /// Set whether case is significant when performing auto-completion searches.
    /// </summary>
    public const int SCI_AUTOCSETIGNORECASE = 2115;
    
    /// <summary>
    /// <code>get bool AutoCGetIgnoreCase=2116(, )</code>
    /// Retrieve state of ignore case flag.
    /// </summary>
    public const int SCI_AUTOCGETIGNORECASE = 2116;
    
    /// <summary>
    /// <code>fun void UserListShow=2117(int listType, string itemList)</code>
    /// Display a list of strings and send notification when user chooses one.
    /// </summary>
    public const int SCI_USERLISTSHOW = 2117;
    
    /// <summary>
    /// <code>set void AutoCSetAutoHide=2118(bool autoHide, )</code>
    /// Set whether or not autocompletion is hidden automatically when nothing matches.
    /// </summary>
    public const int SCI_AUTOCSETAUTOHIDE = 2118;
    
    /// <summary>
    /// <code>get bool AutoCGetAutoHide=2119(, )</code>
    /// Retrieve whether or not autocompletion is hidden automatically when nothing matches.
    /// </summary>
    public const int SCI_AUTOCGETAUTOHIDE = 2119;
    
    // AutoCompleteOption
    // ==================
    public const int SC_AUTOCOMPLETE_NORMAL = 0;
    /// <summary>Win32 specific:</summary>
    public const int SC_AUTOCOMPLETE_FIXED_SIZE = 1;
    /// <summary>Always select the first item in the autocompletion list:</summary>
    public const int SC_AUTOCOMPLETE_SELECT_FIRST_ITEM = 2;
    
    /// <summary>
    /// <code>set void AutoCSetOptions=2638(AutoCompleteOption options, )</code>
    /// Set autocompletion options.
    /// </summary>
    public const int SCI_AUTOCSETOPTIONS = 2638;
    
    /// <summary>
    /// <code>get AutoCompleteOption AutoCGetOptions=2639(, )</code>
    /// Retrieve autocompletion options.
    /// </summary>
    public const int SCI_AUTOCGETOPTIONS = 2639;
    
    /// <summary>
    /// <code>set void AutoCSetDropRestOfWord=2270(bool dropRestOfWord, )</code>
    /// Set whether or not autocompletion deletes any word characters
    /// after the inserted text upon completion.
    /// </summary>
    public const int SCI_AUTOCSETDROPRESTOFWORD = 2270;
    
    /// <summary>
    /// <code>get bool AutoCGetDropRestOfWord=2271(, )</code>
    /// Retrieve whether or not autocompletion deletes any word characters
    /// after the inserted text upon completion.
    /// </summary>
    public const int SCI_AUTOCGETDROPRESTOFWORD = 2271;
    
    /// <summary>
    /// <code>fun void RegisterImage=2405(int type, string xpmData)</code>
    /// Register an XPM image for use in autocompletion lists.
    /// </summary>
    public const int SCI_REGISTERIMAGE = 2405;
    
    /// <summary>
    /// <code>fun void ClearRegisteredImages=2408(, )</code>
    /// Clear all the registered XPM images.
    /// </summary>
    public const int SCI_CLEARREGISTEREDIMAGES = 2408;
    
    /// <summary>
    /// <code>get int AutoCGetTypeSeparator=2285(, )</code>
    /// Retrieve the auto-completion list type-separator character.
    /// </summary>
    public const int SCI_AUTOCGETTYPESEPARATOR = 2285;
    
    /// <summary>
    /// <code>set void AutoCSetTypeSeparator=2286(int separatorCharacter, )</code>
    /// Change the type-separator character in the string setting up an auto-completion list.
    /// Default is '?' but can be changed if items contain '?'.
    /// </summary>
    public const int SCI_AUTOCSETTYPESEPARATOR = 2286;
    
    /// <summary>
    /// <code>set void AutoCSetMaxWidth=2208(int characterCount, )</code>
    /// Set the maximum width, in characters, of auto-completion and user lists.
    /// Set to 0 to autosize to fit longest item, which is the default.
    /// </summary>
    public const int SCI_AUTOCSETMAXWIDTH = 2208;
    
    /// <summary>
    /// <code>get int AutoCGetMaxWidth=2209(, )</code>
    /// Get the maximum width, in characters, of auto-completion and user lists.
    /// </summary>
    public const int SCI_AUTOCGETMAXWIDTH = 2209;
    
    /// <summary>
    /// <code>set void AutoCSetMaxHeight=2210(int rowCount, )</code>
    /// Set the maximum height, in rows, of auto-completion and user lists.
    /// The default is 5 rows.
    /// </summary>
    public const int SCI_AUTOCSETMAXHEIGHT = 2210;
    
    /// <summary>
    /// <code>get int AutoCGetMaxHeight=2211(, )</code>
    /// Set the maximum height, in rows, of auto-completion and user lists.
    /// </summary>
    public const int SCI_AUTOCGETMAXHEIGHT = 2211;
    
    /// <summary>
    /// <code>set void AutoCSetStyle=2109(int style, )</code>
    /// Set the style number used for auto-completion and user lists fonts.
    /// </summary>
    public const int SCI_AUTOCSETSTYLE = 2109;
    
    /// <summary>
    /// <code>get int AutoCGetStyle=2120(, )</code>
    /// Get the style number used for auto-completion and user lists fonts.
    /// </summary>
    public const int SCI_AUTOCGETSTYLE = 2120;
    
    /// <summary>
    /// <code>set void AutoCSetImageScale=2815(int scalePercent, )</code>
    /// Set the scale factor in percent for auto-completion list images.
    /// </summary>
    public const int SCI_AUTOCSETIMAGESCALE = 2815;
    
    /// <summary>
    /// <code>get int AutoCGetImageScale=2816(, )</code>
    /// Get the scale factor in percent for auto-completion list images.
    /// </summary>
    public const int SCI_AUTOCGETIMAGESCALE = 2816;
    
    /// <summary>
    /// <code>set void SetIndent=2122(int indentSize, )</code>
    /// Set the number of spaces used for one level of indentation.
    /// </summary>
    public const int SCI_SETINDENT = 2122;
    
    /// <summary>
    /// <code>get int GetIndent=2123(, )</code>
    /// Retrieve indentation size.
    /// </summary>
    public const int SCI_GETINDENT = 2123;
    
    /// <summary>
    /// <code>set void SetUseTabs=2124(bool useTabs, )</code>
    /// Indentation will only use space characters if useTabs is false, otherwise
    /// it will use a combination of tabs and spaces.
    /// </summary>
    public const int SCI_SETUSETABS = 2124;
    
    /// <summary>
    /// <code>get bool GetUseTabs=2125(, )</code>
    /// Retrieve whether tabs will be used in indentation.
    /// </summary>
    public const int SCI_GETUSETABS = 2125;
    
    /// <summary>
    /// <code>set void SetLineIndentation=2126(line line, int indentation)</code>
    /// Change the indentation of a line to a number of columns.
    /// </summary>
    public const int SCI_SETLINEINDENTATION = 2126;
    
    /// <summary>
    /// <code>get int GetLineIndentation=2127(line line, )</code>
    /// Retrieve the number of columns that a line is indented.
    /// </summary>
    public const int SCI_GETLINEINDENTATION = 2127;
    
    /// <summary>
    /// <code>get position GetLineIndentPosition=2128(line line, )</code>
    /// Retrieve the position before the first non indentation character on a line.
    /// </summary>
    public const int SCI_GETLINEINDENTPOSITION = 2128;
    
    /// <summary>
    /// <code>get position GetColumn=2129(position pos, )</code>
    /// Retrieve the column number of a position, taking tab width into account.
    /// </summary>
    public const int SCI_GETCOLUMN = 2129;
    
    /// <summary>
    /// <code>fun position CountCharacters=2633(position start, position end)</code>
    /// Count characters between two positions.
    /// </summary>
    public const int SCI_COUNTCHARACTERS = 2633;
    
    /// <summary>
    /// <code>fun position CountCodeUnits=2715(position start, position end)</code>
    /// Count code units between two positions.
    /// </summary>
    public const int SCI_COUNTCODEUNITS = 2715;
    
    /// <summary>
    /// <code>set void SetHScrollBar=2130(bool visible, )</code>
    /// Show or hide the horizontal scroll bar.
    /// </summary>
    public const int SCI_SETHSCROLLBAR = 2130;
    
    /// <summary>
    /// <code>get bool GetHScrollBar=2131(, )</code>
    /// Is the horizontal scroll bar visible?
    /// </summary>
    public const int SCI_GETHSCROLLBAR = 2131;
    
    // IndentView
    // ==========
    public const int SC_IV_NONE = 0;
    public const int SC_IV_REAL = 1;
    public const int SC_IV_LOOKFORWARD = 2;
    public const int SC_IV_LOOKBOTH = 3;
    
    /// <summary>
    /// <code>set void SetIndentationGuides=2132(IndentView indentView, )</code>
    /// Show or hide indentation guides.
    /// </summary>
    public const int SCI_SETINDENTATIONGUIDES = 2132;
    
    /// <summary>
    /// <code>get IndentView GetIndentationGuides=2133(, )</code>
    /// Are the indentation guides visible?
    /// </summary>
    public const int SCI_GETINDENTATIONGUIDES = 2133;
    
    /// <summary>
    /// <code>set void SetHighlightGuide=2134(position column, )</code>
    /// Set the highlighted indentation guide column.
    /// 0 = no highlighted guide.
    /// </summary>
    public const int SCI_SETHIGHLIGHTGUIDE = 2134;
    
    /// <summary>
    /// <code>get position GetHighlightGuide=2135(, )</code>
    /// Get the highlighted indentation guide column.
    /// </summary>
    public const int SCI_GETHIGHLIGHTGUIDE = 2135;
    
    /// <summary>
    /// <code>get position GetLineEndPosition=2136(line line, )</code>
    /// Get the position after the last visible characters on a line.
    /// </summary>
    public const int SCI_GETLINEENDPOSITION = 2136;
    
    /// <summary>
    /// <code>get int GetCodePage=2137(, )</code>
    /// Get the code page used to interpret the bytes of the document as characters.
    /// </summary>
    public const int SCI_GETCODEPAGE = 2137;
    
    /// <summary>
    /// <code>get colour GetCaretFore=2138(, )</code>
    /// Get the foreground colour of the caret.
    /// </summary>
    public const int SCI_GETCARETFORE = 2138;
    
    /// <summary>
    /// <code>get bool GetReadOnly=2140(, )</code>
    /// In read-only mode?
    /// </summary>
    public const int SCI_GETREADONLY = 2140;
    
    /// <summary>
    /// <code>set void SetCurrentPos=2141(position caret, )</code>
    /// Sets the position of the caret.
    /// </summary>
    public const int SCI_SETCURRENTPOS = 2141;
    
    /// <summary>
    /// <code>set void SetSelectionStart=2142(position anchor, )</code>
    /// Sets the position that starts the selection - this becomes the anchor.
    /// </summary>
    public const int SCI_SETSELECTIONSTART = 2142;
    
    /// <summary>
    /// <code>get position GetSelectionStart=2143(, )</code>
    /// Returns the position at the start of the selection.
    /// </summary>
    public const int SCI_GETSELECTIONSTART = 2143;
    
    /// <summary>
    /// <code>set void SetSelectionEnd=2144(position caret, )</code>
    /// Sets the position that ends the selection - this becomes the caret.
    /// </summary>
    public const int SCI_SETSELECTIONEND = 2144;
    
    /// <summary>
    /// <code>get position GetSelectionEnd=2145(, )</code>
    /// Returns the position at the end of the selection.
    /// </summary>
    public const int SCI_GETSELECTIONEND = 2145;
    
    /// <summary>
    /// <code>fun void SetEmptySelection=2556(position caret, )</code>
    /// Set caret to a position, while removing any existing selection.
    /// </summary>
    public const int SCI_SETEMPTYSELECTION = 2556;
    
    /// <summary>
    /// <code>set void SetPrintMagnification=2146(int magnification, )</code>
    /// Sets the print magnification added to the point size of each style for printing.
    /// </summary>
    public const int SCI_SETPRINTMAGNIFICATION = 2146;
    
    /// <summary>
    /// <code>get int GetPrintMagnification=2147(, )</code>
    /// Returns the print magnification.
    /// </summary>
    public const int SCI_GETPRINTMAGNIFICATION = 2147;
    
    // PrintOption
    // ===========
    /// <summary>
    /// PrintColourMode - use same colours as screen.
    /// with the exception of line number margins, which use a white background
    /// </summary>
    public const int SC_PRINT_NORMAL = 0;
    /// <summary>PrintColourMode - invert the light value of each style for printing.</summary>
    public const int SC_PRINT_INVERTLIGHT = 1;
    /// <summary>PrintColourMode - force black text on white background for printing.</summary>
    public const int SC_PRINT_BLACKONWHITE = 2;
    /// <summary>PrintColourMode - text stays coloured, but all background is forced to be white for printing.</summary>
    public const int SC_PRINT_COLOURONWHITE = 3;
    /// <summary>PrintColourMode - only the default-background is forced to be white for printing.</summary>
    public const int SC_PRINT_COLOURONWHITEDEFAULTBG = 4;
    /// <summary>PrintColourMode - use same colours as screen, including line number margins.</summary>
    public const int SC_PRINT_SCREENCOLOURS = 5;
    
    /// <summary>
    /// <code>set void SetPrintColourMode=2148(PrintOption mode, )</code>
    /// Modify colours when printing for clearer printed text.
    /// </summary>
    public const int SCI_SETPRINTCOLOURMODE = 2148;
    
    /// <summary>
    /// <code>get PrintOption GetPrintColourMode=2149(, )</code>
    /// Returns the print colour mode.
    /// </summary>
    public const int SCI_GETPRINTCOLOURMODE = 2149;
    
    // FindOption
    // ==========
    public const uint SCFIND_NONE = 0x0;
    public const uint SCFIND_WHOLEWORD = 0x2;
    public const uint SCFIND_MATCHCASE = 0x4;
    public const uint SCFIND_WORDSTART = 0x00100000;
    public const uint SCFIND_REGEXP = 0x00200000;
    public const uint SCFIND_POSIX = 0x00400000;
    public const uint SCFIND_CXX11REGEX = 0x00800000;
    
    /// <summary>
    /// <code>fun position FindText=2150(FindOption searchFlags, findtext ft)</code>
    /// Find some text in the document.
    /// </summary>
    public const int SCI_FINDTEXT = 2150;
    
    /// <summary>
    /// <code>fun position FindTextFull=2196(FindOption searchFlags, findtextfull ft)</code>
    /// Find some text in the document.
    /// </summary>
    public const int SCI_FINDTEXTFULL = 2196;
    
    /// <summary>
    /// <code>fun position FormatRange=2151(bool draw, formatrange fr)</code>
    /// Draw the document into a display context such as a printer.
    /// </summary>
    public const int SCI_FORMATRANGE = 2151;
    
    /// <summary>
    /// <code>fun position FormatRangeFull=2777(bool draw, formatrangefull fr)</code>
    /// Draw the document into a display context such as a printer.
    /// </summary>
    public const int SCI_FORMATRANGEFULL = 2777;
    
    // ChangeHistoryOption
    // ===================
    public const int SC_CHANGE_HISTORY_DISABLED = 0;
    public const int SC_CHANGE_HISTORY_ENABLED = 1;
    public const int SC_CHANGE_HISTORY_MARKERS = 2;
    public const int SC_CHANGE_HISTORY_INDICATORS = 4;
    
    /// <summary>
    /// <code>set void SetChangeHistory=2780(ChangeHistoryOption changeHistory, )</code>
    /// Enable or disable change history.
    /// </summary>
    public const int SCI_SETCHANGEHISTORY = 2780;
    
    /// <summary>
    /// <code>get ChangeHistoryOption GetChangeHistory=2781(, )</code>
    /// Report change history status.
    /// </summary>
    public const int SCI_GETCHANGEHISTORY = 2781;
    
    // UndoSelectionHistoryOption
    // ==========================
    public const int SC_UNDO_SELECTION_HISTORY_DISABLED = 0;
    public const int SC_UNDO_SELECTION_HISTORY_ENABLED = 1;
    public const int SC_UNDO_SELECTION_HISTORY_SCROLL = 2;
    
    /// <summary>
    /// <code>set void SetUndoSelectionHistory=2782(UndoSelectionHistoryOption undoSelectionHistory, )</code>
    /// Enable or disable undo selection history.
    /// </summary>
    public const int SCI_SETUNDOSELECTIONHISTORY = 2782;
    
    /// <summary>
    /// <code>get UndoSelectionHistoryOption GetUndoSelectionHistory=2783(, )</code>
    /// Report undo selection history status.
    /// </summary>
    public const int SCI_GETUNDOSELECTIONHISTORY = 2783;
    
    /// <summary>
    /// <code>set void SetSelectionSerialized=2784(, string selectionString)</code>
    /// Set selection from serialized form.
    /// </summary>
    public const int SCI_SETSELECTIONSERIALIZED = 2784;
    
    /// <summary>
    /// <code>get position GetSelectionSerialized=2785(, stringresult selectionString)</code>
    /// Retrieve serialized form of selection.
    /// </summary>
    public const int SCI_GETSELECTIONSERIALIZED = 2785;
    
    /// <summary>
    /// <code>get line GetFirstVisibleLine=2152(, )</code>
    /// Retrieve the display line at the top of the display.
    /// </summary>
    public const int SCI_GETFIRSTVISIBLELINE = 2152;
    
    /// <summary>
    /// <code>fun position GetLine=2153(line line, stringresult text)</code>
    /// Retrieve the contents of a line.
    /// Returns the length of the line.
    /// </summary>
    public const int SCI_GETLINE = 2153;
    
    /// <summary>
    /// <code>get line GetLineCount=2154(, )</code>
    /// Returns the number of lines in the document. There is always at least one.
    /// </summary>
    public const int SCI_GETLINECOUNT = 2154;
    
    /// <summary>
    /// <code>set void AllocateLines=2089(line lines, )</code>
    /// Enlarge the number of lines allocated.
    /// </summary>
    public const int SCI_ALLOCATELINES = 2089;
    
    /// <summary>
    /// <code>set void SetMarginLeft=2155(, int pixelWidth)</code>
    /// Sets the size in pixels of the left margin.
    /// </summary>
    public const int SCI_SETMARGINLEFT = 2155;
    
    /// <summary>
    /// <code>get int GetMarginLeft=2156(, )</code>
    /// Returns the size in pixels of the left margin.
    /// </summary>
    public const int SCI_GETMARGINLEFT = 2156;
    
    /// <summary>
    /// <code>set void SetMarginRight=2157(, int pixelWidth)</code>
    /// Sets the size in pixels of the right margin.
    /// </summary>
    public const int SCI_SETMARGINRIGHT = 2157;
    
    /// <summary>
    /// <code>get int GetMarginRight=2158(, )</code>
    /// Returns the size in pixels of the right margin.
    /// </summary>
    public const int SCI_GETMARGINRIGHT = 2158;
    
    /// <summary>
    /// <code>get bool GetModify=2159(, )</code>
    /// Is the document different from when it was last saved?
    /// </summary>
    public const int SCI_GETMODIFY = 2159;
    
    /// <summary>
    /// <code>fun void SetSel=2160(position anchor, position caret)</code>
    /// Select a range of text.
    /// </summary>
    public const int SCI_SETSEL = 2160;
    
    /// <summary>
    /// <code>fun position GetSelText=2161(, stringresult text)</code>
    /// Retrieve the selected text.
    /// Return the length of the text.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_GETSELTEXT = 2161;
    
    /// <summary>
    /// <code>fun position GetTextRange=2162(, textrange tr)</code>
    /// Retrieve a range of text.
    /// Return the length of the text.
    /// </summary>
    public const int SCI_GETTEXTRANGE = 2162;
    
    /// <summary>
    /// <code>fun position GetTextRangeFull=2039(, textrangefull tr)</code>
    /// Retrieve a range of text that can be past 2GB.
    /// Return the length of the text.
    /// </summary>
    public const int SCI_GETTEXTRANGEFULL = 2039;
    
    /// <summary>
    /// <code>fun void HideSelection=2163(bool hide, )</code>
    /// Draw the selection either highlighted or in normal (non-highlighted) style.
    /// </summary>
    public const int SCI_HIDESELECTION = 2163;
    
    /// <summary>
    /// <code>get bool GetSelectionHidden=2088(, )</code>
    /// </summary>
    public const int SCI_GETSELECTIONHIDDEN = 2088;
    
    /// <summary>
    /// <code>fun int PointXFromPosition=2164(, position pos)</code>
    /// Retrieve the x value of the point in the window where a position is displayed.
    /// </summary>
    public const int SCI_POINTXFROMPOSITION = 2164;
    
    /// <summary>
    /// <code>fun int PointYFromPosition=2165(, position pos)</code>
    /// Retrieve the y value of the point in the window where a position is displayed.
    /// </summary>
    public const int SCI_POINTYFROMPOSITION = 2165;
    
    /// <summary>
    /// <code>fun line LineFromPosition=2166(position pos, )</code>
    /// Retrieve the line containing a position.
    /// </summary>
    public const int SCI_LINEFROMPOSITION = 2166;
    
    /// <summary>
    /// <code>fun position PositionFromLine=2167(line line, )</code>
    /// Retrieve the position at the start of a line.
    /// </summary>
    public const int SCI_POSITIONFROMLINE = 2167;
    
    /// <summary>
    /// <code>fun void LineScroll=2168(position columns, line lines)</code>
    /// Scroll horizontally and vertically.
    /// </summary>
    public const int SCI_LINESCROLL = 2168;
    
    /// <summary>
    /// <code>fun void ScrollVertical=2817(line docLine, line subLine)</code>
    /// Scroll vertically with allowance for wrapping.
    /// </summary>
    public const int SCI_SCROLLVERTICAL = 2817;
    
    /// <summary>
    /// <code>fun void ScrollCaret=2169(, )</code>
    /// Ensure the caret is visible.
    /// </summary>
    public const int SCI_SCROLLCARET = 2169;
    
    /// <summary>
    /// <code>fun void ScrollRange=2569(position secondary, position primary)</code>
    /// Scroll the argument positions and the range between them into view giving
    /// priority to the primary position then the secondary position.
    /// This may be used to make a search match visible.
    /// </summary>
    public const int SCI_SCROLLRANGE = 2569;
    
    /// <summary>
    /// <code>fun void ReplaceSel=2170(, string text)</code>
    /// Replace the selected text with the argument text.
    /// </summary>
    public const int SCI_REPLACESEL = 2170;
    
    /// <summary>
    /// <code>set void SetReadOnly=2171(bool readOnly, )</code>
    /// Set to read only or read write.
    /// </summary>
    public const int SCI_SETREADONLY = 2171;
    
    /// <summary>
    /// <code>fun void Null=2172(, )</code>
    /// Null operation.
    /// </summary>
    public const int SCI_NULL = 2172;
    
    /// <summary>
    /// <code>fun bool CanPaste=2173(, )</code>
    /// Will a paste succeed?
    /// </summary>
    public const int SCI_CANPASTE = 2173;
    
    /// <summary>
    /// <code>fun bool CanUndo=2174(, )</code>
    /// Are there any undoable actions in the undo history?
    /// </summary>
    public const int SCI_CANUNDO = 2174;
    
    /// <summary>
    /// <code>fun void EmptyUndoBuffer=2175(, )</code>
    /// Delete the undo history.
    /// </summary>
    public const int SCI_EMPTYUNDOBUFFER = 2175;
    
    /// <summary>
    /// <code>fun void Undo=2176(, )</code>
    /// Undo one action in the undo history.
    /// </summary>
    public const int SCI_UNDO = 2176;
    
    /// <summary>
    /// <code>fun void Cut=2177(, )</code>
    /// Cut the selection to the clipboard.
    /// </summary>
    public const int SCI_CUT = 2177;
    
    /// <summary>
    /// <code>fun void Copy=2178(, )</code>
    /// Copy the selection to the clipboard.
    /// </summary>
    public const int SCI_COPY = 2178;
    
    /// <summary>
    /// <code>fun void Paste=2179(, )</code>
    /// Paste the contents of the clipboard into the document replacing the selection.
    /// </summary>
    public const int SCI_PASTE = 2179;
    
    /// <summary>
    /// <code>fun void Clear=2180(, )</code>
    /// Clear the selection.
    /// </summary>
    public const int SCI_CLEAR = 2180;
    
    /// <summary>
    /// <code>fun void SetText=2181(, string text)</code>
    /// Replace the contents of the document with the argument text.
    /// </summary>
    public const int SCI_SETTEXT = 2181;
    
    /// <summary>
    /// <code>fun position GetText=2182(position length, stringresult text)</code>
    /// Retrieve all the text in the document.
    /// Returns number of characters retrieved.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_GETTEXT = 2182;
    
    /// <summary>
    /// <code>get position GetTextLength=2183(, )</code>
    /// Retrieve the number of characters in the document.
    /// </summary>
    public const int SCI_GETTEXTLENGTH = 2183;
    
    /// <summary>
    /// <code>get pointer GetDirectFunction=2184(, )</code>
    /// Retrieve a pointer to a function that processes messages for this Scintilla.
    /// </summary>
    public const int SCI_GETDIRECTFUNCTION = 2184;
    
    /// <summary>
    /// <code>get pointer GetDirectStatusFunction=2772(, )</code>
    /// Retrieve a pointer to a function that processes messages for this Scintilla and returns status.
    /// </summary>
    public const int SCI_GETDIRECTSTATUSFUNCTION = 2772;
    
    /// <summary>
    /// <code>get pointer GetDirectPointer=2185(, )</code>
    /// Retrieve a pointer value to use as the first argument when calling
    /// the function returned by GetDirectFunction.
    /// </summary>
    public const int SCI_GETDIRECTPOINTER = 2185;
    
    /// <summary>
    /// <code>set void SetOvertype=2186(bool overType, )</code>
    /// Set to overtype (true) or insert mode.
    /// </summary>
    public const int SCI_SETOVERTYPE = 2186;
    
    /// <summary>
    /// <code>get bool GetOvertype=2187(, )</code>
    /// Returns true if overtype mode is active otherwise false is returned.
    /// </summary>
    public const int SCI_GETOVERTYPE = 2187;
    
    /// <summary>
    /// <code>set void SetCaretWidth=2188(int pixelWidth, )</code>
    /// Set the width of the insert mode caret.
    /// </summary>
    public const int SCI_SETCARETWIDTH = 2188;
    
    /// <summary>
    /// <code>get int GetCaretWidth=2189(, )</code>
    /// Returns the width of the insert mode caret.
    /// </summary>
    public const int SCI_GETCARETWIDTH = 2189;
    
    /// <summary>
    /// <code>set void SetTargetStart=2190(position start, )</code>
    /// Sets the position that starts the target which is used for updating the
    /// document without affecting the scroll position.
    /// </summary>
    public const int SCI_SETTARGETSTART = 2190;
    
    /// <summary>
    /// <code>get position GetTargetStart=2191(, )</code>
    /// Get the position that starts the target.
    /// </summary>
    public const int SCI_GETTARGETSTART = 2191;
    
    /// <summary>
    /// <code>set void SetTargetStartVirtualSpace=2728(position space, )</code>
    /// Sets the virtual space of the target start
    /// </summary>
    public const int SCI_SETTARGETSTARTVIRTUALSPACE = 2728;
    
    /// <summary>
    /// <code>get position GetTargetStartVirtualSpace=2729(, )</code>
    /// Get the virtual space of the target start
    /// </summary>
    public const int SCI_GETTARGETSTARTVIRTUALSPACE = 2729;
    
    /// <summary>
    /// <code>set void SetTargetEnd=2192(position end, )</code>
    /// Sets the position that ends the target which is used for updating the
    /// document without affecting the scroll position.
    /// </summary>
    public const int SCI_SETTARGETEND = 2192;
    
    /// <summary>
    /// <code>get position GetTargetEnd=2193(, )</code>
    /// Get the position that ends the target.
    /// </summary>
    public const int SCI_GETTARGETEND = 2193;
    
    /// <summary>
    /// <code>set void SetTargetEndVirtualSpace=2730(position space, )</code>
    /// Sets the virtual space of the target end
    /// </summary>
    public const int SCI_SETTARGETENDVIRTUALSPACE = 2730;
    
    /// <summary>
    /// <code>get position GetTargetEndVirtualSpace=2731(, )</code>
    /// Get the virtual space of the target end
    /// </summary>
    public const int SCI_GETTARGETENDVIRTUALSPACE = 2731;
    
    /// <summary>
    /// <code>fun void SetTargetRange=2686(position start, position end)</code>
    /// Sets both the start and end of the target in one call.
    /// </summary>
    public const int SCI_SETTARGETRANGE = 2686;
    
    /// <summary>
    /// <code>get position GetTargetText=2687(, stringresult text)</code>
    /// Retrieve the text in the target.
    /// </summary>
    public const int SCI_GETTARGETTEXT = 2687;
    
    /// <summary>
    /// <code>fun void TargetFromSelection=2287(, )</code>
    /// Make the target range start and end be the same as the selection range start and end.
    /// </summary>
    public const int SCI_TARGETFROMSELECTION = 2287;
    
    /// <summary>
    /// <code>fun void TargetWholeDocument=2690(, )</code>
    /// Sets the target to the whole document.
    /// </summary>
    public const int SCI_TARGETWHOLEDOCUMENT = 2690;
    
    /// <summary>
    /// <code>fun position ReplaceTarget=2194(position length, string text)</code>
    /// Replace the target text with the argument text.
    /// Text is counted so it can contain NULs.
    /// Returns the length of the replacement text.
    /// </summary>
    public const int SCI_REPLACETARGET = 2194;
    
    /// <summary>
    /// <code>fun position ReplaceTargetRE=2195(position length, string text)</code>
    /// Replace the target text with the argument text after \d processing.
    /// Text is counted so it can contain NULs.
    /// Looks for \d where d is between 1 and 9 and replaces these with the strings
    /// matched in the last search operation which were surrounded by \( and \).
    /// Returns the length of the replacement text including any change
    /// caused by processing the \d patterns.
    /// </summary>
    public const int SCI_REPLACETARGETRE = 2195;
    
    /// <summary>
    /// <code>fun position ReplaceTargetMinimal=2779(position length, string text)</code>
    /// Replace the target text with the argument text but ignore prefix and suffix that
    /// are the same as current.
    /// </summary>
    public const int SCI_REPLACETARGETMINIMAL = 2779;
    
    /// <summary>
    /// <code>fun position SearchInTarget=2197(position length, string text)</code>
    /// Search for a counted string in the target and set the target to the found
    /// range. Text is counted so it can contain NULs.
    /// Returns start of found range or -1 for failure in which case target is not moved.
    /// </summary>
    public const int SCI_SEARCHINTARGET = 2197;
    
    /// <summary>
    /// <code>set void SetSearchFlags=2198(FindOption searchFlags, )</code>
    /// Set the search flags used by SearchInTarget.
    /// </summary>
    public const int SCI_SETSEARCHFLAGS = 2198;
    
    /// <summary>
    /// <code>get FindOption GetSearchFlags=2199(, )</code>
    /// Get the search flags used by SearchInTarget.
    /// </summary>
    public const int SCI_GETSEARCHFLAGS = 2199;
    
    /// <summary>
    /// <code>fun void CallTipShow=2200(position pos, string definition)</code>
    /// Show a call tip containing a definition near position pos.
    /// </summary>
    public const int SCI_CALLTIPSHOW = 2200;
    
    /// <summary>
    /// <code>fun void CallTipCancel=2201(, )</code>
    /// Remove the call tip from the screen.
    /// </summary>
    public const int SCI_CALLTIPCANCEL = 2201;
    
    /// <summary>
    /// <code>fun bool CallTipActive=2202(, )</code>
    /// Is there an active call tip?
    /// </summary>
    public const int SCI_CALLTIPACTIVE = 2202;
    
    /// <summary>
    /// <code>get position CallTipPosStart=2203(, )</code>
    /// Retrieve the position where the caret was before displaying the call tip.
    /// </summary>
    public const int SCI_CALLTIPPOSSTART = 2203;
    
    /// <summary>
    /// <code>set void CallTipSetPosStart=2214(position posStart, )</code>
    /// Set the start position in order to change when backspacing removes the calltip.
    /// </summary>
    public const int SCI_CALLTIPSETPOSSTART = 2214;
    
    /// <summary>
    /// <code>fun void CallTipSetHlt=2204(position highlightStart, position highlightEnd)</code>
    /// Highlight a segment of the definition.
    /// </summary>
    public const int SCI_CALLTIPSETHLT = 2204;
    
    /// <summary>
    /// <code>set void CallTipSetBack=2205(colour back, )</code>
    /// Set the background colour for the call tip.
    /// </summary>
    public const int SCI_CALLTIPSETBACK = 2205;
    
    /// <summary>
    /// <code>set void CallTipSetFore=2206(colour fore, )</code>
    /// Set the foreground colour for the call tip.
    /// </summary>
    public const int SCI_CALLTIPSETFORE = 2206;
    
    /// <summary>
    /// <code>set void CallTipSetForeHlt=2207(colour fore, )</code>
    /// Set the foreground colour for the highlighted part of the call tip.
    /// </summary>
    public const int SCI_CALLTIPSETFOREHLT = 2207;
    
    /// <summary>
    /// <code>set void CallTipUseStyle=2212(int tabSize, )</code>
    /// Enable use of STYLE_CALLTIP and set call tip tab size in pixels.
    /// </summary>
    public const int SCI_CALLTIPUSESTYLE = 2212;
    
    /// <summary>
    /// <code>set void CallTipSetPosition=2213(bool above, )</code>
    /// Set position of calltip, above or below text.
    /// </summary>
    public const int SCI_CALLTIPSETPOSITION = 2213;
    
    /// <summary>
    /// <code>fun line VisibleFromDocLine=2220(line docLine, )</code>
    /// Find the display line of a document line taking hidden lines into account.
    /// </summary>
    public const int SCI_VISIBLEFROMDOCLINE = 2220;
    
    /// <summary>
    /// <code>fun line DocLineFromVisible=2221(line displayLine, )</code>
    /// Find the document line of a display line taking hidden lines into account.
    /// </summary>
    public const int SCI_DOCLINEFROMVISIBLE = 2221;
    
    /// <summary>
    /// <code>fun line WrapCount=2235(line docLine, )</code>
    /// The number of display lines needed to wrap a document line
    /// </summary>
    public const int SCI_WRAPCOUNT = 2235;
    
    // FoldLevel
    // =========
    public const uint SC_FOLDLEVELNONE = 0x0;
    public const uint SC_FOLDLEVELBASE = 0x400;
    public const uint SC_FOLDLEVELWHITEFLAG = 0x1000;
    public const uint SC_FOLDLEVELHEADERFLAG = 0x2000;
    public const uint SC_FOLDLEVELNUMBERMASK = 0x0FFF;
    
    /// <summary>
    /// <code>set void SetFoldLevel=2222(line line, FoldLevel level)</code>
    /// Set the fold level of a line.
    /// This encodes an integer level along with flags indicating whether the
    /// line is a header and whether it is effectively white space.
    /// </summary>
    public const int SCI_SETFOLDLEVEL = 2222;
    
    /// <summary>
    /// <code>get FoldLevel GetFoldLevel=2223(line line, )</code>
    /// Retrieve the fold level of a line.
    /// </summary>
    public const int SCI_GETFOLDLEVEL = 2223;
    
    /// <summary>
    /// <code>get line GetLastChild=2224(line line, FoldLevel level)</code>
    /// Find the last child line of a header line.
    /// </summary>
    public const int SCI_GETLASTCHILD = 2224;
    
    /// <summary>
    /// <code>get line GetFoldParent=2225(line line, )</code>
    /// Find the parent line of a child line.
    /// </summary>
    public const int SCI_GETFOLDPARENT = 2225;
    
    /// <summary>
    /// <code>fun void ShowLines=2226(line lineStart, line lineEnd)</code>
    /// Make a range of lines visible.
    /// </summary>
    public const int SCI_SHOWLINES = 2226;
    
    /// <summary>
    /// <code>fun void HideLines=2227(line lineStart, line lineEnd)</code>
    /// Make a range of lines invisible.
    /// </summary>
    public const int SCI_HIDELINES = 2227;
    
    /// <summary>
    /// <code>get bool GetLineVisible=2228(line line, )</code>
    /// Is a line visible?
    /// </summary>
    public const int SCI_GETLINEVISIBLE = 2228;
    
    /// <summary>
    /// <code>get bool GetAllLinesVisible=2236(, )</code>
    /// Are all lines visible?
    /// </summary>
    public const int SCI_GETALLLINESVISIBLE = 2236;
    
    /// <summary>
    /// <code>set void SetFoldExpanded=2229(line line, bool expanded)</code>
    /// Show the children of a header line.
    /// </summary>
    public const int SCI_SETFOLDEXPANDED = 2229;
    
    /// <summary>
    /// <code>get bool GetFoldExpanded=2230(line line, )</code>
    /// Is a header line expanded?
    /// </summary>
    public const int SCI_GETFOLDEXPANDED = 2230;
    
    /// <summary>
    /// <code>fun void ToggleFold=2231(line line, )</code>
    /// Switch a header line between expanded and contracted.
    /// </summary>
    public const int SCI_TOGGLEFOLD = 2231;
    
    /// <summary>
    /// <code>fun void ToggleFoldShowText=2700(line line, string text)</code>
    /// Switch a header line between expanded and contracted and show some text after the line.
    /// </summary>
    public const int SCI_TOGGLEFOLDSHOWTEXT = 2700;
    
    // FoldDisplayTextStyle
    // ====================
    public const int SC_FOLDDISPLAYTEXT_HIDDEN = 0;
    public const int SC_FOLDDISPLAYTEXT_STANDARD = 1;
    public const int SC_FOLDDISPLAYTEXT_BOXED = 2;
    
    /// <summary>
    /// <code>set void FoldDisplayTextSetStyle=2701(FoldDisplayTextStyle style, )</code>
    /// Set the style of fold display text.
    /// </summary>
    public const int SCI_FOLDDISPLAYTEXTSETSTYLE = 2701;
    
    /// <summary>
    /// <code>get FoldDisplayTextStyle FoldDisplayTextGetStyle=2707(, )</code>
    /// Get the style of fold display text.
    /// </summary>
    public const int SCI_FOLDDISPLAYTEXTGETSTYLE = 2707;
    
    /// <summary>
    /// <code>fun void SetDefaultFoldDisplayText=2722(, string text)</code>
    /// Set the default fold display text.
    /// </summary>
    public const int SCI_SETDEFAULTFOLDDISPLAYTEXT = 2722;
    
    /// <summary>
    /// <code>fun int GetDefaultFoldDisplayText=2723(, stringresult text)</code>
    /// Get the default fold display text.
    /// </summary>
    public const int SCI_GETDEFAULTFOLDDISPLAYTEXT = 2723;
    
    // FoldAction
    // ==========
    public const int SC_FOLDACTION_CONTRACT = 0;
    public const int SC_FOLDACTION_EXPAND = 1;
    public const int SC_FOLDACTION_TOGGLE = 2;
    public const int SC_FOLDACTION_CONTRACT_EVERY_LEVEL = 4;
    
    /// <summary>
    /// <code>fun void FoldLine=2237(line line, FoldAction action)</code>
    /// Expand or contract a fold header.
    /// </summary>
    public const int SCI_FOLDLINE = 2237;
    
    /// <summary>
    /// <code>fun void FoldChildren=2238(line line, FoldAction action)</code>
    /// Expand or contract a fold header and its children.
    /// </summary>
    public const int SCI_FOLDCHILDREN = 2238;
    
    /// <summary>
    /// <code>fun void ExpandChildren=2239(line line, FoldLevel level)</code>
    /// Expand a fold header and all children. Use the level argument instead of the line's current level.
    /// </summary>
    public const int SCI_EXPANDCHILDREN = 2239;
    
    /// <summary>
    /// <code>fun void FoldAll=2662(FoldAction action, )</code>
    /// Expand or contract all fold headers.
    /// </summary>
    public const int SCI_FOLDALL = 2662;
    
    /// <summary>
    /// <code>fun void EnsureVisible=2232(line line, )</code>
    /// Ensure a particular line is visible by expanding any header line hiding it.
    /// </summary>
    public const int SCI_ENSUREVISIBLE = 2232;
    
    // AutomaticFold
    // =============
    public const uint SC_AUTOMATICFOLD_NONE = 0x0000;
    public const uint SC_AUTOMATICFOLD_SHOW = 0x0001;
    public const uint SC_AUTOMATICFOLD_CLICK = 0x0002;
    public const uint SC_AUTOMATICFOLD_CHANGE = 0x0004;
    
    /// <summary>
    /// <code>set void SetAutomaticFold=2663(AutomaticFold automaticFold, )</code>
    /// Set automatic folding behaviours.
    /// </summary>
    public const int SCI_SETAUTOMATICFOLD = 2663;
    
    /// <summary>
    /// <code>get AutomaticFold GetAutomaticFold=2664(, )</code>
    /// Get automatic folding behaviours.
    /// </summary>
    public const int SCI_GETAUTOMATICFOLD = 2664;
    
    // FoldFlag
    // ========
    public const uint SC_FOLDFLAG_NONE = 0x0000;
    public const uint SC_FOLDFLAG_LINEBEFORE_EXPANDED = 0x0002;
    public const uint SC_FOLDFLAG_LINEBEFORE_CONTRACTED = 0x0004;
    public const uint SC_FOLDFLAG_LINEAFTER_EXPANDED = 0x0008;
    public const uint SC_FOLDFLAG_LINEAFTER_CONTRACTED = 0x0010;
    public const uint SC_FOLDFLAG_LEVELNUMBERS = 0x0040;
    public const uint SC_FOLDFLAG_LINESTATE = 0x0080;
    
    /// <summary>
    /// <code>set void SetFoldFlags=2233(FoldFlag flags, )</code>
    /// Set some style options for folding.
    /// </summary>
    public const int SCI_SETFOLDFLAGS = 2233;
    
    /// <summary>
    /// <code>fun void EnsureVisibleEnforcePolicy=2234(line line, )</code>
    /// Ensure a particular line is visible by expanding any header line hiding it.
    /// Use the currently set visibility policy to determine which range to display.
    /// </summary>
    public const int SCI_ENSUREVISIBLEENFORCEPOLICY = 2234;
    
    /// <summary>
    /// <code>set void SetTabIndents=2260(bool tabIndents, )</code>
    /// Sets whether a tab pressed when caret is within indentation indents.
    /// </summary>
    public const int SCI_SETTABINDENTS = 2260;
    
    /// <summary>
    /// <code>get bool GetTabIndents=2261(, )</code>
    /// Does a tab pressed when caret is within indentation indent?
    /// </summary>
    public const int SCI_GETTABINDENTS = 2261;
    
    /// <summary>
    /// <code>set void SetBackSpaceUnIndents=2262(bool bsUnIndents, )</code>
    /// Sets whether a backspace pressed when caret is within indentation unindents.
    /// </summary>
    public const int SCI_SETBACKSPACEUNINDENTS = 2262;
    
    /// <summary>
    /// <code>get bool GetBackSpaceUnIndents=2263(, )</code>
    /// Does a backspace pressed when caret is within indentation unindent?
    /// </summary>
    public const int SCI_GETBACKSPACEUNINDENTS = 2263;
    
    public const int SC_TIME_FOREVER = 10000000;
    
    /// <summary>
    /// <code>set void SetMouseDwellTime=2264(int periodMilliseconds, )</code>
    /// Sets the time the mouse must sit still to generate a mouse dwell event.
    /// </summary>
    public const int SCI_SETMOUSEDWELLTIME = 2264;
    
    /// <summary>
    /// <code>get int GetMouseDwellTime=2265(, )</code>
    /// Retrieve the time the mouse must sit still to generate a mouse dwell event.
    /// </summary>
    public const int SCI_GETMOUSEDWELLTIME = 2265;
    
    /// <summary>
    /// <code>fun position WordStartPosition=2266(position pos, bool onlyWordCharacters)</code>
    /// Get position of start of word.
    /// </summary>
    public const int SCI_WORDSTARTPOSITION = 2266;
    
    /// <summary>
    /// <code>fun position WordEndPosition=2267(position pos, bool onlyWordCharacters)</code>
    /// Get position of end of word.
    /// </summary>
    public const int SCI_WORDENDPOSITION = 2267;
    
    /// <summary>
    /// <code>fun bool IsRangeWord=2691(position start, position end)</code>
    /// Is the range start..end considered a word?
    /// </summary>
    public const int SCI_ISRANGEWORD = 2691;
    
    // IdleStyling
    // ===========
    public const int SC_IDLESTYLING_NONE = 0;
    public const int SC_IDLESTYLING_TOVISIBLE = 1;
    public const int SC_IDLESTYLING_AFTERVISIBLE = 2;
    public const int SC_IDLESTYLING_ALL = 3;
    
    /// <summary>
    /// <code>set void SetIdleStyling=2692(IdleStyling idleStyling, )</code>
    /// Sets limits to idle styling.
    /// </summary>
    public const int SCI_SETIDLESTYLING = 2692;
    
    /// <summary>
    /// <code>get IdleStyling GetIdleStyling=2693(, )</code>
    /// Retrieve the limits to idle styling.
    /// </summary>
    public const int SCI_GETIDLESTYLING = 2693;
    
    // Wrap
    // ====
    public const int SC_WRAP_NONE = 0;
    public const int SC_WRAP_WORD = 1;
    public const int SC_WRAP_CHAR = 2;
    public const int SC_WRAP_WHITESPACE = 3;
    
    /// <summary>
    /// <code>set void SetWrapMode=2268(Wrap wrapMode, )</code>
    /// Sets whether text is word wrapped.
    /// </summary>
    public const int SCI_SETWRAPMODE = 2268;
    
    /// <summary>
    /// <code>get Wrap GetWrapMode=2269(, )</code>
    /// Retrieve whether text is word wrapped.
    /// </summary>
    public const int SCI_GETWRAPMODE = 2269;
    
    // WrapVisualFlag
    // ==============
    public const uint SC_WRAPVISUALFLAG_NONE = 0x0000;
    public const uint SC_WRAPVISUALFLAG_END = 0x0001;
    public const uint SC_WRAPVISUALFLAG_START = 0x0002;
    public const uint SC_WRAPVISUALFLAG_MARGIN = 0x0004;
    
    /// <summary>
    /// <code>set void SetWrapVisualFlags=2460(WrapVisualFlag wrapVisualFlags, )</code>
    /// Set the display mode of visual flags for wrapped lines.
    /// </summary>
    public const int SCI_SETWRAPVISUALFLAGS = 2460;
    
    /// <summary>
    /// <code>get WrapVisualFlag GetWrapVisualFlags=2461(, )</code>
    /// Retrive the display mode of visual flags for wrapped lines.
    /// </summary>
    public const int SCI_GETWRAPVISUALFLAGS = 2461;
    
    // WrapVisualLocation
    // ==================
    public const uint SC_WRAPVISUALFLAGLOC_DEFAULT = 0x0000;
    public const uint SC_WRAPVISUALFLAGLOC_END_BY_TEXT = 0x0001;
    public const uint SC_WRAPVISUALFLAGLOC_START_BY_TEXT = 0x0002;
    
    /// <summary>
    /// <code>set void SetWrapVisualFlagsLocation=2462(WrapVisualLocation wrapVisualFlagsLocation, )</code>
    /// Set the location of visual flags for wrapped lines.
    /// </summary>
    public const int SCI_SETWRAPVISUALFLAGSLOCATION = 2462;
    
    /// <summary>
    /// <code>get WrapVisualLocation GetWrapVisualFlagsLocation=2463(, )</code>
    /// Retrive the location of visual flags for wrapped lines.
    /// </summary>
    public const int SCI_GETWRAPVISUALFLAGSLOCATION = 2463;
    
    /// <summary>
    /// <code>set void SetWrapStartIndent=2464(int indent, )</code>
    /// Set the start indent for wrapped lines.
    /// </summary>
    public const int SCI_SETWRAPSTARTINDENT = 2464;
    
    /// <summary>
    /// <code>get int GetWrapStartIndent=2465(, )</code>
    /// Retrive the start indent for wrapped lines.
    /// </summary>
    public const int SCI_GETWRAPSTARTINDENT = 2465;
    
    // WrapIndentMode
    // ==============
    public const int SC_WRAPINDENT_FIXED = 0;
    public const int SC_WRAPINDENT_SAME = 1;
    public const int SC_WRAPINDENT_INDENT = 2;
    public const int SC_WRAPINDENT_DEEPINDENT = 3;
    
    /// <summary>
    /// <code>set void SetWrapIndentMode=2472(WrapIndentMode wrapIndentMode, )</code>
    /// Sets how wrapped sublines are placed. Default is fixed.
    /// </summary>
    public const int SCI_SETWRAPINDENTMODE = 2472;
    
    /// <summary>
    /// <code>get WrapIndentMode GetWrapIndentMode=2473(, )</code>
    /// Retrieve how wrapped sublines are placed. Default is fixed.
    /// </summary>
    public const int SCI_GETWRAPINDENTMODE = 2473;
    
    // LineCache
    // =========
    public const int SC_CACHE_NONE = 0;
    public const int SC_CACHE_CARET = 1;
    public const int SC_CACHE_PAGE = 2;
    public const int SC_CACHE_DOCUMENT = 3;
    
    /// <summary>
    /// <code>set void SetLayoutCache=2272(LineCache cacheMode, )</code>
    /// Sets the degree of caching of layout information.
    /// </summary>
    public const int SCI_SETLAYOUTCACHE = 2272;
    
    /// <summary>
    /// <code>get LineCache GetLayoutCache=2273(, )</code>
    /// Retrieve the degree of caching of layout information.
    /// </summary>
    public const int SCI_GETLAYOUTCACHE = 2273;
    
    /// <summary>
    /// <code>set void SetScrollWidth=2274(int pixelWidth, )</code>
    /// Sets the document width assumed for scrolling.
    /// </summary>
    public const int SCI_SETSCROLLWIDTH = 2274;
    
    /// <summary>
    /// <code>get int GetScrollWidth=2275(, )</code>
    /// Retrieve the document width assumed for scrolling.
    /// </summary>
    public const int SCI_GETSCROLLWIDTH = 2275;
    
    /// <summary>
    /// <code>set void SetScrollWidthTracking=2516(bool tracking, )</code>
    /// Sets whether the maximum width line displayed is used to set scroll width.
    /// </summary>
    public const int SCI_SETSCROLLWIDTHTRACKING = 2516;
    
    /// <summary>
    /// <code>get bool GetScrollWidthTracking=2517(, )</code>
    /// Retrieve whether the scroll width tracks wide lines.
    /// </summary>
    public const int SCI_GETSCROLLWIDTHTRACKING = 2517;
    
    /// <summary>
    /// <code>fun int TextWidth=2276(int style, string text)</code>
    /// Measure the pixel width of some text in a particular style.
    /// NUL terminated text argument.
    /// Does not handle tab or control characters.
    /// </summary>
    public const int SCI_TEXTWIDTH = 2276;
    
    /// <summary>
    /// <code>set void SetEndAtLastLine=2277(bool endAtLastLine, )</code>
    /// Sets the scroll range so that maximum scroll position has
    /// the last line at the bottom of the view (default).
    /// Setting this to false allows scrolling one page below the last line.
    /// </summary>
    public const int SCI_SETENDATLASTLINE = 2277;
    
    /// <summary>
    /// <code>get bool GetEndAtLastLine=2278(, )</code>
    /// Retrieve whether the maximum scroll position has the last
    /// line at the bottom of the view.
    /// </summary>
    public const int SCI_GETENDATLASTLINE = 2278;
    
    /// <summary>
    /// <code>fun int TextHeight=2279(line line, )</code>
    /// Retrieve the height of a particular line of text in pixels.
    /// </summary>
    public const int SCI_TEXTHEIGHT = 2279;
    
    /// <summary>
    /// <code>set void SetVScrollBar=2280(bool visible, )</code>
    /// Show or hide the vertical scroll bar.
    /// </summary>
    public const int SCI_SETVSCROLLBAR = 2280;
    
    /// <summary>
    /// <code>get bool GetVScrollBar=2281(, )</code>
    /// Is the vertical scroll bar visible?
    /// </summary>
    public const int SCI_GETVSCROLLBAR = 2281;
    
    /// <summary>
    /// <code>fun void AppendText=2282(position length, string text)</code>
    /// Append a string to the end of the document without changing the selection.
    /// </summary>
    public const int SCI_APPENDTEXT = 2282;
    
    // PhasesDraw
    // ==========
    public const int SC_PHASES_ONE = 0;
    public const int SC_PHASES_TWO = 1;
    public const int SC_PHASES_MULTIPLE = 2;
    
    /// <summary>
    /// <code>get PhasesDraw GetPhasesDraw=2673(, )</code>
    /// How many phases is drawing done in?
    /// </summary>
    public const int SCI_GETPHASESDRAW = 2673;
    
    /// <summary>
    /// <code>set void SetPhasesDraw=2674(PhasesDraw phases, )</code>
    /// In one phase draw, text is drawn in a series of rectangular blocks with no overlap.
    /// In two phase draw, text is drawn in a series of lines allowing runs to overlap horizontally.
    /// In multiple phase draw, each element is drawn over the whole drawing area, allowing text
    /// to overlap from one line to the next.
    /// </summary>
    public const int SCI_SETPHASESDRAW = 2674;
    
    // FontQuality
    // ===========
    public const uint SC_EFF_QUALITY_MASK = 0xF;
    public const int SC_EFF_QUALITY_DEFAULT = 0;
    public const int SC_EFF_QUALITY_NON_ANTIALIASED = 1;
    public const int SC_EFF_QUALITY_ANTIALIASED = 2;
    public const int SC_EFF_QUALITY_LCD_OPTIMIZED = 3;
    
    /// <summary>
    /// <code>set void SetFontQuality=2611(FontQuality fontQuality, )</code>
    /// Choose the quality level for text from the FontQuality enumeration.
    /// </summary>
    public const int SCI_SETFONTQUALITY = 2611;
    
    /// <summary>
    /// <code>get FontQuality GetFontQuality=2612(, )</code>
    /// Retrieve the quality level for text.
    /// </summary>
    public const int SCI_GETFONTQUALITY = 2612;
    
    /// <summary>
    /// <code>set void SetFirstVisibleLine=2613(line displayLine, )</code>
    /// Scroll so that a display line is at the top of the display.
    /// </summary>
    public const int SCI_SETFIRSTVISIBLELINE = 2613;
    
    // MultiPaste
    // ==========
    public const int SC_MULTIPASTE_ONCE = 0;
    public const int SC_MULTIPASTE_EACH = 1;
    
    /// <summary>
    /// <code>set void SetMultiPaste=2614(MultiPaste multiPaste, )</code>
    /// Change the effect of pasting when there are multiple selections.
    /// </summary>
    public const int SCI_SETMULTIPASTE = 2614;
    
    /// <summary>
    /// <code>get MultiPaste GetMultiPaste=2615(, )</code>
    /// Retrieve the effect of pasting when there are multiple selections.
    /// </summary>
    public const int SCI_GETMULTIPASTE = 2615;
    
    /// <summary>
    /// <code>get int GetTag=2616(int tagNumber, stringresult tagValue)</code>
    /// Retrieve the value of a tag from a regular expression search.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_GETTAG = 2616;
    
    /// <summary>
    /// <code>fun void LinesJoin=2288(, )</code>
    /// Join the lines in the target.
    /// </summary>
    public const int SCI_LINESJOIN = 2288;
    
    /// <summary>
    /// <code>fun void LinesSplit=2289(int pixelWidth, )</code>
    /// Split the lines in the target into lines that are less wide than pixelWidth
    /// where possible.
    /// </summary>
    public const int SCI_LINESSPLIT = 2289;
    
    /// <summary>
    /// <code>fun void SetFoldMarginColour=2290(bool useSetting, colour back)</code>
    /// Set one of the colours used as a chequerboard pattern in the fold margin
    /// </summary>
    public const int SCI_SETFOLDMARGINCOLOUR = 2290;
    
    /// <summary>
    /// <code>fun void SetFoldMarginHiColour=2291(bool useSetting, colour fore)</code>
    /// Set the other colour used as a chequerboard pattern in the fold margin
    /// </summary>
    public const int SCI_SETFOLDMARGINHICOLOUR = 2291;
    
    // Accessibility
    // =============
    public const int SC_ACCESSIBILITY_DISABLED = 0;
    public const int SC_ACCESSIBILITY_ENABLED = 1;
    
    /// <summary>
    /// <code>set void SetAccessibility=2702(Accessibility accessibility, )</code>
    /// Enable or disable accessibility.
    /// </summary>
    public const int SCI_SETACCESSIBILITY = 2702;
    
    /// <summary>
    /// <code>get Accessibility GetAccessibility=2703(, )</code>
    /// Report accessibility status.
    /// </summary>
    public const int SCI_GETACCESSIBILITY = 2703;
    
    /// <summary>
    /// <code>fun void LineDown=2300(, )</code>
    /// Move caret down one line.
    /// </summary>
    public const int SCI_LINEDOWN = 2300;
    
    /// <summary>
    /// <code>fun void LineDownExtend=2301(, )</code>
    /// Move caret down one line extending selection to new caret position.
    /// </summary>
    public const int SCI_LINEDOWNEXTEND = 2301;
    
    /// <summary>
    /// <code>fun void LineUp=2302(, )</code>
    /// Move caret up one line.
    /// </summary>
    public const int SCI_LINEUP = 2302;
    
    /// <summary>
    /// <code>fun void LineUpExtend=2303(, )</code>
    /// Move caret up one line extending selection to new caret position.
    /// </summary>
    public const int SCI_LINEUPEXTEND = 2303;
    
    /// <summary>
    /// <code>fun void CharLeft=2304(, )</code>
    /// Move caret left one character.
    /// </summary>
    public const int SCI_CHARLEFT = 2304;
    
    /// <summary>
    /// <code>fun void CharLeftExtend=2305(, )</code>
    /// Move caret left one character extending selection to new caret position.
    /// </summary>
    public const int SCI_CHARLEFTEXTEND = 2305;
    
    /// <summary>
    /// <code>fun void CharRight=2306(, )</code>
    /// Move caret right one character.
    /// </summary>
    public const int SCI_CHARRIGHT = 2306;
    
    /// <summary>
    /// <code>fun void CharRightExtend=2307(, )</code>
    /// Move caret right one character extending selection to new caret position.
    /// </summary>
    public const int SCI_CHARRIGHTEXTEND = 2307;
    
    /// <summary>
    /// <code>fun void WordLeft=2308(, )</code>
    /// Move caret left one word.
    /// </summary>
    public const int SCI_WORDLEFT = 2308;
    
    /// <summary>
    /// <code>fun void WordLeftExtend=2309(, )</code>
    /// Move caret left one word extending selection to new caret position.
    /// </summary>
    public const int SCI_WORDLEFTEXTEND = 2309;
    
    /// <summary>
    /// <code>fun void WordRight=2310(, )</code>
    /// Move caret right one word.
    /// </summary>
    public const int SCI_WORDRIGHT = 2310;
    
    /// <summary>
    /// <code>fun void WordRightExtend=2311(, )</code>
    /// Move caret right one word extending selection to new caret position.
    /// </summary>
    public const int SCI_WORDRIGHTEXTEND = 2311;
    
    /// <summary>
    /// <code>fun void Home=2312(, )</code>
    /// Move caret to first position on line.
    /// </summary>
    public const int SCI_HOME = 2312;
    
    /// <summary>
    /// <code>fun void HomeExtend=2313(, )</code>
    /// Move caret to first position on line extending selection to new caret position.
    /// </summary>
    public const int SCI_HOMEEXTEND = 2313;
    
    /// <summary>
    /// <code>fun void LineEnd=2314(, )</code>
    /// Move caret to last position on line.
    /// </summary>
    public const int SCI_LINEEND = 2314;
    
    /// <summary>
    /// <code>fun void LineEndExtend=2315(, )</code>
    /// Move caret to last position on line extending selection to new caret position.
    /// </summary>
    public const int SCI_LINEENDEXTEND = 2315;
    
    /// <summary>
    /// <code>fun void DocumentStart=2316(, )</code>
    /// Move caret to first position in document.
    /// </summary>
    public const int SCI_DOCUMENTSTART = 2316;
    
    /// <summary>
    /// <code>fun void DocumentStartExtend=2317(, )</code>
    /// Move caret to first position in document extending selection to new caret position.
    /// </summary>
    public const int SCI_DOCUMENTSTARTEXTEND = 2317;
    
    /// <summary>
    /// <code>fun void DocumentEnd=2318(, )</code>
    /// Move caret to last position in document.
    /// </summary>
    public const int SCI_DOCUMENTEND = 2318;
    
    /// <summary>
    /// <code>fun void DocumentEndExtend=2319(, )</code>
    /// Move caret to last position in document extending selection to new caret position.
    /// </summary>
    public const int SCI_DOCUMENTENDEXTEND = 2319;
    
    /// <summary>
    /// <code>fun void PageUp=2320(, )</code>
    /// Move caret one page up.
    /// </summary>
    public const int SCI_PAGEUP = 2320;
    
    /// <summary>
    /// <code>fun void PageUpExtend=2321(, )</code>
    /// Move caret one page up extending selection to new caret position.
    /// </summary>
    public const int SCI_PAGEUPEXTEND = 2321;
    
    /// <summary>
    /// <code>fun void PageDown=2322(, )</code>
    /// Move caret one page down.
    /// </summary>
    public const int SCI_PAGEDOWN = 2322;
    
    /// <summary>
    /// <code>fun void PageDownExtend=2323(, )</code>
    /// Move caret one page down extending selection to new caret position.
    /// </summary>
    public const int SCI_PAGEDOWNEXTEND = 2323;
    
    /// <summary>
    /// <code>fun void EditToggleOvertype=2324(, )</code>
    /// Switch from insert to overtype mode or the reverse.
    /// </summary>
    public const int SCI_EDITTOGGLEOVERTYPE = 2324;
    
    /// <summary>
    /// <code>fun void Cancel=2325(, )</code>
    /// Cancel any modes such as call tip or auto-completion list display.
    /// </summary>
    public const int SCI_CANCEL = 2325;
    
    /// <summary>
    /// <code>fun void DeleteBack=2326(, )</code>
    /// Delete the selection or if no selection, the character before the caret.
    /// </summary>
    public const int SCI_DELETEBACK = 2326;
    
    /// <summary>
    /// <code>fun void Tab=2327(, )</code>
    /// If selection is empty or all on one line replace the selection with a tab character.
    /// If more than one line selected, indent the lines.
    /// </summary>
    public const int SCI_TAB = 2327;
    
    /// <summary>
    /// <code>fun void LineIndent=2813(, )</code>
    /// Indent the current and selected lines.
    /// </summary>
    public const int SCI_LINEINDENT = 2813;
    
    /// <summary>
    /// <code>fun void BackTab=2328(, )</code>
    /// If selection is empty or all on one line dedent the line if caret is at start, else move caret.
    /// If more than one line selected, dedent the lines.
    /// </summary>
    public const int SCI_BACKTAB = 2328;
    
    /// <summary>
    /// <code>fun void LineDedent=2814(, )</code>
    /// Dedent the current and selected lines.
    /// </summary>
    public const int SCI_LINEDEDENT = 2814;
    
    /// <summary>
    /// <code>fun void NewLine=2329(, )</code>
    /// Insert a new line, may use a CRLF, CR or LF depending on EOL mode.
    /// </summary>
    public const int SCI_NEWLINE = 2329;
    
    /// <summary>
    /// <code>fun void FormFeed=2330(, )</code>
    /// Insert a Form Feed character.
    /// </summary>
    public const int SCI_FORMFEED = 2330;
    
    /// <summary>
    /// <code>fun void VCHome=2331(, )</code>
    /// Move caret to before first visible character on line.
    /// If already there move to first character on line.
    /// </summary>
    public const int SCI_VCHOME = 2331;
    
    /// <summary>
    /// <code>fun void VCHomeExtend=2332(, )</code>
    /// Like VCHome but extending selection to new caret position.
    /// </summary>
    public const int SCI_VCHOMEEXTEND = 2332;
    
    /// <summary>
    /// <code>fun void ZoomIn=2333(, )</code>
    /// Magnify the displayed text by increasing the sizes by 1 point.
    /// </summary>
    public const int SCI_ZOOMIN = 2333;
    
    /// <summary>
    /// <code>fun void ZoomOut=2334(, )</code>
    /// Make the displayed text smaller by decreasing the sizes by 1 point.
    /// </summary>
    public const int SCI_ZOOMOUT = 2334;
    
    /// <summary>
    /// <code>fun void DelWordLeft=2335(, )</code>
    /// Delete the word to the left of the caret.
    /// </summary>
    public const int SCI_DELWORDLEFT = 2335;
    
    /// <summary>
    /// <code>fun void DelWordRight=2336(, )</code>
    /// Delete the word to the right of the caret.
    /// </summary>
    public const int SCI_DELWORDRIGHT = 2336;
    
    /// <summary>
    /// <code>fun void DelWordRightEnd=2518(, )</code>
    /// Delete the word to the right of the caret, but not the trailing non-word characters.
    /// </summary>
    public const int SCI_DELWORDRIGHTEND = 2518;
    
    /// <summary>
    /// <code>fun void LineCut=2337(, )</code>
    /// Cut the line containing the caret.
    /// </summary>
    public const int SCI_LINECUT = 2337;
    
    /// <summary>
    /// <code>fun void LineDelete=2338(, )</code>
    /// Delete the line containing the caret.
    /// </summary>
    public const int SCI_LINEDELETE = 2338;
    
    /// <summary>
    /// <code>fun void LineTranspose=2339(, )</code>
    /// Switch the current line with the previous.
    /// </summary>
    public const int SCI_LINETRANSPOSE = 2339;
    
    /// <summary>
    /// <code>fun void LineReverse=2354(, )</code>
    /// Reverse order of selected lines.
    /// </summary>
    public const int SCI_LINEREVERSE = 2354;
    
    /// <summary>
    /// <code>fun void LineDuplicate=2404(, )</code>
    /// Duplicate the current line.
    /// </summary>
    public const int SCI_LINEDUPLICATE = 2404;
    
    /// <summary>
    /// <code>fun void LowerCase=2340(, )</code>
    /// Transform the selection to lower case.
    /// </summary>
    public const int SCI_LOWERCASE = 2340;
    
    /// <summary>
    /// <code>fun void UpperCase=2341(, )</code>
    /// Transform the selection to upper case.
    /// </summary>
    public const int SCI_UPPERCASE = 2341;
    
    /// <summary>
    /// <code>fun void LineScrollDown=2342(, )</code>
    /// Scroll the document down, keeping the caret visible.
    /// </summary>
    public const int SCI_LINESCROLLDOWN = 2342;
    
    /// <summary>
    /// <code>fun void LineScrollUp=2343(, )</code>
    /// Scroll the document up, keeping the caret visible.
    /// </summary>
    public const int SCI_LINESCROLLUP = 2343;
    
    /// <summary>
    /// <code>fun void DeleteBackNotLine=2344(, )</code>
    /// Delete the selection or if no selection, the character before the caret.
    /// Will not delete the character before at the start of a line.
    /// </summary>
    public const int SCI_DELETEBACKNOTLINE = 2344;
    
    /// <summary>
    /// <code>fun void HomeDisplay=2345(, )</code>
    /// Move caret to first position on display line.
    /// </summary>
    public const int SCI_HOMEDISPLAY = 2345;
    
    /// <summary>
    /// <code>fun void HomeDisplayExtend=2346(, )</code>
    /// Move caret to first position on display line extending selection to
    /// new caret position.
    /// </summary>
    public const int SCI_HOMEDISPLAYEXTEND = 2346;
    
    /// <summary>
    /// <code>fun void LineEndDisplay=2347(, )</code>
    /// Move caret to last position on display line.
    /// </summary>
    public const int SCI_LINEENDDISPLAY = 2347;
    
    /// <summary>
    /// <code>fun void LineEndDisplayExtend=2348(, )</code>
    /// Move caret to last position on display line extending selection to new
    /// caret position.
    /// </summary>
    public const int SCI_LINEENDDISPLAYEXTEND = 2348;
    
    /// <summary>
    /// <code>fun void HomeWrap=2349(, )</code>
    /// Like Home but when word-wrap is enabled goes first to start of display line
    /// HomeDisplay, then to start of document line Home.
    /// </summary>
    public const int SCI_HOMEWRAP = 2349;
    
    /// <summary>
    /// <code>fun void HomeWrapExtend=2450(, )</code>
    /// Like HomeExtend but when word-wrap is enabled extends first to start of display line
    /// HomeDisplayExtend, then to start of document line HomeExtend.
    /// </summary>
    public const int SCI_HOMEWRAPEXTEND = 2450;
    
    /// <summary>
    /// <code>fun void LineEndWrap=2451(, )</code>
    /// Like LineEnd but when word-wrap is enabled goes first to end of display line
    /// LineEndDisplay, then to start of document line LineEnd.
    /// </summary>
    public const int SCI_LINEENDWRAP = 2451;
    
    /// <summary>
    /// <code>fun void LineEndWrapExtend=2452(, )</code>
    /// Like LineEndExtend but when word-wrap is enabled extends first to end of display line
    /// LineEndDisplayExtend, then to start of document line LineEndExtend.
    /// </summary>
    public const int SCI_LINEENDWRAPEXTEND = 2452;
    
    /// <summary>
    /// <code>fun void VCHomeWrap=2453(, )</code>
    /// Like VCHome but when word-wrap is enabled goes first to start of display line
    /// VCHomeDisplay, then behaves like VCHome.
    /// </summary>
    public const int SCI_VCHOMEWRAP = 2453;
    
    /// <summary>
    /// <code>fun void VCHomeWrapExtend=2454(, )</code>
    /// Like VCHomeExtend but when word-wrap is enabled extends first to start of display line
    /// VCHomeDisplayExtend, then behaves like VCHomeExtend.
    /// </summary>
    public const int SCI_VCHOMEWRAPEXTEND = 2454;
    
    /// <summary>
    /// <code>fun void LineCopy=2455(, )</code>
    /// Copy the line containing the caret.
    /// </summary>
    public const int SCI_LINECOPY = 2455;
    
    /// <summary>
    /// <code>fun void MoveCaretInsideView=2401(, )</code>
    /// Move the caret inside current view if it's not there already.
    /// </summary>
    public const int SCI_MOVECARETINSIDEVIEW = 2401;
    
    /// <summary>
    /// <code>fun position LineLength=2350(line line, )</code>
    /// How many characters are on a line, including end of line characters?
    /// </summary>
    public const int SCI_LINELENGTH = 2350;
    
    /// <summary>
    /// <code>fun void BraceHighlight=2351(position posA, position posB)</code>
    /// Highlight the characters at two positions.
    /// </summary>
    public const int SCI_BRACEHIGHLIGHT = 2351;
    
    /// <summary>
    /// <code>fun void BraceHighlightIndicator=2498(bool useSetting, int indicator)</code>
    /// Use specified indicator to highlight matching braces instead of changing their style.
    /// </summary>
    public const int SCI_BRACEHIGHLIGHTINDICATOR = 2498;
    
    /// <summary>
    /// <code>fun void BraceBadLight=2352(position pos, )</code>
    /// Highlight the character at a position indicating there is no matching brace.
    /// </summary>
    public const int SCI_BRACEBADLIGHT = 2352;
    
    /// <summary>
    /// <code>fun void BraceBadLightIndicator=2499(bool useSetting, int indicator)</code>
    /// Use specified indicator to highlight non matching brace instead of changing its style.
    /// </summary>
    public const int SCI_BRACEBADLIGHTINDICATOR = 2499;
    
    /// <summary>
    /// <code>fun position BraceMatch=2353(position pos, int maxReStyle)</code>
    /// Find the position of a matching brace or INVALID_POSITION if no match.
    /// The maxReStyle must be 0 for now. It may be defined in a future release.
    /// </summary>
    public const int SCI_BRACEMATCH = 2353;
    
    /// <summary>
    /// <code>fun position BraceMatchNext=2369(position pos, position startPos)</code>
    /// Similar to BraceMatch, but matching starts at the explicit start position.
    /// </summary>
    public const int SCI_BRACEMATCHNEXT = 2369;
    
    /// <summary>
    /// <code>get bool GetViewEOL=2355(, )</code>
    /// Are the end of line characters visible?
    /// </summary>
    public const int SCI_GETVIEWEOL = 2355;
    
    /// <summary>
    /// <code>set void SetViewEOL=2356(bool visible, )</code>
    /// Make the end of line characters visible or invisible.
    /// </summary>
    public const int SCI_SETVIEWEOL = 2356;
    
    /// <summary>
    /// <code>get pointer GetDocPointer=2357(, )</code>
    /// Retrieve a pointer to the document object.
    /// </summary>
    public const int SCI_GETDOCPOINTER = 2357;
    
    /// <summary>
    /// <code>set void SetDocPointer=2358(, pointer doc)</code>
    /// Change the document object used.
    /// </summary>
    public const int SCI_SETDOCPOINTER = 2358;
    
    /// <summary>
    /// <code>set void SetModEventMask=2359(ModificationFlags eventMask, )</code>
    /// Set which document modification events are sent to the container.
    /// </summary>
    public const int SCI_SETMODEVENTMASK = 2359;
    
    // EdgeVisualStyle
    // ===============
    public const int EDGE_NONE = 0;
    public const int EDGE_LINE = 1;
    public const int EDGE_BACKGROUND = 2;
    public const int EDGE_MULTILINE = 3;
    
    /// <summary>
    /// <code>get position GetEdgeColumn=2360(, )</code>
    /// Retrieve the column number which text should be kept within.
    /// </summary>
    public const int SCI_GETEDGECOLUMN = 2360;
    
    /// <summary>
    /// <code>set void SetEdgeColumn=2361(position column, )</code>
    /// Set the column number of the edge.
    /// If text goes past the edge then it is highlighted.
    /// </summary>
    public const int SCI_SETEDGECOLUMN = 2361;
    
    /// <summary>
    /// <code>get EdgeVisualStyle GetEdgeMode=2362(, )</code>
    /// Retrieve the edge highlight mode.
    /// </summary>
    public const int SCI_GETEDGEMODE = 2362;
    
    /// <summary>
    /// <code>set void SetEdgeMode=2363(EdgeVisualStyle edgeMode, )</code>
    /// The edge may be displayed by a line (EDGE_LINE/EDGE_MULTILINE) or by highlighting text that
    /// goes beyond it (EDGE_BACKGROUND) or not displayed at all (EDGE_NONE).
    /// </summary>
    public const int SCI_SETEDGEMODE = 2363;
    
    /// <summary>
    /// <code>get colour GetEdgeColour=2364(, )</code>
    /// Retrieve the colour used in edge indication.
    /// </summary>
    public const int SCI_GETEDGECOLOUR = 2364;
    
    /// <summary>
    /// <code>set void SetEdgeColour=2365(colour edgeColour, )</code>
    /// Change the colour used in edge indication.
    /// </summary>
    public const int SCI_SETEDGECOLOUR = 2365;
    
    /// <summary>
    /// <code>fun void MultiEdgeAddLine=2694(position column, colour edgeColour)</code>
    /// Add a new vertical edge to the view.
    /// </summary>
    public const int SCI_MULTIEDGEADDLINE = 2694;
    
    /// <summary>
    /// <code>fun void MultiEdgeClearAll=2695(, )</code>
    /// Clear all vertical edges.
    /// </summary>
    public const int SCI_MULTIEDGECLEARALL = 2695;
    
    /// <summary>
    /// <code>get position GetMultiEdgeColumn=2749(int which, )</code>
    /// Get multi edge positions.
    /// </summary>
    public const int SCI_GETMULTIEDGECOLUMN = 2749;
    
    /// <summary>
    /// <code>fun void SearchAnchor=2366(, )</code>
    /// Sets the current caret position to be the search anchor.
    /// </summary>
    public const int SCI_SEARCHANCHOR = 2366;
    
    /// <summary>
    /// <code>fun position SearchNext=2367(FindOption searchFlags, string text)</code>
    /// Find some text starting at the search anchor.
    /// Does not ensure the selection is visible.
    /// </summary>
    public const int SCI_SEARCHNEXT = 2367;
    
    /// <summary>
    /// <code>fun position SearchPrev=2368(FindOption searchFlags, string text)</code>
    /// Find some text starting at the search anchor and moving backwards.
    /// Does not ensure the selection is visible.
    /// </summary>
    public const int SCI_SEARCHPREV = 2368;
    
    /// <summary>
    /// <code>get line LinesOnScreen=2370(, )</code>
    /// Retrieves the number of lines completely visible.
    /// </summary>
    public const int SCI_LINESONSCREEN = 2370;
    
    // PopUp
    // =====
    public const int SC_POPUP_NEVER = 0;
    public const int SC_POPUP_ALL = 1;
    public const int SC_POPUP_TEXT = 2;
    
    /// <summary>
    /// <code>fun void UsePopUp=2371(PopUp popUpMode, )</code>
    /// Set whether a pop up menu is displayed automatically when the user presses
    /// the wrong mouse button on certain areas.
    /// </summary>
    public const int SCI_USEPOPUP = 2371;
    
    /// <summary>
    /// <code>get bool SelectionIsRectangle=2372(, )</code>
    /// Is the selection rectangular? The alternative is the more common stream selection.
    /// </summary>
    public const int SCI_SELECTIONISRECTANGLE = 2372;
    
    /// <summary>
    /// <code>set void SetZoom=2373(int zoomInPoints, )</code>
    /// Set the zoom level. This number of points is added to the size of all fonts.
    /// It may be positive to magnify or negative to reduce.
    /// </summary>
    public const int SCI_SETZOOM = 2373;
    
    /// <summary>
    /// <code>get int GetZoom=2374(, )</code>
    /// Retrieve the zoom level.
    /// </summary>
    public const int SCI_GETZOOM = 2374;
    
    // DocumentOption
    // ==============
    public const int SC_DOCUMENTOPTION_DEFAULT = 0;
    public const uint SC_DOCUMENTOPTION_STYLES_NONE = 0x1;
    public const uint SC_DOCUMENTOPTION_TEXT_LARGE = 0x100;
    
    /// <summary>
    /// <code>fun pointer CreateDocument=2375(position bytes, DocumentOption documentOptions)</code>
    /// Create a new document object.
    /// Starts with reference count of 1 and not selected into editor.
    /// </summary>
    public const int SCI_CREATEDOCUMENT = 2375;
    
    /// <summary>
    /// <code>fun void AddRefDocument=2376(, pointer doc)</code>
    /// Extend life of document.
    /// </summary>
    public const int SCI_ADDREFDOCUMENT = 2376;
    
    /// <summary>
    /// <code>fun void ReleaseDocument=2377(, pointer doc)</code>
    /// Release a reference to the document, deleting document if it fades to black.
    /// </summary>
    public const int SCI_RELEASEDOCUMENT = 2377;
    
    /// <summary>
    /// <code>get DocumentOption GetDocumentOptions=2379(, )</code>
    /// Get which document options are set.
    /// </summary>
    public const int SCI_GETDOCUMENTOPTIONS = 2379;
    
    /// <summary>
    /// <code>get ModificationFlags GetModEventMask=2378(, )</code>
    /// Get which document modification events are sent to the container.
    /// </summary>
    public const int SCI_GETMODEVENTMASK = 2378;
    
    /// <summary>
    /// <code>set void SetCommandEvents=2717(bool commandEvents, )</code>
    /// Set whether command events are sent to the container.
    /// </summary>
    public const int SCI_SETCOMMANDEVENTS = 2717;
    
    /// <summary>
    /// <code>get bool GetCommandEvents=2718(, )</code>
    /// Get whether command events are sent to the container.
    /// </summary>
    public const int SCI_GETCOMMANDEVENTS = 2718;
    
    /// <summary>
    /// <code>set void SetFocus=2380(bool focus, )</code>
    /// Change internal focus flag.
    /// </summary>
    public const int SCI_SETFOCUS = 2380;
    
    /// <summary>
    /// <code>get bool GetFocus=2381(, )</code>
    /// Get internal focus flag.
    /// </summary>
    public const int SCI_GETFOCUS = 2381;
    
    // Status
    // ======
    public const int SC_STATUS_OK = 0;
    public const int SC_STATUS_FAILURE = 1;
    public const int SC_STATUS_BADALLOC = 2;
    public const int SC_STATUS_WARN_START = 1000;
    public const int SC_STATUS_WARN_REGEX = 1001;
    
    /// <summary>
    /// <code>set void SetStatus=2382(Status status, )</code>
    /// Change error status - 0 = OK.
    /// </summary>
    public const int SCI_SETSTATUS = 2382;
    
    /// <summary>
    /// <code>get Status GetStatus=2383(, )</code>
    /// Get error status.
    /// </summary>
    public const int SCI_GETSTATUS = 2383;
    
    /// <summary>
    /// <code>set void SetMouseDownCaptures=2384(bool captures, )</code>
    /// Set whether the mouse is captured when its button is pressed.
    /// </summary>
    public const int SCI_SETMOUSEDOWNCAPTURES = 2384;
    
    /// <summary>
    /// <code>get bool GetMouseDownCaptures=2385(, )</code>
    /// Get whether mouse gets captured.
    /// </summary>
    public const int SCI_GETMOUSEDOWNCAPTURES = 2385;
    
    /// <summary>
    /// <code>set void SetMouseWheelCaptures=2696(bool captures, )</code>
    /// Set whether the mouse wheel can be active outside the window.
    /// </summary>
    public const int SCI_SETMOUSEWHEELCAPTURES = 2696;
    
    /// <summary>
    /// <code>get bool GetMouseWheelCaptures=2697(, )</code>
    /// Get whether mouse wheel can be active outside the window.
    /// </summary>
    public const int SCI_GETMOUSEWHEELCAPTURES = 2697;
    
    /// <summary>
    /// <code>set void SetCursor=2386(CursorShape cursorType, )</code>
    /// Sets the cursor to one of the SC_CURSOR* values.
    /// </summary>
    public const int SCI_SETCURSOR = 2386;
    
    /// <summary>
    /// <code>get CursorShape GetCursor=2387(, )</code>
    /// Get cursor type.
    /// </summary>
    public const int SCI_GETCURSOR = 2387;
    
    /// <summary>
    /// <code>set void SetControlCharSymbol=2388(int symbol, )</code>
    /// Change the way control characters are displayed:
    /// If symbol is &lt; 32, keep the drawn way, else, use the given character.
    /// </summary>
    public const int SCI_SETCONTROLCHARSYMBOL = 2388;
    
    /// <summary>
    /// <code>get int GetControlCharSymbol=2389(, )</code>
    /// Get the way control characters are displayed.
    /// </summary>
    public const int SCI_GETCONTROLCHARSYMBOL = 2389;
    
    /// <summary>
    /// <code>fun void WordPartLeft=2390(, )</code>
    /// Move to the previous change in capitalisation.
    /// </summary>
    public const int SCI_WORDPARTLEFT = 2390;
    
    /// <summary>
    /// <code>fun void WordPartLeftExtend=2391(, )</code>
    /// Move to the previous change in capitalisation extending selection
    /// to new caret position.
    /// </summary>
    public const int SCI_WORDPARTLEFTEXTEND = 2391;
    
    /// <summary>
    /// <code>fun void WordPartRight=2392(, )</code>
    /// Move to the change next in capitalisation.
    /// </summary>
    public const int SCI_WORDPARTRIGHT = 2392;
    
    /// <summary>
    /// <code>fun void WordPartRightExtend=2393(, )</code>
    /// Move to the next change in capitalisation extending selection
    /// to new caret position.
    /// </summary>
    public const int SCI_WORDPARTRIGHTEXTEND = 2393;
    
    // VisiblePolicy
    // =============
    public const uint VISIBLE_SLOP = 0x01;
    public const uint VISIBLE_STRICT = 0x04;
    
    /// <summary>
    /// <code>fun void SetVisiblePolicy=2394(VisiblePolicy visiblePolicy, int visibleSlop)</code>
    /// Set the way the display area is determined when a particular line
    /// is to be moved to by Find, FindNext, GotoLine, etc.
    /// </summary>
    public const int SCI_SETVISIBLEPOLICY = 2394;
    
    /// <summary>
    /// <code>fun void DelLineLeft=2395(, )</code>
    /// Delete back from the current position to the start of the line.
    /// </summary>
    public const int SCI_DELLINELEFT = 2395;
    
    /// <summary>
    /// <code>fun void DelLineRight=2396(, )</code>
    /// Delete forwards from the current position to the end of the line.
    /// </summary>
    public const int SCI_DELLINERIGHT = 2396;
    
    /// <summary>
    /// <code>set void SetXOffset=2397(int xOffset, )</code>
    /// Set the xOffset (ie, horizontal scroll position).
    /// </summary>
    public const int SCI_SETXOFFSET = 2397;
    
    /// <summary>
    /// <code>get int GetXOffset=2398(, )</code>
    /// Get the xOffset (ie, horizontal scroll position).
    /// </summary>
    public const int SCI_GETXOFFSET = 2398;
    
    /// <summary>
    /// <code>fun void ChooseCaretX=2399(, )</code>
    /// Set the last x chosen value to be the caret x position.
    /// </summary>
    public const int SCI_CHOOSECARETX = 2399;
    
    /// <summary>
    /// <code>fun void GrabFocus=2400(, )</code>
    /// Set the focus to this Scintilla widget.
    /// </summary>
    public const int SCI_GRABFOCUS = 2400;
    
    // CaretPolicy
    // ===========
    /// <summary>
    /// Caret policy, used by SetXCaretPolicy and SetYCaretPolicy.
    /// If CARET_SLOP is set, we can define a slop value: caretSlop.
    /// This value defines an unwanted zone (UZ) where the caret is... unwanted.
    /// This zone is defined as a number of pixels near the vertical margins,
    /// and as a number of lines near the horizontal margins.
    /// By keeping the caret away from the edges, it is seen within its context,
    /// so it is likely that the identifier that the caret is on can be completely seen,
    /// and that the current line is seen with some of the lines following it which are
    /// often dependent on that line.
    /// </summary>
    public const uint CARET_SLOP = 0x01;
    /// <summary>
    /// If CARET_STRICT is set, the policy is enforced... strictly.
    /// The caret is centred on the display if slop is not set,
    /// and cannot go in the UZ if slop is set.
    /// </summary>
    public const uint CARET_STRICT = 0x04;
    /// <summary>
    /// If CARET_JUMPS is set, the display is moved more energetically
    /// so the caret can move in the same direction longer before the policy is applied again.
    /// </summary>
    public const uint CARET_JUMPS = 0x10;
    /// <summary>
    /// If CARET_EVEN is not set, instead of having symmetrical UZs,
    /// the left and bottom UZs are extended up to right and top UZs respectively.
    /// This way, we favour the displaying of useful information: the beginning of lines,
    /// where most code reside, and the lines after the caret, eg. the body of a function.
    /// </summary>
    public const uint CARET_EVEN = 0x08;
    
    /// <summary>
    /// <code>fun void SetXCaretPolicy=2402(CaretPolicy caretPolicy, int caretSlop)</code>
    /// Set the way the caret is kept visible when going sideways.
    /// The exclusion zone is given in pixels.
    /// </summary>
    public const int SCI_SETXCARETPOLICY = 2402;
    
    /// <summary>
    /// <code>fun void SetYCaretPolicy=2403(CaretPolicy caretPolicy, int caretSlop)</code>
    /// Set the way the line the caret is on is kept visible.
    /// The exclusion zone is given in lines.
    /// </summary>
    public const int SCI_SETYCARETPOLICY = 2403;
    
    /// <summary>
    /// <code>set void SetPrintWrapMode=2406(Wrap wrapMode, )</code>
    /// Set printing to line wrapped (SC_WRAP_WORD) or not line wrapped (SC_WRAP_NONE).
    /// </summary>
    public const int SCI_SETPRINTWRAPMODE = 2406;
    
    /// <summary>
    /// <code>get Wrap GetPrintWrapMode=2407(, )</code>
    /// Is printing line wrapped?
    /// </summary>
    public const int SCI_GETPRINTWRAPMODE = 2407;
    
    /// <summary>
    /// <code>set void SetHotspotActiveFore=2410(bool useSetting, colour fore)</code>
    /// Set a fore colour for active hotspots.
    /// </summary>
    public const int SCI_SETHOTSPOTACTIVEFORE = 2410;
    
    /// <summary>
    /// <code>get colour GetHotspotActiveFore=2494(, )</code>
    /// Get the fore colour for active hotspots.
    /// </summary>
    public const int SCI_GETHOTSPOTACTIVEFORE = 2494;
    
    /// <summary>
    /// <code>set void SetHotspotActiveBack=2411(bool useSetting, colour back)</code>
    /// Set a back colour for active hotspots.
    /// </summary>
    public const int SCI_SETHOTSPOTACTIVEBACK = 2411;
    
    /// <summary>
    /// <code>get colour GetHotspotActiveBack=2495(, )</code>
    /// Get the back colour for active hotspots.
    /// </summary>
    public const int SCI_GETHOTSPOTACTIVEBACK = 2495;
    
    /// <summary>
    /// <code>set void SetHotspotActiveUnderline=2412(bool underline, )</code>
    /// Enable / Disable underlining active hotspots.
    /// </summary>
    public const int SCI_SETHOTSPOTACTIVEUNDERLINE = 2412;
    
    /// <summary>
    /// <code>get bool GetHotspotActiveUnderline=2496(, )</code>
    /// Get whether underlining for active hotspots.
    /// </summary>
    public const int SCI_GETHOTSPOTACTIVEUNDERLINE = 2496;
    
    /// <summary>
    /// <code>set void SetHotspotSingleLine=2421(bool singleLine, )</code>
    /// Limit hotspots to single line so hotspots on two lines don't merge.
    /// </summary>
    public const int SCI_SETHOTSPOTSINGLELINE = 2421;
    
    /// <summary>
    /// <code>get bool GetHotspotSingleLine=2497(, )</code>
    /// Get the HotspotSingleLine property
    /// </summary>
    public const int SCI_GETHOTSPOTSINGLELINE = 2497;
    
    /// <summary>
    /// <code>fun void ParaDown=2413(, )</code>
    /// Move caret down one paragraph (delimited by empty lines).
    /// </summary>
    public const int SCI_PARADOWN = 2413;
    
    /// <summary>
    /// <code>fun void ParaDownExtend=2414(, )</code>
    /// Extend selection down one paragraph (delimited by empty lines).
    /// </summary>
    public const int SCI_PARADOWNEXTEND = 2414;
    
    /// <summary>
    /// <code>fun void ParaUp=2415(, )</code>
    /// Move caret up one paragraph (delimited by empty lines).
    /// </summary>
    public const int SCI_PARAUP = 2415;
    
    /// <summary>
    /// <code>fun void ParaUpExtend=2416(, )</code>
    /// Extend selection up one paragraph (delimited by empty lines).
    /// </summary>
    public const int SCI_PARAUPEXTEND = 2416;
    
    /// <summary>
    /// <code>fun position PositionBefore=2417(position pos, )</code>
    /// Given a valid document position, return the previous position taking code
    /// page into account. Returns 0 if passed 0.
    /// </summary>
    public const int SCI_POSITIONBEFORE = 2417;
    
    /// <summary>
    /// <code>fun position PositionAfter=2418(position pos, )</code>
    /// Given a valid document position, return the next position taking code
    /// page into account. Maximum value returned is the last position in the document.
    /// </summary>
    public const int SCI_POSITIONAFTER = 2418;
    
    /// <summary>
    /// <code>fun position PositionRelative=2670(position pos, position relative)</code>
    /// Given a valid document position, return a position that differs in a number
    /// of characters. Returned value is always between 0 and last position in document.
    /// </summary>
    public const int SCI_POSITIONRELATIVE = 2670;
    
    /// <summary>
    /// <code>fun position PositionRelativeCodeUnits=2716(position pos, position relative)</code>
    /// Given a valid document position, return a position that differs in a number
    /// of UTF-16 code units. Returned value is always between 0 and last position in document.
    /// The result may point half way (2 bytes) inside a non-BMP character.
    /// </summary>
    public const int SCI_POSITIONRELATIVECODEUNITS = 2716;
    
    /// <summary>
    /// <code>fun void CopyRange=2419(position start, position end)</code>
    /// Copy a range of text to the clipboard. Positions are clipped into the document.
    /// </summary>
    public const int SCI_COPYRANGE = 2419;
    
    /// <summary>
    /// <code>fun void CopyText=2420(position length, string text)</code>
    /// Copy argument text to the clipboard.
    /// </summary>
    public const int SCI_COPYTEXT = 2420;
    
    // SelectionMode
    // =============
    public const int SC_SEL_STREAM = 0;
    public const int SC_SEL_RECTANGLE = 1;
    public const int SC_SEL_LINES = 2;
    public const int SC_SEL_THIN = 3;
    
    /// <summary>
    /// <code>set void SetSelectionMode=2422(SelectionMode selectionMode, )</code>
    /// Set the selection mode to stream (SC_SEL_STREAM) or rectangular (SC_SEL_RECTANGLE/SC_SEL_THIN) or
    /// by lines (SC_SEL_LINES).
    /// </summary>
    public const int SCI_SETSELECTIONMODE = 2422;
    
    /// <summary>
    /// <code>fun void ChangeSelectionMode=2659(SelectionMode selectionMode, )</code>
    /// Set the selection mode to stream (SC_SEL_STREAM) or rectangular (SC_SEL_RECTANGLE/SC_SEL_THIN) or
    /// by lines (SC_SEL_LINES) without changing MoveExtendsSelection.
    /// </summary>
    public const int SCI_CHANGESELECTIONMODE = 2659;
    
    /// <summary>
    /// <code>get SelectionMode GetSelectionMode=2423(, )</code>
    /// Get the mode of the current selection.
    /// </summary>
    public const int SCI_GETSELECTIONMODE = 2423;
    
    /// <summary>
    /// <code>set void SetMoveExtendsSelection=2719(bool moveExtendsSelection, )</code>
    /// Set whether or not regular caret moves will extend or reduce the selection.
    /// </summary>
    public const int SCI_SETMOVEEXTENDSSELECTION = 2719;
    
    /// <summary>
    /// <code>get bool GetMoveExtendsSelection=2706(, )</code>
    /// Get whether or not regular caret moves will extend or reduce the selection.
    /// </summary>
    public const int SCI_GETMOVEEXTENDSSELECTION = 2706;
    
    /// <summary>
    /// <code>fun position GetLineSelStartPosition=2424(line line, )</code>
    /// Retrieve the position of the start of the selection at the given line (INVALID_POSITION if no selection on this line).
    /// </summary>
    public const int SCI_GETLINESELSTARTPOSITION = 2424;
    
    /// <summary>
    /// <code>fun position GetLineSelEndPosition=2425(line line, )</code>
    /// Retrieve the position of the end of the selection at the given line (INVALID_POSITION if no selection on this line).
    /// </summary>
    public const int SCI_GETLINESELENDPOSITION = 2425;
    
    /// <summary>
    /// <code>fun void LineDownRectExtend=2426(, )</code>
    /// Move caret down one line, extending rectangular selection to new caret position.
    /// </summary>
    public const int SCI_LINEDOWNRECTEXTEND = 2426;
    
    /// <summary>
    /// <code>fun void LineUpRectExtend=2427(, )</code>
    /// Move caret up one line, extending rectangular selection to new caret position.
    /// </summary>
    public const int SCI_LINEUPRECTEXTEND = 2427;
    
    /// <summary>
    /// <code>fun void CharLeftRectExtend=2428(, )</code>
    /// Move caret left one character, extending rectangular selection to new caret position.
    /// </summary>
    public const int SCI_CHARLEFTRECTEXTEND = 2428;
    
    /// <summary>
    /// <code>fun void CharRightRectExtend=2429(, )</code>
    /// Move caret right one character, extending rectangular selection to new caret position.
    /// </summary>
    public const int SCI_CHARRIGHTRECTEXTEND = 2429;
    
    /// <summary>
    /// <code>fun void HomeRectExtend=2430(, )</code>
    /// Move caret to first position on line, extending rectangular selection to new caret position.
    /// </summary>
    public const int SCI_HOMERECTEXTEND = 2430;
    
    /// <summary>
    /// <code>fun void VCHomeRectExtend=2431(, )</code>
    /// Move caret to before first visible character on line.
    /// If already there move to first character on line.
    /// In either case, extend rectangular selection to new caret position.
    /// </summary>
    public const int SCI_VCHOMERECTEXTEND = 2431;
    
    /// <summary>
    /// <code>fun void LineEndRectExtend=2432(, )</code>
    /// Move caret to last position on line, extending rectangular selection to new caret position.
    /// </summary>
    public const int SCI_LINEENDRECTEXTEND = 2432;
    
    /// <summary>
    /// <code>fun void PageUpRectExtend=2433(, )</code>
    /// Move caret one page up, extending rectangular selection to new caret position.
    /// </summary>
    public const int SCI_PAGEUPRECTEXTEND = 2433;
    
    /// <summary>
    /// <code>fun void PageDownRectExtend=2434(, )</code>
    /// Move caret one page down, extending rectangular selection to new caret position.
    /// </summary>
    public const int SCI_PAGEDOWNRECTEXTEND = 2434;
    
    /// <summary>
    /// <code>fun void StutteredPageUp=2435(, )</code>
    /// Move caret to top of page, or one page up if already at top of page.
    /// </summary>
    public const int SCI_STUTTEREDPAGEUP = 2435;
    
    /// <summary>
    /// <code>fun void StutteredPageUpExtend=2436(, )</code>
    /// Move caret to top of page, or one page up if already at top of page, extending selection to new caret position.
    /// </summary>
    public const int SCI_STUTTEREDPAGEUPEXTEND = 2436;
    
    /// <summary>
    /// <code>fun void StutteredPageDown=2437(, )</code>
    /// Move caret to bottom of page, or one page down if already at bottom of page.
    /// </summary>
    public const int SCI_STUTTEREDPAGEDOWN = 2437;
    
    /// <summary>
    /// <code>fun void StutteredPageDownExtend=2438(, )</code>
    /// Move caret to bottom of page, or one page down if already at bottom of page, extending selection to new caret position.
    /// </summary>
    public const int SCI_STUTTEREDPAGEDOWNEXTEND = 2438;
    
    /// <summary>
    /// <code>fun void WordLeftEnd=2439(, )</code>
    /// Move caret left one word, position cursor at end of word.
    /// </summary>
    public const int SCI_WORDLEFTEND = 2439;
    
    /// <summary>
    /// <code>fun void WordLeftEndExtend=2440(, )</code>
    /// Move caret left one word, position cursor at end of word, extending selection to new caret position.
    /// </summary>
    public const int SCI_WORDLEFTENDEXTEND = 2440;
    
    /// <summary>
    /// <code>fun void WordRightEnd=2441(, )</code>
    /// Move caret right one word, position cursor at end of word.
    /// </summary>
    public const int SCI_WORDRIGHTEND = 2441;
    
    /// <summary>
    /// <code>fun void WordRightEndExtend=2442(, )</code>
    /// Move caret right one word, position cursor at end of word, extending selection to new caret position.
    /// </summary>
    public const int SCI_WORDRIGHTENDEXTEND = 2442;
    
    /// <summary>
    /// <code>set void SetWhitespaceChars=2443(, string characters)</code>
    /// Set the set of characters making up whitespace for when moving or selecting by word.
    /// Should be called after SetWordChars.
    /// </summary>
    public const int SCI_SETWHITESPACECHARS = 2443;
    
    /// <summary>
    /// <code>get int GetWhitespaceChars=2647(, stringresult characters)</code>
    /// Get the set of characters making up whitespace for when moving or selecting by word.
    /// </summary>
    public const int SCI_GETWHITESPACECHARS = 2647;
    
    /// <summary>
    /// <code>set void SetPunctuationChars=2648(, string characters)</code>
    /// Set the set of characters making up punctuation characters
    /// Should be called after SetWordChars.
    /// </summary>
    public const int SCI_SETPUNCTUATIONCHARS = 2648;
    
    /// <summary>
    /// <code>get int GetPunctuationChars=2649(, stringresult characters)</code>
    /// Get the set of characters making up punctuation characters
    /// </summary>
    public const int SCI_GETPUNCTUATIONCHARS = 2649;
    
    /// <summary>
    /// <code>fun void SetCharsDefault=2444(, )</code>
    /// Reset the set of characters for whitespace and word characters to the defaults.
    /// </summary>
    public const int SCI_SETCHARSDEFAULT = 2444;
    
    /// <summary>
    /// <code>get int AutoCGetCurrent=2445(, )</code>
    /// Get currently selected item position in the auto-completion list
    /// </summary>
    public const int SCI_AUTOCGETCURRENT = 2445;
    
    /// <summary>
    /// <code>get int AutoCGetCurrentText=2610(, stringresult text)</code>
    /// Get currently selected item text in the auto-completion list
    /// Returns the length of the item text
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_AUTOCGETCURRENTTEXT = 2610;
    
    // CaseInsensitiveBehaviour
    // ========================
    public const int SC_CASEINSENSITIVEBEHAVIOUR_RESPECTCASE = 0;
    public const int SC_CASEINSENSITIVEBEHAVIOUR_IGNORECASE = 1;
    
    /// <summary>
    /// <code>set void AutoCSetCaseInsensitiveBehaviour=2634(CaseInsensitiveBehaviour behaviour, )</code>
    /// Set auto-completion case insensitive behaviour to either prefer case-sensitive matches or have no preference.
    /// </summary>
    public const int SCI_AUTOCSETCASEINSENSITIVEBEHAVIOUR = 2634;
    
    /// <summary>
    /// <code>get CaseInsensitiveBehaviour AutoCGetCaseInsensitiveBehaviour=2635(, )</code>
    /// Get auto-completion case insensitive behaviour.
    /// </summary>
    public const int SCI_AUTOCGETCASEINSENSITIVEBEHAVIOUR = 2635;
    
    // MultiAutoComplete
    // =================
    public const int SC_MULTIAUTOC_ONCE = 0;
    public const int SC_MULTIAUTOC_EACH = 1;
    
    /// <summary>
    /// <code>set void AutoCSetMulti=2636(MultiAutoComplete multi, )</code>
    /// Change the effect of autocompleting when there are multiple selections.
    /// </summary>
    public const int SCI_AUTOCSETMULTI = 2636;
    
    /// <summary>
    /// <code>get MultiAutoComplete AutoCGetMulti=2637(, )</code>
    /// Retrieve the effect of autocompleting when there are multiple selections.
    /// </summary>
    public const int SCI_AUTOCGETMULTI = 2637;
    
    // Ordering
    // ========
    public const int SC_ORDER_PRESORTED = 0;
    public const int SC_ORDER_PERFORMSORT = 1;
    public const int SC_ORDER_CUSTOM = 2;
    
    /// <summary>
    /// <code>set void AutoCSetOrder=2660(Ordering order, )</code>
    /// Set the way autocompletion lists are ordered.
    /// </summary>
    public const int SCI_AUTOCSETORDER = 2660;
    
    /// <summary>
    /// <code>get Ordering AutoCGetOrder=2661(, )</code>
    /// Get the way autocompletion lists are ordered.
    /// </summary>
    public const int SCI_AUTOCGETORDER = 2661;
    
    /// <summary>
    /// <code>fun void Allocate=2446(position bytes, )</code>
    /// Enlarge the document to a particular size of text bytes.
    /// </summary>
    public const int SCI_ALLOCATE = 2446;
    
    /// <summary>
    /// <code>fun position TargetAsUTF8=2447(, stringresult s)</code>
    /// Returns the target converted to UTF8.
    /// Return the length in bytes.
    /// </summary>
    public const int SCI_TARGETASUTF8 = 2447;
    
    /// <summary>
    /// <code>fun void SetLengthForEncode=2448(position bytes, )</code>
    /// Set the length of the utf8 argument for calling EncodedFromUTF8.
    /// Set to -1 and the string will be measured to the first nul.
    /// </summary>
    public const int SCI_SETLENGTHFORENCODE = 2448;
    
    /// <summary>
    /// <code>fun position EncodedFromUTF8=2449(string utf8, stringresult encoded)</code>
    /// Translates a UTF8 string into the document encoding.
    /// Return the length of the result in bytes.
    /// On error return 0.
    /// </summary>
    public const int SCI_ENCODEDFROMUTF8 = 2449;
    
    /// <summary>
    /// <code>fun position FindColumn=2456(line line, position column)</code>
    /// Find the position of a column on a line taking into account tabs and
    /// multi-byte characters. If beyond end of line, return line end position.
    /// </summary>
    public const int SCI_FINDCOLUMN = 2456;
    
    // CaretSticky
    // ===========
    public const int SC_CARETSTICKY_OFF = 0;
    public const int SC_CARETSTICKY_ON = 1;
    public const int SC_CARETSTICKY_WHITESPACE = 2;
    
    /// <summary>
    /// <code>get CaretSticky GetCaretSticky=2457(, )</code>
    /// Can the caret preferred x position only be changed by explicit movement commands?
    /// </summary>
    public const int SCI_GETCARETSTICKY = 2457;
    
    /// <summary>
    /// <code>set void SetCaretSticky=2458(CaretSticky useCaretStickyBehaviour, )</code>
    /// Stop the caret preferred x position changing when the user types.
    /// </summary>
    public const int SCI_SETCARETSTICKY = 2458;
    
    /// <summary>
    /// <code>fun void ToggleCaretSticky=2459(, )</code>
    /// Switch between sticky and non-sticky: meant to be bound to a key.
    /// </summary>
    public const int SCI_TOGGLECARETSTICKY = 2459;
    
    /// <summary>
    /// <code>set void SetPasteConvertEndings=2467(bool convert, )</code>
    /// Enable/Disable convert-on-paste for line endings
    /// </summary>
    public const int SCI_SETPASTECONVERTENDINGS = 2467;
    
    /// <summary>
    /// <code>get bool GetPasteConvertEndings=2468(, )</code>
    /// Get convert-on-paste setting
    /// </summary>
    public const int SCI_GETPASTECONVERTENDINGS = 2468;
    
    /// <summary>
    /// <code>fun void ReplaceRectangular=2771(position length, string text)</code>
    /// Replace the selection with text like a rectangular paste.
    /// </summary>
    public const int SCI_REPLACERECTANGULAR = 2771;
    
    /// <summary>
    /// <code>fun void SelectionDuplicate=2469(, )</code>
    /// Duplicate the selection. If selection empty duplicate the line containing the caret.
    /// </summary>
    public const int SCI_SELECTIONDUPLICATE = 2469;
    
    /// <summary>
    /// <code>set void SetCaretLineBackAlpha=2470(Alpha alpha, )</code>
    /// Set background alpha of the caret line.
    /// </summary>
    public const int SCI_SETCARETLINEBACKALPHA = 2470;
    
    /// <summary>
    /// <code>get Alpha GetCaretLineBackAlpha=2471(, )</code>
    /// Get the background alpha of the caret line.
    /// </summary>
    public const int SCI_GETCARETLINEBACKALPHA = 2471;
    
    // CaretStyle
    // ==========
    public const int CARETSTYLE_INVISIBLE = 0;
    public const int CARETSTYLE_LINE = 1;
    public const int CARETSTYLE_BLOCK = 2;
    public const int CARETSTYLE_OVERSTRIKE_BAR = 0;
    public const uint CARETSTYLE_OVERSTRIKE_BLOCK = 0x10;
    public const uint CARETSTYLE_CURSES = 0x20;
    public const uint CARETSTYLE_INS_MASK = 0xF;
    public const uint CARETSTYLE_BLOCK_AFTER = 0x100;
    
    /// <summary>
    /// <code>set void SetCaretStyle=2512(CaretStyle caretStyle, )</code>
    /// Set the style of the caret to be drawn.
    /// </summary>
    public const int SCI_SETCARETSTYLE = 2512;
    
    /// <summary>
    /// <code>get CaretStyle GetCaretStyle=2513(, )</code>
    /// Returns the current style of the caret.
    /// </summary>
    public const int SCI_GETCARETSTYLE = 2513;
    
    /// <summary>
    /// <code>set void SetIndicatorCurrent=2500(int indicator, )</code>
    /// Set the indicator used for IndicatorFillRange and IndicatorClearRange
    /// </summary>
    public const int SCI_SETINDICATORCURRENT = 2500;
    
    /// <summary>
    /// <code>get int GetIndicatorCurrent=2501(, )</code>
    /// Get the current indicator
    /// </summary>
    public const int SCI_GETINDICATORCURRENT = 2501;
    
    /// <summary>
    /// <code>set void SetIndicatorValue=2502(int value, )</code>
    /// Set the value used for IndicatorFillRange
    /// </summary>
    public const int SCI_SETINDICATORVALUE = 2502;
    
    /// <summary>
    /// <code>get int GetIndicatorValue=2503(, )</code>
    /// Get the current indicator value
    /// </summary>
    public const int SCI_GETINDICATORVALUE = 2503;
    
    /// <summary>
    /// <code>fun void IndicatorFillRange=2504(position start, position lengthFill)</code>
    /// Turn a indicator on over a range.
    /// </summary>
    public const int SCI_INDICATORFILLRANGE = 2504;
    
    /// <summary>
    /// <code>fun void IndicatorClearRange=2505(position start, position lengthClear)</code>
    /// Turn a indicator off over a range.
    /// </summary>
    public const int SCI_INDICATORCLEARRANGE = 2505;
    
    /// <summary>
    /// <code>fun int IndicatorAllOnFor=2506(position pos, )</code>
    /// Are any indicators present at pos?
    /// </summary>
    public const int SCI_INDICATORALLONFOR = 2506;
    
    /// <summary>
    /// <code>fun int IndicatorValueAt=2507(int indicator, position pos)</code>
    /// What value does a particular indicator have at a position?
    /// </summary>
    public const int SCI_INDICATORVALUEAT = 2507;
    
    /// <summary>
    /// <code>fun position IndicatorStart=2508(int indicator, position pos)</code>
    /// Where does a particular indicator start?
    /// </summary>
    public const int SCI_INDICATORSTART = 2508;
    
    /// <summary>
    /// <code>fun position IndicatorEnd=2509(int indicator, position pos)</code>
    /// Where does a particular indicator end?
    /// </summary>
    public const int SCI_INDICATOREND = 2509;
    
    /// <summary>
    /// <code>set void SetPositionCache=2514(int size, )</code>
    /// Set number of entries in position cache
    /// </summary>
    public const int SCI_SETPOSITIONCACHE = 2514;
    
    /// <summary>
    /// <code>get int GetPositionCache=2515(, )</code>
    /// How many entries are allocated to the position cache?
    /// </summary>
    public const int SCI_GETPOSITIONCACHE = 2515;
    
    /// <summary>
    /// <code>set void SetLayoutThreads=2775(int threads, )</code>
    /// Set maximum number of threads used for layout
    /// </summary>
    public const int SCI_SETLAYOUTTHREADS = 2775;
    
    /// <summary>
    /// <code>get int GetLayoutThreads=2776(, )</code>
    /// Get maximum number of threads used for layout
    /// </summary>
    public const int SCI_GETLAYOUTTHREADS = 2776;
    
    /// <summary>
    /// <code>fun void CopyAllowLine=2519(, )</code>
    /// Copy the selection, if selection empty copy the line with the caret
    /// </summary>
    public const int SCI_COPYALLOWLINE = 2519;
    
    /// <summary>
    /// <code>fun void CutAllowLine=2810(, )</code>
    /// Cut the selection, if selection empty cut the line with the caret
    /// </summary>
    public const int SCI_CUTALLOWLINE = 2810;
    
    /// <summary>
    /// <code>set void SetCopySeparator=2811(, string separator)</code>
    /// Set the string to separate parts when copying a multiple selection.
    /// </summary>
    public const int SCI_SETCOPYSEPARATOR = 2811;
    
    /// <summary>
    /// <code>get int GetCopySeparator=2812(, stringresult separator)</code>
    /// Get the string to separate parts when copying a multiple selection.
    /// </summary>
    public const int SCI_GETCOPYSEPARATOR = 2812;
    
    /// <summary>
    /// <code>get pointer GetCharacterPointer=2520(, )</code>
    /// Compact the document buffer and return a read-only pointer to the
    /// characters in the document.
    /// </summary>
    public const int SCI_GETCHARACTERPOINTER = 2520;
    
    /// <summary>
    /// <code>get pointer GetRangePointer=2643(position start, position lengthRange)</code>
    /// Return a read-only pointer to a range of characters in the document.
    /// May move the gap so that the range is contiguous, but will only move up
    /// to lengthRange bytes.
    /// </summary>
    public const int SCI_GETRANGEPOINTER = 2643;
    
    /// <summary>
    /// <code>get position GetGapPosition=2644(, )</code>
    /// Return a position which, to avoid performance costs, should not be within
    /// the range of a call to GetRangePointer.
    /// </summary>
    public const int SCI_GETGAPPOSITION = 2644;
    
    /// <summary>
    /// <code>set void IndicSetAlpha=2523(int indicator, Alpha alpha)</code>
    /// Set the alpha fill colour of the given indicator.
    /// </summary>
    public const int SCI_INDICSETALPHA = 2523;
    
    /// <summary>
    /// <code>get Alpha IndicGetAlpha=2524(int indicator, )</code>
    /// Get the alpha fill colour of the given indicator.
    /// </summary>
    public const int SCI_INDICGETALPHA = 2524;
    
    /// <summary>
    /// <code>set void IndicSetOutlineAlpha=2558(int indicator, Alpha alpha)</code>
    /// Set the alpha outline colour of the given indicator.
    /// </summary>
    public const int SCI_INDICSETOUTLINEALPHA = 2558;
    
    /// <summary>
    /// <code>get Alpha IndicGetOutlineAlpha=2559(int indicator, )</code>
    /// Get the alpha outline colour of the given indicator.
    /// </summary>
    public const int SCI_INDICGETOUTLINEALPHA = 2559;
    
    /// <summary>
    /// <code>set void SetExtraAscent=2525(int extraAscent, )</code>
    /// Set extra ascent for each line
    /// </summary>
    public const int SCI_SETEXTRAASCENT = 2525;
    
    /// <summary>
    /// <code>get int GetExtraAscent=2526(, )</code>
    /// Get extra ascent for each line
    /// </summary>
    public const int SCI_GETEXTRAASCENT = 2526;
    
    /// <summary>
    /// <code>set void SetExtraDescent=2527(int extraDescent, )</code>
    /// Set extra descent for each line
    /// </summary>
    public const int SCI_SETEXTRADESCENT = 2527;
    
    /// <summary>
    /// <code>get int GetExtraDescent=2528(, )</code>
    /// Get extra descent for each line
    /// </summary>
    public const int SCI_GETEXTRADESCENT = 2528;
    
    /// <summary>
    /// <code>fun MarkerSymbol MarkerSymbolDefined=2529(int markerNumber, )</code>
    /// Which symbol was defined for markerNumber with MarkerDefine
    /// </summary>
    public const int SCI_MARKERSYMBOLDEFINED = 2529;
    
    /// <summary>
    /// <code>set void MarginSetText=2530(line line, string text)</code>
    /// Set the text in the text margin for a line
    /// </summary>
    public const int SCI_MARGINSETTEXT = 2530;
    
    /// <summary>
    /// <code>get int MarginGetText=2531(line line, stringresult text)</code>
    /// Get the text in the text margin for a line
    /// </summary>
    public const int SCI_MARGINGETTEXT = 2531;
    
    /// <summary>
    /// <code>set void MarginSetStyle=2532(line line, int style)</code>
    /// Set the style number for the text margin for a line
    /// </summary>
    public const int SCI_MARGINSETSTYLE = 2532;
    
    /// <summary>
    /// <code>get int MarginGetStyle=2533(line line, )</code>
    /// Get the style number for the text margin for a line
    /// </summary>
    public const int SCI_MARGINGETSTYLE = 2533;
    
    /// <summary>
    /// <code>set void MarginSetStyles=2534(line line, string styles)</code>
    /// Set the style in the text margin for a line
    /// </summary>
    public const int SCI_MARGINSETSTYLES = 2534;
    
    /// <summary>
    /// <code>get int MarginGetStyles=2535(line line, stringresult styles)</code>
    /// Get the styles in the text margin for a line
    /// </summary>
    public const int SCI_MARGINGETSTYLES = 2535;
    
    /// <summary>
    /// <code>fun void MarginTextClearAll=2536(, )</code>
    /// Clear the margin text on all lines
    /// </summary>
    public const int SCI_MARGINTEXTCLEARALL = 2536;
    
    /// <summary>
    /// <code>set void MarginSetStyleOffset=2537(int style, )</code>
    /// Get the start of the range of style numbers used for margin text
    /// </summary>
    public const int SCI_MARGINSETSTYLEOFFSET = 2537;
    
    /// <summary>
    /// <code>get int MarginGetStyleOffset=2538(, )</code>
    /// Get the start of the range of style numbers used for margin text
    /// </summary>
    public const int SCI_MARGINGETSTYLEOFFSET = 2538;
    
    // MarginOption
    // ============
    public const int SC_MARGINOPTION_NONE = 0;
    public const int SC_MARGINOPTION_SUBLINESELECT = 1;
    
    /// <summary>
    /// <code>set void SetMarginOptions=2539(MarginOption marginOptions, )</code>
    /// Set the margin options.
    /// </summary>
    public const int SCI_SETMARGINOPTIONS = 2539;
    
    /// <summary>
    /// <code>get MarginOption GetMarginOptions=2557(, )</code>
    /// Get the margin options.
    /// </summary>
    public const int SCI_GETMARGINOPTIONS = 2557;
    
    /// <summary>
    /// <code>set void AnnotationSetText=2540(line line, string text)</code>
    /// Set the annotation text for a line
    /// </summary>
    public const int SCI_ANNOTATIONSETTEXT = 2540;
    
    /// <summary>
    /// <code>get int AnnotationGetText=2541(line line, stringresult text)</code>
    /// Get the annotation text for a line
    /// </summary>
    public const int SCI_ANNOTATIONGETTEXT = 2541;
    
    /// <summary>
    /// <code>set void AnnotationSetStyle=2542(line line, int style)</code>
    /// Set the style number for the annotations for a line
    /// </summary>
    public const int SCI_ANNOTATIONSETSTYLE = 2542;
    
    /// <summary>
    /// <code>get int AnnotationGetStyle=2543(line line, )</code>
    /// Get the style number for the annotations for a line
    /// </summary>
    public const int SCI_ANNOTATIONGETSTYLE = 2543;
    
    /// <summary>
    /// <code>set void AnnotationSetStyles=2544(line line, string styles)</code>
    /// Set the annotation styles for a line
    /// </summary>
    public const int SCI_ANNOTATIONSETSTYLES = 2544;
    
    /// <summary>
    /// <code>get int AnnotationGetStyles=2545(line line, stringresult styles)</code>
    /// Get the annotation styles for a line
    /// </summary>
    public const int SCI_ANNOTATIONGETSTYLES = 2545;
    
    /// <summary>
    /// <code>get int AnnotationGetLines=2546(line line, )</code>
    /// Get the number of annotation lines for a line
    /// </summary>
    public const int SCI_ANNOTATIONGETLINES = 2546;
    
    /// <summary>
    /// <code>fun void AnnotationClearAll=2547(, )</code>
    /// Clear the annotations from all lines
    /// </summary>
    public const int SCI_ANNOTATIONCLEARALL = 2547;
    
    // AnnotationVisible
    // =================
    public const int ANNOTATION_HIDDEN = 0;
    public const int ANNOTATION_STANDARD = 1;
    public const int ANNOTATION_BOXED = 2;
    public const int ANNOTATION_INDENTED = 3;
    
    /// <summary>
    /// <code>set void AnnotationSetVisible=2548(AnnotationVisible visible, )</code>
    /// Set the visibility for the annotations for a view
    /// </summary>
    public const int SCI_ANNOTATIONSETVISIBLE = 2548;
    
    /// <summary>
    /// <code>get AnnotationVisible AnnotationGetVisible=2549(, )</code>
    /// Get the visibility for the annotations for a view
    /// </summary>
    public const int SCI_ANNOTATIONGETVISIBLE = 2549;
    
    /// <summary>
    /// <code>set void AnnotationSetStyleOffset=2550(int style, )</code>
    /// Get the start of the range of style numbers used for annotations
    /// </summary>
    public const int SCI_ANNOTATIONSETSTYLEOFFSET = 2550;
    
    /// <summary>
    /// <code>get int AnnotationGetStyleOffset=2551(, )</code>
    /// Get the start of the range of style numbers used for annotations
    /// </summary>
    public const int SCI_ANNOTATIONGETSTYLEOFFSET = 2551;
    
    /// <summary>
    /// <code>fun void ReleaseAllExtendedStyles=2552(, )</code>
    /// Release all extended (&gt;255) style numbers
    /// </summary>
    public const int SCI_RELEASEALLEXTENDEDSTYLES = 2552;
    
    /// <summary>
    /// <code>fun int AllocateExtendedStyles=2553(int numberStyles, )</code>
    /// Allocate some extended (&gt;255) style numbers and return the start of the range
    /// </summary>
    public const int SCI_ALLOCATEEXTENDEDSTYLES = 2553;
    
    // UndoFlags
    // =========
    public const int UNDO_NONE = 0;
    public const int UNDO_MAY_COALESCE = 1;
    
    /// <summary>
    /// <code>fun void AddUndoAction=2560(int token, UndoFlags flags)</code>
    /// Add a container action to the undo stack
    /// </summary>
    public const int SCI_ADDUNDOACTION = 2560;
    
    /// <summary>
    /// <code>fun position CharPositionFromPoint=2561(int x, int y)</code>
    /// Find the position of a character from a point within the window.
    /// </summary>
    public const int SCI_CHARPOSITIONFROMPOINT = 2561;
    
    /// <summary>
    /// <code>fun position CharPositionFromPointClose=2562(int x, int y)</code>
    /// Find the position of a character from a point within the window.
    /// Return INVALID_POSITION if not close to text.
    /// </summary>
    public const int SCI_CHARPOSITIONFROMPOINTCLOSE = 2562;
    
    /// <summary>
    /// <code>set void SetMouseSelectionRectangularSwitch=2668(bool mouseSelectionRectangularSwitch, )</code>
    /// Set whether switching to rectangular mode while selecting with the mouse is allowed.
    /// </summary>
    public const int SCI_SETMOUSESELECTIONRECTANGULARSWITCH = 2668;
    
    /// <summary>
    /// <code>get bool GetMouseSelectionRectangularSwitch=2669(, )</code>
    /// Whether switching to rectangular mode while selecting with the mouse is allowed.
    /// </summary>
    public const int SCI_GETMOUSESELECTIONRECTANGULARSWITCH = 2669;
    
    /// <summary>
    /// <code>set void SetMultipleSelection=2563(bool multipleSelection, )</code>
    /// Set whether multiple selections can be made
    /// </summary>
    public const int SCI_SETMULTIPLESELECTION = 2563;
    
    /// <summary>
    /// <code>get bool GetMultipleSelection=2564(, )</code>
    /// Whether multiple selections can be made
    /// </summary>
    public const int SCI_GETMULTIPLESELECTION = 2564;
    
    /// <summary>
    /// <code>set void SetAdditionalSelectionTyping=2565(bool additionalSelectionTyping, )</code>
    /// Set whether typing can be performed into multiple selections
    /// </summary>
    public const int SCI_SETADDITIONALSELECTIONTYPING = 2565;
    
    /// <summary>
    /// <code>get bool GetAdditionalSelectionTyping=2566(, )</code>
    /// Whether typing can be performed into multiple selections
    /// </summary>
    public const int SCI_GETADDITIONALSELECTIONTYPING = 2566;
    
    /// <summary>
    /// <code>set void SetAdditionalCaretsBlink=2567(bool additionalCaretsBlink, )</code>
    /// Set whether additional carets will blink
    /// </summary>
    public const int SCI_SETADDITIONALCARETSBLINK = 2567;
    
    /// <summary>
    /// <code>get bool GetAdditionalCaretsBlink=2568(, )</code>
    /// Whether additional carets will blink
    /// </summary>
    public const int SCI_GETADDITIONALCARETSBLINK = 2568;
    
    /// <summary>
    /// <code>set void SetAdditionalCaretsVisible=2608(bool additionalCaretsVisible, )</code>
    /// Set whether additional carets are visible
    /// </summary>
    public const int SCI_SETADDITIONALCARETSVISIBLE = 2608;
    
    /// <summary>
    /// <code>get bool GetAdditionalCaretsVisible=2609(, )</code>
    /// Whether additional carets are visible
    /// </summary>
    public const int SCI_GETADDITIONALCARETSVISIBLE = 2609;
    
    /// <summary>
    /// <code>get int GetSelections=2570(, )</code>
    /// How many selections are there?
    /// </summary>
    public const int SCI_GETSELECTIONS = 2570;
    
    /// <summary>
    /// <code>get bool GetSelectionEmpty=2650(, )</code>
    /// Is every selected range empty?
    /// </summary>
    public const int SCI_GETSELECTIONEMPTY = 2650;
    
    /// <summary>
    /// <code>fun void ClearSelections=2571(, )</code>
    /// Clear selections to a single empty stream selection
    /// </summary>
    public const int SCI_CLEARSELECTIONS = 2571;
    
    /// <summary>
    /// <code>fun void SetSelection=2572(position caret, position anchor)</code>
    /// Set a simple selection
    /// </summary>
    public const int SCI_SETSELECTION = 2572;
    
    /// <summary>
    /// <code>fun void AddSelection=2573(position caret, position anchor)</code>
    /// Add a selection
    /// </summary>
    public const int SCI_ADDSELECTION = 2573;
    
    /// <summary>
    /// <code>fun int SelectionFromPoint=2474(int x, int y)</code>
    /// Find the selection index for a point. -1 when not at a selection.
    /// </summary>
    public const int SCI_SELECTIONFROMPOINT = 2474;
    
    /// <summary>
    /// <code>fun void DropSelectionN=2671(int selection, )</code>
    /// Drop one selection
    /// </summary>
    public const int SCI_DROPSELECTIONN = 2671;
    
    /// <summary>
    /// <code>set void SetMainSelection=2574(int selection, )</code>
    /// Set the main selection
    /// </summary>
    public const int SCI_SETMAINSELECTION = 2574;
    
    /// <summary>
    /// <code>get int GetMainSelection=2575(, )</code>
    /// Which selection is the main selection
    /// </summary>
    public const int SCI_GETMAINSELECTION = 2575;
    
    /// <summary>
    /// <code>set void SetSelectionNCaret=2576(int selection, position caret)</code>
    /// Set the caret position of the nth selection.
    /// </summary>
    public const int SCI_SETSELECTIONNCARET = 2576;
    
    /// <summary>
    /// <code>get position GetSelectionNCaret=2577(int selection, )</code>
    /// Return the caret position of the nth selection.
    /// </summary>
    public const int SCI_GETSELECTIONNCARET = 2577;
    
    /// <summary>
    /// <code>set void SetSelectionNAnchor=2578(int selection, position anchor)</code>
    /// Set the anchor position of the nth selection.
    /// </summary>
    public const int SCI_SETSELECTIONNANCHOR = 2578;
    
    /// <summary>
    /// <code>get position GetSelectionNAnchor=2579(int selection, )</code>
    /// Return the anchor position of the nth selection.
    /// </summary>
    public const int SCI_GETSELECTIONNANCHOR = 2579;
    
    /// <summary>
    /// <code>set void SetSelectionNCaretVirtualSpace=2580(int selection, position space)</code>
    /// Set the virtual space of the caret of the nth selection.
    /// </summary>
    public const int SCI_SETSELECTIONNCARETVIRTUALSPACE = 2580;
    
    /// <summary>
    /// <code>get position GetSelectionNCaretVirtualSpace=2581(int selection, )</code>
    /// Return the virtual space of the caret of the nth selection.
    /// </summary>
    public const int SCI_GETSELECTIONNCARETVIRTUALSPACE = 2581;
    
    /// <summary>
    /// <code>set void SetSelectionNAnchorVirtualSpace=2582(int selection, position space)</code>
    /// Set the virtual space of the anchor of the nth selection.
    /// </summary>
    public const int SCI_SETSELECTIONNANCHORVIRTUALSPACE = 2582;
    
    /// <summary>
    /// <code>get position GetSelectionNAnchorVirtualSpace=2583(int selection, )</code>
    /// Return the virtual space of the anchor of the nth selection.
    /// </summary>
    public const int SCI_GETSELECTIONNANCHORVIRTUALSPACE = 2583;
    
    /// <summary>
    /// <code>set void SetSelectionNStart=2584(int selection, position anchor)</code>
    /// Sets the position that starts the selection - this becomes the anchor.
    /// </summary>
    public const int SCI_SETSELECTIONNSTART = 2584;
    
    /// <summary>
    /// <code>get position GetSelectionNStart=2585(int selection, )</code>
    /// Returns the position at the start of the selection.
    /// </summary>
    public const int SCI_GETSELECTIONNSTART = 2585;
    
    /// <summary>
    /// <code>get position GetSelectionNStartVirtualSpace=2726(int selection, )</code>
    /// Returns the virtual space at the start of the selection.
    /// </summary>
    public const int SCI_GETSELECTIONNSTARTVIRTUALSPACE = 2726;
    
    /// <summary>
    /// <code>set void SetSelectionNEnd=2586(int selection, position caret)</code>
    /// Sets the position that ends the selection - this becomes the currentPosition.
    /// </summary>
    public const int SCI_SETSELECTIONNEND = 2586;
    
    /// <summary>
    /// <code>get position GetSelectionNEndVirtualSpace=2727(int selection, )</code>
    /// Returns the virtual space at the end of the selection.
    /// </summary>
    public const int SCI_GETSELECTIONNENDVIRTUALSPACE = 2727;
    
    /// <summary>
    /// <code>get position GetSelectionNEnd=2587(int selection, )</code>
    /// Returns the position at the end of the selection.
    /// </summary>
    public const int SCI_GETSELECTIONNEND = 2587;
    
    /// <summary>
    /// <code>set void SetRectangularSelectionCaret=2588(position caret, )</code>
    /// Set the caret position of the rectangular selection.
    /// </summary>
    public const int SCI_SETRECTANGULARSELECTIONCARET = 2588;
    
    /// <summary>
    /// <code>get position GetRectangularSelectionCaret=2589(, )</code>
    /// Return the caret position of the rectangular selection.
    /// </summary>
    public const int SCI_GETRECTANGULARSELECTIONCARET = 2589;
    
    /// <summary>
    /// <code>set void SetRectangularSelectionAnchor=2590(position anchor, )</code>
    /// Set the anchor position of the rectangular selection.
    /// </summary>
    public const int SCI_SETRECTANGULARSELECTIONANCHOR = 2590;
    
    /// <summary>
    /// <code>get position GetRectangularSelectionAnchor=2591(, )</code>
    /// Return the anchor position of the rectangular selection.
    /// </summary>
    public const int SCI_GETRECTANGULARSELECTIONANCHOR = 2591;
    
    /// <summary>
    /// <code>set void SetRectangularSelectionCaretVirtualSpace=2592(position space, )</code>
    /// Set the virtual space of the caret of the rectangular selection.
    /// </summary>
    public const int SCI_SETRECTANGULARSELECTIONCARETVIRTUALSPACE = 2592;
    
    /// <summary>
    /// <code>get position GetRectangularSelectionCaretVirtualSpace=2593(, )</code>
    /// Return the virtual space of the caret of the rectangular selection.
    /// </summary>
    public const int SCI_GETRECTANGULARSELECTIONCARETVIRTUALSPACE = 2593;
    
    /// <summary>
    /// <code>set void SetRectangularSelectionAnchorVirtualSpace=2594(position space, )</code>
    /// Set the virtual space of the anchor of the rectangular selection.
    /// </summary>
    public const int SCI_SETRECTANGULARSELECTIONANCHORVIRTUALSPACE = 2594;
    
    /// <summary>
    /// <code>get position GetRectangularSelectionAnchorVirtualSpace=2595(, )</code>
    /// Return the virtual space of the anchor of the rectangular selection.
    /// </summary>
    public const int SCI_GETRECTANGULARSELECTIONANCHORVIRTUALSPACE = 2595;
    
    // VirtualSpace
    // ============
    public const int SCVS_NONE = 0;
    public const int SCVS_RECTANGULARSELECTION = 1;
    public const int SCVS_USERACCESSIBLE = 2;
    public const int SCVS_NOWRAPLINESTART = 4;
    
    /// <summary>
    /// <code>set void SetVirtualSpaceOptions=2596(VirtualSpace virtualSpaceOptions, )</code>
    /// Set options for virtual space behaviour.
    /// </summary>
    public const int SCI_SETVIRTUALSPACEOPTIONS = 2596;
    
    /// <summary>
    /// <code>get VirtualSpace GetVirtualSpaceOptions=2597(, )</code>
    /// Return options for virtual space behaviour.
    /// </summary>
    public const int SCI_GETVIRTUALSPACEOPTIONS = 2597;
    
    /// <summary>
    /// <code>set void SetRectangularSelectionModifier=2598(int modifier, )</code>
    /// </summary>
    public const int SCI_SETRECTANGULARSELECTIONMODIFIER = 2598;
    
    /// <summary>
    /// <code>get int GetRectangularSelectionModifier=2599(, )</code>
    /// Get the modifier key used for rectangular selection.
    /// </summary>
    public const int SCI_GETRECTANGULARSELECTIONMODIFIER = 2599;
    
    /// <summary>
    /// <code>set void SetAdditionalSelFore=2600(colour fore, )</code>
    /// Set the foreground colour of additional selections.
    /// Must have previously called SetSelFore with non-zero first argument for this to have an effect.
    /// </summary>
    public const int SCI_SETADDITIONALSELFORE = 2600;
    
    /// <summary>
    /// <code>set void SetAdditionalSelBack=2601(colour back, )</code>
    /// Set the background colour of additional selections.
    /// Must have previously called SetSelBack with non-zero first argument for this to have an effect.
    /// </summary>
    public const int SCI_SETADDITIONALSELBACK = 2601;
    
    /// <summary>
    /// <code>set void SetAdditionalSelAlpha=2602(Alpha alpha, )</code>
    /// Set the alpha of the selection.
    /// </summary>
    public const int SCI_SETADDITIONALSELALPHA = 2602;
    
    /// <summary>
    /// <code>get Alpha GetAdditionalSelAlpha=2603(, )</code>
    /// Get the alpha of the selection.
    /// </summary>
    public const int SCI_GETADDITIONALSELALPHA = 2603;
    
    /// <summary>
    /// <code>set void SetAdditionalCaretFore=2604(colour fore, )</code>
    /// Set the foreground colour of additional carets.
    /// </summary>
    public const int SCI_SETADDITIONALCARETFORE = 2604;
    
    /// <summary>
    /// <code>get colour GetAdditionalCaretFore=2605(, )</code>
    /// Get the foreground colour of additional carets.
    /// </summary>
    public const int SCI_GETADDITIONALCARETFORE = 2605;
    
    /// <summary>
    /// <code>fun void RotateSelection=2606(, )</code>
    /// Set the main selection to the next selection.
    /// </summary>
    public const int SCI_ROTATESELECTION = 2606;
    
    /// <summary>
    /// <code>fun void SwapMainAnchorCaret=2607(, )</code>
    /// Swap that caret and anchor of the main selection.
    /// </summary>
    public const int SCI_SWAPMAINANCHORCARET = 2607;
    
    /// <summary>
    /// <code>fun void MultipleSelectAddNext=2688(, )</code>
    /// Add the next occurrence of the main selection to the set of selections as main.
    /// If the current selection is empty then select word around caret.
    /// </summary>
    public const int SCI_MULTIPLESELECTADDNEXT = 2688;
    
    /// <summary>
    /// <code>fun void MultipleSelectAddEach=2689(, )</code>
    /// Add each occurrence of the main selection in the target to the set of selections.
    /// If the current selection is empty then select word around caret.
    /// </summary>
    public const int SCI_MULTIPLESELECTADDEACH = 2689;
    
    /// <summary>
    /// <code>fun int ChangeLexerState=2617(position start, position end)</code>
    /// Indicate that the internal state of a lexer has changed over a range and therefore
    /// there may be a need to redraw.
    /// </summary>
    public const int SCI_CHANGELEXERSTATE = 2617;
    
    /// <summary>
    /// <code>fun line ContractedFoldNext=2618(line lineStart, )</code>
    /// Find the next line at or after lineStart that is a contracted fold header line.
    /// Return -1 when no more lines.
    /// </summary>
    public const int SCI_CONTRACTEDFOLDNEXT = 2618;
    
    /// <summary>
    /// <code>fun void VerticalCentreCaret=2619(, )</code>
    /// Centre current line in window.
    /// </summary>
    public const int SCI_VERTICALCENTRECARET = 2619;
    
    /// <summary>
    /// <code>fun void MoveSelectedLinesUp=2620(, )</code>
    /// Move the selected lines up one line, shifting the line above after the selection
    /// </summary>
    public const int SCI_MOVESELECTEDLINESUP = 2620;
    
    /// <summary>
    /// <code>fun void MoveSelectedLinesDown=2621(, )</code>
    /// Move the selected lines down one line, shifting the line below before the selection
    /// </summary>
    public const int SCI_MOVESELECTEDLINESDOWN = 2621;
    
    /// <summary>
    /// <code>set void SetIdentifier=2622(int identifier, )</code>
    /// Set the identifier reported as idFrom in notification messages.
    /// </summary>
    public const int SCI_SETIDENTIFIER = 2622;
    
    /// <summary>
    /// <code>get int GetIdentifier=2623(, )</code>
    /// Get the identifier.
    /// </summary>
    public const int SCI_GETIDENTIFIER = 2623;
    
    /// <summary>
    /// <code>set void RGBAImageSetWidth=2624(int width, )</code>
    /// Set the width for future RGBA image data.
    /// </summary>
    public const int SCI_RGBAIMAGESETWIDTH = 2624;
    
    /// <summary>
    /// <code>set void RGBAImageSetHeight=2625(int height, )</code>
    /// Set the height for future RGBA image data.
    /// </summary>
    public const int SCI_RGBAIMAGESETHEIGHT = 2625;
    
    /// <summary>
    /// <code>set void RGBAImageSetScale=2651(int scalePercent, )</code>
    /// Set the scale factor in percent for future RGBA image data.
    /// </summary>
    public const int SCI_RGBAIMAGESETSCALE = 2651;
    
    /// <summary>
    /// <code>fun void MarkerDefineRGBAImage=2626(int markerNumber, string pixels)</code>
    /// Define a marker from RGBA data.
    /// It has the width and height from RGBAImageSetWidth/Height
    /// </summary>
    public const int SCI_MARKERDEFINERGBAIMAGE = 2626;
    
    /// <summary>
    /// <code>fun void RegisterRGBAImage=2627(int type, string pixels)</code>
    /// Register an RGBA image for use in autocompletion lists.
    /// It has the width and height from RGBAImageSetWidth/Height
    /// </summary>
    public const int SCI_REGISTERRGBAIMAGE = 2627;
    
    /// <summary>
    /// <code>fun void ScrollToStart=2628(, )</code>
    /// Scroll to start of document.
    /// </summary>
    public const int SCI_SCROLLTOSTART = 2628;
    
    /// <summary>
    /// <code>fun void ScrollToEnd=2629(, )</code>
    /// Scroll to end of document.
    /// </summary>
    public const int SCI_SCROLLTOEND = 2629;
    
    // Technology
    // ==========
    public const int SC_TECHNOLOGY_DEFAULT = 0;
    public const int SC_TECHNOLOGY_DIRECTWRITE = 1;
    public const int SC_TECHNOLOGY_DIRECTWRITERETAIN = 2;
    public const int SC_TECHNOLOGY_DIRECTWRITEDC = 3;
    public const int SC_TECHNOLOGY_DIRECT_WRITE_1 = 4;
    
    /// <summary>
    /// <code>set void SetTechnology=2630(Technology technology, )</code>
    /// Set the technology used.
    /// </summary>
    public const int SCI_SETTECHNOLOGY = 2630;
    
    /// <summary>
    /// <code>get Technology GetTechnology=2631(, )</code>
    /// Get the tech.
    /// </summary>
    public const int SCI_GETTECHNOLOGY = 2631;
    
    /// <summary>
    /// <code>fun pointer CreateLoader=2632(position bytes, DocumentOption documentOptions)</code>
    /// Create an ILoader*.
    /// </summary>
    public const int SCI_CREATELOADER = 2632;
    
    /// <summary>
    /// <code>fun void FindIndicatorShow=2640(position start, position end)</code>
    /// On macOS, show a find indicator.
    /// </summary>
    public const int SCI_FINDINDICATORSHOW = 2640;
    
    /// <summary>
    /// <code>fun void FindIndicatorFlash=2641(position start, position end)</code>
    /// On macOS, flash a find indicator, then fade out.
    /// </summary>
    public const int SCI_FINDINDICATORFLASH = 2641;
    
    /// <summary>
    /// <code>fun void FindIndicatorHide=2642(, )</code>
    /// On macOS, hide the find indicator.
    /// </summary>
    public const int SCI_FINDINDICATORHIDE = 2642;
    
    /// <summary>
    /// <code>fun void VCHomeDisplay=2652(, )</code>
    /// Move caret to before first visible character on display line.
    /// If already there move to first character on display line.
    /// </summary>
    public const int SCI_VCHOMEDISPLAY = 2652;
    
    /// <summary>
    /// <code>fun void VCHomeDisplayExtend=2653(, )</code>
    /// Like VCHomeDisplay but extending selection to new caret position.
    /// </summary>
    public const int SCI_VCHOMEDISPLAYEXTEND = 2653;
    
    /// <summary>
    /// <code>get bool GetCaretLineVisibleAlways=2654(, )</code>
    /// Is the caret line always visible?
    /// </summary>
    public const int SCI_GETCARETLINEVISIBLEALWAYS = 2654;
    
    /// <summary>
    /// <code>set void SetCaretLineVisibleAlways=2655(bool alwaysVisible, )</code>
    /// Sets the caret line to always visible.
    /// </summary>
    public const int SCI_SETCARETLINEVISIBLEALWAYS = 2655;
    
    // LineEndType
    // ===========
    public const int SC_LINE_END_TYPE_DEFAULT = 0;
    public const int SC_LINE_END_TYPE_UNICODE = 1;
    
    /// <summary>
    /// <code>set void SetLineEndTypesAllowed=2656(LineEndType lineEndBitSet, )</code>
    /// Set the line end types that the application wants to use. May not be used if incompatible with lexer or encoding.
    /// </summary>
    public const int SCI_SETLINEENDTYPESALLOWED = 2656;
    
    /// <summary>
    /// <code>get LineEndType GetLineEndTypesAllowed=2657(, )</code>
    /// Get the line end types currently allowed.
    /// </summary>
    public const int SCI_GETLINEENDTYPESALLOWED = 2657;
    
    /// <summary>
    /// <code>get LineEndType GetLineEndTypesActive=2658(, )</code>
    /// Get the line end types currently recognised. May be a subset of the allowed types due to lexer limitation.
    /// </summary>
    public const int SCI_GETLINEENDTYPESACTIVE = 2658;
    
    /// <summary>
    /// <code>set void SetRepresentation=2665(string encodedCharacter, string representation)</code>
    /// Set the way a character is drawn.
    /// </summary>
    public const int SCI_SETREPRESENTATION = 2665;
    
    /// <summary>
    /// <code>get int GetRepresentation=2666(string encodedCharacter, stringresult representation)</code>
    /// Get the way a character is drawn.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_GETREPRESENTATION = 2666;
    
    /// <summary>
    /// <code>fun void ClearRepresentation=2667(string encodedCharacter, )</code>
    /// Remove a character representation.
    /// </summary>
    public const int SCI_CLEARREPRESENTATION = 2667;
    
    /// <summary>
    /// <code>fun void ClearAllRepresentations=2770(, )</code>
    /// Clear representations to default.
    /// </summary>
    public const int SCI_CLEARALLREPRESENTATIONS = 2770;
    
    // RepresentationAppearance
    // ========================
    public const int SC_REPRESENTATION_PLAIN = 0;
    public const int SC_REPRESENTATION_BLOB = 1;
    public const uint SC_REPRESENTATION_COLOUR = 0x10;
    
    /// <summary>
    /// <code>set void SetRepresentationAppearance=2766(string encodedCharacter, RepresentationAppearance appearance)</code>
    /// Set the appearance of a representation.
    /// </summary>
    public const int SCI_SETREPRESENTATIONAPPEARANCE = 2766;
    
    /// <summary>
    /// <code>get RepresentationAppearance GetRepresentationAppearance=2767(string encodedCharacter, )</code>
    /// Get the appearance of a representation.
    /// </summary>
    public const int SCI_GETREPRESENTATIONAPPEARANCE = 2767;
    
    /// <summary>
    /// <code>set void SetRepresentationColour=2768(string encodedCharacter, colouralpha colour)</code>
    /// Set the colour of a representation.
    /// </summary>
    public const int SCI_SETREPRESENTATIONCOLOUR = 2768;
    
    /// <summary>
    /// <code>get colouralpha GetRepresentationColour=2769(string encodedCharacter, )</code>
    /// Get the colour of a representation.
    /// </summary>
    public const int SCI_GETREPRESENTATIONCOLOUR = 2769;
    
    /// <summary>
    /// <code>set void EOLAnnotationSetText=2740(line line, string text)</code>
    /// Set the end of line annotation text for a line
    /// </summary>
    public const int SCI_EOLANNOTATIONSETTEXT = 2740;
    
    /// <summary>
    /// <code>get int EOLAnnotationGetText=2741(line line, stringresult text)</code>
    /// Get the end of line annotation text for a line
    /// </summary>
    public const int SCI_EOLANNOTATIONGETTEXT = 2741;
    
    /// <summary>
    /// <code>set void EOLAnnotationSetStyle=2742(line line, int style)</code>
    /// Set the style number for the end of line annotations for a line
    /// </summary>
    public const int SCI_EOLANNOTATIONSETSTYLE = 2742;
    
    /// <summary>
    /// <code>get int EOLAnnotationGetStyle=2743(line line, )</code>
    /// Get the style number for the end of line annotations for a line
    /// </summary>
    public const int SCI_EOLANNOTATIONGETSTYLE = 2743;
    
    /// <summary>
    /// <code>fun void EOLAnnotationClearAll=2744(, )</code>
    /// Clear the end of annotations from all lines
    /// </summary>
    public const int SCI_EOLANNOTATIONCLEARALL = 2744;
    
    // EOLAnnotationVisible
    // ====================
    public const uint EOLANNOTATION_HIDDEN = 0x0;
    public const uint EOLANNOTATION_STANDARD = 0x1;
    public const uint EOLANNOTATION_BOXED = 0x2;
    public const uint EOLANNOTATION_STADIUM = 0x100;
    public const uint EOLANNOTATION_FLAT_CIRCLE = 0x101;
    public const uint EOLANNOTATION_ANGLE_CIRCLE = 0x102;
    public const uint EOLANNOTATION_CIRCLE_FLAT = 0x110;
    public const uint EOLANNOTATION_FLATS = 0x111;
    public const uint EOLANNOTATION_ANGLE_FLAT = 0x112;
    public const uint EOLANNOTATION_CIRCLE_ANGLE = 0x120;
    public const uint EOLANNOTATION_FLAT_ANGLE = 0x121;
    public const uint EOLANNOTATION_ANGLES = 0x122;
    
    /// <summary>
    /// <code>set void EOLAnnotationSetVisible=2745(EOLAnnotationVisible visible, )</code>
    /// Set the visibility for the end of line annotations for a view
    /// </summary>
    public const int SCI_EOLANNOTATIONSETVISIBLE = 2745;
    
    /// <summary>
    /// <code>get EOLAnnotationVisible EOLAnnotationGetVisible=2746(, )</code>
    /// Get the visibility for the end of line annotations for a view
    /// </summary>
    public const int SCI_EOLANNOTATIONGETVISIBLE = 2746;
    
    /// <summary>
    /// <code>set void EOLAnnotationSetStyleOffset=2747(int style, )</code>
    /// Get the start of the range of style numbers used for end of line annotations
    /// </summary>
    public const int SCI_EOLANNOTATIONSETSTYLEOFFSET = 2747;
    
    /// <summary>
    /// <code>get int EOLAnnotationGetStyleOffset=2748(, )</code>
    /// Get the start of the range of style numbers used for end of line annotations
    /// </summary>
    public const int SCI_EOLANNOTATIONGETSTYLEOFFSET = 2748;
    
    // Supports
    // ========
    public const int SC_SUPPORTS_LINE_DRAWS_FINAL = 0;
    public const int SC_SUPPORTS_PIXEL_DIVISIONS = 1;
    public const int SC_SUPPORTS_FRACTIONAL_STROKE_WIDTH = 2;
    public const int SC_SUPPORTS_TRANSLUCENT_STROKE = 3;
    public const int SC_SUPPORTS_PIXEL_MODIFICATION = 4;
    public const int SC_SUPPORTS_THREAD_SAFE_MEASURE_WIDTHS = 5;
    
    /// <summary>
    /// <code>get bool SupportsFeature=2750(Supports feature, )</code>
    /// Get whether a feature is supported
    /// </summary>
    public const int SCI_SUPPORTSFEATURE = 2750;
    
    // LineCharacterIndexType
    // ======================
    public const int SC_LINECHARACTERINDEX_NONE = 0;
    public const int SC_LINECHARACTERINDEX_UTF32 = 1;
    public const int SC_LINECHARACTERINDEX_UTF16 = 2;
    
    /// <summary>
    /// <code>get LineCharacterIndexType GetLineCharacterIndex=2710(, )</code>
    /// Retrieve line character index state.
    /// </summary>
    public const int SCI_GETLINECHARACTERINDEX = 2710;
    
    /// <summary>
    /// <code>fun void AllocateLineCharacterIndex=2711(LineCharacterIndexType lineCharacterIndex, )</code>
    /// Request line character index be created or its use count increased.
    /// </summary>
    public const int SCI_ALLOCATELINECHARACTERINDEX = 2711;
    
    /// <summary>
    /// <code>fun void ReleaseLineCharacterIndex=2712(LineCharacterIndexType lineCharacterIndex, )</code>
    /// Decrease use count of line character index and remove if 0.
    /// </summary>
    public const int SCI_RELEASELINECHARACTERINDEX = 2712;
    
    /// <summary>
    /// <code>fun line LineFromIndexPosition=2713(position pos, LineCharacterIndexType lineCharacterIndex)</code>
    /// Retrieve the document line containing a position measured in index units.
    /// </summary>
    public const int SCI_LINEFROMINDEXPOSITION = 2713;
    
    /// <summary>
    /// <code>fun position IndexPositionFromLine=2714(line line, LineCharacterIndexType lineCharacterIndex)</code>
    /// Retrieve the position measured in index units at the start of a document line.
    /// </summary>
    public const int SCI_INDEXPOSITIONFROMLINE = 2714;
    
    /// <summary>
    /// <code>get bool GetDragDropEnabled=2818(, )</code>
    /// Get whether drag-and-drop is enabled or disabled
    /// </summary>
    public const int SCI_GETDRAGDROPENABLED = 2818;
    
    /// <summary>
    /// <code>set void SetDragDropEnabled=2819(bool dragDropEnabled, )</code>
    /// Enable or disable drag-and-drop
    /// </summary>
    public const int SCI_SETDRAGDROPENABLED = 2819;
    
    /// <summary>
    /// <code>fun void StartRecord=3001(, )</code>
    /// Start notifying the container of all key presses and commands.
    /// </summary>
    public const int SCI_STARTRECORD = 3001;
    
    /// <summary>
    /// <code>fun void StopRecord=3002(, )</code>
    /// Stop notifying the container of all key presses and commands.
    /// </summary>
    public const int SCI_STOPRECORD = 3002;
    
    /// <summary>
    /// <code>get int GetLexer=4002(, )</code>
    /// Retrieve the lexing language of the document.
    /// </summary>
    public const int SCI_GETLEXER = 4002;
    
    /// <summary>
    /// <code>fun void Colourise=4003(position start, position end)</code>
    /// Colourise a segment of the document using the current lexing language.
    /// </summary>
    public const int SCI_COLOURISE = 4003;
    
    /// <summary>
    /// <code>set void SetProperty=4004(string key, string value)</code>
    /// Set up a value that may be used by a lexer for some optional feature.
    /// </summary>
    public const int SCI_SETPROPERTY = 4004;
    
    /// <summary>Maximum value of keywordSet parameter of SetKeyWords.</summary>
    public const int KEYWORDSET_MAX = 8;
    
    /// <summary>
    /// <code>set void SetKeyWords=4005(int keyWordSet, string keyWords)</code>
    /// Set up the key words used by the lexer.
    /// </summary>
    public const int SCI_SETKEYWORDS = 4005;
    
    /// <summary>
    /// <code>get int GetProperty=4008(string key, stringresult value)</code>
    /// Retrieve a "property" value previously set with SetProperty.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_GETPROPERTY = 4008;
    
    /// <summary>
    /// <code>get int GetPropertyExpanded=4009(string key, stringresult value)</code>
    /// Retrieve a "property" value previously set with SetProperty,
    /// with "$()" variable replacement on returned buffer.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_GETPROPERTYEXPANDED = 4009;
    
    /// <summary>
    /// <code>get int GetPropertyInt=4010(string key, int defaultValue)</code>
    /// Retrieve a "property" value previously set with SetProperty,
    /// interpreted as an int AFTER any "$()" variable replacement.
    /// </summary>
    public const int SCI_GETPROPERTYINT = 4010;
    
    /// <summary>
    /// <code>get int GetLexerLanguage=4012(, stringresult language)</code>
    /// Retrieve the name of the lexer.
    /// Return the length of the text.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_GETLEXERLANGUAGE = 4012;
    
    /// <summary>
    /// <code>fun pointer PrivateLexerCall=4013(int operation, pointer pointer)</code>
    /// For private communication between an application and a known lexer.
    /// </summary>
    public const int SCI_PRIVATELEXERCALL = 4013;
    
    /// <summary>
    /// <code>fun int PropertyNames=4014(, stringresult names)</code>
    /// Retrieve a '\n' separated list of properties understood by the current lexer.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_PROPERTYNAMES = 4014;
    
    // TypeProperty
    // ============
    public const int SC_TYPE_BOOLEAN = 0;
    public const int SC_TYPE_INTEGER = 1;
    public const int SC_TYPE_STRING = 2;
    
    /// <summary>
    /// <code>fun TypeProperty PropertyType=4015(string name, )</code>
    /// Retrieve the type of a property.
    /// </summary>
    public const int SCI_PROPERTYTYPE = 4015;
    
    /// <summary>
    /// <code>fun int DescribeProperty=4016(string name, stringresult description)</code>
    /// Describe a property.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_DESCRIBEPROPERTY = 4016;
    
    /// <summary>
    /// <code>fun int DescribeKeyWordSets=4017(, stringresult descriptions)</code>
    /// Retrieve a '\n' separated list of descriptions of the keyword sets understood by the current lexer.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_DESCRIBEKEYWORDSETS = 4017;
    
    /// <summary>
    /// <code>get LineEndType GetLineEndTypesSupported=4018(, )</code>
    /// Bit set of LineEndType enumertion for which line ends beyond the standard
    /// LF, CR, and CRLF are supported by the lexer.
    /// </summary>
    public const int SCI_GETLINEENDTYPESSUPPORTED = 4018;
    
    /// <summary>
    /// <code>fun int AllocateSubStyles=4020(int styleBase, int numberStyles)</code>
    /// Allocate a set of sub styles for a particular base style, returning start of range
    /// </summary>
    public const int SCI_ALLOCATESUBSTYLES = 4020;
    
    /// <summary>
    /// <code>get int GetSubStylesStart=4021(int styleBase, )</code>
    /// The starting style number for the sub styles associated with a base style
    /// </summary>
    public const int SCI_GETSUBSTYLESSTART = 4021;
    
    /// <summary>
    /// <code>get int GetSubStylesLength=4022(int styleBase, )</code>
    /// The number of sub styles associated with a base style
    /// </summary>
    public const int SCI_GETSUBSTYLESLENGTH = 4022;
    
    /// <summary>
    /// <code>get int GetStyleFromSubStyle=4027(int subStyle, )</code>
    /// For a sub style, return the base style, else return the argument.
    /// </summary>
    public const int SCI_GETSTYLEFROMSUBSTYLE = 4027;
    
    /// <summary>
    /// <code>get int GetPrimaryStyleFromStyle=4028(int style, )</code>
    /// For a secondary style, return the primary style, else return the argument.
    /// </summary>
    public const int SCI_GETPRIMARYSTYLEFROMSTYLE = 4028;
    
    /// <summary>
    /// <code>fun void FreeSubStyles=4023(, )</code>
    /// Free allocated sub styles
    /// </summary>
    public const int SCI_FREESUBSTYLES = 4023;
    
    /// <summary>
    /// <code>set void SetIdentifiers=4024(int style, string identifiers)</code>
    /// Set the identifiers that are shown in a particular style
    /// </summary>
    public const int SCI_SETIDENTIFIERS = 4024;
    
    /// <summary>
    /// <code>get int DistanceToSecondaryStyles=4025(, )</code>
    /// Where styles are duplicated by a feature such as active/inactive code
    /// return the distance between the two types.
    /// </summary>
    public const int SCI_DISTANCETOSECONDARYSTYLES = 4025;
    
    /// <summary>
    /// <code>get int GetSubStyleBases=4026(, stringresult styles)</code>
    /// Get the set of base styles that can be extended with sub styles
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_GETSUBSTYLEBASES = 4026;
    
    /// <summary>
    /// <code>get int GetNamedStyles=4029(, )</code>
    /// Retrieve the number of named styles for the lexer.
    /// </summary>
    public const int SCI_GETNAMEDSTYLES = 4029;
    
    /// <summary>
    /// <code>fun int NameOfStyle=4030(int style, stringresult name)</code>
    /// Retrieve the name of a style.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_NAMEOFSTYLE = 4030;
    
    /// <summary>
    /// <code>fun int TagsOfStyle=4031(int style, stringresult tags)</code>
    /// Retrieve a ' ' separated list of style tags like "literal quoted string".
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_TAGSOFSTYLE = 4031;
    
    /// <summary>
    /// <code>fun int DescriptionOfStyle=4032(int style, stringresult description)</code>
    /// Retrieve a description of a style.
    /// Result is NUL-terminated.
    /// </summary>
    public const int SCI_DESCRIPTIONOFSTYLE = 4032;
    
    /// <summary>
    /// <code>set void SetILexer=4033(, pointer ilexer)</code>
    /// Set the lexer from an ILexer*.
    /// </summary>
    public const int SCI_SETILEXER = 4033;
    
    // ModificationFlags
    // =================
    public const uint SC_MOD_NONE = 0x0;
    public const uint SC_MOD_INSERTTEXT = 0x1;
    public const uint SC_MOD_DELETETEXT = 0x2;
    public const uint SC_MOD_CHANGESTYLE = 0x4;
    public const uint SC_MOD_CHANGEFOLD = 0x8;
    public const uint SC_PERFORMED_USER = 0x10;
    public const uint SC_PERFORMED_UNDO = 0x20;
    public const uint SC_PERFORMED_REDO = 0x40;
    public const uint SC_MULTISTEPUNDOREDO = 0x80;
    public const uint SC_LASTSTEPINUNDOREDO = 0x100;
    public const uint SC_MOD_CHANGEMARKER = 0x200;
    public const uint SC_MOD_BEFOREINSERT = 0x400;
    public const uint SC_MOD_BEFOREDELETE = 0x800;
    public const uint SC_MULTILINEUNDOREDO = 0x1000;
    public const uint SC_STARTACTION = 0x2000;
    public const uint SC_MOD_CHANGEINDICATOR = 0x4000;
    public const uint SC_MOD_CHANGELINESTATE = 0x8000;
    public const uint SC_MOD_CHANGEMARGIN = 0x10000;
    public const uint SC_MOD_CHANGEANNOTATION = 0x20000;
    public const uint SC_MOD_CONTAINER = 0x40000;
    public const uint SC_MOD_LEXERSTATE = 0x80000;
    public const uint SC_MOD_INSERTCHECK = 0x100000;
    public const uint SC_MOD_CHANGETABSTOPS = 0x200000;
    public const uint SC_MOD_CHANGEEOLANNOTATION = 0x400000;
    public const uint SC_MODEVENTMASKALL = 0x7FFFFF;
    
    // Update
    // ======
    public const uint SC_UPDATE_NONE = 0x0;
    public const uint SC_UPDATE_CONTENT = 0x1;
    public const uint SC_UPDATE_SELECTION = 0x2;
    public const uint SC_UPDATE_V_SCROLL = 0x4;
    public const uint SC_UPDATE_H_SCROLL = 0x8;
    
    // FocusChange
    // ===========
    public const int SCEN_CHANGE = 768;
    public const int SCEN_SETFOCUS = 512;
    public const int SCEN_KILLFOCUS = 256;
    
    // Keys
    // ====
    public const int SCK_DOWN = 300;
    public const int SCK_UP = 301;
    public const int SCK_LEFT = 302;
    public const int SCK_RIGHT = 303;
    public const int SCK_HOME = 304;
    public const int SCK_END = 305;
    public const int SCK_PRIOR = 306;
    public const int SCK_NEXT = 307;
    public const int SCK_DELETE = 308;
    public const int SCK_INSERT = 309;
    public const int SCK_ESCAPE = 7;
    public const int SCK_BACK = 8;
    public const int SCK_TAB = 9;
    public const int SCK_RETURN = 13;
    public const int SCK_ADD = 310;
    public const int SCK_SUBTRACT = 311;
    public const int SCK_DIVIDE = 312;
    public const int SCK_WIN = 313;
    public const int SCK_RWIN = 314;
    public const int SCK_MENU = 315;
    
    // KeyMod
    // ======
    public const int SCMOD_NORM = 0;
    public const int SCMOD_SHIFT = 1;
    public const int SCMOD_CTRL = 2;
    public const int SCMOD_ALT = 4;
    public const int SCMOD_SUPER = 8;
    public const int SCMOD_META = 16;
    
    // CompletionMethods
    // =================
    public const int SC_AC_FILLUP = 1;
    public const int SC_AC_DOUBLECLICK = 2;
    public const int SC_AC_TAB = 3;
    public const int SC_AC_NEWLINE = 4;
    public const int SC_AC_COMMAND = 5;
    public const int SC_AC_SINGLE_CHOICE = 6;
    
    // CharacterSource
    // ===============
    /// <summary>Direct input characters.</summary>
    public const int SC_CHARACTERSOURCE_DIRECT_INPUT = 0;
    /// <summary>IME (inline mode) or dead key tentative input characters.</summary>
    public const int SC_CHARACTERSOURCE_TENTATIVE_INPUT = 1;
    /// <summary>IME (either inline or windowed mode) full composited string.</summary>
    public const int SC_CHARACTERSOURCE_IME_RESULT = 2;
    
    /// <summary>
    /// <code>evt void StyleNeeded=2000(int position)</code>
    /// </summary>
    public const int SCN_STYLENEEDED = 2000;
    
    /// <summary>
    /// <code>evt void CharAdded=2001(int ch, int characterSource)</code>
    /// </summary>
    public const int SCN_CHARADDED = 2001;
    
    /// <summary>
    /// <code>evt void SavePointReached=2002()</code>
    /// </summary>
    public const int SCN_SAVEPOINTREACHED = 2002;
    
    /// <summary>
    /// <code>evt void SavePointLeft=2003()</code>
    /// </summary>
    public const int SCN_SAVEPOINTLEFT = 2003;
    
    /// <summary>
    /// <code>evt void ModifyAttemptRO=2004()</code>
    /// </summary>
    public const int SCN_MODIFYATTEMPTRO = 2004;
    
    /// <summary>
    /// <code>evt void Key=2005(int ch, int modifiers)</code>
    /// GTK Specific to work around focus and accelerator problems:
    /// </summary>
    public const int SCN_KEY = 2005;
    
    /// <summary>
    /// <code>evt void DoubleClick=2006(int modifiers, int position, int line)</code>
    /// </summary>
    public const int SCN_DOUBLECLICK = 2006;
    
    /// <summary>
    /// <code>evt void UpdateUI=2007(int updated)</code>
    /// </summary>
    public const int SCN_UPDATEUI = 2007;
    
    /// <summary>
    /// <code>evt void Modified=2008(int position, int modificationType, string text, int length, int linesAdded, int line, int foldLevelNow, int foldLevelPrev, int token, int annotationLinesAdded)</code>
    /// </summary>
    public const int SCN_MODIFIED = 2008;
    
    /// <summary>
    /// <code>evt void MacroRecord=2009(int message, int wParam, int lParam)</code>
    /// </summary>
    public const int SCN_MACRORECORD = 2009;
    
    /// <summary>
    /// <code>evt void MarginClick=2010(int modifiers, int position, int margin)</code>
    /// </summary>
    public const int SCN_MARGINCLICK = 2010;
    
    /// <summary>
    /// <code>evt void NeedShown=2011(int position, int length)</code>
    /// </summary>
    public const int SCN_NEEDSHOWN = 2011;
    
    /// <summary>
    /// <code>evt void Painted=2013()</code>
    /// </summary>
    public const int SCN_PAINTED = 2013;
    
    /// <summary>
    /// <code>evt void UserListSelection=2014(int listType, string text, int position, int ch, CompletionMethods listCompletionMethod)</code>
    /// </summary>
    public const int SCN_USERLISTSELECTION = 2014;
    
    /// <summary>
    /// <code>evt void URIDropped=2015(string text)</code>
    /// </summary>
    public const int SCN_URIDROPPED = 2015;
    
    /// <summary>
    /// <code>evt void DwellStart=2016(int position, int x, int y)</code>
    /// </summary>
    public const int SCN_DWELLSTART = 2016;
    
    /// <summary>
    /// <code>evt void DwellEnd=2017(int position, int x, int y)</code>
    /// </summary>
    public const int SCN_DWELLEND = 2017;
    
    /// <summary>
    /// <code>evt void Zoom=2018()</code>
    /// </summary>
    public const int SCN_ZOOM = 2018;
    
    /// <summary>
    /// <code>evt void HotSpotClick=2019(int modifiers, int position)</code>
    /// </summary>
    public const int SCN_HOTSPOTCLICK = 2019;
    
    /// <summary>
    /// <code>evt void HotSpotDoubleClick=2020(int modifiers, int position)</code>
    /// </summary>
    public const int SCN_HOTSPOTDOUBLECLICK = 2020;
    
    /// <summary>
    /// <code>evt void CallTipClick=2021(int position)</code>
    /// </summary>
    public const int SCN_CALLTIPCLICK = 2021;
    
    /// <summary>
    /// <code>evt void AutoCSelection=2022(string text, int position, int ch, CompletionMethods listCompletionMethod)</code>
    /// </summary>
    public const int SCN_AUTOCSELECTION = 2022;
    
    /// <summary>
    /// <code>evt void IndicatorClick=2023(int modifiers, int position)</code>
    /// </summary>
    public const int SCN_INDICATORCLICK = 2023;
    
    /// <summary>
    /// <code>evt void IndicatorRelease=2024(int modifiers, int position)</code>
    /// </summary>
    public const int SCN_INDICATORRELEASE = 2024;
    
    /// <summary>
    /// <code>evt void AutoCCancelled=2025()</code>
    /// </summary>
    public const int SCN_AUTOCCANCELLED = 2025;
    
    /// <summary>
    /// <code>evt void AutoCCharDeleted=2026()</code>
    /// </summary>
    public const int SCN_AUTOCCHARDELETED = 2026;
    
    /// <summary>
    /// <code>evt void HotSpotReleaseClick=2027(int modifiers, int position)</code>
    /// </summary>
    public const int SCN_HOTSPOTRELEASECLICK = 2027;
    
    /// <summary>
    /// <code>evt void FocusIn=2028()</code>
    /// </summary>
    public const int SCN_FOCUSIN = 2028;
    
    /// <summary>
    /// <code>evt void FocusOut=2029()</code>
    /// </summary>
    public const int SCN_FOCUSOUT = 2029;
    
    /// <summary>
    /// <code>evt void AutoCCompleted=2030(string text, int position, int ch, CompletionMethods listCompletionMethod)</code>
    /// </summary>
    public const int SCN_AUTOCCOMPLETED = 2030;
    
    /// <summary>
    /// <code>evt void MarginRightClick=2031(int modifiers, int position, int margin)</code>
    /// </summary>
    public const int SCN_MARGINRIGHTCLICK = 2031;
    
    /// <summary>
    /// <code>evt void AutoCSelectionChange=2032(int listType, string text, int position)</code>
    /// </summary>
    public const int SCN_AUTOCSELECTIONCHANGE = 2032;
    
    // Bidirectional
    // =============
    public const int SC_BIDIRECTIONAL_DISABLED = 0;
    public const int SC_BIDIRECTIONAL_L2R = 1;
    public const int SC_BIDIRECTIONAL_R2L = 2;
    
    /// <summary>
    /// <code>get Bidirectional GetBidirectional=2708(, )</code>
    /// Retrieve bidirectional text display state.
    /// </summary>
    public const int SCI_GETBIDIRECTIONAL = 2708;
    
    /// <summary>
    /// <code>set void SetBidirectional=2709(Bidirectional bidirectional, )</code>
    /// Set bidirectional text display state.
    /// </summary>
    public const int SCI_SETBIDIRECTIONAL = 2709;
    
    /// <summary>
    /// <code>set void SetStyleBits=2090(int bits, )</code>
    /// Divide each styling byte into lexical class bits (default: 5) and indicator
    /// bits (default: 3). If a lexer requires more than 32 lexical states, then this
    /// is used to expand the possible states.
    /// </summary>
    [Obsolete("Deprecated")] public const int SCI_SETSTYLEBITS = 2090;
    
    /// <summary>
    /// <code>get int GetStyleBits=2091(, )</code>
    /// Retrieve number of bits in style bytes used to hold the lexical state.
    /// </summary>
    [Obsolete("Deprecated")] public const int SCI_GETSTYLEBITS = 2091;
    
    /// <summary>
    /// <code>get int GetStyleBitsNeeded=4011(, )</code>
    /// Retrieve the number of bits the current lexer needs for styling.
    /// </summary>
    [Obsolete("Deprecated")] public const int SCI_GETSTYLEBITSNEEDED = 4011;
    
    /// <summary>
    /// <code>set void SetKeysUnicode=2521(bool keysUnicode, )</code>
    /// Always interpret keyboard input as Unicode
    /// </summary>
    [Obsolete("Deprecated")] public const int SCI_SETKEYSUNICODE = 2521;
    
    /// <summary>
    /// <code>get bool GetKeysUnicode=2522(, )</code>
    /// Are keys always interpreted as Unicode?
    /// </summary>
    [Obsolete("Deprecated")] public const int SCI_GETKEYSUNICODE = 2522;
    
    /// <summary>
    /// <code>get bool GetTwoPhaseDraw=2283(, )</code>
    /// Is drawing done in two phases with backgrounds drawn before foregrounds?
    /// </summary>
    [Obsolete("Deprecated")] public const int SCI_GETTWOPHASEDRAW = 2283;
    
    /// <summary>
    /// <code>set void SetTwoPhaseDraw=2284(bool twoPhase, )</code>
    /// In twoPhaseDraw mode, drawing is performed in two phases, first the background
    /// and then the foreground. This avoids chopping off characters that overlap the next run.
    /// </summary>
    [Obsolete("Deprecated")] public const int SCI_SETTWOPHASEDRAW = 2284;
    
    [Obsolete("Deprecated")] public const uint INDIC0_MASK = 0x20;
    
    [Obsolete("Deprecated")] public const uint INDIC1_MASK = 0x40;
    
    [Obsolete("Deprecated")] public const uint INDIC2_MASK = 0x80;
    
    [Obsolete("Deprecated")] public const uint INDICS_MASK = 0xE0;
}
