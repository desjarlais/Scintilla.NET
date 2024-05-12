using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ScintillaNET;

internal class FlagsConverter : TypeConverter
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
        if (value is Enum valueEnum && destinationType == typeof(string))
        {
            Type enumType = valueEnum.GetType();
            ulong valueBits = Convert.ToUInt64(valueEnum);
            if (valueBits == 0)
            {
                return Enum.ToObject(enumType, 0).ToString();
            }
            ulong bits = 0;
            StringBuilder sb = new StringBuilder();
            void Add(Enum item, ulong itemBits)
            {
                if (itemBits != 0 && valueEnum.HasFlag(item))
                {
                    bits |= itemBits;
                    if (sb.Length > 0)
                        sb.Append(" | ");
                    sb.Append(item);
                }
            }
            foreach (Enum item in Enum.GetValues(enumType))
            {
                ulong itemBits = Convert.ToUInt64(item);
                if (Helpers.PopCount(itemBits) == 1)
                    Add(item, itemBits);
            }
            foreach (Enum item in Enum.GetValues(enumType))
            {
                ulong itemBits = Convert.ToUInt64(item);
                if (Helpers.PopCount(itemBits) > 1 && (bits & itemBits) != itemBits)
                    Add(item, itemBits);
            }
            return sb.ToString();
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string str)
        {
            Type t = context.PropertyDescriptor.PropertyType;
            ulong bits = 0;
            var nameList = str.Split('|').Select(x => x.Trim());
            foreach (var name in nameList)
            {
                bits |= Convert.ToUInt64(Enum.Parse(t, name));
            }
            return Enum.ToObject(t, bits);
        }
        return base.ConvertFrom(context, culture, value);
    }
}
