using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mozart.Utility
{
    /// <summary>
    /// A static generic thread-safe repository for in-memory storage
    /// </summary>
    /// <typeparam name="TK">Key Type</typeparam>
    /// <typeparam name="T">Value Type</typeparam>
    public class Repository<TK, T>
    {
        private static ConcurrentDictionary<TK, T> Container { get; set; }

        static Repository()
        {
            Container = new ConcurrentDictionary<TK, T>();
        }

        /// <summary>
        /// Adds or updates the entity T with key TK
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T AddOrUpdate(TK key, T entity)
        {
            return Container.AddOrUpdate(key, entity, (s, o) => entity);
        }

        /// <summary>
        /// Removes the entity T with key TK
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(TK key)
        {
            T entity;
            return Container.TryRemove(key, out entity);
        }

        /// <summary>
        /// Removes all entities matching the expression f
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int Remove(Func<T, bool> f)
        {
            return FindWithKeys(f).Count(o => Remove(o.Key));
        }

        /// <summary>
        /// Clears the repository
        /// </summary>
        public static void RemoveAll()
        {
            Container = new ConcurrentDictionary<TK, T>();
        }

        /// <summary>
        /// Find all entities T matching the expression f
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<T> Find(Func<T, bool> f)
        {
            return Container.Values.Where(f);
        }

        /// <summary>
        /// Find all entities T matching the expression f and returns a Dictionary TK,T
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IDictionary<TK, T> FindWithKeys(Func<T, bool> f)
        {
            var y = from x in Container
                    where f.Invoke(x.Value)
                    select x;
            return y.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Returns all entities as a Dictionary TK,T
        /// </summary>
        /// <returns></returns>
        public static IDictionary<TK, T> GetAllWithKeys()
        {
            return Container;
        }

        /// <summary>
        /// Returns all entities T from the repository
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> GetAll()
        {
            return Container.Values;
        }

        /// <summary>
        /// Get a single entity T with the key TK
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetById(TK key)
        {
            return Container.ContainsKey(key) ? Container[key] : default(T);
        }

        /// <summary>
        /// Get a single entity T as a KeyValuePair TK,T with the key TK
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static KeyValuePair<TK, T> GetByIdWithKey(TK key)
        {
            return Container.ContainsKey(key) ? new KeyValuePair<TK, T>(key, Container[key]) : new KeyValuePair<TK, T>(key, default(T));
        }

        /// <summary>
        /// Checks if the repository has a key TK
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(TK key)
        {
            return Container.ContainsKey(key);
        }
    }

    /// <summary>
    /// A static generic thread-safe repository for in-memory storage
    /// </summary>
    /// <typeparam name="TK">Key Type</typeparam>
    /// <typeparam name="T">Value Type</typeparam>
    public class RepositoryInstance<TK, T>
    {
        public ConcurrentDictionary<TK, T> Container { get; set; }

        public RepositoryInstance()
        {
            Container = new ConcurrentDictionary<TK, T>();
        }

        /// <summary>
        /// Adds or updates the entity T with key TK
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddOrUpdate(TK key, T entity)
        {
            return Container.AddOrUpdate(key, entity, (s, o) => entity);
        }

        /// <summary>
        /// Removes the entity T with key TK
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TK key)
        {
            T entity;
            return Container.TryRemove(key, out entity);
        }

        /// <summary>
        /// Removes all entities matching the expression f
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public int Remove(Func<T, bool> f)
        {
            return FindWithKeys(f).Count(o => Remove(o.Key));
        }

        /// <summary>
        /// Clears the repository
        /// </summary>
        public void RemoveAll()
        {
            Container = new ConcurrentDictionary<TK, T>();
        }

        /// <summary>
        /// Find all entities T matching the expression f
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IEnumerable<T> Find(Func<T, bool> f)
        {
            return Container.Values.Where(f);
        }

        /// <summary>
        /// Find all entities T matching the expression f and returns a Dictionary TK,T
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IDictionary<TK, T> FindWithKeys(Func<T, bool> f)
        {
            var y = from x in Container
                    where f.Invoke(x.Value)
                    select x;
            return y.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Returns all entities as a Dictionary TK,T
        /// </summary>
        /// <returns></returns>
        public IDictionary<TK, T> GetAllWithKeys()
        {
            return Container;
        }

        /// <summary>
        /// Returns all entities T from the repository
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return Container.Values;
        }

        /// <summary>
        /// Get a single entity T with the key TK
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetById(TK key)
        {
            return Container.ContainsKey(key) ? Container[key] : default(T);
        }

        /// <summary>
        /// Get a single entity T as a KeyValuePair TK,T with the key TK
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyValuePair<TK, T> GetByIdWithKey(TK key)
        {
            return Container.ContainsKey(key) ? new KeyValuePair<TK, T>(key, Container[key]) : new KeyValuePair<TK, T>(key, default(T));
        }

        /// <summary>
        /// Checks if the repository has a key TK
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TK key)
        {
            return Container.ContainsKey(key);
        }
    }
}
