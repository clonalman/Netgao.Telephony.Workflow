using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow;
using System.Workflow.Activities;

namespace Netgao.Telephony.Workflow
{
    [ExternalDataExchange]
    //[CorrelationParameter("uniqueId")]
    public interface ITerminalService 
	{
        /// <summary>
        /// 启动事件
        /// </summary>
        //[CorrelationInitializer]
        //[CorrelationAlias("uniqueId", "e.InstanceId")]
        event EventHandler<TerminalEventArgs> Started;
        /// <summary>
        /// 完成事件
        /// </summary>
        //[CorrelationAlias("uniqueId", "e.InstanceId")]
        event EventHandler<TerminalEventArgs> Completed;
        /// <summary>
        /// 终止事件
        /// </summary>
        //[CorrelationAlias("uniqueId", "e.InstanceId")]
        event EventHandler<TerminalEventArgs> Terminated;
        /// <summary>
        /// 超时事件
        /// </summary>
        //[CorrelationAlias("uniqueId", "e.InstanceId")]
        event EventHandler<TerminalEventArgs> Timeout;
        /// <summary>
        /// 通道
        /// </summary>
        TerminalCollection Terminals { get; }
        /// <summary>
        /// 摘机
        /// </summary>
        /// <param name="channel">工作流实例ID</param>
        //[CorrelationInitializer]
        void Pickup(Guid uniqueId);
        /// <summary>
        /// 挂机
        /// </summary>
        /// <param name="channel">工作流实例ID</param>
        //[CorrelationInitializer]
        void Hangup(Guid uniqueId);        
        /// <summary>
        /// 呼叫
        /// </summary>
        /// <param name="channel">工作流实例ID</param>
        /// <param name="callType">呼叫方式</param>
        //[CorrelationInitializer]
        void BlindTransfer(Guid uniqueId, string calleeId);
        /// <summary>
        /// 发送信号音
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="callerId"></param>
        //[CorrelationInitializer]
        void SendTone(Guid uniqueId, int toneType, int delay);
        /// <summary>
        /// 清除缓冲
        /// </summary>
        //[CorrelationInitializer]
        void Play(Guid uniqueId, string fileName, bool stopOnDTMF);
        /// <summary>
        /// 通话
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="volume">音量</param>
        /// <param name="recording">是否录音</param>
        void Talk(Guid uniqueId, int volume, bool recording);
        /// <summary>
        /// 排队
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="priority"></param>
        void Queue(Guid uniqueId, int priority);
        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="channel">工作流实例ID</param>
        void Clear(Guid uniqueId);
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="logName"></param>
        /// <param name="channel"></param>
        //[CorrelationInitializer]
        void Log(Guid uniqueId, string logName);
	}
}
