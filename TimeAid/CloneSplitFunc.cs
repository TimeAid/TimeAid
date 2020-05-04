using System;

namespace TimeAid
{
    public delegate T CloneSplitFunc<T, in IntervalType>(T original, IntervalType newStart, IntervalType newEnd)
        where IntervalType : IComparable;
}