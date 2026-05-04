namespace ScintillaNET;

/// <summary>
/// The visual appearance of an indicator.
/// </summary>
public enum IndicatorStyle
{
    /// <summary>
    /// Underlined with a single, straight line.
    /// </summary>
    Plain = SciApi.INDIC_PLAIN,

    /// <summary>
    /// A squiggly underline. Requires 3 pixels of descender space.
    /// </summary>
    Squiggle = SciApi.INDIC_SQUIGGLE,

    /// <summary>
    /// A line of small T shapes.
    /// </summary>
    TT = SciApi.INDIC_TT,

    /// <summary>
    /// Diagonal hatching.
    /// </summary>
    Diagonal = SciApi.INDIC_DIAGONAL,

    /// <summary>
    /// Strike out.
    /// </summary>
    Strike = SciApi.INDIC_STRIKE,

    /// <summary>
    /// An indicator with no visual effect.
    /// </summary>
    Hidden = SciApi.INDIC_HIDDEN,

    /// <summary>
    /// A rectangle around the text.
    /// </summary>
    Box = SciApi.INDIC_BOX,

    /// <summary>
    /// A rectangle around the text with rounded corners. The rectangle outline and fill transparencies can be adjusted using
    /// <see cref="Indicator.Alpha" /> and <see cref="Indicator.OutlineAlpha" />.
    /// </summary>
    RoundBox = SciApi.INDIC_ROUNDBOX,

    /// <summary>
    /// A rectangle around the text. The rectangle outline and fill transparencies can be adjusted using
    /// <see cref="Indicator.Alpha" /> and <see cref="Indicator.OutlineAlpha"/>.
    /// </summary>
    StraightBox = SciApi.INDIC_STRAIGHTBOX,

    /// <summary>
    /// A dashed underline.
    /// </summary>
    Dash = SciApi.INDIC_DASH,

    /// <summary>
    /// A dotted underline.
    /// </summary>
    Dots = SciApi.INDIC_DOTS,

    /// <summary>
    /// Similar to <see cref="Squiggle" /> but only using 2 vertical pixels so will fit under small fonts.
    /// </summary>
    SquiggleLow = SciApi.INDIC_SQUIGGLELOW,

    /// <summary>
    /// A dotted rectangle around the text. The dots transparencies can be adjusted using
    /// <see cref="Indicator.Alpha" /> and <see cref="Indicator.OutlineAlpha" />.
    /// </summary>
    DotBox = SciApi.INDIC_DOTBOX,

    /// <summary>
    /// A version of Squiggle that draws using a pixmap instead of as a series of line segments for performance.
    /// </summary>
    SquigglePixmap = SciApi.INDIC_SQUIGGLEPIXMAP,

    // PIXMAP

    /// <summary>
    /// A 2-pixel thick underline with 1 pixel insets on either side.
    /// </summary>
    CompositionThick = SciApi.INDIC_COMPOSITIONTHICK,

    /// <summary>
    /// A 1-pixel thick underline with 1 pixel insets on either side.
    /// </summary>
    CompositionThin = SciApi.INDIC_COMPOSITIONTHIN,

    /// <summary>
    /// A rectangle around the entire character area. The rectangle outline and fill transparencies can be adjusted using
    /// <see cref="Indicator.Alpha" /> and <see cref="Indicator.OutlineAlpha"/>.
    /// </summary>
    FullBox = SciApi.INDIC_FULLBOX,

    /// <summary>
    /// An indicator that will change the foreground color of text to the foreground color of the indicator.
    /// </summary>
    TextFore = SciApi.INDIC_TEXTFORE,

    /// <summary>
    /// A triangle below the start of the indicator range.
    /// </summary>
    Point = SciApi.INDIC_POINT,

    /// <summary>
    /// A triangle below the center of the first character of the indicator range.
    /// </summary>
    PointCharacter = SciApi.INDIC_POINTCHARACTER,

    /// <summary>
    /// A vertical gradient between a color and alpha at top to fully transparent at bottom.
    /// </summary>
    Gradient = SciApi.INDIC_GRADIENT,

    /// <summary>
    /// A vertical gradient with color and alpha in the mid-line fading to fully transparent at top and bottom.
    /// </summary>
    GradientCenter = SciApi.INDIC_GRADIENTCENTRE,

    /// <summary>
    /// A triangle above the start of the indicator range.
    /// </summary>
    PointTop = SciApi.INDIC_POINT_TOP,
}
