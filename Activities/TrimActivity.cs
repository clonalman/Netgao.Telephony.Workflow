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
    [ToolboxBitmap(typeof(TrimActivity), "Resources.TrimActivity.png")]
    public partial class TrimActivity : CallMethodActivity
	{
        public static readonly DependencyProperty TrimCharsProperty = DependencyProperty.Register("TrimChars", typeof(string), typeof(TrimActivity));

        public TrimActivity()
		{
			InitializeComponent();
            base.InterfaceType = typeof(IChannelService);
            base.MethodName = "Reset";
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
        [SccCategory("Activity_Property")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TrimChars
        {
            get { return (string)base.GetValue(TrimCharsProperty); }
            set { base.SetValue(TrimCharsProperty, value); }
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);

            if (Channel != null && !String.IsNullOrEmpty(Channel.DtmfString))
            {
                ParameterBindings["lineId"].Value = WorkflowInstanceId; // Channel.LineId
                ParameterBindings["name"].Value = "DtmfString";
                ParameterBindings["value"].Value = Channel.DtmfString.Trim(TrimChars.ToCharArray());
            }
            else
            {
                ParameterBindings["lineId"].Value = WorkflowInstanceId; // Channel.LineId
                ParameterBindings["name"].Value = "DtmfString";
                ParameterBindings["value"].Value = String.Empty;
            }
        }

	}
}
