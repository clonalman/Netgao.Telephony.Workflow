using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using System.Transactions;
using System.Threading; 

namespace Netgao.Telephony.Workflow
{
    public sealed class UccPendingWorkItem : IPendingWork
    {
        private Mutex mutex;
        private bool result;

        public UccPendingWorkItem(int ch)
        {
            mutex = OpenMutex(ch);
        }

        private static Mutex OpenMutex(int ch)
        {
            Mutex mutex = null;
            string mutexName = "Mutex-" + ch.ToString();
            try
            {
                mutex = Mutex.OpenExisting(mutexName);
            }
            catch
            {
                mutex = new Mutex(true, mutexName);
            }
            return mutex;
        }


        public bool Success
        {
            get { return result; }
            set { result = value; }
        }

        public bool WaitForCommit()
        {
            return mutex.WaitOne();
        }

        public void Commit(Transaction transaction, ICollection items)
        {
           
        }

        /**/
        /// <summary> 
        /// SEts the outcome and signals completion. 
        /// </summary> 
        /// <param name="succeeded"></param> 
        /// <param name="items"></param> 
        public void Complete(bool succeeded, ICollection items)
        {
            Success = succeeded;
            mutex.ReleaseMutex();
            mutex.Close();
        }

        public bool MustCommit(ICollection items)
        {
            //always return true to avoid hang scenarios if not  
            //using persistence. 
            return true;
        }

    } 

}
