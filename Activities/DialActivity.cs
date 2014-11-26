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

namespace Call.Core.Activities
{
    [ToolboxItem(typeof(ActivityToolboxItem))]
    [ToolboxBitmap(typeof(DialActivity), "Resources.DTMFActivity.png")]
    public partial class DialActivity : CallMethodActivity
	{
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(int), typeof(DialActivity));
        public static readonly DependencyProperty DTMFProperty = DependencyProperty.Register("DTMF", typeof(string), typeof(DialActivity));

        public DialActivity()
		{
			InitializeComponent();

            base.InterfaceType = typeof(ITerminalService);
            base.MethodName = "Dial";
            base.Description = "拨号";
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
        [SccCategory("Activity_Property")]
        public string DTMF
        {
            get { return (string)base.GetValue(DTMFProperty); }
            set { base.SetValue(DTMFProperty, value); }
        }

        [DefaultValue(0xFFFF)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        public int Length
        {
            get { return (int)base.GetValue(LengthProperty); }
            set { base.SetValue(LengthProperty, value); }
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);

            ParameterBindings["uniqueId"].Value = WorkflowInstanceId; // Channel.UniqueId
            ParameterBindings["length"].Value = Length;
            ParameterBindings["dtmf"].Value = DTMF;
        }
	}
}
