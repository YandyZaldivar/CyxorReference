/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Linq;
using System.Collections.Generic;

namespace Alimatic.Pt.Models
{
    public class WorkerActivity
    {
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public string Observations { get; set; }

        public List<WorkerActivityCompletion> Completions { get; set; } = new List<WorkerActivityCompletion>();

        public bool IsScheduledOn(int? day = null, int? month = null, int? year = null)
        {
            if (!Activity.Enabled)
                return false;

            if (Activity.EndingDate == null)
                return day == null;

            var date = DateTime.Now;
            date = new DateTime(year ?? date.Year, month ?? date.Month, day ?? date.Day);

            if (day == null && month == null && year == null)
                return true;

            if (day == null && month == null)
                return Activity.EndingDate?.Year == date.Year;

            if (day == null)
                return Activity.EndingDate?.Year == date.Year && Activity.EndingDate?.Month == date.Month;

            return Activity.EndingDate?.Date == date;
        }

        //public bool IsActiveOn(int? day = null, int? month = null, int? year = null)
        //{
        //    if (IsScheduledOn(day, month, year))
        //        return true;


        //}

        public bool IsCompletedOn(int? day = null, int? month = null, int? year = null)
        {
            if (!Activity.Enabled)
                return false;

            var date = DateTime.Now;
            date = new DateTime(year ?? date.Year, month ?? date.Month, day ?? date.Day);

            if (day == null && month == null && year == null)
                return Completions.Any();

            if (day == null && month == null)
                return Completions.SingleOrDefault(p => p.Completion.Date.Year == date.Year) != null;

            if (day == null)
                return Completions.SingleOrDefault(p => p.Completion.Date.Year == date.Year && p.Completion.Date.Month == date.Month) != null;
            else if (Activity.EndingDate == null)
                return false;

            return Completions.SingleOrDefault(p => p.Completion.Date.Date == date) != null;
        }

        public bool IsIncompletedOn(int? day = null, int? month = null, int? year = null)
        {
            if (!Activity.Enabled)
                return false;

            if (IsCompletedOn(day, month, year))
                return false;

            if (Activity.EndingDate == null)
                return true;

            var date = DateTime.Now;
            date = new DateTime(year ?? date.Year, month ?? date.Month, day ?? date.Day);

            if (day == null && month == null && year == null)
                return !Completions.Any();

            if (day == null && month == null)
            {
                if (Activity.EndingDate == null)
                    return !Completions.Any(p => p.Completion.Date.Year == date.Year);

                return Activity.EndingDate?.Year < date.Year;
            }

            if (day == null)
                if (Activity.EndingDate != null)
                    return Activity.EndingDate?.Year < date.Year || Activity.EndingDate?.Month < date.Month;

            if (Activity.EndingDate == null)
                return !Completions.Any(p => p.Completion.Date.Year == date.Year && p.Completion.Date.Month == date.Month);

            return Activity.EndingDate < date;
        }

        /*
        public bool IsCompletedOutOfDate(int? day = null, int? month = null, int? year = null)
        {
            if (!Activity.Enabled)
                return false;

            var date = DateTime.Now;
            date = new DateTime(year ?? date.Year, month ?? date.Month, day ?? date.Day);

            if (day == null && month == null && year == null)
                return Completions.Any() ? Completions.Last().Completion.Date > Activity.EndingDate : false;

            if (day == null && month == null)
                return Completions.SingleOrDefault(p => p.Completion.Date.Year == date.Year) != null;

            if (day == null)
                return Completions.SingleOrDefault(p => p.Completion.Date.Year == date.Year && p.Completion.Date.Month == date.Month) != null;
            else if (Activity.EndingDate == null)
                return false;

            return Completions.SingleOrDefault(p => p.Completion.Date.Date == date) != null;
        }
        */

        /*
        public bool IsCompletedOn2(int? day = null, int? month = null, int? year = null)
        {
            var date = DateTime.Now;

            date = new DateTime(year ?? date.Year, month ?? date.Month, date.Day);

            if (year == null && month == null)
                return Completions.Any();
            else if (year == null)
                return Completions.SingleOrDefault(p => p.Completion.Date.Year == date.Year && p.Completion.Date.Month == month) != null;
            else
                return Completions.SingleOrDefault(p => p.Completion.Date.Year == year && p.Completion.Date.Month == month) != null;
        }

        //public bool IsIncomplete(int? month = null, int? year = null) => !IsCompleted(month, year);

        public bool IsCompletedOutOfDate(int? month = null, int? year = null)
        {
            if (Activity.EndingDate == null)
                return false;

            if (!IsCompleted(month, year))
                return false;

            var date = DateTime.Now;

            month = month ?? date.Month;
            year = year ?? date.Year;

            var completion = Completions.Single(p => p.Completion.Date.Year == year && p.Completion.Date.Month == month).Completion;

            return completion.Date > Activity.EndingDate;
        }
        */
    }
}
/* { Alimatic.Server } */
