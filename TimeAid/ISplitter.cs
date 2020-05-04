using System;

namespace TimeAid
{
    public interface ISplitter<T, IntervalType> where IntervalType : IComparable
    {
        SplitItem<T>[] Split(T oldItem, T newItem);
    }
}