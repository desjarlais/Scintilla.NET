namespace ScintillaNET;

/// <summary>
/// Fold actions.
/// </summary>
public enum FoldAction
{
    /// <summary>
    /// Contract the fold.
    /// </summary>
    Contract = SciApi.SC_FOLDACTION_CONTRACT,

    /// <summary>
    /// Expand the fold.
    /// </summary>
    Expand = SciApi.SC_FOLDACTION_EXPAND,

    /// <summary>
    /// Toggle between contracted and expanded.
    /// </summary>
    Toggle = SciApi.SC_FOLDACTION_TOGGLE,

    /// <summary>
    /// Used for <see cref="Scintilla.FoldAll"/> only, can be combined with <see cref="Contract"/> or <see cref="Toggle"/> to contract all levels instead of only top-level.
    /// </summary>
    ContractEveryLevel = SciApi.SC_FOLDACTION_CONTRACT_EVERY_LEVEL,
}
