using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Netgao.Telephony.Workflow.Activities
{
    [ToolboxItem(false)]
    public partial class HandleEventActivity : HandleExternalEventActivity, IEventActivity
    {
        public static readonly DependencyProperty QueueNameProperty = DependencyProperty.FromName("QueueName", typeof(HandleExternalEventActivity));
        public static readonly DependencyProperty RulesProperty = DependencyProperty.Register("Rules", typeof(RuleExpression), typeof(HandleEventActivity));
        public static readonly DependencyProperty ChannelProperty = DependencyProperty.Register("Channel", typeof(ITerminalInstance), typeof(HandleEventActivity));

        public HandleEventActivity()
        {
            InitializeComponent();
            base.InterfaceType = typeof(ITerminalService);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Type InterfaceType
        {
            get { return base.InterfaceType; }
            set { throw new InvalidOperationException("Cannot set InterfaceType on a derived HandleExternalEventActivity."); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ITerminalInstance Channel
        {
            get { return base.GetValue(ChannelProperty) as ITerminalInstance; }
            set { base.SetValue(ChannelProperty, value); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual RuleExpression Rules
        {
            get { return base.GetValue(RulesProperty) as RuleExpression; }
            set { base.SetValue(RulesProperty, value); }
        }

        protected virtual void InitializeArguments(object[] args)
        {
            TerminalEventArgs e = args.OfType<TerminalEventArgs>().SingleOrDefault();
            if (e != null)
            {
                var descriptors = TypeDescriptor.GetProperties(e);
                foreach (var descriptor in descriptors.OfType<PropertyDescriptor>())
                {
                    if (typeof(TerminalEventArgs).IsAssignableFrom(descriptor.ComponentType))
                    {
                        var property = DependencyProperty.FromName(descriptor.Name, GetType());
                        if (property != null)
                        {
                            base.SetValue(property, descriptor.GetValue(e));
                        }
                    }
                }
            }
        }

        protected virtual bool Validate(ActivityExecutionContext context, object[] args)
        {
            InitializeArguments(args);

            if (Rules != null)
            {
                return Rules.Evaluate(this, context);
            }
            return true;
        }

        #region IEventActivity 成员

        void IEventActivity.Subscribe(ActivityExecutionContext parentContext, IActivityEventListener<QueueEventArgs> parentEventHandler)
        {
            if (parentContext == null)
                throw new ArgumentNullException("parentContext");
            if (parentEventHandler == null)
                throw new ArgumentNullException("parentEventHandler");

            WorkflowQueuingService queueService = parentContext.GetService<WorkflowQueuingService>();
            if (queueService != null)
            {
                Array.ForEach(new IComparable[] 
                {
                    new EventQueueName(typeof(ITerminalService), "Started"),
                    new EventQueueName(typeof(ITerminalService), "Completed"),
                    new EventQueueName(typeof(ITerminalService), "Terminated"),
                    new EventQueueName(typeof(ITerminalService), "Timeout")
                }, 
                (queueName) =>
                {
                    WorkflowQueue workflowQueue;
                    if (queueService.Exists(queueName))
                    {
                        workflowQueue = queueService.GetWorkflowQueue(queueName);
                    }
                    else
                    {
                        workflowQueue = queueService.CreateWorkflowQueue(queueName, true);
                    }
                    workflowQueue.RegisterForQueueItemAvailable(new FilterEventListener(parentEventHandler, this), QualifiedName);
                });

                //IComparable queueName = (IComparable)GetValue(QueueNameProperty);
                //if (queueName != null)
                //{

                //    WorkflowQueue workflowQueue;
                //    if (queueService.Exists(queueName))
                //    {
                //        workflowQueue = queueService.GetWorkflowQueue(queueName);
                //        workflowQueue.Enabled = true;
                //    }
                //    else
                //    {
                //        workflowQueue = queueService.CreateWorkflowQueue(queueName, true);
                //    }
                //    workflowQueue.RegisterForQueueItemAvailable(new FilterEventListener(parentEventHandler, this), QualifiedName);
                //}
            }
        }

        //void IEventActivity.Unsubscribe(ActivityExecutionContext parentContext, IActivityEventListener<QueueEventArgs> parentEventHandler)
        //{
        //    if (parentContext == null)
        //        throw new ArgumentNullException("parentContext");
        //    if (parentEventHandler == null)
        //        throw new ArgumentNullException("parentEventHandler");

        //    WorkflowQueuingService queueService = parentContext.GetService<WorkflowQueuingService>();
        //    if (queueService != null)
        //    {
        //        IComparable queueName = (IComparable)GetValue(QueueNameProperty);
        //        if (queueName != null && queueService.Exists(queueName))
        //        {

        //            WorkflowQueue workflowQueue = queueService.GetWorkflowQueue(queueName);
        //            if (workflowQueue.Count > 0)
        //            {
        //                IMethodMessage msg = workflowQueue.Dequeue() as IMethodMessage;
        //                if (msg != null)
        //                {
        //                    MethodMessageWrapper wrapper = msg as MethodMessageWrapper;
        //                    if (wrapper != null)
        //                    {
        //                        msg = wrapper.GetWrappedMessage();
        //                    }

        //                    workflowQueue.Enqueue(msg);
        //                    RegisterForStatusChange(Activity.ClosedEvent, Parent as IActivityEventListener<ActivityExecutionStatusChangedEventArgs>);

        //                    //RegisterForStatusChange(Activity.ClosedEvent, Parent as IActivityEventListener<ActivityExecutionStatusChangedEventArgs>);

        //                    //InitializeArguments(msg.Args);

        //                    //if (Validate(context))
        //                    //{
        //                    //    context.ExecuteActivity(this);
        //                    //}
        //                    //else
        //                    //{
        //                    //    workflowQueue.Enqueue(msg);
        //                    //    RegisterForStatusChange(Activity.ClosedEvent, Parent as IActivityEventListener<ActivityExecutionStatusChangedEventArgs>);
        //                    //}
        //                }
        //            }
        //            workflowQueue.UnregisterForQueueItemAvailable(parentEventHandler);
        //        }
        //    }
        //}

        #endregion

        [Serializable]
        private class FilterEventListener : IActivityEventListener<QueueEventArgs>
        {
            private IActivityEventListener<QueueEventArgs> parentEventHandler;
            private HandleEventActivity thisActivity;

            public FilterEventListener(IActivityEventListener<QueueEventArgs> parentEventHandler, HandleEventActivity thisActivity)
            {
                this.parentEventHandler = parentEventHandler;
                this.thisActivity = thisActivity;
            }

            public void OnEvent(object sender, QueueEventArgs e)
            {
                ActivityExecutionContext context = sender as ActivityExecutionContext;
                if (context != null)
                {
                    WorkflowQueuingService queuingService = context.GetService<WorkflowQueuingService>();
                    if (queuingService != null)
                    {
                        WorkflowQueue workflowQueue = queuingService.GetWorkflowQueue(e.QueueName);

                        AbandonQueue(context, workflowQueue);

                        if (workflowQueue.Count > 0)
                        {
                            IMethodMessage msg = workflowQueue.Peek() as IMethodMessage;
                            if (msg != null)
                            {
                                IComparable queueName = (IComparable)thisActivity.GetValue(QueueNameProperty);
                                if (e.QueueName.CompareTo(queueName) == 0 &&
                                    thisActivity.Validate(context, msg.Args))
                                {
                                    ///////////////////////////////////////////////////////////////
                                    //MethodMessageWrapper wrapper = msg as MethodMessageWrapper;
                                    //if (wrapper != null)
                                    //{
                                    //    msg = wrapper.GetWrappedMessage(true);
                                    //}
                                    //workflowQueue.Enqueue(msg);
                                    //////////////////////////////////////////////////////////////
                                    Thread.Sleep(50);
                                    AbandonQueue(context, workflowQueue);

                                    //if (msg.Args.Length > 1)
                                    //{
                                    //    SccLogWatcher.Trace("合法消息：{0}/{1}", msg.Args[0], msg.Args[1]);
                                    //}
                                    parentEventHandler.OnEvent(sender, e);
                                   
                                    
                                }
                                else
                                {
                                    ////////////////////////////////////////////////////////////////
                                    //MethodMessageWrapper wrapper = msg as MethodMessageWrapper;
                                    //if (wrapper == null || !wrapper.Verify(this))
                                    //{
                                    //    workflowQueue.Enqueue(new MethodMessageWrapper(msg, this));
                                    //}
                                    ////////////////////////////////////////////////////////////////
                                    if (msg.MethodName == "Terminated")
                                    {
                                        AbandonWorkflows(context);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public void AbandonQueue(ActivityExecutionContext context, WorkflowQueue workflowQueue)
            {
                while (workflowQueue.Count > 1)
                {
                    IMethodMessage msg = workflowQueue.Dequeue() as IMethodMessage;
                    if (msg != null)
                    {
                        if (msg.Args.Length > 1)
                        {
                            UccLogWatcher.Trace("抛弃消息：{0} {1} {2}", msg.Args[0], msg.Args[1], this.thisActivity.WorkflowInstanceId);
                        }
                        if (msg.MethodName == "Terminated")
                        {
                            AbandonWorkflows(context);
                        }
                    }
                }
            }

            public void AbandonWorkflows(ActivityExecutionContext context)
            {
                //context.CancelActivity(thisActivity);

                CallWorkflowService service = context.GetService<CallWorkflowService>();
                if (service != null)
                {
                    service.CloseAllWorkflows(thisActivity.WorkflowInstanceId);
                }
            }

            public void RegisterForQueueItemAvailable(WorkflowQueue workflowQueue, MethodMessageWrapper wrapper)
            {
                if (wrapper != null)
                {
                    workflowQueue.RegisterForQueueItemAvailable(wrapper.Item as IActivityEventListener<QueueEventArgs>, thisActivity.QualifiedName);
                    RegisterForQueueItemAvailable(workflowQueue, wrapper.GetWrappedMessage(false) as MethodMessageWrapper);
                }
            }

            public void UnregisterForQueueItemAvailable(WorkflowQueue workflowQueue, MethodMessageWrapper wrapper)
            {
                if (wrapper != null)
                {
                    workflowQueue.UnregisterForQueueItemAvailable(wrapper.Item as IActivityEventListener<QueueEventArgs>);
                    UnregisterForQueueItemAvailable(workflowQueue, wrapper.GetWrappedMessage(false) as MethodMessageWrapper);
                }
            }
        }
    }
}
