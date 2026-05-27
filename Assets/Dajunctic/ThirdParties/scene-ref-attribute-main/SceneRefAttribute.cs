using System;
using UnityEngine;

namespace KBCore.Refs
{

    internal enum RefLoc
    {

        Anywhere = -1,

        Self = 0,

        Parent = 1,

        Child = 2,

        Scene = 4,
    }

    [Flags]
    public enum Flag
    {

        None = 0,

        Optional = 1 << 0,

        IncludeInactive = 1 << 1,

        Editable = 1 << 2,

        ExcludeSelf = 1 << 3,

        EditableAnywhere = 1 << 4 | Editable
    }

    [AttributeUsage(AttributeTargets.Field)]
    public abstract class SceneRefAttribute : PropertyAttribute
    {
        internal RefLoc Loc { get; }
        internal Flag Flags { get; }

        internal SceneRefFilter Filter
        {
            get
            {
                if (this._filterType == null)
                    return null;
                return (SceneRefFilter) Activator.CreateInstance(this._filterType);
            }
        }

        private readonly Type _filterType;

        internal SceneRefAttribute(
            RefLoc loc, 
            Flag flags,
            Type filter
        ) 
        {
            this.Loc = loc;
            this.Flags = flags;
            this._filterType = filter;
        }

        internal bool HasFlags(Flag flags)
            => (this.Flags & flags) == flags;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AnywhereAttribute : SceneRefAttribute
    {
        public AnywhereAttribute(Flag flags = Flag.None, Type filter = null) 
            : base(RefLoc.Anywhere, flags, filter)
        {}
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SelfAttribute : SceneRefAttribute
    {
        public SelfAttribute(Flag flags = Flag.None, Type filter = null) 
            : base(RefLoc.Self, flags, filter)
        {}
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ParentAttribute : SceneRefAttribute
    {
        public ParentAttribute(Flag flags = Flag.None, Type filter = null) 
            : base(RefLoc.Parent, flags, filter)
        {}
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ChildAttribute : SceneRefAttribute
    {
        public ChildAttribute(Flag flags = Flag.None, Type filter = null) 
            : base(RefLoc.Child, flags, filter)
        {}
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SceneAttribute : SceneRefAttribute
    {
        public SceneAttribute(Flag flags = Flag.None, Type filter = null) 
            : base(RefLoc.Scene, flags, filter)
        {}
    }
}
