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
    using Abstraction;
    using Models;
    using Timer = System.Timers.Timer;

    public interface ICronDaemon
    {
        void AddJob(string schedule, ThreadStart action);

        void Start();

        void Stop();
    }

    public class CronDaemon : ICronDaemon
    {
        private List<ICronJob> CronJobs { get; } = new List<ICronJob>();

        private Timer Timer { get; } = new Timer(30000);

        private DateTime LastRun { get; set; } = DateTime.Now;

        public CronDaemon()
        {
            this.Timer.AutoReset = true;
            this.Timer.Elapsed += this.timer_elapsed;
        }

        public void AddJob(string schedule, ThreadStart action)
        {
            var cj = new CronJob(schedule, action);
            this.CronJobs.Add(cj);
        }

        public void Start()
        {
            this.Timer.Start();
        }

        public void Stop()
        {
            this.Timer.Stop();

            foreach (CronJob job in this.CronJobs)
            {
                job.Abort();
            }
        }

        private void timer_elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute == this.LastRun.Minute)
            {
                return;
            }

            this.LastRun = DateTime.Now;
            foreach (var job in this.CronJobs)
            {
                job.Execute(DateTime.Now);
            }
        }
    }
}