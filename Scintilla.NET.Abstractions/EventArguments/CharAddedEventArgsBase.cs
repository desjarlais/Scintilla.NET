namespace Scintilla.NET.Abstractions.EventArguments;

/// <summary>
/// Provides data for the <see cref="Scintilla.CharAdded" /> event.
/// </summary>
public abstract class CharAddedEventArgsBase : EventArgs
{
    /// <summary>
    /// Gets the text character added to a <see cref="Scintilla" /> control.
    /// </summary>
    /// <returns>The character added.</returns>
    public virtual int Char { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CharAddedEventArgsBase" /> class.
    /// </summary>
    /// <param name="ch">The character added.</param>
    protected CharAddedEventArgsBase(int ch)
    {
        Char = ch;
    }
}