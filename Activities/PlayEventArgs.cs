using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class PlayEventArgs : TerminalEventArgs
	{
        public PlayEventArgs(ITerminalInstance instance, string fileName, bool stopOnDTMF)
            : base(instance)
        {
            Channel.SetVariable("FileName", fileName);
            Channel.SetVariable("StopOnDTMF", stopOnDTMF);
        }

        public string FileName
        {
            get { return Channel.GetVariable<string>("FileName"); }
        }

        public bool StopOnDTMF
        {
            get { return Channel.GetVariable<bool>("StopOnDTMF"); }
        }

        public override string ToString()
        {
            return String.Format("Play({0}, \"{1}\",{2})", Channel.Pad, FileName, StopOnDTMF);
        }
	}
}
