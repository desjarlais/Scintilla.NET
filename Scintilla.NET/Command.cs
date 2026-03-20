namespace ScintillaNET;

/// <summary>
/// Actions which can be performed by the application or bound to keys in a <see cref="Scintilla" /> control.
/// </summary>
public enum Command
{
    /// <summary>
    /// When bound to keys performs the standard platform behavior.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Performs no action and when bound to keys prevents them from propagating to the parent window.
    /// </summary>
    Null = SciApi.SCI_NULL,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret down one line.
    /// </summary>
    LineDown = SciApi.SCI_LINEDOWN,

    /// <summary>
    /// Extends the selection down one line.
    /// </summary>
    LineDownExtend = SciApi.SCI_LINEDOWNEXTEND,

    /// <summary>
    /// Extends the rectangular selection down one line.
    /// </summary>
    LineDownRectExtend = SciApi.SCI_LINEDOWNRECTEXTEND,

    /// <summary>
    /// Scrolls down one line.
    /// </summary>
    LineScrollDown = SciApi.SCI_LINESCROLLDOWN,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret up one line.
    /// </summary>
    LineUp = SciApi.SCI_LINEUP,

    /// <summary>
    /// Extends the selection up one line.
    /// </summary>
    LineUpExtend = SciApi.SCI_LINEUPEXTEND,

    /// <summary>
    /// Extends the rectangular selection up one line.
    /// </summary>
    LineUpRectExtend = SciApi.SCI_LINEUPRECTEXTEND,

    /// <summary>
    /// Scrolls up one line.
    /// </summary>
    LineScrollUp = SciApi.SCI_LINESCROLLUP,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret down one paragraph.
    /// </summary>
    ParaDown = SciApi.SCI_PARADOWN,

    /// <summary>
    /// Extends the selection down one paragraph.
    /// </summary>
    ParaDownExtend = SciApi.SCI_PARADOWNEXTEND,

    /// <summary>
    /// Moves the caret up one paragraph.
    /// </summary>
    ParaUp = SciApi.SCI_PARAUP,

    /// <summary>
    /// Extends the selection up one paragraph.
    /// </summary>
    ParaUpExtend = SciApi.SCI_PARAUPEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret left one character.
    /// </summary>
    CharLeft = SciApi.SCI_CHARLEFT,

    /// <summary>
    /// Extends the selection left one character.
    /// </summary>
    CharLeftExtend = SciApi.SCI_CHARLEFTEXTEND,

    /// <summary>
    /// Extends the rectangular selection left one character.
    /// </summary>
    CharLeftRectExtend = SciApi.SCI_CHARLEFTRECTEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret right one character.
    /// </summary>
    CharRight = SciApi.SCI_CHARRIGHT,

    /// <summary>
    /// Extends the selection right one character.
    /// </summary>
    CharRightExtend = SciApi.SCI_CHARRIGHTEXTEND,

    /// <summary>
    /// Extends the rectangular selection right one character.
    /// </summary>
    CharRightRectExtend = SciApi.SCI_CHARRIGHTRECTEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the start of the previous word.
    /// </summary>
    WordLeft = SciApi.SCI_WORDLEFT,

    /// <summary>
    /// Extends the selection to the start of the previous word.
    /// </summary>
    WordLeftExtend = SciApi.SCI_WORDLEFTEXTEND,

    /// <summary>
    /// Moves the caret to the start of the next word.
    /// </summary>
    WordRight = SciApi.SCI_WORDRIGHT,

    /// <summary>
    /// Extends the selection to the start of the next word.
    /// </summary>
    WordRightExtend = SciApi.SCI_WORDRIGHTEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the end of the previous word.
    /// </summary>
    WordLeftEnd = SciApi.SCI_WORDLEFTEND,

    /// <summary>
    /// Extends the selection to the end of the previous word.
    /// </summary>
    WordLeftEndExtend = SciApi.SCI_WORDLEFTENDEXTEND,

    /// <summary>
    /// Moves the caret to the end of the next word.
    /// </summary>
    WordRightEnd = SciApi.SCI_WORDRIGHTEND,

    /// <summary>
    /// Extends the selection to the end of the next word.
    /// </summary>
    WordRightEndExtend = SciApi.SCI_WORDRIGHTENDEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the previous word segment (case change or underscore).
    /// </summary>
    WordPartLeft = SciApi.SCI_WORDPARTLEFT,

    /// <summary>
    /// Extends the selection to the previous word segment (case change or underscore).
    /// </summary>
    WordPartLeftExtend = SciApi.SCI_WORDPARTLEFTEXTEND,

    /// <summary>
    /// Moves the caret to the next word segment (case change or underscore).
    /// </summary>
    WordPartRight = SciApi.SCI_WORDPARTRIGHT,

    /// <summary>
    /// Extends the selection to the next word segment (case change or underscore).
    /// </summary>
    WordPartRightExtend = SciApi.SCI_WORDPARTRIGHTEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the start of the line.
    /// </summary>
    Home = SciApi.SCI_HOME,

    /// <summary>
    /// Extends the selection to the start of the line.
    /// </summary>
    HomeExtend = SciApi.SCI_HOMEEXTEND,

    /// <summary>
    /// Extends the rectangular selection to the start of the line.
    /// </summary>
    HomeRectExtend = SciApi.SCI_HOMERECTEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the start of the display line.
    /// </summary>
    HomeDisplay = SciApi.SCI_HOMEDISPLAY,

    /// <summary>
    /// Extends the selection to the start of the display line.
    /// </summary>
    HomeDisplayExtend = SciApi.SCI_HOMEDISPLAYEXTEND,

    /// <summary>
    /// Moves the caret to the start of the display or document line.
    /// </summary>
    HomeWrap = SciApi.SCI_HOMEWRAP,

    /// <summary>
    /// Extends the selection to the start of the display or document line.
    /// </summary>
    HomeWrapExtend = SciApi.SCI_HOMEWRAPEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the first non-whitespace character of the line.
    /// </summary>
    VcHome = SciApi.SCI_VCHOME,

    /// <summary>
    /// Extends the selection to the first non-whitespace character of the line.
    /// </summary>
    VcHomeExtend = SciApi.SCI_VCHOMEEXTEND,

    /// <summary>
    /// Extends the rectangular selection to the first non-whitespace character of the line.
    /// </summary>
    VcHomeRectExtend = SciApi.SCI_VCHOMERECTEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the first non-whitespace character of the display or document line.
    /// </summary>
    VcHomeWrap = SciApi.SCI_VCHOMEWRAP,

    /// <summary>
    /// Extends the selection to the first non-whitespace character of the display or document line.
    /// </summary>
    VcHomeWrapExtend = SciApi.SCI_VCHOMEWRAPEXTEND,

    /// <summary>
    /// Moves the caret to the first non-whitespace character of the display line.
    /// </summary>
    VcHomeDisplay = SciApi.SCI_VCHOMEDISPLAY,

    /// <summary>
    /// Extends the selection to the first non-whitespace character of the display line.
    /// </summary>
    VcHomeDisplayExtend = SciApi.SCI_VCHOMEDISPLAYEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the end of the document line.
    /// </summary>
    LineEnd = SciApi.SCI_LINEEND,

    /// <summary>
    /// Extends the selection to the end of the document line.
    /// </summary>
    LineEndExtend = SciApi.SCI_LINEENDEXTEND,

    /// <summary>
    /// Extends the rectangular selection to the end of the document line.
    /// </summary>
    LineEndRectExtend = SciApi.SCI_LINEENDRECTEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the end of the display line.
    /// </summary>
    LineEndDisplay = SciApi.SCI_LINEENDDISPLAY,

    /// <summary>
    /// Extends the selection to the end of the display line.
    /// </summary>
    LineEndDisplayExtend = SciApi.SCI_LINEENDDISPLAYEXTEND,

    /// <summary>
    /// Moves the caret to the end of the display or document line.
    /// </summary>
    LineEndWrap = SciApi.SCI_LINEENDWRAP,

    /// <summary>
    /// Extends the selection to the end of the display or document line.
    /// </summary>
    LineEndWrapExtend = SciApi.SCI_LINEENDWRAPEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret to the start of the document.
    /// </summary>
    DocumentStart = SciApi.SCI_DOCUMENTSTART,

    /// <summary>
    /// Extends the selection to the start of the document.
    /// </summary>
    DocumentStartExtend = SciApi.SCI_DOCUMENTSTARTEXTEND,

    /// <summary>
    /// Moves the caret to the end of the document.
    /// </summary>
    DocumentEnd = SciApi.SCI_DOCUMENTEND,

    /// <summary>
    /// Extends the selection to the end of the document.
    /// </summary>
    DocumentEndExtend = SciApi.SCI_DOCUMENTENDEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret up one page.
    /// </summary>
    PageUp = SciApi.SCI_PAGEUP,

    /// <summary>
    /// Extends the selection up one page.
    /// </summary>
    PageUpExtend = SciApi.SCI_PAGEUPEXTEND,

    /// <summary>
    /// Extends the rectangular selection up one page.
    /// </summary>
    PageUpRectExtend = SciApi.SCI_PAGEUPRECTEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret down one page.
    /// </summary>
    PageDown = SciApi.SCI_PAGEDOWN,

    /// <summary>
    /// Extends the selection down one page.
    /// </summary>
    PageDownExtend = SciApi.SCI_PAGEDOWNEXTEND,

    /// <summary>
    /// Extends the rectangular selection down one page.
    /// </summary>
    PageDownRectExtend = SciApi.SCI_PAGEDOWNRECTEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret up one window or page.
    /// </summary>
    StutteredPageUp = SciApi.SCI_STUTTEREDPAGEUP,

    /// <summary>
    /// Extends the selection up one window or page.
    /// </summary>
    StutteredPageUpExtend = SciApi.SCI_STUTTEREDPAGEUPEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the caret down one window or page.
    /// </summary>
    StutteredPageDown = SciApi.SCI_STUTTEREDPAGEDOWN,

    /// <summary>
    /// Extends the selection down one window or page.
    /// </summary>
    StutteredPageDownExtend = SciApi.SCI_STUTTEREDPAGEDOWNEXTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Deletes the character left of the caret.
    /// </summary>
    DeleteBack = SciApi.SCI_DELETEBACK,

    /// <summary>
    /// Deletes the character (excluding line breaks) left of the caret.
    /// </summary>
    DeleteBackNotLine = SciApi.SCI_DELETEBACKNOTLINE,

    // --------------------------------------------------------------------

    /// <summary>
    /// Deletes from the caret to the start of the previous word.
    /// </summary>
    DelWordLeft = SciApi.SCI_DELWORDLEFT,

    /// <summary>
    /// Deletes from the caret to the start of the next word.
    /// </summary>
    DelWordRight = SciApi.SCI_DELWORDRIGHT,

    /// <summary>
    /// Deletes from the caret to the end of the next word.
    /// </summary>
    DelWordRightEnd = SciApi.SCI_DELWORDRIGHTEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Deletes the characters left of the caret to the start of the line.
    /// </summary>
    DelLineLeft = SciApi.SCI_DELLINELEFT,

    /// <summary>
    /// Deletes the characters right of the caret to the start of the line.
    /// </summary>
    DelLineRight = SciApi.SCI_DELLINERIGHT,

    /// <summary>
    /// Deletes the current line.
    /// </summary>
    LineDelete = SciApi.SCI_LINEDELETE,

    // --------------------------------------------------------------------

    /// <summary>
    /// Removes the current line and places it on the clipboard.
    /// </summary>
    LineCut = SciApi.SCI_LINECUT,

    /// <summary>
    /// Copies the current line and places it on the clipboard.
    /// </summary>
    LineCopy = SciApi.SCI_LINECOPY,

    /// <summary>
    /// Transposes the current and previous lines.
    /// </summary>
    LineTranspose = SciApi.SCI_LINETRANSPOSE,

    /// <summary>
    /// Reverses the current line.
    /// </summary>
    LineReverse = SciApi.SCI_LINEREVERSE,

    /// <summary>
    /// Duplicates the current line.
    /// </summary>
    LineDuplicate = SciApi.SCI_LINEDUPLICATE,

    // --------------------------------------------------------------------

    /// <summary>
    /// Converts the selection to lowercase.
    /// </summary>
    Lowercase = SciApi.SCI_LOWERCASE,

    /// <summary>
    /// Converts the selection to uppercase.
    /// </summary>
    Uppercase = SciApi.SCI_UPPERCASE,

    /// <summary>
    /// Cancels autocompletion, calltip display, and drops any additional selections.
    /// </summary>
    Cancel = SciApi.SCI_CANCEL,

    /// <summary>
    /// Toggles overtype. See <see cref="Scintilla.Overtype" />.
    /// </summary>
    EditToggleOvertype = SciApi.SCI_EDITTOGGLEOVERTYPE,

    // --------------------------------------------------------------------

    /// <summary>
    /// Inserts a newline character.
    /// </summary>
    NewLine = SciApi.SCI_NEWLINE,

    /// <summary>
    /// Inserts a form feed character.
    /// </summary>
    FormFeed = SciApi.SCI_FORMFEED,

    /// <summary>
    /// Adds a tab (indent) character.
    /// </summary>
    Tab = SciApi.SCI_TAB,

    /// <summary>
    /// Removes a tab (indent) character from the start of a line.
    /// </summary>
    BackTab = SciApi.SCI_BACKTAB,

    // --------------------------------------------------------------------

    /// <summary>
    /// Duplicates the current selection.
    /// </summary>
    SelectionDuplicate = SciApi.SCI_SELECTIONDUPLICATE,

    /// <summary>
    /// Moves the caret vertically to the center of the screen.
    /// </summary>
    VerticalCenterCaret = SciApi.SCI_VERTICALCENTRECARET,

    // --------------------------------------------------------------------

    /// <summary>
    /// Moves the selected lines up.
    /// </summary>
    MoveSelectedLinesUp = SciApi.SCI_MOVESELECTEDLINESUP,

    /// <summary>
    /// Moves the selected lines down.
    /// </summary>
    MoveSelectedLinesDown = SciApi.SCI_MOVESELECTEDLINESDOWN,

    // --------------------------------------------------------------------

    /// <summary>
    /// Scrolls to the start of the document without changing the selection.
    /// </summary>
    ScrollToStart = SciApi.SCI_SCROLLTOSTART,

    /// <summary>
    /// Scrolls to the end of the document without changing the selection.
    /// </summary>
    ScrollToEnd = SciApi.SCI_SCROLLTOEND,

    // --------------------------------------------------------------------

    /// <summary>
    /// Command equivalent to <see cref="Scintilla.ZoomIn" />.
    /// </summary>
    ZoomIn = SciApi.SCI_ZOOMIN,

    /// <summary>
    /// Command equivalent to <see cref="Scintilla.ZoomOut" />.
    /// </summary>
    ZoomOut = SciApi.SCI_ZOOMOUT,

    /// <summary>
    /// Command equivalent to <see cref="Scintilla.Undo" />.
    /// </summary>
    Undo = SciApi.SCI_UNDO,

    /// <summary>
    /// Command equivalent to <see cref="Scintilla.Redo" />.
    /// </summary>
    Redo = SciApi.SCI_REDO,

    /// <summary>
    /// Command equivalent to <see cref="Scintilla.SwapMainAnchorCaret" />
    /// </summary>
    SwapMainAnchorCaret = SciApi.SCI_SWAPMAINANCHORCARET,

    /// <summary>
    /// Command equivalent to <see cref="Scintilla.RotateSelection" />
    /// </summary>
    RotateSelection = SciApi.SCI_ROTATESELECTION,

    /// <summary>
    /// Command equivalent to <see cref="Scintilla.MultipleSelectAddNext" />
    /// </summary>
    MultipleSelectAddNext = SciApi.SCI_MULTIPLESELECTADDNEXT,

    /// <summary>
    /// Command equivalent to <see cref="Scintilla.MultipleSelectAddEach" />
    /// </summary>
    MultipleSelectAddEach = SciApi.SCI_MULTIPLESELECTADDEACH,

    /// <summary>
    /// Command equivalent to <see cref="Scintilla.SelectAll" />
    /// </summary>
    SelectAll = SciApi.SCI_SELECTALL
}
