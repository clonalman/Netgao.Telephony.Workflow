using System;
using System.Resources;
using System.Reflection;
using System.ComponentModel;

internal sealed class Scc
{
    public const string StringResources = "Netgao.Telephony.Workflow.Properties.Resources";
}

[AttributeUsage(AttributeTargets.All)]
internal sealed class SccCategoryAttribute : CategoryAttribute
{
    private string resourceSet;

    public SccCategoryAttribute(string category)
        : this(category, Scc.StringResources)
    {
        
    }

    public SccCategoryAttribute(string category, string resourceSet)
        : base(category)
    {
        this.resourceSet = string.Empty;
        this.resourceSet = resourceSet;
    }

    protected override string GetLocalizedString(string value)
    {
        if (this.resourceSet.Length > 0)
        {
            ResourceManager manager = new ResourceManager(this.resourceSet, Assembly.GetExecutingAssembly());
            return manager.GetString(value);
        }
        return value;
    }
}

[AttributeUsage(AttributeTargets.All)]
internal sealed class SccDescriptionAttribute : DescriptionAttribute
{
    public SccDescriptionAttribute(string description)
        : this(description, Scc.StringResources)
    {
        
    }

    public SccDescriptionAttribute(string description, string resourceSet)
    {
        base.DescriptionValue = new ResourceManager(resourceSet, Assembly.GetExecutingAssembly()).GetString(description);
    }
}

