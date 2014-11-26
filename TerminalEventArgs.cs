using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Transactions;
using System.Linq;
using System.Threading;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;

namespace Netgao.Telephony.Workflow
{
    [Serializable]
    public class TerminalEventArgs : ExternalDataEventArgs
	{
        public TerminalEventArgs(ITerminalInstance channel)
            : base(channel.UniqueId)
        {
            this.Channel = channel;
            this.WaitForIdle = true;
        }

        public ITerminalInstance Channel
        {
            get;
            private set;
        }
	}
}
