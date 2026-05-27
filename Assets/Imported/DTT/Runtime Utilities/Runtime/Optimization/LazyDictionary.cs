using System;
using System.Collections.Generic;

namespace DTT.Utils.Optimization
{
    /// <summary>
    /// A dictionary variant that allows for constructors to be added to 
    /// delay the initial creation of a class to when it is needed.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class LazyDictionary<TKey, TValue> : LazyDictionaryBase<TKey, TValue> where TValue : class
    {
        /// <summary>
        /// Wraps the item value and its constructor, providing
        /// a one time initialization upon retrieval.
        /// </summary>
        private class Container
        {
            /// <summary>
            /// The cached item value.
            /// </summary>
            private TValue _value;

            /// <summary>
            /// The constructor with which to initialize the value.
            /// </summary>
            private readonly Func<TValue> _constructor;

            /// <summary>
            /// The accessor to the cached item value.
            /// </summary>
            public TValue Value => _value ?? (_value = _constructor());

            /// <summary>
            /// Creates a new instance, storing the given constructor.
            /// </summary>
            /// <param name="constructor">The constructor with which to initialize the value.</param>
            public Container(Func<TValue> constructor) => _constructor = constructor;
        }

        /// <summary>
        /// Contains the keys with their value in their respective containers.
        /// </summary>
        private readonly Dictionary<TKey, Container> _values = new Dictionary<TKey, Container>();


        /// <summary>
        /// Adds a new item to the dictionary with its respective constructor.
        /// </summary>
        /// <param name="key">The key for the value.</param>
        /// <param name="constructor">
        /// The constructor with which to initialize the value.
        /// </param>
        public void Add(TKey key, Func<TValue> constructor)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            _values.Add(key, new Container(constructor));
        }

        /// <summary>
        /// Should return the item value based on the given key.
        /// </summary>
        /// <param name="key">The key to get the value for.</param>
        /// <returns>The item value.</returns>
        protected override TValue GetValue(TKey key) => _values[key].Value;
    }
}
