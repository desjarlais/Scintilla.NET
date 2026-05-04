using System;

namespace ScintillaNET;

/// <summary>
/// Indicates which character indexes are active for a line.
/// Character indexes are currently only supported for UTF-8 documents.
/// </summary>
[Flags]
public enum LineCharacterIndexType : uint
{
    /// <summary>
    /// Indicates no character indexes active for a line.
    /// </summary>
    None = SciApi.SC_LINECHARACTERINDEX_NONE,

    /// <summary>
    /// Indicates UTF-32 character indexes are active for a line.
    /// </summary>
    Utf32 = SciApi.SC_LINECHARACTERINDEX_UTF32,

    /// <summary>
    /// Indicates UTF-16 character indexes are active for a line.
    /// </summary>
    Utf16 = SciApi.SC_LINECHARACTERINDEX_UTF16,
}
