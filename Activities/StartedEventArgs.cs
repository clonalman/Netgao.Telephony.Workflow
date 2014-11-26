using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class StartedEventArgs : TerminalEventArgs
    {
        public StartedEventArgs(ITerminalInstance instance)
            : base(instance)
        {
            
        }

        public override string ToString()
        {
            return String.Format("OnStart({0})", Channel.Pad);
        }
    }
}
