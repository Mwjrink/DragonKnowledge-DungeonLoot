using System.Collections.Generic;

namespace DKDG.Utils
{
    public class ThreadSafeList<T> : ThreadSafeCollection<List<T>, T>, IList<T>, ICollection<T>, IEnumerable<T>
    {
        #region Indexers

        public T this[int index]
        {
            get
            {
                using (locker.Read())
                    return collection[index];
            }

            set
            {
                using (locker.Write())
                    collection[index] = value;
            }
        }

        #endregion Indexers

        #region Constructors

        private ThreadSafeList() : base(new List<T>())
        {
        }

        private ThreadSafeList(IEnumerable<T> inner) : base(new List<T>(inner))
        {
        }

        #endregion Constructors

        #region Methods

        public int IndexOf(T item)
        {
            using (locker.Read())
                return collection.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            using (locker.Write())
                collection.Insert(index, item);
        }

        public ThreadSafeList<T> New()
        {
            ThreadSafeList<T> temp = null;
            Dispatch.er.Invoke(() => { temp = new ThreadSafeList<T>(); });
            return temp;
        }

        public ThreadSafeList<T> New(IEnumerable<T> inner)
        {
            ThreadSafeList<T> temp = null;
            Dispatch.er.Invoke(() => { temp = new ThreadSafeList<T>(inner); });
            return temp;
        }

        public void RemoveAt(int index)
        {
            using (locker.Write())
                collection.RemoveAt(index);
        }

        #endregion Methods
    }
}
