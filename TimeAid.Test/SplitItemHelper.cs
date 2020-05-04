namespace TimeAid.Test
{
    public static class SplitItemHelper
    {
        public static SplitItem<T> Create<T>(T original, T value)
        {
            return new SplitItem<T>(original, value);
        }

        public static SplitItem<T> Original<T>(T original)
        {
            return new SplitItem<T>(original, original);
        }
    }
}