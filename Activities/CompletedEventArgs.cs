using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class CompletedEventArgs : TerminalEventArgs
    {
        public CompletedEventArgs(ITerminalInstance instance, int result)
            : base(instance)
        {
            Channel.SetVariable("Result", result);
        }


        public int Result
        {
            get { return Channel.GetVariable<int>("Result"); }
        }

        public override string ToString()
        {
            return String.Format("OnCompleted({0}, {1})", Channel.Pad, Result);
        }
    }
}
