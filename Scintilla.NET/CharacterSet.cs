using System;

namespace ScintillaNET;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Character sets that can be used for different styles via <see cref="SciApi.SCI_STYLESETCHARACTERSET"/>.
/// </summary>
public enum CharacterSet
{
    Ansi = SciApi.SC_CHARSET_ANSI,
    Default = SciApi.SC_CHARSET_DEFAULT,
    Baltic = SciApi.SC_CHARSET_BALTIC,
    ChineseBig5 = SciApi.SC_CHARSET_CHINESEBIG5,
    EastEurope = SciApi.SC_CHARSET_EASTEUROPE,
    Gb2312 = SciApi.SC_CHARSET_GB2312,
    Greek = SciApi.SC_CHARSET_GREEK,
    Hangul = SciApi.SC_CHARSET_HANGUL,
    Mac = SciApi.SC_CHARSET_MAC,
    Oem = SciApi.SC_CHARSET_OEM,
    Russian = SciApi.SC_CHARSET_RUSSIAN,
    Oem866 = SciApi.SC_CHARSET_OEM866,
    Cyrillic = SciApi.SC_CHARSET_CYRILLIC,
    ShiftJis = SciApi.SC_CHARSET_SHIFTJIS,
    Symbol = SciApi.SC_CHARSET_SYMBOL,
    Turkish = SciApi.SC_CHARSET_TURKISH,
    Johab = SciApi.SC_CHARSET_JOHAB,
    Hebrew = SciApi.SC_CHARSET_HEBREW,
    Arabic = SciApi.SC_CHARSET_ARABIC,
    Vietnamese = SciApi.SC_CHARSET_VIETNAMESE,
    Thai = SciApi.SC_CHARSET_THAI,
    Iso8859_15 = SciApi.SC_CHARSET_8859_15,
}
