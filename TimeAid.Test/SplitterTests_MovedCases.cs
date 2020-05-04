using System;
using System.Linq;
using NUnit.Framework;

namespace TimeAid.Test
{
    public class SplitterTests_MovedCases
    {
        private Splitter<Meeting> _splitter;

        [SetUp]
        public void Setup()
        {
            _splitter = new Splitter<Meeting>(
                MeetingHelper.StartSelector,
                MeetingHelper.EndSelector,
                MeetingHelper.CreateClone);
        }

        [Test]
        public void move_end_of_oldItem_when_newItem_starts_in_oldItem()
        {
            var oldItem = MeetingHelper.CreateMeeting(0, 6);
            var newItem = MeetingHelper.CreateMeeting(3, 9);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Value.Start, Is.EqualTo(0));
            Assert.That(result[0].Value.End, Is.EqualTo(3));
            Assert.That(result[0].Value.OriginalMeeting, Is.EqualTo(oldItem));
            Assert.That(result[1].Value, Is.EqualTo(newItem));
        }
        
        [Test]
        public void move_start_of_oldItem_when_newItem_ends_in_oldItem()
        {
            var oldItem = MeetingHelper.CreateMeeting(3, 9);
            var newItem = MeetingHelper.CreateMeeting(0, 6);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Value, Is.EqualTo(newItem));
            Assert.That(result[1].Value.Start, Is.EqualTo(6));
            Assert.That(result[1].Value.End, Is.EqualTo(9));
            Assert.That(result[1].Value.OriginalMeeting, Is.EqualTo(oldItem));
        }
    }
}