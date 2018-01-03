using System;
using System.Linq;
using System.Reflection;

namespace Gizy.Extensions
{
    public static class TypeExtensions
    {
        public static bool Implements<T>(this Type type, T @interface) where T : class
        {
            if (@interface != null && (!(@interface is Type) || !(@interface as Type).IsInterface))
            {
                throw new ArgumentException("Only interfaces can be 'implemented'.");
            }
    
            return (@interface as Type).IsAssignableFrom(type);
        }

        public static bool IsSubClassOfGeneric(this Type child, Type parent)
        {
            if (child == parent)
                return false;
        
            if (child.IsSubclassOf(parent))
                return true;
    
            Type[] parameters = parent.GetGenericArguments();
            bool isParameterLessGeneric = !(parameters.Length > 0 && (parameters[0].Attributes & TypeAttributes.BeforeFieldInit) == TypeAttributes.BeforeFieldInit);
    
            while (child != null && child != typeof(object))
            {
                Type cur = GetFullTypeDefinition(child);
                if (parent == cur || isParameterLessGeneric && cur.GetInterfaces().Select(GetFullTypeDefinition).Contains(GetFullTypeDefinition(parent)))
                    return true;
                if (!isParameterLessGeneric)
                    if (GetFullTypeDefinition(parent) == cur && !cur.IsInterface)
                    {
                        if (VerifyGenericArguments(GetFullTypeDefinition(parent), cur))
                            if (VerifyGenericArguments(parent, child))
                                return true;
                    }
                    else
                    if (child.GetInterfaces().Where(i => GetFullTypeDefinition(parent) == GetFullTypeDefinition(i)).Any(item => VerifyGenericArguments(parent, item)))
                    {
                        return true;
                    }

                child = child.BaseType;
            }
    
            return false;
        }

        private static Type GetFullTypeDefinition(Type type)
        {
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        private static bool VerifyGenericArguments(Type parent, Type child)
        {
            Type[] childArguments = child.GetGenericArguments();
            Type[] parentArguments = parent.GetGenericArguments();
            if (childArguments.Length == parentArguments.Length)
                return !childArguments.Where((t, i) => (t.Assembly != parentArguments[i].Assembly || t.Name != parentArguments[i].Name || t.Namespace != parentArguments[i].Namespace) && !t.IsSubclassOf(parentArguments[i])).Any();

            return true;
        }
    }
}