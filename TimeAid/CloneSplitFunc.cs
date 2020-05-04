namespace TimeAid
{
    public delegate T CloneSplitFunc<T>(T original, int newStart, int newEnd);
}