namespace ScintillaNET;

/// <summary>
/// Lexer property types.
/// </summary>
public enum PropertyType
{
    /// <summary>
    /// A Boolean property. This is the default.
    /// </summary>
    Boolean = SciApi.SC_TYPE_BOOLEAN,

    /// <summary>
    /// An integer property.
    /// </summary>
    Integer = SciApi.SC_TYPE_INTEGER,

    /// <summary>
    /// A string property.
    /// </summary>
    String = SciApi.SC_TYPE_STRING,
}
