namespace ScintillaNET;

/// <summary>
/// Visibility and location of annotations in a <see cref="Scintilla" /> control
/// </summary>
public enum Annotation
{
    /// <summary>
    /// Annotations are not displayed. This is the default.
    /// </summary>
    Hidden = SciApi.ANNOTATION_HIDDEN,

    /// <summary>
    /// Annotations are drawn left justified with no adornment.
    /// </summary>
    Standard = SciApi.ANNOTATION_STANDARD,

    /// <summary>
    /// Annotations are indented to match the text and are surrounded by a box.
    /// </summary>
    Boxed = SciApi.ANNOTATION_BOXED,

    /// <summary>
    /// Annotations are indented to match the text.
    /// </summary>
    Indented = SciApi.ANNOTATION_INDENTED,
}
