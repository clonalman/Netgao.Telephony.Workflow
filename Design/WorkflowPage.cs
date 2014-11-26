using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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
    public partial class WorkflowPage : TabPage, IServiceProvider
	{
        private string typeName;
        private string nameSpace;
        private WorkflowView workflowView;
        private DesignSurface designSurface;
        private WorkflowLoader loader;
        private PropertyGrid propertyGrid;

        public WorkflowPage(PropertyGrid propertyGrid)
		{
			InitializeComponent();
            this.propertyGrid = propertyGrid;
		}

        public WorkflowView WorkflowView
        {
            get
            {
                return workflowView;
            }
        }

        public string NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public string Xoml
        {
            get
            {
                string xoml = String.Empty;
                if (this.loader != null)
                {
                    try
                    {
                        this.loader.Flush();
                        xoml = this.loader.Xoml;
                    }
                    catch
                    {
                    }
                }
                return xoml;
            }

            set
            {
                try
                {
                    if (!String.IsNullOrEmpty(value))
                        LoadWorkflow(value);
                }
                catch
                {
                }
            }
        }

        public void LoadWorkflow(string xoml)
        {
            SuspendLayout();


            designSurface = new DesignSurface();
            loader = new WorkflowLoader();
            loader.Xoml = xoml;

            designSurface.BeginLoad(loader);

            IDesignerHost designerHost = designSurface.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designerHost != null && designerHost.RootComponent != null)
            {
                IRootDesigner rootDesigner = designerHost.GetDesigner(designerHost.RootComponent) as IRootDesigner;
                if (rootDesigner != null)
                {
                    UnloadWorkflow();

                    workflowView = rootDesigner.GetView(ViewTechnology.Default) as WorkflowView;
                    Controls.Add(workflowView);
                    workflowView.Dock = DockStyle.Fill;
                    workflowView.TabIndex = 1;
                    workflowView.TabStop = true;
                    workflowView.HScrollBar.TabStop = false;
                    workflowView.VScrollBar.TabStop = false;
                    workflowView.Focus();

                    if (propertyGrid != null)
                    {
                        propertyGrid.Site = designerHost.RootComponent.Site;
                    }

                    ISelectionService selectionService = GetService(typeof(ISelectionService)) as ISelectionService;
                    if (selectionService != null)
                    {
                        selectionService.SelectionChanged += new EventHandler(OnSelectionChanged);
                    }
                }
            }

            ResumeLayout(true);

            /*
            // Add the code compile unit for the xaml file
            TypeProvider typeProvider = (TypeProvider)GetService(typeof(ITypeProvider));
            if (typeProvider != null)
            {
                this.loader.XamlCodeCompileUnit = new CodeCompileUnit();
                this.loader.XamlCodeCompileUnit.Namespaces.Add(Helpers.GenerateCodeFromXomlDocument(Helpers.GetRootActivity(xoml), this, ref this.nameSpace, ref this.typeName));
                typeProvider.AddCodeCompileUnit(this.loader.XamlCodeCompileUnit);
            }
            */
        }

 
        private void UnloadWorkflow()
        {
            IDesignerHost designerHost = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designerHost != null && designerHost.Container.Components.Count > 0)
                WorkflowLoader.DestroyObjectGraphFromDesignerHost(designerHost, designerHost.RootComponent as Activity);

            ISelectionService selectionService = GetService(typeof(ISelectionService)) as ISelectionService;
            if (selectionService != null)
                selectionService.SelectionChanged -= new EventHandler(OnSelectionChanged);


            if (workflowView != null)
            {
                Controls.Remove(workflowView);
                workflowView.Dispose();
                workflowView = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                UnloadWorkflow();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            ISelectionService selectionService = GetService(typeof(ISelectionService)) as ISelectionService;
            if (selectionService != null)
            {
                if (propertyGrid != null)
                {
                    propertyGrid.SelectedObjects = new ArrayList(selectionService.GetSelectedComponents()).ToArray();
                    propertyGrid.PropertySort = PropertySort.Categorized;

                }
            }
        }

        public void SaveToPNG()
        {
            //using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            //{
            //    saveFileDialog.Filter = "Png files (*.Png)|*.Png|All files (*.*)|*.*";
            //    saveFileDialog.FilterIndex = 1;
            //    saveFileDialog.RestoreDirectory = true;
            //    saveFileDialog.FileName = loader.RootActivity.Name;
            //    saveFileDialog.ShowDialog();
            //    workflowView.SaveWorkflowImage(saveFileDialog.FileName, ImageFormat.Png);
            //}
        }
        /// <summary>
        /// Save the file. We also refresh the type provider when we save the file
        /// </summary>
        /// <param name="filePath">The path of the file to save</param>
        public void Save(string filePath)
        {
            if (this.loader != null)
            {
                if (!String.IsNullOrEmpty(filePath))
                    this.loader.Xoml = filePath;
                this.loader.Flush();
            }

            /*
            // Referesh the code compile unit every time the file is saved
            TypeProvider typeProvider = (TypeProvider)GetService(typeof(ITypeProvider));
            if (typeProvider != null)
            {
                typeProvider.RemoveCodeCompileUnit(this.loader.XamlCodeCompileUnit);
                this.loader.XamlCodeCompileUnit = new CodeCompileUnit();
                this.loader.XamlCodeCompileUnit.Namespaces.Add(Helpers.GenerateCodeFromXomlDocument(Helpers.GetRootActivity(filePath != null ? filePath : this.loader.Xoml), this, ref this.nameSpace, ref this.typeName));
                typeProvider.AddCodeCompileUnit(this.loader.XamlCodeCompileUnit);
            }
             * */
             
        }

        protected override object GetService(Type service)
        {
            return base.GetService(service) ?? (this as IServiceProvider).GetService(service);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            IServiceProvider serviceProvider = workflowView as IServiceProvider;
            return serviceProvider != null ? serviceProvider.GetService(serviceType) : null;
        }        
    }
}
