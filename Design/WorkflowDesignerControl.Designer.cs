//---------------------------------------------------------------------
//  This file is part of the WindowsWorkflow.NET web site samples.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
// 
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
// 
//  THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//---------------------------------------------------------------------

namespace Netgao.Telephony.Workflow.Design
{
    partial class WorkflowDesignerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("标准组件", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("工作流程", System.Windows.Forms.HorizontalAlignment.Left);
            this.panel1 = new System.Windows.Forms.Panel();
            this.workflowViewSplitter = new System.Windows.Forms.SplitContainer();
            this.workflowViewTabControl = new System.Windows.Forms.TabControl();
            this.tabPageMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeOthersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.diagramTabPage = new System.Windows.Forms.TabPage();
            this.listView = new System.Windows.Forms.ListView();
            this.listViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.activityTabPage = new System.Windows.Forms.TabPage();
            this.propertyGridSplitter = new System.Windows.Forms.SplitContainer();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.panel1.SuspendLayout();
            this.workflowViewSplitter.Panel1.SuspendLayout();
            this.workflowViewSplitter.Panel2.SuspendLayout();
            this.workflowViewSplitter.SuspendLayout();
            this.tabPageMenuStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.diagramTabPage.SuspendLayout();
            this.listViewMenu.SuspendLayout();
            this.activityTabPage.SuspendLayout();
            this.propertyGridSplitter.Panel2.SuspendLayout();
            this.propertyGridSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.workflowViewSplitter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(659, 522);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // workflowViewSplitter
            // 
            this.workflowViewSplitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.workflowViewSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workflowViewSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.workflowViewSplitter.Location = new System.Drawing.Point(0, 0);
            this.workflowViewSplitter.Name = "workflowViewSplitter";
            // 
            // workflowViewSplitter.Panel1
            // 
            this.workflowViewSplitter.Panel1.Controls.Add(this.workflowViewTabControl);
            // 
            // workflowViewSplitter.Panel2
            // 
            this.workflowViewSplitter.Panel2.Controls.Add(this.tabControl1);
            this.workflowViewSplitter.Size = new System.Drawing.Size(659, 522);
            this.workflowViewSplitter.SplitterDistance = 394;
            this.workflowViewSplitter.TabIndex = 0;
            this.workflowViewSplitter.Text = "splitContainer1";
            // 
            // workflowViewTabControl
            // 
            this.workflowViewTabControl.ContextMenuStrip = this.tabPageMenuStrip;
            this.workflowViewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workflowViewTabControl.ItemSize = new System.Drawing.Size(0, 17);
            this.workflowViewTabControl.Location = new System.Drawing.Point(0, 0);
            this.workflowViewTabControl.Name = "workflowViewTabControl";
            this.workflowViewTabControl.Padding = new System.Drawing.Point(6, 4);
            this.workflowViewTabControl.SelectedIndex = 0;
            this.workflowViewTabControl.ShowToolTips = true;
            this.workflowViewTabControl.Size = new System.Drawing.Size(392, 520);
            this.workflowViewTabControl.TabIndex = 0;
            this.workflowViewTabControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.workflowViewTabControl_MouseDoubleClick);
            this.workflowViewTabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.workflowViewTabControl_MouseDown);
            // 
            // tabPageMenuStrip
            // 
            this.tabPageMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeMenuItem,
            this.closeOthersMenuItem});
            this.tabPageMenuStrip.Name = "tabPageMenuStrip";
            this.tabPageMenuStrip.Size = new System.Drawing.Size(185, 48);
            this.tabPageMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tabPageMenuStrip_ItemClicked);
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Name = "closeMenuItem";
            this.closeMenuItem.Size = new System.Drawing.Size(184, 22);
            this.closeMenuItem.Text = "关闭(&C)";
            // 
            // closeOthersMenuItem
            // 
            this.closeOthersMenuItem.Name = "closeOthersMenuItem";
            this.closeOthersMenuItem.Size = new System.Drawing.Size(184, 22);
            this.closeOthersMenuItem.Text = "除此之外全部关闭(&A)";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.diagramTabPage);
            this.tabControl1.Controls.Add(this.activityTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(6, 4);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(259, 520);
            this.tabControl1.TabIndex = 1;
            // 
            // diagramTabPage
            // 
            this.diagramTabPage.Controls.Add(this.listView);
            this.diagramTabPage.Location = new System.Drawing.Point(4, 23);
            this.diagramTabPage.Name = "diagramTabPage";
            this.diagramTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.diagramTabPage.Size = new System.Drawing.Size(251, 493);
            this.diagramTabPage.TabIndex = 1;
            this.diagramTabPage.Text = "流程图";
            this.diagramTabPage.UseVisualStyleBackColor = true;
            // 
            // listView
            // 
            this.listView.BackColor = System.Drawing.Color.White;
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView.ContextMenuStrip = this.listViewMenu;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "标准组件";
            listViewGroup1.Name = "standardGroup";
            listViewGroup2.Header = "工作流程";
            listViewGroup2.Name = "workflowGroup";
            this.listView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listView.Location = new System.Drawing.Point(3, 3);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(245, 487);
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
            // 
            // listViewMenu
            // 
            this.listViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStrip});
            this.listViewMenu.Name = "listViewMenu";
            this.listViewMenu.Size = new System.Drawing.Size(153, 48);
            this.listViewMenu.Opening += new System.ComponentModel.CancelEventHandler(this.listViewMenu_Opening);
            // 
            // deleteToolStrip
            // 
            this.deleteToolStrip.Name = "deleteToolStrip";
            this.deleteToolStrip.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStrip.Text = "删除";
            this.deleteToolStrip.Click += new System.EventHandler(this.deleteToolStrip_Click);
            // 
            // activityTabPage
            // 
            this.activityTabPage.Controls.Add(this.propertyGridSplitter);
            this.activityTabPage.Location = new System.Drawing.Point(4, 23);
            this.activityTabPage.Name = "activityTabPage";
            this.activityTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.activityTabPage.Size = new System.Drawing.Size(251, 493);
            this.activityTabPage.TabIndex = 0;
            this.activityTabPage.Text = "活动项";
            this.activityTabPage.UseVisualStyleBackColor = true;
            // 
            // propertyGridSplitter
            // 
            this.propertyGridSplitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.propertyGridSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridSplitter.Location = new System.Drawing.Point(3, 3);
            this.propertyGridSplitter.Name = "propertyGridSplitter";
            this.propertyGridSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // propertyGridSplitter.Panel2
            // 
            this.propertyGridSplitter.Panel2.Controls.Add(this.propertyGrid);
            this.propertyGridSplitter.Size = new System.Drawing.Size(245, 487);
            this.propertyGridSplitter.SplitterDistance = 244;
            this.propertyGridSplitter.TabIndex = 1;
            this.propertyGridSplitter.Text = "splitContainer2";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(243, 237);
            this.propertyGrid.TabIndex = 1;
            // 
            // WorkflowDesignerControl
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Name = "WorkflowDesignerControl";
            this.Size = new System.Drawing.Size(659, 522);
            this.panel1.ResumeLayout(false);
            this.workflowViewSplitter.Panel1.ResumeLayout(false);
            this.workflowViewSplitter.Panel2.ResumeLayout(false);
            this.workflowViewSplitter.ResumeLayout(false);
            this.tabPageMenuStrip.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.diagramTabPage.ResumeLayout(false);
            this.listViewMenu.ResumeLayout(false);
            this.activityTabPage.ResumeLayout(false);
            this.propertyGridSplitter.Panel2.ResumeLayout(false);
            this.propertyGridSplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer workflowViewSplitter;
        private System.Windows.Forms.TabControl workflowViewTabControl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage activityTabPage;
        private System.Windows.Forms.TabPage diagramTabPage;
        private System.Windows.Forms.SplitContainer propertyGridSplitter;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ContextMenuStrip tabPageMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeOthersMenuItem;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ContextMenuStrip listViewMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStrip;
    }
}
