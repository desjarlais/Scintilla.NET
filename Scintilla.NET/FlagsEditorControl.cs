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
        button_Ok.Text = NativeMethods.GetMessageBoxString(0);
        button_Cancel.Text = NativeMethods.GetMessageBoxString(1);
    }

    private readonly Type enumType;
    private readonly Enum initialValue;
    public Enum Value { get; protected set; }

    private IWindowsFormsEditorService editorService;

    private int inCheck;

    public FlagsEditorControl(IWindowsFormsEditorService editorService, Enum value) : this()
    {
        this.editorService = editorService;

        enumType = value.GetType();
        Value = initialValue = value;

        inCheck++;
        try
        {
            ulong allBits = CalculateEnumAllValue(enumType);
            bool hasAll = false;
            ulong valueBits = Convert.ToUInt64(Value);
            foreach (string itemName in Enum.GetNames(enumType))
            {
                Enum item = (Enum)Enum.Parse(enumType, itemName);
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
                    flowLayoutPanel_CheckBoxList.Controls.Add(checkBox);
                }
            }
            if (!hasAll)
            {
                var checkBox = new CheckBox() {
                    Text = "All",
                    CheckState = CheckStateFromBits(allBits, valueBits),
                    AutoSize = true,
                    Tag = (Enum)Enum.ToObject(enumType, allBits),
                    Margin = new Padding(3, 0, 3, 0),
                    Padding = Padding.Empty,
                    UseVisualStyleBackColor = true,
                };
                checkBox.CheckStateChanged += checkBox_CheckStateChanged;
                flowLayoutPanel_CheckBoxList.Controls.Add(checkBox);
            }
            this.AutoSize = true;
        }
        finally
        {
            inCheck--;
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
        if (inCheck > 0)
            return;
        inCheck++;
        try
        {
            var checkTarget = (CheckBox)sender;
            var checkBoxList = flowLayoutPanel_CheckBoxList.Controls.OfType<CheckBox>();
            ulong valueBits = CombineEnumBits(checkBoxList);
            ulong changedBits = Convert.ToUInt64(checkTarget.Tag);
            if (checkTarget.CheckState == CheckState.Checked)
                valueBits |= changedBits;
            else if (checkTarget.CheckState == CheckState.Unchecked)
                valueBits &= ~changedBits;
            Value = (Enum)Enum.ToObject(enumType, valueBits);
            foreach (CheckBox checkBox in checkBoxList)
            {
                var itemBits = Convert.ToUInt64(checkBox.Tag);
                checkBox.CheckState = CheckStateFromBits(itemBits, valueBits);
            }
        }
        finally
        {
            inCheck--;
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
            button_Ok_Click(button_Ok, EventArgs.Empty);
            return true;
        }
        if (keyData == Keys.Escape)
        {
            button_Cancel_Click(button_Cancel, EventArgs.Empty);
            return true;
        }
        return base.ProcessDialogKey(keyData);
    }

    private void button_Ok_Click(object sender, EventArgs e)
    {
        editorService.CloseDropDown();
    }

    private void button_Cancel_Click(object sender, EventArgs e)
    {
        Value = initialValue;
        editorService.CloseDropDown();
    }
}
