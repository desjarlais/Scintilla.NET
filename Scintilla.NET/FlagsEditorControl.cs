using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ScintillaNET;

internal partial class FlagsEditorControl : UserControl
{
    public FlagsEditorControl()
    {
        InitializeComponent();
        this.button_Ok.Text = NativeMethods.GetMessageBoxString(0);
        this.button_Cancel.Text = NativeMethods.GetMessageBoxString(1);
    }

    private readonly Type enumType;
    private readonly Enum initialValue;
    public Enum Value { get; protected set; }

    private readonly IWindowsFormsEditorService editorService;

    private int inCheck;

    public FlagsEditorControl(IWindowsFormsEditorService editorService, Enum value) : this()
    {
        this.editorService = editorService;

        this.enumType = value.GetType();
        Value = this.initialValue = value;

        this.inCheck++;
        try
        {
            ulong allBits = CalculateEnumAllValue(this.enumType);
            bool hasAll = false;
            ulong valueBits = Convert.ToUInt64(Value);
            foreach (string itemName in Enum.GetNames(this.enumType))
            {
                var item = (Enum)Enum.Parse(this.enumType, itemName);
                ulong itemBits = Convert.ToUInt64(item);
                if (itemBits == allBits)
                    hasAll = true;
                if (itemBits != 0)
                {
                    var checkBox = new CheckBox() {
                        Text = itemName,
                        CheckState = CheckStateFromBits(itemBits, valueBits),
                        AutoSize = true,
                        Tag = item,
                        Margin = new Padding(3, 0, 3, 0),
                        Padding = Padding.Empty,
                        UseVisualStyleBackColor = true,
                    };
                    checkBox.CheckStateChanged += checkBox_CheckStateChanged;
                    this.flowLayoutPanel_CheckBoxList.Controls.Add(checkBox);
                }
            }

            if (!hasAll)
            {
                var checkBox = new CheckBox() {
                    Text = "All",
                    CheckState = CheckStateFromBits(allBits, valueBits),
                    AutoSize = true,
                    Tag = (Enum)Enum.ToObject(this.enumType, allBits),
                    Margin = new Padding(3, 0, 3, 0),
                    Padding = Padding.Empty,
                    UseVisualStyleBackColor = true,
                };
                checkBox.CheckStateChanged += checkBox_CheckStateChanged;
                this.flowLayoutPanel_CheckBoxList.Controls.Add(checkBox);
            }

            AutoSize = true;
        }
        finally
        {
            this.inCheck--;
        }
    }

    private static ulong CalculateEnumAllValue(Type enumType)
    {
        ulong all = 0;
        foreach (Enum bits in Enum.GetValues(enumType))
        {
            all |= Convert.ToUInt64(bits);
        }

        return all;
    }

    private static ulong CombineEnumBits(IEnumerable<CheckBox> checkBoxList)
    {
        ulong bits = 0;
        foreach (CheckBox checkBox in checkBoxList.Where(c => c.CheckState == CheckState.Checked))
        {
            bits |= Convert.ToUInt64(checkBox.Tag);
        }

        return bits;
    }

    private void checkBox_CheckStateChanged(object sender, EventArgs e)
    {
        if (this.inCheck > 0)
            return;
        this.inCheck++;
        try
        {
            var checkTarget = (CheckBox)sender;
            IEnumerable<CheckBox> checkBoxList = this.flowLayoutPanel_CheckBoxList.Controls.OfType<CheckBox>();
            ulong valueBits = CombineEnumBits(checkBoxList);
            ulong changedBits = Convert.ToUInt64(checkTarget.Tag);
            if (checkTarget.CheckState == CheckState.Checked)
                valueBits |= changedBits;
            else if (checkTarget.CheckState == CheckState.Unchecked)
                valueBits &= ~changedBits;
            Value = (Enum)Enum.ToObject(this.enumType, valueBits);
            foreach (CheckBox checkBox in checkBoxList)
            {
                ulong itemBits = Convert.ToUInt64(checkBox.Tag);
                checkBox.CheckState = CheckStateFromBits(itemBits, valueBits);
            }
        }
        finally
        {
            this.inCheck--;
        }
    }

    private static CheckState CheckStateFromBits(ulong itemBits, ulong valueBits)
    {
        return (itemBits & valueBits) == itemBits ? CheckState.Checked : (itemBits & valueBits) == 0 ? CheckState.Unchecked : CheckState.Indeterminate;
    }

    protected override bool ProcessDialogKey(Keys keyData)
    {
        if (keyData == Keys.Return)
        {
            button_Ok_Click(this.button_Ok, EventArgs.Empty);
            return true;
        }

        if (keyData == Keys.Escape)
        {
            button_Cancel_Click(this.button_Cancel, EventArgs.Empty);
            return true;
        }

        return base.ProcessDialogKey(keyData);
    }

    private void button_Ok_Click(object sender, EventArgs e)
    {
        this.editorService.CloseDropDown();
    }

    private void button_Cancel_Click(object sender, EventArgs e)
    {
        Value = this.initialValue;
        this.editorService.CloseDropDown();
    }
}
