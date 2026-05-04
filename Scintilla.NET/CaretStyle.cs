using System;

namespace ScintillaNET;

/// <summary>
/// The caret visual style.
/// </summary>
[Flags]
public enum CaretStyle : uint
{
    /// <summary>
    /// Carets are not drawn at all.
    /// </summary>
    Invisible = SciApi.CARETSTYLE_INVISIBLE,

    /// <summary>
    /// Draws insertion carets as lines. This is the default for insert mode.
    /// </summary>
    Line = SciApi.CARETSTYLE_LINE,

    /// <summary>
    /// Draws insertion carets as blocks.
    /// </summary>
    Block = SciApi.CARETSTYLE_BLOCK,

    /// <summary>
    /// Bit mask for styles used in insert mode.
    /// </summary>
    InsertMask = SciApi.CARETSTYLE_INS_MASK,

    /// <summary>
    /// Draws an overstrike caret as a bar. This is the default for overtype mode.
    /// </summary>
    OverstrikeBar = SciApi.CARETSTYLE_OVERSTRIKE_BAR,

    /// <summary>
    /// Draws an overstrike caret as a block. This should be ored with one of the first three styles.
    /// </summary>
    OverstrikeBlock = SciApi.CARETSTYLE_OVERSTRIKE_BLOCK,

    /// <summary>
    /// Draws carets that cannot be drawn in a curses (terminal) environment (such as additional carets), and draws them as blocks. The main caret is left to be drawn by the terminal itself. This setting is typically a standalone setting.
    /// </summary>
    Curses = SciApi.CARETSTYLE_CURSES,

    /// <summary>
    /// When the caret end of a range is at the end and a block caret style is chosen, draws the block outside the selection instead of inside. This can be ored with <see cref="SciApi.CARETSTYLE_BLOCK"/> or <see cref="SciApi.CARETSTYLE_CURSES"/>.
    /// </summary>
    BlockAfter = SciApi.CARETSTYLE_BLOCK_AFTER,
}
