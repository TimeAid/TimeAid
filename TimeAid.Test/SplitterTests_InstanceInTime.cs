using System;
using System.Linq;
using NUnit.Framework;

namespace TimeAid.Test
{
    public class SplitterTests_InstanceInTime
    {
        private Splitter<Meeting, int> _splitter;

        [SetUp]
        public void Setup()
        {
            _splitter = new Splitter<Meeting,int>(
                MeetingHelper.StartSelector,
                MeetingHelper.EndSelector,
                MeetingHelper.CreateClone);
        }

        [Test]
        public void return_both_if_newItem_is_after_oldItem()
        {
            var oldItem = MeetingHelper.CreateMeeting(0, 0);
            var newItem = MeetingHelper.CreateMeeting(1, 1);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Value, Is.EqualTo(oldItem));
            Assert.That(result[1].Value, Is.EqualTo(newItem));
        }

        [Test]
        public void return_both_if_newItem_is_before_oldItem()
        {
            var oldItem = MeetingHelper.CreateMeeting(1, 1);
            var newItem = MeetingHelper.CreateMeeting(0, 0);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Value, Is.EqualTo(newItem));
            Assert.That(result[1].Value, Is.EqualTo(oldItem));
        }

        [Test]
        public void return_newItem_if_newItem_has_same_start_stop_as_oldItem()
        {
            var oldItem = MeetingHelper.CreateMeeting(0, 0);
            var newItem = MeetingHelper.CreateMeeting(0, 0);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(newItem));
        }
    }
}