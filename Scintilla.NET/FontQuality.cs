namespace ScintillaNET;

/// <summary>
/// The font quality (antialiasing method) used to render text.
/// </summary>
public enum FontQuality : uint
{
    /// <summary>
    /// Specifies that the character quality of the font does not matter; so the lowest quality can be used.
    /// This is the default.
    /// </summary>
    Default = SciApi.SC_EFF_QUALITY_DEFAULT,

    /// <summary>
    /// Specifies that anti-aliasing should not be used when rendering text.
    /// </summary>
    NonAntiAliased = SciApi.SC_EFF_QUALITY_NON_ANTIALIASED,

    /// <summary>
    /// Specifies that anti-aliasing should be used when rendering text, if the font supports it.
    /// </summary>
    AntiAliased = SciApi.SC_EFF_QUALITY_ANTIALIASED,

    /// <summary>
    /// Specifies that ClearType anti-aliasing should be used when rendering text, if the font supports it.
    /// </summary>
    LcdOptimized = SciApi.SC_EFF_QUALITY_LCD_OPTIMIZED,

    /// <summary>
    /// In case it is necessary to squeeze more options into this enum, only a limited number of bits defined by <see cref="Mask"/> will be used for quality.
    /// </summary>
    Mask = SciApi.SC_EFF_QUALITY_MASK,
}
