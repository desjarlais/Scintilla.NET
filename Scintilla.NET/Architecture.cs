namespace ScintillaNET;

/// <summary>
/// Indicates the processor architecture.
/// </summary>
public enum Architecture
{
    /// <summary>
    /// An Intel-based 32-bit processor architecture.
    /// </summary>
    X86 = 0,

    /// <summary>
    /// An Intel-based 64-bit processor architecture.
    /// </summary>
    X64 = 1,

    /// <summary>
    /// A 32-bit ARM processor architecture.
    /// </summary>
    Arm = 2,

    /// <summary>
    /// A 64-bit ARM processor architecture.
    /// </summary>
    Arm64 = 3,
}
