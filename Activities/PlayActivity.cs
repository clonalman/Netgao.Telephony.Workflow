using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.IO;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Netgao.Telephony.Workflow.Activities
{
    using Netgao.Telephony.Workflow.Design;

    [ToolboxItem(typeof(ActivityToolboxItem))]
    [ToolboxBitmap(typeof(PlayActivity), "Resources.PlayActivity.png")]
    public partial class PlayActivity : CallMethodActivity
	{
        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(PlayActivity));
        public static readonly DependencyProperty StopOnDTMFProperty = DependencyProperty.Register("StopOnDTMF", typeof(bool), typeof(PlayActivity));

        public PlayActivity()
		{
			InitializeComponent();

            base.InterfaceType = typeof(ITerminalService);
            base.MethodName = "Play";
            base.Description = "播放语音";
		}

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Type InterfaceType
        {
            get { return base.InterfaceType; }
            set { throw new InvalidOperationException("Cannot set InterfaceType on a derived CallExternalMethodActivity."); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string MethodName
        {
            get { return base.MethodName; }
            set { throw new InvalidOperationException("Cannot set EventName on a derived CallExternalMethodActivity."); }
        }

        [DefaultValue("")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Editor(typeof(FileDialogEditor), typeof(UITypeEditor))]
        [FileDialogEditor.FileDialog("All Files (*.wav)|*.wav")]
        [SccCategory("Activity_Property")]
        public string FileName
        {
            get { return (string)base.GetValue(FileNameProperty); }
            set { base.SetValue(FileNameProperty, value); }
        }

        [DefaultValue(false)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        public bool StopOnDTMF
        {
            get { return (bool)base.GetValue(StopOnDTMFProperty); }
            set { base.SetValue(StopOnDTMFProperty, value); }
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);
            ParameterBindings["uniqueId"].Value = WorkflowInstanceId; // Channel.UniqueId
            ParameterBindings["fileName"].Value = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName.Replace("/", @"\"));
            ParameterBindings["stopOnDTMF"].Value = StopOnDTMF;
        }
	}
}
