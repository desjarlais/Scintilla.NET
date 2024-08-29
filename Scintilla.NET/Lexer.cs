namespace ScintillaNET
{
    /// <summary>
    /// Supported lexer names helper.
    /// </summary>
    public enum Lexer
    {
        /// <summary>
        /// Lexer for Assembler, just for the MASM syntax
        /// </summary>
        SCLEX_A68K,
        /// <summary>
        /// Lexer for APDL. Based on the lexer for Assembler by The Black Horus.
        /// </summary>
        SCLEX_ADPL,
        /// <summary>
        /// This lexer is for the Asymptote vector graphics language
        /// </summary>
        SCLEX_ASYMPTOTE,
        /// <summary>
        /// Lexer for AutoIt3
        /// </summary>
        SCLEX_AU3,
        /// <summary>
        /// Lexer for Avenue
        /// </summary>
        SCLEX_AVE,
        /// <summary>
        /// Lexer for AviSynth
        /// </summary>
        SCLEX_AVS,
        /// <summary>
        /// Lexer for ABAQUS. Based on the lexer for APDL by Hadar Raz.
        /// </summary>
        SCLEX_ABAQUS,
        /// <summary>
        /// Lexer for Ada 95
        /// </summary>
        SCLEX_ADA,
        /// <summary>
        /// Lexer for Asciidoc
        /// </summary>
        SCLEX_ASCIIDOC,
        /// <summary>
        /// Lexer for Assembler, just for the MASM syntax
        /// </summary>
        SCLEX_ASM,
        /// <summary>
        /// Lexer for Assembler
        /// </summary>
        SCLEX_AS,
        /// <summary>
        /// Lexer for ASN.1
        /// </summary>
        SCLEX_ASN1,
        /// <summary>
        /// Lexer for Baan.
        /// </summary>
        SCLEX_BAAN,
        /// <summary>
        /// Lexer for Bash.
        /// </summary>
        SCLEX_BASH,
        /// <summary>
        /// Lexer for BlitzBasic.
        /// </summary>
        SCLEX_BLITZBASIC,
        /// <summary>
        /// Lexer for PureBasic.
        /// </summary>
        SCLEX_PUREBASIC,
        /// <summary>
        /// Lexer for FreeBasic.
        /// </summary>
        SCLEX_FREEBASIC,
        /// <summary>
        /// Lexer for batch files
        /// </summary>
        SCLEX_BATCH,
        /// <summary>
        /// Lexer for BibTeX coloring scheme.
        /// </summary>
        SCLEX_BIBTEX,
        /// <summary>
        /// Lexer for Bullant
        /// </summary>
        SCLEX_BULLANT,
        /// <summary>
        /// Lexer for Common Intermediate Language (CIL)
        /// </summary>
        SCLEX_CIL,
        /// <summary>
        /// Case Sensitive Clarion Language Lexer
        /// </summary>
        SCLEX_CLW,
        /// <summary>
        /// Lexer for Clarion with no case sensitivity.
        /// </summary>
        SCLEX_CLWNOCASE,
        /// <summary>
        /// Lexer for COBOL
        /// </summary>
        SCLEX_COBOL,
        /// <summary>
        /// Lexer for Case Sensitive C++
        /// </summary>
        SCLEX_CPP,
        /// <summary>
        /// Lexer for no case sensitivity C++
        /// </summary>
        SCLEX_CPPNOCASE,
        /// <summary>
        /// Lexer for C#
        /// </summary>
        SCLEX_CSHARP,
        /// <summary>
        /// Lexer for Cascading Style Sheets
        /// </summary>
        SCLEX_CSS,
        /// <summary>
        /// Lexer for Java
        /// </summary>
        SCLEX_JAVA,
        /// <summary>
        /// Lexer for JavaScript
        /// </summary>
        SCLEX_JAVASCRIPT,
        /// <summary>
        /// Lexer for Objective Caml.
        /// </summary>
        SCLEX_CAML,
        /// <summary>
        /// Lexer for Cmake
        /// </summary>
        SCLEX_CMAKE,
        /// <summary>
        /// Lexer for CoffeeScript.
        /// </summary>
        SCLEX_COFFEESCRIPT,
        /// <summary>
        /// Apache Configuration Files
        /// </summary>
        SCLEX_CONF,
        /// <summary>
        /// Lexer to use with extended crontab files used by a powerful
        /// Windows scheduler/event monitor/automation manager nnCron.
        /// </summary>
        SCLEX_NNCRONTAB,
        /// <summary>
        /// Lexer for Csound (Orchestra and Score)
        /// </summary>
        SCLEX_CSOUND,
        /// <summary>
        /// Lexer for D
        /// </summary>
        SCLEX_D,
        /// <summary>
        /// Lexer for MSC Nastran DMAP.
        /// </summary>
        SCLEX_DMAP,
        /// <summary>
        /// Lexer for DMIS.
        /// </summary>
        SCLEX_DMIS,
        /// <summary>
        /// Lexer for DataFlex.
        /// </summary>
        SCLEX_DATAFLEX,
        /// <summary>
        /// Lexer for diff results.
        /// </summary>
        SCLEX_DIFF,
        /// <summary>
        /// Lexer for ECL.
        /// </summary>
        SCLEX_ECL,
        /// <summary>
        /// Lexer for EDIFACT
        /// </summary>
        SCLEX_EDIFACT,
        /// <summary>
        /// Lexer for ESCRIPT
        /// </summary>
        SCLEX_ESCRIPT,
        /// <summary>
        /// Lexer for Eiffel.
        /// </summary>
        SCLEX_EIFFEL,
        /// <summary>
        /// Lexer for EiffelKW
        /// </summary>
        SCLEX_EIFFELKW,
        /// <summary>
        /// Lexer for Erlang.
        /// </summary>
        SCLEX_ERLANG,
        /// <summary>
        /// Lexer for error lists. Used for the output pane in SciTE
        /// </summary>
        SCLEX_ERRORLIST,
        /// <summary>
        /// Lexer for F# 5.0
        /// </summary>
        SCLEX_FSHARP,
        /// <summary>
        /// Lexer for Harbour and FlagShip.
        /// </summary>
        SCLEX_FLAGSHIP,
        /// <summary>
        /// Lexer for Forth
        /// </summary>
        SCLEX_FORTH,
        /// <summary>
        /// Lexer for Fortran
        /// </summary>
        SCLEX_FORTRAN,
        /// <summary>
        /// Lexer for Fortran 77
        /// </summary>
        SCLEX_F77,
        /// <summary>
        /// Lexer for the GAP language. (The GAP System for Computational Discrete Algebra)
        /// </summary>
        SCLEX_GAP,
        /// <summary>
        /// Lexer for GDScript.
        /// </summary>
        SCLEX_GDSCRIPT,
        /// <summary>
        /// This is the Lexer for Gui4Cli, included in SciLexer.dll
        /// </summary>
        SCLEX_GUI4CLI,
        /// <summary>
        /// Lexer for HTML
        /// </summary>
        SCLEX_HTML,
        /// <summary>
        /// Lexer for XML
        /// </summary>
        SCLEX_XML,
        /// <summary>
        /// Lexer for PHPScript
        /// </summary>
        SCLEX_PHPSCRIPT,
        /// <summary>
        /// A haskell lexer for the scintilla code control.
        /// </summary>
        SCLEX_HASKELL,
        /// <summary>
        /// Lexer for Literate Haskell
        /// </summary>
        SCLEX_LITERATEHASKELL,
        /// <summary>
        /// Lexer for Motorola S-Record.
        /// </summary>
        SCLEX_SREC,
        /// <summary>
        /// Lexer for Intel HEX
        /// </summary>
        SCLEX_IHEX,
        /// <summary>
        /// Lexer for Tektronix extended HEX
        /// </summary>
        SCLEX_TEHEX,
        /// <summary>
        /// Lexer for Hollywood
        /// </summary>
        SCLEX_HOLLYWOOD,
        /// <summary>
        /// Lexer for no language. Used for indentation-based folding of files.
        /// </summary>
        SCLEX_INDENT,
        /// <summary>
        /// Lexer for Inno Setup scripts.
        /// </summary>
        SCLEX_INNOSETUP,
        /// <summary>
        /// Lexer for JSON and JSON-LD formats
        /// </summary>
        SCLEX_JSON,
        /// <summary>
        /// Lexer for Julia
        /// </summary>
        SCLEX_JULIA,
        /// <summary>
        /// Lexer for KIX-Scripts.
        /// </summary>
        SCLEX_KIX,
        /// <summary>
        /// Lexer for KVIrc script.
        /// </summary>
        SCLEX_KVIRC,
        /// <summary>
        /// Lexer for LaTeX2e.
        /// </summary>
        SCLEX_LATEX,
        /// <summary>
        /// Lexer for Lisp
        /// </summary>
        SCLEX_LISP,
        /// <summary>
        /// Lexer for the Basser Lout (>= version 3) typesetting language
        /// </summary>
        SCLEX_LOUT,
        /// <summary>
        /// Lexer for Lua language
        /// </summary>
        SCLEX_LUA,
        /// <summary>
        /// Lexer for MMIX Assembler Language.
        /// </summary>
        SCLEX_MMIXAL,
        /// <summary>
        /// Lexer for MPT specific files. Based on LexOthers.cxx
        /// LOT = the text log file created by the MPT application while running a test program
        /// </summary>
        SCLEX_LOT,
        /// <summary>
        /// Lexer for MSSQL
        /// </summary>
        SCLEX_MSSQL,
        /// <summary>
        /// Lexer for GE(r) Smallworld(tm) MagikSF
        /// </summary>
        SCLEX_MAGIK,
        /// <summary>
        /// Lexer for make files.
        /// </summary>
        SCLEX_MAKEFILE,
        /// <summary>
        /// A simple Markdown lexer for scintilla.
        /// </summary>
        SCLEX_MARKDOWN,
        /// <summary>
        /// Lexer for Matlab.
        /// </summary>
        SCLEX_MATLAB,
        /// <summary>
        /// Lexer for Octave
        /// </summary>
        SCLEX_OCTAVE,
        /// <summary>
        /// Lexer for Maxima 
        /// </summary>
        SCLEX_MAXIMA,
        /// <summary>
        /// Lexer for general context conformant metapost coloring scheme
        /// </summary>
        SCLEX_METAPOST,
        /// <summary>
        /// Lexer for Modula-2/3 documents.
        /// </summary>
        SCLEX_MODULA,
        /// <summary>
        /// Lexer for MySQL
        /// </summary>
        SCLEX_MYSQL,
        /// <summary>
        /// Lexer for NIM
        /// </summary>
        SCLEX_NIM,
        /// <summary>
        /// Lexer for NIMROD
        /// </summary>
        SCLEX_NIMROD,
        /// <summary>
        /// Lexer for NSIS
        /// </summary>
        SCLEX_NSIS,
        /// <summary>
        /// Lexer for no language. Used for plain text and unrecognized files.
        /// </summary>
        SCLEX_NULL,
        /// <summary>
        /// Lexer for OScript sources; ocx files and/or OSpace dumps.
        /// </summary>
        SCLEX_OSCRIPT,
        /// <summary>
        /// Lexer for OPAL (functional language similar to Haskell)
        /// </summary>
        SCLEX_OPAL,
        /// <summary>
        /// Lexer for PowerBasic 
        /// </summary>
        SCLEX_POWERBASIC,
        /// <summary>
        /// Lexer for PL/M
        /// </summary>
        SCLEX_PLM,
        /// <summary>
        /// Lexer for GetText Translation (PO) files.
        /// </summary>
        SCLEX_PO,
        /// <summary>
        /// Lexer for POV-Ray SDL
        /// </summary>
        SCLEX_POV,
        /// <summary>
        /// Lexer for PostScript
        /// </summary>
        SCLEX_POSTSCRIPT,
        /// <summary>
        /// Lexer for Pascal
        /// </summary>
        SCLEX_PASCAL,
        /// <summary>
        /// Lexer for Perl
        /// </summary>
        SCLEX_PERL,
        /// <summary>
        /// Lexer for PowerPro
        /// </summary>
        SCLEX_POWERPRO,
        /// <summary>
        /// Lexer for PowerShellj scripts
        /// </summary>
        SCLEX_POWERSHELL,
        /// <summary>
        /// Lexer for Progress 4GL.
        /// </summary>
        SCLEX_PROGRESS,
        /// <summary>
        /// Lexer for properties files.
        /// </summary>
        SCLEX_PROPERTIES,
        /// <summary>
        /// Lexer for Python
        /// </summary>
        SCLEX_PYTHON,
        /// <summary>
        /// Lexer for R.
        /// </summary>
        SCLEX_R,
        /// <summary>
        /// Lexer for S.
        /// </summary>
        SCLEX_S,
        /// <summary>
        /// Lexer for SPlus Statistics Program.
        /// </summary>
        SCLEX_SPLUS,
        /// <summary>
        /// Lexer for Raku
        /// </summary>
        SCLEX_RAKU,
        /// <summary>
        /// Lexer for REBOL
        /// </summary>
        SCLEX_REBOL,
        /// <summary>
        /// Lexer for Windows registration files(.reg)
        /// </summary>
        SCLEX_REGISTRY,
        /// <summary>
        /// Lexer for Ruby
        /// </summary>
        SCLEX_RUBY,
        /// <summary>
        /// Lexer for Rust
        /// </summary>
        SCLEX_RUST,
        /// <summary>
        /// Lexer for SAS
        /// </summary>
        SCLEX_SAS,
        /// <summary>
        /// Lexer for SML
        /// </summary>
        SCLEX_SML,
        /// <summary>
        /// Lexer for SQL, including PL/SQL and SQL*Plus.
        /// </summary>
        SCLEX_SQL,
        /// <summary>
        /// Lexer for Structured Text language.
        /// </summary>
        SCLEX_STTXT,
        /// <summary>
        /// Lexer for Scriptol
        /// </summary>
        SCLEX_SCRIPTOL,
        /// <summary>
        /// Lexer for SmallTalk
        /// </summary>
        SCLEX_SMALLTALK,
        /// <summary>
        /// Lexer for SORCUS installation files
        /// </summary>
        SCLEX_SORCUS,
        /// <summary>
        /// Lexer for Specman E language.
        /// </summary>
        SCLEX_SPECMAN,
        /// <summary>
        /// Lexer for SPICE
        /// </summary>
        SCLEX_SPICE,
        /// <summary>
        /// Lexer for Stata
        /// </summary>
        SCLEX_STATA,
        /// <summary>
        /// Lexer for TACL
        /// </summary>
        SCLEX_TACL,
        /// <summary>
        /// Lexer for TADS3
        /// </summary>
        SCLEX_TADS3,
        /// <summary>
        /// Lexer for TAL
        /// </summary>
        SCLEX_TAL,
        /// <summary>
        /// Lexer for TCL
        /// </summary>
        SCLEX_TCL,
        /// <summary>
        /// Lexer for Take Command / TCC batch scripts (.bat, .btm, .cmd).
        /// </summary>
        SCLEX_TCMD,
        /// <summary>
        /// Lexer for LaTeX general context conformant tex coloring scheme
        /// </summary>
        SCLEX_TEX,
        /// <summary>
        /// A simple Txt2tags lexer for scintilla.
        /// </summary>
        SCLEX_TXT2TAGS,
        /// <summary>
        /// Visual Basic
        /// </summary>
        SCLEX_VB,
        /// <summary>
        /// Visual Basic Script
        /// </summary>
        SCLEX_VBSCRIPT,
        /// <summary>
        /// Lexer for vhdl
        /// </summary>
        SCLEX_VHDL,
        /// <summary>
        /// Lexer for Verilog
        /// </summary>
        SCLEX_VERILOG,
        /// <summary>
        /// Lexer for Visual Prolog
        /// </summary>
        SCLEX_VISUALPROLOG,
        /// <summary>
        /// Lexer for X12
        /// </summary>
        SCLEX_X12,
        /// <summary>
        /// Lexer for Yaml
        /// </summary>
        SCLEX_YAML,
    }
}
