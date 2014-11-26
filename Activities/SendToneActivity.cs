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
    [ToolboxBitmap(typeof(SendToneActivity), "Resources.SendToneActivity.png")]
    public partial class SendToneActivity : CallMethodActivity
	{
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(ToneType), typeof(SendToneActivity));
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(SendToneActivity));

        public SendToneActivity()
		{
			InitializeComponent();

            base.InterfaceType = typeof(ITerminalService);
            base.MethodName = "SendTone";
            base.Description = "发信号音";
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

        [DefaultValue(ToneType.DialTone)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        public ToneType ToneType
        {
            get { return (ToneType)base.GetValue(TypeProperty); }
            set { base.SetValue(TypeProperty, value); }
        }

        [DefaultValue(0)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        public int Delay
        {
            get { return (int)base.GetValue(DelayProperty); }
            set { base.SetValue(DelayProperty, value); }
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);
            ParameterBindings["uniqueId"].Value = WorkflowInstanceId; // Channel.UniqueId
            ParameterBindings["toneType"].Value = ToneType;
            ParameterBindings["delay"].Value = Delay;
        }

        
	}
}
