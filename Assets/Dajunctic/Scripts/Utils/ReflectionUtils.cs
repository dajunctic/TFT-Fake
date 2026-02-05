using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Dajunctic
{
    public static class ReflectionUtils
    {
        public static BindingFlags CommonBindings => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static IEnumerable<FieldInfo> GetAttributeFieldInfos<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetFields(CommonBindings).Where(i => i.GetCustomAttribute<TAttribute>() != null);
        }

        public static IEnumerable<string> GetAttributeStringValues<TAttribute>(this object target) where TAttribute : Attribute
        {
            var fieldInfos = target.GetType().GetAttributeFieldInfos<TAttribute>();
            var result = new List<string>();

            foreach (var fieldInfo in fieldInfos)
            {
                var value = fieldInfo.GetValue(target);
                if (value is string stringValue)
                {
                    result.Add(stringValue);
                }
                else if (value is IEnumerable<string> stringEnumerable)
                {
                    result.AddRange(stringEnumerable);
                }
            }
            return result;        
        }
    }
}