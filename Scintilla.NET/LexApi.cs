using System;
using System.Runtime.InteropServices;

namespace ScintillaNET;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class LexApi
{
    public delegate IntPtr LexerFactoryFunction();
    
    public delegate IntPtr CreateLexer(string lexerName);
    public delegate int GetLexerCount();
    public unsafe delegate void GetLexerName(uint index, byte* name, int bufferLength);
    [Obsolete("Deprecated")] public delegate string LexerNameFromID(int identifier);
    public delegate LexerFactoryFunction GetLexerFactory(uint index);
    public delegate string GetLibraryPropertyNames();
    public delegate void SetLibraryProperty(string key, string value);
    public delegate string GetNameSpace();

    // Lexer
    // =====
    public const int SCLEX_CONTAINER = 0;
    public const int SCLEX_NULL = 1;
    public const int SCLEX_PYTHON = 2;
    public const int SCLEX_CPP = 3;
    public const int SCLEX_HTML = 4;
    public const int SCLEX_XML = 5;
    public const int SCLEX_PERL = 6;
    public const int SCLEX_SQL = 7;
    public const int SCLEX_VB = 8;
    public const int SCLEX_PROPERTIES = 9;
    public const int SCLEX_ERRORLIST = 10;
    public const int SCLEX_MAKEFILE = 11;
    public const int SCLEX_BATCH = 12;
    public const int SCLEX_XCODE = 13;
    public const int SCLEX_LATEX = 14;
    public const int SCLEX_LUA = 15;
    public const int SCLEX_DIFF = 16;
    public const int SCLEX_CONF = 17;
    public const int SCLEX_PASCAL = 18;
    public const int SCLEX_AVE = 19;
    public const int SCLEX_ADA = 20;
    public const int SCLEX_LISP = 21;
    public const int SCLEX_RUBY = 22;
    public const int SCLEX_EIFFEL = 23;
    public const int SCLEX_EIFFELKW = 24;
    public const int SCLEX_TCL = 25;
    public const int SCLEX_NNCRONTAB = 26;
    public const int SCLEX_BULLANT = 27;
    public const int SCLEX_VBSCRIPT = 28;
    public const int SCLEX_BAAN = 31;
    public const int SCLEX_MATLAB = 32;
    public const int SCLEX_SCRIPTOL = 33;
    public const int SCLEX_ASM = 34;
    public const int SCLEX_CPPNOCASE = 35;
    public const int SCLEX_FORTRAN = 36;
    public const int SCLEX_F77 = 37;
    public const int SCLEX_CSS = 38;
    public const int SCLEX_POV = 39;
    public const int SCLEX_LOUT = 40;
    public const int SCLEX_ESCRIPT = 41;
    public const int SCLEX_PS = 42;
    public const int SCLEX_NSIS = 43;
    public const int SCLEX_MMIXAL = 44;
    public const int SCLEX_CLW = 45;
    public const int SCLEX_CLWNOCASE = 46;
    public const int SCLEX_LOT = 47;
    public const int SCLEX_YAML = 48;
    public const int SCLEX_TEX = 49;
    public const int SCLEX_METAPOST = 50;
    public const int SCLEX_POWERBASIC = 51;
    public const int SCLEX_FORTH = 52;
    public const int SCLEX_ERLANG = 53;
    public const int SCLEX_OCTAVE = 54;
    public const int SCLEX_MSSQL = 55;
    public const int SCLEX_VERILOG = 56;
    public const int SCLEX_KIX = 57;
    public const int SCLEX_GUI4CLI = 58;
    public const int SCLEX_SPECMAN = 59;
    public const int SCLEX_AU3 = 60;
    public const int SCLEX_APDL = 61;
    public const int SCLEX_BASH = 62;
    public const int SCLEX_ASN1 = 63;
    public const int SCLEX_VHDL = 64;
    public const int SCLEX_CAML = 65;
    public const int SCLEX_BLITZBASIC = 66;
    public const int SCLEX_PUREBASIC = 67;
    public const int SCLEX_HASKELL = 68;
    public const int SCLEX_PHPSCRIPT = 69;
    public const int SCLEX_TADS3 = 70;
    public const int SCLEX_REBOL = 71;
    public const int SCLEX_SMALLTALK = 72;
    public const int SCLEX_FLAGSHIP = 73;
    public const int SCLEX_CSOUND = 74;
    public const int SCLEX_FREEBASIC = 75;
    public const int SCLEX_INNOSETUP = 76;
    public const int SCLEX_OPAL = 77;
    public const int SCLEX_SPICE = 78;
    public const int SCLEX_D = 79;
    public const int SCLEX_CMAKE = 80;
    public const int SCLEX_GAP = 81;
    public const int SCLEX_PLM = 82;
    public const int SCLEX_PROGRESS = 83;
    public const int SCLEX_ABAQUS = 84;
    public const int SCLEX_ASYMPTOTE = 85;
    public const int SCLEX_R = 86;
    public const int SCLEX_MAGIK = 87;
    public const int SCLEX_POWERSHELL = 88;
    public const int SCLEX_MYSQL = 89;
    public const int SCLEX_PO = 90;
    public const int SCLEX_TAL = 91;
    public const int SCLEX_COBOL = 92;
    public const int SCLEX_TACL = 93;
    public const int SCLEX_SORCUS = 94;
    public const int SCLEX_POWERPRO = 95;
    public const int SCLEX_NIMROD = 96;
    public const int SCLEX_SML = 97;
    public const int SCLEX_MARKDOWN = 98;
    public const int SCLEX_TXT2TAGS = 99;
    public const int SCLEX_A68K = 100;
    public const int SCLEX_MODULA = 101;
    public const int SCLEX_COFFEESCRIPT = 102;
    public const int SCLEX_TCMD = 103;
    public const int SCLEX_AVS = 104;
    public const int SCLEX_ECL = 105;
    public const int SCLEX_OSCRIPT = 106;
    public const int SCLEX_VISUALPROLOG = 107;
    public const int SCLEX_LITERATEHASKELL = 108;
    public const int SCLEX_STTXT = 109;
    public const int SCLEX_KVIRC = 110;
    public const int SCLEX_RUST = 111;
    public const int SCLEX_DMAP = 112;
    public const int SCLEX_AS = 113;
    public const int SCLEX_DMIS = 114;
    public const int SCLEX_REGISTRY = 115;
    public const int SCLEX_BIBTEX = 116;
    public const int SCLEX_SREC = 117;
    public const int SCLEX_IHEX = 118;
    public const int SCLEX_TEHEX = 119;
    public const int SCLEX_JSON = 120;
    public const int SCLEX_EDIFACT = 121;
    public const int SCLEX_INDENT = 122;
    public const int SCLEX_MAXIMA = 123;
    public const int SCLEX_STATA = 124;
    public const int SCLEX_SAS = 125;
    public const int SCLEX_NIM = 126;
    public const int SCLEX_CIL = 127;
    public const int SCLEX_X12 = 128;
    public const int SCLEX_DATAFLEX = 129;
    public const int SCLEX_HOLLYWOOD = 130;
    public const int SCLEX_RAKU = 131;
    public const int SCLEX_FSHARP = 132;
    public const int SCLEX_JULIA = 133;
    public const int SCLEX_ASCIIDOC = 134;
    public const int SCLEX_GDSCRIPT = 135;
    public const int SCLEX_TOML = 136;
    public const int SCLEX_TROFF = 137;
    public const int SCLEX_DART = 138;
    public const int SCLEX_ZIG = 139;
    public const int SCLEX_NIX = 140;
    public const int SCLEX_SINEX = 141;
    public const int SCLEX_ESCSEQ = 142;
    /// <summary>
    /// When a lexer specifies its language as SCLEX_AUTOMATIC it receives a
    /// value assigned in sequence from SCLEX_AUTOMATIC+1.
    /// </summary>
    public const int SCLEX_AUTOMATIC = 1000;
    
    // Python
    // Nimrod
    // ======
    public const int SCE_P_DEFAULT = 0;
    public const int SCE_P_COMMENTLINE = 1;
    public const int SCE_P_NUMBER = 2;
    public const int SCE_P_STRING = 3;
    public const int SCE_P_CHARACTER = 4;
    public const int SCE_P_WORD = 5;
    public const int SCE_P_TRIPLE = 6;
    public const int SCE_P_TRIPLEDOUBLE = 7;
    public const int SCE_P_CLASSNAME = 8;
    public const int SCE_P_DEFNAME = 9;
    public const int SCE_P_OPERATOR = 10;
    public const int SCE_P_IDENTIFIER = 11;
    public const int SCE_P_COMMENTBLOCK = 12;
    public const int SCE_P_STRINGEOL = 13;
    public const int SCE_P_WORD2 = 14;
    public const int SCE_P_DECORATOR = 15;
    public const int SCE_P_FSTRING = 16;
    public const int SCE_P_FCHARACTER = 17;
    public const int SCE_P_FTRIPLE = 18;
    public const int SCE_P_FTRIPLEDOUBLE = 19;
    public const int SCE_P_ATTRIBUTE = 20;
    
    // Cpp
    // BullAnt
    // TACL
    // TAL
    // =======
    public const int SCE_C_DEFAULT = 0;
    public const int SCE_C_COMMENT = 1;
    public const int SCE_C_COMMENTLINE = 2;
    public const int SCE_C_COMMENTDOC = 3;
    public const int SCE_C_NUMBER = 4;
    public const int SCE_C_WORD = 5;
    public const int SCE_C_STRING = 6;
    public const int SCE_C_CHARACTER = 7;
    public const int SCE_C_UUID = 8;
    public const int SCE_C_PREPROCESSOR = 9;
    public const int SCE_C_OPERATOR = 10;
    public const int SCE_C_IDENTIFIER = 11;
    public const int SCE_C_STRINGEOL = 12;
    public const int SCE_C_VERBATIM = 13;
    public const int SCE_C_REGEX = 14;
    public const int SCE_C_COMMENTLINEDOC = 15;
    public const int SCE_C_WORD2 = 16;
    public const int SCE_C_COMMENTDOCKEYWORD = 17;
    public const int SCE_C_COMMENTDOCKEYWORDERROR = 18;
    public const int SCE_C_GLOBALCLASS = 19;
    public const int SCE_C_STRINGRAW = 20;
    public const int SCE_C_TRIPLEVERBATIM = 21;
    public const int SCE_C_HASHQUOTEDSTRING = 22;
    public const int SCE_C_PREPROCESSORCOMMENT = 23;
    public const int SCE_C_PREPROCESSORCOMMENTDOC = 24;
    public const int SCE_C_USERLITERAL = 25;
    public const int SCE_C_TASKMARKER = 26;
    public const int SCE_C_ESCAPESEQUENCE = 27;
    
    // COBOL
    // =====
    public const int SCE_COBOL_DEFAULT = 0;
    public const int SCE_COBOL_COMMENT = 1;
    public const int SCE_COBOL_COMMENTLINE = 2;
    public const int SCE_COBOL_COMMENTDOC = 3;
    public const int SCE_COBOL_NUMBER = 4;
    public const int SCE_COBOL_WORD = 5;
    public const int SCE_COBOL_STRING = 6;
    public const int SCE_COBOL_CHARACTER = 7;
    public const int SCE_COBOL_WORD3 = 8;
    public const int SCE_COBOL_PREPROCESSOR = 9;
    public const int SCE_COBOL_OPERATOR = 10;
    public const int SCE_COBOL_IDENTIFIER = 11;
    public const int SCE_COBOL_WORD2 = 16;
    
    // D
    // ===
    public const int SCE_D_DEFAULT = 0;
    public const int SCE_D_COMMENT = 1;
    public const int SCE_D_COMMENTLINE = 2;
    public const int SCE_D_COMMENTDOC = 3;
    public const int SCE_D_COMMENTNESTED = 4;
    public const int SCE_D_NUMBER = 5;
    public const int SCE_D_WORD = 6;
    public const int SCE_D_WORD2 = 7;
    public const int SCE_D_WORD3 = 8;
    public const int SCE_D_TYPEDEF = 9;
    public const int SCE_D_STRING = 10;
    public const int SCE_D_STRINGEOL = 11;
    public const int SCE_D_CHARACTER = 12;
    public const int SCE_D_OPERATOR = 13;
    public const int SCE_D_IDENTIFIER = 14;
    public const int SCE_D_COMMENTLINEDOC = 15;
    public const int SCE_D_COMMENTDOCKEYWORD = 16;
    public const int SCE_D_COMMENTDOCKEYWORDERROR = 17;
    public const int SCE_D_STRINGB = 18;
    public const int SCE_D_STRINGR = 19;
    public const int SCE_D_WORD5 = 20;
    public const int SCE_D_WORD6 = 21;
    public const int SCE_D_WORD7 = 22;
    
    // TCL
    // ===
    public const int SCE_TCL_DEFAULT = 0;
    public const int SCE_TCL_COMMENT = 1;
    public const int SCE_TCL_COMMENTLINE = 2;
    public const int SCE_TCL_NUMBER = 3;
    public const int SCE_TCL_WORD_IN_QUOTE = 4;
    public const int SCE_TCL_IN_QUOTE = 5;
    public const int SCE_TCL_OPERATOR = 6;
    public const int SCE_TCL_IDENTIFIER = 7;
    public const int SCE_TCL_SUBSTITUTION = 8;
    public const int SCE_TCL_SUB_BRACE = 9;
    public const int SCE_TCL_MODIFIER = 10;
    public const int SCE_TCL_EXPAND = 11;
    public const int SCE_TCL_WORD = 12;
    public const int SCE_TCL_WORD2 = 13;
    public const int SCE_TCL_WORD3 = 14;
    public const int SCE_TCL_WORD4 = 15;
    public const int SCE_TCL_WORD5 = 16;
    public const int SCE_TCL_WORD6 = 17;
    public const int SCE_TCL_WORD7 = 18;
    public const int SCE_TCL_WORD8 = 19;
    public const int SCE_TCL_COMMENT_BOX = 20;
    public const int SCE_TCL_BLOCK_COMMENT = 21;
    
    // HTML
    // XML
    // ====
    public const int SCE_H_DEFAULT = 0;
    public const int SCE_H_TAG = 1;
    public const int SCE_H_TAGUNKNOWN = 2;
    public const int SCE_H_ATTRIBUTE = 3;
    public const int SCE_H_ATTRIBUTEUNKNOWN = 4;
    public const int SCE_H_NUMBER = 5;
    public const int SCE_H_DOUBLESTRING = 6;
    public const int SCE_H_SINGLESTRING = 7;
    public const int SCE_H_OTHER = 8;
    public const int SCE_H_COMMENT = 9;
    public const int SCE_H_ENTITY = 10;
    /// <summary>XML and ASP</summary>
    public const int SCE_H_TAGEND = 11;
    public const int SCE_H_XMLSTART = 12;
    public const int SCE_H_XMLEND = 13;
    public const int SCE_H_SCRIPT = 14;
    public const int SCE_H_ASP = 15;
    public const int SCE_H_ASPAT = 16;
    public const int SCE_H_CDATA = 17;
    public const int SCE_H_QUESTION = 18;
    /// <summary>More HTML</summary>
    public const int SCE_H_VALUE = 19;
    /// <summary>X-Code, ASP.NET, JSP</summary>
    public const int SCE_H_XCCOMMENT = 20;
    /// <summary>SGML</summary>
    public const int SCE_H_SGML_DEFAULT = 21;
    public const int SCE_H_SGML_COMMAND = 22;
    public const int SCE_H_SGML_1ST_PARAM = 23;
    public const int SCE_H_SGML_DOUBLESTRING = 24;
    public const int SCE_H_SGML_SIMPLESTRING = 25;
    public const int SCE_H_SGML_ERROR = 26;
    public const int SCE_H_SGML_SPECIAL = 27;
    public const int SCE_H_SGML_ENTITY = 28;
    public const int SCE_H_SGML_COMMENT = 29;
    public const int SCE_H_SGML_1ST_PARAM_COMMENT = 30;
    public const int SCE_H_SGML_BLOCK_DEFAULT = 31;
    /// <summary>Embedded Javascript</summary>
    public const int SCE_HJ_START = 40;
    public const int SCE_HJ_DEFAULT = 41;
    public const int SCE_HJ_COMMENT = 42;
    public const int SCE_HJ_COMMENTLINE = 43;
    public const int SCE_HJ_COMMENTDOC = 44;
    public const int SCE_HJ_NUMBER = 45;
    public const int SCE_HJ_WORD = 46;
    public const int SCE_HJ_KEYWORD = 47;
    public const int SCE_HJ_DOUBLESTRING = 48;
    public const int SCE_HJ_SINGLESTRING = 49;
    public const int SCE_HJ_SYMBOLS = 50;
    public const int SCE_HJ_STRINGEOL = 51;
    public const int SCE_HJ_REGEX = 52;
    public const int SCE_HJ_TEMPLATELITERAL = 53;
    /// <summary>ASP Javascript</summary>
    public const int SCE_HJA_START = 55;
    public const int SCE_HJA_DEFAULT = 56;
    public const int SCE_HJA_COMMENT = 57;
    public const int SCE_HJA_COMMENTLINE = 58;
    public const int SCE_HJA_COMMENTDOC = 59;
    public const int SCE_HJA_NUMBER = 60;
    public const int SCE_HJA_WORD = 61;
    public const int SCE_HJA_KEYWORD = 62;
    public const int SCE_HJA_DOUBLESTRING = 63;
    public const int SCE_HJA_SINGLESTRING = 64;
    public const int SCE_HJA_SYMBOLS = 65;
    public const int SCE_HJA_STRINGEOL = 66;
    public const int SCE_HJA_REGEX = 67;
    public const int SCE_HJA_TEMPLATELITERAL = 68;
    /// <summary>Embedded VBScript</summary>
    public const int SCE_HB_START = 70;
    public const int SCE_HB_DEFAULT = 71;
    public const int SCE_HB_COMMENTLINE = 72;
    public const int SCE_HB_NUMBER = 73;
    public const int SCE_HB_WORD = 74;
    public const int SCE_HB_STRING = 75;
    public const int SCE_HB_IDENTIFIER = 76;
    public const int SCE_HB_STRINGEOL = 77;
    /// <summary>ASP VBScript</summary>
    public const int SCE_HBA_START = 80;
    public const int SCE_HBA_DEFAULT = 81;
    public const int SCE_HBA_COMMENTLINE = 82;
    public const int SCE_HBA_NUMBER = 83;
    public const int SCE_HBA_WORD = 84;
    public const int SCE_HBA_STRING = 85;
    public const int SCE_HBA_IDENTIFIER = 86;
    public const int SCE_HBA_STRINGEOL = 87;
    /// <summary>Embedded Python</summary>
    public const int SCE_HP_START = 90;
    public const int SCE_HP_DEFAULT = 91;
    public const int SCE_HP_COMMENTLINE = 92;
    public const int SCE_HP_NUMBER = 93;
    public const int SCE_HP_STRING = 94;
    public const int SCE_HP_CHARACTER = 95;
    public const int SCE_HP_WORD = 96;
    public const int SCE_HP_TRIPLE = 97;
    public const int SCE_HP_TRIPLEDOUBLE = 98;
    public const int SCE_HP_CLASSNAME = 99;
    public const int SCE_HP_DEFNAME = 100;
    public const int SCE_HP_OPERATOR = 101;
    public const int SCE_HP_IDENTIFIER = 102;
    /// <summary>PHP</summary>
    public const int SCE_HPHP_COMPLEX_VARIABLE = 104;
    /// <summary>ASP Python</summary>
    public const int SCE_HPA_START = 105;
    public const int SCE_HPA_DEFAULT = 106;
    public const int SCE_HPA_COMMENTLINE = 107;
    public const int SCE_HPA_NUMBER = 108;
    public const int SCE_HPA_STRING = 109;
    public const int SCE_HPA_CHARACTER = 110;
    public const int SCE_HPA_WORD = 111;
    public const int SCE_HPA_TRIPLE = 112;
    public const int SCE_HPA_TRIPLEDOUBLE = 113;
    public const int SCE_HPA_CLASSNAME = 114;
    public const int SCE_HPA_DEFNAME = 115;
    public const int SCE_HPA_OPERATOR = 116;
    public const int SCE_HPA_IDENTIFIER = 117;
    /// <summary>PHP</summary>
    public const int SCE_HPHP_DEFAULT = 118;
    public const int SCE_HPHP_HSTRING = 119;
    public const int SCE_HPHP_SIMPLESTRING = 120;
    public const int SCE_HPHP_WORD = 121;
    public const int SCE_HPHP_NUMBER = 122;
    public const int SCE_HPHP_VARIABLE = 123;
    public const int SCE_HPHP_COMMENT = 124;
    public const int SCE_HPHP_COMMENTLINE = 125;
    public const int SCE_HPHP_HSTRING_VARIABLE = 126;
    public const int SCE_HPHP_OPERATOR = 127;
    
    // Perl
    // ====
    public const int SCE_PL_DEFAULT = 0;
    public const int SCE_PL_ERROR = 1;
    public const int SCE_PL_COMMENTLINE = 2;
    public const int SCE_PL_POD = 3;
    public const int SCE_PL_NUMBER = 4;
    public const int SCE_PL_WORD = 5;
    public const int SCE_PL_STRING = 6;
    public const int SCE_PL_CHARACTER = 7;
    public const int SCE_PL_PUNCTUATION = 8;
    public const int SCE_PL_PREPROCESSOR = 9;
    public const int SCE_PL_OPERATOR = 10;
    public const int SCE_PL_IDENTIFIER = 11;
    public const int SCE_PL_SCALAR = 12;
    public const int SCE_PL_ARRAY = 13;
    public const int SCE_PL_HASH = 14;
    public const int SCE_PL_SYMBOLTABLE = 15;
    public const int SCE_PL_VARIABLE_INDEXER = 16;
    public const int SCE_PL_REGEX = 17;
    public const int SCE_PL_REGSUBST = 18;
    public const int SCE_PL_LONGQUOTE = 19;
    public const int SCE_PL_BACKTICKS = 20;
    public const int SCE_PL_DATASECTION = 21;
    public const int SCE_PL_HERE_DELIM = 22;
    public const int SCE_PL_HERE_Q = 23;
    public const int SCE_PL_HERE_QQ = 24;
    public const int SCE_PL_HERE_QX = 25;
    public const int SCE_PL_STRING_Q = 26;
    public const int SCE_PL_STRING_QQ = 27;
    public const int SCE_PL_STRING_QX = 28;
    public const int SCE_PL_STRING_QR = 29;
    public const int SCE_PL_STRING_QW = 30;
    public const int SCE_PL_POD_VERB = 31;
    public const int SCE_PL_SUB_PROTOTYPE = 40;
    public const int SCE_PL_FORMAT_IDENT = 41;
    public const int SCE_PL_FORMAT = 42;
    public const int SCE_PL_STRING_VAR = 43;
    public const int SCE_PL_XLAT = 44;
    public const int SCE_PL_REGEX_VAR = 54;
    public const int SCE_PL_REGSUBST_VAR = 55;
    public const int SCE_PL_BACKTICKS_VAR = 57;
    public const int SCE_PL_HERE_QQ_VAR = 61;
    public const int SCE_PL_HERE_QX_VAR = 62;
    public const int SCE_PL_STRING_QQ_VAR = 64;
    public const int SCE_PL_STRING_QX_VAR = 65;
    public const int SCE_PL_STRING_QR_VAR = 66;
    
    // Ruby
    // ====
    public const int SCE_RB_DEFAULT = 0;
    public const int SCE_RB_ERROR = 1;
    public const int SCE_RB_COMMENTLINE = 2;
    public const int SCE_RB_POD = 3;
    public const int SCE_RB_NUMBER = 4;
    public const int SCE_RB_WORD = 5;
    public const int SCE_RB_STRING = 6;
    public const int SCE_RB_CHARACTER = 7;
    public const int SCE_RB_CLASSNAME = 8;
    public const int SCE_RB_DEFNAME = 9;
    public const int SCE_RB_OPERATOR = 10;
    public const int SCE_RB_IDENTIFIER = 11;
    public const int SCE_RB_REGEX = 12;
    public const int SCE_RB_GLOBAL = 13;
    public const int SCE_RB_SYMBOL = 14;
    public const int SCE_RB_MODULE_NAME = 15;
    public const int SCE_RB_INSTANCE_VAR = 16;
    public const int SCE_RB_CLASS_VAR = 17;
    public const int SCE_RB_BACKTICKS = 18;
    public const int SCE_RB_DATASECTION = 19;
    public const int SCE_RB_HERE_DELIM = 20;
    public const int SCE_RB_HERE_Q = 21;
    public const int SCE_RB_HERE_QQ = 22;
    public const int SCE_RB_HERE_QX = 23;
    public const int SCE_RB_STRING_Q = 24;
    public const int SCE_RB_STRING_QQ = 25;
    public const int SCE_RB_STRING_QX = 26;
    public const int SCE_RB_STRING_QR = 27;
    public const int SCE_RB_STRING_QW = 28;
    public const int SCE_RB_WORD_DEMOTED = 29;
    public const int SCE_RB_STDIN = 30;
    public const int SCE_RB_STDOUT = 31;
    public const int SCE_RB_STDERR = 40;
    public const int SCE_RB_STRING_W = 41;
    public const int SCE_RB_STRING_I = 42;
    public const int SCE_RB_STRING_QI = 43;
    public const int SCE_RB_STRING_QS = 44;
    public const int SCE_RB_UPPER_BOUND = 45;
    
    // VB
    // VBScript
    // PowerBasic
    // BlitzBasic
    // PureBasic
    // FreeBasic
    // ==========
    public const int SCE_B_DEFAULT = 0;
    public const int SCE_B_COMMENT = 1;
    public const int SCE_B_NUMBER = 2;
    public const int SCE_B_KEYWORD = 3;
    public const int SCE_B_STRING = 4;
    public const int SCE_B_PREPROCESSOR = 5;
    public const int SCE_B_OPERATOR = 6;
    public const int SCE_B_IDENTIFIER = 7;
    public const int SCE_B_DATE = 8;
    public const int SCE_B_STRINGEOL = 9;
    public const int SCE_B_KEYWORD2 = 10;
    public const int SCE_B_KEYWORD3 = 11;
    public const int SCE_B_KEYWORD4 = 12;
    public const int SCE_B_CONSTANT = 13;
    public const int SCE_B_ASM = 14;
    public const int SCE_B_LABEL = 15;
    public const int SCE_B_ERROR = 16;
    public const int SCE_B_HEXNUMBER = 17;
    public const int SCE_B_BINNUMBER = 18;
    public const int SCE_B_COMMENTBLOCK = 19;
    public const int SCE_B_DOCLINE = 20;
    public const int SCE_B_DOCBLOCK = 21;
    public const int SCE_B_DOCKEYWORD = 22;
    
    // Properties
    // ==========
    public const int SCE_PROPS_DEFAULT = 0;
    public const int SCE_PROPS_COMMENT = 1;
    public const int SCE_PROPS_SECTION = 2;
    public const int SCE_PROPS_ASSIGNMENT = 3;
    public const int SCE_PROPS_DEFVAL = 4;
    public const int SCE_PROPS_KEY = 5;
    
    // LaTeX
    // =====
    public const int SCE_L_DEFAULT = 0;
    public const int SCE_L_COMMAND = 1;
    public const int SCE_L_TAG = 2;
    public const int SCE_L_MATH = 3;
    public const int SCE_L_COMMENT = 4;
    public const int SCE_L_TAG2 = 5;
    public const int SCE_L_MATH2 = 6;
    public const int SCE_L_COMMENT2 = 7;
    public const int SCE_L_VERBATIM = 8;
    public const int SCE_L_SHORTCMD = 9;
    public const int SCE_L_SPECIAL = 10;
    public const int SCE_L_CMDOPT = 11;
    public const int SCE_L_ERROR = 12;
    
    // Lua
    // ===
    public const int SCE_LUA_DEFAULT = 0;
    public const int SCE_LUA_COMMENT = 1;
    public const int SCE_LUA_COMMENTLINE = 2;
    public const int SCE_LUA_COMMENTDOC = 3;
    public const int SCE_LUA_NUMBER = 4;
    public const int SCE_LUA_WORD = 5;
    public const int SCE_LUA_STRING = 6;
    public const int SCE_LUA_CHARACTER = 7;
    public const int SCE_LUA_LITERALSTRING = 8;
    public const int SCE_LUA_PREPROCESSOR = 9;
    public const int SCE_LUA_OPERATOR = 10;
    public const int SCE_LUA_IDENTIFIER = 11;
    public const int SCE_LUA_STRINGEOL = 12;
    public const int SCE_LUA_WORD2 = 13;
    public const int SCE_LUA_WORD3 = 14;
    public const int SCE_LUA_WORD4 = 15;
    public const int SCE_LUA_WORD5 = 16;
    public const int SCE_LUA_WORD6 = 17;
    public const int SCE_LUA_WORD7 = 18;
    public const int SCE_LUA_WORD8 = 19;
    public const int SCE_LUA_LABEL = 20;
    
    // ErrorList
    // =========
    public const int SCE_ERR_DEFAULT = 0;
    public const int SCE_ERR_PYTHON = 1;
    public const int SCE_ERR_GCC = 2;
    public const int SCE_ERR_MS = 3;
    public const int SCE_ERR_CMD = 4;
    public const int SCE_ERR_BORLAND = 5;
    public const int SCE_ERR_PERL = 6;
    public const int SCE_ERR_NET = 7;
    public const int SCE_ERR_LUA = 8;
    public const int SCE_ERR_CTAG = 9;
    public const int SCE_ERR_DIFF_CHANGED = 10;
    public const int SCE_ERR_DIFF_ADDITION = 11;
    public const int SCE_ERR_DIFF_DELETION = 12;
    public const int SCE_ERR_DIFF_MESSAGE = 13;
    public const int SCE_ERR_PHP = 14;
    public const int SCE_ERR_ELF = 15;
    public const int SCE_ERR_IFC = 16;
    public const int SCE_ERR_IFORT = 17;
    public const int SCE_ERR_ABSF = 18;
    public const int SCE_ERR_TIDY = 19;
    public const int SCE_ERR_JAVA_STACK = 20;
    public const int SCE_ERR_VALUE = 21;
    public const int SCE_ERR_GCC_INCLUDED_FROM = 22;
    public const int SCE_ERR_ESCSEQ = 23;
    public const int SCE_ERR_ESCSEQ_UNKNOWN = 24;
    public const int SCE_ERR_GCC_EXCERPT = 25;
    public const int SCE_ERR_BASH = 26;
    public const int SCE_ERR_ES_BLACK = 40;
    public const int SCE_ERR_ES_RED = 41;
    public const int SCE_ERR_ES_GREEN = 42;
    public const int SCE_ERR_ES_BROWN = 43;
    public const int SCE_ERR_ES_BLUE = 44;
    public const int SCE_ERR_ES_MAGENTA = 45;
    public const int SCE_ERR_ES_CYAN = 46;
    public const int SCE_ERR_ES_GRAY = 47;
    public const int SCE_ERR_ES_DARK_GRAY = 48;
    public const int SCE_ERR_ES_BRIGHT_RED = 49;
    public const int SCE_ERR_ES_BRIGHT_GREEN = 50;
    public const int SCE_ERR_ES_YELLOW = 51;
    public const int SCE_ERR_ES_BRIGHT_BLUE = 52;
    public const int SCE_ERR_ES_BRIGHT_MAGENTA = 53;
    public const int SCE_ERR_ES_BRIGHT_CYAN = 54;
    public const int SCE_ERR_ES_WHITE = 55;
    
    // Batch
    // =====
    public const int SCE_BAT_DEFAULT = 0;
    public const int SCE_BAT_COMMENT = 1;
    public const int SCE_BAT_WORD = 2;
    public const int SCE_BAT_LABEL = 3;
    public const int SCE_BAT_HIDE = 4;
    public const int SCE_BAT_COMMAND = 5;
    public const int SCE_BAT_IDENTIFIER = 6;
    public const int SCE_BAT_OPERATOR = 7;
    public const int SCE_BAT_AFTER_LABEL = 8;
    
    // TCMD
    // ====
    public const int SCE_TCMD_DEFAULT = 0;
    public const int SCE_TCMD_COMMENT = 1;
    public const int SCE_TCMD_WORD = 2;
    public const int SCE_TCMD_LABEL = 3;
    public const int SCE_TCMD_HIDE = 4;
    public const int SCE_TCMD_COMMAND = 5;
    public const int SCE_TCMD_IDENTIFIER = 6;
    public const int SCE_TCMD_OPERATOR = 7;
    public const int SCE_TCMD_ENVIRONMENT = 8;
    public const int SCE_TCMD_EXPANSION = 9;
    public const int SCE_TCMD_CLABEL = 10;
    
    // MakeFile
    // ========
    public const int SCE_MAKE_DEFAULT = 0;
    public const int SCE_MAKE_COMMENT = 1;
    public const int SCE_MAKE_PREPROCESSOR = 2;
    public const int SCE_MAKE_IDENTIFIER = 3;
    public const int SCE_MAKE_OPERATOR = 4;
    public const int SCE_MAKE_TARGET = 5;
    public const int SCE_MAKE_IDEOL = 9;
    
    // Diff
    // ====
    public const int SCE_DIFF_DEFAULT = 0;
    public const int SCE_DIFF_COMMENT = 1;
    public const int SCE_DIFF_COMMAND = 2;
    public const int SCE_DIFF_HEADER = 3;
    public const int SCE_DIFF_POSITION = 4;
    public const int SCE_DIFF_DELETED = 5;
    public const int SCE_DIFF_ADDED = 6;
    public const int SCE_DIFF_CHANGED = 7;
    public const int SCE_DIFF_PATCH_ADD = 8;
    public const int SCE_DIFF_PATCH_DELETE = 9;
    public const int SCE_DIFF_REMOVED_PATCH_ADD = 10;
    public const int SCE_DIFF_REMOVED_PATCH_DELETE = 11;
    
    // Conf
    // ====
    public const int SCE_CONF_DEFAULT = 0;
    public const int SCE_CONF_COMMENT = 1;
    public const int SCE_CONF_NUMBER = 2;
    public const int SCE_CONF_IDENTIFIER = 3;
    public const int SCE_CONF_EXTENSION = 4;
    public const int SCE_CONF_PARAMETER = 5;
    public const int SCE_CONF_STRING = 6;
    public const int SCE_CONF_OPERATOR = 7;
    public const int SCE_CONF_IP = 8;
    public const int SCE_CONF_DIRECTIVE = 9;
    
    // Avenue
    // ======
    public const int SCE_AVE_DEFAULT = 0;
    public const int SCE_AVE_COMMENT = 1;
    public const int SCE_AVE_NUMBER = 2;
    public const int SCE_AVE_WORD = 3;
    public const int SCE_AVE_STRING = 6;
    public const int SCE_AVE_ENUM = 7;
    public const int SCE_AVE_STRINGEOL = 8;
    public const int SCE_AVE_IDENTIFIER = 9;
    public const int SCE_AVE_OPERATOR = 10;
    public const int SCE_AVE_WORD1 = 11;
    public const int SCE_AVE_WORD2 = 12;
    public const int SCE_AVE_WORD3 = 13;
    public const int SCE_AVE_WORD4 = 14;
    public const int SCE_AVE_WORD5 = 15;
    public const int SCE_AVE_WORD6 = 16;
    
    // Ada
    // ===
    public const int SCE_ADA_DEFAULT = 0;
    public const int SCE_ADA_WORD = 1;
    public const int SCE_ADA_IDENTIFIER = 2;
    public const int SCE_ADA_NUMBER = 3;
    public const int SCE_ADA_DELIMITER = 4;
    public const int SCE_ADA_CHARACTER = 5;
    public const int SCE_ADA_CHARACTEREOL = 6;
    public const int SCE_ADA_STRING = 7;
    public const int SCE_ADA_STRINGEOL = 8;
    public const int SCE_ADA_LABEL = 9;
    public const int SCE_ADA_COMMENTLINE = 10;
    public const int SCE_ADA_ILLEGAL = 11;
    
    // Baan
    // ====
    public const int SCE_BAAN_DEFAULT = 0;
    public const int SCE_BAAN_COMMENT = 1;
    public const int SCE_BAAN_COMMENTDOC = 2;
    public const int SCE_BAAN_NUMBER = 3;
    public const int SCE_BAAN_WORD = 4;
    public const int SCE_BAAN_STRING = 5;
    public const int SCE_BAAN_PREPROCESSOR = 6;
    public const int SCE_BAAN_OPERATOR = 7;
    public const int SCE_BAAN_IDENTIFIER = 8;
    public const int SCE_BAAN_STRINGEOL = 9;
    public const int SCE_BAAN_WORD2 = 10;
    public const int SCE_BAAN_WORD3 = 11;
    public const int SCE_BAAN_WORD4 = 12;
    public const int SCE_BAAN_WORD5 = 13;
    public const int SCE_BAAN_WORD6 = 14;
    public const int SCE_BAAN_WORD7 = 15;
    public const int SCE_BAAN_WORD8 = 16;
    public const int SCE_BAAN_WORD9 = 17;
    public const int SCE_BAAN_TABLEDEF = 18;
    public const int SCE_BAAN_TABLESQL = 19;
    public const int SCE_BAAN_FUNCTION = 20;
    public const int SCE_BAAN_DOMDEF = 21;
    public const int SCE_BAAN_FUNCDEF = 22;
    public const int SCE_BAAN_OBJECTDEF = 23;
    public const int SCE_BAAN_DEFINEDEF = 24;
    
    // Lisp
    // ====
    public const int SCE_LISP_DEFAULT = 0;
    public const int SCE_LISP_COMMENT = 1;
    public const int SCE_LISP_NUMBER = 2;
    public const int SCE_LISP_KEYWORD = 3;
    public const int SCE_LISP_KEYWORD_KW = 4;
    public const int SCE_LISP_SYMBOL = 5;
    public const int SCE_LISP_STRING = 6;
    public const int SCE_LISP_STRINGEOL = 8;
    public const int SCE_LISP_IDENTIFIER = 9;
    public const int SCE_LISP_OPERATOR = 10;
    public const int SCE_LISP_SPECIAL = 11;
    public const int SCE_LISP_MULTI_COMMENT = 12;
    
    // Eiffel
    // EiffelKW
    // ========
    public const int SCE_EIFFEL_DEFAULT = 0;
    public const int SCE_EIFFEL_COMMENTLINE = 1;
    public const int SCE_EIFFEL_NUMBER = 2;
    public const int SCE_EIFFEL_WORD = 3;
    public const int SCE_EIFFEL_STRING = 4;
    public const int SCE_EIFFEL_CHARACTER = 5;
    public const int SCE_EIFFEL_OPERATOR = 6;
    public const int SCE_EIFFEL_IDENTIFIER = 7;
    public const int SCE_EIFFEL_STRINGEOL = 8;
    
    // NNCronTab
    // =========
    public const int SCE_NNCRONTAB_DEFAULT = 0;
    public const int SCE_NNCRONTAB_COMMENT = 1;
    public const int SCE_NNCRONTAB_TASK = 2;
    public const int SCE_NNCRONTAB_SECTION = 3;
    public const int SCE_NNCRONTAB_KEYWORD = 4;
    public const int SCE_NNCRONTAB_MODIFIER = 5;
    public const int SCE_NNCRONTAB_ASTERISK = 6;
    public const int SCE_NNCRONTAB_NUMBER = 7;
    public const int SCE_NNCRONTAB_STRING = 8;
    public const int SCE_NNCRONTAB_ENVIRONMENT = 9;
    public const int SCE_NNCRONTAB_IDENTIFIER = 10;
    
    // Forth
    // =====
    public const int SCE_FORTH_DEFAULT = 0;
    public const int SCE_FORTH_COMMENT = 1;
    public const int SCE_FORTH_COMMENT_ML = 2;
    public const int SCE_FORTH_IDENTIFIER = 3;
    public const int SCE_FORTH_CONTROL = 4;
    public const int SCE_FORTH_KEYWORD = 5;
    public const int SCE_FORTH_DEFWORD = 6;
    public const int SCE_FORTH_PREWORD1 = 7;
    public const int SCE_FORTH_PREWORD2 = 8;
    public const int SCE_FORTH_NUMBER = 9;
    public const int SCE_FORTH_STRING = 10;
    public const int SCE_FORTH_LOCALE = 11;
    
    // MatLab
    // ======
    public const int SCE_MATLAB_DEFAULT = 0;
    public const int SCE_MATLAB_COMMENT = 1;
    public const int SCE_MATLAB_COMMAND = 2;
    public const int SCE_MATLAB_NUMBER = 3;
    public const int SCE_MATLAB_KEYWORD = 4;
    /// <summary>single quoted string</summary>
    public const int SCE_MATLAB_STRING = 5;
    public const int SCE_MATLAB_OPERATOR = 6;
    public const int SCE_MATLAB_IDENTIFIER = 7;
    public const int SCE_MATLAB_DOUBLEQUOTESTRING = 8;
    
    // Maxima
    // ======
    public const int SCE_MAXIMA_OPERATOR = 0;
    public const int SCE_MAXIMA_COMMANDENDING = 1;
    public const int SCE_MAXIMA_COMMENT = 2;
    public const int SCE_MAXIMA_NUMBER = 3;
    public const int SCE_MAXIMA_STRING = 4;
    public const int SCE_MAXIMA_COMMAND = 5;
    public const int SCE_MAXIMA_VARIABLE = 6;
    public const int SCE_MAXIMA_UNKNOWN = 7;
    
    // Sol
    // ===
    public const int SCE_SCRIPTOL_DEFAULT = 0;
    public const int SCE_SCRIPTOL_WHITE = 1;
    public const int SCE_SCRIPTOL_COMMENTLINE = 2;
    public const int SCE_SCRIPTOL_PERSISTENT = 3;
    public const int SCE_SCRIPTOL_CSTYLE = 4;
    public const int SCE_SCRIPTOL_COMMENTBLOCK = 5;
    public const int SCE_SCRIPTOL_NUMBER = 6;
    public const int SCE_SCRIPTOL_STRING = 7;
    public const int SCE_SCRIPTOL_CHARACTER = 8;
    public const int SCE_SCRIPTOL_STRINGEOL = 9;
    public const int SCE_SCRIPTOL_KEYWORD = 10;
    public const int SCE_SCRIPTOL_OPERATOR = 11;
    public const int SCE_SCRIPTOL_IDENTIFIER = 12;
    public const int SCE_SCRIPTOL_TRIPLE = 13;
    public const int SCE_SCRIPTOL_CLASSNAME = 14;
    public const int SCE_SCRIPTOL_PREPROCESSOR = 15;
    
    // Asm
    // As
    // ===
    public const int SCE_ASM_DEFAULT = 0;
    public const int SCE_ASM_COMMENT = 1;
    public const int SCE_ASM_NUMBER = 2;
    public const int SCE_ASM_STRING = 3;
    public const int SCE_ASM_OPERATOR = 4;
    public const int SCE_ASM_IDENTIFIER = 5;
    public const int SCE_ASM_CPUINSTRUCTION = 6;
    public const int SCE_ASM_MATHINSTRUCTION = 7;
    public const int SCE_ASM_REGISTER = 8;
    public const int SCE_ASM_DIRECTIVE = 9;
    public const int SCE_ASM_DIRECTIVEOPERAND = 10;
    public const int SCE_ASM_COMMENTBLOCK = 11;
    public const int SCE_ASM_CHARACTER = 12;
    public const int SCE_ASM_STRINGEOL = 13;
    public const int SCE_ASM_EXTINSTRUCTION = 14;
    public const int SCE_ASM_COMMENTDIRECTIVE = 15;
    
    // Fortran
    // F77
    // =======
    public const int SCE_F_DEFAULT = 0;
    public const int SCE_F_COMMENT = 1;
    public const int SCE_F_NUMBER = 2;
    public const int SCE_F_STRING1 = 3;
    public const int SCE_F_STRING2 = 4;
    public const int SCE_F_STRINGEOL = 5;
    public const int SCE_F_OPERATOR = 6;
    public const int SCE_F_IDENTIFIER = 7;
    public const int SCE_F_WORD = 8;
    public const int SCE_F_WORD2 = 9;
    public const int SCE_F_WORD3 = 10;
    public const int SCE_F_PREPROCESSOR = 11;
    public const int SCE_F_OPERATOR2 = 12;
    public const int SCE_F_LABEL = 13;
    public const int SCE_F_CONTINUATION = 14;
    
    // CSS
    // ===
    public const int SCE_CSS_DEFAULT = 0;
    public const int SCE_CSS_TAG = 1;
    public const int SCE_CSS_CLASS = 2;
    public const int SCE_CSS_PSEUDOCLASS = 3;
    public const int SCE_CSS_UNKNOWN_PSEUDOCLASS = 4;
    public const int SCE_CSS_OPERATOR = 5;
    public const int SCE_CSS_IDENTIFIER = 6;
    public const int SCE_CSS_UNKNOWN_IDENTIFIER = 7;
    public const int SCE_CSS_VALUE = 8;
    public const int SCE_CSS_COMMENT = 9;
    public const int SCE_CSS_ID = 10;
    public const int SCE_CSS_IMPORTANT = 11;
    public const int SCE_CSS_DIRECTIVE = 12;
    public const int SCE_CSS_DOUBLESTRING = 13;
    public const int SCE_CSS_SINGLESTRING = 14;
    public const int SCE_CSS_IDENTIFIER2 = 15;
    public const int SCE_CSS_ATTRIBUTE = 16;
    public const int SCE_CSS_IDENTIFIER3 = 17;
    public const int SCE_CSS_PSEUDOELEMENT = 18;
    public const int SCE_CSS_EXTENDED_IDENTIFIER = 19;
    public const int SCE_CSS_EXTENDED_PSEUDOCLASS = 20;
    public const int SCE_CSS_EXTENDED_PSEUDOELEMENT = 21;
    public const int SCE_CSS_GROUP_RULE = 22;
    public const int SCE_CSS_VARIABLE = 23;
    
    // POV
    // ===
    public const int SCE_POV_DEFAULT = 0;
    public const int SCE_POV_COMMENT = 1;
    public const int SCE_POV_COMMENTLINE = 2;
    public const int SCE_POV_NUMBER = 3;
    public const int SCE_POV_OPERATOR = 4;
    public const int SCE_POV_IDENTIFIER = 5;
    public const int SCE_POV_STRING = 6;
    public const int SCE_POV_STRINGEOL = 7;
    public const int SCE_POV_DIRECTIVE = 8;
    public const int SCE_POV_BADDIRECTIVE = 9;
    public const int SCE_POV_WORD2 = 10;
    public const int SCE_POV_WORD3 = 11;
    public const int SCE_POV_WORD4 = 12;
    public const int SCE_POV_WORD5 = 13;
    public const int SCE_POV_WORD6 = 14;
    public const int SCE_POV_WORD7 = 15;
    public const int SCE_POV_WORD8 = 16;
    
    // LOUT
    // ====
    public const int SCE_LOUT_DEFAULT = 0;
    public const int SCE_LOUT_COMMENT = 1;
    public const int SCE_LOUT_NUMBER = 2;
    public const int SCE_LOUT_WORD = 3;
    public const int SCE_LOUT_WORD2 = 4;
    public const int SCE_LOUT_WORD3 = 5;
    public const int SCE_LOUT_WORD4 = 6;
    public const int SCE_LOUT_STRING = 7;
    public const int SCE_LOUT_OPERATOR = 8;
    public const int SCE_LOUT_IDENTIFIER = 9;
    public const int SCE_LOUT_STRINGEOL = 10;
    
    // ESCRIPT
    // =======
    public const int SCE_ESCRIPT_DEFAULT = 0;
    public const int SCE_ESCRIPT_COMMENT = 1;
    public const int SCE_ESCRIPT_COMMENTLINE = 2;
    public const int SCE_ESCRIPT_COMMENTDOC = 3;
    public const int SCE_ESCRIPT_NUMBER = 4;
    public const int SCE_ESCRIPT_WORD = 5;
    public const int SCE_ESCRIPT_STRING = 6;
    public const int SCE_ESCRIPT_OPERATOR = 7;
    public const int SCE_ESCRIPT_IDENTIFIER = 8;
    public const int SCE_ESCRIPT_BRACE = 9;
    public const int SCE_ESCRIPT_WORD2 = 10;
    public const int SCE_ESCRIPT_WORD3 = 11;
    
    // PS
    // ===
    public const int SCE_PS_DEFAULT = 0;
    public const int SCE_PS_COMMENT = 1;
    public const int SCE_PS_DSC_COMMENT = 2;
    public const int SCE_PS_DSC_VALUE = 3;
    public const int SCE_PS_NUMBER = 4;
    public const int SCE_PS_NAME = 5;
    public const int SCE_PS_KEYWORD = 6;
    public const int SCE_PS_LITERAL = 7;
    public const int SCE_PS_IMMEVAL = 8;
    public const int SCE_PS_PAREN_ARRAY = 9;
    public const int SCE_PS_PAREN_DICT = 10;
    public const int SCE_PS_PAREN_PROC = 11;
    public const int SCE_PS_TEXT = 12;
    public const int SCE_PS_HEXSTRING = 13;
    public const int SCE_PS_BASE85STRING = 14;
    public const int SCE_PS_BADSTRINGCHAR = 15;
    
    // NSIS
    // ====
    public const int SCE_NSIS_DEFAULT = 0;
    public const int SCE_NSIS_COMMENT = 1;
    public const int SCE_NSIS_STRINGDQ = 2;
    public const int SCE_NSIS_STRINGLQ = 3;
    public const int SCE_NSIS_STRINGRQ = 4;
    public const int SCE_NSIS_FUNCTION = 5;
    public const int SCE_NSIS_VARIABLE = 6;
    public const int SCE_NSIS_LABEL = 7;
    public const int SCE_NSIS_USERDEFINED = 8;
    public const int SCE_NSIS_SECTIONDEF = 9;
    public const int SCE_NSIS_SUBSECTIONDEF = 10;
    public const int SCE_NSIS_IFDEFINEDEF = 11;
    public const int SCE_NSIS_MACRODEF = 12;
    public const int SCE_NSIS_STRINGVAR = 13;
    public const int SCE_NSIS_NUMBER = 14;
    public const int SCE_NSIS_SECTIONGROUP = 15;
    public const int SCE_NSIS_PAGEEX = 16;
    public const int SCE_NSIS_FUNCTIONDEF = 17;
    public const int SCE_NSIS_COMMENTBOX = 18;
    
    // MMIXAL
    // ======
    public const int SCE_MMIXAL_LEADWS = 0;
    public const int SCE_MMIXAL_COMMENT = 1;
    public const int SCE_MMIXAL_LABEL = 2;
    public const int SCE_MMIXAL_OPCODE = 3;
    public const int SCE_MMIXAL_OPCODE_PRE = 4;
    public const int SCE_MMIXAL_OPCODE_VALID = 5;
    public const int SCE_MMIXAL_OPCODE_UNKNOWN = 6;
    public const int SCE_MMIXAL_OPCODE_POST = 7;
    public const int SCE_MMIXAL_OPERANDS = 8;
    public const int SCE_MMIXAL_NUMBER = 9;
    public const int SCE_MMIXAL_REF = 10;
    public const int SCE_MMIXAL_CHAR = 11;
    public const int SCE_MMIXAL_STRING = 12;
    public const int SCE_MMIXAL_REGISTER = 13;
    public const int SCE_MMIXAL_HEX = 14;
    public const int SCE_MMIXAL_OPERATOR = 15;
    public const int SCE_MMIXAL_SYMBOL = 16;
    public const int SCE_MMIXAL_INCLUDE = 17;
    
    // Clarion
    // =======
    public const int SCE_CLW_DEFAULT = 0;
    public const int SCE_CLW_LABEL = 1;
    public const int SCE_CLW_COMMENT = 2;
    public const int SCE_CLW_STRING = 3;
    public const int SCE_CLW_USER_IDENTIFIER = 4;
    public const int SCE_CLW_INTEGER_CONSTANT = 5;
    public const int SCE_CLW_REAL_CONSTANT = 6;
    public const int SCE_CLW_PICTURE_STRING = 7;
    public const int SCE_CLW_KEYWORD = 8;
    public const int SCE_CLW_COMPILER_DIRECTIVE = 9;
    public const int SCE_CLW_RUNTIME_EXPRESSIONS = 10;
    public const int SCE_CLW_BUILTIN_PROCEDURES_FUNCTION = 11;
    public const int SCE_CLW_STRUCTURE_DATA_TYPE = 12;
    public const int SCE_CLW_ATTRIBUTE = 13;
    public const int SCE_CLW_STANDARD_EQUATE = 14;
    public const int SCE_CLW_ERROR = 15;
    public const int SCE_CLW_DEPRECATED = 16;
    
    // LOT
    // ===
    public const int SCE_LOT_DEFAULT = 0;
    public const int SCE_LOT_HEADER = 1;
    public const int SCE_LOT_BREAK = 2;
    public const int SCE_LOT_SET = 3;
    public const int SCE_LOT_PASS = 4;
    public const int SCE_LOT_FAIL = 5;
    public const int SCE_LOT_ABORT = 6;
    
    // YAML
    // ====
    public const int SCE_YAML_DEFAULT = 0;
    public const int SCE_YAML_COMMENT = 1;
    public const int SCE_YAML_IDENTIFIER = 2;
    public const int SCE_YAML_KEYWORD = 3;
    public const int SCE_YAML_NUMBER = 4;
    public const int SCE_YAML_REFERENCE = 5;
    public const int SCE_YAML_DOCUMENT = 6;
    public const int SCE_YAML_TEXT = 7;
    public const int SCE_YAML_ERROR = 8;
    public const int SCE_YAML_OPERATOR = 9;
    
    // TeX
    // ===
    public const int SCE_TEX_DEFAULT = 0;
    public const int SCE_TEX_SPECIAL = 1;
    public const int SCE_TEX_GROUP = 2;
    public const int SCE_TEX_SYMBOL = 3;
    public const int SCE_TEX_COMMAND = 4;
    public const int SCE_TEX_TEXT = 5;
    
    // Metapost
    // ========
    public const int SCE_METAPOST_DEFAULT = 0;
    public const int SCE_METAPOST_SPECIAL = 1;
    public const int SCE_METAPOST_GROUP = 2;
    public const int SCE_METAPOST_SYMBOL = 3;
    public const int SCE_METAPOST_COMMAND = 4;
    public const int SCE_METAPOST_TEXT = 5;
    public const int SCE_METAPOST_EXTRA = 6;
    
    // Erlang
    // ======
    public const int SCE_ERLANG_DEFAULT = 0;
    public const int SCE_ERLANG_COMMENT = 1;
    public const int SCE_ERLANG_VARIABLE = 2;
    public const int SCE_ERLANG_NUMBER = 3;
    public const int SCE_ERLANG_KEYWORD = 4;
    public const int SCE_ERLANG_STRING = 5;
    public const int SCE_ERLANG_OPERATOR = 6;
    public const int SCE_ERLANG_ATOM = 7;
    public const int SCE_ERLANG_FUNCTION_NAME = 8;
    public const int SCE_ERLANG_CHARACTER = 9;
    public const int SCE_ERLANG_MACRO = 10;
    public const int SCE_ERLANG_RECORD = 11;
    public const int SCE_ERLANG_PREPROC = 12;
    public const int SCE_ERLANG_NODE_NAME = 13;
    public const int SCE_ERLANG_COMMENT_FUNCTION = 14;
    public const int SCE_ERLANG_COMMENT_MODULE = 15;
    public const int SCE_ERLANG_COMMENT_DOC = 16;
    public const int SCE_ERLANG_COMMENT_DOC_MACRO = 17;
    public const int SCE_ERLANG_ATOM_QUOTED = 18;
    public const int SCE_ERLANG_MACRO_QUOTED = 19;
    public const int SCE_ERLANG_RECORD_QUOTED = 20;
    public const int SCE_ERLANG_NODE_NAME_QUOTED = 21;
    public const int SCE_ERLANG_BIFS = 22;
    public const int SCE_ERLANG_MODULES = 23;
    public const int SCE_ERLANG_MODULES_ATT = 24;
    public const int SCE_ERLANG_UNKNOWN = 31;
    
    // Julia
    // =====
    public const int SCE_JULIA_DEFAULT = 0;
    public const int SCE_JULIA_COMMENT = 1;
    public const int SCE_JULIA_NUMBER = 2;
    public const int SCE_JULIA_KEYWORD1 = 3;
    public const int SCE_JULIA_KEYWORD2 = 4;
    public const int SCE_JULIA_KEYWORD3 = 5;
    public const int SCE_JULIA_CHAR = 6;
    public const int SCE_JULIA_OPERATOR = 7;
    public const int SCE_JULIA_BRACKET = 8;
    public const int SCE_JULIA_IDENTIFIER = 9;
    public const int SCE_JULIA_STRING = 10;
    public const int SCE_JULIA_SYMBOL = 11;
    public const int SCE_JULIA_MACRO = 12;
    public const int SCE_JULIA_STRINGINTERP = 13;
    public const int SCE_JULIA_DOCSTRING = 14;
    public const int SCE_JULIA_STRINGLITERAL = 15;
    public const int SCE_JULIA_COMMAND = 16;
    public const int SCE_JULIA_COMMANDLITERAL = 17;
    public const int SCE_JULIA_TYPEANNOT = 18;
    public const int SCE_JULIA_LEXERROR = 19;
    public const int SCE_JULIA_KEYWORD4 = 20;
    public const int SCE_JULIA_TYPEOPERATOR = 21;
    
    // MSSQL
    // =====
    public const int SCE_MSSQL_DEFAULT = 0;
    public const int SCE_MSSQL_COMMENT = 1;
    public const int SCE_MSSQL_LINE_COMMENT = 2;
    public const int SCE_MSSQL_NUMBER = 3;
    public const int SCE_MSSQL_STRING = 4;
    public const int SCE_MSSQL_OPERATOR = 5;
    public const int SCE_MSSQL_IDENTIFIER = 6;
    public const int SCE_MSSQL_VARIABLE = 7;
    public const int SCE_MSSQL_COLUMN_NAME = 8;
    public const int SCE_MSSQL_STATEMENT = 9;
    public const int SCE_MSSQL_DATATYPE = 10;
    public const int SCE_MSSQL_SYSTABLE = 11;
    public const int SCE_MSSQL_GLOBAL_VARIABLE = 12;
    public const int SCE_MSSQL_FUNCTION = 13;
    public const int SCE_MSSQL_STORED_PROCEDURE = 14;
    public const int SCE_MSSQL_DEFAULT_PREF_DATATYPE = 15;
    public const int SCE_MSSQL_COLUMN_NAME_2 = 16;
    
    // Verilog
    // =======
    public const int SCE_V_DEFAULT = 0;
    public const int SCE_V_COMMENT = 1;
    public const int SCE_V_COMMENTLINE = 2;
    public const int SCE_V_COMMENTLINEBANG = 3;
    public const int SCE_V_NUMBER = 4;
    public const int SCE_V_WORD = 5;
    public const int SCE_V_STRING = 6;
    public const int SCE_V_WORD2 = 7;
    public const int SCE_V_WORD3 = 8;
    public const int SCE_V_PREPROCESSOR = 9;
    public const int SCE_V_OPERATOR = 10;
    public const int SCE_V_IDENTIFIER = 11;
    public const int SCE_V_STRINGEOL = 12;
    public const int SCE_V_USER = 19;
    public const int SCE_V_COMMENT_WORD = 20;
    public const int SCE_V_INPUT = 21;
    public const int SCE_V_OUTPUT = 22;
    public const int SCE_V_INOUT = 23;
    public const int SCE_V_PORT_CONNECT = 24;
    
    // Kix
    // ===
    public const int SCE_KIX_DEFAULT = 0;
    public const int SCE_KIX_COMMENT = 1;
    public const int SCE_KIX_STRING1 = 2;
    public const int SCE_KIX_STRING2 = 3;
    public const int SCE_KIX_NUMBER = 4;
    public const int SCE_KIX_VAR = 5;
    public const int SCE_KIX_MACRO = 6;
    public const int SCE_KIX_KEYWORD = 7;
    public const int SCE_KIX_FUNCTIONS = 8;
    public const int SCE_KIX_OPERATOR = 9;
    public const int SCE_KIX_COMMENTSTREAM = 10;
    public const int SCE_KIX_IDENTIFIER = 31;
    
    // Gui4Cli
    // =======
    public const int SCE_GC_DEFAULT = 0;
    public const int SCE_GC_COMMENTLINE = 1;
    public const int SCE_GC_COMMENTBLOCK = 2;
    public const int SCE_GC_GLOBAL = 3;
    public const int SCE_GC_EVENT = 4;
    public const int SCE_GC_ATTRIBUTE = 5;
    public const int SCE_GC_CONTROL = 6;
    public const int SCE_GC_COMMAND = 7;
    public const int SCE_GC_STRING = 8;
    public const int SCE_GC_OPERATOR = 9;
    
    // Specman
    // =======
    public const int SCE_SN_DEFAULT = 0;
    public const int SCE_SN_CODE = 1;
    public const int SCE_SN_COMMENTLINE = 2;
    public const int SCE_SN_COMMENTLINEBANG = 3;
    public const int SCE_SN_NUMBER = 4;
    public const int SCE_SN_WORD = 5;
    public const int SCE_SN_STRING = 6;
    public const int SCE_SN_WORD2 = 7;
    public const int SCE_SN_WORD3 = 8;
    public const int SCE_SN_PREPROCESSOR = 9;
    public const int SCE_SN_OPERATOR = 10;
    public const int SCE_SN_IDENTIFIER = 11;
    public const int SCE_SN_STRINGEOL = 12;
    public const int SCE_SN_REGEXTAG = 13;
    public const int SCE_SN_SIGNAL = 14;
    public const int SCE_SN_USER = 19;
    
    // Au3
    // ===
    public const int SCE_AU3_DEFAULT = 0;
    public const int SCE_AU3_COMMENT = 1;
    public const int SCE_AU3_COMMENTBLOCK = 2;
    public const int SCE_AU3_NUMBER = 3;
    public const int SCE_AU3_FUNCTION = 4;
    public const int SCE_AU3_KEYWORD = 5;
    public const int SCE_AU3_MACRO = 6;
    public const int SCE_AU3_STRING = 7;
    public const int SCE_AU3_OPERATOR = 8;
    public const int SCE_AU3_VARIABLE = 9;
    public const int SCE_AU3_SENT = 10;
    public const int SCE_AU3_PREPROCESSOR = 11;
    public const int SCE_AU3_SPECIAL = 12;
    public const int SCE_AU3_EXPAND = 13;
    public const int SCE_AU3_COMOBJ = 14;
    public const int SCE_AU3_UDF = 15;
    
    // APDL
    // ====
    public const int SCE_APDL_DEFAULT = 0;
    public const int SCE_APDL_COMMENT = 1;
    public const int SCE_APDL_COMMENTBLOCK = 2;
    public const int SCE_APDL_NUMBER = 3;
    public const int SCE_APDL_STRING = 4;
    public const int SCE_APDL_OPERATOR = 5;
    public const int SCE_APDL_WORD = 6;
    public const int SCE_APDL_PROCESSOR = 7;
    public const int SCE_APDL_COMMAND = 8;
    public const int SCE_APDL_SLASHCOMMAND = 9;
    public const int SCE_APDL_STARCOMMAND = 10;
    public const int SCE_APDL_ARGUMENT = 11;
    public const int SCE_APDL_FUNCTION = 12;
    
    // Bash
    // ====
    public const int SCE_SH_DEFAULT = 0;
    public const int SCE_SH_ERROR = 1;
    public const int SCE_SH_COMMENTLINE = 2;
    public const int SCE_SH_NUMBER = 3;
    public const int SCE_SH_WORD = 4;
    public const int SCE_SH_STRING = 5;
    public const int SCE_SH_CHARACTER = 6;
    public const int SCE_SH_OPERATOR = 7;
    public const int SCE_SH_IDENTIFIER = 8;
    public const int SCE_SH_SCALAR = 9;
    public const int SCE_SH_PARAM = 10;
    public const int SCE_SH_BACKTICKS = 11;
    public const int SCE_SH_HERE_DELIM = 12;
    public const int SCE_SH_HERE_Q = 13;
    
    // Asn1
    // ====
    public const int SCE_ASN1_DEFAULT = 0;
    public const int SCE_ASN1_COMMENT = 1;
    public const int SCE_ASN1_IDENTIFIER = 2;
    public const int SCE_ASN1_STRING = 3;
    public const int SCE_ASN1_OID = 4;
    public const int SCE_ASN1_SCALAR = 5;
    public const int SCE_ASN1_KEYWORD = 6;
    public const int SCE_ASN1_ATTRIBUTE = 7;
    public const int SCE_ASN1_DESCRIPTOR = 8;
    public const int SCE_ASN1_TYPE = 9;
    public const int SCE_ASN1_OPERATOR = 10;
    
    // VHDL
    // ====
    public const int SCE_VHDL_DEFAULT = 0;
    public const int SCE_VHDL_COMMENT = 1;
    public const int SCE_VHDL_COMMENTLINEBANG = 2;
    public const int SCE_VHDL_NUMBER = 3;
    public const int SCE_VHDL_STRING = 4;
    public const int SCE_VHDL_OPERATOR = 5;
    public const int SCE_VHDL_IDENTIFIER = 6;
    public const int SCE_VHDL_STRINGEOL = 7;
    public const int SCE_VHDL_KEYWORD = 8;
    public const int SCE_VHDL_STDOPERATOR = 9;
    public const int SCE_VHDL_ATTRIBUTE = 10;
    public const int SCE_VHDL_STDFUNCTION = 11;
    public const int SCE_VHDL_STDPACKAGE = 12;
    public const int SCE_VHDL_STDTYPE = 13;
    public const int SCE_VHDL_USERWORD = 14;
    public const int SCE_VHDL_BLOCK_COMMENT = 15;
    
    // Caml
    // ====
    public const int SCE_CAML_DEFAULT = 0;
    public const int SCE_CAML_IDENTIFIER = 1;
    public const int SCE_CAML_TAGNAME = 2;
    public const int SCE_CAML_KEYWORD = 3;
    public const int SCE_CAML_KEYWORD2 = 4;
    public const int SCE_CAML_KEYWORD3 = 5;
    public const int SCE_CAML_LINENUM = 6;
    public const int SCE_CAML_OPERATOR = 7;
    public const int SCE_CAML_NUMBER = 8;
    public const int SCE_CAML_CHAR = 9;
    public const int SCE_CAML_WHITE = 10;
    public const int SCE_CAML_STRING = 11;
    public const int SCE_CAML_COMMENT = 12;
    public const int SCE_CAML_COMMENT1 = 13;
    public const int SCE_CAML_COMMENT2 = 14;
    public const int SCE_CAML_COMMENT3 = 15;
    
    // Haskell
    // =======
    public const int SCE_HA_DEFAULT = 0;
    public const int SCE_HA_IDENTIFIER = 1;
    public const int SCE_HA_KEYWORD = 2;
    public const int SCE_HA_NUMBER = 3;
    public const int SCE_HA_STRING = 4;
    public const int SCE_HA_CHARACTER = 5;
    public const int SCE_HA_CLASS = 6;
    public const int SCE_HA_MODULE = 7;
    public const int SCE_HA_CAPITAL = 8;
    public const int SCE_HA_DATA = 9;
    public const int SCE_HA_IMPORT = 10;
    public const int SCE_HA_OPERATOR = 11;
    public const int SCE_HA_INSTANCE = 12;
    public const int SCE_HA_COMMENTLINE = 13;
    public const int SCE_HA_COMMENTBLOCK = 14;
    public const int SCE_HA_COMMENTBLOCK2 = 15;
    public const int SCE_HA_COMMENTBLOCK3 = 16;
    public const int SCE_HA_PRAGMA = 17;
    public const int SCE_HA_PREPROCESSOR = 18;
    public const int SCE_HA_STRINGEOL = 19;
    public const int SCE_HA_RESERVED_OPERATOR = 20;
    public const int SCE_HA_LITERATE_COMMENT = 21;
    public const int SCE_HA_LITERATE_CODEDELIM = 22;
    
    // TADS3
    // =====
    public const int SCE_T3_DEFAULT = 0;
    public const int SCE_T3_X_DEFAULT = 1;
    public const int SCE_T3_PREPROCESSOR = 2;
    public const int SCE_T3_BLOCK_COMMENT = 3;
    public const int SCE_T3_LINE_COMMENT = 4;
    public const int SCE_T3_OPERATOR = 5;
    public const int SCE_T3_KEYWORD = 6;
    public const int SCE_T3_NUMBER = 7;
    public const int SCE_T3_IDENTIFIER = 8;
    public const int SCE_T3_S_STRING = 9;
    public const int SCE_T3_D_STRING = 10;
    public const int SCE_T3_X_STRING = 11;
    public const int SCE_T3_LIB_DIRECTIVE = 12;
    public const int SCE_T3_MSG_PARAM = 13;
    public const int SCE_T3_HTML_TAG = 14;
    public const int SCE_T3_HTML_DEFAULT = 15;
    public const int SCE_T3_HTML_STRING = 16;
    public const int SCE_T3_USER1 = 17;
    public const int SCE_T3_USER2 = 18;
    public const int SCE_T3_USER3 = 19;
    public const int SCE_T3_BRACE = 20;
    
    // Rebol
    // =====
    public const int SCE_REBOL_DEFAULT = 0;
    public const int SCE_REBOL_COMMENTLINE = 1;
    public const int SCE_REBOL_COMMENTBLOCK = 2;
    public const int SCE_REBOL_PREFACE = 3;
    public const int SCE_REBOL_OPERATOR = 4;
    public const int SCE_REBOL_CHARACTER = 5;
    public const int SCE_REBOL_QUOTEDSTRING = 6;
    public const int SCE_REBOL_BRACEDSTRING = 7;
    public const int SCE_REBOL_NUMBER = 8;
    public const int SCE_REBOL_PAIR = 9;
    public const int SCE_REBOL_TUPLE = 10;
    public const int SCE_REBOL_BINARY = 11;
    public const int SCE_REBOL_MONEY = 12;
    public const int SCE_REBOL_ISSUE = 13;
    public const int SCE_REBOL_TAG = 14;
    public const int SCE_REBOL_FILE = 15;
    public const int SCE_REBOL_EMAIL = 16;
    public const int SCE_REBOL_URL = 17;
    public const int SCE_REBOL_DATE = 18;
    public const int SCE_REBOL_TIME = 19;
    public const int SCE_REBOL_IDENTIFIER = 20;
    public const int SCE_REBOL_WORD = 21;
    public const int SCE_REBOL_WORD2 = 22;
    public const int SCE_REBOL_WORD3 = 23;
    public const int SCE_REBOL_WORD4 = 24;
    public const int SCE_REBOL_WORD5 = 25;
    public const int SCE_REBOL_WORD6 = 26;
    public const int SCE_REBOL_WORD7 = 27;
    public const int SCE_REBOL_WORD8 = 28;
    
    // SQL
    // ===
    public const int SCE_SQL_DEFAULT = 0;
    public const int SCE_SQL_COMMENT = 1;
    public const int SCE_SQL_COMMENTLINE = 2;
    public const int SCE_SQL_COMMENTDOC = 3;
    public const int SCE_SQL_NUMBER = 4;
    public const int SCE_SQL_WORD = 5;
    public const int SCE_SQL_STRING = 6;
    public const int SCE_SQL_CHARACTER = 7;
    public const int SCE_SQL_SQLPLUS = 8;
    public const int SCE_SQL_SQLPLUS_PROMPT = 9;
    public const int SCE_SQL_OPERATOR = 10;
    public const int SCE_SQL_IDENTIFIER = 11;
    public const int SCE_SQL_SQLPLUS_COMMENT = 13;
    public const int SCE_SQL_COMMENTLINEDOC = 15;
    public const int SCE_SQL_WORD2 = 16;
    public const int SCE_SQL_COMMENTDOCKEYWORD = 17;
    public const int SCE_SQL_COMMENTDOCKEYWORDERROR = 18;
    public const int SCE_SQL_USER1 = 19;
    public const int SCE_SQL_USER2 = 20;
    public const int SCE_SQL_USER3 = 21;
    public const int SCE_SQL_USER4 = 22;
    public const int SCE_SQL_QUOTEDIDENTIFIER = 23;
    public const int SCE_SQL_QOPERATOR = 24;
    
    // Smalltalk
    // =========
    public const int SCE_ST_DEFAULT = 0;
    public const int SCE_ST_STRING = 1;
    public const int SCE_ST_NUMBER = 2;
    public const int SCE_ST_COMMENT = 3;
    public const int SCE_ST_SYMBOL = 4;
    public const int SCE_ST_BINARY = 5;
    public const int SCE_ST_BOOL = 6;
    public const int SCE_ST_SELF = 7;
    public const int SCE_ST_SUPER = 8;
    public const int SCE_ST_NIL = 9;
    public const int SCE_ST_GLOBAL = 10;
    public const int SCE_ST_RETURN = 11;
    public const int SCE_ST_SPECIAL = 12;
    public const int SCE_ST_KWSEND = 13;
    public const int SCE_ST_ASSIGN = 14;
    public const int SCE_ST_CHARACTER = 15;
    public const int SCE_ST_SPEC_SEL = 16;
    
    // FlagShip
    // ========
    public const int SCE_FS_DEFAULT = 0;
    public const int SCE_FS_COMMENT = 1;
    public const int SCE_FS_COMMENTLINE = 2;
    public const int SCE_FS_COMMENTDOC = 3;
    public const int SCE_FS_COMMENTLINEDOC = 4;
    public const int SCE_FS_COMMENTDOCKEYWORD = 5;
    public const int SCE_FS_COMMENTDOCKEYWORDERROR = 6;
    public const int SCE_FS_KEYWORD = 7;
    public const int SCE_FS_KEYWORD2 = 8;
    public const int SCE_FS_KEYWORD3 = 9;
    public const int SCE_FS_KEYWORD4 = 10;
    public const int SCE_FS_NUMBER = 11;
    public const int SCE_FS_STRING = 12;
    public const int SCE_FS_PREPROCESSOR = 13;
    public const int SCE_FS_OPERATOR = 14;
    public const int SCE_FS_IDENTIFIER = 15;
    public const int SCE_FS_DATE = 16;
    public const int SCE_FS_STRINGEOL = 17;
    public const int SCE_FS_CONSTANT = 18;
    public const int SCE_FS_WORDOPERATOR = 19;
    public const int SCE_FS_DISABLEDCODE = 20;
    public const int SCE_FS_DEFAULT_C = 21;
    public const int SCE_FS_COMMENTDOC_C = 22;
    public const int SCE_FS_COMMENTLINEDOC_C = 23;
    public const int SCE_FS_KEYWORD_C = 24;
    public const int SCE_FS_KEYWORD2_C = 25;
    public const int SCE_FS_NUMBER_C = 26;
    public const int SCE_FS_STRING_C = 27;
    public const int SCE_FS_PREPROCESSOR_C = 28;
    public const int SCE_FS_OPERATOR_C = 29;
    public const int SCE_FS_IDENTIFIER_C = 30;
    public const int SCE_FS_STRINGEOL_C = 31;
    
    // Csound
    // ======
    public const int SCE_CSOUND_DEFAULT = 0;
    public const int SCE_CSOUND_COMMENT = 1;
    public const int SCE_CSOUND_NUMBER = 2;
    public const int SCE_CSOUND_OPERATOR = 3;
    public const int SCE_CSOUND_INSTR = 4;
    public const int SCE_CSOUND_IDENTIFIER = 5;
    public const int SCE_CSOUND_OPCODE = 6;
    public const int SCE_CSOUND_HEADERSTMT = 7;
    public const int SCE_CSOUND_USERKEYWORD = 8;
    public const int SCE_CSOUND_COMMENTBLOCK = 9;
    public const int SCE_CSOUND_PARAM = 10;
    public const int SCE_CSOUND_ARATE_VAR = 11;
    public const int SCE_CSOUND_KRATE_VAR = 12;
    public const int SCE_CSOUND_IRATE_VAR = 13;
    public const int SCE_CSOUND_GLOBAL_VAR = 14;
    public const int SCE_CSOUND_STRINGEOL = 15;
    
    // Inno
    // ====
    public const int SCE_INNO_DEFAULT = 0;
    public const int SCE_INNO_COMMENT = 1;
    public const int SCE_INNO_KEYWORD = 2;
    public const int SCE_INNO_PARAMETER = 3;
    public const int SCE_INNO_SECTION = 4;
    public const int SCE_INNO_PREPROC = 5;
    public const int SCE_INNO_INLINE_EXPANSION = 6;
    public const int SCE_INNO_COMMENT_PASCAL = 7;
    public const int SCE_INNO_KEYWORD_PASCAL = 8;
    public const int SCE_INNO_KEYWORD_USER = 9;
    public const int SCE_INNO_STRING_DOUBLE = 10;
    public const int SCE_INNO_STRING_SINGLE = 11;
    public const int SCE_INNO_IDENTIFIER = 12;
    
    // Opal
    // ====
    public const int SCE_OPAL_SPACE = 0;
    public const int SCE_OPAL_COMMENT_BLOCK = 1;
    public const int SCE_OPAL_COMMENT_LINE = 2;
    public const int SCE_OPAL_INTEGER = 3;
    public const int SCE_OPAL_KEYWORD = 4;
    public const int SCE_OPAL_SORT = 5;
    public const int SCE_OPAL_STRING = 6;
    public const int SCE_OPAL_PAR = 7;
    public const int SCE_OPAL_BOOL_CONST = 8;
    public const int SCE_OPAL_DEFAULT = 32;
    
    // Spice
    // =====
    public const int SCE_SPICE_DEFAULT = 0;
    public const int SCE_SPICE_IDENTIFIER = 1;
    public const int SCE_SPICE_KEYWORD = 2;
    public const int SCE_SPICE_KEYWORD2 = 3;
    public const int SCE_SPICE_KEYWORD3 = 4;
    public const int SCE_SPICE_NUMBER = 5;
    public const int SCE_SPICE_DELIMITER = 6;
    public const int SCE_SPICE_VALUE = 7;
    public const int SCE_SPICE_COMMENTLINE = 8;
    
    // CMAKE
    // =====
    public const int SCE_CMAKE_DEFAULT = 0;
    public const int SCE_CMAKE_COMMENT = 1;
    public const int SCE_CMAKE_STRINGDQ = 2;
    public const int SCE_CMAKE_STRINGLQ = 3;
    public const int SCE_CMAKE_STRINGRQ = 4;
    public const int SCE_CMAKE_COMMANDS = 5;
    public const int SCE_CMAKE_PARAMETERS = 6;
    public const int SCE_CMAKE_VARIABLE = 7;
    public const int SCE_CMAKE_USERDEFINED = 8;
    public const int SCE_CMAKE_WHILEDEF = 9;
    public const int SCE_CMAKE_FOREACHDEF = 10;
    public const int SCE_CMAKE_IFDEFINEDEF = 11;
    public const int SCE_CMAKE_MACRODEF = 12;
    public const int SCE_CMAKE_STRINGVAR = 13;
    public const int SCE_CMAKE_NUMBER = 14;
    
    // Gap
    // ===
    public const int SCE_GAP_DEFAULT = 0;
    public const int SCE_GAP_IDENTIFIER = 1;
    public const int SCE_GAP_KEYWORD = 2;
    public const int SCE_GAP_KEYWORD2 = 3;
    public const int SCE_GAP_KEYWORD3 = 4;
    public const int SCE_GAP_KEYWORD4 = 5;
    public const int SCE_GAP_STRING = 6;
    public const int SCE_GAP_CHAR = 7;
    public const int SCE_GAP_OPERATOR = 8;
    public const int SCE_GAP_COMMENT = 9;
    public const int SCE_GAP_NUMBER = 10;
    public const int SCE_GAP_STRINGEOL = 11;
    
    // PLM
    // ===
    public const int SCE_PLM_DEFAULT = 0;
    public const int SCE_PLM_COMMENT = 1;
    public const int SCE_PLM_STRING = 2;
    public const int SCE_PLM_NUMBER = 3;
    public const int SCE_PLM_IDENTIFIER = 4;
    public const int SCE_PLM_OPERATOR = 5;
    public const int SCE_PLM_CONTROL = 6;
    public const int SCE_PLM_KEYWORD = 7;
    
    // Progress
    // ========
    public const int SCE_ABL_DEFAULT = 0;
    public const int SCE_ABL_NUMBER = 1;
    public const int SCE_ABL_WORD = 2;
    public const int SCE_ABL_STRING = 3;
    public const int SCE_ABL_CHARACTER = 4;
    public const int SCE_ABL_PREPROCESSOR = 5;
    public const int SCE_ABL_OPERATOR = 6;
    public const int SCE_ABL_IDENTIFIER = 7;
    public const int SCE_ABL_BLOCK = 8;
    public const int SCE_ABL_END = 9;
    public const int SCE_ABL_COMMENT = 10;
    public const int SCE_ABL_TASKMARKER = 11;
    public const int SCE_ABL_LINECOMMENT = 12;
    public const int SCE_ABL_ANNOTATION = 13;
    public const int SCE_ABL_TYPEDANNOTATION = 14;
    
    // ABAQUS
    // ======
    public const int SCE_ABAQUS_DEFAULT = 0;
    public const int SCE_ABAQUS_COMMENT = 1;
    public const int SCE_ABAQUS_COMMENTBLOCK = 2;
    public const int SCE_ABAQUS_NUMBER = 3;
    public const int SCE_ABAQUS_STRING = 4;
    public const int SCE_ABAQUS_OPERATOR = 5;
    public const int SCE_ABAQUS_WORD = 6;
    public const int SCE_ABAQUS_PROCESSOR = 7;
    public const int SCE_ABAQUS_COMMAND = 8;
    public const int SCE_ABAQUS_SLASHCOMMAND = 9;
    public const int SCE_ABAQUS_STARCOMMAND = 10;
    public const int SCE_ABAQUS_ARGUMENT = 11;
    public const int SCE_ABAQUS_FUNCTION = 12;
    
    // Asymptote
    // =========
    public const int SCE_ASY_DEFAULT = 0;
    public const int SCE_ASY_COMMENT = 1;
    public const int SCE_ASY_COMMENTLINE = 2;
    public const int SCE_ASY_NUMBER = 3;
    public const int SCE_ASY_WORD = 4;
    public const int SCE_ASY_STRING = 5;
    public const int SCE_ASY_CHARACTER = 6;
    public const int SCE_ASY_OPERATOR = 7;
    public const int SCE_ASY_IDENTIFIER = 8;
    public const int SCE_ASY_STRINGEOL = 9;
    public const int SCE_ASY_COMMENTLINEDOC = 10;
    public const int SCE_ASY_WORD2 = 11;
    
    // R
    // ===
    public const int SCE_R_DEFAULT = 0;
    public const int SCE_R_COMMENT = 1;
    public const int SCE_R_KWORD = 2;
    public const int SCE_R_BASEKWORD = 3;
    public const int SCE_R_OTHERKWORD = 4;
    public const int SCE_R_NUMBER = 5;
    public const int SCE_R_STRING = 6;
    public const int SCE_R_STRING2 = 7;
    public const int SCE_R_OPERATOR = 8;
    public const int SCE_R_IDENTIFIER = 9;
    public const int SCE_R_INFIX = 10;
    public const int SCE_R_INFIXEOL = 11;
    public const int SCE_R_BACKTICKS = 12;
    public const int SCE_R_RAWSTRING = 13;
    public const int SCE_R_RAWSTRING2 = 14;
    public const int SCE_R_ESCAPESEQUENCE = 15;
    
    // MagikSF
    // =======
    public const int SCE_MAGIK_DEFAULT = 0;
    public const int SCE_MAGIK_COMMENT = 1;
    public const int SCE_MAGIK_HYPER_COMMENT = 16;
    public const int SCE_MAGIK_STRING = 2;
    public const int SCE_MAGIK_CHARACTER = 3;
    public const int SCE_MAGIK_NUMBER = 4;
    public const int SCE_MAGIK_IDENTIFIER = 5;
    public const int SCE_MAGIK_OPERATOR = 6;
    public const int SCE_MAGIK_FLOW = 7;
    public const int SCE_MAGIK_CONTAINER = 8;
    public const int SCE_MAGIK_BRACKET_BLOCK = 9;
    public const int SCE_MAGIK_BRACE_BLOCK = 10;
    public const int SCE_MAGIK_SQBRACKET_BLOCK = 11;
    public const int SCE_MAGIK_UNKNOWN_KEYWORD = 12;
    public const int SCE_MAGIK_KEYWORD = 13;
    public const int SCE_MAGIK_PRAGMA = 14;
    public const int SCE_MAGIK_SYMBOL = 15;
    
    // PowerShell
    // ==========
    public const int SCE_POWERSHELL_DEFAULT = 0;
    public const int SCE_POWERSHELL_COMMENT = 1;
    public const int SCE_POWERSHELL_STRING = 2;
    public const int SCE_POWERSHELL_CHARACTER = 3;
    public const int SCE_POWERSHELL_NUMBER = 4;
    public const int SCE_POWERSHELL_VARIABLE = 5;
    public const int SCE_POWERSHELL_OPERATOR = 6;
    public const int SCE_POWERSHELL_IDENTIFIER = 7;
    public const int SCE_POWERSHELL_KEYWORD = 8;
    public const int SCE_POWERSHELL_CMDLET = 9;
    public const int SCE_POWERSHELL_ALIAS = 10;
    public const int SCE_POWERSHELL_FUNCTION = 11;
    public const int SCE_POWERSHELL_USER1 = 12;
    public const int SCE_POWERSHELL_COMMENTSTREAM = 13;
    public const int SCE_POWERSHELL_HERE_STRING = 14;
    public const int SCE_POWERSHELL_HERE_CHARACTER = 15;
    public const int SCE_POWERSHELL_COMMENTDOCKEYWORD = 16;
    
    // MySQL
    // =====
    public const int SCE_MYSQL_DEFAULT = 0;
    public const int SCE_MYSQL_COMMENT = 1;
    public const int SCE_MYSQL_COMMENTLINE = 2;
    public const int SCE_MYSQL_VARIABLE = 3;
    public const int SCE_MYSQL_SYSTEMVARIABLE = 4;
    public const int SCE_MYSQL_KNOWNSYSTEMVARIABLE = 5;
    public const int SCE_MYSQL_NUMBER = 6;
    public const int SCE_MYSQL_MAJORKEYWORD = 7;
    public const int SCE_MYSQL_KEYWORD = 8;
    public const int SCE_MYSQL_DATABASEOBJECT = 9;
    public const int SCE_MYSQL_PROCEDUREKEYWORD = 10;
    public const int SCE_MYSQL_STRING = 11;
    public const int SCE_MYSQL_SQSTRING = 12;
    public const int SCE_MYSQL_DQSTRING = 13;
    public const int SCE_MYSQL_OPERATOR = 14;
    public const int SCE_MYSQL_FUNCTION = 15;
    public const int SCE_MYSQL_IDENTIFIER = 16;
    public const int SCE_MYSQL_QUOTEDIDENTIFIER = 17;
    public const int SCE_MYSQL_USER1 = 18;
    public const int SCE_MYSQL_USER2 = 19;
    public const int SCE_MYSQL_USER3 = 20;
    public const int SCE_MYSQL_HIDDENCOMMAND = 21;
    public const int SCE_MYSQL_PLACEHOLDER = 22;
    
    // Po
    // ===
    public const int SCE_PO_DEFAULT = 0;
    public const int SCE_PO_COMMENT = 1;
    public const int SCE_PO_MSGID = 2;
    public const int SCE_PO_MSGID_TEXT = 3;
    public const int SCE_PO_MSGSTR = 4;
    public const int SCE_PO_MSGSTR_TEXT = 5;
    public const int SCE_PO_MSGCTXT = 6;
    public const int SCE_PO_MSGCTXT_TEXT = 7;
    public const int SCE_PO_FUZZY = 8;
    public const int SCE_PO_PROGRAMMER_COMMENT = 9;
    public const int SCE_PO_REFERENCE = 10;
    public const int SCE_PO_FLAGS = 11;
    public const int SCE_PO_MSGID_TEXT_EOL = 12;
    public const int SCE_PO_MSGSTR_TEXT_EOL = 13;
    public const int SCE_PO_MSGCTXT_TEXT_EOL = 14;
    public const int SCE_PO_ERROR = 15;
    
    // Pascal
    // ======
    public const int SCE_PAS_DEFAULT = 0;
    public const int SCE_PAS_IDENTIFIER = 1;
    public const int SCE_PAS_COMMENT = 2;
    public const int SCE_PAS_COMMENT2 = 3;
    public const int SCE_PAS_COMMENTLINE = 4;
    public const int SCE_PAS_PREPROCESSOR = 5;
    public const int SCE_PAS_PREPROCESSOR2 = 6;
    public const int SCE_PAS_NUMBER = 7;
    public const int SCE_PAS_HEXNUMBER = 8;
    public const int SCE_PAS_WORD = 9;
    public const int SCE_PAS_STRING = 10;
    public const int SCE_PAS_STRINGEOL = 11;
    public const int SCE_PAS_CHARACTER = 12;
    public const int SCE_PAS_OPERATOR = 13;
    public const int SCE_PAS_ASM = 14;
    
    // SORCUS
    // ======
    public const int SCE_SORCUS_DEFAULT = 0;
    public const int SCE_SORCUS_COMMAND = 1;
    public const int SCE_SORCUS_PARAMETER = 2;
    public const int SCE_SORCUS_COMMENTLINE = 3;
    public const int SCE_SORCUS_STRING = 4;
    public const int SCE_SORCUS_STRINGEOL = 5;
    public const int SCE_SORCUS_IDENTIFIER = 6;
    public const int SCE_SORCUS_OPERATOR = 7;
    public const int SCE_SORCUS_NUMBER = 8;
    public const int SCE_SORCUS_CONSTANT = 9;
    
    // PowerPro
    // ========
    public const int SCE_POWERPRO_DEFAULT = 0;
    public const int SCE_POWERPRO_COMMENTBLOCK = 1;
    public const int SCE_POWERPRO_COMMENTLINE = 2;
    public const int SCE_POWERPRO_NUMBER = 3;
    public const int SCE_POWERPRO_WORD = 4;
    public const int SCE_POWERPRO_WORD2 = 5;
    public const int SCE_POWERPRO_WORD3 = 6;
    public const int SCE_POWERPRO_WORD4 = 7;
    public const int SCE_POWERPRO_DOUBLEQUOTEDSTRING = 8;
    public const int SCE_POWERPRO_SINGLEQUOTEDSTRING = 9;
    public const int SCE_POWERPRO_LINECONTINUE = 10;
    public const int SCE_POWERPRO_OPERATOR = 11;
    public const int SCE_POWERPRO_IDENTIFIER = 12;
    public const int SCE_POWERPRO_STRINGEOL = 13;
    public const int SCE_POWERPRO_VERBATIM = 14;
    public const int SCE_POWERPRO_ALTQUOTE = 15;
    public const int SCE_POWERPRO_FUNCTION = 16;
    
    // SML
    // ===
    public const int SCE_SML_DEFAULT = 0;
    public const int SCE_SML_IDENTIFIER = 1;
    public const int SCE_SML_TAGNAME = 2;
    public const int SCE_SML_KEYWORD = 3;
    public const int SCE_SML_KEYWORD2 = 4;
    public const int SCE_SML_KEYWORD3 = 5;
    public const int SCE_SML_LINENUM = 6;
    public const int SCE_SML_OPERATOR = 7;
    public const int SCE_SML_NUMBER = 8;
    public const int SCE_SML_CHAR = 9;
    public const int SCE_SML_STRING = 11;
    public const int SCE_SML_COMMENT = 12;
    public const int SCE_SML_COMMENT1 = 13;
    public const int SCE_SML_COMMENT2 = 14;
    public const int SCE_SML_COMMENT3 = 15;
    
    // Markdown
    // ========
    public const int SCE_MARKDOWN_DEFAULT = 0;
    public const int SCE_MARKDOWN_LINE_BEGIN = 1;
    public const int SCE_MARKDOWN_STRONG1 = 2;
    public const int SCE_MARKDOWN_STRONG2 = 3;
    public const int SCE_MARKDOWN_EM1 = 4;
    public const int SCE_MARKDOWN_EM2 = 5;
    public const int SCE_MARKDOWN_HEADER1 = 6;
    public const int SCE_MARKDOWN_HEADER2 = 7;
    public const int SCE_MARKDOWN_HEADER3 = 8;
    public const int SCE_MARKDOWN_HEADER4 = 9;
    public const int SCE_MARKDOWN_HEADER5 = 10;
    public const int SCE_MARKDOWN_HEADER6 = 11;
    public const int SCE_MARKDOWN_PRECHAR = 12;
    public const int SCE_MARKDOWN_ULIST_ITEM = 13;
    public const int SCE_MARKDOWN_OLIST_ITEM = 14;
    public const int SCE_MARKDOWN_BLOCKQUOTE = 15;
    public const int SCE_MARKDOWN_STRIKEOUT = 16;
    public const int SCE_MARKDOWN_HRULE = 17;
    public const int SCE_MARKDOWN_LINK = 18;
    public const int SCE_MARKDOWN_CODE = 19;
    public const int SCE_MARKDOWN_CODE2 = 20;
    public const int SCE_MARKDOWN_CODEBK = 21;
    
    // Txt2tags
    // ========
    public const int SCE_TXT2TAGS_DEFAULT = 0;
    public const int SCE_TXT2TAGS_LINE_BEGIN = 1;
    public const int SCE_TXT2TAGS_STRONG1 = 2;
    public const int SCE_TXT2TAGS_STRONG2 = 3;
    public const int SCE_TXT2TAGS_EM1 = 4;
    public const int SCE_TXT2TAGS_EM2 = 5;
    public const int SCE_TXT2TAGS_HEADER1 = 6;
    public const int SCE_TXT2TAGS_HEADER2 = 7;
    public const int SCE_TXT2TAGS_HEADER3 = 8;
    public const int SCE_TXT2TAGS_HEADER4 = 9;
    public const int SCE_TXT2TAGS_HEADER5 = 10;
    public const int SCE_TXT2TAGS_HEADER6 = 11;
    public const int SCE_TXT2TAGS_PRECHAR = 12;
    public const int SCE_TXT2TAGS_ULIST_ITEM = 13;
    public const int SCE_TXT2TAGS_OLIST_ITEM = 14;
    public const int SCE_TXT2TAGS_BLOCKQUOTE = 15;
    public const int SCE_TXT2TAGS_STRIKEOUT = 16;
    public const int SCE_TXT2TAGS_HRULE = 17;
    public const int SCE_TXT2TAGS_LINK = 18;
    public const int SCE_TXT2TAGS_CODE = 19;
    public const int SCE_TXT2TAGS_CODE2 = 20;
    public const int SCE_TXT2TAGS_CODEBK = 21;
    public const int SCE_TXT2TAGS_COMMENT = 22;
    public const int SCE_TXT2TAGS_OPTION = 23;
    public const int SCE_TXT2TAGS_PREPROC = 24;
    public const int SCE_TXT2TAGS_POSTPROC = 25;
    
    // A68k
    // ====
    public const int SCE_A68K_DEFAULT = 0;
    public const int SCE_A68K_COMMENT = 1;
    public const int SCE_A68K_NUMBER_DEC = 2;
    public const int SCE_A68K_NUMBER_BIN = 3;
    public const int SCE_A68K_NUMBER_HEX = 4;
    public const int SCE_A68K_STRING1 = 5;
    public const int SCE_A68K_OPERATOR = 6;
    public const int SCE_A68K_CPUINSTRUCTION = 7;
    public const int SCE_A68K_EXTINSTRUCTION = 8;
    public const int SCE_A68K_REGISTER = 9;
    public const int SCE_A68K_DIRECTIVE = 10;
    public const int SCE_A68K_MACRO_ARG = 11;
    public const int SCE_A68K_LABEL = 12;
    public const int SCE_A68K_STRING2 = 13;
    public const int SCE_A68K_IDENTIFIER = 14;
    public const int SCE_A68K_MACRO_DECLARATION = 15;
    public const int SCE_A68K_COMMENT_WORD = 16;
    public const int SCE_A68K_COMMENT_SPECIAL = 17;
    public const int SCE_A68K_COMMENT_DOXYGEN = 18;
    
    // Modula
    // ======
    public const int SCE_MODULA_DEFAULT = 0;
    public const int SCE_MODULA_COMMENT = 1;
    public const int SCE_MODULA_DOXYCOMM = 2;
    public const int SCE_MODULA_DOXYKEY = 3;
    public const int SCE_MODULA_KEYWORD = 4;
    public const int SCE_MODULA_RESERVED = 5;
    public const int SCE_MODULA_NUMBER = 6;
    public const int SCE_MODULA_BASENUM = 7;
    public const int SCE_MODULA_FLOAT = 8;
    public const int SCE_MODULA_STRING = 9;
    public const int SCE_MODULA_STRSPEC = 10;
    public const int SCE_MODULA_CHAR = 11;
    public const int SCE_MODULA_CHARSPEC = 12;
    public const int SCE_MODULA_PROC = 13;
    public const int SCE_MODULA_PRAGMA = 14;
    public const int SCE_MODULA_PRGKEY = 15;
    public const int SCE_MODULA_OPERATOR = 16;
    public const int SCE_MODULA_BADSTR = 17;
    
    // CoffeeScript
    // ============
    public const int SCE_COFFEESCRIPT_DEFAULT = 0;
    public const int SCE_COFFEESCRIPT_COMMENT = 1;
    public const int SCE_COFFEESCRIPT_COMMENTLINE = 2;
    public const int SCE_COFFEESCRIPT_COMMENTDOC = 3;
    public const int SCE_COFFEESCRIPT_NUMBER = 4;
    public const int SCE_COFFEESCRIPT_WORD = 5;
    public const int SCE_COFFEESCRIPT_STRING = 6;
    public const int SCE_COFFEESCRIPT_CHARACTER = 7;
    public const int SCE_COFFEESCRIPT_UUID = 8;
    public const int SCE_COFFEESCRIPT_PREPROCESSOR = 9;
    public const int SCE_COFFEESCRIPT_OPERATOR = 10;
    public const int SCE_COFFEESCRIPT_IDENTIFIER = 11;
    public const int SCE_COFFEESCRIPT_STRINGEOL = 12;
    public const int SCE_COFFEESCRIPT_VERBATIM = 13;
    public const int SCE_COFFEESCRIPT_REGEX = 14;
    public const int SCE_COFFEESCRIPT_COMMENTLINEDOC = 15;
    public const int SCE_COFFEESCRIPT_WORD2 = 16;
    public const int SCE_COFFEESCRIPT_COMMENTDOCKEYWORD = 17;
    public const int SCE_COFFEESCRIPT_COMMENTDOCKEYWORDERROR = 18;
    public const int SCE_COFFEESCRIPT_GLOBALCLASS = 19;
    public const int SCE_COFFEESCRIPT_STRINGRAW = 20;
    public const int SCE_COFFEESCRIPT_TRIPLEVERBATIM = 21;
    public const int SCE_COFFEESCRIPT_COMMENTBLOCK = 22;
    public const int SCE_COFFEESCRIPT_VERBOSE_REGEX = 23;
    public const int SCE_COFFEESCRIPT_VERBOSE_REGEX_COMMENT = 24;
    public const int SCE_COFFEESCRIPT_INSTANCEPROPERTY = 25;
    
    // AVS
    // ===
    public const int SCE_AVS_DEFAULT = 0;
    public const int SCE_AVS_COMMENTBLOCK = 1;
    public const int SCE_AVS_COMMENTBLOCKN = 2;
    public const int SCE_AVS_COMMENTLINE = 3;
    public const int SCE_AVS_NUMBER = 4;
    public const int SCE_AVS_OPERATOR = 5;
    public const int SCE_AVS_IDENTIFIER = 6;
    public const int SCE_AVS_STRING = 7;
    public const int SCE_AVS_TRIPLESTRING = 8;
    public const int SCE_AVS_KEYWORD = 9;
    public const int SCE_AVS_FILTER = 10;
    public const int SCE_AVS_PLUGIN = 11;
    public const int SCE_AVS_FUNCTION = 12;
    public const int SCE_AVS_CLIPPROP = 13;
    public const int SCE_AVS_USERDFN = 14;
    
    // ECL
    // ===
    public const int SCE_ECL_DEFAULT = 0;
    public const int SCE_ECL_COMMENT = 1;
    public const int SCE_ECL_COMMENTLINE = 2;
    public const int SCE_ECL_NUMBER = 3;
    public const int SCE_ECL_STRING = 4;
    public const int SCE_ECL_WORD0 = 5;
    public const int SCE_ECL_OPERATOR = 6;
    public const int SCE_ECL_CHARACTER = 7;
    public const int SCE_ECL_UUID = 8;
    public const int SCE_ECL_PREPROCESSOR = 9;
    public const int SCE_ECL_UNKNOWN = 10;
    public const int SCE_ECL_IDENTIFIER = 11;
    public const int SCE_ECL_STRINGEOL = 12;
    public const int SCE_ECL_VERBATIM = 13;
    public const int SCE_ECL_REGEX = 14;
    public const int SCE_ECL_COMMENTLINEDOC = 15;
    public const int SCE_ECL_WORD1 = 16;
    public const int SCE_ECL_COMMENTDOCKEYWORD = 17;
    public const int SCE_ECL_COMMENTDOCKEYWORDERROR = 18;
    public const int SCE_ECL_WORD2 = 19;
    public const int SCE_ECL_WORD3 = 20;
    public const int SCE_ECL_WORD4 = 21;
    public const int SCE_ECL_WORD5 = 22;
    public const int SCE_ECL_COMMENTDOC = 23;
    public const int SCE_ECL_ADDED = 24;
    public const int SCE_ECL_DELETED = 25;
    public const int SCE_ECL_CHANGED = 26;
    public const int SCE_ECL_MOVED = 27;
    
    // OScript
    // =======
    public const int SCE_OSCRIPT_DEFAULT = 0;
    public const int SCE_OSCRIPT_LINE_COMMENT = 1;
    public const int SCE_OSCRIPT_BLOCK_COMMENT = 2;
    public const int SCE_OSCRIPT_DOC_COMMENT = 3;
    public const int SCE_OSCRIPT_PREPROCESSOR = 4;
    public const int SCE_OSCRIPT_NUMBER = 5;
    public const int SCE_OSCRIPT_SINGLEQUOTE_STRING = 6;
    public const int SCE_OSCRIPT_DOUBLEQUOTE_STRING = 7;
    public const int SCE_OSCRIPT_CONSTANT = 8;
    public const int SCE_OSCRIPT_IDENTIFIER = 9;
    public const int SCE_OSCRIPT_GLOBAL = 10;
    public const int SCE_OSCRIPT_KEYWORD = 11;
    public const int SCE_OSCRIPT_OPERATOR = 12;
    public const int SCE_OSCRIPT_LABEL = 13;
    public const int SCE_OSCRIPT_TYPE = 14;
    public const int SCE_OSCRIPT_FUNCTION = 15;
    public const int SCE_OSCRIPT_OBJECT = 16;
    public const int SCE_OSCRIPT_PROPERTY = 17;
    public const int SCE_OSCRIPT_METHOD = 18;
    
    // VisualProlog
    // ============
    public const int SCE_VISUALPROLOG_DEFAULT = 0;
    public const int SCE_VISUALPROLOG_KEY_MAJOR = 1;
    public const int SCE_VISUALPROLOG_KEY_MINOR = 2;
    public const int SCE_VISUALPROLOG_KEY_DIRECTIVE = 3;
    public const int SCE_VISUALPROLOG_COMMENT_BLOCK = 4;
    public const int SCE_VISUALPROLOG_COMMENT_LINE = 5;
    public const int SCE_VISUALPROLOG_COMMENT_KEY = 6;
    public const int SCE_VISUALPROLOG_COMMENT_KEY_ERROR = 7;
    public const int SCE_VISUALPROLOG_IDENTIFIER = 8;
    public const int SCE_VISUALPROLOG_VARIABLE = 9;
    public const int SCE_VISUALPROLOG_ANONYMOUS = 10;
    public const int SCE_VISUALPROLOG_NUMBER = 11;
    public const int SCE_VISUALPROLOG_OPERATOR = 12;
    public const int SCE_VISUALPROLOG_UNUSED1 = 13;
    public const int SCE_VISUALPROLOG_UNUSED2 = 14;
    public const int SCE_VISUALPROLOG_UNUSED3 = 15;
    public const int SCE_VISUALPROLOG_STRING_QUOTE = 16;
    public const int SCE_VISUALPROLOG_STRING_ESCAPE = 17;
    public const int SCE_VISUALPROLOG_STRING_ESCAPE_ERROR = 18;
    public const int SCE_VISUALPROLOG_UNUSED4 = 19;
    public const int SCE_VISUALPROLOG_STRING = 20;
    public const int SCE_VISUALPROLOG_UNUSED5 = 21;
    public const int SCE_VISUALPROLOG_STRING_EOL = 22;
    public const int SCE_VISUALPROLOG_EMBEDDED = 23;
    public const int SCE_VISUALPROLOG_PLACEHOLDER = 24;
    
    // StructuredText
    // ==============
    public const int SCE_STTXT_DEFAULT = 0;
    public const int SCE_STTXT_COMMENT = 1;
    public const int SCE_STTXT_COMMENTLINE = 2;
    public const int SCE_STTXT_KEYWORD = 3;
    public const int SCE_STTXT_TYPE = 4;
    public const int SCE_STTXT_FUNCTION = 5;
    public const int SCE_STTXT_FB = 6;
    public const int SCE_STTXT_NUMBER = 7;
    public const int SCE_STTXT_HEXNUMBER = 8;
    public const int SCE_STTXT_PRAGMA = 9;
    public const int SCE_STTXT_OPERATOR = 10;
    public const int SCE_STTXT_CHARACTER = 11;
    public const int SCE_STTXT_STRING1 = 12;
    public const int SCE_STTXT_STRING2 = 13;
    public const int SCE_STTXT_STRINGEOL = 14;
    public const int SCE_STTXT_IDENTIFIER = 15;
    public const int SCE_STTXT_DATETIME = 16;
    public const int SCE_STTXT_VARS = 17;
    public const int SCE_STTXT_PRAGMAS = 18;
    
    // KVIrc
    // =====
    public const int SCE_KVIRC_DEFAULT = 0;
    public const int SCE_KVIRC_COMMENT = 1;
    public const int SCE_KVIRC_COMMENTBLOCK = 2;
    public const int SCE_KVIRC_STRING = 3;
    public const int SCE_KVIRC_WORD = 4;
    public const int SCE_KVIRC_KEYWORD = 5;
    public const int SCE_KVIRC_FUNCTION_KEYWORD = 6;
    public const int SCE_KVIRC_FUNCTION = 7;
    public const int SCE_KVIRC_VARIABLE = 8;
    public const int SCE_KVIRC_NUMBER = 9;
    public const int SCE_KVIRC_OPERATOR = 10;
    public const int SCE_KVIRC_STRING_FUNCTION = 11;
    public const int SCE_KVIRC_STRING_VARIABLE = 12;
    
    // Rust
    // ====
    public const int SCE_RUST_DEFAULT = 0;
    public const int SCE_RUST_COMMENTBLOCK = 1;
    public const int SCE_RUST_COMMENTLINE = 2;
    public const int SCE_RUST_COMMENTBLOCKDOC = 3;
    public const int SCE_RUST_COMMENTLINEDOC = 4;
    public const int SCE_RUST_NUMBER = 5;
    public const int SCE_RUST_WORD = 6;
    public const int SCE_RUST_WORD2 = 7;
    public const int SCE_RUST_WORD3 = 8;
    public const int SCE_RUST_WORD4 = 9;
    public const int SCE_RUST_WORD5 = 10;
    public const int SCE_RUST_WORD6 = 11;
    public const int SCE_RUST_WORD7 = 12;
    public const int SCE_RUST_STRING = 13;
    public const int SCE_RUST_STRINGR = 14;
    public const int SCE_RUST_CHARACTER = 15;
    public const int SCE_RUST_OPERATOR = 16;
    public const int SCE_RUST_IDENTIFIER = 17;
    public const int SCE_RUST_LIFETIME = 18;
    public const int SCE_RUST_MACRO = 19;
    public const int SCE_RUST_LEXERROR = 20;
    public const int SCE_RUST_BYTESTRING = 21;
    public const int SCE_RUST_BYTESTRINGR = 22;
    public const int SCE_RUST_BYTECHARACTER = 23;
    public const int SCE_RUST_CSTRING = 24;
    public const int SCE_RUST_CSTRINGR = 25;
    
    // DMAP
    // ====
    public const int SCE_DMAP_DEFAULT = 0;
    public const int SCE_DMAP_COMMENT = 1;
    public const int SCE_DMAP_NUMBER = 2;
    public const int SCE_DMAP_STRING1 = 3;
    public const int SCE_DMAP_STRING2 = 4;
    public const int SCE_DMAP_STRINGEOL = 5;
    public const int SCE_DMAP_OPERATOR = 6;
    public const int SCE_DMAP_IDENTIFIER = 7;
    public const int SCE_DMAP_WORD = 8;
    public const int SCE_DMAP_WORD2 = 9;
    public const int SCE_DMAP_WORD3 = 10;
    
    // DMIS
    // ====
    public const int SCE_DMIS_DEFAULT = 0;
    public const int SCE_DMIS_COMMENT = 1;
    public const int SCE_DMIS_STRING = 2;
    public const int SCE_DMIS_NUMBER = 3;
    public const int SCE_DMIS_KEYWORD = 4;
    public const int SCE_DMIS_MAJORWORD = 5;
    public const int SCE_DMIS_MINORWORD = 6;
    public const int SCE_DMIS_UNSUPPORTED_MAJOR = 7;
    public const int SCE_DMIS_UNSUPPORTED_MINOR = 8;
    public const int SCE_DMIS_LABEL = 9;
    
    // REG
    // ===
    public const int SCE_REG_DEFAULT = 0;
    public const int SCE_REG_COMMENT = 1;
    public const int SCE_REG_VALUENAME = 2;
    public const int SCE_REG_STRING = 3;
    public const int SCE_REG_HEXDIGIT = 4;
    public const int SCE_REG_VALUETYPE = 5;
    public const int SCE_REG_ADDEDKEY = 6;
    public const int SCE_REG_DELETEDKEY = 7;
    public const int SCE_REG_ESCAPED = 8;
    public const int SCE_REG_KEYPATH_GUID = 9;
    public const int SCE_REG_STRING_GUID = 10;
    public const int SCE_REG_PARAMETER = 11;
    public const int SCE_REG_OPERATOR = 12;
    
    // BibTeX
    // ======
    public const int SCE_BIBTEX_DEFAULT = 0;
    public const int SCE_BIBTEX_ENTRY = 1;
    public const int SCE_BIBTEX_UNKNOWN_ENTRY = 2;
    public const int SCE_BIBTEX_KEY = 3;
    public const int SCE_BIBTEX_PARAMETER = 4;
    public const int SCE_BIBTEX_VALUE = 5;
    public const int SCE_BIBTEX_COMMENT = 6;
    
    // Srec
    // ====
    public const int SCE_HEX_DEFAULT = 0;
    public const int SCE_HEX_RECSTART = 1;
    public const int SCE_HEX_RECTYPE = 2;
    public const int SCE_HEX_RECTYPE_UNKNOWN = 3;
    public const int SCE_HEX_BYTECOUNT = 4;
    public const int SCE_HEX_BYTECOUNT_WRONG = 5;
    public const int SCE_HEX_NOADDRESS = 6;
    public const int SCE_HEX_DATAADDRESS = 7;
    public const int SCE_HEX_RECCOUNT = 8;
    public const int SCE_HEX_STARTADDRESS = 9;
    public const int SCE_HEX_ADDRESSFIELD_UNKNOWN = 10;
    public const int SCE_HEX_EXTENDEDADDRESS = 11;
    public const int SCE_HEX_DATA_ODD = 12;
    public const int SCE_HEX_DATA_EVEN = 13;
    public const int SCE_HEX_DATA_UNKNOWN = 14;
    public const int SCE_HEX_DATA_EMPTY = 15;
    public const int SCE_HEX_CHECKSUM = 16;
    public const int SCE_HEX_CHECKSUM_WRONG = 17;
    public const int SCE_HEX_GARBAGE = 18;
    
    // JSON
    // ====
    public const int SCE_JSON_DEFAULT = 0;
    public const int SCE_JSON_NUMBER = 1;
    public const int SCE_JSON_STRING = 2;
    public const int SCE_JSON_STRINGEOL = 3;
    public const int SCE_JSON_PROPERTYNAME = 4;
    public const int SCE_JSON_ESCAPESEQUENCE = 5;
    public const int SCE_JSON_LINECOMMENT = 6;
    public const int SCE_JSON_BLOCKCOMMENT = 7;
    public const int SCE_JSON_OPERATOR = 8;
    public const int SCE_JSON_URI = 9;
    public const int SCE_JSON_COMPACTIRI = 10;
    public const int SCE_JSON_KEYWORD = 11;
    public const int SCE_JSON_LDKEYWORD = 12;
    public const int SCE_JSON_ERROR = 13;
    
    // EDIFACT
    // =======
    public const int SCE_EDI_DEFAULT = 0;
    public const int SCE_EDI_SEGMENTSTART = 1;
    public const int SCE_EDI_SEGMENTEND = 2;
    public const int SCE_EDI_SEP_ELEMENT = 3;
    public const int SCE_EDI_SEP_COMPOSITE = 4;
    public const int SCE_EDI_SEP_RELEASE = 5;
    public const int SCE_EDI_UNA = 6;
    public const int SCE_EDI_UNH = 7;
    public const int SCE_EDI_BADSEGMENT = 8;
    
    // STATA
    // =====
    public const int SCE_STATA_DEFAULT = 0;
    public const int SCE_STATA_COMMENT = 1;
    public const int SCE_STATA_COMMENTLINE = 2;
    public const int SCE_STATA_COMMENTBLOCK = 3;
    public const int SCE_STATA_NUMBER = 4;
    public const int SCE_STATA_OPERATOR = 5;
    public const int SCE_STATA_IDENTIFIER = 6;
    public const int SCE_STATA_STRING = 7;
    public const int SCE_STATA_TYPE = 8;
    public const int SCE_STATA_WORD = 9;
    public const int SCE_STATA_GLOBAL_MACRO = 10;
    public const int SCE_STATA_MACRO = 11;
    
    // SAS
    // ===
    public const int SCE_SAS_DEFAULT = 0;
    public const int SCE_SAS_COMMENT = 1;
    public const int SCE_SAS_COMMENTLINE = 2;
    public const int SCE_SAS_COMMENTBLOCK = 3;
    public const int SCE_SAS_NUMBER = 4;
    public const int SCE_SAS_OPERATOR = 5;
    public const int SCE_SAS_IDENTIFIER = 6;
    public const int SCE_SAS_STRING = 7;
    public const int SCE_SAS_TYPE = 8;
    public const int SCE_SAS_WORD = 9;
    public const int SCE_SAS_GLOBAL_MACRO = 10;
    public const int SCE_SAS_MACRO = 11;
    public const int SCE_SAS_MACRO_KEYWORD = 12;
    public const int SCE_SAS_BLOCK_KEYWORD = 13;
    public const int SCE_SAS_MACRO_FUNCTION = 14;
    public const int SCE_SAS_STATEMENT = 15;
    
    // Nim
    // ===
    public const int SCE_NIM_DEFAULT = 0;
    public const int SCE_NIM_COMMENT = 1;
    public const int SCE_NIM_COMMENTDOC = 2;
    public const int SCE_NIM_COMMENTLINE = 3;
    public const int SCE_NIM_COMMENTLINEDOC = 4;
    public const int SCE_NIM_NUMBER = 5;
    public const int SCE_NIM_STRING = 6;
    public const int SCE_NIM_CHARACTER = 7;
    public const int SCE_NIM_WORD = 8;
    public const int SCE_NIM_TRIPLE = 9;
    public const int SCE_NIM_TRIPLEDOUBLE = 10;
    public const int SCE_NIM_BACKTICKS = 11;
    public const int SCE_NIM_FUNCNAME = 12;
    public const int SCE_NIM_STRINGEOL = 13;
    public const int SCE_NIM_NUMERROR = 14;
    public const int SCE_NIM_OPERATOR = 15;
    public const int SCE_NIM_IDENTIFIER = 16;
    
    // CIL
    // ===
    public const int SCE_CIL_DEFAULT = 0;
    public const int SCE_CIL_COMMENT = 1;
    public const int SCE_CIL_COMMENTLINE = 2;
    public const int SCE_CIL_WORD = 3;
    public const int SCE_CIL_WORD2 = 4;
    public const int SCE_CIL_WORD3 = 5;
    public const int SCE_CIL_STRING = 6;
    public const int SCE_CIL_LABEL = 7;
    public const int SCE_CIL_OPERATOR = 8;
    public const int SCE_CIL_IDENTIFIER = 9;
    public const int SCE_CIL_STRINGEOL = 10;
    
    // X12
    // ===
    public const int SCE_X12_DEFAULT = 0;
    public const int SCE_X12_BAD = 1;
    public const int SCE_X12_ENVELOPE = 2;
    public const int SCE_X12_FUNCTIONGROUP = 3;
    public const int SCE_X12_TRANSACTIONSET = 4;
    public const int SCE_X12_SEGMENTHEADER = 5;
    public const int SCE_X12_SEGMENTEND = 6;
    public const int SCE_X12_SEP_ELEMENT = 7;
    public const int SCE_X12_SEP_SUBELEMENT = 8;
    
    // Dataflex
    // ========
    public const int SCE_DF_DEFAULT = 0;
    public const int SCE_DF_IDENTIFIER = 1;
    public const int SCE_DF_METATAG = 2;
    public const int SCE_DF_IMAGE = 3;
    public const int SCE_DF_COMMENTLINE = 4;
    public const int SCE_DF_PREPROCESSOR = 5;
    public const int SCE_DF_PREPROCESSOR2 = 6;
    public const int SCE_DF_NUMBER = 7;
    public const int SCE_DF_HEXNUMBER = 8;
    public const int SCE_DF_WORD = 9;
    public const int SCE_DF_STRING = 10;
    public const int SCE_DF_STRINGEOL = 11;
    public const int SCE_DF_SCOPEWORD = 12;
    public const int SCE_DF_OPERATOR = 13;
    public const int SCE_DF_ICODE = 14;
    
    // Hollywood
    // =========
    public const int SCE_HOLLYWOOD_DEFAULT = 0;
    public const int SCE_HOLLYWOOD_COMMENT = 1;
    public const int SCE_HOLLYWOOD_COMMENTBLOCK = 2;
    public const int SCE_HOLLYWOOD_NUMBER = 3;
    public const int SCE_HOLLYWOOD_KEYWORD = 4;
    public const int SCE_HOLLYWOOD_STDAPI = 5;
    public const int SCE_HOLLYWOOD_PLUGINAPI = 6;
    public const int SCE_HOLLYWOOD_PLUGINMETHOD = 7;
    public const int SCE_HOLLYWOOD_STRING = 8;
    public const int SCE_HOLLYWOOD_STRINGBLOCK = 9;
    public const int SCE_HOLLYWOOD_PREPROCESSOR = 10;
    public const int SCE_HOLLYWOOD_OPERATOR = 11;
    public const int SCE_HOLLYWOOD_IDENTIFIER = 12;
    public const int SCE_HOLLYWOOD_CONSTANT = 13;
    public const int SCE_HOLLYWOOD_HEXNUMBER = 14;
    
    // Raku
    // ====
    public const int SCE_RAKU_DEFAULT = 0;
    public const int SCE_RAKU_ERROR = 1;
    public const int SCE_RAKU_COMMENTLINE = 2;
    public const int SCE_RAKU_COMMENTEMBED = 3;
    public const int SCE_RAKU_POD = 4;
    public const int SCE_RAKU_CHARACTER = 5;
    public const int SCE_RAKU_HEREDOC_Q = 6;
    public const int SCE_RAKU_HEREDOC_QQ = 7;
    public const int SCE_RAKU_STRING = 8;
    public const int SCE_RAKU_STRING_Q = 9;
    public const int SCE_RAKU_STRING_QQ = 10;
    public const int SCE_RAKU_STRING_Q_LANG = 11;
    public const int SCE_RAKU_STRING_VAR = 12;
    public const int SCE_RAKU_REGEX = 13;
    public const int SCE_RAKU_REGEX_VAR = 14;
    public const int SCE_RAKU_ADVERB = 15;
    public const int SCE_RAKU_NUMBER = 16;
    public const int SCE_RAKU_PREPROCESSOR = 17;
    public const int SCE_RAKU_OPERATOR = 18;
    public const int SCE_RAKU_WORD = 19;
    public const int SCE_RAKU_FUNCTION = 20;
    public const int SCE_RAKU_IDENTIFIER = 21;
    public const int SCE_RAKU_TYPEDEF = 22;
    public const int SCE_RAKU_MU = 23;
    public const int SCE_RAKU_POSITIONAL = 24;
    public const int SCE_RAKU_ASSOCIATIVE = 25;
    public const int SCE_RAKU_CALLABLE = 26;
    public const int SCE_RAKU_GRAMMAR = 27;
    public const int SCE_RAKU_CLASS = 28;
    
    // FSharp
    // ======
    public const int SCE_FSHARP_DEFAULT = 0;
    public const int SCE_FSHARP_KEYWORD = 1;
    public const int SCE_FSHARP_KEYWORD2 = 2;
    public const int SCE_FSHARP_KEYWORD3 = 3;
    public const int SCE_FSHARP_KEYWORD4 = 4;
    public const int SCE_FSHARP_KEYWORD5 = 5;
    public const int SCE_FSHARP_IDENTIFIER = 6;
    public const int SCE_FSHARP_QUOT_IDENTIFIER = 7;
    public const int SCE_FSHARP_COMMENT = 8;
    public const int SCE_FSHARP_COMMENTLINE = 9;
    public const int SCE_FSHARP_PREPROCESSOR = 10;
    public const int SCE_FSHARP_LINENUM = 11;
    public const int SCE_FSHARP_OPERATOR = 12;
    public const int SCE_FSHARP_NUMBER = 13;
    public const int SCE_FSHARP_CHARACTER = 14;
    public const int SCE_FSHARP_STRING = 15;
    public const int SCE_FSHARP_VERBATIM = 16;
    public const int SCE_FSHARP_QUOTATION = 17;
    public const int SCE_FSHARP_ATTRIBUTE = 18;
    public const int SCE_FSHARP_FORMAT_SPEC = 19;
    
    // Asciidoc
    // ========
    public const int SCE_ASCIIDOC_DEFAULT = 0;
    public const int SCE_ASCIIDOC_STRONG1 = 1;
    public const int SCE_ASCIIDOC_STRONG2 = 2;
    public const int SCE_ASCIIDOC_EM1 = 3;
    public const int SCE_ASCIIDOC_EM2 = 4;
    public const int SCE_ASCIIDOC_HEADER1 = 5;
    public const int SCE_ASCIIDOC_HEADER2 = 6;
    public const int SCE_ASCIIDOC_HEADER3 = 7;
    public const int SCE_ASCIIDOC_HEADER4 = 8;
    public const int SCE_ASCIIDOC_HEADER5 = 9;
    public const int SCE_ASCIIDOC_HEADER6 = 10;
    public const int SCE_ASCIIDOC_ULIST_ITEM = 11;
    public const int SCE_ASCIIDOC_OLIST_ITEM = 12;
    public const int SCE_ASCIIDOC_BLOCKQUOTE = 13;
    public const int SCE_ASCIIDOC_LINK = 14;
    public const int SCE_ASCIIDOC_CODEBK = 15;
    public const int SCE_ASCIIDOC_PASSBK = 16;
    public const int SCE_ASCIIDOC_COMMENT = 17;
    public const int SCE_ASCIIDOC_COMMENTBK = 18;
    public const int SCE_ASCIIDOC_LITERAL = 19;
    public const int SCE_ASCIIDOC_LITERALBK = 20;
    public const int SCE_ASCIIDOC_ATTRIB = 21;
    public const int SCE_ASCIIDOC_ATTRIBVAL = 22;
    public const int SCE_ASCIIDOC_MACRO = 23;
    
    // GDScript
    // ========
    public const int SCE_GD_DEFAULT = 0;
    public const int SCE_GD_COMMENTLINE = 1;
    public const int SCE_GD_NUMBER = 2;
    public const int SCE_GD_STRING = 3;
    public const int SCE_GD_CHARACTER = 4;
    public const int SCE_GD_WORD = 5;
    public const int SCE_GD_TRIPLE = 6;
    public const int SCE_GD_TRIPLEDOUBLE = 7;
    public const int SCE_GD_CLASSNAME = 8;
    public const int SCE_GD_FUNCNAME = 9;
    public const int SCE_GD_OPERATOR = 10;
    public const int SCE_GD_IDENTIFIER = 11;
    public const int SCE_GD_COMMENTBLOCK = 12;
    public const int SCE_GD_STRINGEOL = 13;
    public const int SCE_GD_WORD2 = 14;
    public const int SCE_GD_ANNOTATION = 15;
    public const int SCE_GD_NODEPATH = 16;
    
    // TOML
    // ====
    public const int SCE_TOML_DEFAULT = 0;
    public const int SCE_TOML_COMMENT = 1;
    public const int SCE_TOML_IDENTIFIER = 2;
    public const int SCE_TOML_KEYWORD = 3;
    public const int SCE_TOML_NUMBER = 4;
    public const int SCE_TOML_TABLE = 5;
    public const int SCE_TOML_KEY = 6;
    public const int SCE_TOML_ERROR = 7;
    public const int SCE_TOML_OPERATOR = 8;
    public const int SCE_TOML_STRING_SQ = 9;
    public const int SCE_TOML_STRING_DQ = 10;
    public const int SCE_TOML_TRIPLE_STRING_SQ = 11;
    public const int SCE_TOML_TRIPLE_STRING_DQ = 12;
    public const int SCE_TOML_ESCAPECHAR = 13;
    public const int SCE_TOML_DATETIME = 14;
    public const int SCE_TOML_STRINGEOL = 15;
    
    // troff
    // =====
    public const int SCE_TROFF_DEFAULT = 0;
    public const int SCE_TROFF_REQUEST = 1;
    public const int SCE_TROFF_COMMAND = 2;
    public const int SCE_TROFF_NUMBER = 3;
    public const int SCE_TROFF_OPERATOR = 4;
    public const int SCE_TROFF_STRING = 5;
    public const int SCE_TROFF_COMMENT = 6;
    public const int SCE_TROFF_IGNORE = 7;
    public const int SCE_TROFF_ESCAPE_STRING = 8;
    public const int SCE_TROFF_ESCAPE_MACRO = 9;
    public const int SCE_TROFF_ESCAPE_FONT = 10;
    public const int SCE_TROFF_ESCAPE_NUMBER = 11;
    public const int SCE_TROFF_ESCAPE_COLOUR = 12;
    public const int SCE_TROFF_ESCAPE_GLYPH = 13;
    public const int SCE_TROFF_ESCAPE_ENV = 14;
    public const int SCE_TROFF_ESCAPE_SUPPRESSION = 15;
    public const int SCE_TROFF_ESCAPE_SIZE = 16;
    public const int SCE_TROFF_ESCAPE_TRANSPARENT = 17;
    public const int SCE_TROFF_ESCAPE_ISVALID = 18;
    public const int SCE_TROFF_ESCAPE_DRAW = 19;
    public const int SCE_TROFF_ESCAPE_MOVE = 20;
    public const int SCE_TROFF_ESCAPE_HEIGHT = 21;
    public const int SCE_TROFF_ESCAPE_OVERSTRIKE = 22;
    public const int SCE_TROFF_ESCAPE_SLANT = 23;
    public const int SCE_TROFF_ESCAPE_WIDTH = 24;
    public const int SCE_TROFF_ESCAPE_VSPACING = 25;
    public const int SCE_TROFF_ESCAPE_DEVICE = 26;
    public const int SCE_TROFF_ESCAPE_NOMOVE = 27;
    
    // Dart
    // ====
    public const int SCE_DART_DEFAULT = 0;
    public const int SCE_DART_COMMENTLINE = 1;
    public const int SCE_DART_COMMENTLINEDOC = 2;
    public const int SCE_DART_COMMENTBLOCK = 3;
    public const int SCE_DART_COMMENTBLOCKDOC = 4;
    public const int SCE_DART_STRING_SQ = 5;
    public const int SCE_DART_STRING_DQ = 6;
    public const int SCE_DART_TRIPLE_STRING_SQ = 7;
    public const int SCE_DART_TRIPLE_STRING_DQ = 8;
    public const int SCE_DART_RAWSTRING_SQ = 9;
    public const int SCE_DART_RAWSTRING_DQ = 10;
    public const int SCE_DART_TRIPLE_RAWSTRING_SQ = 11;
    public const int SCE_DART_TRIPLE_RAWSTRING_DQ = 12;
    public const int SCE_DART_ESCAPECHAR = 13;
    public const int SCE_DART_IDENTIFIER = 14;
    public const int SCE_DART_IDENTIFIER_STRING = 15;
    public const int SCE_DART_OPERATOR = 16;
    public const int SCE_DART_OPERATOR_STRING = 17;
    public const int SCE_DART_SYMBOL_IDENTIFIER = 18;
    public const int SCE_DART_SYMBOL_OPERATOR = 19;
    public const int SCE_DART_NUMBER = 20;
    public const int SCE_DART_KEY = 21;
    public const int SCE_DART_METADATA = 22;
    public const int SCE_DART_KW_PRIMARY = 23;
    public const int SCE_DART_KW_SECONDARY = 24;
    public const int SCE_DART_KW_TERTIARY = 25;
    public const int SCE_DART_KW_TYPE = 26;
    public const int SCE_DART_STRINGEOL = 27;
    
    // Zig
    // ===
    public const int SCE_ZIG_DEFAULT = 0;
    public const int SCE_ZIG_COMMENTLINE = 1;
    public const int SCE_ZIG_COMMENTLINEDOC = 2;
    public const int SCE_ZIG_COMMENTLINETOP = 3;
    public const int SCE_ZIG_NUMBER = 4;
    public const int SCE_ZIG_OPERATOR = 5;
    public const int SCE_ZIG_CHARACTER = 6;
    public const int SCE_ZIG_STRING = 7;
    public const int SCE_ZIG_MULTISTRING = 8;
    public const int SCE_ZIG_ESCAPECHAR = 9;
    public const int SCE_ZIG_IDENTIFIER = 10;
    public const int SCE_ZIG_FUNCTION = 11;
    public const int SCE_ZIG_BUILTIN_FUNCTION = 12;
    public const int SCE_ZIG_KW_PRIMARY = 13;
    public const int SCE_ZIG_KW_SECONDARY = 14;
    public const int SCE_ZIG_KW_TERTIARY = 15;
    public const int SCE_ZIG_KW_TYPE = 16;
    public const int SCE_ZIG_IDENTIFIER_STRING = 17;
    public const int SCE_ZIG_STRINGEOL = 18;
    
    // Nix
    // ===
    public const int SCE_NIX_DEFAULT = 0;
    public const int SCE_NIX_COMMENTLINE = 1;
    public const int SCE_NIX_COMMENTBLOCK = 2;
    public const int SCE_NIX_STRING = 3;
    public const int SCE_NIX_STRING_MULTILINE = 4;
    public const int SCE_NIX_ESCAPECHAR = 5;
    public const int SCE_NIX_IDENTIFIER = 6;
    public const int SCE_NIX_OPERATOR = 7;
    public const int SCE_NIX_OPERATOR_STRING = 8;
    public const int SCE_NIX_NUMBER = 9;
    public const int SCE_NIX_KEY = 10;
    public const int SCE_NIX_PATH = 11;
    public const int SCE_NIX_KEYWORD1 = 12;
    public const int SCE_NIX_KEYWORD2 = 13;
    public const int SCE_NIX_KEYWORD3 = 14;
    public const int SCE_NIX_KEYWORD4 = 15;
    public const int SCE_NIX_STRINGEOL = 16;
    
    // Sinex
    // =====
    public const int SCE_SINEX_DEFAULT = 0;
    public const int SCE_SINEX_COMMENTLINE = 1;
    public const int SCE_SINEX_BLOCK_START = 2;
    public const int SCE_SINEX_BLOCK_END = 3;
    public const int SCE_SINEX_DATE = 4;
    public const int SCE_SINEX_NUMBER = 5;
    
    // ESCSEQ
    // ======
    public const int SCE_ESCSEQ_DEFAULT = 0;
    public const int SCE_ESCSEQ_BLACK_DEFAULT = 1;
    public const int SCE_ESCSEQ_RED_DEFAULT = 2;
    public const int SCE_ESCSEQ_GREEN_DEFAULT = 3;
    public const int SCE_ESCSEQ_YELLOW_DEFAULT = 4;
    public const int SCE_ESCSEQ_BLUE_DEFAULT = 5;
    public const int SCE_ESCSEQ_MAGENTA_DEFAULT = 6;
    public const int SCE_ESCSEQ_CYAN_DEFAULT = 7;
    public const int SCE_ESCSEQ_WHITE_DEFAULT = 8;
    public const int SCE_ESCSEQ_DEFAULT_BLACK = 9;
    public const int SCE_ESCSEQ_BLACK_BLACK = 10;
    public const int SCE_ESCSEQ_RED_BLACK = 11;
    public const int SCE_ESCSEQ_GREEN_BLACK = 12;
    public const int SCE_ESCSEQ_YELLOW_BLACK = 13;
    public const int SCE_ESCSEQ_BLUE_BLACK = 14;
    public const int SCE_ESCSEQ_MAGENTA_BLACK = 15;
    public const int SCE_ESCSEQ_CYAN_BLACK = 16;
    public const int SCE_ESCSEQ_WHITE_BLACK = 17;
    public const int SCE_ESCSEQ_DEFAULT_RED = 18;
    public const int SCE_ESCSEQ_BLACK_RED = 19;
    public const int SCE_ESCSEQ_RED_RED = 20;
    public const int SCE_ESCSEQ_GREEN_RED = 21;
    public const int SCE_ESCSEQ_YELLOW_RED = 22;
    public const int SCE_ESCSEQ_BLUE_RED = 23;
    public const int SCE_ESCSEQ_MAGENTA_RED = 24;
    public const int SCE_ESCSEQ_CYAN_RED = 25;
    public const int SCE_ESCSEQ_WHITE_RED = 26;
    public const int SCE_ESCSEQ_DEFAULT_GREEN = 27;
    public const int SCE_ESCSEQ_BLACK_GREEN = 28;
    public const int SCE_ESCSEQ_RED_GREEN = 29;
    public const int SCE_ESCSEQ_GREEN_GREEN = 30;
    public const int SCE_ESCSEQ_YELLOW_GREEN = 40;
    public const int SCE_ESCSEQ_BLUE_GREEN = 41;
    public const int SCE_ESCSEQ_MAGENTA_GREEN = 42;
    public const int SCE_ESCSEQ_CYAN_GREEN = 43;
    public const int SCE_ESCSEQ_WHITE_GREEN = 44;
    public const int SCE_ESCSEQ_DEFAULT_YELLOW = 45;
    public const int SCE_ESCSEQ_BLACK_YELLOW = 46;
    public const int SCE_ESCSEQ_RED_YELLOW = 47;
    public const int SCE_ESCSEQ_GREEN_YELLOW = 48;
    public const int SCE_ESCSEQ_YELLOW_YELLOW = 49;
    public const int SCE_ESCSEQ_BLUE_YELLOW = 50;
    public const int SCE_ESCSEQ_MAGENTA_YELLOW = 51;
    public const int SCE_ESCSEQ_CYAN_YELLOW = 52;
    public const int SCE_ESCSEQ_WHITE_YELLOW = 53;
    public const int SCE_ESCSEQ_DEFAULT_BLUE = 54;
    public const int SCE_ESCSEQ_BLACK_BLUE = 55;
    public const int SCE_ESCSEQ_RED_BLUE = 56;
    public const int SCE_ESCSEQ_GREEN_BLUE = 57;
    public const int SCE_ESCSEQ_YELLOW_BLUE = 58;
    public const int SCE_ESCSEQ_BLUE_BLUE = 59;
    public const int SCE_ESCSEQ_MAGENTA_BLUE = 60;
    public const int SCE_ESCSEQ_CYAN_BLUE = 61;
    public const int SCE_ESCSEQ_WHITE_BLUE = 62;
    public const int SCE_ESCSEQ_DEFAULT_MAGENTA = 63;
    public const int SCE_ESCSEQ_BLACK_MAGENTA = 64;
    public const int SCE_ESCSEQ_RED_MAGENTA = 65;
    public const int SCE_ESCSEQ_GREEN_MAGENTA = 66;
    public const int SCE_ESCSEQ_YELLOW_MAGENTA = 67;
    public const int SCE_ESCSEQ_BLUE_MAGENTA = 68;
    public const int SCE_ESCSEQ_MAGENTA_MAGENTA = 69;
    public const int SCE_ESCSEQ_CYAN_MAGENTA = 70;
    public const int SCE_ESCSEQ_WHITE_MAGENTA = 71;
    public const int SCE_ESCSEQ_DEFAULT_CYAN = 72;
    public const int SCE_ESCSEQ_BLACK_CYAN = 73;
    public const int SCE_ESCSEQ_RED_CYAN = 74;
    public const int SCE_ESCSEQ_GREEN_CYAN = 75;
    public const int SCE_ESCSEQ_YELLOW_CYAN = 76;
    public const int SCE_ESCSEQ_BLUE_CYAN = 77;
    public const int SCE_ESCSEQ_MAGENTA_CYAN = 78;
    public const int SCE_ESCSEQ_CYAN_CYAN = 79;
    public const int SCE_ESCSEQ_WHITE_CYAN = 80;
    public const int SCE_ESCSEQ_DEFAULT_WHITE = 81;
    public const int SCE_ESCSEQ_BLACK_WHITE = 82;
    public const int SCE_ESCSEQ_RED_WHITE = 83;
    public const int SCE_ESCSEQ_GREEN_WHITE = 84;
    public const int SCE_ESCSEQ_YELLOW_WHITE = 85;
    public const int SCE_ESCSEQ_BLUE_WHITE = 86;
    public const int SCE_ESCSEQ_MAGENTA_WHITE = 87;
    public const int SCE_ESCSEQ_CYAN_WHITE = 88;
    public const int SCE_ESCSEQ_WHITE_WHITE = 89;
    public const int SCE_ESCSEQ_BOLD_DEFAULT = 90;
    public const int SCE_ESCSEQ_BOLD_BLACK_DEFAULT = 91;
    public const int SCE_ESCSEQ_BOLD_RED_DEFAULT = 92;
    public const int SCE_ESCSEQ_BOLD_GREEN_DEFAULT = 93;
    public const int SCE_ESCSEQ_BOLD_YELLOW_DEFAULT = 94;
    public const int SCE_ESCSEQ_BOLD_BLUE_DEFAULT = 95;
    public const int SCE_ESCSEQ_BOLD_MAGENTA_DEFAULT = 96;
    public const int SCE_ESCSEQ_BOLD_CYAN_DEFAULT = 97;
    public const int SCE_ESCSEQ_BOLD_WHITE_DEFAULT = 98;
    public const int SCE_ESCSEQ_BOLD_DEFAULT_BLACK = 99;
    public const int SCE_ESCSEQ_BOLD_BLACK_BLACK = 100;
    public const int SCE_ESCSEQ_BOLD_RED_BLACK = 101;
    public const int SCE_ESCSEQ_BOLD_GREEN_BLACK = 102;
    public const int SCE_ESCSEQ_BOLD_YELLOW_BLACK = 103;
    public const int SCE_ESCSEQ_BOLD_BLUE_BLACK = 104;
    public const int SCE_ESCSEQ_BOLD_MAGENTA_BLACK = 105;
    public const int SCE_ESCSEQ_BOLD_CYAN_BLACK = 106;
    public const int SCE_ESCSEQ_BOLD_WHITE_BLACK = 107;
    public const int SCE_ESCSEQ_BOLD_DEFAULT_RED = 108;
    public const int SCE_ESCSEQ_BOLD_BLACK_RED = 109;
    public const int SCE_ESCSEQ_BOLD_RED_RED = 110;
    public const int SCE_ESCSEQ_BOLD_GREEN_RED = 111;
    public const int SCE_ESCSEQ_BOLD_YELLOW_RED = 112;
    public const int SCE_ESCSEQ_BOLD_BLUE_RED = 113;
    public const int SCE_ESCSEQ_BOLD_MAGENTA_RED = 114;
    public const int SCE_ESCSEQ_BOLD_CYAN_RED = 115;
    public const int SCE_ESCSEQ_BOLD_WHITE_RED = 116;
    public const int SCE_ESCSEQ_BOLD_DEFAULT_GREEN = 117;
    public const int SCE_ESCSEQ_BOLD_BLACK_GREEN = 118;
    public const int SCE_ESCSEQ_BOLD_RED_GREEN = 119;
    public const int SCE_ESCSEQ_BOLD_GREEN_GREEN = 120;
    public const int SCE_ESCSEQ_BOLD_YELLOW_GREEN = 121;
    public const int SCE_ESCSEQ_BOLD_BLUE_GREEN = 122;
    public const int SCE_ESCSEQ_BOLD_MAGENTA_GREEN = 123;
    public const int SCE_ESCSEQ_BOLD_CYAN_GREEN = 124;
    public const int SCE_ESCSEQ_BOLD_WHITE_GREEN = 125;
    public const int SCE_ESCSEQ_BOLD_DEFAULT_YELLOW = 126;
    public const int SCE_ESCSEQ_BOLD_BLACK_YELLOW = 127;
    public const int SCE_ESCSEQ_BOLD_RED_YELLOW = 128;
    public const int SCE_ESCSEQ_BOLD_GREEN_YELLOW = 129;
    public const int SCE_ESCSEQ_BOLD_YELLOW_YELLOW = 130;
    public const int SCE_ESCSEQ_BOLD_BLUE_YELLOW = 131;
    public const int SCE_ESCSEQ_BOLD_MAGENTA_YELLOW = 132;
    public const int SCE_ESCSEQ_BOLD_CYAN_YELLOW = 133;
    public const int SCE_ESCSEQ_BOLD_WHITE_YELLOW = 134;
    public const int SCE_ESCSEQ_BOLD_DEFAULT_BLUE = 135;
    public const int SCE_ESCSEQ_BOLD_BLACK_BLUE = 136;
    public const int SCE_ESCSEQ_BOLD_RED_BLUE = 137;
    public const int SCE_ESCSEQ_BOLD_GREEN_BLUE = 138;
    public const int SCE_ESCSEQ_BOLD_YELLOW_BLUE = 139;
    public const int SCE_ESCSEQ_BOLD_BLUE_BLUE = 140;
    public const int SCE_ESCSEQ_BOLD_MAGENTA_BLUE = 141;
    public const int SCE_ESCSEQ_BOLD_CYAN_BLUE = 142;
    public const int SCE_ESCSEQ_BOLD_WHITE_BLUE = 143;
    public const int SCE_ESCSEQ_BOLD_DEFAULT_MAGENTA = 144;
    public const int SCE_ESCSEQ_BOLD_BLACK_MAGENTA = 145;
    public const int SCE_ESCSEQ_BOLD_RED_MAGENTA = 146;
    public const int SCE_ESCSEQ_BOLD_GREEN_MAGENTA = 147;
    public const int SCE_ESCSEQ_BOLD_YELLOW_MAGENTA = 148;
    public const int SCE_ESCSEQ_BOLD_BLUE_MAGENTA = 149;
    public const int SCE_ESCSEQ_BOLD_MAGENTA_MAGENTA = 150;
    public const int SCE_ESCSEQ_BOLD_CYAN_MAGENTA = 151;
    public const int SCE_ESCSEQ_BOLD_WHITE_MAGENTA = 152;
    public const int SCE_ESCSEQ_BOLD_DEFAULT_CYAN = 153;
    public const int SCE_ESCSEQ_BOLD_BLACK_CYAN = 154;
    public const int SCE_ESCSEQ_BOLD_RED_CYAN = 155;
    public const int SCE_ESCSEQ_BOLD_GREEN_CYAN = 156;
    public const int SCE_ESCSEQ_BOLD_YELLOW_CYAN = 157;
    public const int SCE_ESCSEQ_BOLD_BLUE_CYAN = 158;
    public const int SCE_ESCSEQ_BOLD_MAGENTA_CYAN = 159;
    public const int SCE_ESCSEQ_BOLD_CYAN_CYAN = 160;
    public const int SCE_ESCSEQ_BOLD_WHITE_CYAN = 161;
    public const int SCE_ESCSEQ_BOLD_DEFAULT_WHITE = 162;
    public const int SCE_ESCSEQ_BOLD_BLACK_WHITE = 163;
    public const int SCE_ESCSEQ_BOLD_RED_WHITE = 164;
    public const int SCE_ESCSEQ_BOLD_GREEN_WHITE = 165;
    public const int SCE_ESCSEQ_BOLD_YELLOW_WHITE = 166;
    public const int SCE_ESCSEQ_BOLD_BLUE_WHITE = 167;
    public const int SCE_ESCSEQ_BOLD_MAGENTA_WHITE = 168;
    public const int SCE_ESCSEQ_BOLD_CYAN_WHITE = 169;
    public const int SCE_ESCSEQ_BOLD_WHITE_WHITE = 170;
    public const int SCE_ESCSEQ_IDENTIFIER = 171;
    public const int SCE_ESCSEQ_UNKNOWN = 172;
}
