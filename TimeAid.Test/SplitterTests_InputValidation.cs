using System;
using System.Linq;
using NUnit.Framework;

namespace TimeAid.Test
{
    public class SplitterTests_InputValidation
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
        public void Throws_when_oldItem_isNull()
        {
            Meeting oldItem = null;
            Meeting newItem = MeetingHelper.CreateMeeting(0, 3);

            Assert.That(
                () => _splitter.Split(oldItem, newItem).ToArray(),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("oldItem")
            );
        }
        
        [Test]
        public void Throws_when_newItem_isNull()
        {
            var oldItem = MeetingHelper.CreateMeeting(0, 3);
            Meeting newItem = null;
            Assert.That(
                () => _splitter.Split(oldItem, newItem).ToArray(),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("newItem")
            );
        }

        [Test]
        public void Throws_when_oldItem_has_end_before_start()
        {
            var oldItem = MeetingHelper.CreateMeeting(3, 0);
            var newItem = MeetingHelper.CreateMeeting(0, 3);
            Assert.That(
                () => _splitter.Split(oldItem, newItem).ToArray(),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("oldItem")
                    .And.Message.StartsWith("End cannot occur before start")
            );
        }        
        
        [Test]
        public void Throws_when_newItem_has_end_before_start()
        {
            var oldItem = MeetingHelper.CreateMeeting(0, 0);
            var newItem = MeetingHelper.CreateMeeting(3, 0);
            Assert.That(
                () => _splitter.Split(oldItem, newItem).ToArray(),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("newItem")
                    .And.Message.StartsWith("End cannot occur before start")
            );
        }


    }
}