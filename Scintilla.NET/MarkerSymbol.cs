namespace ScintillaNET;

/// <summary>
/// The symbol displayed by a <see cref="Marker" />
/// </summary>
public enum MarkerSymbol
{
    /// <summary>
    /// A circle. This symbol is typically used to indicate a breakpoint.
    /// </summary>
    Circle = SciApi.SC_MARK_CIRCLE,

    /// <summary>
    /// A rectangel with rounded edges.
    /// </summary>
    RoundRect = SciApi.SC_MARK_ROUNDRECT,

    /// <summary>
    /// An arrow (triangle) pointing right.
    /// </summary>
    Arrow = SciApi.SC_MARK_ARROW,

    /// <summary>
    /// A rectangle that is wider than it is tall.
    /// </summary>
    SmallRect = SciApi.SC_MARK_SMALLRECT,

    /// <summary>
    /// An arrow and tail pointing right. This symbol is typically used to indicate the current line of execution.
    /// </summary>
    ShortArrow = SciApi.SC_MARK_SHORTARROW,

    /// <summary>
    /// An invisible symbol useful for tracking the movement of lines.
    /// </summary>
    Empty = SciApi.SC_MARK_EMPTY,

    /// <summary>
    /// An arrow (triangle) pointing down.
    /// </summary>
    ArrowDown = SciApi.SC_MARK_ARROWDOWN,

    /// <summary>
    /// A minus (-) symbol.
    /// </summary>
    Minus = SciApi.SC_MARK_MINUS,

    /// <summary>
    /// A plus (+) symbol.
    /// </summary>
    Plus = SciApi.SC_MARK_PLUS,

    /// <summary>
    /// A thin vertical line. This symbol is typically used on the middle line of an expanded fold block.
    /// </summary>
    VLine = SciApi.SC_MARK_VLINE,

    /// <summary>
    /// A thin 'L' shaped line. This symbol is typically used on the last line of an expanded fold block.
    /// </summary>
    LCorner = SciApi.SC_MARK_LCORNER,

    /// <summary>
    /// A thin 't' shaped line. This symbol is typically used on the last line of an expanded nested fold block.
    /// </summary>
    TCorner = SciApi.SC_MARK_TCORNER,

    /// <summary>
    /// A plus (+) symbol with surrounding box. This symbol is typically used on the first line of a collapsed fold block.
    /// </summary>
    BoxPlus = SciApi.SC_MARK_BOXPLUS,

    /// <summary>
    /// A plus (+) symbol with surrounding box and thin vertical line. This symbol is typically used on the first line of a collapsed nested fold block.
    /// </summary>
    BoxPlusConnected = SciApi.SC_MARK_BOXPLUSCONNECTED,

    /// <summary>
    /// A minus (-) symbol with surrounding box. This symbol is typically used on the first line of an expanded fold block.
    /// </summary>
    BoxMinus = SciApi.SC_MARK_BOXMINUS,

    /// <summary>
    /// A minus (-) symbol with surrounding box and thin vertical line. This symbol is typically used on the first line of an expanded nested fold block.
    /// </summary>
    BoxMinusConnected = SciApi.SC_MARK_BOXMINUSCONNECTED,

    /// <summary>
    /// Similar to a <see cref="LCorner" />, but curved.
    /// </summary>
    LCornerCurve = SciApi.SC_MARK_LCORNERCURVE,

    /// <summary>
    /// Similar to a <see cref="TCorner" />, but curved.
    /// </summary>
    TCornerCurve = SciApi.SC_MARK_TCORNERCURVE,

    /// <summary>
    /// Similar to a <see cref="BoxPlus" /> but surrounded by a circle.
    /// </summary>
    CirclePlus = SciApi.SC_MARK_CIRCLEPLUS,

    /// <summary>
    /// Similar to a <see cref="BoxPlusConnected" />, but surrounded by a circle.
    /// </summary>
    CirclePlusConnected = SciApi.SC_MARK_CIRCLEPLUSCONNECTED,

    /// <summary>
    /// Similar to a <see cref="BoxMinus" />, but surrounded by a circle.
    /// </summary>
    CircleMinus = SciApi.SC_MARK_CIRCLEMINUS,

    /// <summary>
    /// Similar to a <see cref="BoxMinusConnected" />, but surrounded by a circle.
    /// </summary>
    CircleMinusConnected = SciApi.SC_MARK_CIRCLEMINUSCONNECTED,

    /// <summary>
    /// A special marker that displays no symbol but will affect the background color of the line.
    /// </summary>
    Background = SciApi.SC_MARK_BACKGROUND,

    /// <summary>
    /// Three dots (ellipsis).
    /// </summary>
    DotDotDot = SciApi.SC_MARK_DOTDOTDOT,

    /// <summary>
    /// Three bracket style arrows.
    /// </summary>
    Arrows = SciApi.SC_MARK_ARROWS,

    // PixMap = SciApi.SC_MARK_PIXMAP,

    /// <summary>
    /// A rectangle occupying the entire marker space.
    /// </summary>
    FullRect = SciApi.SC_MARK_FULLRECT,

    /// <summary>
    /// A rectangle occupying only the left edge of the marker space.
    /// </summary>
    LeftRect = SciApi.SC_MARK_LEFTRECT,

    /// <summary>
    /// A special marker left available to plugins.
    /// </summary>
    Available = SciApi.SC_MARK_AVAILABLE,

    /// <summary>
    /// A special marker that displays no symbol but will underline the current line text.
    /// </summary>
    Underline = SciApi.SC_MARK_UNDERLINE,

    /// <summary>
    /// A user-defined image. Images can be set using the <see cref="Marker.DefineRgbaImage" /> method.
    /// </summary>
    RgbaImage = SciApi.SC_MARK_RGBAIMAGE,

    /// <summary>
    /// A left-rotated bookmark.
    /// </summary>
    Bookmark = SciApi.SC_MARK_BOOKMARK,

    /// <summary>
    /// A bookmark.
    /// </summary>
    VerticalBookmark = SciApi.SC_MARK_VERTICALBOOKMARK,

    /// <summary>
    /// A slim  rectangular vertical bar.
    /// </summary>
    Bar = SciApi.SC_MARK_BAR,

    // Character = SciApi.SC_MARK_CHARACTER
}
