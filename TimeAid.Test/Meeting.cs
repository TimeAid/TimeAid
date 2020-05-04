namespace TimeAid.Test
{
    public class Meeting
    {
        public int Start;
        public int End;
        public string Name;
        public Meeting OriginalMeeting;

        public Meeting(int start, int end, string name, Meeting original = null)
        {
            Start = start;
            End = end;
            Name = name;
            OriginalMeeting = original;
        }

        public override string ToString()
        {
            if (OriginalMeeting == null)
            {
                return Name;
            }
            else
            {
                return $"{Name} (Clone of {OriginalMeeting})";
            }
        }
    }
}