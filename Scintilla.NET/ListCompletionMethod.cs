namespace ScintillaNET;

/// <summary>
/// Indicates how an autocompletion occurred.
/// </summary>
public enum ListCompletionMethod
{
    /// <summary>
    /// A fillup character (see <see cref="Scintilla.AutoCSetFillUps" />) triggered the completion.
    /// The character used is indicated by the <see cref="AutoCSelectionEventArgs.Char" /> property.
    /// </summary>
    FillUp = SciApi.SC_AC_FILLUP,

    /// <summary>
    /// A double-click triggered the completion.
    /// </summary>
    DoubleClick = SciApi.SC_AC_DOUBLECLICK,

    /// <summary>
    /// A tab key or the <see cref="ScintillaNET.Command.Tab" /> command triggered the completion.
    /// </summary>
    Tab = SciApi.SC_AC_TAB,

    /// <summary>
    /// A new line or <see cref="ScintillaNET.Command.NewLine" /> command triggered the completion.
    /// </summary>
    NewLine = SciApi.SC_AC_NEWLINE,

    /// <summary>
    /// The <see cref="Scintilla.AutoCSelect" /> method triggered the completion.
    /// </summary>
    Command = SciApi.SC_AC_COMMAND,

    /// <summary>
    /// There was only a single choice in the list and 'choose single' mode was active as set by <see cref="SciApi.SCI_AUTOCSETCHOOSESINGLE"/>. <see cref="SciApi.SCNotification.ch"/> is 0.
    /// </summary>
    SingleChoice = SciApi.SC_AC_SINGLE_CHOICE,
}
