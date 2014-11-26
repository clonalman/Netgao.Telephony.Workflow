using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Call.Core.Activities
{
    [Serializable]
    public sealed class DialEventArgs : TerminalEventArgs
    {
        public DialEventArgs(ITerminalInstance instance, string dtmf, int length)
            : base(instance)
        {
            Channel.SetVariable("DTMF", dtmf);
            Channel.SetVariable("Length", length);
        }

        public string DTMF
        {
            get { return Channel.GetVariable<string>("DTMF"); }
        }

        public int Length
        {
            get { return Channel.GetVariable<int>("Length"); }
        }

        public override string ToString()
        {
            return String.Format("Dial({0}, \"{1}\", {2})", Channel.Pad, DTMF, Length);
        }
    }
}
