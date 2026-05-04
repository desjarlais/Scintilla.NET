using System;

namespace ScintillaNET;

/// <summary>
/// Different shapes available for EOL annotations.
/// Different shapes depend on drawing API so <see cref="SciApi.SC_TECHNOLOGY_DEFAULT"/> on Win32 only draws a rectangle.
/// Only one shape can currently be used for the whole document
/// </summary>
[Flags]
public enum EOLAnnotationVisible : uint
{
    /// <summary>
    /// End of Line Annotations are not displayed.
    /// </summary>
    Hidden = SciApi.EOLANNOTATION_HIDDEN,

    /// <summary>
    /// End of Line Annotations are drawn left justified with no adornment.
    /// </summary>
    Standard = SciApi.EOLANNOTATION_STANDARD,

    /// <summary>
    /// End of Line Annotations are indented to match the text and are surrounded by a box.
    /// </summary>
    Boxed = SciApi.EOLANNOTATION_BOXED,

    /// <summary>
    /// Surround with a ◖stadium◗ - a rectangle with rounded ends.
    /// </summary>
    Stadium = SciApi.EOLANNOTATION_STADIUM,

    /// <summary>
    /// Surround with a |shape◗ with flat left end and curved right end.
    /// </summary>
    FlatCircle = SciApi.EOLANNOTATION_FLAT_CIRCLE,

    /// <summary>
    /// Surround with a ◄shape◗ with angled left end and curved right end.
    /// </summary>
    AngleCircle = SciApi.EOLANNOTATION_ANGLE_CIRCLE,

    /// <summary>
    /// Surround with a ◖shape| with curved left end and flat right end.
    /// </summary>
    CircleFlat = SciApi.EOLANNOTATION_CIRCLE_FLAT,

    /// <summary>
    /// Surround with a |shape| with flat ends.
    /// </summary>
    Flats = SciApi.EOLANNOTATION_FLATS,

    /// <summary>
    /// Surround with a ◄shape| with angled left end and flat right end.
    /// </summary>
    AngleFlat = SciApi.EOLANNOTATION_ANGLE_FLAT,

    /// <summary>
    /// Surround with a ◖shape▶ with curved left end and angled right end.
    /// </summary>
    CircleAngle = SciApi.EOLANNOTATION_CIRCLE_ANGLE,

    /// <summary>
    /// Surround with a |shape▶ with flat left end and angled right end.
    /// </summary>
    FlatAngle = SciApi.EOLANNOTATION_FLAT_ANGLE,

    /// <summary>
    /// Surround with a ◄shape▶ with angles on each end.
    /// </summary>
    Angles = SciApi.EOLANNOTATION_ANGLES,
}
