namespace ScintillaNET;

/// <summary>
/// Controls how the selection history is saved for each action, which can then be restored when an undo/redo is performed.
/// </summary>
public enum UndoSelectionHistoryOption
{
    /// <summary>
    /// The default: Undo selection history turned off.
    /// </summary>
    Disabled = SciApi.SC_UNDO_SELECTION_HISTORY_DISABLED,
    
    /// <summary>
    /// Restore selection for each undo and redo.
    /// </summary>
    Enabled = SciApi.SC_UNDO_SELECTION_HISTORY_ENABLED,
    
    /// <summary>
    /// Restore vertical scroll position. Has no effect without SC_UNDO_SELECTION_HISTORY_ENABLED.
    /// </summary>
    Scroll = SciApi.SC_UNDO_SELECTION_HISTORY_SCROLL,
}
