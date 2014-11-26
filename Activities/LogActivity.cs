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
    [ToolboxBitmap(typeof(LogActivity), "Resources.LogActivity.png")]
    public partial class LogActivity : CallMethodActivity
	{
        public static readonly DependencyProperty LogNameProperty = DependencyProperty.Register("LogName", typeof(string), typeof(LogActivity));

        public LogActivity()
		{
			InitializeComponent();
            base.InterfaceType = typeof(ITerminalService);
            base.MethodName = "Log";
            base.Description = "日志";
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

        [SccCategory("Activity_Property")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string LogName
        {
            get { return (string)base.GetValue(LogNameProperty); }
            set { base.SetValue(LogNameProperty, value); }
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);
            ParameterBindings["uniqueId"].Value = WorkflowInstanceId; // Channel.UniqueId
            ParameterBindings["logName"].Value = LogName;
        }
	}
}
