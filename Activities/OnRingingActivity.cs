using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Linq.Expressions;
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
    [ToolboxBitmap(typeof(OnRingingActivity), "Resources.OnRingingActivity.png")]
    public sealed partial class OnRingingActivity : OnStartedActivity
	{
        public static readonly DependencyProperty RingCountProperty = DependencyProperty.Register("RingCount", typeof(int), typeof(OnRingingActivity));

        public OnRingingActivity()
		{
			InitializeComponent();
            base.Description = "振铃事件";
		}

        [Browsable(true)]
        [SccCategory("Activity_Rules")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override RuleExpression Rules
        {
            get { return base.Rules; }
            set { base.Rules = value; }
        }

        [DefaultValue(null)]
        [Browsable(true)]
        [SccCategory("Activity_Property")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RingCount
        {
            get { return (int)base.GetValue(RingCountProperty); }
        }

        protected override void OnInvoked(EventArgs e)
        {
            base.OnInvoked(e);

            RingingEventArgs args = e as RingingEventArgs;
            if (args != null)
            {
                base.SetValue(RingCountProperty, args.RingCount);
            }
        }
	}
}
