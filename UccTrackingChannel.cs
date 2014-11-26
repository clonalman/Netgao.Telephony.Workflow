using System;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Tracking;

namespace Netgao.Telephony.Workflow
{
    public class UccTrackingChannel : TrackingChannel
    {
        private TrackingParameters parameters;

        public UccTrackingChannel(TrackingParameters parameters)
            : base()
        {
            this.parameters = parameters;
        }

        protected override void Send(TrackingRecord record)
        {
            ActivityTrackingRecord r1 = record as ActivityTrackingRecord;
            WorkflowTrackingRecord r2 = record as WorkflowTrackingRecord;
            UserTrackingRecord r3 = record as UserTrackingRecord;

            if (r1 != null)
            {
                UccLogWatcher.Trace(r1.QualifiedName + " (" + r1.ActivityType.Name + ") -> " + r1.ExecutionStatus);
            }
            else if (r2 != null)
            {
                UccLogWatcher.Trace(r2.TrackingWorkflowEvent.ToString());
            }
            else if (r3 != null)
            {
                UccLogWatcher.Trace(r3.QualifiedName + " (" + r3.ActivityType.Name + ") " + r3.UserDataKey + "= " + r3.UserData);
            }
        }


        protected override void InstanceCompletedOrTerminated()
        {
            
        }
    }


}
