using System;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;

namespace TimeAid.Test
{
    public class ListMergerTests_Simple
    {
        private ListMerger<Meeting> sut;
        private ISplitter<Meeting> _splitter;

        [SetUp]
        public void SetUp()
        {
            _splitter = A.Fake<ISplitter<Meeting>>(x => x.Strict().Named("Merger"));
            sut = new ListMerger<Meeting>(_splitter);
        }

        [Test]
        public void returns_empty_when_no_items()
        {
            var meetings = new Meeting[0];
            var result = sut.Merge(meetings).ToArray();
            Assert.That(result, Has.Length.EqualTo(0));
        }

        [Test]
        public void returns_input_when_single_item()
        {
            var m1 = MeetingHelper.CreateMeeting(0, 0);
            var meetings = new[] {m1};
            var result = sut.Merge(meetings).ToArray();
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(m1));
        }

       
    }
}