// ------------------------------------------------------------------------------------------------
//  <copyright file="ICronSchedule.cs"
//             company="fluxter.net">
//       Copyright (c) fluxter.net. All rights reserved.
//  </copyright>
//  <author>Marcel Kallen</author>
//  <created>08.08.2018 - 21:13</created>
// ------------------------------------------------------------------------------------------------

namespace Fluxter.CronManager
{
    using System;

    public interface ICronSchedule
    {
        bool isValid(string expression);

        bool isTime(DateTime date_time);
    }
}