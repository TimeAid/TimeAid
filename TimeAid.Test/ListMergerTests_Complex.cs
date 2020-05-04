using System;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;

namespace TimeAid.Test
{
    public class ListMergerTests_Complex
    {
        private ListMerger<Meeting, int> sut;
        private ISplitter<Meeting, int> _splitter;

        [SetUp]
        public void SetUp()
        {
            _splitter = A.Fake<ISplitter<Meeting, int>>(x => x.Strict().Named("Merger"));
            sut = new ListMerger<Meeting, int>(_splitter);
        }

        [Test]
        public void return_last_since_everything_overlaps()
        {
            var m1 = MeetingHelper.CreateMeeting(1, 1);
            var m2 = MeetingHelper.CreateMeeting(1, 1);
            var m3 = MeetingHelper.CreateMeeting(1, 1);

            var inputItems = new[] {m1, m2, m3};

            var m4 = MeetingHelper.CreateMeeting(4, 4);

            var merge1And2 = new[] {new SplitItem<Meeting>(m2, m2)};
            var merge2And3 = new[] {new SplitItem<Meeting>(m3, m3)};

            A.CallTo(() => _splitter.Split(m1, m2)).Returns(merge1And2);
            A.CallTo(() => _splitter.Split(m2, m3)).Returns(merge2And3);

            var result = sut.Merge(inputItems).ToArray();

            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(m3));
        }

        [Test]
        public void return_all_since_nothing_overlaps()
        {
            var m1 = MeetingHelper.CreateMeeting(1, 1);
            var m2 = MeetingHelper.CreateMeeting(2, 2);
            var m3 = MeetingHelper.CreateMeeting(3, 3);

            var inputItems = new[] {m1, m2, m3};

            var m4 = MeetingHelper.CreateMeeting(4, 4);

            var merge1And2 = new[] {new SplitItem<Meeting>(m1, m1), new SplitItem<Meeting>(m2, m2)};
            var merge1And3 = new[] {new SplitItem<Meeting>(m1, m1), new SplitItem<Meeting>(m3, m3)};
            var merge2And3 = new[] {new SplitItem<Meeting>(m2, m2), new SplitItem<Meeting>(m3, m3)};

            A.CallTo(() => _splitter.Split(m1, m2)).Returns(merge1And2);
            A.CallTo(() => _splitter.Split(m1, m3)).Returns(merge1And3);
            A.CallTo(() => _splitter.Split(m2, m3)).Returns(merge2And3);

            var result = sut.Merge(inputItems).ToArray();

            Assert.That(result, Has.Length.EqualTo(3));
            Assert.That(result[0], Is.EqualTo(m1));
            Assert.That(result[1], Is.EqualTo(m2));
            Assert.That(result[2], Is.EqualTo(m3));
        }

        [Test]
        public void return_first_and_last_when_second_splits()
        {
            var m1 = MeetingHelper.CreateMeeting(0, 9);
            var m1a = MeetingHelper.CreateMeeting(0, 3);
            var m1b = MeetingHelper.CreateMeeting(6, 9);
            var m2 = MeetingHelper.CreateMeeting(3, 6);
            var m3 = MeetingHelper.CreateMeeting(3, 6);

            var inputItems = new[] {m1, m2, m3};

            var m4 = MeetingHelper.CreateMeeting(4, 4);

            var merge1And2 = new[]
            {
                SplitItemHelper.Create(m1, m1a),
                SplitItemHelper.Original(m2),
                SplitItemHelper.Create(m1, m1b),
            };

            var merge1aAnd3 = new[]
            {
                SplitItemHelper.Original(m1a),
                SplitItemHelper.Original(m3),
            };

            var merge2And3 = new[]
            {
                SplitItemHelper.Original(m3)
            };

            var merge1bAnd3 = new[]
            {
                SplitItemHelper.Original(m3),
                SplitItemHelper.Original(m1b),
            };

            A.CallTo(() => _splitter.Split(m1, m2)).Returns(merge1And2);
            A.CallTo(() => _splitter.Split(m1a, m3)).Returns(merge1aAnd3);
            A.CallTo(() => _splitter.Split(m2, m3)).Returns(merge2And3);
            A.CallTo(() => _splitter.Split(m1b, m3)).Returns(merge1bAnd3);

            var result = sut.Merge(inputItems).ToArray();

            Assert.That(result, Has.Length.EqualTo(3));
            Assert.That(result[0], Is.EqualTo(m1a));
            Assert.That(result[1], Is.EqualTo(m1b));
            Assert.That(result[2], Is.EqualTo(m3));
        }
    }
}