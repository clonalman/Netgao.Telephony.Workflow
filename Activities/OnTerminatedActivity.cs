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
    [ToolboxBitmap(typeof(OnTerminatedActivity), "Resources.OnTerminatedActivity.png")]
    public partial class OnTerminatedActivity : HandleEventActivity
	{
        //public static readonly DependencyProperty ExceptionProperty = DependencyProperty.Register("Exception", typeof(Exception), typeof(TerminatedActivity));

        public OnTerminatedActivity()
		{
			InitializeComponent();
            base.EventName = "Terminated";
            base.Description = "终止";
		}

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string EventName
        {
            get { return base.EventName; }
            set { throw new InvalidOperationException("Cannot set EventName on a derived HandleExternalEventActivity."); }
        }

        //[DefaultValue(null)]
        //[Browsable(true)]
        //[SccCategory("Activity_Property")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public Exception Exception
        //{
        //    get { return (Exception)base.GetValue(ExceptionProperty); }
        //}
	}
}
