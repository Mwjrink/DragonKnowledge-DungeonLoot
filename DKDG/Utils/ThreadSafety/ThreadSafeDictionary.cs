using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DKDG.Utils
{
    public class ThreadSafeDictionary<K, V> : ThreadSafeCollection<Dictionary<K, V>, KeyValuePair<K, V>>, IDictionary<K, V>, IEnumerable, ISerializable, IDeserializationCallback
    {
        #region Indexers

        public V this[K index]
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

        #region Properties

        public ICollection<K> Keys
        {
            get
            {
                using (locker.Read())
                    return collection.Keys;
            }
        }

        public ICollection<V> Values
        {
            get
            {
                using (locker.Read())
                    return collection.Values;
            }
        }

        #endregion Properties

        #region Constructors

        private ThreadSafeDictionary() : base(new Dictionary<K, V>())
        {
        }

        #endregion Constructors

        #region Methods

        public void Add(K key, V value)
        {
            using (locker.Write())
                collection.Add(key, value);
        }

        public bool ContainsKey(K key)
        {
            using (locker.Read())
                return collection.ContainsKey(key);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            using (locker.Read())
                collection.GetObjectData(info, context);
        }

        public ThreadSafeDictionary<K, V> New()
        {
            ThreadSafeDictionary<K, V> temp = null;
            Dispatch.er.Invoke(() => { temp = new ThreadSafeDictionary<K, V>(); });
            return temp;
        }

        public void OnDeserialization(object sender)
        {
            using (locker.Read())
                collection.OnDeserialization(sender);
        }

        public bool Remove(K key)
        {
            using (locker.Write())
                return collection.Remove(key);
        }

        public bool TryGetValue(K key, out V value)
        {
            using (locker.Read())
                return collection.TryGetValue(key, out value);
        }

        #endregion Methods
    }
}
