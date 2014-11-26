using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Netgao.Telephony.Workflow.Activities
{
    public partial class CalleeActivity
    {
        #region Designer generated code

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            this.GoTo = new Netgao.Telephony.Workflow.Activities.GoToActivity();
            this.Route = new Netgao.Telephony.Workflow.Activities.BlindTransferActivity();
            this.OnReceived = new Netgao.Telephony.Workflow.Activities.OnDialingActivity();
            // 
            // GoTo
            // 
            this.GoTo.Name = "GoTo";
            this.GoTo.TargetWorkflow = null;
            // 
            // Route
            // 
            this.Route.CalleeId = null;
            this.Route.Description = "占线";
            this.Route.Name = "Route";
            // 
            // OnReceived
            // 
            this.OnReceived.Description = "接收按键";
            this.OnReceived.Name = "OnReceived";
            this.OnReceived.Rules = null;
            // 
            // CalleeActivity
            // 
            this.Activities.Add(this.OnReceived);
            this.Activities.Add(this.Route);
            this.Activities.Add(this.GoTo);
            this.Name = "CalleeActivity";
            this.CanModifyActivities = false;

        }

        #endregion

        private BlindTransferActivity Route;
        private GoToActivity GoTo;
        private OnDialingActivity OnReceived;




    }
}
