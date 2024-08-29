using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms.Design;

namespace ScintillaNET
{
    internal class ScintillaDesigner : ControlDesigner
    {
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            var scrollWidthTracking = (PropertyDescriptor)properties[nameof(Scintilla.ScrollWidthTracking)];
            scrollWidthTracking = TypeDescriptor.CreateProperty(
                scrollWidthTracking.ComponentType,
                scrollWidthTracking,
                scrollWidthTracking.Attributes.Cast<Attribute>().Concat(new Attribute[] { new RefreshPropertiesAttribute(RefreshProperties.All) }).ToArray()
            );
            properties[nameof(Scintilla.ScrollWidthTracking)] = scrollWidthTracking;

            var scrollWidth = (PropertyDescriptor)properties[nameof(Scintilla.ScrollWidth)];
            if ((bool)scrollWidthTracking.GetValue(Component))
            {
                scrollWidth = TypeDescriptor.CreateProperty(
                    scrollWidth.ComponentType,
                    scrollWidth,
                    scrollWidth.Attributes.Cast<Attribute>().Concat(
                        new Attribute[] {
                            new BrowsableAttribute(false),
                            new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden),
                        }
                    ).ToArray()
                );
                properties[nameof(Scintilla.ScrollWidth)] = scrollWidth;
            }
        }
    }
}
