using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class TalkEventArgs : TerminalEventArgs
    {
        public TalkEventArgs(ITerminalInstance instance, int volume, bool recording)
            : base(instance)
        {
            Channel.SetVariable("Volume", volume);
            Channel.SetVariable("Recording", recording);
        }

        public int Volume
        {
            get { return Channel.GetVariable<int>("Volume"); }
        }

        public bool Recording
        {
            get { return Channel.GetVariable<bool>("Recording"); }
        }

        public override string ToString()
        {
            return String.Format("Talk({0}, {1},{2})", Channel.Pad, Volume, Recording);
        }
    }
}
