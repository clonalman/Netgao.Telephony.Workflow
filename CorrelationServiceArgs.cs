using System;
using System.Collections.Generic;
using System.Workflow.Activities;


namespace Netgao.Telephony.Workflow
{
    [Serializable]
    public class CorrelationServiceArgs : ExternalDataEventArgs
    {
        public CorrelationServiceArgs(Guid instanceId, string key)
            : base(instanceId)
        {
            this.Key = key;
        }

        public string Key
        {
            get;
            private set;
        }
    }
}
