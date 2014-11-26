using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Serialization;

namespace Netgao.Telephony.Workflow.Activities
{
    internal class RuleExpressionValidator : ConditionValidator
	{
        public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
        {
            return ValidateProperties(manager, obj);
        }
	}
}
