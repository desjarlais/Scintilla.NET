using System;

namespace ScintillaNET;

/// <summary>
/// Specifies the how patterns are matched when performing a search in a <see cref="Scintilla" /> control.
/// </summary>
/// <remarks>This enumeration has a FlagsAttribute attribute that allows a bitwise combination of its member values.</remarks>
[Flags]
public enum SearchFlags : uint
{
    /// <summary>
    /// Case-insensitive literal match.
    /// </summary>
    None = SciApi.SCFIND_NONE,

    /// <summary>
    /// A match only occurs with text that matches the case of the search string.
    /// </summary>
    MatchCase = SciApi.SCFIND_MATCHCASE,

    /// <summary>
    /// A match only occurs if the characters before and after are not word characters as defined by <see cref="Scintilla.WordChars"/>.
    /// </summary>
    WholeWord = SciApi.SCFIND_WHOLEWORD,

    /// <summary>
    /// A match only occurs if the character before is not a word character as defined by <see cref="Scintilla.WordChars"/>.
    /// </summary>
    WordStart = SciApi.SCFIND_WORDSTART,

    /// <summary>
    /// The search string should be interpreted as a regular expression.
    /// Uses Scintilla's base implementation unless combined with <see cref="Cxx11Regex"/>.
    /// Regular expressions will only match ranges within a single line, never matching over multiple lines.
    /// </summary>
    Regex = SciApi.SCFIND_REGEXP,

    /// <summary>
    /// Treat regular expression in a more POSIX compatible manner by interpreting bare '(' and ')' for tagged sections rather than "\(" and "\)".
    /// Has no effect when <see cref="Cxx11Regex"/> is set.
    /// </summary>
    Posix = SciApi.SCFIND_POSIX,

    /// <summary>
    /// The search string should be interpreted as a regular expression and use the C++11 &lt;regex&gt; standard library engine.
    /// The <see cref="Scintilla.Status" /> property can queried to determine if the regular expression is invalid.
    /// The ECMAScript flag is set on the regex object and documents will exhibit Unicode-compliant behaviour.
    /// Regular expressions will only match ranges within a single line, never matching over multiple lines.
    /// Must also have <see cref="Regex"/> set.
    /// </summary>
    Cxx11Regex = SciApi.SCFIND_CXX11REGEX,
}
