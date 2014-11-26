using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Xml;
using Microsoft.CSharp;

namespace Netgao.Telephony.Workflow.Activities
{
    [ToolboxItem(false)]
    [ActivityValidator(typeof(RuleExpressionValidator))]
    [DesignerSerializer(typeof(WorkflowMarkupSerializer), typeof(WorkflowMarkupSerializer))]
    [DesignerSerializer(typeof(DependencyObjectCodeDomSerializer), typeof(CodeDomSerializer))]
    [TypeConverter(typeof(RuleExpressionTypeConverter))]
    [Editor(typeof(RuleExpressionEditor), typeof(UITypeEditor))]
    public class RuleExpression : ActivityCondition
	{
        public static readonly DependencyProperty ReferenceProperty = DependencyProperty.Register("Reference", typeof(RuleConditionReference), typeof(RuleExpression));

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public RuleConditionReference Reference
        {
            get { return (RuleConditionReference)base.GetValue(ReferenceProperty); }
            set { base.SetValue(ReferenceProperty, value); }
        }

        public override bool Evaluate(Activity activity, IServiceProvider provider)
        {
            RuleExpressionCondition condition = GetRuleConditionFromManifest(activity, Reference.ConditionName);

            if (condition != null)
            {
                RuleValidation validation = new RuleValidation(activity.GetType(), null);
                if (condition.Validate(validation))
                {
                    return condition.Evaluate(new RuleExecution(validation, activity, provider as ActivityExecutionContext));
                }
            }
            return false;
        }

        public object GetValue(ITypeDescriptorContext context, string name)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(RuleExpression));
            PropertyDescriptorCollection properties = converter.GetProperties(context, this);
            if (properties != null)
            {
                PropertyDescriptor property = properties.Find(name, true);
                if (property != null)
                {
                    return property.GetValue(this);
                }
            }
            return null;
        }

        public void SetValue(ITypeDescriptorContext context, string name, object value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(RuleExpression));
            PropertyDescriptorCollection properties = converter.GetProperties(context, this);
            if (properties != null)
            {
                PropertyDescriptor property = properties.Find(name, true);
                if (property != null)
                {
                    property.SetValue(this, value);
                }
            }
        }

        private static RuleExpressionCondition GetRuleConditionFromManifest(Activity activity, string conditionName)
        {
            Activity rootActivity = GetRootActivity(activity);
            if (rootActivity != null)
            {
                RuleDefinitions ruleDefinitions = GetRuleDefinitionsFromManifest(rootActivity.GetType());

                if (ruleDefinitions != null)
                {
                    RuleConditionCollection conditions = ruleDefinitions.Conditions;
                    if ((conditions != null) && conditions.Contains(conditionName))
                    {
                        return (RuleExpressionCondition)conditions[conditionName];
                    }
                }
            }
            return null;
        }

        private static RuleDefinitions GetRuleDefinitionsFromManifest(Type workflowType)
        {
            if (workflowType == null)
            {
                throw new ArgumentNullException("workflowType");
            }
            RuleDefinitions definitions = null;
            if (rulesResources.ContainsKey(workflowType))
            {
                definitions = (RuleDefinitions)rulesResources[workflowType];
            }
            else
            {
                string name = workflowType.Name + ".rules";
                Stream manifestResourceStream = workflowType.Module.Assembly.GetManifestResourceStream(workflowType, name);
                if (manifestResourceStream == null)
                {
                    manifestResourceStream = workflowType.Module.Assembly.GetManifestResourceStream(name);
                }
                if (manifestResourceStream != null)
                {
                    using (StreamReader reader = new StreamReader(manifestResourceStream))
                    {
                        using (XmlReader reader2 = XmlReader.Create(reader))
                        {
                            definitions = new WorkflowMarkupSerializer().Deserialize(reader2) as RuleDefinitions;
                        }
                    }
                }
                lock (rulesResources)
                {
                    rulesResources[workflowType] = definitions;
                }
            }
            return definitions;
        }

        private static Activity GetRootActivity(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException("activity");
            }
            while (activity.Parent != null)
            {
                activity = activity.Parent;
            }
            return activity;
        }

        private static Hashtable rulesResources =  Hashtable.Synchronized(new Hashtable());


        private string ConvertCodeDomExpressionToCSharpText(CodeExpression expression)
        {
            try
            {
                using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
                {
                    StringWriter result = new StringWriter();

                    CodeGeneratorOptions genOptions = new CodeGeneratorOptions();
                    genOptions.BlankLinesBetweenMembers = false;
                    genOptions.BracingStyle = "C";
                    genOptions.ElseOnClosing = false;
                    genOptions.IndentString = "    ";

                    codeProvider.GenerateCodeFromExpression(expression, result, genOptions);

                    return result.ToString();
                }
            }
            catch
            {
                return null;
            }
        }

        public override string ToString()
        {
            if (Reference != null)
            {
                return Reference.ConditionName;
            }
            else
            {
                return base.ToString();
            }
        }
    }

    
}
