using System;
using FluentAssertions;
using iot_scheduler.Entities;
using NUnit.Framework;

namespace iot
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidScheduleNoDays()
        {
            var startTime = DateTime.Now.AddMinutes(-5).TimeOfDay.ToString();
            var duration = 60 * 10;

            var scheduleObj = new Schedule(startTime, duration, null, null);
            scheduleObj.ShouldRun().Should().BeTrue();
        }

        [Test]
        public void ValidScheduleWithDays()
        {
            var startTime = DateTime.Now.AddMinutes(-5).TimeOfDay.ToString();
            var duration = 60 * 10;
            var dayOfWeek = (int) DateTime.Now.DayOfWeek;
            int[] days = {dayOfWeek};

            var scheduleObj = new Schedule(startTime, duration, days, null);
            scheduleObj.ShouldRun().Should().BeTrue();
        }

        [Test]
        public void InvalidScheduleNoDays()
        {
            var startTime = DateTime.Now.AddMinutes(5).TimeOfDay.ToString();
            var duration = 60;

            var scheduleObj = new Schedule(startTime, duration, null, null);
            scheduleObj.ShouldRun().Should().BeFalse();
        }

        [Test]
        public void InvalidValidScheduleWithDays()
        {
            var startTime = DateTime.Now.AddMinutes(-5).TimeOfDay.ToString();
            var duration = 60 * 10;
            var dayOfWeek = (int) DateTime.Now.AddDays(-1).DayOfWeek;
            int[] days = {dayOfWeek};

            var scheduleObj = new Schedule(startTime, duration, days, null);
            scheduleObj.ShouldRun().Should().BeFalse();
        }
    }
}