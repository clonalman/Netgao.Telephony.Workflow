using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class ClearEventArgs : TerminalEventArgs
	{
        public ClearEventArgs(ITerminalInstance instance)
            : base(instance)
        {
            
        }

        public override string ToString()
        {
            return String.Format("Clear({0})", Channel.Pad);
        }
	}
}
