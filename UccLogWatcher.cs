using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace Netgao.Telephony.Workflow
{
    public sealed class UccLogWatcher : StreamWriter
    {
        public static readonly TextWriter Watcher = Synchronized(new UccLogWatcher());

        static UccLogWatcher()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => Watcher.Close();
            AppDomain.CurrentDomain.DomainUnload += (sender, e) => Watcher.Close();
        }

        public UccLogWatcher()
            : base(GetLogFileName(), true)
        {
            
        }

        public static void Error(string message)
        {
            Watcher.WriteLine("{0} > {1}", DateTime.Now, message);
            Watcher.Flush();
        }

        public static void Error(string message, Exception ex)
        {
            Watcher.WriteLine("{0} > {1}", DateTime.Now, ex.Message);
            Watcher.Flush();
        }

        public static void Trace(string format, params object[] args)
        {
            if (System.Environment.UserInteractive)
            {
                Watcher.WriteLine("{0} > {1}", DateTime.Now, String.Format(format, args));
                Watcher.Flush();
            }
            else
            {
                Watcher.WriteLine("{0} > {1}", DateTime.Now, String.Format(format, args));
                Watcher.Flush();
            }
        }

        private static string GetLogFileName()
        {
            return String.Format(@"{0}\logs\server-log-{1}.log", Path.GetDirectoryName(Application.ExecutablePath), DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
