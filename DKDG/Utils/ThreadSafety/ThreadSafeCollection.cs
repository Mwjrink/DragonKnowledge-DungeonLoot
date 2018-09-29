using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace DKDG.Utils
{
    public abstract class ThreadSafeCollection<U, T> : ICollection<T>
        where U : ICollection<T>
    {
        #region Fields

        protected readonly U collection;
        protected readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        #endregion Fields

        #region Properties

        public int Count
        {
            get
            {
                using (locker.Read())
                    return collection.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                using (locker.Read())
                    return collection.IsReadOnly;
            }
        }

        #endregion Properties

        #region Constructors

        protected ThreadSafeCollection(U inner)
        {
            collection = inner;
        }

        #endregion Constructors

        #region Methods

        public void Add(T item)
        {
            using (locker.Write())
                collection.Add(item);
        }

        public void Clear()
        {
            using (locker.Write())
                collection.Clear();
        }

        public bool Contains(T item)
        {
            using (locker.Read())
                return collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            using (locker.Read())
                collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            using (locker.Read())
                return new ThreadSafeEnumerator<T>(collection.GetEnumerator(), locker);
        }

        public bool Remove(T item)
        {
            using (locker.Write())
                return collection.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            using (locker.Read())
                return new ThreadSafeEnumerator<T>(collection.GetEnumerator(), locker);
        }

        #endregion Methods
    }
}
