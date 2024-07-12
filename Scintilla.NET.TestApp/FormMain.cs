using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScintillaNET.TestApp;

public partial class FormMain : Form
{
    string baseTitle;
    string? currentFileName = null;

    public string? CurrentFileName
    {
        get => this.currentFileName;
        set
        {
            BaseTitle = Path.GetFileName(this.currentFileName = value);
        }
    }

    public string BaseTitle
    {
        get => this.baseTitle;
        set
        {
            this.Text = (this.baseTitle = value) + (scintilla.Modified ? " *" : "");
        }
    }

    public FormMain()
    {
        InitializeComponent();

        baseTitle = this.Text;

        scintilla.AssignCmdKey(Keys.Control | Keys.Shift | Keys.Z, Command.Redo);

        scintilla.LexerName = "cpp";

        SetScintillaStyles(scintilla);
        AdjustLineNumberMargin(scintilla);
        AdjustMarkerMargin(scintilla);
        AdjustFoldMargin(scintilla);

        Version scintillaNetVersion = scintilla.GetType().Assembly.GetName().Version;
        string version = scintillaNetVersion.Revision == 0 ? scintillaNetVersion.ToString(3) : scintillaNetVersion.ToString();
        string scintillaVersion = scintilla.ScintillaVersion;
        string lexillaVersion = scintilla.LexillaVersion;

        toolStripStatusLabel_Version.Text = $"ScintillaNET v{version} (Scintilla v{scintillaVersion}, Lexilla v{lexillaVersion})";

        foreach (var group in Lexilla.GetLexerNames().ToArray().OrderBy(x => x).GroupBy(x => char.ToUpperInvariant(x[0])))
        {
            char first = group.Key;

            if (group.Count() > 1)
            {
                var item = (ToolStripMenuItem)lexersToolStripMenuItem.DropDownItems.Add(first.ToString());

                foreach (string lexer in group)
                    item.DropDownItems.Add(lexer, null, Lexer_Click);
            }
            else
                lexersToolStripMenuItem.DropDownItems.Add(group.Single(), null, Lexer_Click);
        }
    }

    private void Lexer_Click(object sender, EventArgs e)
    {
        ToolStripItem item = (ToolStripItem)sender;
        scintilla.LexerName = item.Text;
        SetScintillaStyles(scintilla);
        scintilla.Colorize(0, scintilla.TextLength);
        AdjustFoldMargin(scintilla);
    }

    private void FormMain_Shown(object sender, EventArgs e)
    {
        scintilla.Select();
    }

    private static void SetScintillaStyles(Scintilla scintilla)
    {
        scintilla.StyleClearAll();

        // Configure the CPP (C#) lexer styles
        scintilla.Styles[1].ForeColor = Color.FromArgb(0x00, 0x80, 0x00);  // COMMENT
        scintilla.Styles[2].ForeColor = Color.FromArgb(0x00, 0x80, 0x00);  // COMMENT LINE
        scintilla.Styles[3].ForeColor = Color.FromArgb(0x00, 0x80, 0x80);  // COMMENT DOC
        scintilla.Styles[4].ForeColor = Color.FromArgb(0xFF, 0x80, 0x00);  // NUMBER
        scintilla.Styles[5].ForeColor = Color.FromArgb(0x00, 0x00, 0xFF);  // INSTRUCTION WORD
        scintilla.Styles[6].ForeColor = Color.FromArgb(0x80, 0x80, 0x80);  // STRING
        scintilla.Styles[7].ForeColor = Color.FromArgb(0x80, 0x80, 0x80);  // CHARACTER
        scintilla.Styles[9].ForeColor = Color.FromArgb(0x80, 0x40, 0x00);  // PREPROCESSOR
        scintilla.Styles[10].ForeColor = Color.FromArgb(0x00, 0x00, 0x80); // OPERATOR
        scintilla.Styles[11].ForeColor = Color.FromArgb(0x00, 0x00, 0x00); // DEFAULT
        scintilla.Styles[13].ForeColor = Color.FromArgb(0x00, 0x00, 0x00); // VERBATIM
        scintilla.Styles[14].ForeColor = Color.FromArgb(0x00, 0x00, 0x00); // REGEX
        scintilla.Styles[15].ForeColor = Color.FromArgb(0x00, 0x80, 0x80); // COMMENT LINE DOC
        scintilla.Styles[16].ForeColor = Color.FromArgb(0x80, 0x00, 0xFF); // TYPE WORD
        scintilla.Styles[17].ForeColor = Color.FromArgb(0x00, 0x80, 0x80); // COMMENT DOC KEYWORD
        scintilla.Styles[18].ForeColor = Color.FromArgb(0x00, 0x80, 0x80); // COMMENT DOC KEYWORD ERROR
        scintilla.Styles[23].ForeColor = Color.FromArgb(0x00, 0x80, 0x00); // PREPROCESSOR COMMENT
        scintilla.Styles[24].ForeColor = Color.FromArgb(0x00, 0x80, 0x80); // PREPROCESSOR COMMENT DOC
        scintilla.Styles[5].Bold = true;
        scintilla.Styles[10].Bold = true;
        scintilla.Styles[14].Bold = true;
        scintilla.Styles[17].Bold = true;

        scintilla.SetKeywords(0,
            "abstract add alias as ascending async await base break case catch checked continue default delegate descending do dynamic else event explicit extern false finally fixed for foreach from get global goto group if implicit in interface internal into is join let lock nameof namespace new null object operator orderby out override params partial private protected public readonly ref remove return sealed select set sizeof stackalloc switch this throw true try typeof unchecked unsafe using value virtual when where while yield");
        scintilla.SetKeywords(1,
            "bool byte char class const decimal double enum float int long nint nuint sbyte short static string struct uint ulong ushort var void");
    }

    private static byte CountDigits(int x)
    {
        if (x == 0)
            return 1;

        byte result = 0;
        while (x > 0)
        {
            result++;
            x /= 10;
        }

        return result;
    }

    private static readonly Dictionary<Scintilla, int> maxLineNumberCharLengthMap = [];

    private static void AdjustLineNumberMargin(Scintilla scintilla)
    {
        int maxLineNumberCharLength = CountDigits(scintilla.Lines.Count);
        if (maxLineNumberCharLength == (maxLineNumberCharLengthMap.TryGetValue(scintilla, out int charLen) ? charLen : 0))
            return;

        const int padding = 2;
        scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('0', maxLineNumberCharLength + 1)) + padding;
        maxLineNumberCharLengthMap[scintilla] = maxLineNumberCharLength;
    }

    private static void AdjustMarkerMargin(Scintilla scintilla)
    {
        scintilla.Margins[1].Width = 16;
        scintilla.Margins[1].Sensitive = false;
        //scintilla.Markers[Marker.HistoryRevertedToModified].SetForeColor(Color.Orange);
        //scintilla.Markers[Marker.HistoryRevertedToModified].SetBackColor(scintilla.Margins[1].BackColor);
        //scintilla.Markers[Marker.HistoryRevertedToOrigin].SetForeColor(Color.Orange);
        //scintilla.Markers[Marker.HistoryRevertedToOrigin].SetBackColor(scintilla.Margins[1].BackColor);
    }

    private static void AdjustFoldMargin(Scintilla scintilla)
    {
        // Instruct the lexer to calculate folding
        scintilla.SetProperty("fold", "1");

        // Configure a margin to display folding symbols
        scintilla.Margins[2].Type = MarginType.Symbol;
        scintilla.Margins[2].Mask = Marker.MaskFolders;
        scintilla.Margins[2].Sensitive = true;
        scintilla.Margins[2].Width = 20;

        // Set colors for all folding markers
        for (int i = 25; i <= 31; i++)
        {
            scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
            scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
        }

        // Configure folding markers with respective symbols
        scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
        scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
        scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
        scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
        scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
        scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
        scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

        // Enable automatic folding
        scintilla.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        {
            CurrentFileName = openFileDialog.FileName;
            scintilla.Text = File.ReadAllText(CurrentFileName, Encoding.UTF8);
            scintilla.ClearChangeHistory();
            scintilla.SetSavePoint();
        }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (CurrentFileName is null && saveFileDialog.ShowDialog(this) == DialogResult.OK)
            CurrentFileName = saveFileDialog.FileName;

        if (CurrentFileName is not null)
        {
            File.WriteAllText(CurrentFileName, scintilla.Text, Encoding.UTF8);
            scintilla.SetSavePoint();
        }
    }

    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        //if (scintilla.Modified)
        //{
        //    if (MessageBox.Show("You have unsaved changes, are you sure to exit?", "Scintilla", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
        //        e.Cancel = true;
        //}
    }

    private void describeKeywordSetsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        scintilla.ReplaceSelection(scintilla.DescribeKeywordSets());
    }

    private void scintilla_TextChanged(object sender, EventArgs e)
    {
        AdjustLineNumberMargin(scintilla);
    }

    private void scintilla_SavePointLeft(object sender, EventArgs e)
    {
        Text = BaseTitle + " *";
    }

    private void scintilla_SavePointReached(object sender, EventArgs e)
    {
        Text = BaseTitle;
    }
}
