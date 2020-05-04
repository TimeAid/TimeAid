namespace TimeAid
{
    public interface ISplitter<T>
    {
        SplitItem<T>[] Split(T oldItem, T newItem);
    }
}