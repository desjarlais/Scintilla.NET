using System;

namespace ScintillaNET;

/// <summary>
/// Provides data for the <see cref="Scintilla.UpdateUI" /> event.
/// </summary>
public class UpdateUIEventArgs : EventArgs
{
    private readonly Scintilla scintilla;
    private readonly int bytePosition;
    private int? position;

    #region Properties

    /// <summary>
    /// The UI update that occurred.
    /// </summary>
    /// <returns>A bitwise combination of <see cref="UpdateChange" /> values specifying the UI update that occurred.</returns>
    public UpdateChange Change { get; private set; }

    /// <summary>
    /// Gets the zero-based document start position where the Text was changed.
    /// </summary>
    /// <returns>The zero-based document start position where the Text was changed.</returns>
    public int Position
    {
        get
        {
            if (this.bytePosition == Scintilla.InvalidPosition || this.scintilla is null)
            {
                return Scintilla.InvalidPosition;
            }

            this.position ??= this.scintilla.Lines.ByteToCharPosition(this.bytePosition);

            return (int)this.position;
        }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUIEventArgs" /> class.
    /// </summary>
    /// <param name="change">A bitwise combination of <see cref="UpdateChange" /> values specifying the reason to update the UI.</param>
    public UpdateUIEventArgs(UpdateChange change)
        : this(null, change, Scintilla.InvalidPosition)
    {
    }

    internal UpdateUIEventArgs(Scintilla scintilla, UpdateChange change, int bytePosition)
    {
        this.scintilla = scintilla;
        this.bytePosition = bytePosition;
        Change = change;
    }

    #endregion Constructors
}
