namespace TimeAid
{
    public class Period
    {
        public Period(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; }
        public int End { get; }
    }
}