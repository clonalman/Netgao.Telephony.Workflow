using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Workflow.Activities.Rules.Design;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace Netgao.Telephony.Workflow.Activities
{
    internal sealed class RuleExpressionTypeConverter : TypeConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(new PropertyDescriptor[] 
                { 
                    new RuleExpressionPropertyDescriptor(context, TypeDescriptor.CreateProperty(typeof(RuleExpression), "ConditionName", typeof(string), 
                        new Attribute[] { new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content), new ValidationOptionAttribute(ValidationOption.None), DesignOnlyAttribute.Yes })),
                    new RuleExpressionPropertyDescriptor(context, TypeDescriptor.CreateProperty(typeof(RuleExpression), "Expression", typeof(CodeExpression), 
                        new Attribute[] { new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content), new ValidationOptionAttribute(ValidationOption.None), DesignOnlyAttribute.Yes }))
                });
        }

        
        private class RuleExpressionPropertyDescriptor : SimplePropertyDescriptor
        {
            private ITypeDescriptorContext context;
            private PropertyDescriptor descriptor;

            public RuleExpressionPropertyDescriptor(ITypeDescriptorContext context, PropertyDescriptor descriptor)
                : base(descriptor.ComponentType, descriptor.Name, descriptor.PropertyType)
            {
                this.context = context;
                this.descriptor = descriptor;
            }

            public override object GetValue(object component)
            {
                RuleExpression ruleExpression = component as RuleExpression;
                if (ruleExpression != null)
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(RuleConditionReference));
                    PropertyDescriptorCollection properties = converter.GetProperties(context, ruleExpression.Reference);
                    if (properties != null)
                    {
                        PropertyDescriptor property = properties.Find(Name, true);
                        if (property != null)
                        {
                            return property.GetValue(ruleExpression.Reference);
                        }
                    }
                }
                return null;
            }

            public override void SetValue(object component, object value)
            {
                RuleExpression ruleExpression = component as RuleExpression;
                if (ruleExpression != null)
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(RuleConditionReference));
                    PropertyDescriptorCollection properties = converter.GetProperties(context, ruleExpression.Reference);
                    if (properties != null)
                    {
                        PropertyDescriptor property = properties.Find(Name, true);
                        if (property != null)
                        {
                            property.SetValue(ruleExpression.Reference, value);
                        }
                    }
                }
            }
        }
    }
}
