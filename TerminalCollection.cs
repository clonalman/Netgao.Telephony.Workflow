using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Netgao.Telephony.Workflow
{
    [Serializable]
    public class TerminalCollection : KeyedCollection<Guid, ITerminalInstance>
	{
        public TerminalCollection()
            : base()
        {

        }
        protected override Guid GetKeyForItem(ITerminalInstance item)
        {
            return item.UniqueId;
        }
    }
}
