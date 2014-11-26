using System;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Netgao.Telephony.Workflow.Design
{
    internal class FileDialogEditor : UITypeEditor
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
                    if (fileDlg.FileName.StartsWith(defaultPath))
                    {
                        value = fileDlg.FileName.Replace(defaultPath, "").Replace("\\", "/");
                    }
                    else
                    {
                        value = fileDlg.FileName;
                    }
                }
            }
			return value;
		}

        private FileDialog CreateFileDialog(ITypeDescriptorContext context)
        {
            FileDialog fileDlg;
            FileDialogAttribute filterAtt = (FileDialogAttribute)context.PropertyDescriptor.Attributes[typeof(FileDialogAttribute)];
            if (filterAtt != null)
            {
                switch (filterAtt.Type)
                {
                    default:
                    case FileDialogType.OpenFileDialog:
                        {
                            fileDlg = new OpenFileDialog();
                        } break;
                    case FileDialogType.SaveFileDialog:
                        {
                            fileDlg = new SaveFileDialog();
                        } break;
                }
                fileDlg.Filter = filterAtt.Filter;
            }
            else
            {
                fileDlg = new OpenFileDialog();
            }
        
            return fileDlg;
        }
		
		[AttributeUsage(AttributeTargets.Property)]
        public class FileDialogAttribute : Attribute
		{
            public FileDialogAttribute(string filter)
                : this(filter, FileDialogType.OpenFileDialog)
            {

            }
            public FileDialogAttribute(string filter, FileDialogType type)
			{
                this.Filter = filter;
                this.Type = type;
			}

            public string Filter
            {
                get;
                private set;
            }

            public FileDialogType Type
            {
                get;
                private set;
            }
		}

        public enum FileDialogType
        {
            OpenFileDialog,
            SaveFileDialog
        }
	}
}
