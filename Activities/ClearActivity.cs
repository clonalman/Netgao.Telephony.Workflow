using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Threading;
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
    [ToolboxBitmap(typeof(ClearActivity), "Resources.ClearActivity.png")]
    public partial class ClearActivity : CallMethodActivity
	{
        public ClearActivity()
		{
			InitializeComponent();
            base.InterfaceType = typeof(ITerminalService);
            base.MethodName = "Clear";
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

        protected override void OnActivityExecutionContextLoad(IServiceProvider provider)
        {
            base.OnActivityExecutionContextLoad(provider);

            base.MethodInvoking += (sender, e) =>
            {
                WorkflowQueuingService queuingService = provider.GetService(typeof(WorkflowQueuingService)) as WorkflowQueuingService;
                if (queuingService != null)
                {
                    Array.ForEach(new IComparable[] 
                    {
                        new EventQueueName(typeof(ITerminalService), "Started"),
                        new EventQueueName(typeof(ITerminalService), "Completed")
                    },
                    (queueName) =>
                    {
                        if (queuingService.Exists(queueName))
                        {
                            Thread.Sleep(50);
                            WorkflowQueue workflowQueue = queuingService.GetWorkflowQueue(queueName);
                            while (workflowQueue.Count > 0) workflowQueue.Dequeue();
                        }
                    });
                }
            };
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);

            ParameterBindings["uniqueId"].Value = WorkflowInstanceId; // Channel.UniqueId
        }

	}
}
