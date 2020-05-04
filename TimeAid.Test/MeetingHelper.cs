namespace TimeAid.Test
{
    public static class MeetingHelper
    {
        public static Meeting CreateMeeting(int start, int end)
        {
            return new Meeting(start, end, $"Meeting {start}-{end}");
        }

        public static int StartSelector(Meeting x) => x.Start;
        public static int EndSelector(Meeting x) => x.End;

        public static Meeting CreateClone(Meeting original, int start, int end)
        {
            return new Meeting(start, end, original.Name, original);
        }
    }
}