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
    [ToolboxBitmap(typeof(DebugActivity), "Resources.DebugActivity.png")]
    public partial class DebugActivity : Activity
	{
        public static readonly DependencyProperty ChannelProperty = DependencyProperty.Register("Channel", typeof(ITerminalInstance), typeof(DebugActivity));
        public static readonly DependencyProperty VariableProperty = DependencyProperty.Register("Variable", typeof(string), typeof(DebugActivity));

        public DebugActivity()
		{
			InitializeComponent();
		}

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [SccCategory("Activity_Property")]
        public virtual ITerminalInstance Channel
        {
            get { return base.GetValue(ChannelProperty) as ITerminalInstance; }
            set { base.SetValue(ChannelProperty, value); }
        }

        [SccCategory("Activity_Property")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [TypeConverter(typeof(ChannelTypeConverter))]
        public virtual string Variable
        {
            get { return (string)base.GetValue(VariableProperty); }
            set { base.SetValue(VariableProperty, value); }
        }

        protected override void InitializeProperties()
        {
            base.InitializeProperties();

            if (!base.IsBindingSet(ChannelProperty))
            {
                OnStartedActivity started = Parent.EnabledActivities.OfType<OnStartedActivity>()
                                                   .LastOrDefault(p => Parent.EnabledActivities.IndexOf(p) < Parent.EnabledActivities.IndexOf(this));
                if (started != null)
                {
                    base.SetBinding(ChannelProperty, new ActivityBind(started.QualifiedName, "Channel"));
                }
            }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (Channel != null)
            {
                UccLogWatcher.Trace("Channel: UniqueId={0}, LineNumber={1}, Pad={2}, DtmfString={3}", 
                    Channel.UniqueId, Channel.Number, Channel.Pad, Channel.DtmfString);
            }
            return base.Execute(executionContext);


        }
	}
}
