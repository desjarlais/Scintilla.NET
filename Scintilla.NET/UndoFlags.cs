using System;

namespace ScintillaNET;

/// <summary>
/// Controls undo handling behavior in <see cref="SciApi.SCI_ADDUNDOACTION"/>.
/// </summary>
[Flags]
public enum UndoFlags : uint
{
    /// <summary>
    /// No special handling of undo actions.
    /// </summary>
    None = SciApi.UNDO_NONE,

    /// <summary>
    /// Indicates the container action may be coalesced along with any insertion and deletion actions into a single compound action.
    /// Coalescing treats coalescible container actions as transparent so will still only group together insertions that look like typing or deletions that look like multiple uses of the Backspace or Delete keys.
    /// </summary>
    MayCoalesce = SciApi.UNDO_MAY_COALESCE,
}
