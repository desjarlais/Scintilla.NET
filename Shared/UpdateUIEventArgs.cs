using Scintilla.NET.Abstractions.Enumerations;
using Scintilla.NET.Abstractions.EventArguments;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.UpdateUI" /> event.
/// </summary>
// ReSharper disable once InconsistentNaming, part of the API
public class UpdateUIEventArgs : UpdateUIEventArgsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUIEventArgs" /> class.
    /// </summary>
    /// <param name="change">A bitwise combination of <see cref="UpdateChange" /> values specifying the reason to update the UI.</param>
    public UpdateUIEventArgs(UpdateChange change) : base(change)
    {
    }
}