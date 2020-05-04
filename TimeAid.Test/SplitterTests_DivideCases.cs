using System;
using System.Linq;
using NUnit.Framework;

namespace TimeAid.Test
{
    public class SplitterTests_DivideCases
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
        public void return_three_items_when_newItem_splits_oldItem()
        {
            var oldItem = MeetingHelper.CreateMeeting(0, 9);
            var newItem = MeetingHelper.CreateMeeting(3, 6);
            var result = _splitter.Split(oldItem, newItem).ToArray();
            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0].Value.Start, Is.EqualTo(0));
            Assert.That(result[0].Value.End, Is.EqualTo(3));
            Assert.That(result[0].Value.OriginalMeeting, Is.EqualTo(oldItem));
            Assert.That(result[1].Value, Is.EqualTo(newItem));
            Assert.That(result[2].Value.Start, Is.EqualTo(6));
            Assert.That(result[2].Value.End, Is.EqualTo(9));
            Assert.That(result[2].Value.OriginalMeeting, Is.EqualTo(oldItem));
        }
    }
}