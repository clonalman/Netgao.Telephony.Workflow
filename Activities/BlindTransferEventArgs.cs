using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow.Activities
{
    [Serializable]
    public sealed class BlindTransferEventArgs : TerminalEventArgs
    {

        public BlindTransferEventArgs(ITerminalInstance instance, string calleeId)
            : base(instance)
        {
            Channel.SetVariable("CalleeId", calleeId);
        }

        public string CalleeId
        {
            get { return Channel.GetVariable<string>("CalleeId"); }
        }

        public override string ToString()
        {
            return String.Format("BlindTransfer({0}, \"{1}\")", Channel.Pad, CalleeId);
        }
    }
    /*
    public enum RouteType : int
    {
        /// <summary>
        /// 随机
        /// </summary>
        Random,
        /// <summary>
        /// 循环振铃
        /// </summary>
        /// <remarks>
        /// 循环检测各坐席，直到发现空闲的坐席
        /// </remarks>
        Hunting,
        /// <summary>
        /// 集体振铃
        /// </summary>
        /// <remarks>
        /// 在呼叫到达时，同一组的坐席的电话一起振铃
        /// </remarks>
        RingDown
    }

    public enum RouteResult : int
    {
        /// <summary>
        /// 路由失败
        /// </summary>
        Failure,
        /// <summary>
        /// 路由成功
        /// </summary>
        Matching,
        /// <summary>
        /// 路由遇忙
        /// </summary>
        Busy,
        /// <summary>
        /// 空闲时间最长，在ACD等待队列中，呼叫者可以听到等待的人数、自己等待的时间或一段音乐等
        /// </summary>
        NoAnswer
    }
    */
}
