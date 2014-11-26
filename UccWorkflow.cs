using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Workflow.Activities;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;

namespace Netgao.Telephony.Workflow
{
    public class UccWorkflow : IDisposable
    {
        public UccWorkflow(Action<TerminalEventArgs> callback)
        {
            WorkItems = new Dictionary<int, Queue<Action>>();
            WorkflowRuntime = new UccWorkflowRuntime();
            WorkflowRuntime.CreateWorkThread(callback);
        }


        public UccWorkflowRuntime WorkflowRuntime
        {
            get;
            private set;
        }

        public Dictionary<int, Queue<Action>> WorkItems
        {
            get;
            private set;
        }

       

        public void CreateWorkThread(Queue<Action> workItems)
        {
            CreateWorkThread(workItems, 50);
        }

        public void CreateWorkThread(Queue<Action> workItems, int sleep)
        {
            var thread = new UccWorkThread<Action>(workItems, sleep);
            thread.CreateThread((action) => action());
        }

        

        //public void InitializeWorkItems(int count)
        //{
        //    InitializeWorkItems(count, 50);
        //}

        //public void InitializeWorkItems(int count, int sleep)
        //{
        //    for (int ch = 0; ch < count; ch++)
        //    {
        //        WorkItems.Add(ch, new Queue<Action>());
        //        CreateWorkThread(WorkItems[ch], sleep);
        //    }
        //}

        public ITerminalInstance CreateChannel(UccTerminalService service, Guid uniqueId, Dictionary<string, object> parameters)
        {
            CallWorkflowService callService = WorkflowRuntime.GetService<CallWorkflowService>();
            if (callService != null)
            {
                WorkflowInstance wi = callService.GetCurrentWorkflow(uniqueId);

                if (wi != null)
                {
                    lock (service.Terminals)
                    {
                        ITerminalInstance channelInstance = service.Terminals.SingleOrDefault(p => p.UniqueId == wi.InstanceId);
                        if (channelInstance != null)
                        {
                            channelInstance.SetVariable(UccTerminalInstance.VarVariables, parameters);
                            channelInstance.SetVariable(UccTerminalInstance.VarUniqueId, wi.InstanceId);
                        }
                        return channelInstance;
                    }
                }
            }

            return null;
        }

        public void StartWorkflow(UccTerminalService service, Guid uniqueId, Dictionary<string, object> arguments, string workflow)
        {
            CallWorkflowService callService = WorkflowRuntime.GetService<CallWorkflowService>();
            if (callService != null)
            {
                Guid instanceId = Guid.NewGuid();
                try
                {
                    if (!String.IsNullOrEmpty(workflow))
                    {

                        Type workflowType = Type.GetType(workflow, false);
                        if (workflowType != null)
                        {
                            ITerminalInstance ci = new UccTerminalInstance(instanceId, arguments);
                            lock (service.Terminals)
                            {
                                service.Terminals.Add(ci);
                            }

                            callService.StartWorkflow(workflowType, arguments, uniqueId, instanceId);
                            UccLogWatcher.Trace("开始流程：{0}", instanceId);
                        }
                    }
                }
                catch (WorkflowValidationFailedException e)
                {
                    lock (service.Terminals)
                    {
                        service.Terminals.Remove(instanceId);
                    }
                    foreach (ValidationError error in e.Errors)
                    {
                        UccLogWatcher.Error(String.Format("StartWorkflow Error: {0}", error.ErrorText), e);
                    }
                    throw;
                }
                catch (Exception e)
                {
                    lock (service.Terminals)
                    {
                        service.Terminals.Remove(instanceId);
                    }
                    UccLogWatcher.Error(String.Format("StartWorkflow Error: {0}", e.Message), e);
                    throw;
                }
            }
        }

        public void CloseWorkflow(UccTerminalService service, Guid uniqueId)
        {
            CallWorkflowService callService = WorkflowRuntime.GetService<CallWorkflowService>();
            if (callService != null)
            {
                WorkflowInstance wi = callService.GetCurrentWorkflow(uniqueId);
                if (wi != null)
                {
                    lock (service.Terminals)
                    {
                        service.Terminals.Remove(wi.InstanceId);
                        callService.CloseAllWorkflows(wi.InstanceId);
                        UccLogWatcher.Trace("结束流程：{0}", wi.InstanceId);
                    }
                }
            }
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (WorkflowRuntime != null)
                {
                    WorkflowRuntime.Dispose();
                    WorkflowRuntime = null;
                }
                GC.SuppressFinalize(this);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~UccWorkflow()
        {
            Dispose(false);
        }

        
    }
}
