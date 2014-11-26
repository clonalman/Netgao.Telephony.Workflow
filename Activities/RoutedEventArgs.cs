using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class RoutedEventArgs : TerminalEventArgs
    {
        public RoutedEventArgs(ITerminalInstance instance, int result, string calleeId)
            : base(instance)
        {
            Channel.SetVariable("Result", result);
            Channel.SetVariable("CalleeId", calleeId);
        }


        public int Result
        {
            get { return Channel.GetVariable<int>("Result"); }
        }

        public string CalleeId
        {
            get { return Channel.GetVariable<string>("CalleeId"); }
        }

        public override string ToString()
        {
            return String.Format("{0}->OnRouted({1})", Channel.Pad, Result);
        }
    }
}
