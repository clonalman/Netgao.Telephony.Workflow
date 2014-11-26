using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Netgao.Telephony.Workflow.Activities
{
    [ToolboxItem(typeof(ActivityToolboxItem))]
    [ToolboxBitmap(typeof(CalleeActivity), "Resources.CalleeActivity.png")]
    [Designer(typeof(CalleeDesigner), typeof(IDesigner))]

    public partial class CalleeActivity : SequenceActivity, IEventActivity
    {
        public static readonly DependencyProperty RouterProperty = DependencyProperty.Register("Router", typeof(CalleeRouter), typeof(CalleeActivity),
            new PropertyMetadata(DependencyPropertyOptions.Metadata | DependencyPropertyOptions.ReadOnly, new ValidationOptionAttribute(ValidationOption.Required)));

        public CalleeActivity()
        {
            InitializeComponent();
            SetReadOnlyPropertyValue(RouterProperty, new CalleeRouter(this));
        }


        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [RefreshProperties(RefreshProperties.All)]
        public CalleeRouter Router
        {
            get { return (CalleeRouter)base.GetValue(RouterProperty); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEventActivity EventActivity
        {
            get { return this.OnReceived; }
        }

        IComparable IEventActivity.QueueName
        {
            get { return EventActivity.QueueName; }
        }

        void IEventActivity.Subscribe(ActivityExecutionContext parentContext, IActivityEventListener<QueueEventArgs> parentEventHandler)
        {
            EventActivity.Subscribe(parentContext, parentEventHandler);
        }

        void IEventActivity.Unsubscribe(ActivityExecutionContext parentContext, IActivityEventListener<QueueEventArgs> parentEventHandler)
        {
            EventActivity.Unsubscribe(parentContext, parentEventHandler);
        }

        [Serializable]
        [TypeConverter(typeof(CalleeTypeConverter))]
        public class CalleeRouter
        {
            private CalleeActivity callee;

            public CalleeRouter(CalleeActivity callee)
            {
                this.callee = callee;
            }

            [Browsable(true)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            [RefreshProperties(RefreshProperties.All)]
            public OnDialingActivity OnReceived
            {
                get { return this.callee.OnReceived; }
            }
            [Browsable(true)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            [RefreshProperties(RefreshProperties.All)]
            public BlindTransferActivity Route
            {
                get { return this.callee.Route; }
            }
            //[Browsable(true)]
            //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            //[RefreshProperties(RefreshProperties.All)]
            //public GoToActivity GoTo
            //{
            //    get { return this.callee.GoTo; }
            //}
        }

        internal class CalleeTypeConverter : TypeConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                PropertyDescriptorCollection descriptors = new PropertyDescriptorCollection(null);
                descriptors.Add(TypeDescriptor.CreateProperty(typeof(CalleeRouter), "OnReceived", typeof(OnDialingActivity), new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content), new ReadOnlyAttribute(false)));
                descriptors.Add(TypeDescriptor.CreateProperty(typeof(CalleeRouter), "Route", typeof(BlindTransferActivity), new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content), new ReadOnlyAttribute(false)));
                descriptors.Add(TypeDescriptor.CreateProperty(typeof(CalleeRouter), "GoTo", typeof(GoToActivity), new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content), new ReadOnlyAttribute(false)));
                return descriptors.Sort(new string[] { "OnReceived", "Route", "GoTo" });
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }


    }
}
