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
    [ToolboxBitmap(typeof(QueueActivity), "Resources.QueueActivity.png")]
    public partial class QueueActivity : CallMethodActivity
	{
        public static readonly DependencyProperty PriorityProperty = DependencyProperty.Register("Priority", typeof(int), typeof(QueueActivity));

        public QueueActivity()
		{
			InitializeComponent();

            base.InterfaceType = typeof(ITerminalService);
            base.MethodName = "Queue";
            base.Description = "排队";
		}

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Type InterfaceType
        {
            get { return base.InterfaceType; }
            set { throw new InvalidOperationException("Cannot set InterfaceType on a derived CallMethodActivity."); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string MethodName
        {
            get { return base.MethodName; }
            set { throw new InvalidOperationException("Cannot set EventName on a derived CallMethodActivity."); }
        }

        [DefaultValue(0)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        public int Priority
        {
            get { return (int)base.GetValue(PriorityProperty); }
            set { base.SetValue(PriorityProperty, value); }
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);

            ParameterBindings["uniqueId"].Value = WorkflowInstanceId; // Channel.UniqueId
            ParameterBindings["priority"].Value = Priority; 
        }
	}
}
