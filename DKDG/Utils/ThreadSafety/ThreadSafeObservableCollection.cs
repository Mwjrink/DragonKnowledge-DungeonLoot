using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Data;

namespace DKDG.Utils
{
    public class ThreadSafeObservableCollection<T> : ThreadSafeCollection<ObservableCollection<T>, T>, INotifyCollectionChanged, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        #region Events

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion Events

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

        private ThreadSafeObservableCollection() : base(new ObservableCollection<T>())
        {
        }

        private ThreadSafeObservableCollection(IEnumerable<T> inner) : base(new ObservableCollection<T>(inner))
        {
        }

        #endregion Constructors

        #region Methods

        private static void lockCollection(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
        {
            var locker = (ReaderWriterLockSlim)context;
            if (writeAccess)
                using (locker.Write())
                    accessMethod.Invoke();
            else
                using (locker.Read())
                    accessMethod.Invoke();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

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

        public ThreadSafeObservableCollection<T> New()
        {
            ThreadSafeObservableCollection<T> temp = null;
            Dispatch.er.Invoke(() =>
            {
                temp = new ThreadSafeObservableCollection<T>();
                BindingOperations.EnableCollectionSynchronization(collection, locker, lockCollection);
            });
            collection.CollectionChanged += OnCollectionChanged;
            return temp;
        }

        public ThreadSafeObservableCollection<T> New(IEnumerable<T> inner)
        {
            ThreadSafeObservableCollection<T> temp = null;
            Dispatch.er.Invoke(() =>
            {
                temp = new ThreadSafeObservableCollection<T>(inner);
                BindingOperations.EnableCollectionSynchronization(collection, locker, lockCollection);
            });
            collection.CollectionChanged += OnCollectionChanged;
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
