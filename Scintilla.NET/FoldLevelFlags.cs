using System;

namespace ScintillaNET;

/// <summary>
/// Flags for additional line fold level behavior.
/// </summary>
[Flags]
public enum FoldLevelFlags : uint
{
    /// <summary>
    /// Initial level that may occur before folding.
    /// </summary>
    None = SciApi.SC_FOLDLEVELNONE,

    /// <summary>
    /// Default fold level. This is bigger than <see cref="None"/> (0x400) to allow unsigned arithmetic.
    /// </summary>
    Base = SciApi.SC_FOLDLEVELBASE,

    /// <summary>
    /// Indicates that the line is blank and should be treated slightly different than its level may indicate;
    /// otherwise, blank lines should generally not be fold points.
    /// </summary>
    White = SciApi.SC_FOLDLEVELWHITEFLAG,

    /// <summary>
    /// Indicates that the line is a header (fold point).
    /// </summary>
    Header = SciApi.SC_FOLDLEVELHEADERFLAG,

    /// <summary>
    /// The fold level is a number in the range 0 to <see cref="SciApi.SC_FOLDLEVELNUMBERMASK"/>.
    /// </summary>
    NumberMask = SciApi.SC_FOLDLEVELNUMBERMASK,
}
