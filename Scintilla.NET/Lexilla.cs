using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace ScintillaNET;

/// <summary>
/// A class containing methods for interacting with the Lexilla library.
/// </summary>
public class Lexilla
{
    /// <summary>
    /// Initializes the Lexilla.dll library.
    /// </summary>
    /// <param name="lexillaHandle">The handle to the Lexilla.dll file.</param>
    internal Lexilla(FreeLibrarySafeHandle lexillaHandle)
    {
        const string win32Error = "The Scintilla module has no export for the '{0}' procedure.";

        string lpProcName = nameof(LexApi.CreateLexer);

        // Get the Lexilla functions needed to define lexers and create managed callbacks...

        FARPROC functionPointer = PInvoke.GetProcAddress(lexillaHandle, lpProcName);
        if (functionPointer == IntPtr.Zero)
        {
            throw new Win32Exception(string.Format(win32Error, lpProcName),
                new Win32Exception()); // Calls GetLastError
        }

        createLexer = Marshal.GetDelegateForFunctionPointer<LexApi.CreateLexer>(functionPointer);

        lpProcName = nameof(LexApi.GetLexerName);

        functionPointer = PInvoke.GetProcAddress(lexillaHandle, lpProcName);
        if (functionPointer == IntPtr.Zero)
        {
            throw new Win32Exception(string.Format(win32Error, lpProcName),
                new Win32Exception()); // Calls GetLastError
        }

        getLexerName = Marshal.GetDelegateForFunctionPointer<LexApi.GetLexerName>(functionPointer);

        lpProcName = nameof(LexApi.GetLexerCount);

        functionPointer = PInvoke.GetProcAddress(lexillaHandle, lpProcName);
        if (functionPointer == IntPtr.Zero)
        {
            throw new Win32Exception(string.Format(win32Error, lpProcName),
                new Win32Exception()); // Calls GetLastError
        }

        getLexerCount = Marshal.GetDelegateForFunctionPointer<LexApi.GetLexerCount>(functionPointer);

        #pragma warning disable CS0618 // Type or member is obsolete
        lpProcName = nameof(LexApi.LexerNameFromID);

        functionPointer = PInvoke.GetProcAddress(lexillaHandle, lpProcName);
        if (functionPointer == IntPtr.Zero)
        {
            throw new Win32Exception(string.Format(win32Error, lpProcName),
                new Win32Exception()); // Calls GetLastError
        }

        lexerNameFromId = Marshal.GetDelegateForFunctionPointer<LexApi.LexerNameFromID>(functionPointer);
        #pragma warning restore CS0618 // Type or member is obsolete

        //initialized = true;
    }

    #region Fields

    //private static bool initialized;

    #endregion

    #region DllCalls
    private static LexApi.GetLexerCount getLexerCount;

    private static LexApi.GetLexerName getLexerName;

    private static LexApi.CreateLexer createLexer;

    [Obsolete("Depracted")]
    private static LexApi.LexerNameFromID lexerNameFromId;
    #endregion

    #region DllWrapping

    /// <summary>
    /// Gets the lexer count in the Lexilla library.
    /// </summary>
    /// <returns>Amount of lexers defined in the Lexilla library.</returns>
    public static int GetLexerCount()
    {
        return getLexerCount();
    }

    /// <summary>
    /// Creates a lexer with the specified name.
    /// </summary>
    /// <param name="lexerName">The name of the lexer to create.</param>
    /// <returns>A <see cref="IntPtr"/> containing the lexer interface pointer.</returns>
    public static IntPtr CreateLexer(string lexerName)
    {
        return createLexer(lexerName);
    }

    /// <summary>
    /// Gets the name of the lexer specified by an index number.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The name of the lexer if one was found with the specified index; <c>null</c> otherwise.</returns>
    public static string GetLexerName(uint index)
    {
        byte[] name = new byte[128];
        unsafe
        {
            fixed (byte* namePtr = name)
                getLexerName(index, namePtr, name.Length);
        }
        int count = 0;
        while (count < name.Length && name[count] != 0)
            count++;
        return Encoding.ASCII.GetString(name, 0, count);
    }

    /// <summary>
    /// Returns a lexer name with the specified identifier.
    /// </summary>
    /// <param name="identifier">The lexer identifier.</param>
    /// <returns>The name of the lexer if one was found with the specified identifier; <c>null</c> otherwise.</returns>
    [Obsolete("Depracted")]
    public static string LexerNameFromId(int identifier)
    {
        return lexerNameFromId(identifier);
    }

    /// <summary>
    /// Gets the lexer names contained in the Lexilla library.
    /// </summary>
    /// <returns>An <c>IEnumerable&lt;string&gt;</c> value with the lexer names.</returns>
    public static IEnumerable<string> GetLexerNames()
    {
        int count = GetLexerCount();
        for (int i = 0; i < count; i++)
        {
            yield return GetLexerName((uint)i);
        }
    }
    #endregion
}
