using System;
using System.Collections.Generic;
using FluentAssertions;
using iot_scheduler.Entities;
using NUnit.Framework;

namespace iot_scheduler.tests
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
            var startTime = DateTime.Now.TimeOfDay.ToString();
            var scheduleObj = new Schedule(startTime, null, new List<Device>());
            scheduleObj.ShouldRun().Should().BeTrue();
        }

        [Test]
        public void ValidScheduleWithDays()
        {
            var startTime = DateTime.Now.TimeOfDay.ToString();
            var dayOfWeek = (int) DateTime.Now.DayOfWeek;
            int[] days = {dayOfWeek};

            var scheduleObj = new Schedule(startTime, days, new List<Device>());
            scheduleObj.ShouldRun().Should().BeTrue();
        }

        [Test]
        public void InvalidScheduleNoDays()
        {
            var startTime = DateTime.Now.AddMinutes(5).TimeOfDay.ToString();

            var scheduleObj = new Schedule(startTime, null, new List<Device>());
            scheduleObj.ShouldRun().Should().BeFalse();
        }

        [Test]
        public void InvalidValidScheduleWithDays()
        {
            var startTime = DateTime.Now.AddMinutes(-5).TimeOfDay.ToString();
            var dayOfWeek = (int) DateTime.Now.AddDays(-1).DayOfWeek;
            int[] days = {dayOfWeek};

            var scheduleObj = new Schedule(startTime, days, new List<Device>());
            scheduleObj.ShouldRun().Should().BeFalse();
        }
    }
}