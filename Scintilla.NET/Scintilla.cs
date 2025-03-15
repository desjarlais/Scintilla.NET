using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ScintillaNET
{
    /// <summary>
    /// Represents a Scintilla editor control.
    /// </summary>
    [Docking(DockingBehavior.Ask)]
    [Designer(typeof(ScintillaDesigner))]
    public class Scintilla : Control
    {
        static Scintilla()
        {
            List<string> searchedPathList = [];
            foreach (string path in EnumerateSatelliteLibrarySearchPaths())
            {
                string scintillaDllPath = Path.Combine(path, "Scintilla.dll");
                string lexillaDllPath = Path.Combine(path, "Lexilla.dll");
                if (File.Exists(scintillaDllPath) && File.Exists(lexillaDllPath))
                {
                    modulePathScintilla = scintillaDllPath;
                    modulePathLexilla = lexillaDllPath;
                    try
                    {
                        var info = FileVersionInfo.GetVersionInfo(modulePathScintilla);
                        scintillaVersion = info.ProductVersion ?? info.FileVersion;
                        info = FileVersionInfo.GetVersionInfo(modulePathLexilla);
                        lexillaVersion = info.ProductVersion ?? info.FileVersion;
                        return;
                    }
                    catch
                    {
                        searchedPathList.Add(path);
                    }
                }
                else
                {
                    searchedPathList.Add(path);
                }
            }

            string searchedPaths = string.Join("\n", searchedPathList);

            scintillaVersion = "ERROR";
            lexillaVersion = "ERROR";
            // the path to the following .NET or .NET Framework satellite assemblies exists but the assemblies are not found in the directory.
            // (surely a problem in the package itself or in its installation of the project).
            throw new InvalidOperationException($"Scintilla.NET satellite assemblies not found in any of the following paths:\n{searchedPaths}");
        }

        private static bool InDesignProcess()
        {
            using var proc = Process.GetCurrentProcess();
            string procName = proc.ProcessName;
            return
                procName is "devenv" or "DesignToolsServer" or // WinForms app in VS IDE
                "xdesproc" or // WPF app in VS IDE/Blend
                "blend";
        }

        /// <summary>
        /// Enumerates a list of folder paths that the native satellite libraries
        /// ('Scintilla.dll' &amp; 'Lexilla.dll') are searched in.
        /// </summary>
        public static IEnumerable<string> EnumerateSatelliteLibrarySearchPaths()
        {
            // check run-time paths
            string folder = Path.Combine("runtimes", "win-" + Helpers.GetArchitectureRid(WinApiHelpers.GetProcessArchitecture()), "native");
            string location = Assembly.GetExecutingAssembly().Location;
            if (string.IsNullOrWhiteSpace(location))
                location = Assembly.GetEntryAssembly().Location;
            string managedLocation = Path.GetDirectoryName(location) ?? AppDomain.CurrentDomain.BaseDirectory;
            yield return Path.Combine(managedLocation, folder);

            if (InDesignProcess())
            {
                // Look for the assemblies in the nuget global packages folder
                var designtimeAssembly = Assembly.GetAssembly(typeof(Scintilla));
                string nugetScintillaPackageFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @".nuget\packages\scintilla5.net");
                Version packageVersion = designtimeAssembly.GetName().Version;
                string versionString = packageVersion.Revision == 0 ? packageVersion.ToString(3) : packageVersion.ToString();
                yield return Path.Combine(nugetScintillaPackageFolder, versionString, folder);

                // then check the project folder using the Scintilla.NET assembly location
                // move up a few levels to the host project folder and append the location nuget used at install
                string nugetScintillaNETLocation = designtimeAssembly.Location;
                string nugetScintillaPackageName = designtimeAssembly.GetName().Name;
                string rootProjectFolder = Path.GetFullPath(Path.Combine(nugetScintillaNETLocation, @"..\..\..\.."));
                yield return Path.Combine(rootProjectFolder, "packages", nugetScintillaPackageName + "." + versionString, folder);
            }
        }

        #region Fields

        // WM_DESTROY workaround
        private static bool? reparentAll;
        private bool reparent;

        // Static module data
        private static readonly string modulePathScintilla;
        private static readonly string modulePathLexilla;

        private static IntPtr moduleHandle;
        private static NativeMethods.Scintilla_DirectFunction directFunction;
        private static IntPtr lexillaHandle;
        private static Lexilla lexilla;

        // Events
        private static readonly object scNotificationEventKey = new();
        private static readonly object insertCheckEventKey = new();
        private static readonly object beforeInsertEventKey = new();
        private static readonly object beforeDeleteEventKey = new();
        private static readonly object insertEventKey = new();
        private static readonly object deleteEventKey = new();
        private static readonly object updateUIEventKey = new();
        private static readonly object modifyAttemptEventKey = new();
        private static readonly object styleNeededEventKey = new();
        private static readonly object savePointReachedEventKey = new();
        private static readonly object savePointLeftEventKey = new();
        private static readonly object changeAnnotationEventKey = new();
        private static readonly object marginClickEventKey = new();
        private static readonly object marginRightClickEventKey = new();
        private static readonly object charAddedEventKey = new();
        private static readonly object autoCSelectionEventKey = new();
        private static readonly object autoCSelectionChangeEventKey = new();
        private static readonly object autoCCompletedEventKey = new();
        private static readonly object autoCCancelledEventKey = new();
        private static readonly object autoCCharDeletedEventKey = new();
        private static readonly object dwellStartEventKey = new();
        private static readonly object callTipClickEventKey = new();
        private static readonly object dwellEndEventKey = new();
        private static readonly object borderStyleChangedEventKey = new();
        private static readonly object doubleClickEventKey = new();
        private static readonly object paintedEventKey = new();
        private static readonly object needShownEventKey = new();
        private static readonly object hotspotClickEventKey = new();
        private static readonly object hotspotDoubleClickEventKey = new();
        private static readonly object hotspotReleaseClickEventKey = new();
        private static readonly object indicatorClickEventKey = new();
        private static readonly object indicatorReleaseEventKey = new();
        private static readonly object zoomChangedEventKey = new();

        // The goods
        private IntPtr sciPtr;
        private BorderStyle borderStyle;

        // Set style
        private int stylingPosition;
        private int stylingBytePosition;

        // Modified event optimization
        private int? cachedPosition;
        private string cachedText;

        // Double-click
        private bool doubleClick;

        // Pinned data
        private IntPtr fillUpChars;

        // For highlight calculations
        private string lastCallTip = string.Empty;

        // For custom rendering of the 3D border
        private VisualStyleRenderer renderer;

        /// <summary>
        /// A constant used to specify an infinite mouse dwell wait time.
        /// </summary>
        public const int TimeForever = NativeMethods.SC_TIME_FOREVER;

        /// <summary>
        /// A constant used to specify an invalid document position.
        /// </summary>
        public const int InvalidPosition = NativeMethods.INVALID_POSITION;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Sets the name of the lexer by its name.
        /// </summary>
        /// <param name="lexerName">Name of the lexer.</param>
        /// <returns><c>true</c> if the lexer was successfully set, <c>false</c> otherwise.</returns>
        private bool SetLexerByName(string lexerName)
        {
            // this is a special case reserved for the container to do the styling, allowing the StyleNeeded notification to be sent
            // each time text needs styling for display, note from scintilla docs:
            // "If you choose to use the container to do the styling you can use the SCI_SETILEXER command to select NULL,
            // in which case the container is sent a SCN_STYLENEEDED notification each time text needs styling for display."
            if (lexerName == string.Empty)
            {
                DirectMessage(NativeMethods.SCI_SETILEXER, IntPtr.Zero, IntPtr.Zero);
                return true;
            }

            IntPtr ptr = Lexilla.CreateLexer(lexerName);

            if (ptr == IntPtr.Zero)
            {
                return false;
            }

            DirectMessage(NativeMethods.SCI_SETILEXER, IntPtr.Zero, ptr);

            return true;
        }

        /// <summary>
        /// Increases the reference count of the specified document by 1.
        /// </summary>
        /// <param name="document">The document reference count to increase.</param>
        public void AddRefDocument(Document document)
        {
            IntPtr ptr = document.Value;
            DirectMessage(NativeMethods.SCI_ADDREFDOCUMENT, IntPtr.Zero, ptr);
        }

        /// <summary>
        /// Adds an additional selection range to the existing main selection.
        /// </summary>
        /// <param name="caret">The zero-based document position to end the selection.</param>
        /// <param name="anchor">The zero-based document position to start the selection.</param>
        /// <remarks>A main selection must first have been set by a call to <see cref="SetSelection" />.</remarks>
        public void AddSelection(int caret, int anchor)
        {
            int textLength = TextLength;
            caret = Helpers.Clamp(caret, 0, textLength);
            anchor = Helpers.Clamp(anchor, 0, textLength);

            caret = Lines.CharToBytePosition(caret);
            anchor = Lines.CharToBytePosition(anchor);

            DirectMessage(NativeMethods.SCI_ADDSELECTION, new IntPtr(caret), new IntPtr(anchor));
        }

        /// <summary>
        /// Inserts the specified text at the current caret position.
        /// </summary>
        /// <param name="text">The text to insert at the current caret position.</param>
        /// <remarks>The caret position is set to the end of the inserted text, but it is not scrolled into view.</remarks>
        public unsafe void AddText(string text)
        {
            byte[] bytes = Helpers.GetBytes(text ?? string.Empty, Encoding, zeroTerminated: false);
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_ADDTEXT, new IntPtr(bytes.Length), new IntPtr(bp));
        }

        /// <summary>
        /// Allocates some number of substyles for a particular base style. Substyles are allocated contiguously.
        /// </summary>
        /// <param name="styleBase">The lexer style integer</param>
        /// <param name="numberStyles">The amount of substyles to allocate</param>
        /// <returns>Returns the first substyle number allocated.</returns>
        public int AllocateSubstyles(int styleBase, int numberStyles)
        {
            return DirectMessage(NativeMethods.SCI_ALLOCATESUBSTYLES, new IntPtr(styleBase), new IntPtr(numberStyles)).ToInt32();
        }

        /// <summary>
        /// Removes the annotation text for every <see cref="Line" /> in the document.
        /// </summary>
        public void AnnotationClearAll()
        {
            DirectMessage(NativeMethods.SCI_ANNOTATIONCLEARALL);
        }

        /// <summary>
        /// Adds the specified text to the end of the document.
        /// </summary>
        /// <param name="text">The text to add to the document.</param>
        /// <remarks>The current selection is not changed and the new text is not scrolled into view.</remarks>
        public unsafe void AppendText(string text)
        {
            byte[] bytes = Helpers.GetBytes(text ?? string.Empty, Encoding, zeroTerminated: false);
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_APPENDTEXT, new IntPtr(bytes.Length), new IntPtr(bp));
        }

        /// <summary>
        /// Assigns the specified key definition to a <see cref="Scintilla" /> command.
        /// </summary>
        /// <param name="keyDefinition">The key combination to bind.</param>
        /// <param name="sciCommand">The command to assign.</param>
        public void AssignCmdKey(Keys keyDefinition, Command sciCommand)
        {
            int keys = Helpers.TranslateKeys(keyDefinition);
            DirectMessage(NativeMethods.SCI_ASSIGNCMDKEY, new IntPtr(keys), new IntPtr((int)sciCommand));
        }

        /// <summary>
        /// Cancels any displayed autocompletion list.
        /// </summary>
        /// <seealso cref="AutoCStops" />
        public void AutoCCancel()
        {
            DirectMessage(NativeMethods.SCI_AUTOCCANCEL);
        }

        /// <summary>
        /// Triggers completion of the current autocompletion word.
        /// </summary>
        public void AutoCComplete()
        {
            DirectMessage(NativeMethods.SCI_AUTOCCOMPLETE);
        }

        /// <summary>
        /// Selects an item in the autocompletion list.
        /// </summary>
        /// <param name="select">
        /// The autocompletion word to select.
        /// If found, the word in the autocompletion list is selected and the index can be obtained by calling <see cref="AutoCCurrent" />.
        /// If not found, the behavior is determined by <see cref="AutoCAutoHide" />.
        /// </param>
        /// <remarks>
        /// Comparisons are performed according to the <see cref="AutoCIgnoreCase" /> property
        /// and will match the first word starting with <paramref name="select" />.
        /// </remarks>
        /// <seealso cref="AutoCCurrent" />
        /// <seealso cref="AutoCAutoHide" />
        /// <seealso cref="AutoCIgnoreCase" />
        public unsafe void AutoCSelect(string select)
        {
            byte[] bytes = Helpers.GetBytes(select, Encoding, zeroTerminated: true);
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_AUTOCSELECT, IntPtr.Zero, new IntPtr(bp));
        }

        /// <summary>
        /// Sets the characters that, when typed, cause the autocompletion item to be added to the document.
        /// </summary>
        /// <param name="chars">A string of characters that trigger autocompletion. The default is null.</param>
        /// <remarks>Common fillup characters are '(', '[', and '.' depending on the language.</remarks>
        public unsafe void AutoCSetFillUps(string chars)
        {
            // Apparently Scintilla doesn't make a copy of our fill up string; it just keeps a pointer to it....
            // That means we need to keep a copy of the string around for the life of the control AND put it
            // in a place where it won't get moved by the GC.

            chars ??= string.Empty;

            if (this.fillUpChars != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.fillUpChars);
                this.fillUpChars = IntPtr.Zero;
            }

            int count = Encoding.GetByteCount(chars) + 1;
            IntPtr newFillUpChars = Marshal.AllocHGlobal(count);
            fixed (char* ch = chars)
                Encoding.GetBytes(ch, chars.Length, (byte*)newFillUpChars, count);

            ((byte*)newFillUpChars)[count - 1] = 0; // Null terminate
            this.fillUpChars = newFillUpChars;

            // var str = new String((sbyte*)fillUpChars, 0, count, Encoding);

            DirectMessage(NativeMethods.SCI_AUTOCSETFILLUPS, IntPtr.Zero, this.fillUpChars);
        }

        /// <summary>
        /// Displays an auto completion list.
        /// </summary>
        /// <param name="lenEntered">The number of characters already entered to match on.</param>
        /// <param name="list">A list of autocompletion words separated by the <see cref="AutoCSeparator" /> character.</param>
        public unsafe void AutoCShow(int lenEntered, string list)
        {
            if (string.IsNullOrEmpty(list))
                return;

            lenEntered = Helpers.ClampMin(lenEntered, 0);
            if (lenEntered > 0)
            {
                // Convert to bytes by counting back the specified number of characters
                int endPos = DirectMessage(NativeMethods.SCI_GETCURRENTPOS).ToInt32();
                int startPos = endPos;
                for (int i = 0; i < lenEntered; i++)
                    startPos = DirectMessage(NativeMethods.SCI_POSITIONRELATIVE, new IntPtr(startPos), new IntPtr(-1)).ToInt32();

                lenEntered = endPos - startPos;
            }

            byte[] bytes = Helpers.GetBytes(list, Encoding, zeroTerminated: true);
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_AUTOCSHOW, new IntPtr(lenEntered), new IntPtr(bp));
        }

        /// <summary>
        /// Specifies the characters that will automatically cancel autocompletion without the need to call <see cref="AutoCCancel" />.
        /// </summary>
        /// <param name="chars">A String of the characters that will cancel autocompletion. The default is empty.</param>
        /// <remarks>Characters specified should be limited to printable ASCII characters.</remarks>
        public unsafe void AutoCStops(string chars)
        {
            byte[] bytes = Helpers.GetBytes(chars ?? string.Empty, Encoding.ASCII, zeroTerminated: true);
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_AUTOCSTOPS, IntPtr.Zero, new IntPtr(bp));
        }

        /// <summary>
        /// Marks the beginning of a set of actions that should be treated as a single undo action.
        /// </summary>
        /// <remarks>A call to <see cref="BeginUndoAction" /> should be followed by a call to <see cref="EndUndoAction" />.</remarks>
        /// <seealso cref="EndUndoAction" />
        public void BeginUndoAction()
        {
            DirectMessage(NativeMethods.SCI_BEGINUNDOACTION);
        }

        /// <summary>
        /// Styles the specified character position with the <see cref="Style.BraceBad" /> style when there is an unmatched brace.
        /// </summary>
        /// <param name="position">The zero-based document position of the unmatched brace character or <seealso cref="InvalidPosition"/> to remove the highlight.</param>
        public void BraceBadLight(int position)
        {
            position = Helpers.Clamp(position, -1, TextLength);
            if (position > 0)
                position = Lines.CharToBytePosition(position);

            DirectMessage(NativeMethods.SCI_BRACEBADLIGHT, new IntPtr(position));
        }

        /// <summary>
        /// Styles the specified character positions with the <see cref="Style.BraceLight" /> style.
        /// </summary>
        /// <param name="position1">The zero-based document position of the open brace character.</param>
        /// <param name="position2">The zero-based document position of the close brace character.</param>
        /// <remarks>Brace highlighting can be removed by specifying <see cref="InvalidPosition" /> for <paramref name="position1" /> and <paramref name="position2" />.</remarks>
        /// <seealso cref="HighlightGuide" />
        public void BraceHighlight(int position1, int position2)
        {
            int textLength = TextLength;

            position1 = Helpers.Clamp(position1, -1, textLength);
            if (position1 > 0)
                position1 = Lines.CharToBytePosition(position1);

            position2 = Helpers.Clamp(position2, -1, textLength);
            if (position2 > 0)
                position2 = Lines.CharToBytePosition(position2);

            DirectMessage(NativeMethods.SCI_BRACEHIGHLIGHT, new IntPtr(position1), new IntPtr(position2));
        }

        /// <summary>
        /// Finds a corresponding matching brace starting at the position specified.
        /// The brace characters handled are '(', ')', '[', ']', '{', '}', '&lt;', and '&gt;'.
        /// </summary>
        /// <param name="position">The zero-based document position of a brace character to start the search from for a matching brace character.</param>
        /// <returns>The zero-based document position of the corresponding matching brace or <see cref="InvalidPosition" /> it no matching brace could be found.</returns>
        /// <remarks>A match only occurs if the style of the matching brace is the same as the starting brace. Nested braces are handled correctly.</remarks>
        public int BraceMatch(int position)
        {
            position = Helpers.Clamp(position, 0, TextLength);
            position = Lines.CharToBytePosition(position);

            int match = DirectMessage(NativeMethods.SCI_BRACEMATCH, new IntPtr(position), IntPtr.Zero).ToInt32();
            if (match > 0)
                match = Lines.ByteToCharPosition(match);

            return match;
        }

        /// <summary>
        /// Cancels the display of a call tip window.
        /// </summary>
        public void CallTipCancel()
        {
            DirectMessage(NativeMethods.SCI_CALLTIPCANCEL);
        }

        /// <summary>
        /// Sets the color of highlighted text in a call tip.
        /// </summary>
        /// <param name="color">The new highlight text Color. The default is dark blue.</param>
        public void CallTipSetForeHlt(Color color)
        {
            int colour = HelperMethods.ToWin32ColorOpaque(color);
            DirectMessage(NativeMethods.SCI_CALLTIPSETFOREHLT, new IntPtr(colour));
        }

        /// <summary>
        /// Sets the specified range of the call tip text to display in a highlighted style.
        /// </summary>
        /// <param name="hlStart">The zero-based index in the call tip text to start highlighting.</param>
        /// <param name="hlEnd">The zero-based index in the call tip text to stop highlighting (exclusive).</param>
        public unsafe void CallTipSetHlt(int hlStart, int hlEnd)
        {
            // To do the char->byte translation we need to use a cached copy of the last call tip
            hlStart = Helpers.Clamp(hlStart, 0, this.lastCallTip.Length);
            hlEnd = Helpers.Clamp(hlEnd, 0, this.lastCallTip.Length);

            fixed (char* cp = this.lastCallTip)
            {
                hlEnd = Encoding.GetByteCount(cp + hlStart, hlEnd - hlStart);  // The bytes between start and end
                hlStart = Encoding.GetByteCount(cp, hlStart);                  // The bytes between 0 and start
                hlEnd += hlStart;                                              // The bytes between 0 and end
            }

            DirectMessage(NativeMethods.SCI_CALLTIPSETHLT, new IntPtr(hlStart), new IntPtr(hlEnd));
        }

        /// <summary>
        /// Determines whether to display a call tip above or below text.
        /// </summary>
        /// <param name="above">true to display above text; otherwise, false. The default is false.</param>
        public void CallTipSetPosition(bool above)
        {
            IntPtr val = above ? new IntPtr(1) : IntPtr.Zero;
            DirectMessage(NativeMethods.SCI_CALLTIPSETPOSITION, val);
        }

        /// <summary>
        /// Displays a call tip window.
        /// </summary>
        /// <param name="posStart">The zero-based document position where the call tip window should be aligned.</param>
        /// <param name="definition">The call tip text.</param>
        /// <remarks>
        /// Call tips can contain multiple lines separated by '\n' characters. Do not include '\r', as this will most likely print as an empty box.
        /// The '\t' character is supported and the size can be set by using <see cref="CallTipTabSize" />.
        /// </remarks>
        public unsafe void CallTipShow(int posStart, string definition)
        {
            posStart = Helpers.Clamp(posStart, 0, TextLength);
            if (definition == null)
                return;

            this.lastCallTip = definition;
            posStart = Lines.CharToBytePosition(posStart);
            byte[] bytes = Helpers.GetBytes(definition, Encoding, zeroTerminated: true);
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_CALLTIPSHOW, new IntPtr(posStart), new IntPtr(bp));
        }

        /// <summary>
        /// Sets the call tip tab size in pixels.
        /// </summary>
        /// <param name="tabSize">The width in pixels of a tab '\t' character in a call tip. Specifying 0 disables special treatment of tabs.</param>
        public void CallTipTabSize(int tabSize)
        {
            // To support the STYLE_CALLTIP style we call SCI_CALLTIPUSESTYLE when the control is created. At
            // this point we're only adjusting the tab size. This breaks a bit with Scintilla convention, but
            // that's okay because the Scintilla convention is lame.

            tabSize = Helpers.ClampMin(tabSize, 0);
            DirectMessage(NativeMethods.SCI_CALLTIPUSESTYLE, new IntPtr(tabSize));
        }

        /// <summary>
        /// Indicates to the current <see cref="LexerName">Lexer</see> that the internal lexer state has changed in the specified
        /// range and therefore may need to be redrawn.
        /// </summary>
        /// <param name="startPos">The zero-based document position at which the lexer state change starts.</param>
        /// <param name="endPos">The zero-based document position at which the lexer state change ends.</param>
        public void ChangeLexerState(int startPos, int endPos)
        {
            int textLength = TextLength;
            startPos = Helpers.Clamp(startPos, 0, textLength);
            endPos = Helpers.Clamp(endPos, 0, textLength);

            startPos = Lines.CharToBytePosition(startPos);
            endPos = Lines.CharToBytePosition(endPos);

            DirectMessage(NativeMethods.SCI_CHANGELEXERSTATE, new IntPtr(startPos), new IntPtr(endPos));
        }

        /// <summary>
        /// Finds the closest character position to the specified display point.
        /// </summary>
        /// <param name="x">The x pixel coordinate within the client rectangle of the control.</param>
        /// <param name="y">The y pixel coordinate within the client rectangle of the control.</param>
        /// <returns>The zero-based document position of the nearest character to the point specified.</returns>
        public int CharPositionFromPoint(int x, int y)
        {
            int pos = DirectMessage(NativeMethods.SCI_CHARPOSITIONFROMPOINT, new IntPtr(x), new IntPtr(y)).ToInt32();
            pos = Lines.ByteToCharPosition(pos);

            return pos;
        }

        /// <summary>
        /// Finds the closest character position to the specified display point or returns -1
        /// if the point is outside the window or not close to any characters.
        /// </summary>
        /// <param name="x">The x pixel coordinate within the client rectangle of the control.</param>
        /// <param name="y">The y pixel coordinate within the client rectangle of the control.</param>
        /// <returns>The zero-based document position of the nearest character to the point specified when near a character; otherwise, -1.</returns>
        public int CharPositionFromPointClose(int x, int y)
        {
            int pos = DirectMessage(NativeMethods.SCI_CHARPOSITIONFROMPOINTCLOSE, new IntPtr(x), new IntPtr(y)).ToInt32();
            if (pos >= 0)
                pos = Lines.ByteToCharPosition(pos);

            return pos;
        }

        /// <summary>
        /// Explicitly sets the current horizontal offset of the caret as the X position to track
        /// when the user moves the caret vertically using the up and down keys.
        /// </summary>
        /// <remarks>
        /// When not set explicitly, Scintilla automatically sets this value each time the user moves
        /// the caret horizontally.
        /// </remarks>
        public void ChooseCaretX()
        {
            DirectMessage(NativeMethods.SCI_CHOOSECARETX);
        }

        /// <summary>
        /// Removes the selected text from the document.
        /// </summary>
        public void Clear()
        {
            DirectMessage(NativeMethods.SCI_CLEAR);
        }

        /// <summary>
        /// Deletes all document text, unless the document is read-only.
        /// </summary>
        public void ClearAll()
        {
            DirectMessage(NativeMethods.SCI_CLEARALL);
        }

        /// <summary>
        /// Makes the specified key definition do nothing.
        /// </summary>
        /// <param name="keyDefinition">The key combination to bind.</param>
        /// <remarks>This is equivalent to binding the keys to <see cref="Command.Null" />.</remarks>
        public void ClearCmdKey(Keys keyDefinition)
        {
            int keys = Helpers.TranslateKeys(keyDefinition);
            DirectMessage(NativeMethods.SCI_CLEARCMDKEY, new IntPtr(keys));
        }

        /// <summary>
        /// Removes all the key definition command mappings.
        /// </summary>
        public void ClearAllCmdKeys()
        {
            DirectMessage(NativeMethods.SCI_CLEARALLCMDKEYS);
        }

        /// <summary>
        /// Removes all styling from the document and resets the folding state.
        /// </summary>
        public void ClearDocumentStyle()
        {
            DirectMessage(NativeMethods.SCI_CLEARDOCUMENTSTYLE);
        }

        /// <summary>
        /// Removes all images registered for autocompletion lists.
        /// </summary>
        public void ClearRegisteredImages()
        {
            DirectMessage(NativeMethods.SCI_CLEARREGISTEREDIMAGES);
        }

        /// <summary>
        /// Sets a single empty selection at the start of the document.
        /// </summary>
        public void ClearSelections()
        {
            DirectMessage(NativeMethods.SCI_CLEARSELECTIONS);
        }

        /// <summary>
        /// Requests that the current lexer restyle the specified range.
        /// </summary>
        /// <param name="startPos">The zero-based document position at which to start styling.</param>
        /// <param name="endPos">The zero-based document position at which to stop styling (exclusive).</param>
        /// <remarks>This will also cause fold levels in the range specified to be reset.</remarks>
        public void Colorize(int startPos, int endPos)
        {
            int textLength = TextLength;
            startPos = Helpers.Clamp(startPos, 0, textLength);
            endPos = Helpers.Clamp(endPos, 0, textLength);

            startPos = Lines.CharToBytePosition(startPos);
            endPos = Lines.CharToBytePosition(endPos);

            DirectMessage(NativeMethods.SCI_COLOURISE, new IntPtr(startPos), new IntPtr(endPos));
        }

        /// <summary>
        /// Changes all end-of-line characters in the document to the format specified.
        /// </summary>
        /// <param name="eolMode">One of the <see cref="Eol" /> enumeration values.</param>
        public void ConvertEols(Eol eolMode)
        {
            int eol = (int)eolMode;
            DirectMessage(NativeMethods.SCI_CONVERTEOLS, new IntPtr(eol));
        }

        /// <summary>
        /// Copies the selected text from the document and places it on the clipboard.
        /// </summary>
        public void Copy()
        {
            DirectMessage(NativeMethods.SCI_COPY);
        }

        /// <summary>
        /// Copies the selected text from the document and places it on the clipboard.
        /// </summary>
        /// <param name="format">One of the <see cref="CopyFormat" /> enumeration values.</param>
        public void Copy(CopyFormat format)
        {
            Helpers.Copy(this, format, true, false, 0, 0);
        }

        /// <summary>
        /// Copies the selected text from the document and places it on the clipboard.
        /// If the selection is empty the current line is copied.
        /// </summary>
        /// <remarks>
        /// If the selection is empty and the current line copied, an extra "MSDEVLineSelect" marker is added to the
        /// clipboard which is then used in <see cref="Paste" /> to paste the whole line before the current line.
        /// </remarks>
        public void CopyAllowLine()
        {
            DirectMessage(NativeMethods.SCI_COPYALLOWLINE);
        }

        /// <summary>
        /// Copies the selected text from the document and places it on the clipboard.
        /// If the selection is empty the current line is copied.
        /// </summary>
        /// <param name="format">One of the <see cref="CopyFormat" /> enumeration values.</param>
        /// <remarks>
        /// If the selection is empty and the current line copied, an extra "MSDEVLineSelect" marker is added to the
        /// clipboard which is then used in <see cref="Paste" /> to paste the whole line before the current line.
        /// </remarks>
        public void CopyAllowLine(CopyFormat format)
        {
            Helpers.Copy(this, format, true, true, 0, 0);
        }

        /// <summary>
        /// Copies the specified range of text to the clipboard.
        /// </summary>
        /// <param name="start">The zero-based character position in the document to start copying.</param>
        /// <param name="end">The zero-based character position (exclusive) in the document to stop copying.</param>
        public void CopyRange(int start, int end)
        {
            int textLength = TextLength;
            start = Helpers.Clamp(start, 0, textLength);
            end = Helpers.Clamp(end, 0, textLength);

            // Convert to byte positions
            start = Lines.CharToBytePosition(start);
            end = Lines.CharToBytePosition(end);

            DirectMessage(NativeMethods.SCI_COPYRANGE, new IntPtr(start), new IntPtr(end));
        }

        /// <summary>
        /// Copies the specified range of text to the clipboard.
        /// </summary>
        /// <param name="start">The zero-based character position in the document to start copying.</param>
        /// <param name="end">The zero-based character position (exclusive) in the document to stop copying.</param>
        /// <param name="format">One of the <see cref="CopyFormat" /> enumeration values.</param>
        public void CopyRange(int start, int end, CopyFormat format)
        {
            int textLength = TextLength;
            start = Helpers.Clamp(start, 0, textLength);
            end = Helpers.Clamp(end, 0, textLength);
            if (start == end)
                return;

            // Convert to byte positions
            start = Lines.CharToBytePosition(start);
            end = Lines.CharToBytePosition(end);

            Helpers.Copy(this, format, false, false, start, end);
        }

        /// <summary>
        /// Create a new, empty document.
        /// </summary>
        /// <returns>A new <see cref="Document" /> with a reference count of 1.</returns>
        /// <remarks>You are responsible for ensuring the reference count eventually reaches 0 or memory leaks will occur.</remarks>
        public Document CreateDocument()
        {
            IntPtr ptr = DirectMessage(NativeMethods.SCI_CREATEDOCUMENT);
            return new Document { Value = ptr };
        }

        /// <summary>
        /// Creates an <see cref="ILoader" /> object capable of loading a <see cref="Document" /> on a background (non-UI) thread.
        /// </summary>
        /// <param name="length">The initial number of characters to allocate.</param>
        /// <returns>A new <see cref="ILoader" /> object, or null if the loader could not be created.</returns>
        public ILoader CreateLoader(int length)
        {
            length = Helpers.ClampMin(length, 0);
            IntPtr ptr = DirectMessage(NativeMethods.SCI_CREATELOADER, new IntPtr(length));
            if (ptr == IntPtr.Zero)
                return null;

            return new Loader(ptr, Encoding);
        }

        /// <summary>
        /// Cuts the selected text from the document and places it on the clipboard.
        /// </summary>
        public void Cut()
        {
            DirectMessage(NativeMethods.SCI_CUT);
        }

        /// <summary>
        /// Deletes a range of text from the document.
        /// </summary>
        /// <param name="position">The zero-based character position to start deleting.</param>
        /// <param name="length">The number of characters to delete.</param>
        public void DeleteRange(int position, int length)
        {
            int textLength = TextLength;
            position = Helpers.Clamp(position, 0, textLength);
            length = Helpers.Clamp(length, 0, textLength - position);

            // Convert to byte position/length
            int byteStartPos = Lines.CharToBytePosition(position);
            int byteEndPos = Lines.CharToBytePosition(position + length);

            DirectMessage(NativeMethods.SCI_DELETERANGE, new IntPtr(byteStartPos), new IntPtr(byteEndPos - byteStartPos));
        }

        /// <summary>
        /// Retrieves a description of keyword sets supported by the current lexer />.
        /// </summary>
        /// <returns>A String describing each keyword set separated by line breaks for the current lexer.</returns>
        public unsafe string DescribeKeywordSets()
        {
            int length = DirectMessage(NativeMethods.SCI_DESCRIBEKEYWORDSETS).ToInt32();
            byte[] bytes = new byte[length + 1];

            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_DESCRIBEKEYWORDSETS, IntPtr.Zero, new IntPtr(bp));

            string str = Encoding.ASCII.GetString(bytes, 0, length);
            return str;
        }

        /// <summary>
        /// Retrieves a brief description of the specified property name for the current <see cref="LexerName">Lexer</see>.
        /// </summary>
        /// <param name="name">A property name supported by the current <see cref="LexerName">Lexer</see>.</param>
        /// <returns>A String describing the lexer property name if found; otherwise, String.Empty.</returns>
        /// <remarks>A list of supported property names for the current <see cref="LexerName">Lexer</see> can be obtained by calling <see cref="PropertyNames" />.</remarks>
        public unsafe string DescribeProperty(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            byte[] nameBytes = Helpers.GetBytes(name, Encoding.ASCII, zeroTerminated: true);
            fixed (byte* nb = nameBytes)
            {
                int length = DirectMessage(NativeMethods.SCI_DESCRIBEPROPERTY, new IntPtr(nb), IntPtr.Zero).ToInt32();
                if (length == 0)
                    return string.Empty;

                byte[] descriptionBytes = new byte[length + 1];
                fixed (byte* db = descriptionBytes)
                {
                    DirectMessage(NativeMethods.SCI_DESCRIBEPROPERTY, new IntPtr(nb), new IntPtr(db));
                    return Helpers.GetString(new IntPtr(db), length, Encoding.ASCII);
                }
            }
        }

        internal IntPtr DirectMessage(int msg)
        {
            return DirectMessage(msg, IntPtr.Zero, IntPtr.Zero);
        }

        internal IntPtr DirectMessage(int msg, IntPtr wParam)
        {
            return DirectMessage(msg, wParam, IntPtr.Zero);
        }

        /// <summary>
        /// Sends the specified message directly to the native Scintilla window,
        /// bypassing any managed APIs.
        /// </summary>
        /// <param name="msg">The message ID.</param>
        /// <param name="wParam">The message <c>wparam</c> field.</param>
        /// <param name="lParam">The message <c>lparam</c> field.</param>
        /// <returns>An <see cref="IntPtr"/> representing the result of the message request.</returns>
        /// <remarks>This API supports the Scintilla infrastructure and is not intended to be used directly from your code.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual IntPtr DirectMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            // If the control handle, ptr, direct function, etc... hasn't been created yet, it will be now.
            IntPtr result = DirectMessage(SciPointer, msg, wParam, lParam);
            return result;
        }

        private static IntPtr DirectMessage(IntPtr sciPtr, int msg, IntPtr wParam, IntPtr lParam)
        {
            // Like Win32 SendMessage but directly to Scintilla
            IntPtr result = directFunction(sciPtr, msg, wParam, lParam);
            return result;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the Control and its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // WM_DESTROY workaround
                if (this.reparent)
                {
                    this.reparent = false;
                    if (IsHandleCreated)
                        DestroyHandle();
                }

                if (this.fillUpChars != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(this.fillUpChars);
                    this.fillUpChars = IntPtr.Zero;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Returns the zero-based document line index from the specified display line index.
        /// </summary>
        /// <param name="displayLine">The zero-based display line index.</param>
        /// <returns>The zero-based document line index.</returns>
        /// <seealso cref="Line.DisplayIndex" />
        public int DocLineFromVisible(int displayLine)
        {
            displayLine = Helpers.Clamp(displayLine, 0, VisibleLineCount);
            return DirectMessage(NativeMethods.SCI_DOCLINEFROMVISIBLE, new IntPtr(displayLine)).ToInt32();
        }

        /// <summary>
        /// If there are multiple selections, removes the specified selection.
        /// </summary>
        /// <param name="selection">The zero-based selection index.</param>
        /// <seealso cref="Selections" />
        public void DropSelection(int selection)
        {
            selection = Helpers.ClampMin(selection, 0);
            DirectMessage(NativeMethods.SCI_DROPSELECTIONN, new IntPtr(selection));
        }

        /// <summary>
        /// Clears any undo or redo history.
        /// </summary>
        /// <remarks>This will also cause <see cref="SetSavePoint" /> to be called but will not raise the <see cref="SavePointReached" /> event.</remarks>
        public void EmptyUndoBuffer()
        {
            DirectMessage(NativeMethods.SCI_EMPTYUNDOBUFFER);
        }

        /// <summary>
        /// Marks the end of a set of actions that should be treated as a single undo action.
        /// </summary>
        /// <seealso cref="BeginUndoAction" />
        public void EndUndoAction()
        {
            DirectMessage(NativeMethods.SCI_ENDUNDOACTION);
        }

        /// <summary>
        /// Performs the specified <see cref="Scintilla" />command.
        /// </summary>
        /// <param name="sciCommand">The command to perform.</param>
        public void ExecuteCmd(Command sciCommand)
        {
            int cmd = (int)sciCommand;
            DirectMessage(cmd);
        }

        /// <summary>
        /// Search text in document without changing the current selection.
        /// The <paramref name="searchFlags"/> argument controls the search type, which includes regular expression searches.
        /// You can search backwards to find the previous occurrence of a search string by setting the end of the search range before the start.
        /// </summary>
        /// <param name="searchFlags">Specifies the how patterns are matched when performing the search.</param>
        /// <param name="text">String to search for.</param>
        /// <param name="start">Beginning of </param>
        /// <param name="end"></param>
        /// <returns>The position of the found text if it succeeds or <c>-1</c> if the search fails.</returns>
        public unsafe int FindText(SearchFlags searchFlags, string text, int start, int end)
        {
            int bytePos = 0;
            byte[] bytes = Helpers.GetBytes(text ?? string.Empty, Encoding, zeroTerminated: true);

            fixed (byte* bp = bytes)
            {
                NativeMethods.Sci_TextToFind textToFind = new NativeMethods.Sci_TextToFind() {
                    chrg = new NativeMethods.Sci_CharacterRange() {
                        cpMin = Lines.CharToBytePosition(Helpers.Clamp(start, 0, TextLength)),
                        cpMax = Lines.CharToBytePosition(Helpers.Clamp(end, 0, TextLength)),
                    },
                    lpstrText = new IntPtr(bp),
                };
                bytePos = DirectMessage(NativeMethods.SCI_FINDTEXT, (IntPtr)searchFlags, new IntPtr(&textToFind)).ToInt32();
            }

            if (bytePos == -1)
                return bytePos;

            return Lines.ByteToCharPosition(bytePos);
        }

        /// <summary>
        /// Performs the specified fold action on the entire document.
        /// </summary>
        /// <param name="action">One of the <see cref="FoldAction" /> enumeration values.</param>
        /// <remarks>When using <see cref="FoldAction.Toggle" /> the first fold header in the document is examined to decide whether to expand or contract.</remarks>
        public void FoldAll(FoldAction action)
        {
            DirectMessage(NativeMethods.SCI_FOLDALL, new IntPtr((int)action));
        }

        /// <summary>
        /// Changes the appearance of fold text tags.
        /// </summary>
        /// <param name="style">One of the <see cref="FoldDisplayText" /> enumeration values.</param>
        /// <remarks>The text tag to display on a folded line can be set using <see cref="Line.ToggleFoldShowText" />.</remarks>
        /// <seealso cref="Line.ToggleFoldShowText" />.
        public void FoldDisplayTextSetStyle(FoldDisplayText style)
        {
            DirectMessage(NativeMethods.SCI_FOLDDISPLAYTEXTSETSTYLE, new IntPtr((int)style));
        }

        /// <summary>
        /// Frees all allocated substyles.
        /// </summary>
        public void FreeSubstyles()
        {
            DirectMessage(NativeMethods.SCI_FREESUBSTYLES);
        }

        /// <summary>
        /// Returns the character as the specified document position.
        /// </summary>
        /// <param name="position">The zero-based document position of the character to get.</param>
        /// <returns>The character at the specified <paramref name="position" />.</returns>
        public unsafe int GetCharAt(int position)
        {
            position = Helpers.Clamp(position, 0, TextLength);
            position = Lines.CharToBytePosition(position);

            int nextPosition = DirectMessage(NativeMethods.SCI_POSITIONRELATIVE, new IntPtr(position), new IntPtr(1)).ToInt32();
            int length = nextPosition - position;
            if (length <= 1)
            {
                // Position is at single-byte character
                return DirectMessage(NativeMethods.SCI_GETCHARAT, new IntPtr(position)).ToInt32();
            }

            // Position is at multibyte character
            byte[] bytes = new byte[length + 1];
            fixed (byte* bp = bytes)
            {
                NativeMethods.Sci_TextRange* range = stackalloc NativeMethods.Sci_TextRange[1];
                range->chrg.cpMin = position;
                range->chrg.cpMax = nextPosition;
                range->lpstrText = new IntPtr(bp);

                DirectMessage(NativeMethods.SCI_GETTEXTRANGE, IntPtr.Zero, new IntPtr(range));
                string str = Helpers.GetString(new IntPtr(bp), length, Encoding);
                return str[0];
            }
        }

        /// <summary>
        /// Returns the column number of the specified document position, taking the width of tabs into account.
        /// </summary>
        /// <param name="position">The zero-based document position to get the column for.</param>
        /// <returns>The number of columns from the start of the line to the specified document <paramref name="position" />.</returns>
        public int GetColumn(int position)
        {
            position = Helpers.Clamp(position, 0, TextLength);
            position = Lines.CharToBytePosition(position);
            return DirectMessage(NativeMethods.SCI_GETCOLUMN, new IntPtr(position)).ToInt32();
        }

        /// <summary>
        /// Returns the last document position likely to be styled correctly.
        /// </summary>
        /// <returns>The zero-based document position of the last styled character.</returns>
        public int GetEndStyled()
        {
            int pos = DirectMessage(NativeMethods.SCI_GETENDSTYLED).ToInt32();
            return Lines.ByteToCharPosition(pos);
        }

        private static readonly string scintillaVersion;
        private static readonly string lexillaVersion;

        /// <summary>
        /// Gets the product version of the Scintilla.dll user by the control.
        /// </summary>
        public string ScintillaVersion => scintillaVersion;

        /// <summary>
        /// Gets the product version of the Lexilla.dll user by the control.
        /// </summary>
        public string LexillaVersion => lexillaVersion;

        /// <summary>
        /// Gets the Primary style associated with the given Secondary style.
        /// </summary>
        /// <param name="style">The secondary style</param>
        /// <returns>For a secondary style, return the primary style, else return the argument.</returns>
        public int GetPrimaryStyleFromStyle(int style)
        {
            return DirectMessage(NativeMethods.SCI_GETPRIMARYSTYLEFROMSTYLE, new IntPtr(style)).ToInt32();
        }

        /// <summary>
        /// Lookup a property value for the current <see cref="LexerName">Lexer</see>.
        /// </summary>
        /// <param name="name">The property name to lookup.</param>
        /// <returns>
        /// A String representing the property value if found; otherwise, String.Empty.
        /// Any embedded property name macros as described in <see cref="SetProperty" /> will not be replaced (expanded).
        /// </returns>
        /// <seealso cref="GetPropertyExpanded" />
        public unsafe string GetProperty(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            byte[] nameBytes = Helpers.GetBytes(name, Encoding.ASCII, zeroTerminated: true);
            fixed (byte* nb = nameBytes)
            {
                int length = DirectMessage(NativeMethods.SCI_GETPROPERTY, new IntPtr(nb)).ToInt32();
                if (length == 0)
                    return string.Empty;

                byte[] valueBytes = new byte[length + 1];
                fixed (byte* vb = valueBytes)
                {
                    DirectMessage(NativeMethods.SCI_GETPROPERTY, new IntPtr(nb), new IntPtr(vb));
                    return Helpers.GetString(new IntPtr(vb), length, Encoding.ASCII);
                }
            }
        }

        /// <summary>
        /// Lookup a property value for the current <see cref="LexerName">Lexer</see> and expand any embedded property macros.
        /// </summary>
        /// <param name="name">The property name to lookup.</param>
        /// <returns>
        /// A String representing the property value if found; otherwise, String.Empty.
        /// Any embedded property name macros as described in <see cref="SetProperty" /> will be replaced (expanded).
        /// </returns>
        /// <seealso cref="GetProperty" />
        public unsafe string GetPropertyExpanded(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            byte[] nameBytes = Helpers.GetBytes(name, Encoding.ASCII, zeroTerminated: true);
            fixed (byte* nb = nameBytes)
            {
                int length = DirectMessage(NativeMethods.SCI_GETPROPERTYEXPANDED, new IntPtr(nb)).ToInt32();
                if (length == 0)
                    return string.Empty;

                byte[] valueBytes = new byte[length + 1];
                fixed (byte* vb = valueBytes)
                {
                    DirectMessage(NativeMethods.SCI_GETPROPERTYEXPANDED, new IntPtr(nb), new IntPtr(vb));
                    return Helpers.GetString(new IntPtr(vb), length, Encoding.ASCII);
                }
            }
        }

        /// <summary>
        /// Lookup a property value for the current <see cref="LexerName">Lexer</see> and convert it to an integer.
        /// </summary>
        /// <param name="name">The property name to lookup.</param>
        /// <param name="defaultValue">A default value to return if the property name is not found or has no value.</param>
        /// <returns>
        /// An Integer representing the property value if found;
        /// otherwise, <paramref name="defaultValue" /> if not found or the property has no value;
        /// otherwise, 0 if the property is not a number.
        /// </returns>
        public unsafe int GetPropertyInt(string name, int defaultValue)
        {
            if (string.IsNullOrEmpty(name))
                return defaultValue;

            byte[] bytes = Helpers.GetBytes(name, Encoding.ASCII, zeroTerminated: true);
            fixed (byte* bp = bytes)
                return DirectMessage(NativeMethods.SCI_GETPROPERTYINT, new IntPtr(bp), new IntPtr(defaultValue)).ToInt32();
        }

        /// <summary>
        /// Gets the style of the specified document position.
        /// </summary>
        /// <param name="position">The zero-based document position of the character to get the style for.</param>
        /// <returns>The zero-based <see cref="Style" /> index used at the specified <paramref name="position" />.</returns>
        public int GetStyleAt(int position)
        {
            position = Helpers.Clamp(position, 0, TextLength);
            position = Lines.CharToBytePosition(position);

            return DirectMessage(NativeMethods.SCI_GETSTYLEAT, new IntPtr(position)).ToInt32();
        }

        /// <summary>
        /// Gets the lexer base style of a substyle.
        /// </summary>
        /// <param name="subStyle">The integer index of the substyle</param>
        /// <returns>Returns the base style, else returns the argument.</returns>
        public int GetStyleFromSubstyle(int subStyle)
        {
            return DirectMessage(NativeMethods.SCI_GETSTYLEFROMSUBSTYLE, new IntPtr(subStyle)).ToInt32();
        }

        /// <summary>
        /// Gets the length of the number of substyles allocated for a given lexer base style.
        /// </summary>
        /// <param name="styleBase">The lexer style integer</param>
        /// <returns>Returns the length of the substyles allocated for a base style.</returns>
        public int GetSubstylesLength(int styleBase)
        {
            return DirectMessage(NativeMethods.SCI_GETSUBSTYLESLENGTH, new IntPtr(styleBase)).ToInt32();
        }

        /// <summary>
        /// Gets the start index of the substyles for a given lexer base style.
        /// </summary>
        /// <param name="styleBase">The lexer style integer</param>
        /// <returns>Returns the start of the substyles allocated for a base style.</returns>
        public int GetSubstylesStart(int styleBase)
        {
            return DirectMessage(NativeMethods.SCI_GETSUBSTYLESSTART, new IntPtr(styleBase)).ToInt32();
        }

        /// <summary>
        /// Returns the capture group text of the most recent regular expression search.
        /// </summary>
        /// <param name="tagNumber">The capture group (1 through 9) to get the text for.</param>
        /// <returns>A String containing the capture group text if it participated in the match; otherwise, an empty string.</returns>
        /// <seealso cref="SearchInTarget" />
        public unsafe string GetTag(int tagNumber)
        {
            tagNumber = Helpers.Clamp(tagNumber, 1, 9);
            int length = DirectMessage(NativeMethods.SCI_GETTAG, new IntPtr(tagNumber), IntPtr.Zero).ToInt32();
            if (length <= 0)
                return string.Empty;

            byte[] bytes = new byte[length + 1];
            fixed (byte* bp = bytes)
            {
                DirectMessage(NativeMethods.SCI_GETTAG, new IntPtr(tagNumber), new IntPtr(bp));
                return Helpers.GetString(new IntPtr(bp), length, Encoding);
            }
        }

        /// <summary>
        /// Gets a range of text from the document accounting for wide characters.
        /// </summary>
        /// <param name="position">The zero-based starting character position of the range to get.</param>
        /// <param name="length">The number of characters (including wide) to get.</param>
        /// <returns>A string representing the text range.</returns>
        public unsafe string GetWideTextRange(int position, int length)
        {
            int textLength = TextLength;
            position = Helpers.Clamp(position, 0, textLength);
            length = Helpers.Clamp(length, 0, textLength - position);

            // Convert to byte position/length
            int byteStartPos = Lines.CharToWideBytePosition(position);
            int byteEndPos = Lines.CharToWideBytePosition(position + length);

            IntPtr ptr = DirectMessage(NativeMethods.SCI_GETRANGEPOINTER, new IntPtr(byteStartPos), new IntPtr(byteEndPos - byteStartPos));
            if (ptr == IntPtr.Zero)
                return string.Empty;

            return Helpers.GetString(ptr, byteEndPos - byteStartPos, Encoding);
        }

        /// <summary>
        /// Map Lexer enum value to supported lexer ID
        /// </summary>
        /// <param name="lexer">Supported Lexer enum value</param>
        /// <returns>LexerName string for corresponding lexer value</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string GetLexerIDFromLexer(Lexer lexer)
        {
            return lexer switch
            {
                Lexer.SCLEX_A68K => "a68k",
                Lexer.SCLEX_ADPL => "apdl",
                Lexer.SCLEX_ASYMPTOTE => "asy",
                Lexer.SCLEX_AU3 => "au3",
                Lexer.SCLEX_AVE => "ave",
                Lexer.SCLEX_AVS => "avs",
                Lexer.SCLEX_ABAQUS => "abaqus",
                Lexer.SCLEX_ADA => "ada",
                Lexer.SCLEX_ASCIIDOC => "asciidoc",
                Lexer.SCLEX_ASM => "asm",
                Lexer.SCLEX_AS => "as",
                Lexer.SCLEX_ASN1 => "asn1",
                Lexer.SCLEX_BAAN => "baan",
                Lexer.SCLEX_BASH => "bash",
                Lexer.SCLEX_BLITZBASIC => "blitzbasic",
                Lexer.SCLEX_PUREBASIC => "purebasic",
                Lexer.SCLEX_FREEBASIC => "freebasic",
                Lexer.SCLEX_BATCH => "batch",
                Lexer.SCLEX_BIBTEX => "bibtex",
                Lexer.SCLEX_BULLANT => "bullant",
                Lexer.SCLEX_CIL => "cil",
                Lexer.SCLEX_CLW => "clarion",
                Lexer.SCLEX_CLWNOCASE => "clarionnocase",
                Lexer.SCLEX_COBOL => "COBOL",
                Lexer.SCLEX_CPP => "cpp",
                Lexer.SCLEX_CPPNOCASE => "cppnocase",
                Lexer.SCLEX_CSHARP => "cpp",
                Lexer.SCLEX_JAVA => "cpp",
                Lexer.SCLEX_JAVASCRIPT => "cpp",
                Lexer.SCLEX_CSS => "css",
                Lexer.SCLEX_CAML => "caml",
                Lexer.SCLEX_CMAKE => "cmake",
                Lexer.SCLEX_COFFEESCRIPT => "coffeescript",
                Lexer.SCLEX_CONF => "conf",
                Lexer.SCLEX_NNCRONTAB => "nncrontab",
                Lexer.SCLEX_CSOUND => "csound",
                Lexer.SCLEX_D => "d",
                Lexer.SCLEX_DMAP => "DMAP",
                Lexer.SCLEX_DMIS => "DMIS",
                Lexer.SCLEX_DATAFLEX => "dataflex",
                Lexer.SCLEX_DIFF => "diff",
                Lexer.SCLEX_ECL => "ecl",
                Lexer.SCLEX_EDIFACT => "edifact",
                Lexer.SCLEX_ESCRIPT => "escript",
                Lexer.SCLEX_EIFFEL => "eiffel",
                Lexer.SCLEX_EIFFELKW => "eiffelkw",
                Lexer.SCLEX_ERLANG => "erlang",
                Lexer.SCLEX_ERRORLIST => "errorlist",
                Lexer.SCLEX_FSHARP => "fsharp",
                Lexer.SCLEX_FLAGSHIP => "flagship",
                Lexer.SCLEX_FORTH => "forth",
                Lexer.SCLEX_FORTRAN => "fortran",
                Lexer.SCLEX_F77 => "f77",
                Lexer.SCLEX_GAP => "gap",
                Lexer.SCLEX_GDSCRIPT => "gdscript",
                Lexer.SCLEX_GUI4CLI => "gui4cli",
                Lexer.SCLEX_HTML => "hypertext",
                Lexer.SCLEX_XML => "xml",
                Lexer.SCLEX_PHPSCRIPT => "phpscript",
                Lexer.SCLEX_HASKELL => "haskell",
                Lexer.SCLEX_LITERATEHASKELL => "literatehaskell",
                Lexer.SCLEX_SREC => "srec",
                Lexer.SCLEX_IHEX => "ihex",
                Lexer.SCLEX_TEHEX => "tehex",
                Lexer.SCLEX_HOLLYWOOD => "hollywood",
                Lexer.SCLEX_INDENT => "indent",
                Lexer.SCLEX_INNOSETUP => "inno",
                Lexer.SCLEX_JSON => "json",
                Lexer.SCLEX_JULIA => "julia",
                Lexer.SCLEX_KIX => "kix",
                Lexer.SCLEX_KVIRC => "kvirc",
                Lexer.SCLEX_LATEX => "latex",
                Lexer.SCLEX_LISP => "lisp",
                Lexer.SCLEX_LOUT => "lout",
                Lexer.SCLEX_LUA => "lua",
                Lexer.SCLEX_MMIXAL => "mmixal",
                Lexer.SCLEX_LOT => "lot",
                Lexer.SCLEX_MSSQL => "mssql",
                Lexer.SCLEX_MAGIK => "magiksf",
                Lexer.SCLEX_MAKEFILE => "makefile",
                Lexer.SCLEX_MARKDOWN => "markdown",
                Lexer.SCLEX_MATLAB => "matlab",
                Lexer.SCLEX_OCTAVE => "octave",
                Lexer.SCLEX_MAXIMA => "maxima",
                Lexer.SCLEX_METAPOST => "metapost",
                Lexer.SCLEX_MODULA => "modula",
                Lexer.SCLEX_MYSQL => "mysql",
                Lexer.SCLEX_NIM => "nim",
                Lexer.SCLEX_NIMROD => "nimrod",
                Lexer.SCLEX_NSIS => "nsis",
                Lexer.SCLEX_NULL => "null",
                Lexer.SCLEX_OSCRIPT => "oscript",
                Lexer.SCLEX_OPAL => "opal",
                Lexer.SCLEX_POWERBASIC => "powerbasic",
                Lexer.SCLEX_PLM => "PL/M",
                Lexer.SCLEX_PO => "po",
                Lexer.SCLEX_POV => "pov",
                Lexer.SCLEX_POSTSCRIPT => "ps",
                Lexer.SCLEX_PASCAL => "pascal",
                Lexer.SCLEX_PERL => "perl",
                Lexer.SCLEX_POWERPRO => "powerpro",
                Lexer.SCLEX_POWERSHELL => "powershell",
                Lexer.SCLEX_PROGRESS => "abl",
                Lexer.SCLEX_PROPERTIES => "props",
                Lexer.SCLEX_PYTHON => "python",
                Lexer.SCLEX_R => "r",
                Lexer.SCLEX_S => "r",
                Lexer.SCLEX_SPLUS => "r",
                Lexer.SCLEX_RAKU => "raku",
                Lexer.SCLEX_REBOL => "rebol",
                Lexer.SCLEX_REGISTRY => "registry",
                Lexer.SCLEX_RUBY => "ruby",
                Lexer.SCLEX_RUST => "rust",
                Lexer.SCLEX_SAS => "sas",
                Lexer.SCLEX_SML => "SML",
                Lexer.SCLEX_SQL => "sql",
                Lexer.SCLEX_STTXT => "fcST",
                Lexer.SCLEX_SCRIPTOL => "scriptol",
                Lexer.SCLEX_SMALLTALK => "smalltalk",
                Lexer.SCLEX_SORCUS => "sorcins",
                Lexer.SCLEX_SPECMAN => "specman",
                Lexer.SCLEX_SPICE => "spice",
                Lexer.SCLEX_STATA => "stata",
                Lexer.SCLEX_TACL => "TACL",
                Lexer.SCLEX_TADS3 => "tads3",
                Lexer.SCLEX_TAL => "TAL",
                Lexer.SCLEX_TCL => "tcl",
                Lexer.SCLEX_TCMD => "tcmd",
                Lexer.SCLEX_TEX => "tex",
                Lexer.SCLEX_TXT2TAGS => "txt2tags",
                Lexer.SCLEX_VB => "vb",
                Lexer.SCLEX_VBSCRIPT => "vbscript",
                Lexer.SCLEX_VHDL => "vhdl",
                Lexer.SCLEX_VERILOG => "verilog",
                Lexer.SCLEX_VISUALPROLOG => "visualprolog",
                Lexer.SCLEX_X12 => "x12",
                Lexer.SCLEX_YAML => "yaml",
                _ => throw new ArgumentOutOfRangeException(nameof(lexer), lexer, null),
            };
        }

        /// <summary>
        /// Gets a range of text from the document.
        /// </summary>
        /// <param name="position">The zero-based starting character position of the range to get.</param>
        /// <param name="length">The number of characters to get.</param>
        /// <returns>A string representing the text range.</returns>
        public unsafe string GetTextRange(int position, int length)
        {
            int textLength = TextLength;
            position = Helpers.Clamp(position, 0, textLength);
            length = Helpers.Clamp(length, 0, textLength - position);

            // Convert to byte position/length
            int byteStartPos = Lines.CharToBytePosition(position);
            int byteEndPos = Lines.CharToBytePosition(position + length);

            IntPtr ptr = DirectMessage(NativeMethods.SCI_GETRANGEPOINTER, new IntPtr(byteStartPos), new IntPtr(byteEndPos - byteStartPos));
            if (ptr == IntPtr.Zero)
                return string.Empty;

            return Helpers.GetString(ptr, byteEndPos - byteStartPos, Encoding);
        }

        /// <summary>
        /// Gets a range of text from the document formatted as Hypertext Markup Language (HTML).
        /// </summary>
        /// <param name="position">The zero-based starting character position of the range to get.</param>
        /// <param name="length">The number of characters to get.</param>
        /// <returns>A string representing the text range formatted as HTML.</returns>
        public string GetTextRangeAsHtml(int position, int length)
        {
            int textLength = TextLength;
            position = Helpers.Clamp(position, 0, textLength);
            length = Helpers.Clamp(length, 0, textLength - position);

            int startBytePos = Lines.CharToBytePosition(position);
            int endBytePos = Lines.CharToBytePosition(position + length);

            return Helpers.GetHtml(this, startBytePos, endBytePos);
        }

        /// <summary>
        /// Returns the version information of the native Scintilla library.
        /// </summary>
        /// <returns>An object representing the version information of the native Scintilla library.</returns>
        public FileVersionInfo GetVersionInfo()
        {
            var version = FileVersionInfo.GetVersionInfo(modulePathScintilla);

            return version;
        }

        ///<summary>
        /// Gets the word from the position specified.
        /// </summary>
        /// <param name="position">The zero-based document character position to get the word from.</param>
        /// <returns>The word at the specified position.</returns>
        public string GetWordFromPosition(int position)
        {
            int startPosition = WordStartPosition(position, true);
            int endPosition = WordEndPosition(position, true);
            return GetTextRange(startPosition, endPosition - startPosition);
        }

        /// <summary>
        /// Navigates the caret to the document position specified.
        /// </summary>
        /// <param name="position">The zero-based document character position to navigate to.</param>
        /// <remarks>Any selection is discarded.</remarks>
        public void GotoPosition(int position)
        {
            position = Helpers.Clamp(position, 0, TextLength);
            position = Lines.CharToBytePosition(position);
            DirectMessage(NativeMethods.SCI_GOTOPOS, new IntPtr(position));
        }

        /// <summary>
        /// Hides the range of lines specified.
        /// </summary>
        /// <param name="lineStart">The zero-based index of the line range to start hiding.</param>
        /// <param name="lineEnd">The zero-based index of the line range to end hiding.</param>
        /// <seealso cref="ShowLines" />
        /// <seealso cref="Line.Visible" />
        public void HideLines(int lineStart, int lineEnd)
        {
            lineStart = Helpers.Clamp(lineStart, 0, Lines.Count);
            lineEnd = Helpers.Clamp(lineEnd, lineStart, Lines.Count);

            DirectMessage(NativeMethods.SCI_HIDELINES, new IntPtr(lineStart), new IntPtr(lineEnd));
        }

        /// <summary>
        /// Returns a bitmap representing the 32 indicators in use at the specified position.
        /// </summary>
        /// <param name="position">The zero-based character position within the document to test.</param>
        /// <returns>A bitmap indicating which of the 32 indicators are in use at the specified <paramref name="position" />.</returns>
        public uint IndicatorAllOnFor(int position)
        {
            position = Helpers.Clamp(position, 0, TextLength);
            position = Lines.CharToBytePosition(position);

            int bitmap = DirectMessage(NativeMethods.SCI_INDICATORALLONFOR, new IntPtr(position)).ToInt32();
            return unchecked((uint)bitmap);
        }

        /// <summary>
        /// Removes the <see cref="IndicatorCurrent" /> indicator (and user-defined value) from the specified range of text.
        /// </summary>
        /// <param name="position">The zero-based character position within the document to start clearing.</param>
        /// <param name="length">The number of characters to clear.</param>
        public void IndicatorClearRange(int position, int length)
        {
            int textLength = TextLength;
            position = Helpers.Clamp(position, 0, textLength);
            length = Helpers.Clamp(length, 0, textLength - position);

            int startPos = Lines.CharToBytePosition(position);
            int endPos = Lines.CharToBytePosition(position + length);

            DirectMessage(NativeMethods.SCI_INDICATORCLEARRANGE, new IntPtr(startPos), new IntPtr(endPos - startPos));
        }

        /// <summary>
        /// Adds the <see cref="IndicatorCurrent" /> indicator and <see cref="IndicatorValue" /> value to the specified range of text.
        /// </summary>
        /// <param name="position">The zero-based character position within the document to start filling.</param>
        /// <param name="length">The number of characters to fill.</param>
        public void IndicatorFillRange(int position, int length)
        {
            int textLength = TextLength;
            position = Helpers.Clamp(position, 0, textLength);
            length = Helpers.Clamp(length, 0, textLength - position);

            int startPos = Lines.CharToBytePosition(position);
            int endPos = Lines.CharToBytePosition(position + length);

            DirectMessage(NativeMethods.SCI_INDICATORFILLRANGE, new IntPtr(startPos), new IntPtr(endPos - startPos));
        }

        private void InitDocument(Eol eolMode = Eol.CrLf, bool useTabs = false, int tabWidth = 4, int indentWidth = 0)
        {
            // Document.h
            // These properties are stored in the Scintilla document, not the control; meaning, when
            // a user changes documents these properties will change. If the user changes to a new
            // document, these properties will reset to defaults. That can cause confusion for our users
            // who would expect their tab settings, for example, to be unchanged based on which document
            // they have selected into the control. This is where we carry forward any of the user's
            // current settings -- and our default overrides -- to a new document.

            DirectMessage(NativeMethods.SCI_SETCODEPAGE, new IntPtr(NativeMethods.SC_CP_UTF8));
            DirectMessage(NativeMethods.SCI_SETUNDOCOLLECTION, new IntPtr(1));
            DirectMessage(NativeMethods.SCI_SETEOLMODE, new IntPtr((int)eolMode));
            DirectMessage(NativeMethods.SCI_SETUSETABS, useTabs ? new IntPtr(1) : IntPtr.Zero);
            DirectMessage(NativeMethods.SCI_SETTABWIDTH, new IntPtr(tabWidth));
            DirectMessage(NativeMethods.SCI_SETINDENT, new IntPtr(indentWidth));
        }

        /// <summary>
        /// Default Attribute values do not always get applied to the control.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.defaultvalueattribute
        /// "A DefaultValueAttribute will not cause a member to be automatically initialized with the attribute's value. You must set the initial value in your code."
        /// This function is created to be called in the OnHandleCreated event so that we can force the default values to be applied.
        /// </summary>
        private void InitControlProps()
        {
            // I would like to see all of my text please
            ScrollWidth = 1;
            ScrollWidthTracking = true;

            // Reset the valid "word chars" to work around a bug? in Scintilla which includes those below plus non-printable (beyond ASCII 127) characters
            WordChars = "abcdefghijklmnopqrstuvwxyz_ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Hide all default margins
            foreach (Margin margin in Margins)
            {
                margin.Width = 0;
            }
        }

        /// <summary>
        /// Inserts text at the specified position.
        /// </summary>
        /// <param name="position">The zero-based character position to insert the text. Specify -1 to use the current caret position.</param>
        /// <param name="text">The text to insert into the document.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="position" /> less than zero and not equal to -1. -or-
        /// <paramref name="position" /> is greater than the document length.
        /// </exception>
        /// <remarks>No scrolling is performed.</remarks>
        public unsafe void InsertText(int position, string text)
        {
            if (position < -1)
                throw new ArgumentOutOfRangeException("position", "Position must be greater or equal to zero, or -1.");

            if (position != -1)
            {
                int textLength = TextLength;
                if (position > textLength)
                    throw new ArgumentOutOfRangeException("position", "Position cannot exceed document length.");

                position = Lines.CharToBytePosition(position);
            }

            fixed (byte* bp = Helpers.GetBytes(text ?? string.Empty, Encoding, zeroTerminated: true))
                DirectMessage(NativeMethods.SCI_INSERTTEXT, new IntPtr(position), new IntPtr(bp));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="start" /> and <paramref name="end" /> positions are
        /// at the beginning and end of a word, respectively.
        /// </summary>
        /// <param name="start">The zero-based document position of the possible word start.</param>
        /// <param name="end">The zero-based document position of the possible word end.</param>
        /// <returns>
        /// true if <paramref name="start" /> and <paramref name="end" /> are at the beginning and end of a word, respectively;
        /// otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method does not check whether there is whitespace in the search range,
        /// only that the <paramref name="start" /> and <paramref name="end" /> are at word boundaries.
        /// </remarks>
        public bool IsRangeWord(int start, int end)
        {
            int textLength = TextLength;
            start = Helpers.Clamp(start, 0, textLength);
            end = Helpers.Clamp(end, 0, textLength);

            start = Lines.CharToBytePosition(start);
            end = Lines.CharToBytePosition(end);

            return DirectMessage(NativeMethods.SCI_ISRANGEWORD, new IntPtr(start), new IntPtr(end)) != IntPtr.Zero;
        }

        /// <summary>
        /// Returns the line that contains the document position specified.
        /// </summary>
        /// <param name="position">The zero-based document character position.</param>
        /// <returns>The zero-based document line index containing the character <paramref name="position" />.</returns>
        public int LineFromPosition(int position)
        {
            position = Helpers.Clamp(position, 0, TextLength);
            return Lines.LineFromCharPosition(position);
        }

        /// <summary>
        /// Scrolls the display the number of lines and columns specified.
        /// </summary>
        /// <param name="lines">The number of lines to scroll.</param>
        /// <param name="columns">The number of columns to scroll.</param>
        /// <remarks>
        /// Negative values scroll in the opposite direction.
        /// A column is the width in pixels of a space character in the <see cref="Style.Default" /> style.
        /// </remarks>
        public void LineScroll(int lines, int columns)
        {
            DirectMessage(NativeMethods.SCI_LINESCROLL, new IntPtr(columns), new IntPtr(lines));
        }

        /// <summary>
        /// Loads a <see cref="Scintilla" /> compatible lexer from an external DLL.
        /// </summary>
        /// <param name="path">The path to the external lexer DLL.</param>
        public unsafe void LoadLexerLibrary(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            byte[] bytes = Helpers.GetBytes(path, Encoding.Default, zeroTerminated: true);
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_LOADLEXERLIBRARY, IntPtr.Zero, new IntPtr(bp));
        }

        /// <summary>
        /// Removes the specified marker from all lines.
        /// </summary>
        /// <param name="marker">The zero-based <see cref="Marker" /> index to remove from all lines, or -1 to remove all markers from all lines.</param>
        public void MarkerDeleteAll(int marker)
        {
            marker = Helpers.Clamp(marker, -1, Markers.Count - 1);
            DirectMessage(NativeMethods.SCI_MARKERDELETEALL, new IntPtr(marker));
        }

        /// <summary>
        /// Searches the document for the marker handle and deletes the marker if found.
        /// </summary>
        /// <param name="markerHandle">The <see cref="MarkerHandle" /> created by a previous call to <see cref="Line.MarkerAdd" /> of the marker to delete.</param>
        public void MarkerDeleteHandle(MarkerHandle markerHandle)
        {
            DirectMessage(NativeMethods.SCI_MARKERDELETEHANDLE, markerHandle.Value);
        }

        /// <summary>
        /// Enable or disable highlighting of the current folding block.
        /// </summary>
        /// <param name="enabled">true to highlight the current folding block; otherwise, false.</param>
        public void MarkerEnableHighlight(bool enabled)
        {
            IntPtr val = enabled ? new IntPtr(1) : IntPtr.Zero;
            DirectMessage(NativeMethods.SCI_MARKERENABLEHIGHLIGHT, val);
        }

        /// <summary>
        /// Searches the document for the marker handle and returns the line number containing the marker if found.
        /// </summary>
        /// <param name="markerHandle">The <see cref="MarkerHandle" /> created by a previous call to <see cref="Line.MarkerAdd" /> of the marker to search for.</param>
        /// <returns>If found, the zero-based line index containing the marker; otherwise, -1.</returns>
        public int MarkerLineFromHandle(MarkerHandle markerHandle)
        {
            return DirectMessage(NativeMethods.SCI_MARKERLINEFROMHANDLE, markerHandle.Value).ToInt32();
        }

        /// <summary>
        /// Specifies the long line indicator column number and color when <see cref="EdgeMode" /> is <see cref="EdgeMode.MultiLine" />.
        /// </summary>
        /// <param name="column">The zero-based column number to indicate.</param>
        /// <param name="edgeColor">The color of the vertical long line indicator.</param>
        /// <remarks>A column is defined as the width of a space character in the <see cref="Style.Default" /> style.</remarks>
        /// <seealso cref="MultiEdgeClearAll" />
        public void MultiEdgeAddLine(int column, Color edgeColor)
        {
            column = Helpers.ClampMin(column, 0);
            int colour = HelperMethods.ToWin32ColorOpaque(edgeColor);

            DirectMessage(NativeMethods.SCI_MULTIEDGEADDLINE, new IntPtr(column), new IntPtr(colour));
        }

        /// <summary>
        /// Removes all the long line column indicators specified using <seealso cref="MultiEdgeAddLine" />.
        /// </summary>
        /// <seealso cref="MultiEdgeAddLine" />
        public void MultiEdgeClearAll()
        {
            DirectMessage(NativeMethods.SCI_MULTIEDGECLEARALL);
        }

        /// <summary>
        /// Searches for all instances of the main selection within the <see cref="TargetStart" /> and <see cref="TargetEnd" />
        /// range and adds any matches to the selection.
        /// </summary>
        /// <remarks>
        /// The <see cref="SearchFlags" /> property is respected when searching, allowing additional
        /// selections to match on different case sensitivity and word search options.
        /// </remarks>
        /// <seealso cref="MultipleSelectAddNext" />
        public void MultipleSelectAddEach()
        {
            DirectMessage(NativeMethods.SCI_MULTIPLESELECTADDEACH);
        }

        /// <summary>
        /// Searches for the next instance of the main selection within the <see cref="TargetStart" /> and <see cref="TargetEnd" />
        /// range and adds any match to the selection.
        /// </summary>
        /// <remarks>
        /// The <see cref="SearchFlags" /> property is respected when searching, allowing additional
        /// selections to match on different case sensitivity and word search options.
        /// </remarks>
        /// <seealso cref="MultipleSelectAddNext" />
        public void MultipleSelectAddNext()
        {
            DirectMessage(NativeMethods.SCI_MULTIPLESELECTADDNEXT);
        }

        /// <summary>
        /// Raises the <see cref="AutoCCancelled" /> event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnAutoCCancelled(EventArgs e)
        {
            if (Events[autoCCancelledEventKey] is EventHandler<EventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AutoCCharDeleted" /> event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnAutoCCharDeleted(EventArgs e)
        {
            if (Events[autoCCharDeletedEventKey] is EventHandler<EventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AutoCCompleted" /> event.
        /// </summary>
        /// <param name="e">An <see cref="AutoCSelectionEventArgs" /> that contains the event data.</param>
        protected virtual void OnAutoCCompleted(AutoCSelectionEventArgs e)
        {
            if (Events[autoCCompletedEventKey] is EventHandler<AutoCSelectionEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AutoCSelection" /> event.
        /// </summary>
        /// <param name="e">An <see cref="AutoCSelectionEventArgs" /> that contains the event data.</param>
        protected virtual void OnAutoCSelection(AutoCSelectionEventArgs e)
        {
            if (Events[autoCSelectionEventKey] is EventHandler<AutoCSelectionEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AutoCSelectionChange" /> event.
        /// </summary>
        /// <param name="e">An <see cref="AutoCSelectionChangeEventArgs" /> that contains the event data.</param>
        protected virtual void OnAutoCSelectionChange(AutoCSelectionChangeEventArgs e)
        {
            if (Events[autoCSelectionChangeEventKey] is EventHandler<AutoCSelectionChangeEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="BeforeDelete" /> event.
        /// </summary>
        /// <param name="e">A <see cref="BeforeModificationEventArgs" /> that contains the event data.</param>
        protected virtual void OnBeforeDelete(BeforeModificationEventArgs e)
        {
            if (Events[beforeDeleteEventKey] is EventHandler<BeforeModificationEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="BeforeInsert" /> event.
        /// </summary>
        /// <param name="e">A <see cref="BeforeModificationEventArgs" /> that contains the event data.</param>
        protected virtual void OnBeforeInsert(BeforeModificationEventArgs e)
        {
            if (Events[beforeInsertEventKey] is EventHandler<BeforeModificationEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="BorderStyleChanged" /> event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnBorderStyleChanged(EventArgs e)
        {
            if (Events[borderStyleChangedEventKey] is EventHandler handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ChangeAnnotation" /> event.
        /// </summary>
        /// <param name="e">A <see cref="ChangeAnnotationEventArgs" /> that contains the event data.</param>
        protected virtual void OnChangeAnnotation(ChangeAnnotationEventArgs e)
        {
            if (Events[changeAnnotationEventKey] is EventHandler<ChangeAnnotationEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CharAdded" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CharAddedEventArgs" /> that contains the event data.</param>
        protected virtual void OnCharAdded(CharAddedEventArgs e)
        {
            if (Events[charAddedEventKey] is EventHandler<CharAddedEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Delete" /> event.
        /// </summary>
        /// <param name="e">A <see cref="ModificationEventArgs" /> that contains the event data.</param>
        protected virtual void OnDelete(ModificationEventArgs e)
        {
            if (Events[deleteEventKey] is EventHandler<ModificationEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DoubleClick" /> event.
        /// </summary>
        /// <param name="e">A <see cref="DoubleClickEventArgs" /> that contains the event data.</param>
        protected virtual void OnDoubleClick(DoubleClickEventArgs e)
        {
            if (Events[doubleClickEventKey] is EventHandler<DoubleClickEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DwellEnd" /> event.
        /// </summary>
        /// <param name="e">A <see cref="DwellEventArgs" /> that contains the event data.</param>
        protected virtual void OnDwellEnd(DwellEventArgs e)
        {
            if (Events[dwellEndEventKey] is EventHandler<DwellEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DwellStart" /> event.
        /// </summary>
        /// <param name="e">A <see cref="DwellEventArgs" /> that contains the event data.</param>
        protected virtual void OnDwellStart(DwellEventArgs e)
        {
            if (Events[dwellStartEventKey] is EventHandler<DwellEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CallTipClick" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CallTipClickEventArgs" /> that contains the event data.</param>
        protected virtual void OnCallTipClick(CallTipClickEventArgs e)
        {
            if (Events[callTipClickEventKey] is EventHandler<CallTipClickEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the HandleCreated event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override unsafe void OnHandleCreated(EventArgs e)
        {
            // Set more intelligent defaults...
            InitDocument();
            InitControlProps();

            // Enable support for the call tip style and tabs
            DirectMessage(NativeMethods.SCI_CALLTIPUSESTYLE, new IntPtr(16));

            // Native Scintilla uses the WM_CREATE message to register itself as an
            // IDropTarget... beating Windows Forms to the punch. There are many possible
            // ways to solve this, but my favorite is to revoke drag and drop from the
            // native Scintilla control before base.OnHandleCreated does the standard
            // processing of AllowDrop.
            if (!_ScintillaManagedDragDrop)
                NativeMethods.RevokeDragDrop(Handle);

            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Raises the <see cref="HotspotClick" /> event.
        /// </summary>
        /// <param name="e">A <see cref="HotspotClickEventArgs" /> that contains the event data.</param>
        protected virtual void OnHotspotClick(HotspotClickEventArgs e)
        {
            if (Events[hotspotClickEventKey] is EventHandler<HotspotClickEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="HotspotDoubleClick" /> event.
        /// </summary>
        /// <param name="e">A <see cref="HotspotClickEventArgs" /> that contains the event data.</param>
        protected virtual void OnHotspotDoubleClick(HotspotClickEventArgs e)
        {
            if (Events[hotspotDoubleClickEventKey] is EventHandler<HotspotClickEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="HotspotReleaseClick" /> event.
        /// </summary>
        /// <param name="e">A <see cref="HotspotClickEventArgs" /> that contains the event data.</param>
        protected virtual void OnHotspotReleaseClick(HotspotClickEventArgs e)
        {
            if (Events[hotspotReleaseClickEventKey] is EventHandler<HotspotClickEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="IndicatorClick" /> event.
        /// </summary>
        /// <param name="e">An <see cref="IndicatorClickEventArgs" /> that contains the event data.</param>
        protected virtual void OnIndicatorClick(IndicatorClickEventArgs e)
        {
            if (Events[indicatorClickEventKey] is EventHandler<IndicatorClickEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="IndicatorRelease" /> event.
        /// </summary>
        /// <param name="e">An <see cref="IndicatorReleaseEventArgs" /> that contains the event data.</param>
        protected virtual void OnIndicatorRelease(IndicatorReleaseEventArgs e)
        {
            if (Events[indicatorReleaseEventKey] is EventHandler<IndicatorReleaseEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Insert" /> event.
        /// </summary>
        /// <param name="e">A <see cref="ModificationEventArgs" /> that contains the event data.</param>
        protected virtual void OnInsert(ModificationEventArgs e)
        {
            if (Events[insertEventKey] is EventHandler<ModificationEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="InsertCheck" /> event.
        /// </summary>
        /// <param name="e">An <see cref="InsertCheckEventArgs" /> that contains the event data.</param>
        protected virtual void OnInsertCheck(InsertCheckEventArgs e)
        {
            if (Events[insertCheckEventKey] is EventHandler<InsertCheckEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MarginClick" /> event.
        /// </summary>
        /// <param name="e">A <see cref="MarginClickEventArgs" /> that contains the event data.</param>
        protected virtual void OnMarginClick(MarginClickEventArgs e)
        {
            if (Events[marginClickEventKey] is EventHandler<MarginClickEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MarginRightClick" /> event.
        /// </summary>
        /// <param name="e">A <see cref="MarginClickEventArgs" /> that contains the event data.</param>
        protected virtual void OnMarginRightClick(MarginClickEventArgs e)
        {
            if (Events[marginRightClickEventKey] is EventHandler<MarginClickEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ModifyAttempt" /> event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnModifyAttempt(EventArgs e)
        {
            if (Events[modifyAttemptEventKey] is EventHandler<EventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the MouseUp event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Borrowed this from TextBoxBase.OnMouseUp
            if (!this.doubleClick)
            {
                OnClick(e);
                OnMouseClick(e);
            }
            else
            {
                var doubleE = new MouseEventArgs(e.Button, 2, e.X, e.Y, e.Delta);
                OnDoubleClick(doubleE);
                OnMouseDoubleClick(doubleE);
                this.doubleClick = false;
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Raises the <see cref="NeedShown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="NeedShownEventArgs" /> that contains the event data.</param>
        protected virtual void OnNeedShown(NeedShownEventArgs e)
        {
            if (Events[needShownEventKey] is EventHandler<NeedShownEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Painted" /> event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnPainted(EventArgs e)
        {
            if (Events[paintedEventKey] is EventHandler<EventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="SavePointLeft" /> event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnSavePointLeft(EventArgs e)
        {
            if (Events[savePointLeftEventKey] is EventHandler<EventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="SavePointReached" /> event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnSavePointReached(EventArgs e)
        {
            if (Events[savePointReachedEventKey] is EventHandler<EventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="StyleNeeded" /> event.
        /// </summary>
        /// <param name="e">A <see cref="StyleNeededEventArgs" /> that contains the event data.</param>
        protected virtual void OnStyleNeeded(StyleNeededEventArgs e)
        {
            if (Events[styleNeededEventKey] is EventHandler<StyleNeededEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="UpdateUI" /> event.
        /// </summary>
        /// <param name="e">An <see cref="UpdateUIEventArgs" /> that contains the event data.</param>
        protected virtual void OnUpdateUI(UpdateUIEventArgs e)
        {
            if (Events[updateUIEventKey] is EventHandler<UpdateUIEventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ZoomChanged" /> event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnZoomChanged(EventArgs e)
        {
            if (Events[zoomChangedEventKey] is EventHandler<EventArgs> handler)
                handler(this, e);
        }

        /// <summary>
        /// Pastes the contents of the clipboard into the current selection.
        /// </summary>
        public void Paste()
        {
            DirectMessage(NativeMethods.SCI_PASTE);
        }

        /// <summary>
        /// Returns the X display pixel location of the specified document position.
        /// </summary>
        /// <param name="pos">The zero-based document character position.</param>
        /// <returns>The x-coordinate of the specified <paramref name="pos" /> within the client rectangle of the control.</returns>
        public int PointXFromPosition(int pos)
        {
            pos = Helpers.Clamp(pos, 0, TextLength);
            pos = Lines.CharToBytePosition(pos);
            return DirectMessage(NativeMethods.SCI_POINTXFROMPOSITION, IntPtr.Zero, new IntPtr(pos)).ToInt32();
        }

        /// <summary>
        /// Returns the Y display pixel location of the specified document position.
        /// </summary>
        /// <param name="pos">The zero-based document character position.</param>
        /// <returns>The y-coordinate of the specified <paramref name="pos" /> within the client rectangle of the control.</returns>
        public int PointYFromPosition(int pos)
        {
            pos = Helpers.Clamp(pos, 0, TextLength);
            pos = Lines.CharToBytePosition(pos);
            return DirectMessage(NativeMethods.SCI_POINTYFROMPOSITION, IntPtr.Zero, new IntPtr(pos)).ToInt32();
        }

        /// <summary>
        /// Retrieves a list of property names that can be set for the current <see cref="LexerName">Lexer</see>.
        /// </summary>
        /// <returns>A String of property names separated by line breaks.</returns>
        public unsafe string PropertyNames()
        {
            int length = DirectMessage(NativeMethods.SCI_PROPERTYNAMES).ToInt32();
            if (length == 0)
                return string.Empty;

            byte[] bytes = new byte[length + 1];
            fixed (byte* bp = bytes)
            {
                DirectMessage(NativeMethods.SCI_PROPERTYNAMES, IntPtr.Zero, new IntPtr(bp));
                return Helpers.GetString(new IntPtr(bp), length, Encoding.ASCII);
            }
        }

        /// <summary>
        /// Retrieves the data type of the specified property name for the current <see cref="LexerName">Lexer</see>.
        /// </summary>
        /// <param name="name">A property name supported by the current <see cref="LexerName">Lexer</see>.</param>
        /// <returns>One of the <see cref="PropertyType" /> enumeration values. The default is <see cref="ScintillaNET.PropertyType.Boolean" />.</returns>
        /// <remarks>A list of supported property names for the current <see cref="LexerName">Lexer</see> can be obtained by calling <see cref="PropertyNames" />.</remarks>
        public unsafe PropertyType PropertyType(string name)
        {
            if (string.IsNullOrEmpty(name))
                return ScintillaNET.PropertyType.Boolean;

            byte[] bytes = Helpers.GetBytes(name, Encoding.ASCII, zeroTerminated: true);
            fixed (byte* bp = bytes)
                return (PropertyType)DirectMessage(NativeMethods.SCI_PROPERTYTYPE, new IntPtr(bp));
        }

        /// <summary>
        /// Redoes the effect of an <see cref="Undo" /> operation.
        /// </summary>
        public void Redo()
        {
            DirectMessage(NativeMethods.SCI_REDO);
        }

        /// <summary>
        /// Maps the specified image to a type identifer for use in an autocompletion list.
        /// </summary>
        /// <param name="type">The numeric identifier for this image.</param>
        /// <param name="image">The Bitmap to use in an autocompletion list.</param>
        /// <remarks>
        /// The <paramref name="image" /> registered can be referenced by its <paramref name="type" /> identifer in an autocompletion
        /// list by suffixing a word with the <see cref="AutoCTypeSeparator" /> character and the <paramref name="type" /> value. e.g.
        /// "int?2 long?3 short?1" etc....
        /// </remarks>
        /// <seealso cref="AutoCTypeSeparator" />
        public unsafe void RegisterRgbaImage(int type, Bitmap image)
        {
            // TODO Clamp type?
            if (image == null)
                return;

            DirectMessage(NativeMethods.SCI_RGBAIMAGESETWIDTH, new IntPtr(image.Width));
            DirectMessage(NativeMethods.SCI_RGBAIMAGESETHEIGHT, new IntPtr(image.Height));

            byte[] bytes = Helpers.BitmapToArgb(image);
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_REGISTERRGBAIMAGE, new IntPtr(type), new IntPtr(bp));
        }

        /// <summary>
        /// Decreases the reference count of the specified document by 1.
        /// </summary>
        /// <param name="document">
        /// The document reference count to decrease.
        /// When a document's reference count reaches 0 it is destroyed and any associated memory released.
        /// </param>
        public void ReleaseDocument(Document document)
        {
            IntPtr ptr = document.Value;
            DirectMessage(NativeMethods.SCI_RELEASEDOCUMENT, IntPtr.Zero, ptr);
        }

        /// <summary>
        /// Replaces the current selection with the specified text.
        /// </summary>
        /// <param name="text">The text that should replace the current selection.</param>
        /// <remarks>
        /// If there is not a current selection, the text will be inserted at the current caret position.
        /// Following the operation the caret is placed at the end of the inserted text and scrolled into view.
        /// </remarks>
        public unsafe void ReplaceSelection(string text)
        {
            fixed (byte* bp = Helpers.GetBytes(text ?? string.Empty, Encoding, zeroTerminated: true))
                DirectMessage(NativeMethods.SCI_REPLACESEL, IntPtr.Zero, new IntPtr(bp));
        }

        /// <summary>
        /// Replaces the target defined by <see cref="TargetStart" /> and <see cref="TargetEnd" /> with the specified <paramref name="text" />.
        /// </summary>
        /// <param name="text">The text that will replace the current target.</param>
        /// <returns>The length of the replaced text.</returns>
        /// <remarks>
        /// The <see cref="TargetStart" /> and <see cref="TargetEnd" /> properties will be updated to the start and end positions of the replaced text.
        /// The recommended way to delete text in the document is to set the target range to be removed and replace the target with an empty string.
        /// </remarks>
        public unsafe int ReplaceTarget(string text)
        {
            text ??= string.Empty;

            byte[] bytes = Helpers.GetBytes(text, Encoding, false);
            // Scintilla asserts that lParam is not null, so make sure it isn't
            int length = bytes.Length;
            if (length == 0)
                bytes = new byte[] { 0 };
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_REPLACETARGET, new IntPtr(length), new IntPtr(bp));

            return text.Length;
        }

        /// <summary>
        /// Replaces the target text defined by <see cref="TargetStart" /> and <see cref="TargetEnd" /> with the specified value after first substituting
        /// "\1" through "\9" macros in the <paramref name="text" /> with the most recent regular expression capture groups.
        /// </summary>
        /// <param name="text">The text containing "\n" macros that will be substituted with the most recent regular expression capture groups and then replace the current target.</param>
        /// <returns>The length of the replaced text.</returns>
        /// <remarks>
        /// The "\0" macro will be substituted by the entire matched text from the most recent search.
        /// The <see cref="TargetStart" /> and <see cref="TargetEnd" /> properties will be updated to the start and end positions of the replaced text.
        /// </remarks>
        /// <seealso cref="GetTag" />
        public unsafe int ReplaceTargetRe(string text)
        {
            byte[] bytes = Helpers.GetBytes(text ?? string.Empty, Encoding, false);
            // Scintilla asserts that lParam is not null, so make sure it isn't
            int length = bytes.Length;
            if (length == 0)
                bytes = new byte[] { 0 };
            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_REPLACETARGETRE, new IntPtr(length), new IntPtr(bp));

            return Math.Abs(TargetEnd - TargetStart);
        }

        private void ResetAdditionalCaretForeColor()
        {
            AdditionalCaretForeColor = Color.FromArgb(127, 127, 127);
        }

        /// <summary>
        /// Makes the next selection the main selection.
        /// </summary>
        public void RotateSelection()
        {
            DirectMessage(NativeMethods.SCI_ROTATESELECTION);
        }

        private void ScnDoubleClick(ref NativeMethods.SCNotification scn)
        {
            Keys keys = Keys.Modifiers & (Keys)(scn.modifiers << 16);
            var eventArgs = new DoubleClickEventArgs(this, keys, scn.position.ToInt32(), scn.line.ToInt32());
            OnDoubleClick(eventArgs);
        }

        private void ScnHotspotClick(ref NativeMethods.SCNotification scn)
        {
            Keys keys = Keys.Modifiers & (Keys)(scn.modifiers << 16);
            var eventArgs = new HotspotClickEventArgs(this, keys, scn.position.ToInt32());
            switch (scn.nmhdr.code)
            {
                case NativeMethods.SCN_HOTSPOTCLICK:
                    OnHotspotClick(eventArgs);
                    break;

                case NativeMethods.SCN_HOTSPOTDOUBLECLICK:
                    OnHotspotDoubleClick(eventArgs);
                    break;

                case NativeMethods.SCN_HOTSPOTRELEASECLICK:
                    OnHotspotReleaseClick(eventArgs);
                    break;
            }
        }

        private void ScnIndicatorClick(ref NativeMethods.SCNotification scn)
        {
            switch (scn.nmhdr.code)
            {
                case NativeMethods.SCN_INDICATORCLICK:
                    Keys keys = Keys.Modifiers & (Keys)(scn.modifiers << 16);
                    OnIndicatorClick(new IndicatorClickEventArgs(this, keys, scn.position.ToInt32()));
                    break;

                case NativeMethods.SCN_INDICATORRELEASE:
                    OnIndicatorRelease(new IndicatorReleaseEventArgs(this, scn.position.ToInt32()));
                    break;
            }
        }

        private void ScnMarginClick(ref NativeMethods.SCNotification scn)
        {
            Keys keys = Keys.Modifiers & (Keys)(scn.modifiers << 16);
            var eventArgs = new MarginClickEventArgs(this, keys, scn.position.ToInt32(), scn.margin);

            if (scn.nmhdr.code == NativeMethods.SCN_MARGINCLICK)
                OnMarginClick(eventArgs);
            else
                OnMarginRightClick(eventArgs);
        }

        private void ScnModified(ref NativeMethods.SCNotification scn)
        {
            // The InsertCheck, BeforeInsert, BeforeDelete, Insert, and Delete events can all potentially require
            // the same conversions: byte to char position, char* to string, etc.... To avoid doing the same work
            // multiple times we share that data between events.

            if ((scn.modificationType & NativeMethods.SC_MOD_INSERTCHECK) > 0)
            {
                var eventArgs = new InsertCheckEventArgs(this, scn.position.ToInt32(), scn.length.ToInt32(), scn.text);
                OnInsertCheck(eventArgs);

                this.cachedPosition = eventArgs.CachedPosition;
                this.cachedText = eventArgs.CachedText;
            }

            const int sourceMask = NativeMethods.SC_PERFORMED_USER | NativeMethods.SC_PERFORMED_UNDO | NativeMethods.SC_PERFORMED_REDO;

            if ((scn.modificationType & (NativeMethods.SC_MOD_BEFOREDELETE | NativeMethods.SC_MOD_BEFOREINSERT)) > 0)
            {
                var source = (ModificationSource)(scn.modificationType & sourceMask);
                var eventArgs = new BeforeModificationEventArgs(this, source, scn.position.ToInt32(), scn.length.ToInt32(), scn.text) {
                    CachedPosition = this.cachedPosition,
                    CachedText = this.cachedText
                };

                if ((scn.modificationType & NativeMethods.SC_MOD_BEFOREINSERT) > 0)
                {
                    OnBeforeInsert(eventArgs);
                }
                else
                {
                    OnBeforeDelete(eventArgs);
                }

                this.cachedPosition = eventArgs.CachedPosition;
                this.cachedText = eventArgs.CachedText;
            }

            if ((scn.modificationType & (NativeMethods.SC_MOD_DELETETEXT | NativeMethods.SC_MOD_INSERTTEXT)) > 0)
            {
                var source = (ModificationSource)(scn.modificationType & sourceMask);
                var eventArgs = new ModificationEventArgs(this, source, scn.position.ToInt32(), scn.length.ToInt32(), scn.text, scn.linesAdded.ToInt32()) {
                    CachedPosition = this.cachedPosition,
                    CachedText = this.cachedText
                };

                if ((scn.modificationType & NativeMethods.SC_MOD_INSERTTEXT) > 0)
                {
                    OnInsert(eventArgs);
                }
                else
                {
                    OnDelete(eventArgs);
                }

                // Always clear the cache
                this.cachedPosition = null;
                this.cachedText = null;

                // For backward compatibility.... Of course this means that we'll raise two
                // TextChanged events for replace (insert/delete) operations, but that's life.
                OnTextChanged(EventArgs.Empty);
            }

            if ((scn.modificationType & NativeMethods.SC_MOD_CHANGEANNOTATION) > 0)
            {
                var eventArgs = new ChangeAnnotationEventArgs(scn.line.ToInt32());
                OnChangeAnnotation(eventArgs);
            }
        }

        /// <summary>
        /// Scrolls the current position into view, if it is not already visible.
        /// </summary>
        public void ScrollCaret()
        {
            DirectMessage(NativeMethods.SCI_SCROLLCARET);
        }

        /// <summary>
        /// Scrolls the specified range into view.
        /// </summary>
        /// <param name="start">The zero-based document start position to scroll to.</param>
        /// <param name="end">
        /// The zero-based document end position to scroll to if doing so does not cause the <paramref name="start" />
        /// position to scroll out of view.
        /// </param>
        /// <remarks>This may be used to make a search match visible.</remarks>
        public void ScrollRange(int start, int end)
        {
            int textLength = TextLength;
            start = Helpers.Clamp(start, 0, textLength);
            end = Helpers.Clamp(end, 0, textLength);

            // Convert to byte positions
            start = Lines.CharToBytePosition(start);
            end = Lines.CharToBytePosition(end);

            // The arguments would  seem reverse from Scintilla documentation
            // but empirical  evidence suggests this is correct....
            DirectMessage(NativeMethods.SCI_SCROLLRANGE, new IntPtr(start), new IntPtr(end));
        }

        /// <summary>
        /// Searches for the first occurrence of the specified text in the target defined by <see cref="TargetStart" /> and <see cref="TargetEnd" />.
        /// </summary>
        /// <param name="text">The text to search for. The interpretation of the text (i.e. whether it is a regular expression) is defined by the <see cref="SearchFlags" /> property.</param>
        /// <returns>The zero-based start position of the matched text within the document if successful; otherwise, -1.</returns>
        /// <remarks>
        /// If successful, the <see cref="TargetStart" /> and <see cref="TargetEnd" /> properties will be updated to the start and end positions of the matched text.
        /// Searching can be performed in reverse using a <see cref="TargetStart" /> greater than the <see cref="TargetEnd" />.
        /// </remarks>
        public unsafe int SearchInTarget(string text)
        {
            int bytePos = 0;
            byte[] bytes = Helpers.GetBytes(text ?? string.Empty, Encoding, zeroTerminated: false);
            // Scintilla asserts that lParam is not null, so make sure it isn't
            int length = bytes.Length;
            if (length == 0)
                bytes = new byte[] { 0 };
            fixed (byte* bp = bytes)
                bytePos = DirectMessage(NativeMethods.SCI_SEARCHINTARGET, new IntPtr(length), new IntPtr(bp)).ToInt32();

            if (bytePos == -1)
                return bytePos;

            return Lines.ByteToCharPosition(bytePos);
        }

        /// <summary>
        /// Selects all the text in the document.
        /// </summary>
        /// <remarks>The current position is not scrolled into view.</remarks>
        public void SelectAll()
        {
            DirectMessage(NativeMethods.SCI_SELECTALL);
        }

        /// <summary>
        /// Sets the background color of additional selections.
        /// </summary>
        /// <param name="color">Additional selections background color.</param>
        /// <remarks>Calling <see cref="SetSelectionBackColor" /> will reset the <paramref name="color" /> specified.</remarks>
        [Obsolete("Superseded by SelectionAdditionalBackColor property.")]
        public void SetAdditionalSelBack(Color color)
        {
            int colour = HelperMethods.ToWin32ColorOpaque(color);
            DirectMessage(NativeMethods.SCI_SETADDITIONALSELBACK, new IntPtr(colour));
        }

        /// <summary>
        /// Sets the foreground color of additional selections.
        /// </summary>
        /// <param name="color">Additional selections foreground color.</param>
        /// <remarks>Calling <see cref="SetSelectionForeColor" /> will reset the <paramref name="color" /> specified.</remarks>
        [Obsolete("Superseded by SelectionAdditionalTextColor property.")]
        public void SetAdditionalSelFore(Color color)
        {
            int colour = HelperMethods.ToWin32ColorOpaque(color);
            DirectMessage(NativeMethods.SCI_SETADDITIONALSELFORE, new IntPtr(colour));
        }

        /// <summary>
        /// Removes any selection and places the caret at the specified position.
        /// </summary>
        /// <param name="pos">The zero-based document position to place the caret at.</param>
        /// <remarks>The caret is not scrolled into view.</remarks>
        public void SetEmptySelection(int pos)
        {
            pos = Helpers.Clamp(pos, 0, TextLength);
            pos = Lines.CharToBytePosition(pos);
            DirectMessage(NativeMethods.SCI_SETEMPTYSELECTION, new IntPtr(pos));
        }

        /// <summary>
        /// Sets additional options for displaying folds.
        /// </summary>
        /// <param name="flags">A bitwise combination of the <see cref="FoldFlags" /> enumeration.</param>
        public void SetFoldFlags(FoldFlags flags)
        {
            DirectMessage(NativeMethods.SCI_SETFOLDFLAGS, new IntPtr((int)flags));
        }

        /// <summary>
        /// Sets a global override to the fold margin color.
        /// </summary>
        /// <param name="use">true to override the fold margin color; otherwise, false.</param>
        /// <param name="color">The global fold margin color.</param>
        /// <seealso cref="SetFoldMarginHighlightColor" />
        public void SetFoldMarginColor(bool use, Color color)
        {
            int colour = HelperMethods.ToWin32ColorOpaque(color);
            IntPtr useFoldMarginColour = use ? new IntPtr(1) : IntPtr.Zero;

            DirectMessage(NativeMethods.SCI_SETFOLDMARGINCOLOUR, useFoldMarginColour, new IntPtr(colour));
        }

        /// <summary>
        /// Sets a global override to the fold margin highlight color.
        /// </summary>
        /// <param name="use">true to override the fold margin highlight color; otherwise, false.</param>
        /// <param name="color">The global fold margin highlight color.</param>
        /// <seealso cref="SetFoldMarginColor" />
        public void SetFoldMarginHighlightColor(bool use, Color color)
        {
            int colour = HelperMethods.ToWin32ColorOpaque(color);
            IntPtr useFoldMarginHighlightColour = use ? new IntPtr(1) : IntPtr.Zero;

            DirectMessage(NativeMethods.SCI_SETFOLDMARGINHICOLOUR, useFoldMarginHighlightColour, new IntPtr(colour));
        }

        /// <summary>
        /// Similar to <see cref="SetKeywords" /> but for substyles.
        /// </summary>
        /// <param name="style">The substyle integer index</param>
        /// <param name="identifiers">A list of words separated by whitespace (space, tab, '\n', '\r') characters.</param>
        public unsafe void SetIdentifiers(int style, string identifiers)
        {
            int baseStyle = GetStyleFromSubstyle(style);
            int min = GetSubstylesStart(baseStyle);
            int length = GetSubstylesLength(baseStyle);
            int max = (length > 0) ? min + length - 1 : min;

            style = Helpers.Clamp(style, min, max);
            byte[] bytes = Helpers.GetBytes(identifiers ?? string.Empty, Encoding.ASCII, zeroTerminated: true);

            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_SETIDENTIFIERS, new IntPtr(style), new IntPtr(bp));
        }

        /// <summary>
        /// Updates a keyword set used by the current <see cref="LexerName">Lexer</see>.
        /// </summary>
        /// <param name="set">The zero-based index of the keyword set to update.</param>
        /// <param name="keywords">
        /// A list of keywords pertaining to the current <see cref="LexerName">Lexer</see> separated by whitespace (space, tab, '\n', '\r') characters.
        /// </param>
        /// <remarks>The keywords specified will be styled according to the current <see cref="LexerName">Lexer</see>.</remarks>
        /// <seealso cref="DescribeKeywordSets" />
        public unsafe void SetKeywords(int set, string keywords)
        {
            set = Helpers.Clamp(set, 0, NativeMethods.KEYWORDSET_MAX);
            byte[] bytes = Helpers.GetBytes(keywords ?? string.Empty, Encoding.ASCII, zeroTerminated: true);

            fixed (byte* bp = bytes)
                DirectMessage(NativeMethods.SCI_SETKEYWORDS, new IntPtr(set), new IntPtr(bp));
        }

        /// <summary>
        /// Sets the application-wide behavior for destroying <see cref="Scintilla" /> controls.
        /// </summary>
        /// <param name="reparent">
        /// true to reparent Scintilla controls to message-only windows when destroyed rather than actually destroying the control handle; otherwise, false.
        /// The default is true.
        /// </param>
        /// <remarks>This method must be called prior to the first <see cref="Scintilla" /> control being created.</remarks>
        public static void SetDestroyHandleBehavior(bool reparent)
        {
            // WM_DESTROY workaround
            Scintilla.reparentAll ??= reparent;
        }

        /// <summary>
        /// Passes the specified property name-value pair to the current <see cref="LexerName">Lexer</see>.
        /// </summary>
        /// <param name="name">The property name to set.</param>
        /// <param name="value">
        /// The property value. Values can refer to other property names using the syntax $(name), where 'name' is another property
        /// name for the current <see cref="LexerName">Lexer</see>. When the property value is retrieved by a call to <see cref="GetPropertyExpanded" />
        /// the embedded property name macro will be replaced (expanded) with that current property value.
        /// </param>
        /// <remarks>Property names are case-sensitive.</remarks>
        public unsafe void SetProperty(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                return;

            byte[] nameBytes = Helpers.GetBytes(name, Encoding.ASCII, zeroTerminated: true);
            byte[] valueBytes = Helpers.GetBytes(value ?? string.Empty, Encoding.ASCII, zeroTerminated: true);

            fixed (byte* nb = nameBytes)
            fixed (byte* vb = valueBytes)
            {
                DirectMessage(NativeMethods.SCI_SETPROPERTY, new IntPtr(nb), new IntPtr(vb));
            }
        }

        private bool SetRenderer(VisualStyleElement element)
        {
            if (!Application.RenderWithVisualStyles)
                return false;

            if (!VisualStyleRenderer.IsElementDefined(element))
                return false;

            if (this.renderer == null)
                this.renderer = new VisualStyleRenderer(element);
            else
                this.renderer.SetParameters(element);

            return true;
        }

        /// <summary>
        /// Marks the document as unmodified.
        /// </summary>
        /// <seealso cref="Modified" />
        public void SetSavePoint()
        {
            DirectMessage(NativeMethods.SCI_SETSAVEPOINT);
        }

        /// <summary>
        /// Sets the anchor and current position.
        /// </summary>
        /// <param name="anchorPos">The zero-based document position to start the selection.</param>
        /// <param name="currentPos">The zero-based document position to end the selection.</param>
        /// <remarks>
        /// A negative value for <paramref name="currentPos" /> signifies the end of the document.
        /// A negative value for <paramref name="anchorPos" /> signifies no selection (i.e. sets the <paramref name="anchorPos" />
        /// to the same position as the <paramref name="currentPos" />).
        /// The current position is scrolled into view following this operation.
        /// </remarks>
        public void SetSel(int anchorPos, int currentPos)
        {
            if (anchorPos == currentPos)
            {
                // Optimization so that we don't have to translate the anchor position
                // when we can instead just pass -1 and have Scintilla handle it.
                anchorPos = -1;
            }

            int textLength = TextLength;

            if (anchorPos >= 0)
            {
                anchorPos = Helpers.Clamp(anchorPos, 0, textLength);
                anchorPos = Lines.CharToBytePosition(anchorPos);
            }

            if (currentPos >= 0)
            {
                currentPos = Helpers.Clamp(currentPos, 0, textLength);
                currentPos = Lines.CharToBytePosition(currentPos);
            }

            DirectMessage(NativeMethods.SCI_SETSEL, new IntPtr(anchorPos), new IntPtr(currentPos));
        }

        /// <summary>
        /// Sets a single selection from anchor to caret.
        /// </summary>
        /// <param name="caret">The zero-based document position to end the selection.</param>
        /// <param name="anchor">The zero-based document position to start the selection.</param>
        public void SetSelection(int caret, int anchor)
        {
            int textLength = TextLength;

            caret = Helpers.Clamp(caret, 0, textLength);
            anchor = Helpers.Clamp(anchor, 0, textLength);

            caret = Lines.CharToBytePosition(caret);
            anchor = Lines.CharToBytePosition(anchor);

            DirectMessage(NativeMethods.SCI_SETSELECTION, new IntPtr(caret), new IntPtr(anchor));
        }

        /// <summary>
        /// Sets a global override to the selection background color.
        /// </summary>
        /// <param name="use">true to override the selection background color; otherwise, false.</param>
        /// <param name="color">The global selection background color.</param>
        /// <seealso cref="SetSelectionForeColor" />
        [Obsolete("Superseded by SelectionBackColor property.")]
        public void SetSelectionBackColor(bool use, Color color)
        {
            int colour = HelperMethods.ToWin32ColorOpaque(color);
            IntPtr useSelectionForeColour = use ? new IntPtr(1) : IntPtr.Zero;

            DirectMessage(NativeMethods.SCI_SETSELBACK, useSelectionForeColour, new IntPtr(colour));
        }

        /// <summary>
        /// Sets a global override to the selection foreground color.
        /// </summary>
        /// <param name="use">true to override the selection foreground color; otherwise, false.</param>
        /// <param name="color">The global selection foreground color.</param>
        /// <seealso cref="SetSelectionBackColor" />
        [Obsolete("Superseded by SelectionTextColor property.")]
        public void SetSelectionForeColor(bool use, Color color)
        {
            int colour = HelperMethods.ToWin32ColorOpaque(color);
            IntPtr useSelectionForeColour = use ? new IntPtr(1) : IntPtr.Zero;

            DirectMessage(NativeMethods.SCI_SETSELFORE, useSelectionForeColour, new IntPtr(colour));
        }

        /// <summary>
        /// Gets or sets the layer where the text selection will be painted. Default value is <see cref="Layer.Base"/>
        /// </summary>
        [DefaultValue(Layer.Base)]
        [Category("Selection")]
        [Description("The layer where the text selection will be painted.")]
        public Layer SelectionLayer
        {
            get
            {
                return (Layer)DirectMessage(NativeMethods.SCI_GETSELECTIONLAYER).ToInt32();
            }
            set
            {
                int layer = (int)value;
                DirectMessage(NativeMethods.SCI_SETSELECTIONLAYER, new IntPtr(layer), IntPtr.Zero);
            }
        }

        /// <summary>
        /// Styles the specified length of characters.
        /// </summary>
        /// <param name="length">The number of characters to style.</param>
        /// <param name="style">The <see cref="Style" /> definition index to assign each character.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> or <paramref name="style" /> is less than zero. -or-
        /// The sum of a preceeding call to <see cref="StartStyling" /> or <see name="SetStyling" /> and <paramref name="length" /> is greater than the document length. -or-
        /// <paramref name="style" /> is greater than or equal to the number of style definitions.
        /// </exception>
        /// <remarks>
        /// The styling position is advanced by <paramref name="length" /> after each call allowing multiple
        /// calls to <see cref="SetStyling" /> for a single call to <see cref="StartStyling" />.
        /// </remarks>
        /// <seealso cref="StartStyling" />
        public void SetStyling(int length, int style)
        {
            int textLength = TextLength;

            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
            if (this.stylingPosition + length > textLength)
                throw new ArgumentOutOfRangeException("length", "Position and length must refer to a range within the document.");
            if (style < 0 || style >= Styles.Count)
                throw new ArgumentOutOfRangeException("style", "Style must be non-negative and less than the size of the collection.");

            int endPos = this.stylingPosition + length;
            int endBytePos = Lines.CharToBytePosition(endPos);
            DirectMessage(NativeMethods.SCI_SETSTYLING, new IntPtr(endBytePos - this.stylingBytePosition), new IntPtr(style));

            // Track this for the next call
            this.stylingPosition = endPos;
            this.stylingBytePosition = endBytePos;
        }

        /// <summary>
        /// Sets the <see cref="TargetStart" /> and <see cref="TargetEnd" /> properties in a single call.
        /// </summary>
        /// <param name="start">The zero-based character position within the document to start a search or replace operation.</param>
        /// <param name="end">The zero-based character position within the document to end a search or replace operation.</param>
        /// <seealso cref="TargetStart" />
        /// <seealso cref="TargetEnd" />
        public void SetTargetRange(int start, int end)
        {
            int textLength = TextLength;
            start = Helpers.Clamp(start, 0, textLength);
            end = Helpers.Clamp(end, 0, textLength);

            start = Lines.CharToBytePosition(start);
            end = Lines.CharToBytePosition(end);

            DirectMessage(NativeMethods.SCI_SETTARGETRANGE, new IntPtr(start), new IntPtr(end));
        }

        /// <summary>
        /// Sets a global override to the whitespace background color.
        /// </summary>
        /// <param name="use">true to override the whitespace background color; otherwise, false.</param>
        /// <param name="color">The global whitespace background color.</param>
        /// <remarks>When not overridden globally, the whitespace background color is determined by the current lexer.</remarks>
        /// <seealso cref="ViewWhitespace" />
        /// <seealso cref="SetWhitespaceForeColor" />
        [Obsolete("Superseded by WhitespaceBackColor property.")]
        public void SetWhitespaceBackColor(bool use, Color color)
        {
            int colour = HelperMethods.ToWin32ColorOpaque(color);
            IntPtr useWhitespaceBackColour = use ? new IntPtr(1) : IntPtr.Zero;

            DirectMessage(NativeMethods.SCI_SETWHITESPACEBACK, useWhitespaceBackColour, new IntPtr(colour));
        }

        /// <summary>
        /// Sets a global override to the whitespace foreground color.
        /// </summary>
        /// <param name="use">true to override the whitespace foreground color; otherwise, false.</param>
        /// <param name="color">The global whitespace foreground color.</param>
        /// <remarks>When not overridden globally, the whitespace foreground color is determined by the current lexer.</remarks>
        /// <seealso cref="ViewWhitespace" />
        /// <seealso cref="SetWhitespaceBackColor" />
        [Obsolete("Superseded by WhitespaceTextColor property.")]
        public void SetWhitespaceForeColor(bool use, Color color)
        {
            int colour = HelperMethods.ToWin32ColorOpaque(color);
            IntPtr useWhitespaceForeColour = use ? new IntPtr(1) : IntPtr.Zero;

            DirectMessage(NativeMethods.SCI_SETWHITESPACEFORE, useWhitespaceForeColour, new IntPtr(colour));
        }

        /// <summary>
        /// Sets the X caret policy.
        /// </summary>
        /// <param name="caretPolicy">a combination of <see cref="CaretPolicy"/> values.</param>
        /// <param name="caretSlop">the caretSlop value</param>
        public void SetXCaretPolicy(CaretPolicy caretPolicy, int caretSlop)
        {
            DirectMessage(NativeMethods.SCI_SETXCARETPOLICY, new IntPtr((int)caretPolicy), new IntPtr(caretSlop));
        }

        /// <summary>
        /// Sets the Y caret policy.
        /// </summary>
        /// <param name="caretPolicy">a combination of <see cref="CaretPolicy"/> values.</param>
        /// <param name="caretSlop">the caretSlop value</param>
        public void SetYCaretPolicy(CaretPolicy caretPolicy, int caretSlop)
        {
            DirectMessage(NativeMethods.SCI_SETYCARETPOLICY, new IntPtr((int)caretPolicy), new IntPtr(caretSlop));
        }

        private bool ShouldSerializeAdditionalCaretForeColor()
        {
            return AdditionalCaretForeColor != Color.FromArgb(127, 127, 127);
        }

        /// <summary>
        /// Shows the range of lines specified.
        /// </summary>
        /// <param name="lineStart">The zero-based index of the line range to start showing.</param>
        /// <param name="lineEnd">The zero-based index of the line range to end showing.</param>
        /// <seealso cref="HideLines" />
        /// <seealso cref="Line.Visible" />
        public void ShowLines(int lineStart, int lineEnd)
        {
            lineStart = Helpers.Clamp(lineStart, 0, Lines.Count);
            lineEnd = Helpers.Clamp(lineEnd, lineStart, Lines.Count);

            DirectMessage(NativeMethods.SCI_SHOWLINES, new IntPtr(lineStart), new IntPtr(lineEnd));
        }

        /// <summary>
        /// Prepares for styling by setting the styling <paramref name="position" /> to start at.
        /// </summary>
        /// <param name="position">The zero-based character position in the document to start styling.</param>
        /// <remarks>
        /// After preparing the document for styling, use successive calls to <see cref="SetStyling" />
        /// to style the document.
        /// </remarks>
        /// <seealso cref="SetStyling" />
        public void StartStyling(int position)
        {
            position = Helpers.Clamp(position, 0, TextLength);
            int pos = Lines.CharToBytePosition(position);
            DirectMessage(NativeMethods.SCI_STARTSTYLING, new IntPtr(pos));

            // Track this so we can validate calls to SetStyling
            this.stylingPosition = position;
            this.stylingBytePosition = pos;
        }

        /// <summary>
        /// Resets all style properties to those currently configured for the <see cref="Style.Default" /> style.
        /// </summary>
        /// <seealso cref="StyleResetDefault" />
        public void StyleClearAll()
        {
            DirectMessage(NativeMethods.SCI_STYLECLEARALL);
        }

        /// <summary>
        /// Resets the <see cref="Style.Default" /> style to its initial state.
        /// </summary>
        /// <seealso cref="StyleClearAll" />
        public void StyleResetDefault()
        {
            DirectMessage(NativeMethods.SCI_STYLERESETDEFAULT);
        }

        /// <summary>
        /// Moves the caret to the opposite end of the main selection.
        /// </summary>
        public void SwapMainAnchorCaret()
        {
            DirectMessage(NativeMethods.SCI_SWAPMAINANCHORCARET);
        }

        /// <summary>
        /// Sets the <see cref="TargetStart" /> and <see cref="TargetEnd" /> to the start and end positions of the selection.
        /// </summary>
        /// <seealso cref="TargetWholeDocument" />
        public void TargetFromSelection()
        {
            DirectMessage(NativeMethods.SCI_TARGETFROMSELECTION);
        }

        /// <summary>
        /// Sets the <see cref="TargetStart" /> and <see cref="TargetEnd" /> to the start and end positions of the document.
        /// </summary>
        /// <seealso cref="TargetFromSelection" />
        public void TargetWholeDocument()
        {
            DirectMessage(NativeMethods.SCI_TARGETWHOLEDOCUMENT);
        }

        /// <summary>
        /// Measures the width in pixels of the specified string when rendered in the specified style.
        /// </summary>
        /// <param name="style">The index of the <see cref="Style" /> to use when rendering the text to measure.</param>
        /// <param name="text">The text to measure.</param>
        /// <returns>The width in pixels.</returns>
        public unsafe int TextWidth(int style, string text)
        {
            style = Helpers.Clamp(style, 0, Styles.Count - 1);
            byte[] bytes = Helpers.GetBytes(text ?? string.Empty, Encoding, zeroTerminated: true);

            fixed (byte* bp = bytes)
            {
                return DirectMessage(NativeMethods.SCI_TEXTWIDTH, new IntPtr(style), new IntPtr(bp)).ToInt32();
            }
        }

        /// <summary>
        /// Undoes the previous action.
        /// </summary>
        public void Undo()
        {
            DirectMessage(NativeMethods.SCI_UNDO);
        }

        /// <summary>
        /// Determines whether to show the right-click context menu.
        /// </summary>
        /// <param name="enablePopup">true to enable the popup window; otherwise, false.</param>
        /// <seealso cref="UsePopup(PopupMode)" />
        public void UsePopup(bool enablePopup)
        {
            // NOTE: The behavior of UsePopup has changed in v3.7.1, however, this approach is still valid
            IntPtr bEnablePopup = enablePopup ? new IntPtr(1) : IntPtr.Zero;
            DirectMessage(NativeMethods.SCI_USEPOPUP, bEnablePopup);
        }

        /// <summary>
        /// Determines the conditions for displaying the standard right-click context menu.
        /// </summary>
        /// <param name="popupMode">One of the <seealso cref="PopupMode" /> enumeration values.</param>
        public void UsePopup(PopupMode popupMode)
        {
            DirectMessage(NativeMethods.SCI_USEPOPUP, new IntPtr((int)popupMode));
        }

        private void WmDestroy(ref Message m)
        {
            // WM_DESTROY workaround
            if (this.reparent && IsHandleCreated)
            {
                // In some circumstances it's possible for the control's window handle to be destroyed
                // and recreated during the life of the control. I have no idea why Windows Forms was coded
                // this way but that creates an issue for us because most/all of our control state is stored
                // in the native Scintilla control (i.e. Handle) and to destroy it will bork us. So, rather
                // than destroying the handle as requested, we "reparent" ourselves to a message-only
                // (invisible) window to keep our handle alive. It doesn't appear that this causes any
                // issues to Windows Forms because it is completely unaware of it. When a control goes through
                // its regular (re)create handle process one of the steps is to assign the parent and so our
                // temporary bait-and-switch gets reconciled again automatically. Our Dispose method ensures
                // that we truly get destroyed when the time is right.

                NativeMethods.SetParent(Handle, new IntPtr(NativeMethods.HWND_MESSAGE));
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);
        }

        private void WmNcPaint(ref Message m)
        {
            // We only paint when border is 3D with visual styles
            if (BorderStyle != BorderStyle.Fixed3DVisualStyles)
            {
                base.WndProc(ref m);
                return;
            }

            // Configure the renderer
            VisualStyleElement element = VisualStyleElement.TextBox.TextEdit.Normal;
            /*if (!Enabled)
                element = VisualStyleElement.TextBox.TextEdit.Disabled;
            else*/
            if (ReadOnly)
                element = VisualStyleElement.TextBox.TextEdit.ReadOnly;
            else if (Focused)
                element = VisualStyleElement.TextBox.TextEdit.Focused;

            if (!SetRenderer(element))
            {
                base.WndProc(ref m);
                return;
            }

            NativeMethods.GetWindowRect(m.HWnd, out NativeMethods.RECT windowRect);
            Size borderSize = SystemInformation.Border3DSize;
            IntPtr hDC = NativeMethods.GetWindowDC(m.HWnd);
            try
            {
                using var graphics = Graphics.FromHdc(hDC);
                // Clip everything except the border
                var bounds = new Rectangle(0, 0, windowRect.right - windowRect.left, windowRect.bottom - windowRect.top);
                graphics.ExcludeClip(Rectangle.Inflate(bounds, -borderSize.Width, -borderSize.Height));

                // Paint the theme border
                if (this.renderer.IsBackgroundPartiallyTransparent())
                    this.renderer.DrawParentBackground(graphics, bounds, this);
                this.renderer.DrawBackground(graphics, bounds);
            }
            finally
            {
                NativeMethods.ReleaseDC(m.HWnd, hDC);
            }

            // Create a new region to pass to the default proc that excludes our border
            IntPtr clipRegion = NativeMethods.CreateRectRgn(
                windowRect.left + borderSize.Width,
                windowRect.top + borderSize.Height,
                windowRect.right - borderSize.Width,
                windowRect.bottom - borderSize.Height);

            if (m.WParam != (IntPtr)1)
                NativeMethods.CombineRgn(clipRegion, clipRegion, m.WParam, NativeMethods.RGN_AND);

            // Call default proc to get the scrollbars, etc... painted
            m.WParam = clipRegion;
            DefWndProc(ref m);
            m.Result = IntPtr.Zero;
        }

        private void WmReflectNotify(ref Message m)
        {
            // A standard Windows notification and a Scintilla notification header are compatible
            var scn = (NativeMethods.SCNotification)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.SCNotification));
            if (scn.nmhdr.code is >= NativeMethods.SCN_STYLENEEDED and <= NativeMethods.SCN_AUTOCSELECTIONCHANGE)
            {
                if (Events[scNotificationEventKey] is EventHandler<SCNotificationEventArgs> handler)
                    handler(this, new SCNotificationEventArgs(scn));

                switch (scn.nmhdr.code)
                {
                    case NativeMethods.SCN_PAINTED:
                        OnPainted(EventArgs.Empty);
                        break;

                    case NativeMethods.SCN_MODIFIED:
                        ScnModified(ref scn);
                        break;

                    case NativeMethods.SCN_MODIFYATTEMPTRO:
                        OnModifyAttempt(EventArgs.Empty);
                        break;

                    case NativeMethods.SCN_STYLENEEDED:
                        OnStyleNeeded(new StyleNeededEventArgs(this, scn.position.ToInt32()));
                        break;

                    case NativeMethods.SCN_SAVEPOINTLEFT:
                        OnSavePointLeft(EventArgs.Empty);
                        break;

                    case NativeMethods.SCN_SAVEPOINTREACHED:
                        OnSavePointReached(EventArgs.Empty);
                        break;

                    case NativeMethods.SCN_MARGINCLICK:
                    case NativeMethods.SCN_MARGINRIGHTCLICK:
                        ScnMarginClick(ref scn);
                        break;

                    case NativeMethods.SCN_UPDATEUI:
                        OnUpdateUI(new UpdateUIEventArgs((UpdateChange)scn.updated));
                        break;

                    case NativeMethods.SCN_CHARADDED:
                        OnCharAdded(new CharAddedEventArgs(scn.ch));
                        break;

                    case NativeMethods.SCN_AUTOCSELECTION:
                        OnAutoCSelection(new AutoCSelectionEventArgs(this, scn.position.ToInt32(), scn.text, scn.ch, (ListCompletionMethod)scn.listCompletionMethod));
                        break;

                    case NativeMethods.SCN_AUTOCCOMPLETED:
                        OnAutoCCompleted(new AutoCSelectionEventArgs(this, scn.position.ToInt32(), scn.text, scn.ch, (ListCompletionMethod)scn.listCompletionMethod));
                        break;

                    case NativeMethods.SCN_AUTOCCANCELLED:
                        OnAutoCCancelled(EventArgs.Empty);
                        break;

                    case NativeMethods.SCN_AUTOCCHARDELETED:
                        OnAutoCCharDeleted(EventArgs.Empty);
                        break;

                    case NativeMethods.SCN_AUTOCSELECTIONCHANGE:
                        OnAutoCSelectionChange(new AutoCSelectionChangeEventArgs(this, scn.text, scn.position.ToInt32(), scn.listType));
                        break;

                    case NativeMethods.SCN_DWELLSTART:
                        OnDwellStart(new DwellEventArgs(this, scn.position.ToInt32(), scn.x, scn.y));
                        break;

                    case NativeMethods.SCN_DWELLEND:
                        OnDwellEnd(new DwellEventArgs(this, scn.position.ToInt32(), scn.x, scn.y));
                        break;

                    case NativeMethods.SCN_DOUBLECLICK:
                        ScnDoubleClick(ref scn);
                        break;

                    case NativeMethods.SCN_NEEDSHOWN:
                        OnNeedShown(new NeedShownEventArgs(this, scn.position.ToInt32(), scn.length.ToInt32()));
                        break;

                    case NativeMethods.SCN_HOTSPOTCLICK:
                    case NativeMethods.SCN_HOTSPOTDOUBLECLICK:
                    case NativeMethods.SCN_HOTSPOTRELEASECLICK:
                        ScnHotspotClick(ref scn);
                        break;

                    case NativeMethods.SCN_INDICATORCLICK:
                    case NativeMethods.SCN_INDICATORRELEASE:
                        ScnIndicatorClick(ref scn);
                        break;

                    case NativeMethods.SCN_ZOOM:
                        OnZoomChanged(EventArgs.Empty);
                        break;

                    case NativeMethods.SCN_CALLTIPCLICK:
                        OnCallTipClick(new CallTipClickEventArgs(this, (CallTipClickType)scn.position.ToInt32()));
                        // scn.position: 1 = Up Arrow, 2 = DownArrow: 0 = Elsewhere
                        break;

                    default:
                        // Not our notification
                        base.WndProc(ref m);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows Message to process.</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_REFLECT + NativeMethods.WM_NOTIFY:
                    WmReflectNotify(ref m);
                    break;

                case NativeMethods.WM_SETCURSOR:
                    DefWndProc(ref m);
                    break;

                case NativeMethods.WM_NCPAINT:
                    WmNcPaint(ref m);
                    break;

                case NativeMethods.WM_LBUTTONDBLCLK:
                case NativeMethods.WM_RBUTTONDBLCLK:
                case NativeMethods.WM_MBUTTONDBLCLK:
                case NativeMethods.WM_XBUTTONDBLCLK:
                    this.doubleClick = true;
                    goto default;

                case NativeMethods.WM_DESTROY:
                    WmDestroy(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// Returns the position where a word ends, searching forward from the position specified.
        /// </summary>
        /// <param name="position">The zero-based document position to start searching from.</param>
        /// <param name="onlyWordCharacters">
        /// true to stop searching at the first non-word character regardless of whether the search started at a word or non-word character.
        /// false to use the first character in the search as a word or non-word indicator and then search for that word or non-word boundary.
        /// </param>
        /// <returns>The zero-based document postion of the word boundary.</returns>
        /// <seealso cref="WordStartPosition" />
        public int WordEndPosition(int position, bool onlyWordCharacters)
        {
            IntPtr onlyWordChars = onlyWordCharacters ? new IntPtr(1) : IntPtr.Zero;
            position = Helpers.Clamp(position, 0, TextLength);
            position = Lines.CharToBytePosition(position);
            position = DirectMessage(NativeMethods.SCI_WORDENDPOSITION, new IntPtr(position), onlyWordChars).ToInt32();
            return Lines.ByteToCharPosition(position);
        }

        /// <summary>
        /// Returns the position where a word starts, searching backward from the position specified.
        /// </summary>
        /// <param name="position">The zero-based document position to start searching from.</param>
        /// <param name="onlyWordCharacters">
        /// true to stop searching at the first non-word character regardless of whether the search started at a word or non-word character.
        /// false to use the first character in the search as a word or non-word indicator and then search for that word or non-word boundary.
        /// </param>
        /// <returns>The zero-based document postion of the word boundary.</returns>
        /// <seealso cref="WordEndPosition" />
        public int WordStartPosition(int position, bool onlyWordCharacters)
        {
            IntPtr onlyWordChars = onlyWordCharacters ? new IntPtr(1) : IntPtr.Zero;
            position = Helpers.Clamp(position, 0, TextLength);
            position = Lines.CharToBytePosition(position);
            position = DirectMessage(NativeMethods.SCI_WORDSTARTPOSITION, new IntPtr(position), onlyWordChars).ToInt32();
            return Lines.ByteToCharPosition(position);
        }

        /// <summary>
        /// Increases the zoom factor by 1 until it reaches 20 points.
        /// </summary>
        /// <seealso cref="Zoom" />
        public void ZoomIn()
        {
            DirectMessage(NativeMethods.SCI_ZOOMIN);
        }

        /// <summary>
        /// Decreases the zoom factor by 1 until it reaches -10 points.
        /// </summary>
        /// <seealso cref="Zoom" />
        public void ZoomOut()
        {
            DirectMessage(NativeMethods.SCI_ZOOMOUT);
        }

        /// <summary>
        /// Sets the representation for a specified character string.
        /// </summary>
        /// <param name="encodedString">The encoded string. I.e. the Ohm character: Ω = \u2126.</param>
        /// <param name="representationString">The representation string for the <paramref name="encodedString"/>. I.e. "OHM".</param>
        /// <remarks>The <see cref="ViewWhitespace"/> must be set to <see cref="WhitespaceMode.VisibleAlways"/> for this to work.</remarks>
        public unsafe void SetRepresentation(string encodedString, string representationString)
        {
            byte[] bytesEncoded = Helpers.GetBytes(encodedString, Encoding, zeroTerminated: true);
            byte[] bytesRepresentation = Helpers.GetBytes(representationString, Encoding, zeroTerminated: true);
            fixed (byte* bpEncoded = bytesEncoded)
            {
                fixed (byte* bpRepresentation = bytesRepresentation)
                {
                    DirectMessage(NativeMethods.SCI_SETREPRESENTATION, new IntPtr(bpEncoded), new IntPtr(bpRepresentation));
                }
            }
        }

        /// <summary>
        /// Sets the representation for a specified character string.
        /// </summary>
        /// <param name="encodedString">The encoded string. I.e. the Ohm character: Ω = \u2126.</param>
        /// <returns>The representation string for the <paramref name="encodedString"/>. I.e. "OHM".</returns>
        public unsafe string GetRepresentation(string encodedString)
        {
            byte[] bytesEncoded = Helpers.GetBytes(encodedString, Encoding, zeroTerminated: true);

            fixed (byte* bpEncoded = bytesEncoded)
            {
                int length = DirectMessage(NativeMethods.SCI_GETREPRESENTATION, new IntPtr(bpEncoded), IntPtr.Zero)
                    .ToInt32();
                byte[] bytesRepresentation = new byte[length + 1];
                fixed (byte* bpRepresentation = bytesRepresentation)
                {
                    DirectMessage(NativeMethods.SCI_GETREPRESENTATION, new IntPtr(bpEncoded), new IntPtr(bpRepresentation));
                    return Helpers.GetString(new IntPtr(bpRepresentation), length, Encoding);
                }
            }
        }

        /// <summary>
        /// Clears the representation from a specified character string.
        /// </summary>
        /// <param name="encodedString">The encoded string. I.e. the Ohm character: Ω = \u2126.</param>
        public unsafe void ClearRepresentation(string encodedString)
        {
            byte[] bytesEncoded = Helpers.GetBytes(encodedString, Encoding, zeroTerminated: true);
            fixed (byte* bpEncoded = bytesEncoded)
            {
                DirectMessage(NativeMethods.SCI_CLEARREPRESENTATION, new IntPtr(bpEncoded), IntPtr.Zero);
            }
        }

        /// <summary>
        /// Clears the change history so that scintilla does not show any saved/modified markers.
        /// Undo buffer is cleared but <see cref="SetSavePoint"/> is not called.
        /// </summary>
        public void ClearChangeHistory()
        {
            EmptyUndoBuffer();
            ChangeHistory ch = ChangeHistory;
            ChangeHistory = ChangeHistory.Disabled;
            ChangeHistory = ch;
        }
        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets whether Scintilla's native drag &amp; drop should be used instead of WinForms based one.
        /// </summary>
        /// <value><c>true</c> if Scintilla's native drag &amp; drop should be used; otherwise, <c>false</c>. The default is false.</value>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Indicates whether Scintilla's native drag && drop should be used instead of WinForms based one.")]
        public bool _ScintillaManagedDragDrop { get; set; }
        // Underscore is used so that WinForms Designer sets it before any other
        // property. Otherwise ApplyResources gets called on the control before
        // the property is set, which then triggers OnHandleCreated before we
        // have the final value.

        /// <summary>
        /// Gets or sets the bi-directionality of the Scintilla control.
        /// </summary>
        /// <value>The bi-directionality of the Scintilla control.</value>
        [DefaultValue(BiDirectionalDisplayType.Disabled)]
        [Category("Behavior")]
        [Description("The bi-directionality of the Scintilla control.")]
        public BiDirectionalDisplayType BiDirectionality
        {
            get => (BiDirectionalDisplayType)DirectMessage(NativeMethods.SCI_GETBIDIRECTIONAL).ToInt32();

            set
            {
                if (value != BiDirectionalDisplayType.Disabled)
                {
                    int technology = DirectMessage(NativeMethods.SCI_GETTECHNOLOGY).ToInt32();
                    if (technology == NativeMethods.SC_TECHNOLOGY_DEFAULT)
                    {
                        DirectMessage(NativeMethods.SCI_SETTECHNOLOGY, new IntPtr(NativeMethods.SC_TECHNOLOGY_DIRECTWRITE));
                    }
                }

                DirectMessage(NativeMethods.SCI_SETBIDIRECTIONAL, new IntPtr((int)value));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the reading layout is from right to left.
        /// </summary>
        /// <value><c>true</c> if reading layout is from right to left; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("A value indicating whether the reading layout is from right to left.")]
        public bool UseRightToLeftReadingLayout
        {
            get
            {
                if (!IsHandleCreated)
                {
                    return false;
                }

                long exStyle = Handle.GetWindowLongPtr(WinApiHelpers.GWL_EXSTYLE).ToInt64();

                return exStyle == (exStyle | WinApiHelpers.WS_EX_LAYOUTRTL);
            }
            set
            {
                if (!IsHandleCreated)
                {
                    return;
                }

                long exStyle = Handle.GetWindowLongPtr(WinApiHelpers.GWL_EXSTYLE).ToInt64();

                if (value)
                {
                    int technology = DirectMessage(NativeMethods.SCI_GETTECHNOLOGY).ToInt32();
                    if (technology != NativeMethods.SC_TECHNOLOGY_DEFAULT)
                    {
                        DirectMessage(NativeMethods.SCI_SETTECHNOLOGY, new IntPtr(NativeMethods.SC_TECHNOLOGY_DEFAULT));
                    }

                    exStyle |= WinApiHelpers.WS_EX_LAYOUTRTL;
                }
                else
                {
                    exStyle &= ~WinApiHelpers.WS_EX_LAYOUTRTL;
                }

                Handle.SetWindowLongPtr(WinApiHelpers.GWL_EXSTYLE, new IntPtr(exStyle));

                // Workaround Scintilla mirrored rendering issue:
                WrapMode wrapMode = WrapMode;
                WrapMode = wrapMode == WrapMode.None ? WrapMode.Word : WrapMode.None;
                WrapMode = wrapMode;
            }
        }

        /// <summary>
        /// Gets or sets the caret foreground color for additional selections.
        /// </summary>
        /// <returns>The caret foreground color in additional selections. The default is (127, 127, 127).</returns>
        [Category("Multiple Selection")]
        [Description("The additional caret foreground color.")]
        public Color AdditionalCaretForeColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_CARET_ADDITIONAL)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_CARET_ADDITIONAL), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets whether the carets in additional selections will blink.
        /// </summary>
        /// <returns>true if additional selection carets should blink; otherwise, false. The default is true.</returns>
        [DefaultValue(true)]
        [Category("Multiple Selection")]
        [Description("Whether the carets in additional selections should blink.")]
        public bool AdditionalCaretsBlink
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETADDITIONALCARETSBLINK) != IntPtr.Zero;
            }
            set
            {
                IntPtr additionalCaretsBlink = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETADDITIONALCARETSBLINK, additionalCaretsBlink);
            }
        }

        /// <summary>
        /// Gets or sets whether the carets in additional selections are visible.
        /// </summary>
        /// <returns>true if additional selection carets are visible; otherwise, false. The default is true.</returns>
        [DefaultValue(true)]
        [Category("Multiple Selection")]
        [Description("Whether the carets in additional selections are visible.")]
        public bool AdditionalCaretsVisible
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETADDITIONALCARETSVISIBLE) != IntPtr.Zero;
            }
            set
            {
                IntPtr additionalCaretsBlink = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETADDITIONALCARETSVISIBLE, additionalCaretsBlink);
            }
        }

        /// <summary>
        /// Gets or sets the alpha transparency of additional multiple selections.
        /// </summary>
        /// <returns>
        /// The alpha transparency ranging from 0 (completely transparent) to 255 (completely opaque).
        /// The value 256 will disable alpha transparency. The default is 256.
        /// </returns>
        [DefaultValue(256)]
        [Category("Multiple Selection")]
        [Description("The transparency of additional selections.")]
        [Obsolete("Use SelectionAdditionalTextColor with alpha channel instead.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int AdditionalSelAlpha
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETADDITIONALSELALPHA).ToInt32();
            }
            set
            {
                value = Helpers.Clamp(value, 0, NativeMethods.SC_ALPHA_NOALPHA);
                DirectMessage(NativeMethods.SCI_SETADDITIONALSELALPHA, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets whether additional typing affects multiple selections.
        /// </summary>
        /// <returns>true if typing will affect multiple selections instead of just the main selection; otherwise, false. The default is false.</returns>
        [DefaultValue(false)]
        [Category("Multiple Selection")]
        [Description("Whether typing, backspace, or delete works with multiple selection simultaneously.")]
        public bool AdditionalSelectionTyping
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETADDITIONALSELECTIONTYPING) != IntPtr.Zero;
            }
            set
            {
                IntPtr additionalSelectionTyping = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETADDITIONALSELECTIONTYPING, additionalSelectionTyping);
            }
        }

        /// <summary>
        /// Gets or sets the current anchor position.
        /// </summary>
        /// <returns>The zero-based character position of the anchor.</returns>
        /// <remarks>
        /// Setting the current anchor position will create a selection between it and the <see cref="CurrentPosition" />.
        /// The caret is not scrolled into view.
        /// </remarks>
        /// <seealso cref="ScrollCaret" />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int AnchorPosition
        {
            get
            {
                int bytePos = DirectMessage(NativeMethods.SCI_GETANCHOR).ToInt32();
                return Lines.ByteToCharPosition(bytePos);
            }
            set
            {
                value = Helpers.Clamp(value, 0, TextLength);
                int bytePos = Lines.CharToBytePosition(value);
                DirectMessage(NativeMethods.SCI_SETANCHOR, new IntPtr(bytePos));
            }
        }

        /// <summary>
        /// Gets or sets the display of annotations.
        /// </summary>
        /// <returns>One of the <see cref="Annotation" /> enumeration values. The default is <see cref="Annotation.Hidden" />.</returns>
        [DefaultValue(Annotation.Hidden)]
        [Category("Appearance")]
        [Description("Display and location of annotations.")]
        public Annotation AnnotationVisible
        {
            get
            {
                return (Annotation)DirectMessage(NativeMethods.SCI_ANNOTATIONGETVISIBLE).ToInt32();
            }
            set
            {
                int visible = (int)value;
                DirectMessage(NativeMethods.SCI_ANNOTATIONSETVISIBLE, new IntPtr(visible));
            }
        }

        /// <summary>
        /// Gets or sets the text color in autocompletion lists.
        /// </summary>
        [Description("The text color in autocompletion lists.")]
        [Category("Autocompletion")]
        [DefaultValue(typeof(Color), "Black")]
        public Color AutocompleteListTextColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_LIST)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_LIST), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the background color in autocompletion lists.
        /// </summary>
        [Description("The background color in autocompletion lists.")]
        [Category("Autocompletion")]
        [DefaultValue(typeof(Color), "White")]
        public Color AutocompleteListBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_LIST_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_LIST_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the text color of selected item in autocompletion lists.
        /// </summary>
        [Description("The text color of selected item in autocompletion lists.")]
        [Category("Autocompletion")]
        [DefaultValue(typeof(Color), "White")]
        public Color AutocompleteListSelectedTextColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_LIST_SELECTED)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_LIST_SELECTED), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the background color of selected item in autocompletion lists.
        /// </summary>
        [Description("The background color of selected item in autocompletion lists.")]
        [Category("Autocompletion")]
        [DefaultValue(typeof(Color), "0, 120, 215")]
        public Color AutocompleteListSelectedBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_LIST_SELECTED_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_LIST_SELECTED_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is an autocompletion list displayed.
        /// </summary>
        /// <returns>true if there is an active autocompletion list; otherwise, false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCActive
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_AUTOCACTIVE) != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets or sets whether to automatically cancel autocompletion when there are no viable matches.
        /// </summary>
        /// <returns>
        /// true to automatically cancel autocompletion when there is no possible match; otherwise, false.
        /// The default is true.
        /// </returns>
        [DefaultValue(true)]
        [Category("Autocompletion")]
        [Description("Whether to automatically cancel autocompletion when no match is possible.")]
        public bool AutoCAutoHide
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_AUTOCGETAUTOHIDE) != IntPtr.Zero;
            }
            set
            {
                IntPtr autoHide = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_AUTOCSETAUTOHIDE, autoHide);
            }
        }

        /// <summary>
        /// Gets or sets whether to cancel an autocompletion if the caret moves from its initial location,
        /// or is allowed to move to the word start.
        /// </summary>
        /// <returns>
        /// true to cancel autocompletion when the caret moves.
        /// false to allow the caret to move to the beginning of the word without cancelling autocompletion.
        /// </returns>
        [DefaultValue(true)]
        [Category("Autocompletion")]
        [Description("Whether to cancel an autocompletion if the caret moves from its initial location, or is allowed to move to the word start.")]
        public bool AutoCCancelAtStart
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_AUTOCGETCANCELATSTART) != IntPtr.Zero;
            }
            set
            {
                IntPtr cancel = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_AUTOCSETCANCELATSTART, cancel);
            }
        }

        /// <summary>
        /// Gets the index of the current autocompletion list selection.
        /// </summary>
        /// <returns>The zero-based index of the current autocompletion selection.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int AutoCCurrent
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_AUTOCGETCURRENT).ToInt32();
            }
        }

        /// <summary>
        /// Gets or sets whether to automatically select an item when it is the only one in an autocompletion list.
        /// </summary>
        /// <returns>
        /// true to automatically choose the only autocompletion item and not display the list; otherwise, false.
        /// The default is false.
        /// </returns>
        [DefaultValue(false)]
        [Category("Autocompletion")]
        [Description("Whether to automatically choose an autocompletion item when it is the only one in the list.")]
        public bool AutoCChooseSingle
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_AUTOCGETCHOOSESINGLE) != IntPtr.Zero;
            }
            set
            {
                IntPtr chooseSingle = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_AUTOCSETCHOOSESINGLE, chooseSingle);
            }
        }

        /// <summary>
        /// Gets or sets whether to delete any word characters following the caret after an autocompletion.
        /// </summary>
        /// <returns>
        /// true to delete any word characters following the caret after autocompletion; otherwise, false.
        /// The default is false.</returns>
        [DefaultValue(false)]
        [Category("Autocompletion")]
        [Description("Whether to delete any existing word characters following the caret after autocompletion.")]
        public bool AutoCDropRestOfWord
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_AUTOCGETDROPRESTOFWORD) != IntPtr.Zero;
            }
            set
            {
                IntPtr dropRestOfWord = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_AUTOCSETDROPRESTOFWORD, dropRestOfWord);
            }
        }

        /// <summary>
        /// Gets or sets whether matching characters to an autocompletion list is case-insensitive.
        /// </summary>
        /// <returns>true to use case-insensitive matching; otherwise, false. The default is false.</returns>
        [DefaultValue(false)]
        [Category("Autocompletion")]
        [Description("Whether autocompletion word matching can ignore case.")]
        public bool AutoCIgnoreCase
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_AUTOCGETIGNORECASE) != IntPtr.Zero;
            }
            set
            {
                IntPtr ignoreCase = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_AUTOCSETIGNORECASE, ignoreCase);
            }
        }

        /// <summary>
        /// Gets or sets the maximum height of the autocompletion list measured in rows.
        /// </summary>
        /// <returns>The max number of rows to display in an autocompletion window. The default is 9.</returns>
        /// <remarks>If there are more items in the list than max rows, a vertical scrollbar is shown.</remarks>
        [DefaultValue(9)]
        [Category("Autocompletion")]
        [Description("The maximum number of rows to display in an autocompletion list.")]
        public int AutoCMaxHeight
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_AUTOCGETMAXHEIGHT).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_AUTOCSETMAXHEIGHT, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the width in characters of the autocompletion list.
        /// </summary>
        /// <returns>
        /// The width of the autocompletion list expressed in characters, or 0 to automatically set the width
        /// to the longest item. The default is 0.
        /// </returns>
        /// <remarks>Any items that cannot be fully displayed will be indicated with ellipsis.</remarks>
        [DefaultValue(0)]
        [Category("Autocompletion")]
        [Description("The width of the autocompletion list measured in characters.")]
        public int AutoCMaxWidth
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_AUTOCGETMAXWIDTH).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_AUTOCSETMAXWIDTH, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the autocompletion list sort order to expect when calling <see cref="AutoCShow" />.
        /// </summary>
        /// <returns>One of the <see cref="Order" /> enumeration values. The default is <see cref="Order.Presorted" />.</returns>
        [DefaultValue(Order.Presorted)]
        [Category("Autocompletion")]
        [Description("The order of words in an autocompletion list.")]
        public Order AutoCOrder
        {
            get
            {
                return (Order)DirectMessage(NativeMethods.SCI_AUTOCGETORDER).ToInt32();
            }
            set
            {
                int order = (int)value;
                DirectMessage(NativeMethods.SCI_AUTOCSETORDER, new IntPtr(order));
            }
        }

        /// <summary>
        /// Gets the document position at the time <see cref="AutoCShow" /> was called.
        /// </summary>
        /// <returns>The zero-based document position at the time <see cref="AutoCShow" /> was called.</returns>
        /// <seealso cref="AutoCShow" />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int AutoCPosStart
        {
            get
            {
                int pos = DirectMessage(NativeMethods.SCI_AUTOCPOSSTART).ToInt32();
                pos = Lines.ByteToCharPosition(pos);

                return pos;
            }
        }

        /// <summary>
        /// Gets or sets the delimiter character used to separate words in an autocompletion list.
        /// </summary>
        /// <returns>The separator character used when calling <see cref="AutoCShow" />. The default is the space character.</returns>
        /// <remarks>The <paramref name="value" /> specified should be limited to printable ASCII characters.</remarks>
        [DefaultValue(' ')]
        [Category("Autocompletion")]
        [Description("The autocompletion list word delimiter. The default is a space character.")]
        public char AutoCSeparator
        {
            get
            {
                int separator = DirectMessage(NativeMethods.SCI_AUTOCGETSEPARATOR).ToInt32();
                return (char)separator;
            }
            set
            {
                // The autocompletion separator character is stored as a byte within Scintilla,
                // not a character. Thus it's possible for a user to supply a character that does
                // not fit within a single byte. The likelyhood of this, however, seems so remote that
                // I'm willing to risk a possible conversion error to provide a better user experience.
                byte separator = (byte)value;
                DirectMessage(NativeMethods.SCI_AUTOCSETSEPARATOR, new IntPtr(separator));
            }
        }

        /// <summary>
        /// Gets or sets the delimiter character used to separate words and image type identifiers in an autocompletion list.
        /// </summary>
        /// <returns>The separator character used to reference an image registered with <see cref="RegisterRgbaImage" />. The default is '?'.</returns>
        /// <remarks>The <paramref name="value" /> specified should be limited to printable ASCII characters.</remarks>
        [DefaultValue('?')]
        [Category("Autocompletion")]
        [Description("The autocompletion list image type delimiter.")]
        public char AutoCTypeSeparator
        {
            get
            {
                int separatorCharacter = DirectMessage(NativeMethods.SCI_AUTOCGETTYPESEPARATOR).ToInt32();
                return (char)separatorCharacter;
            }
            set
            {
                // The autocompletion type separator character is stored as a byte within Scintilla,
                // not a character. Thus it's possible for a user to supply a character that does
                // not fit within a single byte. The likelyhood of this, however, seems so remote that
                // I'm willing to risk a possible conversion error to provide a better user experience.
                byte separatorCharacter = (byte)value;
                DirectMessage(NativeMethods.SCI_AUTOCSETTYPESEPARATOR, new IntPtr(separatorCharacter));
            }
        }

        /// <summary>
        /// Gets or sets the automatic folding flags.
        /// </summary>
        /// <returns>
        /// A bitwise combination of the <see cref="ScintillaNET.AutomaticFold" /> enumeration.
        /// The default is <see cref="ScintillaNET.AutomaticFold.None" />.
        /// </returns>
        [DefaultValue(AutomaticFold.None)]
        [Category("Behavior")]
        [Description("Options for allowing the control to automatically handle folding.")]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(FlagsConverter))]
        public AutomaticFold AutomaticFold
        {
            get
            {
                return (AutomaticFold)DirectMessage(NativeMethods.SCI_GETAUTOMATICFOLD);
            }
            set
            {
                int automaticFold = (int)value;
                DirectMessage(NativeMethods.SCI_SETAUTOMATICFOLD, new IntPtr(automaticFold));
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }

        /// <summary>
        /// Gets or sets whether backspace deletes a character, or unindents.
        /// </summary>
        /// <returns>Whether backspace deletes a character, (false) or unindents (true).</returns>
        [DefaultValue(false)]
        [Category("Indentation")]
        [Description("Determines whether backspace deletes a character, or unindents.")]
        public bool BackspaceUnindents
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETBACKSPACEUNINDENTS) != IntPtr.Zero;
            }
            set
            {
                IntPtr ptr = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETBACKSPACEUNINDENTS, ptr);
            }
        }

        /// <summary>
        /// Gets or sets the border type of the <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>A BorderStyle enumeration value that represents the border type of the control. The default is Fixed3D.</returns>
        /// <exception cref="InvalidEnumArgumentException">A value that is not within the range of valid values for the enumeration was assigned to the property.</exception>
        [DefaultValue(BorderStyle.Fixed3D)]
        [Category("Appearance")]
        [Description("Indicates whether the control should have a border.")]
        public BorderStyle BorderStyle
        {
            get
            {
                return this.borderStyle;
            }
            set
            {
                if (this.borderStyle != value)
                {
                    if (!Enum.IsDefined(typeof(BorderStyle), value))
                        throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));

                    this.borderStyle = value;
                    UpdateStyles();
                    OnBorderStyleChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether drawing is double-buffered.
        /// </summary>
        /// <returns>
        /// true to draw each line into an offscreen bitmap first before copying it to the screen; otherwise, false.
        /// The default is true.
        /// </returns>
        /// <remarks>Disabling buffer can improve performance but will cause flickering.</remarks>
        [DefaultValue(true)]
        [Category("Misc")]
        [Description("Determines whether drawing is double-buffered.")]
        public bool BufferedDraw
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETBUFFEREDDRAW) != IntPtr.Zero;
            }
            set
            {
                IntPtr isBuffered = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETBUFFEREDDRAW, isBuffered);
            }
        }

        /*
        /// <summary>
        /// Gets or sets the current position of a call tip.
        /// </summary>
        /// <returns>The zero-based document position indicated when <see cref="CallTipShow" /> was called to display a call tip.</returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int CallTipPosStart
        {
            get
            {
                var pos = DirectMessage(NativeMethods.SCI_CALLTIPPOSSTART).ToInt32();
                if (pos < 0)
                    return pos;

                return Lines.ByteToCharPosition(pos);
            }
            set
            {
                value = Helpers.Clamp(value, 0, TextLength);
                value = Lines.CharToBytePosition(value);
                DirectMessage(NativeMethods.SCI_CALLTIPSETPOSSTART, new IntPtr(value));
            }
        }
        */

        /// <summary>
        /// Gets a value indicating whether there is a call tip window displayed.
        /// </summary>
        /// <returns>true if there is an active call tip window; otherwise, false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CallTipActive
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_CALLTIPACTIVE) != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is text on the clipboard that can be pasted into the document.
        /// </summary>
        /// <returns>true when there is text on the clipboard to paste; otherwise, false.</returns>
        /// <remarks>The document cannot be <see cref="ReadOnly" />  and the selection cannot contain protected text.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanPaste
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_CANPASTE) != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is an undo action to redo.
        /// </summary>
        /// <returns>true when there is something to redo; otherwise, false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanRedo
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_CANREDO) != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is an action to undo.
        /// </summary>
        /// <returns>true when there is something to undo; otherwise, false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanUndo
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_CANUNDO) != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets or sets the caret foreground color.
        /// </summary>
        /// <returns>The caret foreground color. The default is black.</returns>
        [DefaultValue(typeof(Color), "Black")]
        [Category("Caret")]
        [Description("The caret foreground color.")]
        public Color CaretForeColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_CARET)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_CARET), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the caret line background color.
        /// </summary>
        /// <returns>The caret line background color. The default is yellow.</returns>
        [DefaultValue(typeof(Color), "Transparent")]
        [Category("Caret")]
        [Description("The background color of the current line.")]
        public Color CaretLineBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_CARET_LINE_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_CARET_LINE_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the alpha transparency of the <see cref="CaretLineBackColor" />.
        /// </summary>
        /// <returns>
        /// The alpha transparency ranging from 0 (completely transparent) to 255 (completely opaque).
        /// The value 256 will disable alpha transparency. The default is 256.
        /// </returns>
        [DefaultValue(256)]
        [Category("Caret")]
        [Description("The transparency of the current line background color.")]
        [Obsolete("Use CaretLineBackColor with alpha channel instead.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int CaretLineBackColorAlpha
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETCARETLINEBACKALPHA).ToInt32();
            }
            set
            {
                value = Helpers.Clamp(value, 0, NativeMethods.SC_ALPHA_NOALPHA);
                DirectMessage(NativeMethods.SCI_SETCARETLINEBACKALPHA, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the width of the caret line frame.
        /// </summary>
        /// <returns><see cref="CaretLineVisible" /> must be set to true. A value of 0 disables the frame. The default is 0.</returns>
        [DefaultValue(0)]
        [Category("Caret")]
        [Description("The Width of the current line frame.")]
        public int CaretLineFrame
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETCARETLINEFRAME).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETCARETLINEFRAME, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets whether the caret line is visible (highlighted).
        /// </summary>
        /// <returns>true if the caret line is visible; otherwise, false. The default is true.</returns>
        [DefaultValue(true)]
        [Category("Caret")]
        [Description("Determines whether to highlight the current caret line.")]
        [Obsolete("Use CaretLineBackColor with alpha channel instead.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public bool CaretLineVisible
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETCARETLINEVISIBLE) != IntPtr.Zero;
            }
            set
            {
                IntPtr visible = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETCARETLINEVISIBLE, visible);
            }
        }

        /// <summary>
        /// Gets or sets whether the caret line is always visible even when the window is not in focus.
        /// </summary>
        /// <returns>true if the caret line is always visible; otherwise, false. The default is false.</returns>
        [DefaultValue(false)]
        [Category("Caret")]
        [Description("Determines whether the caret line always visible even when the window is not in focus..")]
        public bool CaretLineVisibleAlways
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETCARETLINEVISIBLEALWAYS) != IntPtr.Zero;
            }
            set
            {
                IntPtr visibleAlways = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETCARETLINEVISIBLEALWAYS, visibleAlways);
            }
        }

        /// <summary>
        /// Gets or sets the layer where the line caret will be painted. Default value is <see cref="Layer.Base"/>
        /// </summary>
        [DefaultValue(Layer.Base)]
        [Category("Caret")]
        [Description("The layer where the line caret will be painted.")]
        public Layer CaretLineLayer
        {
            get
            {
                return (Layer)DirectMessage(NativeMethods.SCI_GETCARETLINELAYER).ToInt32();
            }
            set
            {
                int layer = (int)value;
                DirectMessage(NativeMethods.SCI_SETCARETLINELAYER, new IntPtr(layer));
            }
        }

        /// <summary>
        /// Gets or sets the caret blink rate in milliseconds.
        /// </summary>
        /// <returns>The caret blink rate measured in milliseconds. The default is 530.</returns>
        /// <remarks>A value of 0 will stop the caret blinking.</remarks>
        [DefaultValue(530)]
        [Category("Caret")]
        [Description("The caret blink rate in milliseconds.")]
        public int CaretPeriod
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETCARETPERIOD).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETCARETPERIOD, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the caret display style.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ScintillaNET.CaretStyle" /> enumeration values.
        /// The default is <see cref="ScintillaNET.CaretStyle.Line" />.
        /// </returns>
        [DefaultValue(CaretStyle.Line)]
        [Category("Caret")]
        [Description("The caret display style.")]
        public CaretStyle CaretStyle
        {
            get
            {
                return (CaretStyle)DirectMessage(NativeMethods.SCI_GETCARETSTYLE).ToInt32();
            }
            set
            {
                int style = (int)value;
                DirectMessage(NativeMethods.SCI_SETCARETSTYLE, new IntPtr(style));
            }
        }

        /// <summary>
        /// Gets or sets the width in pixels of the caret.
        /// </summary>
        /// <returns>The width of the caret in pixels. The default is 1 pixel.</returns>
        /// <remarks>
        /// The caret width can only be set to a value of 0, 1, 2 or 3 pixels and is only effective
        /// when the <see cref="CaretStyle" /> property is set to <see cref="ScintillaNET.CaretStyle.Line" />.
        /// </remarks>
        [DefaultValue(1)]
        [Category("Caret")]
        [Description("The width of the caret line measured in pixels (between 0 and 3).")]
        public int CaretWidth
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETCARETWIDTH).ToInt32();
            }
            set
            {
                value = Helpers.Clamp(value, 0, 3);
                DirectMessage(NativeMethods.SCI_SETCARETWIDTH, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets whether Scintilla should keep track of document change history and in which ways it should display the difference.
        /// </summary>
        [DefaultValue(ChangeHistory.Disabled)]
        [Category("Change History")]
        [Description("Controls whether Scintilla should keep track of document change history and in which ways it should display the difference.")]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(FlagsConverter))]
        public ChangeHistory ChangeHistory
        {
            get
            {
                return (ChangeHistory)DirectMessage(NativeMethods.SCI_GETCHANGEHISTORY).ToInt32();
            }
            set
            {
                DirectMessage(NativeMethods.SCI_SETCHANGEHISTORY, new IntPtr((int)value));
            }
        }

        /// <summary>
        /// Gets the required creation parameters when the control handle is created.
        /// </summary>
        /// <returns>A CreateParams that contains the required creation parameters when the handle to the control is created.</returns>
        protected override CreateParams CreateParams
        {
            get
            {
                if (moduleHandle == IntPtr.Zero)
                {
                    // Load the native Scintilla library
                    moduleHandle = NativeMethods.LoadLibrary(modulePathScintilla);
                    lexillaHandle = NativeMethods.LoadLibrary(modulePathLexilla);

                    if (moduleHandle == IntPtr.Zero)
                    {
                        string message = string.Format(CultureInfo.InvariantCulture, "Could not load the Scintilla module at the path '{0}'.", modulePathScintilla);
                        throw new Win32Exception(message, new Win32Exception()); // Calls GetLastError
                    }

                    // For some reason the 32-bit DLL has weird export names.
                    bool is32Bit = IntPtr.Size == 4;

                    // Self-compiled DLLs required this:
                    //var exportName = is32Bit
                    //    ? "_Scintilla_DirectFunction@16"
                    //    : nameof(NativeMethods.Scintilla_DirectFunction);

                    // Native DLL:
                    string exportName = nameof(NativeMethods.Scintilla_DirectFunction);

                    // Get the native Scintilla direct function -- the only function the library exports
                    IntPtr directFunctionPointer = NativeMethods.GetProcAddress(new HandleRef(this, moduleHandle), exportName);
                    if (directFunctionPointer == IntPtr.Zero)
                    {
                        string message = "The Scintilla module has no export for the 'Scintilla_DirectFunction' procedure.";
                        throw new Win32Exception(message, new Win32Exception()); // Calls GetLastError
                    }

                    // Get the native Lexilla.dll methods
                    lexilla = new Lexilla(lexillaHandle);

                    // Create a managed callback
                    directFunction = (NativeMethods.Scintilla_DirectFunction)Marshal.GetDelegateForFunctionPointer(
                        directFunctionPointer,
                        typeof(NativeMethods.Scintilla_DirectFunction));
                }

                CreateParams cp = base.CreateParams;
                cp.ClassName = "Scintilla";

                // The border effect is achieved through a native Windows style
                cp.ExStyle &= ~NativeMethods.WS_EX_CLIENTEDGE;
                cp.Style &= ~NativeMethods.WS_BORDER;
                switch (this.borderStyle)
                {
                    case BorderStyle.Fixed3D:
                    case BorderStyle.Fixed3DVisualStyles:
                        cp.ExStyle |= NativeMethods.WS_EX_CLIENTEDGE;
                        break;
                    case BorderStyle.FixedSingle:
                        cp.Style |= NativeMethods.WS_BORDER;
                        break;
                }

                return cp;
            }
        }

        /// <summary>
        /// Gets the current line index.
        /// </summary>
        /// <returns>The zero-based line index containing the <see cref="CurrentPosition" />.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentLine
        {
            get
            {
                int currentPos = DirectMessage(NativeMethods.SCI_GETCURRENTPOS).ToInt32();
                int line = DirectMessage(NativeMethods.SCI_LINEFROMPOSITION, new IntPtr(currentPos)).ToInt32();
                return line;
            }
        }

        /// <summary>
        /// Gets or sets the current caret position.
        /// </summary>
        /// <returns>The zero-based character position of the caret.</returns>
        /// <remarks>
        /// Setting the current caret position will create a selection between it and the current <see cref="AnchorPosition" />.
        /// The caret is not scrolled into view.
        /// </remarks>
        /// <seealso cref="ScrollCaret" />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentPosition
        {
            get
            {
                int bytePos = DirectMessage(NativeMethods.SCI_GETCURRENTPOS).ToInt32();
                return Lines.ByteToCharPosition(bytePos);
            }
            set
            {
                value = Helpers.Clamp(value, 0, TextLength);
                int bytePos = Lines.CharToBytePosition(value);
                DirectMessage(NativeMethods.SCI_SETCURRENTPOS, new IntPtr(bytePos));
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                base.Cursor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default cursor for the control.
        /// </summary>
        /// <returns>An object of type Cursor representing the current default cursor.</returns>
        protected override Cursor DefaultCursor
        {
            get
            {
                return Cursors.IBeam;
            }
        }

        /// <summary>
        /// Gets the default size of the control.
        /// </summary>
        /// <returns>The default Size of the control.</returns>
        protected override Size DefaultSize
        {
            get
            {
                // I've discovered that using a DefaultSize property other than 'empty' triggers a flaw (IMO)
                // in Windows Forms that will cause CreateParams to be called in the base constructor.
                // That's too early. It makes it impossible to use the Site or DesignMode properties during
                // handle creation because they haven't been set yet. Since we don't currently depend on those
                // properties it's okay, but if we need them this is the place to start fixing things.

                return new Size(200, 100);
            }
        }

        /// <summary>
        /// Gets a value indicating the start index of the secondary styles.
        /// </summary>
        /// <returns>Returns the distance between a primary style and its corresponding secondary style.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int DistanceToSecondaryStyles
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_DISTANCETOSECONDARYSTYLES).ToInt32();
            }
        }

        /// <summary>
        /// Gets or sets the current document used by the control.
        /// </summary>
        /// <returns>The current <see cref="Document" />.</returns>
        /// <remarks>
        /// Setting this property is equivalent to calling <see cref="ReleaseDocument" /> on the current document, and
        /// calling <see cref="CreateDocument" /> if the new <paramref name="value" /> is <see cref="ScintillaNET.Document.Empty" /> or
        /// <see cref="AddRefDocument" /> if the new <paramref name="value" /> is not <see cref="ScintillaNET.Document.Empty" />.
        /// </remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Document Document
        {
            get
            {
                IntPtr ptr = DirectMessage(NativeMethods.SCI_GETDOCPOINTER);
                return new Document { Value = ptr };
            }
            set
            {
                Eol eolMode = EolMode;
                bool useTabs = UseTabs;
                int tabWidth = TabWidth;
                int indentWidth = IndentWidth;

                IntPtr ptr = value.Value;
                DirectMessage(NativeMethods.SCI_SETDOCPOINTER, IntPtr.Zero, ptr);

                // Carry over properties to new document
                InitDocument(eolMode, useTabs, tabWidth, indentWidth);

                // Rebuild the line cache
                Lines.RebuildLineData();
            }
        }

        /// <summary>
        /// Gets or sets the background color to use when indicating long lines with
        /// <see cref="ScintillaNET.EdgeMode.Background" />.
        /// </summary>
        /// <returns>The background Color.</returns>
        [DefaultValue(typeof(Color), "Silver")]
        [Category("Long Lines")]
        [Description("The background color to use when indicating long lines.")]
        public Color EdgeColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETEDGECOLOUR).ToInt32();
                return HelperMethods.FromWin32ColorOpaque(color);
            }
            set
            {
                int color = HelperMethods.ToWin32ColorOpaque(value);
                DirectMessage(NativeMethods.SCI_SETEDGECOLOUR, new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the column number at which to begin indicating long lines.
        /// </summary>
        /// <returns>The number of columns in a long line. The default is 0.</returns>
        /// <remarks>
        /// When using <see cref="ScintillaNET.EdgeMode.Line"/>, a column is defined as the width of a space character in the <see cref="Style.Default" /> style.
        /// When using <see cref="ScintillaNET.EdgeMode.Background" /> a column is equal to a character (including tabs).
        /// </remarks>
        [DefaultValue(0)]
        [Category("Long Lines")]
        [Description("The number of columns at which to display long line indicators.")]
        public int EdgeColumn
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETEDGECOLUMN).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETEDGECOLUMN, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the mode for indicating long lines.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ScintillaNET.EdgeMode" /> enumeration values.
        /// The default is <see cref="ScintillaNET.EdgeMode.None" />.
        /// </returns>
        [DefaultValue(EdgeMode.None)]
        [Category("Long Lines")]
        [Description("Determines how long lines are indicated.")]
        public EdgeMode EdgeMode
        {
            get
            {
                return (EdgeMode)DirectMessage(NativeMethods.SCI_GETEDGEMODE);
            }
            set
            {
                int edgeMode = (int)value;
                DirectMessage(NativeMethods.SCI_SETEDGEMODE, new IntPtr(edgeMode));
            }
        }

        internal Encoding Encoding
        {
            get
            {
                // Should always be UTF-8 unless someone has done an end run around us
                int codePage = (int)DirectMessage(NativeMethods.SCI_GETCODEPAGE);
                return codePage == 0 ? Encoding.Default : Encoding.GetEncoding(codePage);
            }
        }

        /// <summary>
        /// Gets or sets whether vertical scrolling ends at the last line or can scroll past.
        /// </summary>
        /// <returns>true if the maximum vertical scroll position ends at the last line; otherwise, false. The default is true.</returns>
        [DefaultValue(true)]
        [Category("Scrolling")]
        [Description("Determines whether the maximum vertical scroll position ends at the last line or can scroll past.")]
        public bool EndAtLastLine
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETENDATLASTLINE) != IntPtr.Zero;
            }
            set
            {
                IntPtr endAtLastLine = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETENDATLASTLINE, endAtLastLine);
            }
        }

        /// <summary>
        /// Gets or sets the end-of-line mode, or rather, the characters added into
        /// the document when the user presses the Enter key.
        /// </summary>
        /// <returns>One of the <see cref="Eol" /> enumeration values. The default is <see cref="Eol.CrLf" />.</returns>
        [DefaultValue(Eol.CrLf)]
        [Category("Line Endings")]
        [Description("Determines the characters added into the document when the user presses the Enter key.")]
        public Eol EolMode
        {
            get
            {
                return (Eol)DirectMessage(NativeMethods.SCI_GETEOLMODE);
            }
            set
            {
                int eolMode = (int)value;
                DirectMessage(NativeMethods.SCI_SETEOLMODE, new IntPtr(eolMode));
            }
        }

        /// <summary>
        /// Gets or sets the amount of whitespace added to the ascent (top) of each line.
        /// </summary>
        /// <returns>The extra line ascent. The default is zero.</returns>
        [DefaultValue(0)]
        [Category("Whitespace")]
        [Description("Extra whitespace added to the ascent (top) of each line.")]
        public int ExtraAscent
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETEXTRAASCENT).ToInt32();
            }
            set
            {
                DirectMessage(NativeMethods.SCI_SETEXTRAASCENT, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the amount of whitespace added to the descent (bottom) of each line.
        /// </summary>
        /// <returns>The extra line descent. The default is zero.</returns>
        [DefaultValue(0)]
        [Category("Whitespace")]
        [Description("Extra whitespace added to the descent (bottom) of each line.")]
        public int ExtraDescent
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETEXTRADESCENT).ToInt32();
            }
            set
            {
                DirectMessage(NativeMethods.SCI_SETEXTRADESCENT, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the first visible line on screen.
        /// </summary>
        /// <returns>The zero-based index of the first visible screen line.</returns>
        /// <remarks>The value is a visible line, not a document line.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FirstVisibleLine
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETFIRSTVISIBLELINE).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETFIRSTVISIBLELINE, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
        [Category("Appearance")]
        [Description("The font of the text displayed by the control.")]
        public override Font Font
        {
            get
            {
                if (!IsHandleCreated)
                {
                    return base.Font;
                }

                Style defaultFontStyle = Styles[Style.Default];

                FontStyle fontStyle = defaultFontStyle.Bold ? FontStyle.Bold : FontStyle.Regular;

                if (defaultFontStyle.Italic)
                {
                    fontStyle |= FontStyle.Italic;
                }

                if (defaultFontStyle.Underline)
                {
                    fontStyle |= FontStyle.Underline;
                }

                return new Font(defaultFontStyle.Font, defaultFontStyle.SizeF, fontStyle);
            }

            set
            {
                Style defaultFontStyle = Styles[Style.Default];
                value ??= Parent?.Font ?? Control.DefaultFont;
                defaultFontStyle.Font = value.Name;
                defaultFontStyle.SizeF = value.Size;
                defaultFontStyle.Bold = value.Bold;
                defaultFontStyle.Italic = value.Italic;
                defaultFontStyle.Underline = value.Underline;
                base.Font = value;
            }
        }

        /// <summary>
        /// Gets or sets font quality (anti-aliasing method) used to render fonts.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ScintillaNET.FontQuality" /> enumeration values.
        /// The default is <see cref="ScintillaNET.FontQuality.Default" />.
        /// </returns>
        [DefaultValue(FontQuality.Default)]
        [Category("Misc")]
        [Description("Specifies the anti-aliasing method to use when rendering fonts.")]
        public FontQuality FontQuality
        {
            get
            {
                return (FontQuality)DirectMessage(NativeMethods.SCI_GETFONTQUALITY);
            }
            set
            {
                int fontQuality = (int)value;
                DirectMessage(NativeMethods.SCI_SETFONTQUALITY, new IntPtr(fontQuality));
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the column number of the indentation guide to highlight.
        /// </summary>
        /// <returns>The column number of the indentation guide to highlight or 0 if disabled.</returns>
        /// <remarks>Guides are highlighted in the <see cref="Style.BraceLight" /> style. Column numbers can be determined by calling <see cref="GetColumn" />.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int HighlightGuide
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETHIGHLIGHTGUIDE).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETHIGHLIGHTGUIDE, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets whether to display the horizontal scroll bar.
        /// </summary>
        /// <returns>true to display the horizontal scroll bar when needed; otherwise, false. The default is true.</returns>
        [DefaultValue(true)]
        [Category("Scrolling")]
        [Description("Determines whether to show the horizontal scroll bar if needed.")]
        public bool HScrollBar
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETHSCROLLBAR) != IntPtr.Zero;
            }
            set
            {
                IntPtr visible = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETHSCROLLBAR, visible);
            }
        }

        /// <summary>
        /// Gets or sets the strategy used to perform styling using application idle time.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ScintillaNET.IdleStyling" /> enumeration values.
        /// The default is <see cref="ScintillaNET.IdleStyling.None" />.
        /// </returns>
        [DefaultValue(IdleStyling.None)]
        [Category("Misc")]
        [Description("Specifies how to use application idle time for styling.")]
        public IdleStyling IdleStyling
        {
            get
            {
                return (IdleStyling)DirectMessage(NativeMethods.SCI_GETIDLESTYLING);
            }
            set
            {
                int idleStyling = (int)value;
                DirectMessage(NativeMethods.SCI_SETIDLESTYLING, new IntPtr(idleStyling));
            }
        }

        /// <summary>
        /// Gets or sets the size of indentation in terms of space characters.
        /// </summary>
        /// <returns>The indentation size measured in characters. The default is 0.</returns>
        /// <remarks> A value of 0 will make the indent width the same as the tab width.</remarks>
        [DefaultValue(0)]
        [Category("Indentation")]
        [Description("The indentation size in characters or 0 to make it the same as the tab width.")]
        public int IndentWidth
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETINDENT).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETINDENT, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets whether to display indentation guides.
        /// </summary>
        /// <returns>One of the <see cref="IndentView" /> enumeration values. The default is <see cref="IndentView.None" />.</returns>
        /// <remarks>The <see cref="Style.IndentGuide" /> style can be used to specify the foreground and background color of indentation guides.</remarks>
        [DefaultValue(IndentView.None)]
        [Category("Indentation")]
        [Description("Indicates whether indentation guides are displayed.")]
        public IndentView IndentationGuides
        {
            get
            {
                return (IndentView)DirectMessage(NativeMethods.SCI_GETINDENTATIONGUIDES);
            }
            set
            {
                int indentView = (int)value;
                DirectMessage(NativeMethods.SCI_SETINDENTATIONGUIDES, new IntPtr(indentView));
            }
        }

        /// <summary>
        /// Gets or sets the indicator used in a subsequent call to <see cref="IndicatorFillRange" /> or <see cref="IndicatorClearRange" />.
        /// </summary>
        /// <returns>The zero-based indicator index to apply when calling <see cref="IndicatorFillRange" /> or remove when calling <see cref="IndicatorClearRange" />.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int IndicatorCurrent
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETINDICATORCURRENT).ToInt32();
            }
            set
            {
                value = Helpers.Clamp(value, 0, Indicators.Count - 1);
                DirectMessage(NativeMethods.SCI_SETINDICATORCURRENT, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets a collection of objects for working with indicators.
        /// </summary>
        /// <returns>A collection of <see cref="Indicator" /> objects.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IndicatorCollection Indicators { get; private set; }

        /// <summary>
        /// Gets or sets the user-defined value used in a subsequent call to <see cref="IndicatorFillRange" />.
        /// </summary>
        /// <returns>The indicator value to apply when calling <see cref="IndicatorFillRange" />.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int IndicatorValue
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETINDICATORVALUE).ToInt32();
            }
            set
            {
                DirectMessage(NativeMethods.SCI_SETINDICATORVALUE, new IntPtr(value));
            }
        }

        /// <summary>
        /// This is used by clients that have complex focus requirements such as having their own window
        /// that gets the real focus but with the need to indicate that Scintilla has the logical focus.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool InternalFocusFlag
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETFOCUS) != IntPtr.Zero;
            }
            set
            {
                IntPtr focus = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETFOCUS, focus);
            }
        }

        private string lexerName;

        /// <summary>
        /// Gets or sets the name of the lexer.
        /// </summary>
        /// <value>The name of the lexer.</value>
        /// <exception cref="InvalidOperationException">Lexer with the name of 'Value' was not found.</exception>
        [Category("Lexing")]
        public string LexerName
        {
            get => this.lexerName;

            set
            {
                if (string.IsNullOrWhiteSpace(value) && value != string.Empty)
                {
                    this.lexerName = value;

                    return;
                }

                if (!SetLexerByName(value))
                {
                    throw new InvalidOperationException(@$"Lexer with the name of '{value}' was not found.");
                }

                this.lexerName = value;
            }
        }

        /// <summary>
        /// Gets or sets the current lexer by name.
        /// </summary>
        /// <returns>A String representing the current lexer.</returns>
        /// <remarks>Lexer names are case-sensitive.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public unsafe string LexerLanguage
        {
            get
            {
                int length = DirectMessage(NativeMethods.SCI_GETLEXERLANGUAGE).ToInt32();
                if (length == 0)
                    return string.Empty;

                byte[] bytes = new byte[length + 1];
                fixed (byte* bp = bytes)
                {
                    DirectMessage(NativeMethods.SCI_GETLEXERLANGUAGE, IntPtr.Zero, new IntPtr(bp));
                    return Helpers.GetString(new IntPtr(bp), length, Encoding.ASCII);
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    DirectMessage(NativeMethods.SCI_SETLEXERLANGUAGE, IntPtr.Zero, IntPtr.Zero);
                }
                else
                {
                    byte[] bytes = Helpers.GetBytes(value, Encoding.ASCII, zeroTerminated: true);
                    fixed (byte* bp = bytes)
                        DirectMessage(NativeMethods.SCI_SETLEXERLANGUAGE, IntPtr.Zero, new IntPtr(bp));
                }
            }
        }

        /// <summary>
        /// Gets the combined result of the <see cref="LineEndTypesSupported" /> and <see cref="LineEndTypesAllowed" />
        /// properties to report the line end types actively being interpreted.
        /// </summary>
        /// <returns>A bitwise combination of the <see cref="LineEndType" /> enumeration.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LineEndType LineEndTypesActive
        {
            get
            {
                return (LineEndType)DirectMessage(NativeMethods.SCI_GETLINEENDTYPESACTIVE);
            }
        }

        /// <summary>
        /// Gets or sets the line ending types interpreted by the <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>
        /// A bitwise combination of the <see cref="LineEndType" /> enumeration.
        /// The default is <see cref="LineEndType.Default" />.
        /// </returns>
        /// <remarks>The line ending types allowed must also be supported by the current lexer to be effective.</remarks>
        [DefaultValue(LineEndType.Default)]
        [Category("Line Endings")]
        [Description("Line endings types interpreted by the control.")]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(FlagsConverter))]
        public LineEndType LineEndTypesAllowed
        {
            get
            {
                return (LineEndType)DirectMessage(NativeMethods.SCI_GETLINEENDTYPESALLOWED);
            }
            set
            {
                int lineEndBitsSet = (int)value;
                DirectMessage(NativeMethods.SCI_SETLINEENDTYPESALLOWED, new IntPtr(lineEndBitsSet));
            }
        }

        /// <summary>
        /// Gets the different types of line ends supported by the current lexer.
        /// </summary>
        /// <returns>A bitwise combination of the <see cref="LineEndType" /> enumeration.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LineEndType LineEndTypesSupported
        {
            get
            {
                return (LineEndType)DirectMessage(NativeMethods.SCI_GETLINEENDTYPESSUPPORTED);
            }
        }

        /// <summary>
        /// Gets a collection representing lines of text in the <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>A collection of text lines.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LineCollection Lines { get; private set; }

        /// <summary>
        /// Gets the number of lines that can be shown on screen given a constant
        /// line height and the space available.
        /// </summary>
        /// <returns>
        /// The number of screen lines which could be displayed (including any partial lines).
        /// </returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int LinesOnScreen
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_LINESONSCREEN).ToInt32();
            }
        }

        /// <summary>
        /// Gets or sets the main selection when their are multiple selections.
        /// </summary>
        /// <returns>The zero-based main selection index.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MainSelection
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETMAINSELECTION).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETMAINSELECTION, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets a collection representing margins in a <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>A collection of margins.</returns>
        [Category("Collections")]
        [Description("The margins collection.")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MarginCollection Margins { get; private set; }

        /// <summary>
        /// Gets a collection representing markers in a <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>A collection of markers.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MarkerCollection Markers { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the document has been modified (is dirty)
        /// since the last call to <see cref="SetSavePoint" />.
        /// </summary>
        /// <returns>true if the document has been modified; otherwise, false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Modified
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETMODIFY) != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets or sets the time in milliseconds the mouse must linger to generate a <see cref="DwellStart" /> event.
        /// </summary>
        /// <returns>
        /// The time in milliseconds the mouse must linger to generate a <see cref="DwellStart" /> event
        /// or <see cref="Scintilla.TimeForever" /> if dwell events are disabled.
        /// </returns>
        [DefaultValue(TimeForever)]
        [Category("Behavior")]
        [Description("The time in milliseconds the mouse must linger to generate a dwell start event. A value of 10000000 disables dwell events.")]
        public int MouseDwellTime
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETMOUSEDWELLTIME).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETMOUSEDWELLTIME, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the ability to switch to rectangular selection mode while making a selection with the mouse.
        /// </summary>
        /// <returns>
        /// true if the current mouse selection can be switched to a rectangular selection by pressing the ALT key; otherwise, false.
        /// The default is false.
        /// </returns>
        [DefaultValue(false)]
        [Category("Multiple Selection")]
        [Description("Enable or disable the ability to switch to rectangular selection mode while making a selection with the mouse.")]
        public bool MouseSelectionRectangularSwitch
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETMOUSESELECTIONRECTANGULARSWITCH) != IntPtr.Zero;
            }
            set
            {
                IntPtr mouseSelectionRectangularSwitch = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETMOUSESELECTIONRECTANGULARSWITCH, mouseSelectionRectangularSwitch);
            }
        }

        // The MouseWheelCaptures property doesn't seem to work correctly in Windows Forms so hiding for now...
        // P.S. I'm avoiding the MouseDownCaptures property (SCI_SETMOUSEDOWNCAPTURES & SCI_GETMOUSEDOWNCAPTURES) for the same reason... I don't expect it to work in Windows Forms.

        /* 
        /// <summary>
        /// Gets or sets whether to respond to mouse wheel messages if the control has focus but the mouse is not currently over the control.
        /// </summary>
        /// <returns>
        /// true to respond to mouse wheel messages even when the mouse is not currently over the control; otherwise, false.
        /// The default is true.
        /// </returns>
        /// <remarks>Scintilla will still react to the mouse wheel if the mouse pointer is over the editor window.</remarks>
        [DefaultValue(true)]
        [Category("Mouse")]
        [Description("Enable or disable mouse wheel support when the mouse is outside the control bounds, but the control still has focus.")]
        public bool MouseWheelCaptures
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETMOUSEWHEELCAPTURES) != IntPtr.Zero;
            }
            set
            {
                var mouseWheelCaptures = (value ? new IntPtr(1) : IntPtr.Zero);
                DirectMessage(NativeMethods.SCI_SETMOUSEWHEELCAPTURES, mouseWheelCaptures);
            }
        }
        */

        /// <summary>
        /// Gets or sets whether multiple selection is enabled.
        /// </summary>
        /// <returns>
        /// true if multiple selections can be made by holding the CTRL key and dragging the mouse; otherwise, false.
        /// The default is false.
        /// </returns>
        [DefaultValue(false)]
        [Category("Multiple Selection")]
        [Description("Enable or disable multiple selection with the CTRL key.")]
        public bool MultipleSelection
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETMULTIPLESELECTION) != IntPtr.Zero;
            }
            set
            {
                IntPtr multipleSelection = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETMULTIPLESELECTION, multipleSelection);
            }
        }

        /// <summary>
        /// Gets or sets the behavior when pasting text into multiple selections.
        /// </summary>
        /// <returns>One of the <see cref="ScintillaNET.MultiPaste" /> enumeration values. The default is <see cref="ScintillaNET.MultiPaste.Once" />.</returns>
        [DefaultValue(MultiPaste.Once)]
        [Category("Multiple Selection")]
        [Description("Determines how pasted text is applied to multiple selections.")]
        public MultiPaste MultiPaste
        {
            get
            {
                return (MultiPaste)DirectMessage(NativeMethods.SCI_GETMULTIPASTE);
            }
            set
            {
                int multiPaste = (int)value;
                DirectMessage(NativeMethods.SCI_SETMULTIPASTE, new IntPtr(multiPaste));
            }
        }

        /// <summary>
        /// Gets or sets whether to write over text rather than insert it.
        /// </summary>
        /// <return>true to write over text; otherwise, false. The default is false.</return>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Puts the caret into overtype mode.")]
        public bool Overtype
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETOVERTYPE) != IntPtr.Zero;
            }
            set
            {
                IntPtr overtype = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETOVERTYPE, overtype);
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                base.Padding = value;
            }
        }

        /// <summary>
        /// Gets or sets whether line endings in pasted text are convereted to the document <see cref="EolMode" />.
        /// </summary>
        /// <returns>true to convert line endings in pasted text; otherwise, false. The default is true.</returns>
        [DefaultValue(true)]
        [Category("Line Endings")]
        [Description("Whether line endings in pasted text are converted to match the document end-of-line mode.")]
        public bool PasteConvertEndings
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETPASTECONVERTENDINGS) != IntPtr.Zero;
            }
            set
            {
                IntPtr convert = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETPASTECONVERTENDINGS, convert);
            }
        }

        /// <summary>
        /// Gets or sets the number of phases used when drawing.
        /// </summary>
        /// <returns>One of the <see cref="Phases" /> enumeration values. The default is <see cref="Phases.Two" />.</returns>
        [DefaultValue(Phases.Two)]
        [Category("Misc")]
        [Description("Adjusts the number of phases used when drawing.")]
        public Phases PhasesDraw
        {
            get
            {
                return (Phases)DirectMessage(NativeMethods.SCI_GETPHASESDRAW);
            }
            set
            {
                int phases = (int)value;
                DirectMessage(NativeMethods.SCI_SETPHASESDRAW, new IntPtr(phases));
            }
        }

        /// <summary>
        /// Gets or sets whether the document is read-only.
        /// </summary>
        /// <returns>true if the document is read-only; otherwise, false. The default is false.</returns>
        /// <seealso cref="ModifyAttempt" />
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Controls whether the document text can be modified.")]
        public bool ReadOnly
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETREADONLY) != IntPtr.Zero;
            }
            set
            {
                IntPtr readOnly = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETREADONLY, readOnly);
            }
        }

        /// <summary>
        /// Gets or sets the anchor position of the rectangular selection.
        /// </summary>
        /// <returns>The zero-based document position of the rectangular selection anchor.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RectangularSelectionAnchor
        {
            get
            {
                int pos = DirectMessage(NativeMethods.SCI_GETRECTANGULARSELECTIONANCHOR).ToInt32();
                if (pos <= 0)
                    return pos;

                return Lines.ByteToCharPosition(pos);
            }
            set
            {
                value = Helpers.Clamp(value, 0, TextLength);
                value = Lines.CharToBytePosition(value);
                DirectMessage(NativeMethods.SCI_SETRECTANGULARSELECTIONANCHOR, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the amount of anchor virtual space in a rectangular selection.
        /// </summary>
        /// <returns>The amount of virtual space past the end of the line offsetting the rectangular selection anchor.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RectangularSelectionAnchorVirtualSpace
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETRECTANGULARSELECTIONANCHORVIRTUALSPACE).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETRECTANGULARSELECTIONANCHORVIRTUALSPACE, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the caret position of the rectangular selection.
        /// </summary>
        /// <returns>The zero-based document position of the rectangular selection caret.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RectangularSelectionCaret
        {
            get
            {
                int pos = DirectMessage(NativeMethods.SCI_GETRECTANGULARSELECTIONCARET).ToInt32();
                if (pos <= 0)
                    return 0;

                return Lines.ByteToCharPosition(pos);
            }
            set
            {
                value = Helpers.Clamp(value, 0, TextLength);
                value = Lines.CharToBytePosition(value);
                DirectMessage(NativeMethods.SCI_SETRECTANGULARSELECTIONCARET, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the amount of caret virtual space in a rectangular selection.
        /// </summary>
        /// <returns>The amount of virtual space past the end of the line offsetting the rectangular selection caret.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RectangularSelectionCaretVirtualSpace
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETRECTANGULARSELECTIONCARETVIRTUALSPACE).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETRECTANGULARSELECTIONCARETVIRTUALSPACE, new IntPtr(value));
            }
        }

        private IntPtr SciPointer
        {
            get
            {
                // Enforce illegal cross-thread calls the way the Handle property does
                if (Control.CheckForIllegalCrossThreadCalls && InvokeRequired)
                {
                    string message = string.Format(CultureInfo.InvariantCulture, "Control '{0}' accessed from a thread other than the thread it was created on.", Name);
                    throw new InvalidOperationException(message);
                }

                if (this.sciPtr == IntPtr.Zero)
                {
                    // Get a pointer to the native Scintilla object (i.e. C++ 'this') to use with the
                    // direct function. This will happen for each Scintilla control instance.
                    this.sciPtr = NativeMethods.SendMessage(new HandleRef(this, Handle), NativeMethods.SCI_GETDIRECTPOINTER, IntPtr.Zero, IntPtr.Zero);
                }

                return this.sciPtr;
            }
        }

        /// <summary>
        /// Gets or sets the range of the horizontal scroll bar.
        /// </summary>
        /// <returns>The range in pixels of the horizontal scroll bar.</returns>
        /// <remarks>The width will automatically increase as needed when <see cref="ScrollWidthTracking" /> is enabled.</remarks>
        [DefaultValue(1)]
        [Category("Scrolling")]
        [Description("The range in pixels of the horizontal scroll bar.")]
        public int ScrollWidth
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETSCROLLWIDTH).ToInt32();
            }
            set
            {
                DirectMessage(NativeMethods.SCI_SETSCROLLWIDTH, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="ScrollWidth" /> is automatically increased as needed.
        /// </summary>
        /// <returns>
        /// true to automatically increase the horizontal scroll width as needed; otherwise, false.
        /// The default is true.
        /// </returns>
        [DefaultValue(true)]
        [Category("Scrolling")]
        [Description("Determines whether to increase the horizontal scroll width as needed.")]
        public bool ScrollWidthTracking
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETSCROLLWIDTHTRACKING) != IntPtr.Zero;
            }
            set
            {
                IntPtr tracking = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETSCROLLWIDTHTRACKING, tracking);
            }
        }

        /// <summary>
        /// Gets or sets the search flags used when searching text.
        /// </summary>
        /// <returns>A bitwise combination of <see cref="ScintillaNET.SearchFlags" /> values. The default is <see cref="ScintillaNET.SearchFlags.None" />.</returns>
        /// <seealso cref="SearchInTarget" />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SearchFlags SearchFlags
        {
            get
            {
                return (SearchFlags)DirectMessage(NativeMethods.SCI_GETSEARCHFLAGS).ToInt32();
            }
            set
            {
                int searchFlags = (int)value;
                DirectMessage(NativeMethods.SCI_SETSEARCHFLAGS, new IntPtr(searchFlags));
            }
        }

        /// <summary>
        /// Gets the selected text.
        /// </summary>
        /// <returns>The selected text if there is any; otherwise, an empty string.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public unsafe string SelectedText
        {
            get
            {
                // NOTE: For some reason the length returned by this API includes the terminating NULL
                int length = DirectMessage(NativeMethods.SCI_GETSELTEXT).ToInt32();

                if (length <= 0)
                    return string.Empty;

                byte[] bytes = new byte[length + 1];
                fixed (byte* bp = bytes)
                {
                    DirectMessage(NativeMethods.SCI_GETSELTEXT, IntPtr.Zero, new IntPtr(bp));
                    return Helpers.GetString(new IntPtr(bp), length, Encoding);
                }
            }
        }

        /// <summary>
        /// Gets or sets the end position of the selection.
        /// </summary>
        /// <returns>The zero-based document position where the selection ends.</returns>
        /// <remarks>
        /// When getting this property, the return value is <code>Math.Max(<see cref="AnchorPosition" />, <see cref="CurrentPosition" />)</code>.
        /// When setting this property, <see cref="CurrentPosition" /> is set to the value specified and <see cref="AnchorPosition" /> set to <code>Math.Min(<see cref="AnchorPosition" />, <paramref name="value" />)</code>.
        /// The caret is not scrolled into view.
        /// </remarks>
        /// <seealso cref="SelectionStart" />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionEnd
        {
            get
            {
                int pos = DirectMessage(NativeMethods.SCI_GETSELECTIONEND).ToInt32();
                return Lines.ByteToCharPosition(pos);
            }
            set
            {
                value = Helpers.Clamp(value, 0, TextLength);
                value = Lines.CharToBytePosition(value);
                DirectMessage(NativeMethods.SCI_SETSELECTIONEND, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets whether to fill past the end of a line with the selection background color.
        /// </summary>
        /// <returns>true to fill past the end of the line; otherwise, false. The default is false.</returns>
        [DefaultValue(false)]
        [Category("Selection")]
        [Description("Determines whether a selection should fill past the end of the line.")]
        public bool SelectionEolFilled
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETSELEOLFILLED) != IntPtr.Zero;
            }
            set
            {
                IntPtr eolFilled = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETSELEOLFILLED, eolFilled);
            }
        }

        /// <summary>
        /// Gets or sets the color of visible white space.
        /// </summary>
        [Description("The color of visible white space.")]
        [Category("Whitespace")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color WhitespaceTextColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_WHITE_SPACE)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_WHITE_SPACE), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the background color of visible white space.
        /// </summary>
        [Description("The background color of visible white space.")]
        [Category("Whitespace")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color WhitespaceBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_WHITE_SPACE_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_WHITE_SPACE_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the text color of active hot spot.
        /// </summary>
        [Description("The text color of active hot spot.")]
        [Category("Hotspot")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color ActiveHotspotTextColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_HOT_SPOT_ACTIVE)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_HOT_SPOT_ACTIVE), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the background color of active hot spot.
        /// </summary>
        [Description("The background color of active hot spot.")]
        [Category("Hotspot")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color ActiveHotspotBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_HOT_SPOT_ACTIVE_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_HOT_SPOT_ACTIVE_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the text color of main selection.
        /// </summary>
        [Description("The text color of main selection.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color SelectionTextColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_TEXT)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_TEXT), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the background color of main selection.
        /// </summary>
        [Description("The background color of main selection.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "Silver")]
        public Color SelectionBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the text color of additional selections.
        /// </summary>
        [Description("The text color of additional selections.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color SelectionAdditionalTextColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_ADDITIONAL_TEXT)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_ADDITIONAL_TEXT), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the background color of additional selections.
        /// </summary>
        [Description("The background color of additional selections.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "215, 215, 215")]
        public Color SelectionAdditionalBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_ADDITIONAL_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_ADDITIONAL_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the text colour of selections when another window contains the primary selection.
        /// </summary>
        [Description("The text colour of selections when another window contains the primary selection.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color SelectionSecondaryTextColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_SECONDARY_TEXT)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_SECONDARY_TEXT), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the background color of selections when another window contains the primary selection.
        /// </summary>
        [Description("The background color of selections when another window contains the primary selection.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "176, 176, 176")]
        public Color SelectionSecondaryBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_SECONDARY_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_SECONDARY_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the text colour of selections when the control has no focus.
        /// </summary>
        [Description("The text colour of selections when the control has no focus.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color SelectionInactiveTextColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_INACTIVE_TEXT)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_INACTIVE_TEXT), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the selection highlight color to use when the control has no focus.
        /// </summary>
        [Description("The selection highlight color to use when the control has no focus.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "63, 128, 128, 128")]
        public Color SelectionInactiveBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_INACTIVE_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_INACTIVE_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the selected text color to use when the control has no focus.
        /// </summary>
        [Description("The selected text color to use when the control has no focus.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color SelectionInactiveAdditionalTextColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_INACTIVE_ADDITIONAL_TEXT)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_INACTIVE_ADDITIONAL_TEXT), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the selection highlight color to use when the control has no focus.
        /// </summary>
        [Description("The selection highlight color to use when the control has no focus.")]
        [Category("Selection")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color SelectionInactiveAdditionalBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_INACTIVE_ADDITIONAL_BACK)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_SELECTION_INACTIVE_ADDITIONAL_BACK), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the color of fold lines.
        /// </summary>
        [Description("The color of fold lines.")]
        [Category("Folding")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color FoldLineBackColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_FOLD_LINE)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_FOLD_LINE), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets or sets the color of line drawn to show there are lines hidden at that point.
        /// </summary>
        [Description("The color of line drawn to show there are lines hidden at that point.")]
        [Category("Folding")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color FoldLineStripColor
        {
            get
            {
                int color = DirectMessage(NativeMethods.SCI_GETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_HIDDEN_LINE)).ToInt32();
                return HelperMethods.FromWin32Color(color);
            }
            set
            {
                int color = HelperMethods.ToWin32Color(value);
                DirectMessage(NativeMethods.SCI_SETELEMENTCOLOUR, new IntPtr(NativeMethods.SC_ELEMENT_HIDDEN_LINE), new IntPtr(color));
            }
        }

        /// <summary>
        /// Gets a collection representing multiple selections in a <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>A collection of selections.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SelectionCollection Selections { get; private set; }

        /// <summary>
        /// Gets or sets the start position of the selection.
        /// </summary>
        /// <returns>The zero-based document position where the selection starts.</returns>
        /// <remarks>
        /// When getting this property, the return value is <code>Math.Min(<see cref="AnchorPosition" />, <see cref="CurrentPosition" />)</code>.
        /// When setting this property, <see cref="AnchorPosition" /> is set to the value specified and <see cref="CurrentPosition" /> set to <code>Math.Max(<see cref="CurrentPosition" />, <paramref name="value" />)</code>.
        /// The caret is not scrolled into view.
        /// </remarks>
        /// <seealso cref="SelectionEnd" />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get
            {
                int pos = DirectMessage(NativeMethods.SCI_GETSELECTIONSTART).ToInt32();
                return Lines.ByteToCharPosition(pos);
            }
            set
            {
                value = Helpers.Clamp(value, 0, TextLength);
                value = Lines.CharToBytePosition(value);
                DirectMessage(NativeMethods.SCI_SETSELECTIONSTART, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the last internal error code used by Scintilla.
        /// </summary>
        /// <returns>
        /// One of the <see cref="Status" /> enumeration values.
        /// The default is <see cref="ScintillaNET.Status.Ok" />.
        /// </returns>
        /// <remarks>The status can be reset by setting the property to <see cref="ScintillaNET.Status.Ok" />.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Status Status
        {
            get
            {
                return (Status)DirectMessage(NativeMethods.SCI_GETSTATUS);
            }
            set
            {
                int status = (int)value;
                DirectMessage(NativeMethods.SCI_SETSTATUS, new IntPtr(status));
            }
        }

        /// <summary>
        /// Gets a collection representing style definitions in a <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>A collection of style definitions.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StyleCollection Styles { get; private set; }

        /// <summary>
        /// Gets or sets how tab characters are represented when whitespace is visible.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ScintillaNET.TabDrawMode" /> enumeration values.
        /// The default is <see cref="ScintillaNET.TabDrawMode.LongArrow" />.
        /// </returns>
        /// <seealso cref="ViewWhitespace" />
        [DefaultValue(TabDrawMode.LongArrow)]
        [Category("Whitespace")]
        [Description("Style of visible tab characters.")]
        public TabDrawMode TabDrawMode
        {
            get
            {
                return (TabDrawMode)DirectMessage(NativeMethods.SCI_GETTABDRAWMODE);
            }
            set
            {
                int tabDrawMode = (int)value;
                DirectMessage(NativeMethods.SCI_SETTABDRAWMODE, new IntPtr(tabDrawMode));
            }
        }

        /// <summary>
        /// Gets or sets whether tab inserts a tab character, or indents.
        /// </summary>
        /// <returns>Whether tab inserts a tab character (false), or indents (true).</returns>
        [DefaultValue(true)]
        [Category("Indentation")]
        [Description("Determines whether tab inserts a tab character, or indents.")]
        public bool TabIndents
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETTABINDENTS) != IntPtr.Zero;
            }
            set
            {
                IntPtr ptr = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETTABINDENTS, ptr);
            }
        }

        /// <summary>
        /// Gets or sets the width of a tab as a multiple of a space character.
        /// </summary>
        /// <returns>The width of a tab measured in characters. The default is 4.</returns>
        [DefaultValue(4)]
        [Category("Indentation")]
        [Description("The tab size in characters.")]
        public int TabWidth
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETTABWIDTH).ToInt32();
            }
            set
            {
                DirectMessage(NativeMethods.SCI_SETTABWIDTH, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the end position used when performing a search or replace.
        /// </summary>
        /// <returns>The zero-based character position within the document to end a search or replace operation.</returns>
        /// <seealso cref="TargetStart"/>
        /// <seealso cref="SearchInTarget" />
        /// <seealso cref="ReplaceTarget" />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TargetEnd
        {
            get
            {
                // The position can become stale and point to a place outside of the document so we must clamp it
                int bytePos = Helpers.Clamp(DirectMessage(NativeMethods.SCI_GETTARGETEND).ToInt32(), 0, DirectMessage(NativeMethods.SCI_GETTEXTLENGTH).ToInt32());
                return Lines.ByteToCharPosition(bytePos);
            }
            set
            {
                value = Helpers.Clamp(value, 0, TextLength);
                value = Lines.CharToBytePosition(value);
                DirectMessage(NativeMethods.SCI_SETTARGETEND, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the start position used when performing a search or replace.
        /// </summary>
        /// <returns>The zero-based character position within the document to start a search or replace operation.</returns>
        /// <seealso cref="TargetEnd"/>
        /// <seealso cref="SearchInTarget" />
        /// <seealso cref="ReplaceTarget" />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TargetStart
        {
            get
            {
                // The position can become stale and point to a place outside of the document so we must clamp it
                int bytePos = Helpers.Clamp(DirectMessage(NativeMethods.SCI_GETTARGETSTART).ToInt32(), 0, DirectMessage(NativeMethods.SCI_GETTEXTLENGTH).ToInt32());
                return Lines.ByteToCharPosition(bytePos);
            }
            set
            {
                value = Helpers.Clamp(value, 0, TextLength);
                value = Lines.CharToBytePosition(value);
                DirectMessage(NativeMethods.SCI_SETTARGETSTART, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets the current target text.
        /// </summary>
        /// <returns>A String representing the text between <see cref="TargetStart" /> and <see cref="TargetEnd" />.</returns>
        /// <remarks>Targets which have a start position equal or greater to the end position will return an empty String.</remarks>
        /// <seealso cref="TargetStart" />
        /// <seealso cref="TargetEnd" />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public unsafe string TargetText
        {
            get
            {
                int length = DirectMessage(NativeMethods.SCI_GETTARGETTEXT).ToInt32();
                if (length == 0)
                    return string.Empty;

                byte[] bytes = new byte[length + 1];
                fixed (byte* bp = bytes)
                {
                    DirectMessage(NativeMethods.SCI_GETTARGETTEXT, IntPtr.Zero, new IntPtr(bp));
                    return Helpers.GetString(new IntPtr(bp), length, Encoding);
                }
            }
        }

        /// <summary>
        /// Gets or sets the rendering technology used.
        /// </summary>
        /// <returns>
        /// One of the <see cref="Technology" /> enumeration values.
        /// The default is <see cref="ScintillaNET.Technology.Default" />.
        /// </returns>
        [DefaultValue(Technology.Default)]
        [Category("Misc")]
        [Description("The rendering technology used to draw text.")]
        public Technology Technology
        {
            get
            {
                return (Technology)DirectMessage(NativeMethods.SCI_GETTECHNOLOGY);
            }
            set
            {
                int technology = (int)value;
                DirectMessage(NativeMethods.SCI_SETTECHNOLOGY, new IntPtr(technology));
            }
        }

        /// <summary>
        /// Gets or sets the current document text in the <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>The text displayed in the control.</returns>
        /// <remarks>Depending on the length of text get or set, this operation can be expensive.</remarks>
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", typeof(UITypeEditor))]
        [Description("The text associated with this control.")]
        [Category("Appearance")]
        public override unsafe string Text
        {
            get
            {
                int length = DirectMessage(NativeMethods.SCI_GETTEXTLENGTH).ToInt32();
                IntPtr ptr = DirectMessage(NativeMethods.SCI_GETRANGEPOINTER, new IntPtr(0), new IntPtr(length));
                if (ptr == IntPtr.Zero)
                {
                    return string.Empty;
                }

                // Assumption is that moving the gap will always be equal to or less expensive
                // than using one of the APIs which requires an intermediate buffer.
                string text = new((sbyte*)ptr, 0, length, Encoding);
                return text;
            }
            set
            {
                bool previousReadOnly = DesignMode && ReadOnly;

                // Allow Text property change in read-only mode when the designer is active.
                if (previousReadOnly && DesignMode)
                {
                    DirectMessage(NativeMethods.SCI_SETREADONLY, IntPtr.Zero);
                }

                if (string.IsNullOrEmpty(value))
                {
                    DirectMessage(NativeMethods.SCI_CLEARALL);
                }
                else if (value.Contains("\0"))
                {
                    DirectMessage(NativeMethods.SCI_CLEARALL);
                    AppendText(value);
                }
                else
                {
                    fixed (byte* bp = Helpers.GetBytes(value, Encoding, zeroTerminated: true))
                        DirectMessage(NativeMethods.SCI_SETTEXT, IntPtr.Zero, new IntPtr(bp));
                }

                // Allow Text property change in read-only mode when the designer is active.
                if (previousReadOnly && DesignMode)
                {
                    DirectMessage(NativeMethods.SCI_SETREADONLY, new IntPtr(1));
                }
            }
        }

        /// <summary>
        /// Gets the length of the text in the control.
        /// </summary>
        /// <returns>The number of characters in the document.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TextLength => Lines.TextLength;

        /// <summary>
        /// Gets or sets whether to use a mixture of tabs and spaces for indentation or purely spaces.
        /// </summary>
        /// <returns>true to use tab characters; otherwise, false. The default is true.</returns>
        [DefaultValue(false)]
        [Category("Indentation")]
        [Description("Determines whether indentation allows tab characters or purely space characters.")]
        public bool UseTabs
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETUSETABS) != IntPtr.Zero;
            }
            set
            {
                IntPtr useTabs = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETUSETABS, useTabs);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the wait cursor for the current control.
        /// </summary>
        /// <returns>true to use the wait cursor for the current control; otherwise, false. The default is false.</returns>
        public new bool UseWaitCursor
        {
            get
            {
                return base.UseWaitCursor;
            }
            set
            {
                base.UseWaitCursor = value;
                int cursor = value ? NativeMethods.SC_CURSORWAIT : NativeMethods.SC_CURSORNORMAL;
                DirectMessage(NativeMethods.SCI_SETCURSOR, new IntPtr(cursor));
            }
        }

        /// <summary>
        /// Gets or sets the visibility of end-of-line characters.
        /// </summary>
        /// <returns>true to display end-of-line characters; otherwise, false. The default is false.</returns>
        [DefaultValue(false)]
        [Category("Line Endings")]
        [Description("Display end-of-line characters.")]
        public bool ViewEol
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETVIEWEOL) != IntPtr.Zero;
            }
            set
            {
                IntPtr visible = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETVIEWEOL, visible);
            }
        }

        /// <summary>
        /// Gets or sets how to display whitespace characters.
        /// </summary>
        /// <returns>One of the <see cref="WhitespaceMode" /> enumeration values. The default is <see cref="WhitespaceMode.Invisible" />.</returns>
        /// <seealso cref="SetWhitespaceForeColor" />
        /// <seealso cref="SetWhitespaceBackColor" />
        [DefaultValue(WhitespaceMode.Invisible)]
        [Category("Whitespace")]
        [Description("Options for displaying whitespace characters.")]
        public WhitespaceMode ViewWhitespace
        {
            get
            {
                return (WhitespaceMode)DirectMessage(NativeMethods.SCI_GETVIEWWS);
            }
            set
            {
                int wsMode = (int)value;
                DirectMessage(NativeMethods.SCI_SETVIEWWS, new IntPtr(wsMode));
            }
        }

        /// <summary>
        /// Gets or sets the ability for the caret to move into an area beyond the end of each line, otherwise known as virtual space.
        /// </summary>
        /// <returns>
        /// A bitwise combination of the <see cref="VirtualSpace" /> enumeration.
        /// The default is <see cref="VirtualSpace.None" />.
        /// </returns>
        [DefaultValue(VirtualSpace.None)]
        [Category("Behavior")]
        [Description("Options for allowing the caret to move beyond the end of each line.")]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(FlagsConverter))]
        public VirtualSpace VirtualSpaceOptions
        {
            get
            {
                return (VirtualSpace)DirectMessage(NativeMethods.SCI_GETVIRTUALSPACEOPTIONS);
            }
            set
            {
                int virtualSpace = (int)value;
                DirectMessage(NativeMethods.SCI_SETVIRTUALSPACEOPTIONS, new IntPtr(virtualSpace));
            }
        }

        /// <summary>
        /// Gets or sets whether to display the vertical scroll bar.
        /// </summary>
        /// <returns>true to display the vertical scroll bar when needed; otherwise, false. The default is true.</returns>
        [DefaultValue(true)]
        [Category("Scrolling")]
        [Description("Determines whether to show the vertical scroll bar when needed.")]
        public bool VScrollBar
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETVSCROLLBAR) != IntPtr.Zero;
            }
            set
            {
                IntPtr visible = value ? new IntPtr(1) : IntPtr.Zero;
                DirectMessage(NativeMethods.SCI_SETVSCROLLBAR, visible);
            }
        }

        private int VisibleLineCount
        {
            get
            {
                bool wordWrapDisabled = WrapMode == WrapMode.None;
                bool allLinesVisible = Lines.AllLinesVisible;

                if (wordWrapDisabled && allLinesVisible)
                {
                    return Lines.Count;
                }

                int count = 0;
                for (int i = 0; i < Lines.Count; i++)
                {
                    if (allLinesVisible || Lines[i].Visible)
                    {
                        count += wordWrapDisabled ? 1 : Lines[i].WrapCount;
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Gets or sets the characters considered 'whitespace' characters when using any word-based logic.
        /// </summary>
        /// <returns>A string of whitespace characters.</returns>
        [Browsable(false)]
        [Category("Whitespace")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public unsafe string WhitespaceChars
        {
            get
            {
                int length = DirectMessage(NativeMethods.SCI_GETWHITESPACECHARS, IntPtr.Zero, IntPtr.Zero).ToInt32();
                byte[] bytes = new byte[length + 1];
                fixed (byte* bp = bytes)
                {
                    DirectMessage(NativeMethods.SCI_GETWHITESPACECHARS, IntPtr.Zero, new IntPtr(bp));
                    return Helpers.GetString(new IntPtr(bp), length, Encoding.ASCII);
                }
            }
            set
            {
                if (value == null)
                {
                    DirectMessage(NativeMethods.SCI_SETWHITESPACECHARS, IntPtr.Zero, IntPtr.Zero);
                    return;
                }

                // Scintilla stores each of the characters specified in a char array which it then
                // uses as a lookup for word matching logic. Thus, any multibyte chars wouldn't work.
                byte[] bytes = Helpers.GetBytes(value, Encoding.ASCII, zeroTerminated: true);
                fixed (byte* bp = bytes)
                    DirectMessage(NativeMethods.SCI_SETWHITESPACECHARS, IntPtr.Zero, new IntPtr(bp));
            }
        }

        /// <summary>
        /// Gets or sets the size of the dots used to mark whitespace.
        /// </summary>
        /// <returns>The size of the dots used to mark whitespace. The default is 1.</returns>
        /// <seealso cref="ViewWhitespace" />
        [DefaultValue(1)]
        [Category("Whitespace")]
        [Description("The size of whitespace dots.")]
        public int WhitespaceSize
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETWHITESPACESIZE).ToInt32();
            }
            set
            {
                DirectMessage(NativeMethods.SCI_SETWHITESPACESIZE, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the characters considered 'word' characters when using any word-based logic.
        /// </summary>
        /// <returns>A string of word characters.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public unsafe string WordChars
        {
            get
            {
                int length = DirectMessage(NativeMethods.SCI_GETWORDCHARS, IntPtr.Zero, IntPtr.Zero).ToInt32();
                byte[] bytes = new byte[length + 1];
                fixed (byte* bp = bytes)
                {
                    DirectMessage(NativeMethods.SCI_GETWORDCHARS, IntPtr.Zero, new IntPtr(bp));
                    return Helpers.GetString(new IntPtr(bp), length, Encoding.ASCII);
                }
            }
            set
            {
                if (value == null)
                {
                    DirectMessage(NativeMethods.SCI_SETWORDCHARS, IntPtr.Zero, IntPtr.Zero);
                    return;
                }

                // Scintilla stores each of the characters specified in a char array which it then
                // uses as a lookup for word matching logic. Thus, any multibyte chars wouldn't work.
                byte[] bytes = Helpers.GetBytes(value, Encoding.ASCII, zeroTerminated: true);
                fixed (byte* bp = bytes)
                    DirectMessage(NativeMethods.SCI_SETWORDCHARS, IntPtr.Zero, new IntPtr(bp));
            }
        }

        /// <summary>
        /// Gets or sets the line wrapping indent mode.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ScintillaNET.WrapIndentMode" /> enumeration values.
        /// The default is <see cref="ScintillaNET.WrapIndentMode.Fixed" />.
        /// </returns>
        [DefaultValue(WrapIndentMode.Fixed)]
        [Category("Line Wrapping")]
        [Description("Determines how wrapped sublines are indented.")]
        public WrapIndentMode WrapIndentMode
        {
            get
            {
                return (WrapIndentMode)DirectMessage(NativeMethods.SCI_GETWRAPINDENTMODE);
            }
            set
            {
                int wrapIndentMode = (int)value;
                DirectMessage(NativeMethods.SCI_SETWRAPINDENTMODE, new IntPtr(wrapIndentMode));
            }
        }

        /// <summary>
        /// Gets or sets the line wrapping mode.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ScintillaNET.WrapMode" /> enumeration values.
        /// The default is <see cref="ScintillaNET.WrapMode.None" />.
        /// </returns>
        [DefaultValue(WrapMode.None)]
        [Category("Line Wrapping")]
        [Description("The line wrapping strategy.")]
        public WrapMode WrapMode
        {
            get
            {
                return (WrapMode)DirectMessage(NativeMethods.SCI_GETWRAPMODE);
            }
            set
            {
                int wrapMode = (int)value;
                DirectMessage(NativeMethods.SCI_SETWRAPMODE, new IntPtr(wrapMode));
            }
        }

        /// <summary>
        /// Gets or sets the indented size in pixels of wrapped sublines.
        /// </summary>
        /// <returns>The indented size of wrapped sublines measured in pixels. The default is 0.</returns>
        /// <remarks>
        /// Setting <see cref="WrapVisualFlags" /> to <see cref="ScintillaNET.WrapVisualFlags.Start" /> will add an
        /// additional 1 pixel to the value specified.
        /// </remarks>
        [DefaultValue(0)]
        [Category("Line Wrapping")]
        [Description("The amount of pixels to indent wrapped sublines.")]
        public int WrapStartIndent
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETWRAPSTARTINDENT).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETWRAPSTARTINDENT, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the wrap visual flags.
        /// </summary>
        /// <returns>
        /// A bitwise combination of the <see cref="ScintillaNET.WrapVisualFlags" /> enumeration.
        /// The default is <see cref="ScintillaNET.WrapVisualFlags.None" />.
        /// </returns>
        [DefaultValue(WrapVisualFlags.None)]
        [Category("Line Wrapping")]
        [Description("The visual indicator displayed on a wrapped line.")]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(FlagsConverter))]
        public WrapVisualFlags WrapVisualFlags
        {
            get
            {
                return (WrapVisualFlags)DirectMessage(NativeMethods.SCI_GETWRAPVISUALFLAGS);
            }
            set
            {
                int wrapVisualFlags = (int)value;
                DirectMessage(NativeMethods.SCI_SETWRAPVISUALFLAGS, new IntPtr(wrapVisualFlags));
            }
        }

        /// <summary>
        /// Gets or sets additional location options when displaying wrap visual flags.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ScintillaNET.WrapVisualFlagLocation" /> enumeration values.
        /// The default is <see cref="ScintillaNET.WrapVisualFlagLocation.Default" />.
        /// </returns>
        [DefaultValue(WrapVisualFlagLocation.Default)]
        [Category("Line Wrapping")]
        [Description("The location of wrap visual flags in relation to the line text.")]
        public WrapVisualFlagLocation WrapVisualFlagLocation
        {
            get
            {
                return (WrapVisualFlagLocation)DirectMessage(NativeMethods.SCI_GETWRAPVISUALFLAGSLOCATION);
            }
            set
            {
                int location = (int)value;
                DirectMessage(NativeMethods.SCI_SETWRAPVISUALFLAGSLOCATION, new IntPtr(location));
            }
        }

        /// <summary>
        /// Gets or sets the horizontal scroll offset.
        /// </summary>
        /// <returns>The horizontal scroll offset in pixels.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int XOffset
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETXOFFSET).ToInt32();
            }
            set
            {
                value = Helpers.ClampMin(value, 0);
                DirectMessage(NativeMethods.SCI_SETXOFFSET, new IntPtr(value));
            }
        }

        /// <summary>
        /// Gets or sets the zoom factor.
        /// </summary>
        /// <returns>The zoom factor measured in points.</returns>
        /// <remarks>For best results, values should range from -10 to 20 points.</remarks>
        /// <seealso cref="ZoomIn" />
        /// <seealso cref="ZoomOut" />
        [DefaultValue(0)]
        [Category("Appearance")]
        [Description("Zoom factor in points applied to the displayed text.")]
        public int Zoom
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETZOOM).ToInt32();
            }
            set
            {
                DirectMessage(NativeMethods.SCI_SETZOOM, new IntPtr(value));
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Occurs when an autocompletion list is cancelled.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when an autocompletion list is cancelled.")]
        public event EventHandler<EventArgs> AutoCCancelled
        {
            add
            {
                Events.AddHandler(autoCCancelledEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(autoCCancelledEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the user deletes a character while an autocompletion list is active.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the user deletes a character while an autocompletion list is active.")]
        public event EventHandler<EventArgs> AutoCCharDeleted
        {
            add
            {
                Events.AddHandler(autoCCharDeletedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(autoCCharDeletedEventKey, value);
            }
        }

        /// <summary>
        /// Occurs after autocompleted text is inserted.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs after autocompleted text has been inserted.")]
        public event EventHandler<AutoCSelectionEventArgs> AutoCCompleted
        {
            add
            {
                Events.AddHandler(autoCCompletedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(autoCCompletedEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when a user has selected an item in an autocompletion list.
        /// </summary>
        /// <remarks>Automatic insertion can be cancelled by calling <see cref="AutoCCancel" /> from the event handler.</remarks>
        [Category("Notifications")]
        [Description("Occurs when a user has selected an item in an autocompletion list.")]
        public event EventHandler<AutoCSelectionEventArgs> AutoCSelection
        {
            add
            {
                Events.AddHandler(autoCSelectionEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(autoCSelectionEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when a user has highlighted an item in an autocompletion list.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when a user has highlighted an item in an autocompletion list.")]
        public event EventHandler<AutoCSelectionChangeEventArgs> AutoCSelectionChange
        {
            add
            {
                Events.AddHandler(autoCSelectionChangeEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(autoCSelectionChangeEventKey, value);
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler BackColorChanged
        {
            add
            {
                base.BackColorChanged += value;
            }
            remove
            {
                base.BackColorChanged -= value;
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler BackgroundImageChanged
        {
            add
            {
                base.BackgroundImageChanged += value;
            }
            remove
            {
                base.BackgroundImageChanged -= value;
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler BackgroundImageLayoutChanged
        {
            add
            {
                base.BackgroundImageLayoutChanged += value;
            }
            remove
            {
                base.BackgroundImageLayoutChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when text is about to be deleted.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs before text is deleted.")]
        public event EventHandler<BeforeModificationEventArgs> BeforeDelete
        {
            add
            {
                Events.AddHandler(beforeDeleteEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(beforeDeleteEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when text is about to be inserted.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs before text is inserted.")]
        public event EventHandler<BeforeModificationEventArgs> BeforeInsert
        {
            add
            {
                Events.AddHandler(beforeInsertEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(beforeInsertEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Scintilla.BorderStyle" /> property has changed.
        /// </summary>
        [Category("Property Changed")]
        [Description("Occurs when the value of the BorderStyle property changes.")]
        public event EventHandler BorderStyleChanged
        {
            add
            {
                Events.AddHandler(borderStyleChangedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(borderStyleChangedEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when an annotation has changed.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when an annotation has changed.")]
        public event EventHandler<ChangeAnnotationEventArgs> ChangeAnnotation
        {
            add
            {
                Events.AddHandler(changeAnnotationEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(changeAnnotationEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the user enters a text character.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the user types a character.")]
        public event EventHandler<CharAddedEventArgs> CharAdded
        {
            add
            {
                Events.AddHandler(charAddedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(charAddedEventKey, value);
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler CursorChanged
        {
            add
            {
                base.CursorChanged += value;
            }
            remove
            {
                base.CursorChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when text has been deleted from the document.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when text is deleted.")]
        public event EventHandler<ModificationEventArgs> Delete
        {
            add
            {
                Events.AddHandler(deleteEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(deleteEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Scintilla" /> control is double-clicked.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the editor is double clicked.")]
        public new event EventHandler<DoubleClickEventArgs> DoubleClick
        {
            add
            {
                Events.AddHandler(doubleClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(doubleClickEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the mouse moves or another activity such as a key press ends a <see cref="DwellStart" /> event.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the mouse moves from its dwell start position.")]
        public event EventHandler<DwellEventArgs> DwellEnd
        {
            add
            {
                Events.AddHandler(dwellEndEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(dwellEndEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the mouse clicked over a call tip displayed by the <see cref="CallTipShow" /> method.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the mouse is clicked over a calltip.")]
        public event EventHandler<CallTipClickEventArgs> CallTipClick
        {
            add
            {
                Events.AddHandler(callTipClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(callTipClickEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the mouse is kept in one position (hovers) for the <see cref="MouseDwellTime" />.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the mouse is kept in one position (hovers) for a period of time.")]
        public event EventHandler<DwellEventArgs> DwellStart
        {
            add
            {
                Events.AddHandler(dwellStartEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(dwellStartEventKey, value);
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler FontChanged
        {
            add
            {
                base.FontChanged += value;
            }
            remove
            {
                base.FontChanged -= value;
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler ForeColorChanged
        {
            add
            {
                base.ForeColorChanged += value;
            }
            remove
            {
                base.ForeColorChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the user clicks on text that is in a style with the <see cref="Style.Hotspot" /> property set.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the user clicks text styled with the hotspot flag.")]
        public event EventHandler<HotspotClickEventArgs> HotspotClick
        {
            add
            {
                Events.AddHandler(hotspotClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(hotspotClickEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the user double clicks on text that is in a style with the <see cref="Style.Hotspot" /> property set.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the user double clicks text styled with the hotspot flag.")]
        public event EventHandler<HotspotClickEventArgs> HotspotDoubleClick
        {
            add
            {
                Events.AddHandler(hotspotDoubleClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(hotspotDoubleClickEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the user releases a click on text that is in a style with the <see cref="Style.Hotspot" /> property set.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the user releases a click on text styled with the hotspot flag.")]
        public event EventHandler<HotspotClickEventArgs> HotspotReleaseClick
        {
            add
            {
                Events.AddHandler(hotspotReleaseClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(hotspotReleaseClickEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the user clicks on text that has an indicator.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the user clicks text with an indicator.")]
        public event EventHandler<IndicatorClickEventArgs> IndicatorClick
        {
            add
            {
                Events.AddHandler(indicatorClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(indicatorClickEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the user releases a click on text that has an indicator.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the user releases a click on text with an indicator.")]
        public event EventHandler<IndicatorReleaseEventArgs> IndicatorRelease
        {
            add
            {
                Events.AddHandler(indicatorReleaseEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(indicatorReleaseEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when text has been inserted into the document.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when text is inserted.")]
        public event EventHandler<ModificationEventArgs> Insert
        {
            add
            {
                Events.AddHandler(insertEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(insertEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when text is about to be inserted. The inserted text can be changed.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs before text is inserted. Permits changing the inserted text.")]
        public event EventHandler<InsertCheckEventArgs> InsertCheck
        {
            add
            {
                Events.AddHandler(insertCheckEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(insertCheckEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the mouse was clicked inside a margin that was marked as sensitive.
        /// </summary>
        /// <remarks>The <see cref="Margin.Sensitive" /> property must be set for a margin to raise this event.</remarks>
        [Category("Notifications")]
        [Description("Occurs when the mouse is clicked in a sensitive margin.")]
        public event EventHandler<MarginClickEventArgs> MarginClick
        {
            add
            {
                Events.AddHandler(marginClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(marginClickEventKey, value);
            }
        }

        // TODO This isn't working in my tests. Could be Windows Forms interfering.
        /// <summary>
        /// Occurs when the mouse was right-clicked inside a margin that was marked as sensitive.
        /// </summary>
        /// <remarks>The <see cref="Margin.Sensitive" /> property and <see cref="PopupMode.Text" /> must be set for a margin to raise this event.</remarks>
        /// <seealso cref="UsePopup(PopupMode)" />
        [Category("Notifications")]
        [Description("Occurs when the mouse is right-clicked in a sensitive margin.")]
        public event EventHandler<MarginClickEventArgs> MarginRightClick
        {
            add
            {
                Events.AddHandler(marginRightClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(marginRightClickEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when a user attempts to change text while the document is in read-only mode.
        /// </summary>
        /// <seealso cref="ReadOnly" />
        [Category("Notifications")]
        [Description("Occurs when an attempt is made to change text in read-only mode.")]
        public event EventHandler<EventArgs> ModifyAttempt
        {
            add
            {
                Events.AddHandler(modifyAttemptEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(modifyAttemptEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the control determines hidden text needs to be shown.
        /// </summary>
        /// <remarks>An example of when this event might be raised is if the end of line of a contracted fold point is deleted.</remarks>
        [Category("Notifications")]
        [Description("Occurs when hidden (folded) text should be shown.")]
        public event EventHandler<NeedShownEventArgs> NeedShown
        {
            add
            {
                Events.AddHandler(needShownEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(needShownEventKey, value);
            }
        }

        internal event EventHandler<SCNotificationEventArgs> SCNotification
        {
            add
            {
                Events.AddHandler(scNotificationEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(scNotificationEventKey, value);
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event PaintEventHandler Paint
        {
            add
            {
                base.Paint += value;
            }
            remove
            {
                base.Paint -= value;
            }
        }

        /// <summary>
        /// Occurs when painting has just been done.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the control is painted.")]
        public event EventHandler<EventArgs> Painted
        {
            add
            {
                Events.AddHandler(paintedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(paintedEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the document becomes 'dirty'.
        /// </summary>
        /// <remarks>The document 'dirty' state can be checked with the <see cref="Modified" /> property and reset by calling <see cref="SetSavePoint" />.</remarks>
        /// <seealso cref="SetSavePoint" />
        /// <seealso cref="SavePointReached" />
        [Category("Notifications")]
        [Description("Occurs when a save point is left and the document becomes dirty.")]
        public event EventHandler<EventArgs> SavePointLeft
        {
            add
            {
                Events.AddHandler(savePointLeftEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(savePointLeftEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the document 'dirty' flag is reset.
        /// </summary>
        /// <remarks>The document 'dirty' state can be reset by calling <see cref="SetSavePoint" /> or undoing an action that modified the document.</remarks>
        /// <seealso cref="SetSavePoint" />
        /// <seealso cref="SavePointLeft" />
        [Category("Notifications")]
        [Description("Occurs when a save point is reached and the document is no longer dirty.")]
        public event EventHandler<EventArgs> SavePointReached
        {
            add
            {
                Events.AddHandler(savePointReachedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(savePointReachedEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the control is about to display or print text and requires styling.
        /// </summary>
        /// <remarks>
        /// This event is only raised when LexerName is set to Container />.
        /// The last position styled correctly can be determined by calling <see cref="GetEndStyled" />.
        /// </remarks>
        /// <seealso cref="GetEndStyled" />
        [Category("Notifications")]
        [Description("Occurs when the text needs styling.")]
        public event EventHandler<StyleNeededEventArgs> StyleNeeded
        {
            add
            {
                Events.AddHandler(styleNeededEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(styleNeededEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the control UI is updated as a result of changes to text (including styling),
        /// selection, and/or scroll positions.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the control UI is updated.")]
        public event EventHandler<UpdateUIEventArgs> UpdateUI
        {
            add
            {
                Events.AddHandler(updateUIEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(updateUIEventKey, value);
            }
        }

        /// <summary>
        /// Occurs when the user zooms the display using the keyboard or the <see cref="Zoom" /> property is changed.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the control is zoomed.")]
        public event EventHandler<EventArgs> ZoomChanged
        {
            add
            {
                Events.AddHandler(zoomChangedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(zoomChangedEventKey, value);
            }
        }

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Scintilla" /> class.
        /// </summary>
        public Scintilla()
        {
            // WM_DESTROY workaround
            if (Scintilla.reparentAll == null || (bool)Scintilla.reparentAll)
                this.reparent = true;

            // We don't want .NET to use GetWindowText because we manage ('cache') our own text
            base.SetStyle(ControlStyles.CacheText, true);

            // Necessary control styles (see TextBoxBase)
            base.SetStyle(ControlStyles.StandardClick |
                     ControlStyles.StandardDoubleClick |
                     ControlStyles.UseTextForAccessibility |
                     ControlStyles.UserPaint,
                     false);

            this.borderStyle = BorderStyle.Fixed3D;

            Lines = new LineCollection(this);
            Styles = new StyleCollection(this);
            Indicators = new IndicatorCollection(this);
            Margins = new MarginCollection(this);
            Markers = new MarkerCollection(this);
            Selections = new SelectionCollection(this);
        }

        #endregion Constructors

        /// <summary>
        /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
        /// </summary>
        /// <value>The right to left.</value>

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Not used by the Scintilla.NET control.")]
        public new RightToLeft RightToLeft { get; set; }
    }
}
