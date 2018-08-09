// ------------------------------------------------------------------------------------------------
//  <copyright file="CronSchedule.cs"
//             company="fluxter.net">
//       Copyright (c) fluxter.net. All rights reserved.
//  </copyright>
//  <author>Marcel Kallen</author>
//  <created>09.08.2018 - 18:42</created>
// ------------------------------------------------------------------------------------------------

namespace Fluxter.CronDaemon.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Abstraction;

    public class CronSchedule : ICronSchedule
    {
        private static readonly Regex divided_regex = new Regex(@"(\*/\d+)");
        private static readonly Regex list_regex = new Regex(@"(((\d+,)*\d+)+)");
        private static readonly Regex range_regex = new Regex(@"(\d+\-\d+)\/?(\d+)?");

        private static readonly Regex wild_regex = new Regex(@"(\*)");

        private static readonly Regex validation_regex = new Regex(divided_regex + "|" + range_regex + "|" + wild_regex + "|" + list_regex);


        public List<int> days_of_month;

        public List<int> days_of_week;

        public List<int> hours;

        public List<int> minutes;

        public List<int> months;

        private readonly string _expression;

        public CronSchedule()
        {
        }

        public CronSchedule(string expressions)
        {
            this._expression = expressions;
            this.generate();
        }

        public bool IsValid(string expression)
        {
            var matches = validation_regex.Matches(expression);
            return matches.Count > 0; 
        }

        public bool IsTime(DateTime date)
        {
            return this.minutes.Contains(date.Minute) && this.hours.Contains(date.Hour) &&
                   this.days_of_month.Contains(date.Day) && this.months.Contains(date.Month) &&
                   this.days_of_week.Contains((int)date.DayOfWeek);
        }

        private List<int> divided_array(string configuration, int start, int max)
        {
            if (!divided_regex.IsMatch(configuration))
            {
                return new List<int>();
            }

            List<int> ret = new List<int>();
            string[] split = configuration.Split("/".ToCharArray());
            int divisor = int.Parse(split[1]);

            for (int i = start; i < max; ++i)
            {
                if (i % divisor == 0)
                {
                    ret.Add(i);
                }
            }

            return ret;
        }

        private void generate()
        {
            if (!this.IsValid())
            {
                return;
            }

            var matches = validation_regex.Matches(this._expression);

            this.generate_minutes(matches[0].ToString());
            this.generate_hours(matches.Count > 1 ? matches[1].ToString() : "*");
            this.generate_days_of_month(matches.Count > 2 ? matches[2].ToString() : "*");
            this.generate_months(matches.Count > 3 ? matches[3].ToString() : "*");
            this.generate_days_of_weeks(matches.Count > 4 ? matches[4].ToString() : "*");
        }

        private void generate_days_of_month(string match)
        {
            this.days_of_month = this.generate_values(match, 1, 32);
        }

        private void generate_days_of_weeks(string match)
        {
            this.days_of_week = this.generate_values(match, 0, 7);
        }

        private void generate_hours(string match)
        {
            this.hours = this.generate_values(match, 0, 24);
        }

        private void generate_minutes(string match)
        {
            this.minutes = this.generate_values(match, 0, 60);
        }

        private void generate_months(string match)
        {
            this.months = this.generate_values(match, 1, 13);
        }

        private List<int> generate_values(string configuration, int start, int max)
        {
            if (divided_regex.IsMatch(configuration))
            {
                return this.divided_array(configuration, start, max);
            }

            if (range_regex.IsMatch(configuration))
            {
                return this.range_array(configuration);
            }

            if (wild_regex.IsMatch(configuration))
            {
                return this.wild_array(configuration, start, max);
            }

            if (list_regex.IsMatch(configuration))
            {
                return this.list_array(configuration);
            }

            return new List<int>();
        }

        private bool IsValid()
        {
            return this.IsValid(this._expression);
        }

        private List<int> list_array(string configuration)
        {
            if (!list_regex.IsMatch(configuration))
            {
                return new List<int>();
            }

            List<int> ret = new List<int>();

            string[] split = configuration.Split(",".ToCharArray());

            foreach (string s in split)
            {
                ret.Add(int.Parse(s));
            }

            return ret;
        }

        private List<int> range_array(string configuration)
        {
            if (!range_regex.IsMatch(configuration))
            {
                return new List<int>();
            }

            List<int> ret = new List<int>();
            string[] split = configuration.Split("-".ToCharArray());
            int start = int.Parse(split[0]);
            int end = 0;
            if (split[1].Contains("/"))
            {
                split = split[1].Split("/".ToCharArray());
                end = int.Parse(split[0]);
                int divisor = int.Parse(split[1]);

                for (var i = start; i < end; ++i)
                {
                    if (i % divisor == 0)
                    {
                        ret.Add(i);
                    }
                }

                return ret;
            }

            end = int.Parse(split[1]);

            for (int i = start; i <= end; ++i)
            {
                ret.Add(i);
            }

            return ret;
        }

        private List<int> wild_array(string configuration, int start, int max)
        {
            if (!wild_regex.IsMatch(configuration))
            {
                return new List<int>();
            }

            List<int> ret = new List<int>();

            for (int i = start; i < max; ++i)
            {
                ret.Add(i);
            }

            return ret;
        }
    }
}