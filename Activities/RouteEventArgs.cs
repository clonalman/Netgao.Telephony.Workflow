using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace Call.Core.Activities
{
    [Serializable]
    public sealed class RouteEventArgs : ChannelEventArgs
    {

        public RouteEventArgs(IChannelInstance instance, string calleeId, int routing)
            : base(instance)
        {
            Channel.SetVariable("CalleeId", calleeId);
            Channel.SetVariable("Routing", routing);
        }

        public string CalleeId
        {
            get { return Channel.GetVariable<string>("CalleeId"); }
        }

        public int Routing
        {
            get { return Channel.GetVariable<int>("Routing"); }
        }

        public override string ToString()
        {
            return String.Format("{0}->Route(\"{1}\", {2})", Channel.LinePad, CalleeId, Routing);
        }
    }

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

}
