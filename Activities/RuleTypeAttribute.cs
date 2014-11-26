using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netgao.Telephony.Workflow.Activities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RuleTypeAttribute : Attribute
    {
        public RuleTypeAttribute(Type type)
        {
            this.Type = type;
        }

        public Type Type
        {
            get;
            private set;
        }
    }
}
