using DTT.Utils.Exceptions;
using System;
using System.Collections.Generic;

namespace DTT.Utils.Optimization
{
    /// <summary>
    /// A dictionary variant that allows for constructors to be added to 
    /// delay the initial creation of a struct value to where when it is needed
    /// using the <see cref="Nullable{T}"/> wrapper struct.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class LazyValueDictionary<TKey, TValue> : LazyDictionaryBase<TKey, TValue> where TValue : struct
    {
        /// <summary>
        /// Wraps the item value and its constructor, providing
        /// a one time initialization upon retrieval.
        /// </summary>
        private class Container
        {
            /// <summary>
            /// The cached item value. It is wrapped inside a <see cref="Nullable{T}"/>
            /// struct to check whether it is initialized or not.
            /// </summary>
            private TValue? _nullableValue;

            /// <summary>
            /// The constructor with which to initialize the value.
            /// </summary>
            private readonly Func<TValue> _constructor;

            /// <summary>
            /// The accessor to the cached item value. It returns the value of the nullable wrapper
            /// if it has it. Otherwise it will assign the value using the constructor and return
            /// the resulting value.
            /// </summary>
            public TValue Value => _nullableValue ?? (_nullableValue = _constructor()).Value;

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
                throw new LazyDictionaryException("Name of property is null");

            if (constructor == null)
                throw new LazyDictionaryException($"Constructor of {key} is null.");

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
