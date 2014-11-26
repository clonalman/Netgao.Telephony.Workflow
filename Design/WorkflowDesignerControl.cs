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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.Activities;
using System.Workflow.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace Netgao.Telephony.Workflow.Design
{
    public partial class WorkflowDesignerControl : UserControl, IDisposable, IServiceProvider, ISite
    {
        private static readonly string DefaultPath = System.AppDomain.CurrentDomain.BaseDirectory;

        public WorkflowDesignerControl()
        {
            InitializeComponent();

            ToolboxService toolbox = new ToolboxService(this);
            this.propertyGridSplitter.Panel1.Controls.Add(toolbox);
            toolbox.Dock = DockStyle.Fill;
            toolbox.BackColor = BackColor;

            WorkflowTheme.CurrentTheme.ReadOnly = false;
            WorkflowTheme.CurrentTheme.AmbientTheme.ShowConfigErrors = true;

            this.propertyGrid.BackColor = BackColor;
            this.propertyGrid.Site = this;
            this.NameSpace = "Netgao.Telephony.Ivr";
        }

        internal PropertyGrid PropertyGrid
        {
            get { return propertyGrid; }
        }

        [DefaultValue("Netgao.Telephony.Ivr")]
        public string NameSpace
        {
            get;
            set;
        }

        public WorkflowView WorkflowView
        {
            get
            {
                if (workflowViewTabControl.TabCount > 0 &&
                    workflowViewTabControl.SelectedTab != null &&
                    workflowViewTabControl.SelectedTab.Controls.Count > 0)
                {
                    return workflowViewTabControl.SelectedTab.Controls[0] as WorkflowView;
                }
                else return null;
            }
        }

        public string Xoml
        {
            get
            {
                if (workflowViewTabControl.TabCount > 0 &&
                    workflowViewTabControl.SelectedTab != null)
                {
                    return workflowViewTabControl.SelectedTab.Name;
                }
                else return String.Empty;
            }
        }

        protected override object GetService(Type service)
        {
            if (workflowViewTabControl.TabCount > 0)
            {
                IServiceProvider serviceProvider = workflowViewTabControl.SelectedTab as IServiceProvider;
                if (serviceProvider != null)
                {
                    return serviceProvider.GetService(service);
                }
            }
            return base.GetService(service);
        }
                
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ShowDefaultWorkflow();
            LoadXomlFiles();            
        }

        public void LoadXomlFiles()
        {
            InitializeView(new DirectoryInfo(Path.Combine(DefaultPath, "Workflows")));
        }

        private void InitializeView(DirectoryInfo info)
        {
            listView.Bounds = new Rectangle(new Point(8, 8), new Size(200, 100));
            listView.View = View.Details;
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.LabelEdit = false;
            listView.AllowColumnReorder = true;
            listView.CheckBoxes = true;
            listView.FullRowSelect = true;
            listView.MultiSelect = false;
            listView.GridLines = true;
            listView.Sorting = SortOrder.Ascending;
            listView.Clear();

            listView.Columns.Add("Name", 150, HorizontalAlignment.Left);
            listView.Columns.Add("Description", 120, HorizontalAlignment.Left);

            foreach (FileInfo fileInfo in info.GetFiles("*.xoml"))
            {
                using (XmlReader reader = new XmlTextReader(fileInfo.FullName))
                {
                    Activity rootActivity = null;
                    try
                    {
                        WorkflowMarkupSerializer xomlSerializer = new WorkflowMarkupSerializer();
                        rootActivity = xomlSerializer.Deserialize(reader) as Activity;
                        if (rootActivity.Enabled)
                        {
                            ListViewItem item = new ListViewItem(rootActivity.Name, 0);
                            item.Group = listView.Groups["workflowGroup"];
                            item.Checked = String.Compare(fileInfo.Name,"Default.xoml", true) != 0;
                            item.Tag = fileInfo;
                            item.SubItems.Add(rootActivity.Description);
                            listView.Items.Add(item);
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }
        }

        public void AddPage(TabPage tabPage)
        {
            workflowViewTabControl.TabPages.Add(tabPage);
            workflowViewTabControl.SelectedTab = tabPage;
        }

        public void LoadWorkflow(string xoml)
        {
            WorkflowPage tabPage = null;

            int index = workflowViewTabControl.TabPages.IndexOfKey(xoml);
            if (index == -1)
            {
                tabPage = new WorkflowPage(propertyGrid) 
                { 
                  Name = xoml, 
                  ToolTipText = xoml, 
                  Text = Path.GetFileName(xoml),
                  NameSpace = NameSpace,
                  TypeName = Path.GetFileNameWithoutExtension(xoml)
                };
                AddPage(tabPage);
            }
            else
            {
                tabPage = this.workflowViewTabControl.TabPages[index] as WorkflowPage;
                workflowViewTabControl.SelectedTab = tabPage;
            }  
            tabPage.LoadWorkflow(xoml);
        }

       
        public void ShowDefaultWorkflow()
        {
            ShowWorkflow("Default");
        }

        public void ShowWorkflow(string name)
        {

            string path = Path.Combine(DefaultPath, @"Workflows");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, name + @".xoml");

            if (!File.Exists(path))
            {
                SequentialWorkflowActivity workflow = new SequentialWorkflowActivity();
                workflow.Name = name;
                workflow.SetValue(WorkflowMarkupSerializer.XClassProperty, "Netgao.Telephony.Ivr." + name);
                using (XmlWriter xmlWriter = XmlTextWriter.Create(path))
                {
                    WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                    serializer.Serialize(xmlWriter, workflow);
                    xmlWriter.Close();
                }
            }
            LoadWorkflow(path);
        }


        public void InvokeStandardCommand(CommandID cmd)
        {
            IMenuCommandService menuService = GetService(typeof(IMenuCommandService)) as IMenuCommandService;
            if (menuService != null)
                menuService.GlobalInvoke(cmd);
        }


        public void Save(string filePath)
        {
            WorkflowPage page = workflowViewTabControl.SelectedTab as WorkflowPage;
            if (page != null)
            {
                if (!String.IsNullOrEmpty(filePath) && page.Name != filePath)
                {
                    page.Save(filePath);
                    page.Name = filePath;
                    page.Text = Path.GetFileName(filePath);
                    page.ToolTipText = filePath;
                }
                else
                {
                    page.Save(filePath);
                }
            }
        }

        /// <summary>
        /// We listen to the event when the property browser gets the focus and save the xaml
        /// Reason: This is done to support using activities in the rule set editor. In VS, the xaml file is 
        /// flushed when the application is idle or when you save or change the view to new file. Only after the
        /// file is flushed, the type provider is updated with the contents of the xaml file. Hence in this case
        /// we listen to the event where the property grid receives the focus and do a save. This will update the
        /// type provider with the contents of the xaml file (i.e the activities present in the xaml) and the activities
        /// can now be used in the rule editor which works off the type provider. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void propertyGrid_GotFocus(object sender, System.EventArgs e)
        {
            Save(null);
        }

        private string[] GetSelectedFiles()
        {
            var selectedItems = listView.Items.Cast<ListViewItem>().Where(p => p.Checked);
            return selectedItems.Select(p => (p.Tag as FileInfo).FullName).ToArray();
        }

        /// <summary>
        /// Compile the workflow along with the code beside file and the rules file if they exist
        /// </summary>
        public void CompileWorkflow()
        {
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Check for code beside file and rules file
                WorkflowCompiler compiler = new WorkflowCompiler();
                WorkflowCompilerParameters parameters = new WorkflowCompilerParameters();
                parameters.LibraryPaths.Add(DefaultPath);
                parameters.ReferencedAssemblies.Add("Netgao.Telephony.Workflow.dll");
                //parameters.CompilerOptions += "/optimize+ /debug-";

                List<string> files = new List<string>();

                foreach (string fileName in GetSelectedFiles())
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    string codeBesideFile = Path.Combine(Path.GetDirectoryName(fileName), fileNameWithoutExtension + ".cs");
                    string rulesFile = Path.Combine(Path.GetDirectoryName(fileName), fileNameWithoutExtension + ".rules");

                    files.Add(fileName);

                    if (File.Exists(codeBesideFile))
                        files.Add(codeBesideFile);

                    if (File.Exists(rulesFile))
                    {
                        // adding the rules file to the resources
                        string resources = @" /resource:" + rulesFile + "," + this.NameSpace + "." + fileNameWithoutExtension + "." + "rules";
                        parameters.CompilerOptions += resources;
                    }
                }

                // Compile the workflow
                parameters.GenerateInMemory = false;
                parameters.OutputAssembly = Path.Combine(DefaultPath, NameSpace + ".dll");
                WorkflowCompilerResults results = compiler.Compile(parameters, files.ToArray());

                StringBuilder errors = new StringBuilder();
                foreach (CompilerError compilerError in results.Errors)
                {
                    errors.Append(compilerError.ToString() + '\n');
                }

                if (errors.Length != 0)
                {
                    MessageBox.Show(this, errors.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //MessageBox.Show(this, "Workflow compiled successfully. Compiled assembly: \n" + results.CompiledAssembly.GetName(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //MessageBox.Show(this, "流程编译完成！ 编译的组件名：\n" + results.CompiledAssembly.GetName(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(this, "流程编译完成！\t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            finally
            {
                this.Cursor = cursor;
            }
        }

       

        #region ISite Members

        public IComponent Component
        {
            get { return this; }
        }

        public new bool DesignMode
        {
            get { return true; }
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return this.GetService(serviceType);
        }

        #endregion

        private void workflowViewTabControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl != null)
            {
                for (int i = 0; i < tabControl.TabCount; i++)
                {
                    Rectangle recTab = tabControl.GetTabRect(i);
                    if (recTab.Contains(e.Location))
                    {
                        tabControl.TabPages.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        private void workflowViewTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    for (int i = 0; i < tabControl.TabCount; i++)
                    {
                        Rectangle recTab = tabControl.GetTabRect(i);
                        if (recTab.Contains(e.Location))
                        {
                            tabControl.SelectedIndex = i;
                            return;
                        }
                    }
                }
            }
        }

        private void tabPageMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "closeMenuItem":
                    {
                        if (workflowViewTabControl.SelectedTab != null)
                        {
                            workflowViewTabControl.TabPages.Remove(workflowViewTabControl.SelectedTab);
                        }
                    } break;
                case "closeOthersMenuItem":
                    {
                        var tabPages = workflowViewTabControl.TabPages.OfType<TabPage>();
                        Array.ForEach(tabPages.ToArray(), (tabPage) => {
                            if (workflowViewTabControl.SelectedTab != tabPage)
                            {
                                workflowViewTabControl.TabPages.Remove(tabPage);
                            }
                        });
                    } break;
            }
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                item.Checked = true;
                FileInfo fileInfo = item.Tag as FileInfo;
                if (fileInfo != null)
                {
                    LoadWorkflow(fileInfo.FullName);
                }
            }
        }

        private void deleteToolStrip_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem item = listView.SelectedItems[0];
                if (item != null)
                {
                    FileInfo fileInfo = item.Tag as FileInfo;
                    if (fileInfo != null)
                    {
                        if (MessageBox.Show(this, String.Format("真的要删除{0}吗！\t", fileInfo.Name), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            fileInfo.Delete();
                            LoadXomlFiles();
                        }
                        
                    }
                }
            }
            
        }

        private void listViewMenu_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = listView.SelectedItems.Count == 0;
        }

        

       
    }
}
