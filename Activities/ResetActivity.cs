using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
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
    [ToolboxBitmap(typeof(ResetActivity), "Resources.ResetActivity.png")]
    public partial class ResetActivity : CallMethodActivity
	{
        private static readonly DependencyProperty VariableProperty = DependencyProperty.Register("Variable", typeof(string), typeof(ResetActivity));
        private static readonly DependencyProperty AssignmentProperty = DependencyProperty.Register("Assignment", typeof(RuleExpression), typeof(ResetActivity));
        

        public ResetActivity()
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

        [SccCategory("Activity_Property")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [TypeConverter(typeof(ChannelTypeConverter))]
        [ValidationOption(ValidationOption.Required)]
        public virtual string Variable
        {
            get { return (string)base.GetValue(VariableProperty); }
            set { base.SetValue(VariableProperty, value); }
        }

        [SccCategory("Activity_Property")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Required)]
        public virtual RuleExpression Assignment
        {
            get { return base.GetValue(AssignmentProperty) as RuleExpression; }
            set { base.SetValue(AssignmentProperty, value); }
        }

        public bool Assign(object value)
        {
            if (Channel != null)
            {
                Channel.SetVariable(Variable, value);
                return true;
            }
            else return false;
        }

        protected override void OnMethodInvoking(EventArgs e)
        {
            base.OnMethodInvoking(e);

            if (Channel != null)
            {
                if (Assignment.Evaluate(this, null))
                {
                    ParameterBindings["lineId"].Value = WorkflowInstanceId; // Channel.LineId
                    ParameterBindings["name"].Value = Variable;
                    ParameterBindings["value"].Value = Channel.GetVariable<object>(Variable);
                }
            }
        }
    }
}
