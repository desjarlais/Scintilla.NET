using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

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
            List<Enum> enums = [];
            var items = Enum.GetValues(enumType).Cast<Enum>()
                .Select(e => new { @enum = e, bits = Convert.ToUInt64(e), bitCount = Helpers.PopCount(Convert.ToUInt64(e)) })
                .Where(e => e.bits != 0 && e.bitCount > 0)
                .ToList();
            int maxIterations = items.Count;
            for (int i = 0; i < maxIterations && bits != valueBits && items.Count > 0; i++)
            {
                int maxIndex = items
                    .Select(e => new { contrib = Helpers.PopCount(e.bits & valueBits & ~bits), item = e })
                    .MaxIndex(
                        (x, y) =>                                                                    // Select one that:
                        (x.contrib != y.contrib) ? (x.contrib - y.contrib) :                         // Contributes most bits
                        (x.item.bitCount != y.item.bitCount) ? (y.item.bitCount - x.item.bitCount) : // Has least amount of bits set
                        (x.item.bits < y.item.bits) ? -1 : (x.item.bits > y.item.bits ? 1 : 0)       // With the highest integer value
                    );

                var item = items[maxIndex];

                if ((valueBits & item.bits) == item.bits && (bits & item.bits) != item.bits)
                {
                    bits |= item.bits;
                    items.RemoveAt(maxIndex);
                    enums.Add(item.@enum);
                }
            }

            enums.Sort();
            return string.Join(" | ", enums);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string str)
        {
            Type t = context.PropertyDescriptor.PropertyType;
            ulong bits = 0;
            IEnumerable<string> nameList = str.Split('|').Select(x => x.Trim());
            foreach (string name in nameList)
            {
                bits |= Convert.ToUInt64(Enum.Parse(t, name));
            }

            return Enum.ToObject(t, bits);
        }

        return base.ConvertFrom(context, culture, value);
    }
}
