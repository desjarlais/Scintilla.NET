using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.CharAdded" /> event.
/// </summary>
public class CharAddedEventArgs : CharAddedEventArgsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CharAddedEventArgs" /> class.
    /// </summary>
    /// <param name="ch">The character added.</param>
    public CharAddedEventArgs(int ch) : base(ch)
    {
    }
}