using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class PickupEventArgs : TerminalEventArgs
	{

        public PickupEventArgs(ITerminalInstance instance)
            : base(instance)
        {

        }

        public override string ToString()
        {
            return String.Format("Pickup({0})", Channel.Pad);
        }
	}
}
