using System;

namespace KBCore.Refs
{
    internal interface ISerializableRef
    {
        Type RefType { get; }
        object SerializedObject { get; }
        bool HasSerializedObject { get; }

        bool OnSerialize(object value);

        void Clear();
    }

    internal interface ISerializableRef<T> : ISerializableRef
        where T : class
    {
        T Value { get; }
    }
}
