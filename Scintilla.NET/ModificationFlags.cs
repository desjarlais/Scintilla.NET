using System;

namespace ScintillaNET;

/// <summary>
/// Type of modification and the action which caused the modification.
/// These are defined as a bit mask to make it easy to specify which notifications are wanted.
/// One bit is set from each of SC_MOD_* and SC_PERFORMED_*.
/// </summary>
[Flags]
public enum ModificationFlags : uint
{
    /// <summary>
    /// Base value with no fields valid. Will not occur but is useful in tests.
    /// </summary>
    None = SciApi.SC_MOD_NONE,
    
    /// <summary>
    /// Text has been inserted into the document.
    /// </summary>
    InsertText = SciApi.SC_MOD_INSERTTEXT,
    
    /// <summary>
    /// Text has been removed from the document.
    /// </summary>
    DeleteText = SciApi.SC_MOD_DELETETEXT,
    
    /// <summary>
    /// A style change has occurred.
    /// </summary>
    ChangeStyle = SciApi.SC_MOD_CHANGESTYLE,
    
    /// <summary>
    /// A folding change has occurred.
    /// </summary>
    ChangeFold = SciApi.SC_MOD_CHANGEFOLD,

    /// <summary>
    /// Modification is the result of a user operation.
    /// </summary>
    User = SciApi.SC_PERFORMED_USER,

    /// <summary>
    /// Modification is the result of an undo operation.
    /// </summary>
    Undo = SciApi.SC_PERFORMED_UNDO,

    /// <summary>
    /// Modification is the result of a redo operation.
    /// </summary>
    Redo = SciApi.SC_PERFORMED_REDO,

    /// <summary>
    /// This is part of a multi-step Undo or Redo transaction.
    /// </summary>
    MultiStepUndoRedo = SciApi.SC_MULTISTEPUNDOREDO,
    
    /// <summary>
    /// This is the final step in an Undo or Redo transaction.
    /// </summary>
    LastStepInUndoRedo = SciApi.SC_LASTSTEPINUNDOREDO,
    
    /// <summary>
    /// One or more markers has changed in a line.
    /// </summary>
    ChangeMarker = SciApi.SC_MOD_CHANGEMARKER,
    
    /// <summary>
    /// Text is about to be inserted into the document.
    /// </summary>
    BeforeInsert = SciApi.SC_MOD_BEFOREINSERT,
    
    /// <summary>
    /// Text is about to be deleted from the document.
    /// </summary>
    BeforeDelete = SciApi.SC_MOD_BEFOREDELETE,
    
    /// <summary>
    /// An indicator has been added or removed from a range of text.
    /// </summary>
    MultiLineUndoRedo = SciApi.SC_MULTILINEUNDOREDO,

    /// <summary>
    /// A line state has changed because <see cref="SciApi.SCI_SETLINESTATE"/> was called.
    /// </summary>
    StartAction = SciApi.SC_STARTACTION,

    /// <summary>
    /// The explicit tab stops on a line have changed because <see cref="SciApi.SCI_CLEARTABSTOPS"/> or <see cref="SciApi.SCI_ADDTABSTOP"/> was called.
    /// </summary>
    ChangeIndicator = SciApi.SC_MOD_CHANGEINDICATOR,
    
    /// <summary>
    /// The internal state of a lexer has changed over a range.
    /// </summary>
    ChangeLineState = SciApi.SC_MOD_CHANGELINESTATE,
    
    /// <summary>
    /// A text margin has changed.
    /// </summary>
    ChangeMargin = SciApi.SC_MOD_CHANGEMARGIN,
    
    /// <summary>
    /// An annotation has changed.
    /// </summary>
    ChangeAnnotation = SciApi.SC_MOD_CHANGEANNOTATION,
    
    /// <summary>
    /// An EOL annotation has changed.
    /// </summary>
    Container = SciApi.SC_MOD_CONTAINER,

    /// <summary>
    /// Text is about to be inserted. The handler may change the text being inserted by calling <see cref="SciApi.SCI_CHANGEINSERTION"/>. No other modifications may be made in this handler.
    /// </summary>
    LexerState = SciApi.SC_MOD_LEXERSTATE,
    
    /// <summary>
    /// This is part of an Undo or Redo with multi-line changes.
    /// </summary>
    InsertCheck = SciApi.SC_MOD_INSERTCHECK,

    /// <summary>
    /// This is set on a <see cref="SciApi.SC_PERFORMED_USER"/> action when it is the first or only step in an undo transaction. This can be used to integrate the Scintilla undo stack with an undo stack in the container application by adding a Scintilla action to the container's stack for the currently opened container transaction or to open a new container transaction if there is no open container transaction.
    /// </summary>
    ChangeTabstops = SciApi.SC_MOD_CHANGETABSTOPS,

    /// <summary>
    /// This is set on for actions that the container stored into the undo stack with <see cref="SciApi.SCI_ADDUNDOACTION"/>.
    /// </summary>
    ChangeEolAnnotation = SciApi.SC_MOD_CHANGEEOLANNOTATION,

    /// <summary>
    /// This is a mask for all valid flags. This is the default mask state set by <see cref="SciApi.SCI_SETMODEVENTMASK"/>.
    /// </summary>
    EventMaskAll = SciApi.SC_MODEVENTMASKALL,
}
