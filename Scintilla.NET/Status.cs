namespace ScintillaNET;

/// <summary>
/// Possible status codes returned by the <see cref="Scintilla.Status" /> property.
/// </summary>
public enum Status
{
    /// <summary>
    /// No failures.
    /// </summary>
    Ok = SciApi.SC_STATUS_OK,

    /// <summary>
    /// Generic failure.
    /// </summary>
    Failure = SciApi.SC_STATUS_FAILURE,

    /// <summary>
    /// Memory is exhausted.
    /// </summary>
    BadAlloc = SciApi.SC_STATUS_BADALLOC,

    /// <summary>
    /// Regular expression is invalid.
    /// </summary>
    WarnRegex = SciApi.SC_STATUS_WARN_REGEX,
}
