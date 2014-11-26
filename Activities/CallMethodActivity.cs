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
    [ToolboxItem(false)]
    public partial class CallMethodActivity : CallExternalMethodActivity //, IActivityEventListener<CorrelationTokenEventArgs>
	{
        public static readonly DependencyProperty ChannelProperty = DependencyProperty.Register("Channel", typeof(ITerminalInstance), typeof(CallMethodActivity));

		public CallMethodActivity()
		{
			InitializeComponent();
            base.InterfaceType = typeof(ITerminalService);
		}

        protected override void OnActivityExecutionContextLoad(IServiceProvider provider)
        {
            base.OnActivityExecutionContextLoad(provider);

            base.MethodInvoking += (sender, e) =>
            {
                ITerminalService channelService = provider.GetService(typeof(ITerminalService)) as ITerminalService;
                if (channelService != null)
                {
                    base.SetValue(ChannelProperty, channelService.Terminals.SingleOrDefault(p => p.UniqueId == WorkflowInstanceId));
                }
            };

            //if (CorrelationToken != null)
            //{
            //    CorrelationToken contextToken = CorrelationTokenCollection.GetCorrelationToken(this, CorrelationToken.Name, CorrelationToken.OwnerActivityName);
            //    if (contextToken != null)
            //    {
            //        contextToken.SubscribeForCorrelationTokenInitializedEvent(this, this);
            //    }
            //}
        }

        //protected override void OnActivityExecutionContextUnload(IServiceProvider provider)
        //{
        //    if (CorrelationToken != null)
        //    {
        //        CorrelationToken contextToken = CorrelationTokenCollection.GetCorrelationToken(this, CorrelationToken.Name, CorrelationToken.OwnerActivityName);
        //        if (contextToken != null)
        //        {
        //            contextToken.UnsubscribeFromCorrelationTokenInitializedEvent(this, this);
        //        }
        //    }
        //    base.OnActivityExecutionContextUnload(provider);
        //}

        //void IActivityEventListener<CorrelationTokenEventArgs>.OnEvent(object sender, CorrelationTokenEventArgs e)
        //{
        //    if (e.CorrelationToken.Properties != null)
        //    {
        //        CorrelationProperty property = e.CorrelationToken.Properties.SingleOrDefault(p => p.Name == "channel");
        //        if (property != null)
        //        {
        //            base.SetValue(ChannelProperty, property.Value);
        //        }
        //    }
        //}

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ITerminalInstance Channel
        {
            get { return base.GetValue(ChannelProperty) as ITerminalInstance; }
            set { base.SetValue(ChannelProperty, value); }
        }
	}
}
