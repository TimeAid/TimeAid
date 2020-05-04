using System;

namespace TimeAid
{
    public class Splitter<T> : ISplitter<T>
    {
        private readonly TimeFunc<T> _startFunc;
        private readonly TimeFunc<T> _endFunc;
        private readonly CloneSplitFunc<T> _clone;

        public Splitter(TimeFunc<T> startFunc, TimeFunc<T> endFunc, CloneSplitFunc<T> clone)
        {
            _startFunc = startFunc;
            _endFunc = endFunc;
            _clone = clone;
        }

        public SplitItem<T>[] Split(T oldItem, T newItem)
        {
            if (oldItem == null) throw new ArgumentNullException(nameof(oldItem));
            if (newItem == null) throw new ArgumentNullException(nameof(newItem));

            var oldPeriod = CreatePeriod(oldItem);
            var newPeriod = CreatePeriod(newItem);

            if (oldPeriod.End < oldPeriod.Start)
            {
                throw new ArgumentException("End cannot occur before start", nameof(oldItem));
            }

            if (newPeriod.End < newPeriod.Start)
            {
                throw new ArgumentException("End cannot occur before start", nameof(newItem));
            }

            if (newPeriod.Start <= oldPeriod.Start && newPeriod.End >= oldPeriod.End)
            {
                return new[] {new SplitItem<T>(newItem, newItem)};
            }

            if (oldPeriod.End <= newPeriod.Start)
            {
                return new[]
                {
                    new SplitItem<T>(oldItem, oldItem),
                    new SplitItem<T>(newItem, newItem)
                };
            }

            if (newPeriod.End <= oldPeriod.Start)
            {
                return new[]
                {
                    new SplitItem<T>(newItem, newItem),
                    new SplitItem<T>(oldItem, oldItem)
                };
            }

            if (newPeriod.Start > oldPeriod.Start && newPeriod.End < oldPeriod.End)
            {
                return new[]
                {
                    new SplitItem<T>(oldItem, _clone(oldItem, oldPeriod.Start, newPeriod.Start)),
                    new SplitItem<T>(newItem, newItem),
                    new SplitItem<T>(oldItem, _clone(oldItem, newPeriod.End, oldPeriod.End))
                };
            }

            if (newPeriod.Start < oldPeriod.End && newPeriod.Start > oldPeriod.Start)
            {
                return new[]
                {
                    new SplitItem<T>(oldItem, _clone(oldItem, oldPeriod.Start, newPeriod.Start)),
                    new SplitItem<T>(newItem, newItem),
                };
            }

            if (newPeriod.End > oldPeriod.Start)
            {
                return new[]
                {
                    new SplitItem<T>(newItem, newItem),
                    new SplitItem<T>(oldItem, _clone(oldItem, newPeriod.End, oldPeriod.End)),
                };
            }

            throw new Exception(
                $"An unsupported period combination was detected: OldPeriod ({oldPeriod.Start}-{oldPeriod.End}) and newPeriod({newPeriod.Start}-{newPeriod.End})");
        }

        private Period CreatePeriod(T @event)
        {
            return new Period(_startFunc(@event), _endFunc(@event));
        }
    }
}