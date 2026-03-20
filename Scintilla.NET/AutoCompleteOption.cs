namespace ScintillaNET;

/// <summary>
/// Options for autocompletion.
/// </summary>
public enum AutoCompleteOption
{
    /// <summary>
    /// Display autocompletion using default settings.
    /// </summary>
    Normal = SciApi.SC_AUTOCOMPLETE_NORMAL,

    /// <summary>
    /// On Win32 only, use a fixed size list instead of one that can be resized by the user. This also avoids a header rectangle above the list.
    /// </summary>
    FixedSize = SciApi.SC_AUTOCOMPLETE_FIXED_SIZE,

    /// <summary>
    /// Always select the first item from the autocompletion list regardless of the value entered in the editor. Useful when the autocompletion logic of the application sorts autocompletion entries so that the best match is always at the top of the list. Without this option, Scintilla selects the item from the autocompletion list matching the value entered in the editor.
    /// </summary>
    SelectFirstItem = SciApi.SC_AUTOCOMPLETE_SELECT_FIRST_ITEM,
}
