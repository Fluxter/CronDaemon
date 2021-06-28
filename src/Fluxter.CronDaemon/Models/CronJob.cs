// ------------------------------------------------------------------------------------------------
//  <copyright file="CronJob.cs"
//             company="fluxter.net">
//       Copyright (c) fluxter.net. All rights reserved.
//  </copyright>
//  <author>Marcel Kallen</author>
//  <created>08.08.2018 - 21:13</created>
// ------------------------------------------------------------------------------------------------

namespace Fluxter.CronDaemon.Models
{
    using System;
    using System.Threading;
    using Abstraction;

    public class CronJob : ICronJob
    {
        private ICronSchedule CronSchedule { get; } = new CronSchedule();

        private ThreadStart ThreadStartInfo;

        private readonly object _lock = new object();

        private Thread Thread { get; set; }

        public CronJob(string schedule, ThreadStart thread_start)
        {
            this.CronSchedule = new CronSchedule(schedule);
            this.ThreadStartInfo = thread_start;
            this.Thread = new Thread(thread_start);
        }

        public void Execute(DateTime date_time)
        {
            lock (this._lock)
            {
                if (!this.CronSchedule.IsTime(date_time))
                {
                    return;
                }

                if (this.Thread.ThreadState == ThreadState.Running)
                {
                    return;
                }

                this.Thread = new Thread(this.ThreadStartInfo);
                this.Thread.IsBackground = true;
                this.Thread.Start();
            }
        }

        public void Abort()
        {
            if (!this.Thread.IsAlive)
            {
                return;
            }
            
            this.Thread.Interrupt();
        }
    }
}