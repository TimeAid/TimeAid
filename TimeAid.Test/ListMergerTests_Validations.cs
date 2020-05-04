using System;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;

namespace TimeAid.Test
{
    public class ListMergerTests_Validations
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
        public void throws_when_input_is_null()
        {
            Assert.That(() => sut.Merge(null),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items"));
        }

        [Test]
        public void throws_when_merger_returns_nothing()
        {
            // Arrange
            var m1 = MeetingHelper.CreateMeeting(0, 0);
            var m2 = MeetingHelper.CreateMeeting(1, 1);
            var meetings = new[] {m1, m2};

            var emptyMerge = new SplitItem<Meeting>[] { };

            A.CallTo(() => _splitter.Split(m1, m2)).Returns(emptyMerge);

            Assert.That(() => sut.Merge(meetings).ToArray(),
                Throws.Exception.Message.EqualTo("The merger must not provided an empty result"));
        }

        [Test]
        public void throws_because_it_does_not_capture_exceptions()
        {
            var m1 = MeetingHelper.CreateMeeting(0, 0);
            var m2 = MeetingHelper.CreateMeeting(1, 1);
            var meetings = new[] {m1, m2};

            var fakeException = new Exception();

            A.CallTo(() => _splitter.Split(m1, m2)).Throws(fakeException);

            Assert.That(() => sut.Merge(meetings).ToArray(), Throws.Exception.SameAs(fakeException));
        }
        
        [Test]
        public void throws_when_merger_does_not_return_newItem()
        {
            // Arrange
            var m1 = MeetingHelper.CreateMeeting(0, 0);
            var m2 = MeetingHelper.CreateMeeting(1, 1);
            var meetings = new[] {m1, m2};

            var m3 = MeetingHelper.CreateMeeting(2, 2);
            var mergeResults1 = new[] {new SplitItem<Meeting>(m3, m3)};

            A.CallTo(() => _splitter.Split(m1, m2)).Returns(mergeResults1);
            Assert.That(() => sut.Merge(meetings).ToArray(), Throws.Exception.With.Message.EqualTo("The splitter must return the newItem"));
        }           
        
        [Test]
        public void throws_when_merger_invents_an_item()
        {
            // Arrange
            var oldItem = MeetingHelper.CreateMeeting(0, 0);
            var newItem = MeetingHelper.CreateMeeting(1, 1);
            var meetings = new[] {oldItem, newItem};

            var inventedItem = MeetingHelper.CreateMeeting(2, 2);
            var mergeResults1 = new[]
            {
                new SplitItem<Meeting>(newItem, newItem),
                new SplitItem<Meeting>(inventedItem, newItem),
            };

            A.CallTo(() => _splitter.Split(oldItem, newItem)).Returns(mergeResults1);
            Assert.That(() => sut.Merge(meetings).ToArray(), Throws.Exception.With.Message.EqualTo("The splitter should not invent new original items"));
        }   
        
        [Test]
        public void throws_when_merger_returns_newItem_twice()
        {
            // Arrange
            var m1 = MeetingHelper.CreateMeeting(0, 0);
            var m2 = MeetingHelper.CreateMeeting(1, 1);
            var meetings = new[] {m1, m2};

            var m3 = MeetingHelper.CreateMeeting(2, 2);
            var mergeResults1 = new[]
            {
                new SplitItem<Meeting>(m2, m2),
                new SplitItem<Meeting>(m2, m2)
            };

            A.CallTo(() => _splitter.Split(m1, m2)).Returns(mergeResults1);
            Assert.That(() => sut.Merge(meetings).ToArray(), Throws.Exception.With.Message.EqualTo("The splitter must return the newItem only once"));
        }
        
        [Test]
        public void throws_when_merger_returns_oldItem_more_two_times_after()
        {
            // Arrange
            var m1 = MeetingHelper.CreateMeeting(0, 0);
            var m2 = MeetingHelper.CreateMeeting(1, 1);
            var meetings = new[] {m1, m2};

            var m3 = MeetingHelper.CreateMeeting(2, 2);
            var mergeResults1 = new[]
            {
                new SplitItem<Meeting>(m2, m2),
                new SplitItem<Meeting>(m1, m1),
                new SplitItem<Meeting>(m1, m1),
            };

            A.CallTo(() => _splitter.Split(m1, m2)).Returns(mergeResults1);
            Assert.That(() => sut.Merge(meetings).ToArray(), Throws.Exception.With.Message.EqualTo("The splitter can return oldItem only once before and/or once after newItem"));
        }
        
        [Test]
        public void throws_when_merger_returns_oldItem_more_two_times_before()
        {
            // Arrange
            var m1 = MeetingHelper.CreateMeeting(0, 0);
            var m2 = MeetingHelper.CreateMeeting(1, 1);
            var meetings = new[] {m1, m2};

            var m3 = MeetingHelper.CreateMeeting(2, 2);
            var mergeResults1 = new[]
            {
                new SplitItem<Meeting>(m1, m1),
                new SplitItem<Meeting>(m1, m1),
                new SplitItem<Meeting>(m2, m2),
            };

            A.CallTo(() => _splitter.Split(m1, m2)).Returns(mergeResults1);
            Assert.That(() => sut.Merge(meetings).ToArray(), Throws.Exception.With.Message.EqualTo("The splitter can return oldItem only once before and/or once after newItem"));
        }
        
        [Test]
        public void should_not_throw_when_merger_returns_oldItem_once_before_and_once_after()
        {
            // Arrange
            var m1 = MeetingHelper.CreateMeeting(0, 0);
            var m2 = MeetingHelper.CreateMeeting(1, 1);
            var meetings = new[] {m1, m2};

            var m3 = MeetingHelper.CreateMeeting(2, 2);
            var mergeResults1 = new[]
            {
                new SplitItem<Meeting>(m1, m1),
                new SplitItem<Meeting>(m2, m2),
                new SplitItem<Meeting>(m1, m1),
            };

            A.CallTo(() => _splitter.Split(m1, m2)).Returns(mergeResults1);
            Assert.That(() => sut.Merge(meetings).ToArray(), Throws.Nothing);
        }
    }
}