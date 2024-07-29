using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScintillaNET;

/// <summary>
/// Helper methods for the <see cref="Scintilla"/> control.
/// </summary>
public static class HelperMethods
{
    private static readonly Dictionary<int, Color> knownColorMap = [];

    static HelperMethods()
    {
        foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)).Cast<KnownColor>().Where(k => k is >= KnownColor.Transparent and < KnownColor.ButtonFace))
        {
            var color = Color.FromKnownColor(knownColor);
            knownColorMap[ToWin32Color(color)] = color;
        }
    }

    /// <summary>
    /// Converts an ABGR WinAPI color to <see cref="Color"/>.
    /// </summary>
    /// <param name="color">The color value to convert.</param>
    /// <returns>A <see cref="Color"/> equivalent of the ABGR WinAPI color.</returns>
    public static Color FromWin32Color(int color)
    {
        if ((color & 0xFF_00_00_00) == 0)
            return Color.Transparent;

        if (knownColorMap.TryGetValue(color, out Color result))
            // We do all this nonsense because Visual Studio designer does not
            // mark raw colors as default if there exists a known color
            // with the same value.
            return result;

        return Color.FromArgb((color >> 24) & 0xFF, (color >> 0) & 0xFF, (color >> 8) & 0xFF, (color >> 16) & 0xFF);
    }

    /// <summary>
    /// Converts a <see cref="Color"/> to ABGR WinAPI color.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> instance to convert.</param>
    /// <returns>ABGR WinAPI color value of the <see cref="Color"/> instance.</returns>
    public static int ToWin32Color(Color color)
    {
        return (color.A << 24) | (color.R << 0) | (color.G << 8) | (color.B << 16);
    }

    /// <summary>
    /// Converts an ABGR WinAPI color to <see cref="Color"/> while ignoring the alpha channel.
    /// </summary>
    /// <param name="color">The color value to convert.</param>
    /// <returns>A <see cref="Color"/> equivalent of the ABGR WinAPI color with alpha channel value set to max (opaque).</returns>
    public static Color FromWin32ColorOpaque(int color)
    {
        color |= unchecked((int)0xFF_00_00_00);

        if (knownColorMap.TryGetValue(color, out Color result))
            // We do all this nonsense because Visual Studio designer does not
            // mark raw colors as default if there exists a known color
            // with the same value.
            return result;

        return Color.FromArgb((color >> 0) & 0xFF, (color >> 8) & 0xFF, (color >> 16) & 0xFF);
    }

    /// <summary>
    /// Converts a <see cref="Color"/> to ABGR WinAPI color while ignoring the alpha channel.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> instance to convert.</param>
    /// <returns>ABGR WinAPI color value of the <see cref="Color"/> instance with alpha channel value set to max (opaque).</returns>
    public static int ToWin32ColorOpaque(Color color)
    {
        return (0xFF << 24) | (color.R << 0) | (color.G << 8) | (color.B << 16);
    }

    /// <summary>
    /// Gets the folding state of the control as a delimited string containing line indexes.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla"/> control instance.</param>
    /// <param name="separator">The string to use as a separator.</param>
    /// <returns>The folding state of the control.</returns>
    public static string GetFoldingState(this Scintilla scintilla, string separator = ";")
    {
        return string.Join(separator,
            scintilla.Lines.Where(f => !f.Expanded).Select(f => f.Index).OrderBy(f => f).ToArray());
    }

    /// <summary>
    /// Sets the folding state of the state of the control with specified index string.
    /// </summary>
    /// <param name="scintilla">The <see cref="Scintilla"/> control instance.</param>
    /// <param name="foldingState">A string containing the folded line indexes separated with the <paramref name="separator"/> to restore the folding.</param>
    /// <param name="separator">The string to use as a separator.</param>
    public static void SetFoldingState(this Scintilla scintilla, string foldingState, string separator = ";")
    {
        scintilla.FoldAll(FoldAction.Expand);
        foreach (int index in foldingState.Split(new[] { separator }, System.StringSplitOptions.None).Select(int.Parse))
        {
            if (index < 0 || index >= scintilla.Lines.Count)
            {
                continue;
            }

            scintilla.Lines[index].ToggleFold();
        }
    }
}
