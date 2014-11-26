using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Netgao.Telephony.Workflow.Design
{
    internal sealed class TypeDescriptorContext : ITypeDescriptorContext, IServiceProvider, ISite
    {
        private IDesignerHost _designerHost;
        private object _instance;
        private PropertyDescriptor _propDesc;

        public TypeDescriptorContext(IDesignerHost designerHost, PropertyDescriptor propDesc, object instance)
        {
            this._designerHost = designerHost;
            this._propDesc = propDesc;
            this._instance = instance;
        }

        public object GetService(Type serviceType)
        {
            return this._designerHost.GetService(serviceType);
        }

        public void OnComponentChanged()
        {
            if (this.ComponentChangeService != null)
            {
                this.ComponentChangeService.OnComponentChanged(this._instance, this._propDesc, null, null);
            }
        }

        public bool OnComponentChanging()
        {
            if (this.ComponentChangeService != null)
            {
                try
                {
                    this.ComponentChangeService.OnComponentChanging(this._instance, this._propDesc);
                }
                catch (CheckoutException exception)
                {
                    if (exception != CheckoutException.Canceled)
                    {
                        throw exception;
                    }
                    return false;
                }
            }
            return true;
        }

        private IComponentChangeService ComponentChangeService
        {
            get
            {
                return (IComponentChangeService)this._designerHost.GetService(typeof(IComponentChangeService));
            }
        }

        public IContainer Container
        {
            get
            {
                return (IContainer)this._designerHost.GetService(typeof(IContainer));
            }
        }

        public object Instance
        {
            get
            {
                return this._instance;
            }
        }

        public PropertyDescriptor PropertyDescriptor
        {
            get
            {
                return this._propDesc;
            }
        }

        public IComponent Component
        {
            get
            {
                return _designerHost.RootComponent.Site.Component;
            }
        }

        public bool DesignMode
        {
            get { return _designerHost.RootComponent.Site.DesignMode; }
        }

        public string Name
        {
            get { return _designerHost.RootComponent.Site.Name; }
            set { _designerHost.RootComponent.Site.Name = value; }
        }
    }


}
