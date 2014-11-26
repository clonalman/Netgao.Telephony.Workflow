using System;
using System.Collections.Generic;
using System.Drawing;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace Netgao.Telephony.Workflow.Activities
{
    [ActivityDesignerTheme(typeof(CalleeDesignerTheme))]
    public class CalleeDesigner : ActivityDesigner
    {
        private sealed class CalleeDesignerTheme : ActivityDesignerTheme
        {
            public CalleeDesignerTheme(WorkflowTheme theme)
                : base(theme)
            {
                this.BackgroundStyle = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                this.BorderColor = Color.FromArgb(0xff, 0x80, 0x80, 0x80);
                this.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                this.BackColorStart = Color.White;
                this.BackColorEnd = Color.LightSalmon;
            }
        }
    }
}

