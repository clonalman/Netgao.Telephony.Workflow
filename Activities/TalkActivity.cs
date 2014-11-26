using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Netgao.Telephony.Workflow.Activities
{
    [ToolboxItem(typeof(ActivityToolboxItem))]
    [ToolboxBitmap(typeof(TalkActivity), "Resources.TalkActivity.png")]
    public partial class TalkActivity : CallExternalMethodActivity
	{
        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(int), typeof(TalkActivity));
        public static readonly DependencyProperty RecordingProperty = DependencyProperty.Register("Recording", typeof(bool), typeof(TalkActivity));

        public TalkActivity()
		{
			InitializeComponent();

            base.InterfaceType = typeof(ITerminalService);
            base.MethodName = "Talk";
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

        [DefaultValue(0)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        public int Volume
        {
            get { return (int)base.GetValue(VolumeProperty); }
            set { base.SetValue(VolumeProperty, value); }
        }

        [DefaultValue(false)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        public bool Recording
        {
            get { return (bool)base.GetValue(RecordingProperty); }
            set { base.SetValue(RecordingProperty, value); }
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);
            ParameterBindings["uniqueId"].Value = WorkflowInstanceId;
            ParameterBindings["volume"].Value = Volume;
            ParameterBindings["recording"].Value = Recording;
        }
	}
}
