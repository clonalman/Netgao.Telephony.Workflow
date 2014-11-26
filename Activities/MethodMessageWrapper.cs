using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Workflow;
using System.Workflow.ComponentModel;


namespace Netgao.Telephony.Workflow.Activities
{
    internal class MethodMessageWrapper : InternalMessageWrapper, IMethodMessage, IMessage
	{
        public MethodMessageWrapper(IMethodMessage msg, object item)
            : base(msg)
        {
            this.Item = item;
        }

        public object Item
        {
            get;
            private set;
        }

        public bool Verify(object item)
        {
            MethodMessageWrapper wrapper = WrappedMessage as MethodMessageWrapper;
            if (wrapper != null)
            {
                return wrapper.Item == item || wrapper.Verify(item);
            }
            else return false;
        }

        public IMethodMessage GetWrappedMessage(bool deep)
        {
            if (deep)
            {
                MethodMessageWrapper wrapper = WrappedMessage as MethodMessageWrapper;
                if (wrapper != null)
                {
                    return wrapper.GetWrappedMessage(deep);
                }
            }
            return WrappedMessage as IMethodMessage;
        }

        public int WrappedCount
        {
            get
            {
                MethodMessageWrapper wrapper = WrappedMessage as MethodMessageWrapper;
                if (wrapper != null)
                {
                    return 1 + wrapper.WrappedCount;
                }
                else return 0;
            }
        }

        #region IMethodMessage 成员

        public int ArgCount
        {
            get { return (WrappedMessage as IMethodMessage).ArgCount; }
        }

        public object[] Args
        {
            get { return (WrappedMessage as IMethodMessage).Args; }
        }

        public object GetArg(int argNum)
        {
            return (WrappedMessage as IMethodMessage).GetArg(argNum);
        }

        public string GetArgName(int index)
        {
            return (WrappedMessage as IMethodMessage).GetArgName(index);
        }

        public bool HasVarArgs
        {
            get { return (WrappedMessage as IMethodMessage).HasVarArgs; }
        }

        public LogicalCallContext LogicalCallContext
        {
            get { return (WrappedMessage as IMethodMessage).LogicalCallContext; }
        }

        public MethodBase MethodBase
        {
            get { return (WrappedMessage as IMethodMessage).MethodBase; }
        }

        public string MethodName
        {
            get { return (WrappedMessage as IMethodMessage).MethodName; }
        }

        public object MethodSignature
        {
            get { return (WrappedMessage as IMethodMessage).MethodSignature; }
        }

        public string TypeName
        {
            get { return (WrappedMessage as IMethodMessage).TypeName; }
        }

        public string Uri
        {
            get { return (WrappedMessage as IMethodMessage).Uri; }
        }

        #endregion

        #region IMessage 成员

        public IDictionary Properties
        {
            get { return (WrappedMessage as IMethodMessage).Properties; }
        }

        #endregion
    }
}
