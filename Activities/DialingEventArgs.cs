using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class DialingEventArgs : TerminalEventArgs
	{
        public DialingEventArgs(ITerminalInstance instance, string dtmfString)
            : base(instance)
        {
            Channel.SetVariable("DtmfString", dtmfString);
        }

        public string DtmfString
        {
            get { return Channel.GetVariable<string>("DtmfString"); }
        }

        public override string ToString()
        {
            return String.Format("OnDialing({0}, \"{1}\")", Channel.Pad, DtmfString);
        }
	}
}
