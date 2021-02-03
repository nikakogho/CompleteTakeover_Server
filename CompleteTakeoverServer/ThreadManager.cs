using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeoverServer
{
    class ThreadManager
    {
        static readonly List<Action> executeOnMainThread = new List<Action>();
        static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        static bool actionToExecuteOnMainThread = false;

        public static void ExecuteOnMainThread(Action action)
        {
            if (action == null)
            {
                Console.WriteLine("No action found!");
                return;
            }

            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(action);
                actionToExecuteOnMainThread = true;
            }
        }

        public static void UpdateMain()
        {
            if (!actionToExecuteOnMainThread) return;

            executeCopiedOnMainThread.Clear();

            lock (executeOnMainThread)
            {
                executeCopiedOnMainThread.AddRange(executeOnMainThread);
                executeOnMainThread.Clear();
                actionToExecuteOnMainThread = false;
            }

            for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
            {
                executeCopiedOnMainThread[i]();
            }
        }
    }
}
