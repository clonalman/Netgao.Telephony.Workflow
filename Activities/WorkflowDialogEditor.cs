using System;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Serialization;

namespace Netgao.Telephony.Workflow.Activities
{
    internal class WorkflowDialogEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null&& context.Instance != null)
			{
				if (!context.PropertyDescriptor.IsReadOnly)
				{
					return UITypeEditorEditStyle.Modal;
				}
			}
			return UITypeEditorEditStyle.None;
		}
		
		[RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (context == null || provider == null || context.Instance == null)
			{
				return base.EditValue(provider, value);
			}


            using (FileDialog fileDlg = CreateFileDialog(context))
            {
                string defaultPath = AppDomain.CurrentDomain.BaseDirectory;

                fileDlg.Title = "Select " + context.PropertyDescriptor.DisplayName;
                if (value != null && value.ToString() != String.Empty)
                {
                    fileDlg.FileName = Path.Combine(defaultPath, (string)value).Replace("/", "\\");
                }
                fileDlg.InitialDirectory = defaultPath;
                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    using (XmlReader reader = new XmlTextReader(fileDlg.FileName))
                    {
                        try
                        {
                            WorkflowMarkupSerializer xomlSerializer = new WorkflowMarkupSerializer();
                            Activity rootActivity = xomlSerializer.Deserialize(reader) as Activity;
                            if (rootActivity != null)
                            {
                                value = rootActivity.GetValue(WorkflowMarkupSerializer.XClassProperty) + ", Netgao.Telephony.Ivr";  
                                //value = String.Format("\"{x:Type {0}\"}", rootActivity.GetValue(WorkflowMarkupSerializer.XClassProperty));
                            }
                        }
                        catch 
                        {
                            value = null;
                        }
                    }

                }
            }
			return value;
		}

        private FileDialog CreateFileDialog(ITypeDescriptorContext context)
        {
            FileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "All Files (*.xoml)|*.xoml";
            return fileDlg;
        }
		
		
	}
}
