﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Threading;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow
{
    public class UccFilePersistenceService : WorkflowPersistenceService
    {
        public readonly static TimeSpan MaxInterval = new TimeSpan(30, 0, 0, 0);

        private bool unloadOnIdle = false;
        private Dictionary<Guid, Timer> instanceTimers;

        public UccFilePersistenceService(bool unloadOnIdle)
        {
            this.unloadOnIdle = unloadOnIdle;
            this.instanceTimers = new Dictionary<Guid, Timer>();
        }

        protected override void SaveWorkflowInstanceState(Activity rootActivity, bool unlock)
        {
            // Save the workflow 
            Guid contextGuid = (Guid)rootActivity.GetValue(Activity.ActivityContextGuidProperty);
            UccLogWatcher.Trace("Saving instance: {0}\n", contextGuid);
            SerializeToFile(WorkflowPersistenceService.GetDefaultSerializedForm(rootActivity), contextGuid);

            // See when the next timer (Delay activity) for this workflow will expire 
            TimerEventSubscriptionCollection timers = (TimerEventSubscriptionCollection)rootActivity.GetValue(TimerEventSubscriptionCollection.TimerCollectionProperty);
            TimerEventSubscription subscription = timers.Peek();
            if (subscription != null)
            {
                // Set a system timer to automatically reload this workflow when its next timer expires 
                TimerCallback callback = new TimerCallback(ReloadWorkflow);
                TimeSpan timeDifference = subscription.ExpiresAt - DateTime.UtcNow;
                // check to make sure timeDifference is in legal range 
                if (timeDifference > UccFilePersistenceService.MaxInterval)
                {
                    timeDifference = UccFilePersistenceService.MaxInterval;
                }
                else if (timeDifference < TimeSpan.Zero)
                {
                    timeDifference = TimeSpan.Zero;
                }
                this.instanceTimers.Add(contextGuid, new System.Threading.Timer(
                    callback,
                    subscription.WorkflowInstanceId,
                    timeDifference,
                    new TimeSpan(-1)));
            }
        }

        private void ReloadWorkflow(object id)
        {
            // Reload the workflow so that it will continue processing 
            Timer toDispose;
            if (this.instanceTimers.TryGetValue((Guid)id, out toDispose))
            {
                this.instanceTimers.Remove((Guid)id);
                toDispose.Dispose();
            }
            this.Runtime.GetWorkflow((Guid)id);
        }

        // Load workflow instance state. 
        protected override Activity LoadWorkflowInstanceState(Guid instanceId)
        {        
            UccLogWatcher.Trace("Loading instance: {0}\n", instanceId);
            byte[] workflowBytes = DeserializeFromFile(instanceId);
            return WorkflowPersistenceService.RestoreFromDefaultSerializedForm(workflowBytes, null);
        }

        // Unlock the workflow instance state. 
        // Instance state locking is necessary when multiple runtimes share instance persistence store 
        protected override void UnlockWorkflowInstanceState(Activity state)
        {
            //File locking is not supported in this sample 
        }

        // Save the completed activity state. 
        protected override void SaveCompletedContextActivity(Activity activity)
        {
            Guid contextGuid = (Guid)activity.GetValue(Activity.ActivityContextGuidProperty);
            UccLogWatcher.Trace("Saving completed activity context: {0}", contextGuid);
            SerializeToFile(
                WorkflowPersistenceService.GetDefaultSerializedForm(activity), contextGuid);
        }

        // Load the completed activity state. 
        protected override Activity LoadCompletedContextActivity(Guid activityId, Activity outerActivity)
        {
            UccLogWatcher.Trace("Loading completed activity context: {0}", activityId);
            byte[] workflowBytes = DeserializeFromFile(activityId);
            Activity deserializedActivities = WorkflowPersistenceService.RestoreFromDefaultSerializedForm(workflowBytes, outerActivity);
            return deserializedActivities;

        }

        protected override bool UnloadOnIdle(Activity activity)
        {
            return unloadOnIdle;
        }
        // Serialize the activity instance state to file 
        private void SerializeToFile(byte[] workflowBytes, Guid id)
        {
            String filename = id.ToString();
            FileStream fileStream = null;
            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);

                fileStream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None);

                // Get the serialized form 
                fileStream.Write(workflowBytes, 0, workflowBytes.Length);
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }
        // Deserialize the instance state from the file given the instance id 
        private byte[] DeserializeFromFile(Guid id)
        {
            String filename = id.ToString();
            FileStream fileStream = null;
            try
            {
                // File opened for shared reads but no writes by anyone 
                fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                fileStream.Seek(0, SeekOrigin.Begin);
                byte[] workflowBytes = new byte[fileStream.Length];

                // Get the serialized form 
                fileStream.Read(workflowBytes, 0, workflowBytes.Length);

                return workflowBytes;
            }
            finally
            {
                fileStream.Close();
            }
        }
    }

}