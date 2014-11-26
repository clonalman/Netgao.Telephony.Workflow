using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Workflow;
using System.Workflow.Runtime;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow
{
    using Netgao.Telephony.Workflow.Activities;

    [Serializable]
    public sealed class UccTerminalService : ITerminalService
    {
        public UccTerminalService()
        {
            Terminals = new TerminalCollection();
            WorkItems = new Queue<TerminalEventArgs>();
        }

        public TerminalCollection Terminals
        {
            get;
            private set;
        }

        public Queue<TerminalEventArgs> WorkItems
        {
            get;
            private set;
        }

        public event EventHandler<TerminalEventArgs> Started;
        public event EventHandler<TerminalEventArgs> Completed;
        public event EventHandler<TerminalEventArgs> Terminated;
        public event EventHandler<TerminalEventArgs> Timeout;

        public void Pickup(Guid uniqueId)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                lock (WorkItems)
                {
                    WorkItems.Enqueue(state as PickupEventArgs);
                }
            }, new PickupEventArgs(Terminals.Single(p => p.UniqueId == uniqueId).Copy()));
        }

        public void Hangup(Guid uniqueId)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                lock (WorkItems)
                {
                    WorkItems.Enqueue(state as HangupEventArgs);
                }
            }, new HangupEventArgs(Terminals.Single(p => p.UniqueId == uniqueId).Copy()));
        }

        public void Queue(Guid uniqueId, int priority)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                lock (WorkItems)
                {
                    WorkItems.Enqueue(state as QueueExEventArgs);
                }
            }, new QueueExEventArgs(Terminals.Single(p => p.UniqueId == uniqueId).Copy(), priority));
        }

        public void Clear(Guid uniqueId)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                lock (WorkItems)
                {
                    WorkItems.Enqueue(state as ClearEventArgs);
                }
            }, new ClearEventArgs(Terminals.Single(p => p.UniqueId == uniqueId).Copy()));
        }

        public void Talk(Guid uniqueId, int volume, bool recording)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                lock (WorkItems)
                {
                    WorkItems.Enqueue(state as TalkEventArgs);
                }
            }, new TalkEventArgs(Terminals.Single(p => p.UniqueId == uniqueId).Copy(), volume, recording));
        }

        public void BlindTransfer(Guid uniqueId, string calleeId)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                lock (WorkItems)
                {
                    WorkItems.Enqueue(state as BlindTransferEventArgs);
                }
            }, new BlindTransferEventArgs(Terminals.Single(p => p.UniqueId == uniqueId).Copy(), calleeId));
        }

        public void SendTone(Guid uniqueId, int toneType, int delay)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                lock (WorkItems)
                {
                    WorkItems.Enqueue(state as SendToneEventArgs);
                }
            }, new SendToneEventArgs(Terminals.Single(p => p.UniqueId == uniqueId).Copy(), toneType, delay));
        }

        public void Play(Guid uniqueId, string fileName, bool stopOnDTMF)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                lock (WorkItems)
                {
                    WorkItems.Enqueue(state as PlayEventArgs);
                }
            }, new PlayEventArgs(Terminals.Single(p => p.UniqueId == uniqueId).Copy(), fileName, stopOnDTMF));
        }

        public void Log(Guid uniqueId, string logName)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                lock (WorkItems)
                {
                    WorkItems.Enqueue(state as LogEventArgs);
                }
            }, new LogEventArgs(Terminals.Single(p => p.UniqueId == uniqueId).Copy(), logName));
        }

        public void RaiseStartedEvent(TerminalEventArgs args)
        {
            if (Started != null)
                Started(this, args);
        }

        public void RaiseCompletedEvent(TerminalEventArgs args)
        {
            if (Completed != null)
                Completed(this, args);
        }

        public void RaiseTerminatedEvent(TerminalEventArgs args)
        {
            if (Terminated != null)
                Terminated(this, args);
        }

        public void RaiseTimeoutEvent(TerminalEventArgs args)
        {
            if (Timeout != null)
                Timeout(this, args);
        }
    }
}
