using DTT.Utils.Exceptions;
using System;

namespace DTT.Utils.Optimization
{
    /// <summary>
    /// Provides an abstract implementation of a class that stores
    /// values and their constructor to create them the first time 
    /// they are used.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class LazyDictionaryBase<TKey, TValue>
    {
        /// <summary>
        /// The accessor to the collection. The argument should always be the name
        /// of the property.
        /// </summary>
        /// <param name="key">The name of the key.</param>
        /// <returns>The collection value.</returns>
        public TValue this[TKey key]
        {
            get
            {
                try
                {
                    return GetValue(key);
                }
                catch (Exception e)
                {
                    throw new LazyDictionaryException($"Failed returning item for key {key}.", e);
                }
            }
        }

        /// <summary>
        /// Should return the item value based on the given key.
        /// </summary>
        /// <param name="key">The key to get the value for.</param>
        /// <returns>The item value.</returns>
        protected abstract TValue GetValue(TKey key);
    }
}
