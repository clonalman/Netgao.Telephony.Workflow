using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class TerminatedEventArgs : TerminalEventArgs
    {
        public TerminatedEventArgs(ITerminalInstance instance, int reason)
            : base(instance)
        {
            Channel.SetVariable("Reason", reason);
        }

        public int Reason
        {
            get { return Channel.GetVariable<int>("Reason"); }
        }

        public override string ToString()
        {
            return String.Format("OnTerminated({0}, {1})", Channel.Pad, Reason);
        }
    }
}
