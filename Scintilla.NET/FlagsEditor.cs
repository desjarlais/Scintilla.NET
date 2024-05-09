using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ScintillaNET;

internal class FlagsConverter<T> : TypeConverter where T : struct, Enum
{
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (value is Enum e && destinationType == typeof(string))
        {
            if (Convert.ToUInt64(e) == 0)
            {
                return Enum.ToObject(e.GetType(), 0).ToString();
            }
            StringBuilder sb = new StringBuilder();
            foreach (Enum item in Enum.GetValues(e.GetType()))
            {
                if (Convert.ToUInt64(item) != 0 && e.HasFlag(item))
                {
                    if (sb.Length > 0)
                        sb.Append(" | ");
                    sb.Append(item);
                }
            }
            return sb.ToString();
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string str)
        {
            Type t = typeof(T);
            ulong bits = 0;
            var nameList = str.Split('|').Select(x => x.Trim());
            foreach (var name in nameList)
            {
                if (Enum.TryParse(name, out T bit))
                    bits |= Convert.ToUInt64(bit);
                else
                    throw new InvalidCastException($"Cannot convert \"{str.Replace("\"", "\\\"")}\" to {t}.");
            }
            return Enum.ToObject(t, bits);
        }
        return base.ConvertFrom(context, culture, value);
    }
}

internal class FlagsEditor : UITypeEditor
{
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.DropDown;

    public override bool IsDropDownResizable => true;

    private int inCheck = 0;

    private static ulong CombineEnumList(IEnumerable checkedList)
    {
        ulong bits = 0;
        foreach (var item in checkedList)
        {
            bits |= Convert.ToUInt64(item);
        }
        return bits;
    }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        if (value is Enum e && context.PropertyDescriptor.Attributes.OfType<FlagsAttribute>().Any())
        {
            IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            Type enumType = e.GetType();

            CheckedListBox checkedListBox = new() {
                Dock = DockStyle.Fill,
                CheckOnClick = true,
            };
            checkedListBox.ItemCheck += (object sender, ItemCheckEventArgs e) => {
                if (inCheck > 0)
                    return;
                inCheck++;
                try
                {
                    ulong bits = CombineEnumList(checkedListBox.CheckedItems);
                    ulong change = Convert.ToUInt64(checkedListBox.Items[e.Index]);
                    if (e.NewValue == CheckState.Checked)
                        bits |= change;
                    else if (e.NewValue == CheckState.Unchecked)
                        bits &= ~change;
                    Enum enumFinal = (Enum)Enum.ToObject(enumType, bits);
                    for (int i = 0; i < checkedListBox.Items.Count; i++)
                    {
                        Enum itemValue = (Enum)checkedListBox.Items[i];
                        checkedListBox.SetItemChecked(i, enumFinal.HasFlag(itemValue));
                    }
                }
                finally
                {
                    inCheck--;
                }
            };
            inCheck++;
            try
            {
                foreach (Enum item in Enum.GetValues(enumType))
                {
                    if (Convert.ToUInt64(item) != 0)
                        checkedListBox.Items.Add(item, e.HasFlag(item));
                }
            }
            finally
            {
                inCheck--;
            }
            Button okBtton = new() {
                Dock = DockStyle.Bottom,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                UseVisualStyleBackColor = true,
            };
            UserControl userControl = new();
            userControl.Controls.Add(checkedListBox);
            userControl.Controls.Add(okBtton);
            okBtton.Text = NativeMethods.GetMessageBoxString(0); // OK
            okBtton.Click += (object sender, EventArgs e) => {
                ulong bits = 0;
                foreach (Enum item in checkedListBox.CheckedItems)
                {
                    bits |= Convert.ToUInt64(item);
                }
                value = Enum.ToObject(enumType, bits);
                svc.CloseDropDown();
            };
            svc.DropDownControl(userControl);
            return value;
        }
        else
            return base.EditValue(context, provider, value);
    }
}
