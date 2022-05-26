using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TinyHelper
{
    public class DelayedTask
    {
        public static Task GetTask(int milliseconds)
        {
            //Taken from https://stackoverflow.com/questions/15341962/how-to-put-a-task-to-sleep-or-delay-in-c-sharp-4-0
            var tcs = new TaskCompletionSource<object>();
            new Timer(_ => tcs.SetResult(null)).Change(milliseconds, -1);
            return tcs.Task;
        }

        //public static int DelayedExecution(int milliseconds, Action<Task> continuationAction)
        //{
        //    GetTask(milliseconds).ContinueWith(_ => continuationAction);
        //    return Console.Read();
        //}
    }
}
