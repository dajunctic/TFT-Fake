using System;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides extended type checking operations.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if the type implements the other type.
        /// </summary>
        /// <param name="type">The type we need to check.</param>
        /// <param name="interfaceType">The interface we need to check against.</param>
        /// <returns>Whether the interface is implemented.</returns>
        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));

            Type[] interfaces = type.GetInterfaces();
            if (interfaceType.IsGenericTypeDefinition)
            {
                foreach (var item in interfaces)
                    if (item.IsConstructedGenericType && item.GetGenericTypeDefinition() == interfaceType)
                        return true;
            }
            else
            {
                foreach (var item in interfaces)
                    if (item == interfaceType)
                        return true;
            }

            return false;
        }
    }
}