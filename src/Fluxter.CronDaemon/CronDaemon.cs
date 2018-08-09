// ------------------------------------------------------------------------------------------------
//  <copyright file="CronDaemon.cs"
//             company="fluxter.net">
//       Copyright (c) fluxter.net. All rights reserved.
//  </copyright>
//  <author>Marcel Kallen</author>
//  <created>08.08.2018 - 21:13</created>
// ------------------------------------------------------------------------------------------------

namespace Fluxter.CronDaemon
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Timers;
    using Timer = System.Timers.Timer;

    public interface ICronDaemon
    {
        void AddJob(string schedule, ThreadStart action);

        void Start();

        void Stop();
    }

    public class CronDaemon : ICronDaemon
    {
        private readonly List<ICronJob> cron_jobs = new List<ICronJob>();
        private readonly Timer timer = new Timer(30000);
        private DateTime _last = DateTime.Now;

        public CronDaemon()
        {
            this.timer.AutoReset = true;
            this.timer.Elapsed += this.timer_elapsed;
        }

        public void AddJob(string schedule, ThreadStart action)
        {
            var cj = new CronJob(schedule, action);
            this.cron_jobs.Add(cj);
        }

        public void Start()
        {
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();

            foreach (CronJob job in this.cron_jobs)
            {
                job.abort();
            }
        }

        private void timer_elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute == this._last.Minute)
            {
                return;
            }

            this._last = DateTime.Now;
            foreach (ICronJob job in this.cron_jobs)
            {
                job.execute(DateTime.Now);
            }
        }
    }
}