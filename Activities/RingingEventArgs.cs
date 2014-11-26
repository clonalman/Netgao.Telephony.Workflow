using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class RingingEventArgs : TerminalEventArgs
    {
        public RingingEventArgs(ITerminalInstance instance, int ringCount)
            : base(instance)
        {
            Channel.SetVariable("RingCount", ringCount);
        }

        public int RingCount
        {
            get { return Channel.GetVariable<int>("RingCount"); }
        }

        public override string ToString()
        {
            return String.Format("OnRinging({0}, {1})", Channel.Pad, RingCount);
        }
    }
}
