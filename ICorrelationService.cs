using System;
using System.Collections.Generic;
using System.Workflow;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow
{
    [ExternalDataExchange]
    [CorrelationParameter("key")]
    public interface ICorrelationService
    {
        [CorrelationAlias("key", "e.Key")]
        event EventHandler<CorrelationServiceArgs> ChildCompleted;

        [CorrelationInitializer]
        void InitializeCorrelation(string key);

        [CorrelationInitializer]
        void OnChildCompleted(string key);
    }


}
