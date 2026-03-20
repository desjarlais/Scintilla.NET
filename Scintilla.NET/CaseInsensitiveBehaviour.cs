namespace ScintillaNET;

/// <summary>
/// Case sensitivity of auto-complete.
/// </summary>
public enum CaseInsensitiveBehaviour
{
    /// <summary>
    /// Case sensitive auto-complete.
    /// </summary>
    RespectCase = SciApi.SC_CASEINSENSITIVEBEHAVIOUR_RESPECTCASE,

    /// <summary>
    /// Case insensitive auto-complete.
    /// </summary>
    IgnoreCase = SciApi.SC_CASEINSENSITIVEBEHAVIOUR_IGNORECASE,
}
