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
    [ToolboxBitmap(typeof(RouteActivity), "Resources.RouteActivity.png")]
    public partial class RouteActivity : CallMethodActivity
	{
        public static readonly DependencyProperty RoutingProperty = DependencyProperty.Register("Routing", typeof(RouteType), typeof(RouteActivity));
        public static readonly DependencyProperty CalleeIdProperty = DependencyProperty.Register("CalleeId", typeof(string), typeof(RouteActivity));
        
        public RouteActivity()
		{
			InitializeComponent();

            base.InterfaceType = typeof(IChannelService);
            base.MethodName = "Route";
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
        [DefaultValue(RouteType.Random)]
        [SsmCategory("Activity_Property")]
        public RouteType Routing
        {
            get { return (RouteType)base.GetValue(RoutingProperty); }
            set { base.SetValue(RoutingProperty, value); }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SsmCategory("Activity_Property")]
        public string CalleeId
        {
            get { return (string)base.GetValue(CalleeIdProperty); }
            set { base.SetValue(CalleeIdProperty, value); }
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);
            ParameterBindings["lineId"].Value = WorkflowInstanceId; // Channel.LineId
            ParameterBindings["calleeId"].Value = CalleeId;
            ParameterBindings["routing"].Value = Routing;
        }
	}
}
