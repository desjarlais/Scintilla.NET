using System;
using System.Drawing;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.ChangeAnnotation" /> event.
/// </summary>
public class ChangeAnnotationEventArgs : ChangeAnnotationEventArgsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeAnnotationEventArgs" /> class.
    /// </summary>
    /// <param name="line">The zero-based line index of the annotation that changed.</param>
    public ChangeAnnotationEventArgs(int line) : base(line)
    {
    }
}