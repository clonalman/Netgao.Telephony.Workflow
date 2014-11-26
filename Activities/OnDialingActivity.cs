using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
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
    [ToolboxBitmap(typeof(OnDialingActivity), "Resources.OnDialingActivity.png")]
    public sealed partial class OnDialingActivity : OnStartedActivity
	{
        public static readonly DependencyProperty DtmfStringProperty = DependencyProperty.Register("DtmfString", typeof(string), typeof(OnDialingActivity));
        
        public OnDialingActivity()
		{
			InitializeComponent();
            base.Description = "接收按键";
		}

        [Browsable(true)]
        [SccCategory("Activity_Rules")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override RuleExpression Rules
        {
            get { return base.Rules; }
            set { base.Rules = value; }
        }

        [DefaultValue((string)null)]
        [Browsable(true)]
        [SccCategory("Activity_Property")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DtmfString
        {
            get { return (string)base.GetValue(DtmfStringProperty); }
        }

        protected override void InitializeArguments(object[] args)
        {
            base.InitializeArguments(args);
            base.SetValue(DtmfStringProperty, DtmfString);
        }
    }
}
