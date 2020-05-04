using System;

namespace TimeAid
{
    public class Period<T> where T : IComparable
    {
        public Period(T start, T end)
        {
            Start = start;
            End = end;
        }

        public T Start { get; }
        public T End { get; }
    }
}