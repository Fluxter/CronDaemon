// ------------------------------------------------------------------------------------------------
//  <copyright file="ICronJob.cs"
//             company="fluxter.net">
//       Copyright (c) fluxter.net. All rights reserved.
//  </copyright>
//  <author>Marcel Kallen</author>
//  <created>09.08.2018 - 19:16</created>
// ------------------------------------------------------------------------------------------------

namespace Fluxter.CronDaemon.Abstraction
{
    using System;

    public interface ICronJob
    {
        void execute(DateTime date_time);
        void abort();
    }
}