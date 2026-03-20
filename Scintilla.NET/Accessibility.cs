namespace ScintillaNET;

/// <summary>
/// Scintilla supports some platform accessibility features. This support differs between platforms. On GTK and Cocoa the platform accessibility APIs are implemented sufficiently to make screen readers work. On Win32, the system caret is manipulated to help screen readers.
/// On most platforms, accessibility is either implemented or not implemented and this can be discovered with <see cref="SciApi.SCI_GETACCESSIBILITY"/> with <see cref="SciApi.SCI_SETACCESSIBILITY"/> performing no action. On GTK, there are storage and performance costs to accessibility, so it can be disabled by calling <see cref="SciApi.SCI_SETACCESSIBILITY"/>.
/// </summary>
public enum Accessibility
{
    /// <summary>
    /// Accessibility is disabled.
    /// </summary>
    Disabled = SciApi.SC_ACCESSIBILITY_DISABLED,

    /// <summary>
    /// Accessibility is enabled.
    /// </summary>
    Enabled = SciApi.SC_ACCESSIBILITY_ENABLED,
}
