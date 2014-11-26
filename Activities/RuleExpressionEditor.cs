using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Workflow;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Workflow.Activities.Rules.Design;


namespace Netgao.Telephony.Workflow.Activities
{
    using Netgao.Telephony.Workflow.Design;

    internal class RuleExpressionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider serviceProvider, object value)
        {
            if (context == null || serviceProvider == null || context.Instance == null)
            {
                return base.EditValue(serviceProvider, value);
            }

            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)serviceProvider.GetService(typeof(IWindowsFormsEditorService));
            if (editorService != null)
            {
                Activity activity  = null;
                IReferenceService service = serviceProvider.GetService(typeof(IReferenceService)) as IReferenceService;
                if (service != null)
                {
                    activity  = service.GetComponent(context.Instance) as Activity;
                }

                if (activity  != null)
                {
                    ITypeProvider typeProvider = (ITypeProvider)serviceProvider.GetService(typeof(ITypeProvider));
                    RuleExpression ruleExpression = value as RuleExpression;
                    CodeExpression expression = null;
                    if (ruleExpression != null)
                    {
                        expression = (CodeExpression)ruleExpression.GetValue(context, "Expression");
                    }
                    else
                    {
                        //var properties = DependencyProperty.FromType(activity.GetType());
                        //foreach (var property in properties)
                        //{
                        //    if (!property.IsEvent)
                        //    {
                        //        expression = GetCodeExpression(expression, property.Name, activity.GetValue(property));
                        //    }
                        //}
                    }

                    using (RuleConditionDialog dialog = new RuleConditionDialog(activity.GetType(), typeProvider, expression))
                    {
                        if (editorService.ShowDialog(dialog) == DialogResult.OK)
                        {
                            string conditionName = activity.QualifiedName + "." + context.PropertyDescriptor.Name;
                            if (ruleExpression == null)
                            {
                                ruleExpression = new RuleExpression();
                                ruleExpression.Reference = new RuleConditionReference() { ConditionName = conditionName };
                            }
                            IDesignerHost host = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
                            context = new TypeDescriptorContext(host, context.PropertyDescriptor, context.Instance);
                            ruleExpression.SetValue(context, "ConditionName", conditionName);
                            ruleExpression.SetValue(context, "Expression", dialog.Expression);
                            return ruleExpression;
                        }
                    }
                }
            }
            return value;
        }

        private CodeExpression GetCodeExpression(CodeExpression codeExpression, string name, object value)
        {
            CodePropertyReferenceExpression left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), name);
            CodePrimitiveExpression right = new CodePrimitiveExpression(value);
            CodeBinaryOperatorExpression binary = new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.ValueEquality, right);

            if (codeExpression == null)
            {
                codeExpression = binary;
            }
            else
            {
                codeExpression = new CodeBinaryOperatorExpression(codeExpression, CodeBinaryOperatorType.BooleanAnd, binary);
            }
            return codeExpression;
        }
    }
}
