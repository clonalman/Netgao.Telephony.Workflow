using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Serialization;

namespace Netgao.Telephony.Workflow.Activities
{
    internal class WorkflowConverter : StringConverter
	{
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //true means show a combobox
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            //true will limit to list. false will show the list, 
            //but allow free-form entry
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection
               GetStandardValues(ITypeDescriptorContext context)
        {
            ArrayList values = new ArrayList();

            DirectoryInfo info = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workflows"));

            foreach (FileInfo fileInfo in info.GetFiles("*.xoml"))
            {
                using (XmlReader reader = new XmlTextReader(fileInfo.FullName))
                {
                    try
                    {
                        WorkflowMarkupSerializer xomlSerializer = new WorkflowMarkupSerializer();
                        Activity rootActivity = xomlSerializer.Deserialize(reader) as Activity;
                        if (rootActivity != null)
                        {
                            values.Add(rootActivity.GetValue(WorkflowMarkupSerializer.XClassProperty) + ", Netgao.Telephony.Ivr");
                        }
                    }
                    catch
                    {
                        /* do nothing */
                    }
                }
            }

            return new StandardValuesCollection(values);
        }

	}
}
