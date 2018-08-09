// ------------------------------------------------------------------------------------------------
//  <copyright file="ICronSchedule.cs"
//             company="fluxter.net">
//       Copyright (c) fluxter.net. All rights reserved.
//  </copyright>
//  <author>Marcel Kallen</author>
//  <created>09.08.2018 - 18:42</created>
// ------------------------------------------------------------------------------------------------

namespace Fluxter.CronDaemon.Abstraction
{
    using System;

    public interface ICronSchedule
    {
        bool IsValid(string expression);

        bool IsTime(DateTime date);
    }
}