namespace TimeAid
{
    public class SplitItem<T>
    {
        /// <summary>
        /// The original or modified value
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// The original value
        /// </summary>
        public T Original { get; }
        
        public SplitItem(T original, T value)
        {
            Value = value;
            Original = original;
        }
    }
}