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
    [ToolboxBitmap(typeof(BlindTransferActivity), "Resources.BlindTransferActivity.png")]
    public partial class BlindTransferActivity : CallMethodActivity
	{
        public static readonly DependencyProperty CalleeIdProperty = DependencyProperty.Register("CalleeId", typeof(string), typeof(BlindTransferActivity));
        
        public BlindTransferActivity()
		{
			InitializeComponent();

            base.InterfaceType = typeof(ITerminalService);
            base.MethodName = "BlindTransfer";
            base.Description = "路由";
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

        

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        public string CalleeId
        {
            get { return (string)base.GetValue(CalleeIdProperty); }
            set { base.SetValue(CalleeIdProperty, value); }
        }


        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);
            ParameterBindings["uniqueId"].Value = WorkflowInstanceId; // Channel.UniqueId
            ParameterBindings["calleeId"].Value = CalleeId;
        }
	}
}
