using System;
using System.Linq;
using NUnit.Framework;

namespace TimeAid.Test
{
    public class SplitterTests_SimpleCases
    {
        private Splitter<Meeting, int> _splitter;

        [SetUp]
        public void Setup()
        {
            _splitter = new Splitter<Meeting, int>(
                MeetingHelper.StartSelector,
                MeetingHelper.EndSelector,
                MeetingHelper.CreateClone);
        }
        
        [Test]
        public void return_both_if_newItem_starts_after_oldItem()
        {
            var oldItem = MeetingHelper.CreateMeeting(0, 3);
            var newItem = MeetingHelper.CreateMeeting(6, 9);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Value, Is.EqualTo(oldItem));
            Assert.That(result[1].Value, Is.EqualTo(newItem));
        }

        [Test]
        public void return_both_if_newItem_stops_before_oldItem()
        {
            var oldItem = MeetingHelper.CreateMeeting(6, 9);
            var newItem = MeetingHelper.CreateMeeting(0, 3);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Value, Is.EqualTo(newItem));
            Assert.That(result[1].Value, Is.EqualTo(oldItem));
        }

        [Test]
        public void return_both_if_newItem_starts_when_oldItem_ends()
        {
            var oldItem = MeetingHelper.CreateMeeting(0, 3);
            var newItem = MeetingHelper.CreateMeeting(3, 6);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Value, Is.EqualTo(oldItem));
            Assert.That(result[1].Value, Is.EqualTo(newItem));
        }

        [Test]
        public void return_newItem_if_newItem_starts_before_oldItem_starts_and_ends_after_oldItem_ends()
        {
            var oldItem = MeetingHelper.CreateMeeting(3, 6);
            var newItem = MeetingHelper.CreateMeeting(0, 9);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(newItem));
        }

        [Test]
        public void return_newItem_if_newItem_starts_and_stops_at_the_same_time_as_oldItem()
        {
            var oldItem = MeetingHelper.CreateMeeting(3, 6);
            var newItem = MeetingHelper.CreateMeeting(3, 6);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(newItem));
        }

        [Test]
        public void return_newItem_if_newItem_is_longer_and_starts_when_oldItem_starts()
        {
            var oldItem = MeetingHelper.CreateMeeting(3, 6);
            var newItem = MeetingHelper.CreateMeeting(3, 9);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(newItem));
        }

        [Test]
        public void return_newItem_if_newItem_is_longer_and_stops_when_oldItem_stops()
        {
            var oldItem = MeetingHelper.CreateMeeting(3, 6);
            var newItem = MeetingHelper.CreateMeeting(0, 6);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(newItem));
        }
    }
}