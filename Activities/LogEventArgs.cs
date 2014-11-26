using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class LogEventArgs : TerminalEventArgs
    {
        public LogEventArgs(ITerminalInstance instance, string logName)
            : base(instance)
        {
            Channel.SetVariable("LogName", logName);
        }

        public string LogName
        {
            get { return Channel.GetVariable<string>("LogName"); }
        }


        public override string ToString()
        {
            return String.Format("Log({0}, \"{1}\")", Channel.Pad, LogName);
        }
    }
}
