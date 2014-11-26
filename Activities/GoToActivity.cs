using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Xml;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Threading;

namespace Netgao.Telephony.Workflow.Activities
{
    [ToolboxItem(typeof(ActivityToolboxItem))]
    [ToolboxBitmap(typeof(GoToActivity), "Resources.GoToActivity.png")]
    [Designer(typeof(GoToActivityDesigner), typeof(IDesigner))]
    public sealed partial class GoToActivity : CompositeActivity
    {
        public static readonly DependencyProperty InstanceIdProperty = DependencyProperty.Register("InstanceId", typeof(Guid), typeof(GoToActivity), new PropertyMetadata(Guid.Empty));
        public static readonly DependencyProperty ParameterBindingsProperty = DependencyProperty.Register("ParameterBindings", typeof(WorkflowParameterBindingCollection), typeof(GoToActivity), new PropertyMetadata(DependencyPropertyOptions.Metadata | DependencyPropertyOptions.ReadOnly, new Attribute[] { new BrowsableAttribute(false), new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content) }));
        public static readonly DependencyProperty TargetWorkflowProperty = DependencyProperty.Register("TargetWorkflow", typeof(string), typeof(GoToActivity));
        public static readonly DependencyProperty BlockedProperty = DependencyProperty.Register("Blocked", typeof(bool), typeof(GoToActivity), new PropertyMetadata(true, DependencyPropertyOptions.Metadata));

        public GoToActivity()
        {
            InitializeComponent();

            base.SetReadOnlyPropertyValue(ParameterBindingsProperty, new WorkflowParameterBindingCollection(this));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Guid InstanceId
        {
            get { return (Guid)base.GetValue(InstanceIdProperty); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
        public WorkflowParameterBindingCollection ParameterBindings
        {
            get { return (base.GetValue(ParameterBindingsProperty) as WorkflowParameterBindingCollection); }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        [TypeConverter(typeof(WorkflowConverter))]
        //[Editor(typeof(WorkflowDialogEditor), typeof(UITypeEditor))]
        public string TargetWorkflow
        {
            get { return ((string)(base.GetValue(TargetWorkflowProperty))); }
            set { base.SetValue(TargetWorkflowProperty, value); }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SccCategory("Activity_Property")]
        public bool Blocked
        {
            get { return ((bool)(base.GetValue(BlockedProperty))); }
            set { base.SetValue(BlockedProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            CallWorkflowService service = executionContext.GetService<CallWorkflowService>();
            if (service != null)
            {
                base.SetValue(InstanceIdProperty, Guid.NewGuid());

                ITerminalService channelService = executionContext.GetService<ITerminalService>();


                if (channelService != null)
                {
                    ITerminalInstance ch = channelService.Terminals.SingleOrDefault(p => p.UniqueId == WorkflowInstanceId);
                    if (ch != null)
                    {
                        Dictionary<string, object> namedArgumentValues = new Dictionary<string, object>();
                        foreach (WorkflowParameterBinding binding in this.ParameterBindings)
                        {
                            namedArgumentValues.Add(binding.ParameterName, binding.Value);
                        }

                        lock (channelService.Terminals)
                        {
                            channelService.Terminals.Add(ch.Copy(InstanceId));

                            //WorkflowInstance wi = service.GetCurrentWorkflow(WorkflowInstanceId, true);

                            //Activity rootActivity = GetRootActivity(this);
                            //if (rootActivity.GetType() == TargetWorkflow)
                            //{
                            //    wi = service.GetCurrentWorkflow(WorkflowInstanceId, true);
                            //    wi.Abort();
                            //}

                            WorkflowInstance wi = service.StartWorkflow(Type.GetType(TargetWorkflow), namedArgumentValues, WorkflowInstanceId, InstanceId);

                            if (Blocked)
                            {
                                WorkflowQueuingService qs = executionContext.GetService<WorkflowQueuingService>();
                                if (qs != null)
                                {
                                    WorkflowQueue q = qs.CreateWorkflowQueue(wi.InstanceId, false);
                                    q.QueueItemAvailable += new EventHandler<QueueEventArgs>(OnQueueItemAvailable);
                                }
                            }
                        }
                    }
                }

                if (Blocked)
                {
                    return ActivityExecutionStatus.Executing;
                }
                else
                {
                    //channelService.Reset(WorkflowInstanceId, "WorkflowDefinition", TargetWorkflow.AssemblyQualifiedName);
                    return ActivityExecutionStatus.Closed;
                }
            }
            else
            {
                return ActivityExecutionStatus.Closed;
            }
        }

        //void StartWorkflow(IServiceProvider provider)
        //{
        //    CallWorkflowService service = provider.GetService(typeof(CallWorkflowService)) as CallWorkflowService;
        //    if (service != null)
        //    {
        //        Dictionary<string, object> namedArgumentValues = new Dictionary<string, object>();
        //        //foreach (WorkflowParameterBinding binding in this.ParameterBindings)
        //        //{
        //        //    namedArgumentValues.Add(binding.ParameterName, binding.Value);
        //        //}

        //        WorkflowInstance instance = service.StartWorkflow(TargetWorkflow, namedArgumentValues, WorkflowInstanceId);

        //        WorkflowQueuingService qs = provider.GetService(typeof(WorkflowQueuingService)) as WorkflowQueuingService;
        //        if (qs != null)
        //        {
        //            WorkflowQueue q = qs.CreateWorkflowQueue(instance.InstanceId.ToString(), false);
        //            q.QueueItemAvailable += new EventHandler<QueueEventArgs>(OnQueueItemAvailable);
        //        }

        //        //EventQueueName queueName = new EventQueueName(typeof(IChannelService), "Started");
        //        //instance.EnqueueItem(queueName, null, null, null);

        //        base.SetValue(InstanceIdProperty, instance.InstanceId);
        //    }
        //}

        void OnQueueItemAvailable(object sender, QueueEventArgs e)
        {
            ActivityExecutionContext aec = sender as ActivityExecutionContext;
            if (aec != null)
            {
                WorkflowQueuingService qs = aec.GetService<WorkflowQueuingService>();
                WorkflowQueue q = qs.GetWorkflowQueue(e.QueueName);
                //get the outparameters from the workflow
                object o = q.Dequeue();
                //delete the queue
                qs.DeleteWorkflowQueue(e.QueueName);
                aec.CloseActivity();

                ITerminalService channelService = aec.GetService<ITerminalService>();
                lock (channelService.Terminals)
                {
                    if (channelService.Terminals.Contains((Guid)e.QueueName))
                    {
                        channelService.Terminals.Remove((Guid)e.QueueName);
                    }
                }
            }
        }

        private static Activity GetRootActivity(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException("activity");
            }
            while (activity.Parent != null)
            {
                activity = activity.Parent;
            }
            return activity;
        }

        private Dictionary<string, object> ExecuteActivity(Type activityType, IEnumerable<object> services, params object[] inputs)
        {
            Dictionary<string, object> outputs = null;
            Exception ex = null;
            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted +=
                 delegate(object sender, WorkflowCompletedEventArgs e)
                 {
                     outputs = e.OutputParameters;
                     waitHandle.Set();
                 };
                workflowRuntime.WorkflowTerminated +=
                 delegate(object sender, WorkflowTerminatedEventArgs e)
                 {
                     ex = e.Exception;
                     waitHandle.Set();
                 };
                foreach (object svc in services)
                {
                    workflowRuntime.AddService(svc);
                }
                Dictionary<string, object> parms = new Dictionary<string, object>();
                for (int i = 0; i < inputs.Length; i += 2)
                {
                    Debug.Assert(inputs[i] is string);
                    parms.Add((string)inputs[i], inputs[i + 1]);
                }
                WorkflowInstance instance =
                 workflowRuntime.CreateWorkflow(activityType, parms);
                instance.Start();
                waitHandle.WaitOne();
                if (ex != null)
                {
                    //Assert.True( false, ex.Message );
                    return null;
                }
                else
                {
                    return outputs;
                }
            }
        }


    }
}
