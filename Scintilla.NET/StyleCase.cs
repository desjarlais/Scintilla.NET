namespace ScintillaNET;

/// <summary>
/// The possible casing styles of a style.
/// </summary>
public enum StyleCase
{
    /// <summary>
    /// Display the text normally.
    /// </summary>
    Mixed = SciApi.SC_CASE_MIXED,

    /// <summary>
    /// Display the text in upper case.
    /// </summary>
    Upper = SciApi.SC_CASE_UPPER,

    /// <summary>
    /// Display the text in lower case.
    /// </summary>
    Lower = SciApi.SC_CASE_LOWER,

    /// <summary>
    /// Display the text in camel case.
    /// </summary>
    Camel = SciApi.SC_CASE_CAMEL,
}
