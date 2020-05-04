using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeAid
{
    public class ListMerger<T>
    {
        private readonly ISplitter<T> _splitter;

        public ListMerger(ISplitter<T> splitter)
        {
            _splitter = splitter;
        }

        public IEnumerable<T> Merge(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var result = new List<T>();
            foreach (var newItem in items)
            {
                var oldItems = result;
                result = new List<T>();
                foreach (var oldItem in oldItems)
                {
                    var mergeResult = _splitter.Split(oldItem, newItem);
                    if (mergeResult.Length == 0)
                    {
                        throw new Exception("The merger must not provided an empty result");
                    }

                    var newItemsInList = mergeResult.Count(x => ReferenceEquals(x.Original, newItem));
                    var oldItemsAtStart = mergeResult.TakeWhile(x => ReferenceEquals(x.Original, oldItem)).Count();
                    var oldItemsAtEnd = mergeResult.Reverse().TakeWhile(x => ReferenceEquals(x.Original, oldItem))
                        .Count();

                    var hasInventedItems = mergeResult.Any(
                        x => !(ReferenceEquals(x.Original, oldItem) || ReferenceEquals(x.Original, newItem))
                    );

                    if (newItemsInList == 0)
                    {
                        throw new Exception("The splitter must return the newItem");
                    }

                    if (newItemsInList > 1)
                    {
                        throw new Exception("The splitter must return the newItem only once");
                    }

                    if (oldItemsAtStart >= 2 || oldItemsAtEnd >= 2)
                    {
                        throw new Exception(
                            "The splitter can return oldItem only once before and/or once after newItem");
                    }

                    if (hasInventedItems)
                    {
                        throw new Exception("The splitter should not invent new original items");
                    }

                    result.AddRange(mergeResult.Where(x => !Equals(x.Original, newItem)).Select(x => x.Value));
                }

                result.Add(newItem);
            }

            return result;
        }
    }
}