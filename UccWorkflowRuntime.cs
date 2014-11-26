using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Threading;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;

namespace Netgao.Telephony.Workflow
{
    using Netgao.Telephony.Workflow.Activities;

    /// <summary>
    /// 
    /// </summary>
    public sealed class UccWorkflowRuntime : IDisposable
    {
        public UccWorkflowRuntime()
        {
            WorkflowRuntime = CreateWorkflowRuntime();
            WorkflowRuntime.WorkflowCreated += new EventHandler<WorkflowEventArgs>(OnWorkflowCreated);
            WorkflowRuntime.WorkflowTerminated += new EventHandler<WorkflowTerminatedEventArgs>(OnWorkflowTerminated);
            WorkflowRuntime.WorkflowCompleted += new EventHandler<WorkflowCompletedEventArgs>(OnWorkflowCompleted);
            WorkflowRuntime.StartRuntime();
        }



        public void CreateWorkThread(Action<TerminalEventArgs> callback)
        {
            UccTerminalService service = WorkflowRuntime.GetService<UccTerminalService>();
            if (service != null)
            {
                var thread = new UccWorkThread<TerminalEventArgs>(service.WorkItems);
                thread.CreateThread(callback);
            }
        }


        public WorkflowInstance CreateWorkflow(Type workflowType)
        {
            return WorkflowRuntime.CreateWorkflow(workflowType);
        }

        public WorkflowInstance CreateWorkflow(Type workflowType, Dictionary<string, object> parameters)
        {
            return WorkflowRuntime.CreateWorkflow(workflowType, parameters, Guid.NewGuid());
        }

        public WorkflowInstance CreateWorkflow(Type workflowType, Dictionary<string, object> parameters, Guid instanceId)
        {
            return WorkflowRuntime.CreateWorkflow(workflowType, parameters, instanceId);
        }

        public WorkflowInstance CreateWorkflow(string markupFileName, string rulesMarkupFileName)
        {
            return CreateWorkflow(markupFileName, rulesMarkupFileName, new Dictionary<string, object>(), Guid.NewGuid());
        }


        public WorkflowInstance CreateWorkflow(string markupFileName, string rulesMarkupFileName, Dictionary<string, object> parameters)
        {
            return CreateWorkflow(markupFileName, rulesMarkupFileName, parameters, Guid.NewGuid());
        }

        public WorkflowInstance CreateWorkflow(string markupFileName, string rulesMarkupFileName, Dictionary<string, object> parameters, Guid instanceId)
        {
            WorkflowInstance instance = null;
            XmlReader wfReader = null;
            XmlReader rulesReader = null;
            try
            {
                wfReader = XmlReader.Create(markupFileName);
                if (!String.IsNullOrEmpty(rulesMarkupFileName))
                {
                    rulesReader = XmlReader.Create(rulesMarkupFileName);
                    instance = WorkflowRuntime.CreateWorkflow(wfReader, rulesReader, parameters, instanceId);
                }
                else
                {
                    instance = WorkflowRuntime.CreateWorkflow(wfReader, null, parameters, instanceId);
                }
            }
            finally
            {
                if (wfReader != null)
                {
                    wfReader.Close();
                }
                if (rulesReader != null)
                {
                    rulesReader.Close();
                }
            }
            return instance;
        }

        public WorkflowInstance GetWorkflow(Guid instanceId)
        {
            WorkflowInstance instance = null;
            try
            {
                instance = WorkflowRuntime.GetWorkflow(instanceId);
            }
            catch { }
            return instance;
        }

        public object RemoveQueueData(IComparable queueName)
        {
            WorkflowQueuingService queueService = WorkflowRuntime.GetService<WorkflowQueuingService>();
            if (queueService != null)
            {
                WorkflowQueue workflowQueue;
                if (queueService.Exists(queueName))
                {
                    workflowQueue = queueService.GetWorkflowQueue(queueName);
                    if (workflowQueue.Count > 0)
                    {
                        return workflowQueue.Dequeue();
                    }
                }
            }
            return null;
        }

        public T GetService<T>()
        {
            return WorkflowRuntime.GetService<T>();
        }

        public WorkflowRuntime WorkflowRuntime
        {
            get;
            private set;
        }

        private WorkflowRuntime CreateWorkflowRuntime()
        {
            WorkflowRuntime workflowRuntime = new WorkflowRuntime();
           

            //TypeProvider typeProvider = new TypeProvider(workflowRuntime);
            //typeProvider.AddAssemblyReference("Netgao.Telephony.Workflow.dll");
            //workflowRuntime.AddService(typeProvider);

            //WorkflowPersistenceService persistenceService = workflowRuntime.GetService<WorkflowPersistenceService>();

            //if (persistenceService == null)
            // {
            //     workflowRuntime.AddService(new SccFilePersistenceService(false));
            // }    
        
            //workflowRuntime.AddService(new ManualWorkflowSchedulerService());
            workflowRuntime.AddService(new UccTrackingService());
            workflowRuntime.AddService(new CallWorkflowService());

            ExternalDataExchangeService dataExchangeService = workflowRuntime.GetService<ExternalDataExchangeService>();
            if (dataExchangeService == null)
            {
                dataExchangeService = new ExternalDataExchangeService();
                workflowRuntime.AddService(dataExchangeService);
            }

            ITerminalService channelService = workflowRuntime.GetService<ITerminalService>();
            if (channelService == null)
            {
                dataExchangeService.AddService(new UccTerminalService());
            }
            /*
            channelService.Started += (sender, e) =>
            {
                SccLogWatcher.Trace("OnStarted: " + e.ToString() + " " + e.InstanceId);
            };

            channelService.Completed += (sender, e) =>
            {
                SccLogWatcher.Trace("OnCompleted: " + e.ToString() + " " + e.InstanceId);
            };

            channelService.Terminated += (sender, e) =>
            {
                SccLogWatcher.Trace("OnTerminated: " + e.ToString() + " " + e.InstanceId);
            };
            */
            return workflowRuntime;
        }

        public void Dispose()
        {
            Dispose(true);
            
        }

        ~UccWorkflowRuntime()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (WorkflowRuntime != null)
                {
                    if (WorkflowRuntime.IsStarted)
                    {
                        WorkflowRuntime.StopRuntime();
                    }
                    WorkflowRuntime.Dispose();
                    WorkflowRuntime = null;
                }
                GC.SuppressFinalize(this);
            }
        }

        private void OnWorkflowCreated(object sender, WorkflowEventArgs e)
        {
            UccLogWatcher.Trace("WorkflowCreated: {0} {1}", e.WorkflowInstance.GetWorkflowDefinition().Name, e.WorkflowInstance.InstanceId);
        }

        private void OnWorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
        {
           
            UccLogWatcher.Trace("WorkflowCompleted: {0} {1}", e.WorkflowDefinition.Name, e.WorkflowInstance.InstanceId);
        }

        private void OnWorkflowTerminated(object sender, WorkflowTerminatedEventArgs e)
        {
            //SccLogWatcher.Trace("WorkflowTerminated: {0} {1}", e.WorkflowInstance.GetWorkflowDefinition().Name, e.WorkflowInstance.InstanceId);
            UccLogWatcher.Trace("WorkflowTerminated: {0} {1}", e.WorkflowInstance.InstanceId, e.Exception != null ? e.Exception.Message : "");
        }

    }  
}
