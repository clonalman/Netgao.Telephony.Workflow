using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class SendToneEventArgs : TerminalEventArgs
    {

        public SendToneEventArgs(ITerminalInstance instance, int toneType, int delay)
            : base(instance)
        {
            Channel.SetVariable("ToneType", toneType);
            Channel.SetVariable("Delay", delay);
        }

        public int ToneType
        {
            get { return Channel.GetVariable<int>("ToneType"); }
        }

        public int Delay
        {
            get { return Channel.GetVariable<int>("Delay"); }
        }

        public override string ToString()
        {
            return String.Format("SendTone({0}, {1}, {2})", Channel.Pad, ToneType, Delay);
        }
    }

    public enum ToneType : int
    {
        DialTone = 0,
        BusyTone = 1,
        RingbackTone = 2,
        HowlerTone = 3
    }
}
