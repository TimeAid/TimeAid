using System;

namespace TimeAid
{
    public delegate IntervalType TimeFunc<in T, out IntervalType>(T value) where IntervalType : IComparable;
}