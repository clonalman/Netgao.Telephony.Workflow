using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class QueueExEventArgs : TerminalEventArgs
	{

        public QueueExEventArgs(ITerminalInstance instance, int priority)
            : base(instance)
        {
            Channel.SetVariable("Priority", priority);
        }

        public int Priority
        {
            get { return Channel.GetVariable<int>("Priority"); }
        }

        public override string ToString()
        {
            return String.Format("{0}->Queue({1})", Channel.Pad, Priority);
        }
	}
}
