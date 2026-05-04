namespace ScintillaNET;

/// <summary>
/// The stretch of a font can be set with <see cref="SciApi.SCI_STYLESETSTRETCH"/>.
/// Condensed text can be used to display more text in a narrower window and expanded text may be used for clearer text that is easier to read.
/// The API is based on the Cascading Style Sheets font-stretch property.
/// The best supported and useful values are <see cref="FontStretch.Condensed"/>, <see cref="FontStretch.Normal"/>, and <see cref="FontStretch.Expanded"/>.
/// </summary>
public enum FontStretch
{
    /// <summary>
    /// 50%
    /// </summary>
    UltraCondensed = SciApi.SC_STRETCH_ULTRA_CONDENSED,
    
    /// <summary>
    /// 62.5%
    /// </summary>
    ExtraCondensed = SciApi.SC_STRETCH_EXTRA_CONDENSED,
    
    /// <summary>
    /// 75%
    /// </summary>
    Condensed = SciApi.SC_STRETCH_CONDENSED,
    
    /// <summary>
    /// 87.5%
    /// </summary>
    SemiCondensed = SciApi.SC_STRETCH_SEMI_CONDENSED,
    
    /// <summary>
    /// 100%
    /// </summary>
    Normal = SciApi.SC_STRETCH_NORMAL,
    
    /// <summary>
    /// 112.5%
    /// </summary>
    SemiExpanded = SciApi.SC_STRETCH_SEMI_EXPANDED,
    
    /// <summary>
    /// 125%
    /// </summary>
    Expanded = SciApi.SC_STRETCH_EXPANDED,
    
    /// <summary>
    /// 150%
    /// </summary>
    ExtraExpanded = SciApi.SC_STRETCH_EXTRA_EXPANDED,
    
    /// <summary>
    /// 200%
    /// </summary>
    UltraExpanded = SciApi.SC_STRETCH_ULTRA_EXPANDED,
}
