using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Netgao.Telephony.Workflow
{
	public sealed class UccWorkThread<T>
	{
        private bool cancellationPending;
        private int sleep;

        public UccWorkThread(Queue<T> workItems)
            : this(workItems, 50)
        {

        }

        public UccWorkThread(Queue<T> workItems, int sleep)
        {
            this.WorkItems = workItems;
            this.cancellationPending = false;
            this.sleep = sleep;
        }

        public Queue<T> WorkItems
        {
            get;
            private set;
        }

        public void CreateThread(Action<T> callback)
        {
            Thread thread = new Thread(ServeOverLap);
            thread.SetApartmentState(ApartmentState.MTA);
            thread.IsBackground = true;
            thread.Start(callback);
        }

        private void ServeOverLap(object obj)
        {
            Action<T> callback = obj as Action<T>;

            while (!cancellationPending)
            {
                if (WorkItems.Count > 0)
                {
                    lock (WorkItems)
                    {
                        if (WorkItems.Count > 0)
                        {
                            T workItem = WorkItems.Dequeue();
                            if (!Nullable.Equals(workItem, default(T)))
                            {
                                if (callback != null)
                                {
                                    callback(workItem);
                                }
                            }
                        }
                    }

                }
                else
                {
                    Thread.Sleep(sleep);
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                cancellationPending = true;
                GC.SuppressFinalize(this);   
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~UccWorkThread()
        {
            Dispose(false);
        }
	}
}
