using System;
using UnityEngine;

namespace KBCore.Refs
{

    [Serializable]
    public class InterfaceRef<T> : ISerializableRef<T>
        where T : class
    {

        public T Value
        {
            get
            {
                if (!this._hasCast)
                {
                    this._hasCast = true;
                    this._value = this._implementer as T;
                }
                return this._value;
            }
        }

        object ISerializableRef.SerializedObject
            => this._implementer;

        public Type RefType => typeof(T);

        public bool HasSerializedObject => this._implementer != null;
        
        [SerializeField] private Component _implementer;
        private bool _hasCast;
        private T _value;

        bool ISerializableRef.OnSerialize(object value)
        {
            Component c = (Component)value;
            if (c == this._implementer)
                return false;

            this._hasCast = false;
            this._value = null;
            this._implementer = c;
            return true;
        }

        void ISerializableRef.Clear()
        {
            this._hasCast = false;
            this._value = null;
            this._implementer = null;
        }
    }
}
