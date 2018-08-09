// ------------------------------------------------------------------------------------------------
//  <copyright file="CronJob.cs"
//             company="fluxter.net">
//       Copyright (c) fluxter.net. All rights reserved.
//  </copyright>
//  <author>Marcel Kallen</author>
//  <created>08.08.2018 - 21:13</created>
// ------------------------------------------------------------------------------------------------

namespace Fluxter.CronDaemon
{
    using System;
    using System.Threading;

    public interface ICronJob
    {
        void execute(DateTime date_time);
        void abort();
    }

    public class CronJob : ICronJob
    {
        private readonly ICronSchedule _cron_schedule = new CronSchedule();

        private readonly ThreadStart _thread_start;

        private readonly object _lock = new object();

        private Thread _thread;

        public CronJob(string schedule, ThreadStart thread_start)
        {
            this._cron_schedule = new CronSchedule(schedule);
            this._thread_start = thread_start;
            this._thread = new Thread(thread_start);
        }

        public void execute(DateTime date_time)
        {
            lock (this._lock)
            {
                if (!this._cron_schedule.isTime(date_time))
                {
                    return;
                }

                if (this._thread.ThreadState == ThreadState.Running)
                {
                    return;
                }

                this._thread = new Thread(this._thread_start);
                this._thread.Start();
            }
        }

        public void abort()
        {
            this._thread.Abort();
        }
    }
}