using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms.Design;

namespace ScintillaNET;

internal class FlagsEditor : UITypeEditor
{
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.DropDown;

    public override bool IsDropDownResizable => true;

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        if (value is Enum e && context.PropertyDescriptor.Attributes.OfType<FlagsAttribute>().Any())
        {
            IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            var control = new FlagsEditorControl(svc, e);
            svc.DropDownControl(control);
            return control.Value;
        }
        else
            return base.EditValue(context, provider, value);
    }
}
