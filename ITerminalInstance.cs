using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;

namespace Netgao.Telephony.Workflow
{
    public interface ITerminalInstance
	{
        Guid UniqueId { get; }
        int Id { get; }
        string Pad { get; }
        string Number { get; }
        int Type { get; }
        int State { get; }
        string Groups { get; }
        string DtmfString { get; }
        ITerminalInstance SetVariable(string name, object value);
        T GetVariable<T>(string name);
        ITerminalInstance Copy();
        ITerminalInstance Copy(Guid instanceId);
	}
}
