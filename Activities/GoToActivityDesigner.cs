using System;
using System.Collections.Generic;
using System.Drawing;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace Netgao.Telephony.Workflow.Activities
{
    [ActivityDesignerTheme(typeof(GoToDesignerTheme))]
    public class GoToActivityDesigner : ActivityDesigner
    {
        private sealed class GoToDesignerTheme : ActivityDesignerTheme
        {
            public GoToDesignerTheme(WorkflowTheme theme)
                : base(theme)
            {
                this.BackgroundStyle = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                this.BorderColor = Color.FromArgb(0xff, 0x80, 0x80, 0x80);
                this.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                this.BackColorStart = Color.White;
                this.BackColorEnd = Color.LightGreen;
            }
        }
    }
}

