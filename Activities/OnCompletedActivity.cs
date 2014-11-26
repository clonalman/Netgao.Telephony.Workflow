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
    [ToolboxBitmap(typeof(OnCompletedActivity), "Resources.OnCompletedActivity.png")]
    public partial class OnCompletedActivity : HandleEventActivity
	{
        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(int), typeof(OnCompletedActivity));

        public OnCompletedActivity()
		{
			InitializeComponent();
            base.EventName = "Completed";
            base.Description = "完成";
		}

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string EventName
        {
            get { return base.EventName; }
            set { throw new InvalidOperationException("Cannot set EventName on a derived HandleExternalEventActivity."); }
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [SccCategory("Activity_Property")]
        public int Result
        {
            get { return (int)base.GetValue(ResultProperty); }
        }
    }
}
