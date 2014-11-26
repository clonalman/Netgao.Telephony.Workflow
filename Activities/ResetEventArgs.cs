using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;

namespace Call.Core.Activities
{
    [Serializable]
    public sealed class ResetEventArgs : ChannelEventArgs
	{
        public ResetEventArgs(IChannelInstance instance, string name, object value)
            : base(instance)
        {
            Channel.SetVariable("Name", name);
            Channel.SetVariable("Value", value);
        }

        public string Name
        {
            get { return Channel.GetVariable<string>("Name"); }
        }

        public object Value
        {
            get { return Channel.GetVariable<object>("Value"); }
        }

        public override string ToString()
        {
            return String.Format("{0}->Reset(\"{1}\", {2})", Channel.LinePad, Name, Value);
        }
	}
}
