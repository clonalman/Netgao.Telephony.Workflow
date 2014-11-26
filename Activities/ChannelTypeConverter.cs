using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

namespace Netgao.Telephony.Workflow.Activities
{
    internal class ChannelTypeConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context == null) return null;

            List<string> list = new List<string>();
            var descriptors = TypeDescriptor.GetProperties(typeof(ITerminalInstance));
            foreach (var descriptor in descriptors.OfType<PropertyDescriptor>())
            {
                if (typeof(ITerminalInstance).IsAssignableFrom(descriptor.ComponentType))
                {
                    list.Add(descriptor.Name);
                }
            }
            list.Sort();
            return new StandardValuesCollection(list);

        }
    }

}
