using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class HangupEventArgs : TerminalEventArgs
	{
        public HangupEventArgs(ITerminalInstance instance)
            : base(instance)
        {

        }

        public override string ToString()
        {
            return String.Format("Hangup({0})", Channel.Pad);
        }
	}
}
