/******************************************************************************/
/*                                                                            */
/*    _                ___        _..-._                                      */
/*    \`.|\..----...-'`   `-._.-'' _.-..'                                     */
/*    /  ' `         ,       __.-''                                           */
/*    )/` _/     \   `-_,   /                                                 */
/*    `-'" `"\_  ,_.-;_.-\_ ',                                                */
/*        _.-'_./   {_.'   ; /  Netgao.Telephony.Workflow Library                             */
/*       {_.-``-'         {_/   Copyright  2010-2011, DJGAO                   */
/*                                                                            */
/******************************************************************************/
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Netgao.Telephony.Workflow
{
    using Netgao.Telephony.Workflow.Activities;

    [Serializable]
    public sealed class UccTerminalInstance : ITerminalInstance, ICloneable
	{

        public const string VarUniqueId = "UniqueId";
        public const string VarId = "Id";
        public const string VarPad = "Pad";
        public const string VarNumber = "Number";
        public const string VarState = "State";
        public const string VarType = "Type";
        public const string VarGroups = "Groups";
        public const string VarDtmfString = "DtmfString";
        public const string VarVariables = "Variables";

        
        public UccTerminalInstance(IDictionary<string, object> variables)
        {
            this.SetVariable(VarVariables, variables);
        }

        public UccTerminalInstance(Guid instanceId, IDictionary<string, object> variables)
        {
            this.SetVariable(VarVariables, variables);
            this.SetVariable(VarUniqueId, instanceId);
        }

        public UccTerminalInstance(Guid instanceId)
            : this(instanceId, new Dictionary<string, object>())
        {

        }

        public Guid UniqueId
        {
            get { return GetVariable<Guid>(VarUniqueId); }
        }

        public int Id
        {
            get { return GetVariable<int>(VarId); }
        }

        public string Pad
        {
            get { return GetVariable<string>(VarPad); }
        }

        public string Number 
        {
            get { return GetVariable<string>(VarNumber); }
        }

        public int Type 
        {
            get { return GetVariable<int>(VarType); }
        }

        public int State
        {
            get { return GetVariable<int>(VarState); }
        }

        public string Groups
        {
            get { return GetVariable<string>(VarGroups); }
        }

        public string DtmfString
        {
            get { return GetVariable<string>(VarDtmfString); }
        }

        internal IDictionary<string, object> Variables
        {
            get;
            private set;
        }

        public object Clone()
        {
            return this;
        }

        public ITerminalInstance Copy()
        {
            return new UccTerminalInstance(new Dictionary<string, object>(Variables));
        }

        public ITerminalInstance Copy(Guid instanceId)
        {
            return new UccTerminalInstance(instanceId, new Dictionary<string, object>(Variables));
        }

        public ITerminalInstance SetVariable(string name, object value)
        {
            if (VarVariables == name && value is IDictionary<string, object>)
            {
                IDictionary<string, object> dict = value as IDictionary<string, object>;
                if (Variables != null)
                {
                    foreach (var key in dict.Keys)
                    {
                        if (Variables.ContainsKey(key))
                        {
                            Variables[key] = dict[key];
                        }
                        else
                        {
                            Variables.Add(key, dict[key]);
                        }
                    }
                }
                else
                {
                    Variables = dict;
                }
            }
            else
            {
                if (Variables.ContainsKey(name))
                {
                    Variables[name] = value;
                }
                else
                {
                    Variables.Add(name, value);
                }
            }
            return this;
        }

        public T GetVariable<T>(string name)
        {
            if (Variables.ContainsKey(name))
            {
                return (T)Variables[name];
            }
            else
            {
                return default(T);
            }
        }
    }
}
