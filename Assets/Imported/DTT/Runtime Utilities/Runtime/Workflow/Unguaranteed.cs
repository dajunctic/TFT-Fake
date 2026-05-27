using System;

namespace DTT.Utils.Workflow
{
    /// <summary>
    /// Wraps a value that is not guaranteed to be created.
    /// </summary>
    /// <typeparam name="T">The type of value to create.</typeparam>
    public class Unguaranteed<T> where T : class
    {
        /// <summary>
        /// The unguaranteed value.
        /// </summary>
        private readonly T _value;

        /// <summary>
        /// The unguaranteed value.
        /// </summary>
        public T Value
        {
            get
            {
                OnValueAccess();
                return _value;
            }
        }

        /// <summary>
        /// Whether the value was created or not.
        /// </summary>
        public bool IsValueCreated => _value != null;

        /// <summary>
        /// Called when the value is being accessed.
        /// </summary>
        protected virtual void OnValueAccess()
        {
            if (!IsValueCreated)
                throw new InvalidOperationException("Value was not created.");
        }

        /// <summary>
        /// Initializes the value using a constructor for the value.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        public Unguaranteed(Func<T> constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            _value = constructor();
        }
    }
}
