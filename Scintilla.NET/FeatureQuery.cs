namespace ScintillaNET;

/// <summary>
/// Features that can be queried using <see cref="SciApi.SCI_SUPPORTSFEATURE"/>.
/// </summary>
public enum FeatureQuery
{
    /// <summary>
    /// Whether drawing a line draws its final position.
    /// Only false on Win32 GDI.
    /// </summary>
    LineDrawsFinal = SciApi.SC_SUPPORTS_LINE_DRAWS_FINAL,

    /// <summary>
    /// Are logical pixels larger than physical pixels?
    /// Currently only true for macOS Cocoa with 'retina' displays.
    /// When true, creating pixmaps at twice the resolution can produce clearer output with less blur.
    /// </summary>
    PixelDivisions = SciApi.SC_SUPPORTS_PIXEL_DIVISIONS,

    /// <summary>
    /// Can lines be drawn with fractional widths like 1.5 or 0.5 pixels?
    /// </summary>
    FractionalStrokeWidth = SciApi.SC_SUPPORTS_FRACTIONAL_STROKE_WIDTH,
    
    /// <summary>
    /// Can translucent lines, polygons, ellipses, and text be drawn?
    /// </summary>
    TranslucentStroke = SciApi.SC_SUPPORTS_TRANSLUCENT_STROKE,
    
    /// <summary>
    /// Can individual pixels be modified? This is false for character cell platforms like curses.
    /// </summary>
    PixelModification = SciApi.SC_SUPPORTS_PIXEL_MODIFICATION,
    
    /// <summary>
    /// Can text measurement be safely performed concurrently on multiple threads?
    /// Currently only true for macOS Cocoa, DirectWrite on Win32, and GTK on X or Wayland. 
    /// </summary>
    ThreadSafeMeasureWidths = SciApi.SC_SUPPORTS_THREAD_SAFE_MEASURE_WIDTHS,
}
