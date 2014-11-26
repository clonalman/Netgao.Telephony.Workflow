using System;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Tracking;

namespace Netgao.Telephony.Workflow
{
    public class UccTrackingService : TrackingService
    {
        private TrackingProfile profile;
        public UccTrackingService()
            : base()
        {
            profile = CreateTrackingProfile();
        }

        private TrackingProfile CreateTrackingProfile()
        {
            // Listen for activity status changes
            ActivityTrackingLocation loc = new ActivityTrackingLocation(typeof(Activity));
            loc.ExecutionStatusEvents.Add(ActivityExecutionStatus.Initialized);
            loc.ExecutionStatusEvents.Add(ActivityExecutionStatus.Executing);
            loc.ExecutionStatusEvents.Add(ActivityExecutionStatus.Canceling);
            loc.ExecutionStatusEvents.Add(ActivityExecutionStatus.Faulting);
            loc.ExecutionStatusEvents.Add(ActivityExecutionStatus.Closed);
            loc.ExecutionStatusEvents.Add(
              ActivityExecutionStatus.Compensating);
            loc.MatchDerivedTypes = true;
            ActivityTrackPoint atp = new ActivityTrackPoint();
            atp.MatchingLocations.Add(loc);

            // Listen for workflow status events
            WorkflowTrackPoint wtp = new WorkflowTrackPoint();
            wtp.MatchingLocation = new WorkflowTrackingLocation();
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Aborted);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Changed);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Completed);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Created);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Exception);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Idle);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Loaded);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Persisted);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Resumed);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Started);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Suspended);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Terminated);
            wtp.MatchingLocation.Events.Add(TrackingWorkflowEvent.Unloaded);

            // Listen for custom tracking data
            UserTrackingLocation loc2 = new UserTrackingLocation();
            loc2.ActivityType = typeof(Activity);
            loc2.MatchDerivedActivityTypes = true;
            loc2.ArgumentType = typeof(object);
            loc2.MatchDerivedArgumentTypes = true;

            UserTrackPoint utp = new UserTrackPoint();
            utp.MatchingLocations.Add(loc2);

            // Return tracking profile
            TrackingProfile profile = new TrackingProfile();
            profile.Version = new Version(1, 0, 0, 0);
            profile.ActivityTrackPoints.Add(atp);
            profile.WorkflowTrackPoints.Add(wtp);
            profile.UserTrackPoints.Add(utp);

            return profile;
        }

        protected override bool TryGetProfile(Type workflowType, out TrackingProfile profile)
        {
            profile = this.profile;
            return true;
        }

        protected override TrackingChannel GetTrackingChannel(
          TrackingParameters parameters)
        {
            return new UccTrackingChannel(parameters);
        }

        protected override TrackingProfile GetProfile(Guid workflowInstanceId)
        {
            return this.profile;
        }

        protected override TrackingProfile GetProfile(Type workflowType, Version profileVersionId)
        {
            return this.profile;
        }

        protected override bool TryReloadProfile(Type workflowType, Guid workflowInstanceId, out TrackingProfile profile)
        {
            profile = null;
            return false;
        }
    }

}
