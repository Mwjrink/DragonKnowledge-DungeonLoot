using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace DKDG.Utils
{
    public class ThreadSafeEnumerator<T> : IEnumerator<T>
    {
        #region Fields

        private readonly IEnumerator<T> inner;
        private readonly ReaderWriterLockSlim locker;

        #endregion Fields

        #region Properties

        public T Current => inner.Current;

        object IEnumerator.Current => inner.Current;

        #endregion Properties

        #region Constructors

        public ThreadSafeEnumerator(IEnumerator<T> inner, ReaderWriterLockSlim locker)
        {
            this.inner = inner;
            this.locker = locker;

            locker.EnterReadLock();
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            inner.Dispose();
            locker.ExitReadLock();
        }

        public bool MoveNext()
        {
            return inner.MoveNext();
        }

        public void Reset()
        {
            inner.Reset();
        }

        #endregion Methods
    }
}
