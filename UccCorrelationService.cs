using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netgao.Telephony.Workflow
{

    public class UccCorrelationService : ICorrelationService
    {
        public event EventHandler<CorrelationServiceArgs> ChildCompleted;

        public void InitializeCorrelation(string key)
        {
            Console.Out.WriteLine("Key -> [{0}]", key);
        }

        public void OnChildCompleted(string key)
        {
            RaiseChildCompletedEvent(key);
        }

        public void RaiseChildCompletedEvent(string key)
        {
            if (ChildCompleted != null)
            {
                ChildCompleted(null, new CorrelationServiceArgs(new Guid("5D1667BF-61F6-4bf3-81C0-E70CBE15D2EF"), key));
            }
        }
    }
}
