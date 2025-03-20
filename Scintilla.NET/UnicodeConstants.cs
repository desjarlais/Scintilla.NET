using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScintillaNET
{
    /// <summary>
    /// Includes various unicode constants and common code points such as "Replacement Character".
    /// </summary>
    public static class UnicodeConstants
    {
        /// <summary>
        /// Unicode "Replacement Character" (�) as char.
        /// </summary>
        public const char replacementCharacter = '�';
        /// <summary>
        /// Unicode "Replacement Character" (�) as string.
        /// </summary>
        public const string replacementCharacterString = "�";
        /// <summary>
        /// Unicode "Replacement Character" (�) as UTF-8 encoded bytes.
        /// </summary>
        public static readonly byte[] replacementCharacterUtf8 = [0xEF, 0xBF, 0xBD];
        /// <summary>
        /// Unicode "Replacement Character" (�) as UTF-8 encoded null-terminated bytes.
        /// </summary>
        public static readonly byte[] replacementCharacterUtf8z = [0xEF, 0xBF, 0xBD, 0x00];
    }
}
