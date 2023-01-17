namespace Scintilla.NET.Abstractions.EventArguments;

/// <summary>
/// Provides data for the <see cref="Scintilla.ChangeAnnotation" /> event.
/// </summary>
public abstract class ChangeAnnotationEventArgsBase : EventArgs
{
    /// <summary>
    /// Gets the line index where the annotation changed.
    /// </summary>
    /// <returns>The zero-based line index where the annotation change occurred.</returns>
    public virtual int Line { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeAnnotationEventArgsBase" /> class.
    /// </summary>
    /// <param name="line">The zero-based line index of the annotation that changed.</param>
    protected ChangeAnnotationEventArgsBase(int line)
    {
        Line = line;
    }
}