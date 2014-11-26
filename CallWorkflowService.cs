using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Threading;
using System.Linq;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;

namespace Netgao.Telephony.Workflow
{
    public sealed class CallWorkflowService : WorkflowRuntimeService
    {
        #region Fields

        private EventHandler<WorkflowCompletedEventArgs> _CompletedHandler = null;
        private EventHandler<WorkflowTerminatedEventArgs> _TerminatedHandler = null;
        private List<Stack<Guid>> workflowQueue = new List<Stack<Guid>>();

        #endregion Fields

        #region Methods

        public Mutex OpenMutex(Guid instanceId, bool createNew)
        {
            Mutex mutex = null;
            string mutexName = "__CALL_" + instanceId.ToString();
            try
            {
                mutex = Mutex.OpenExisting(mutexName);
            }
            catch
            {
                if (createNew)
                {
                    mutex = new Mutex(true, mutexName);
                }
            }
            return mutex;
        }

        public WorkflowInstance StartWorkflow(Type workflowType, Dictionary<string, object> namedArgumentValues, Guid callerId)
        {
            return StartWorkflow(workflowType, namedArgumentValues, callerId, Guid.NewGuid());
        }

        public WorkflowInstance StartWorkflow(Type workflowType, Dictionary<string, object> namedArgumentValues, Guid callerId, Guid calleeId)
        {
            WorkflowInstance wi = Runtime.CreateWorkflow(workflowType, CreateArgumentValues(workflowType, namedArgumentValues), calleeId);
            Push(callerId, wi.InstanceId);
            wi.Start();
            ManualWorkflowSchedulerService ss = Runtime.GetService<ManualWorkflowSchedulerService>();
            if (ss != null)
            {
                ss.RunWorkflow(wi.InstanceId);
            }

            return wi;
        }

        internal WorkflowInstance StartWorkflow(IServiceProvider provider, Type workflowType, Dictionary<string, object> namedArgumentValues, Guid callerId)
        {
            WorkflowInstance wi = null;
            IStartWorkflow service = provider.GetService(typeof(IStartWorkflow)) as IStartWorkflow;
            if (service != null)
            {
                Guid instanceId = service.StartWorkflow(workflowType, CreateArgumentValues(workflowType, namedArgumentValues));
                Push(callerId, instanceId);
                wi = GetWorkflow(instanceId);

                ManualWorkflowSchedulerService ss = Runtime.GetService<ManualWorkflowSchedulerService>();
                if (ss != null)
                {
                    ss.RunWorkflow(wi.InstanceId);
                }
            }
            return wi;
        }

        public void CloseAllWorkflows(Guid callerId)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                lock (workflowQueue)
                {
                    Stack<Guid> workflowStack = workflowQueue.Find(p => p.Any(m => m == callerId));
                    if (workflowStack != null)
                    {
                        while (workflowStack.Count > 0)
                        {
                            WorkflowInstance wi = GetWorkflow(workflowStack.Pop());
                            try
                            {
                                if (wi != null) wi.Abort();
                            }
                            catch(Exception ex) 
                            {
                                UccLogWatcher.Trace("RaiseEvent Error: Terminate({0}) - {1}", wi.InstanceId, ex.Message);
                            }
                        }
                    }
                }
            });
        }

        Dictionary<string, object> CreateArgumentValues(Type workflowType, Dictionary<string, object> variables)
        {
            Dictionary<string, object> namedArgumentValues = new Dictionary<string, object>();
            foreach (string name in variables.Keys)
            {
                PropertyInfo pi = workflowType.GetProperty(name);
                if (pi != null && pi.CanWrite)
                {
                    namedArgumentValues.Add(name, variables[name]);
                }
            }
            return namedArgumentValues;
        }

        void Push(Guid callerId, Guid calleeId)
        {
            lock (workflowQueue)
            {
                Stack<Guid> workflowStack = workflowQueue.Find(p => p.Any(m => m == callerId));
                if (workflowStack == null)
                {
                    workflowStack = new Stack<Guid>();
                    workflowStack.Push(callerId);
                    workflowQueue.Add(workflowStack);
                }
                if (callerId != calleeId)
                {
                    workflowStack.Push(calleeId);
                }
            }
        }

        public WorkflowInstance GetWorkflow(Guid callerId)
        {
            try
            {
                return Runtime.GetWorkflow(callerId);
            }
            catch
            {
                return null;
            }
        }

        public WorkflowInstance GetCurrentWorkflow(Guid callerId)
        {
            return GetCurrentWorkflow(callerId, false);
        }

        public WorkflowInstance GetCurrentWorkflow(Guid callerId, bool discarding)
        {
            lock (workflowQueue)
            {
                Stack<Guid> workflowStack = workflowQueue.Find(p => p.Any(m => m == callerId));
                if (workflowStack != null)
                {
                    return GetWorkflow(discarding ? workflowStack.Pop() : workflowStack.Peek());
                }
                else return null;
            }
        }

        protected override void OnStarted()
        {
            base.OnStarted();

            if (null == _CompletedHandler)
            {
                _CompletedHandler = delegate(object o, WorkflowCompletedEventArgs e)
                {
                    lock (workflowQueue)
                    {
                        Stack<Guid> workflowStack = workflowQueue.Find(p => p.Any(m => m == e.WorkflowInstance.InstanceId));
                        if (workflowStack != null)
                        {
                            if (workflowStack.Pop() != e.WorkflowInstance.InstanceId)
                            {
                                throw new InvalidOperationException();
                            }

                            try
                            {
                                WorkflowInstance c = Runtime.GetWorkflow(workflowStack.Peek());
                                if (c != null)
                                {
                                    c.EnqueueItem(e.WorkflowInstance.InstanceId, e.OutputParameters, null, null);
                                }
                            }
                            catch { /* do nothing */ }

                            //Mutex mutex = OpenMutex(workflowStack.Peek(), false);
                            //if (mutex != null)
                            //{
                            //    mutex.ReleaseMutex();
                            //}
                        }
                    }

                    
                };
                this.Runtime.WorkflowCompleted += _CompletedHandler;
            }

            if (null == _TerminatedHandler)
            {
                _TerminatedHandler = delegate(object o, WorkflowTerminatedEventArgs e)
                {
                    lock (workflowQueue)
                    {
                        Stack<Guid> workflowStack = workflowQueue.Find(p => p.Any(m => m == e.WorkflowInstance.InstanceId));
                        if (workflowStack != null)
                        {
                            if (workflowStack.Pop() != e.WorkflowInstance.InstanceId)
                            {
                                throw new InvalidOperationException();
                            }
                            try
                            {
                                WorkflowInstance c = Runtime.GetWorkflow(workflowStack.Peek());
                                if (c != null)
                                {
                                    c.EnqueueItem(e.WorkflowInstance.InstanceId, new Exception("Called Workflow Terminated", e.Exception), null, null);
                                }
                            }
                            catch { /* do nothing */ }

                            //Mutex mutex = OpenMutex(workflowStack.Peek(), false);
                            //if (mutex != null)
                            //{
                            //    mutex.ReleaseMutex();
                            //}

                        }
                    }
                };

                this.Runtime.WorkflowTerminated += _TerminatedHandler;
            }
        }

        protected override void OnStopped()
        {
            workflowQueue.Clear();
            base.OnStopped();
        }

        #endregion Methods
    }
}
