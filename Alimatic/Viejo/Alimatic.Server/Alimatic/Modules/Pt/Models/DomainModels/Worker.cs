/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Pt.Models
{
    using Cyxor.Models;
    using Cyxor.Serialization;

    public class Worker// : ISerializable
    {
        [Key]
        public int Id { get; set; }

        //[CyxorIgnore]
        //[NotMapped]
        //public Account Account { get; set; }

        public int ChargeId { get; set; }

        [ForeignKey(nameof(ChargeId))]
        public Charge Charge { get; set; }

        public List<WorkerActivity> Activities { get; set; }

        //public void Serialize(Serializer serializer)
        //{
        //    serializer.Serialize(AccountId);
        //    serializer.Serialize(Account?.Profile?.FullName);
        //    serializer.Serialize(Charge);
        //    serializer.Serialize(Activities);
        //}

        //public void Deserialize(Serializer serializer)
        //{
        //    AccountId = serializer.DeserializeInt32();
        //    serializer.Serialize(Account?.Profile?.FullName);
        //}

        /*
        public IEnumerable<Activity> GetActivitiesOn(int? day = null, int? month = null, int? year = null, bool excludeCompletes = false, bool excludeRepetitives = false, bool excludeOutOfDate = false, bool excludeInactives = false, bool filterRepetitives = false)
        {
            var date = DateTime.Now;
            date = new DateTime(year ?? date.Year, month ?? date.Month, day ?? date.Day);

            var activities = default(IEnumerable<Activity>);

            var exclusiveActivities = from workerActivity in Activities
                                      where workerActivity.Activity.EndingDate != null
                                      select workerActivity.Activity;

            if (year == null && month == null && day == null)
            {
                //activities = from workerActivity in Activities select workerActivity.Activity;
            }
            else if (day == null && month == null)
            {
                if (excludeOutOfDate)
                    exclusiveActivities = from activity in exclusiveActivities
                                          where activity.EndingDate?.Year == date.Year
                                          select activity;

                exclusiveActivities = from workerActivity in Activities
                                      where workerActivity.Activity.EndingDate != null
                                      where workerActivity.Activity.EndingDate?.Year == date.Year || workerActivity.IsCompletedOutOfDate(year: date.Year) ||
                                      !workerActivity.IsCompletedOn(year: date.Year) && workerActivity.Activity.EndingDate?.Year < date.Year
                                      select workerActivity.Activity;
            }
            else if (day == null)
            {
                exclusiveActivities = from activity in exclusiveActivities
                                      where activity.EndingDate?.Year == date.Year && activity.EndingDate?.Month == date.Month
                                      select activity;
            }
            else
            {
                exclusiveActivities = from activity in exclusiveActivities
                                      where activity.EndingDate?.Year == date.Year && activity.EndingDate?.Month == date.Month && activity.EndingDate?.Day == date.Day
                                      select activity;
            }

            activities = exclusiveActivities;

            if (!excludeOutOfDate)
            {

            }

            if (!excludeRepetitives)
                activities.Concat(from workerActivity in Activities
                                  where workerActivity.Activity.EndingDate == null
                                  select workerActivity.Activity);

            if (excludeCompletes)
            {
                activities = from activity in activities
                             join workerActivity in Activities
                             on activity.Id equals workerActivity.ActivityId
                             where workerActivity.IsIncompletedOn(date.Month, date.Year)
                             select activity;
            }

            if (excludeInactives)
                activities = activities.Where(p => p.Enabled);

            return activities;
        }
        */

        public IEnumerable<Activity> GetActivities(int? day = null, int? month = null, int? year = null, bool excludeCompletes = false, bool excludeRepetitives = false, bool excludeOutOfDate = false, bool excludeInactives = false, bool filterRepetitives = false)
        {
            var date = DateTime.Now;
            date = new DateTime(year ?? date.Year, month ?? date.Month, day ?? date.Day);

            var activities = from workerActivity in Activities
                             where workerActivity.IsScheduledOn(day, month, year) ||
                                   workerActivity.IsCompletedOn(day, month, year) ||
                                   workerActivity.IsIncompletedOn(day, month, year)
                             select workerActivity.Activity;

            //activities = exclusiveActivities;

            if (!excludeOutOfDate)
            {

            }

            if (!excludeRepetitives)
                activities.Concat(from workerActivity in Activities
                                  where workerActivity.Activity.EndingDate == null
                                  select workerActivity.Activity);

            if (excludeCompletes)
            {
                activities = from activity in activities
                             join workerActivity in Activities
                             on activity.Id equals workerActivity.ActivityId
                             where workerActivity.IsIncompletedOn(date.Month, date.Year)
                             select activity;
            }

            if (excludeInactives)
                activities = activities.Where(p => p.Enabled);

            return activities;
        }

        public IEnumerable<Activity> GetActivities(int? day, int? month, int? year)
        {
            var date = DateTime.Now;

            var exclusiveActivities = from workerActivity in Activities
                                      where workerActivity.Activity.EndingDate != null
                                      select workerActivity.Activity;

            if (!(year == null && month == null && day == null))
            {
                if (day == null && month == null)
                    exclusiveActivities = from activity in exclusiveActivities
                                          where activity.EndingDate?.Year == year
                                          select activity;
                else if (day == null && year == null)
                    exclusiveActivities = from activity in exclusiveActivities
                                          where activity.EndingDate?.Year == date.Year && activity.EndingDate?.Month == month
                                          select activity;
                else if (year == null && month == null)
                    exclusiveActivities = from activity in exclusiveActivities
                                          where activity.EndingDate?.Year == date.Year && activity.EndingDate?.Month == date.Month && activity.EndingDate?.Day == day
                                          select activity;
                else if (day == null)
                    exclusiveActivities = from activity in exclusiveActivities
                                          where activity.EndingDate?.Year == year && activity.EndingDate?.Month == month
                                          select activity;
                else if (month == null)
                    exclusiveActivities = from activity in exclusiveActivities
                                          where activity.EndingDate?.Year == year && activity.EndingDate?.Month == date.Month && activity.EndingDate?.Day == day
                                          select activity;
                else if (year == null)
                    exclusiveActivities = from activity in exclusiveActivities
                                          where activity.EndingDate?.Year == date.Year && activity.EndingDate?.Month == month && activity.EndingDate?.Day == day
                                          select activity;
                else
                    exclusiveActivities = from activity in exclusiveActivities
                                          where activity.EndingDate?.Year == year && activity.EndingDate?.Month == month && activity.EndingDate?.Day == day
                                          select activity;
            }

            var activities = default(IEnumerable<Activity>);

            return activities;
        }
    }
}
/* { Alimatic.Server } */
