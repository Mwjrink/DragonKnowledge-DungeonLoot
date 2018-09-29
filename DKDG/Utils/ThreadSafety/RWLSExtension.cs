using System;
using System.Threading;

internal static class RWLSExtension
{
    #region Structs

    internal struct ReadLockToken : IDisposable
    {
        #region Fields

        private readonly ReaderWriterLockSlim readLock;

        #endregion Fields

        #region Constructors

        public ReadLockToken(ReaderWriterLockSlim Lock)
        {
            Lock.EnterReadLock();
            readLock = Lock;
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            readLock.ExitReadLock();
        }

        #endregion Methods
    }

    internal struct WriteLockToken : IDisposable
    {
        #region Fields

        private readonly ReaderWriterLockSlim writeLock;

        #endregion Fields

        #region Constructors

        public WriteLockToken(ReaderWriterLockSlim Lock)
        {
            Lock.EnterWriteLock();
            writeLock = Lock;
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            writeLock.ExitWriteLock();
        }

        #endregion Methods
    }

    #endregion Structs

    #region Methods

    public static ReadLockToken Read(this ReaderWriterLockSlim readerWriterLock)
    {
        return new ReadLockToken(readerWriterLock);
    }

    public static WriteLockToken Write(this ReaderWriterLockSlim readerWriterLock)
    {
        return new WriteLockToken(readerWriterLock);
    }

    #endregion Methods
}
