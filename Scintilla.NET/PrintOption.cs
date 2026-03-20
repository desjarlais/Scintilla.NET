namespace ScintillaNET;

/// <summary>
/// The method used to render coloured text on a printer that is probably using white paper.
/// It is especially important to consider the treatment of colour if you use a dark or black screen background. Printing white on black uses up toner and ink very many times faster than the other way around.
/// </summary>
public enum PrintOption
{
    /// <summary>
    /// Print using the current screen colours with the exception of line number margins which print on a white background. This is the default.
    /// </summary>
    Normal = SciApi.SC_PRINT_NORMAL,
    
    /// <summary>
    /// If you use a dark screen background this saves ink by inverting the light value of all colours and printing on a white background.
    /// </summary>
    InvertLight = SciApi.SC_PRINT_INVERTLIGHT,
    
    /// <summary>
    /// Print all text as black on a white background.
    /// </summary>
    BlackOnWhite = SciApi.SC_PRINT_BLACKONWHITE,
    
    /// <summary>
    /// Everything prints in its own colour on a white background.
    /// </summary>
    ColourOnWhite = SciApi.SC_PRINT_COLOURONWHITE,
    
    /// <summary>
    /// Everything prints in its own foreground colour but all styles up to and including <see cref="Style.LineNumber"/> will print on a white background.
    /// </summary>
    ColourOnWhiteDefaultBg = SciApi.SC_PRINT_COLOURONWHITEDEFAULTBG,
    
    /// <summary>
    /// Print using the current screen colours for both foreground and background. This is the only mode that does not set the background colour of the line number margin to white.
    /// </summary>
    ScreenColours = SciApi.SC_PRINT_SCREENCOLOURS,
}
