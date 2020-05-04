using System;

namespace TimeAid
{
    public class Splitter<T,IntervalType> : ISplitter<T, IntervalType> where IntervalType : IComparable
    {
        private readonly TimeFunc<T, IntervalType> _startFunc;
        private readonly TimeFunc<T, IntervalType> _endFunc;
        private readonly CloneSplitFunc<T, IntervalType> _clone;

        public Splitter(TimeFunc<T, IntervalType> startFunc, TimeFunc<T, IntervalType> endFunc, CloneSplitFunc<T, IntervalType> clone)
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

            if (oldPeriod.End.CompareTo(oldPeriod.Start) < 0)
            {
                throw new ArgumentException("End cannot occur before start", nameof(oldItem));
            }

            if (newPeriod.End.CompareTo(newPeriod.Start) < 0)
            {
                throw new ArgumentException("End cannot occur before start", nameof(newItem));
            }

            if (newPeriod.Start.CompareTo(oldPeriod.Start) <= 0 && newPeriod.End.CompareTo(oldPeriod.End) >= 0)
            {
                return new[] {new SplitItem<T>(newItem, newItem)};
            }

            if (oldPeriod.End.CompareTo(newPeriod.Start) <= 0)
            {
                return new[]
                {
                    new SplitItem<T>(oldItem, oldItem),
                    new SplitItem<T>(newItem, newItem)
                };
            }

            if (newPeriod.End.CompareTo(oldPeriod.Start) <= 0)
            {
                return new[]
                {
                    new SplitItem<T>(newItem, newItem),
                    new SplitItem<T>(oldItem, oldItem)
                };
            }

            if (newPeriod.Start.CompareTo(oldPeriod.Start) > 0 && newPeriod.End.CompareTo(oldPeriod.End) < 0)
            {
                return new[]
                {
                    new SplitItem<T>(oldItem, _clone(oldItem, oldPeriod.Start, newPeriod.Start)),
                    new SplitItem<T>(newItem, newItem),
                    new SplitItem<T>(oldItem, _clone(oldItem, newPeriod.End, oldPeriod.End))
                };
            }

            if (newPeriod.Start.CompareTo(oldPeriod.End) < 0 && newPeriod.Start.CompareTo(oldPeriod.Start) > 0)
            {
                return new[]
                {
                    new SplitItem<T>(oldItem, _clone(oldItem, oldPeriod.Start, newPeriod.Start)),
                    new SplitItem<T>(newItem, newItem),
                };
            }

            if (newPeriod.End.CompareTo(oldPeriod.Start) > 0)
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

        private Period<IntervalType> CreatePeriod(T @event)
        {
            return new Period<IntervalType>(_startFunc(@event), _endFunc(@event));
        }
    }
}