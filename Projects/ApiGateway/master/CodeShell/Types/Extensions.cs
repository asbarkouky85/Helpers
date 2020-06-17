using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Types
{
    public static class Extensions {
        public static string GetVersionString(this Assembly assembly)
        {
            var asem = assembly.CustomAttributes.Where(d => d.AttributeType == typeof(AssemblyFileVersionAttribute)).FirstOrDefault();
            if (asem == null)
                return null;
            if (asem.ConstructorArguments.Count == 0)
                return null;
            return (string)asem.ConstructorArguments.FirstOrDefault().Value;
        }
        public static bool IsNullable(this Type type)
        {
            if (type.IsGenericType)
                return type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
            return false;
        }

        public static Type RealType(this Type type)
        {
            if (type.IsNullable())
                return type.GetGenericArguments()[0];
            return type;
        }

        public static Type InnerType(this Type type)
        {
            Type[] args = type.GetGenericArguments();
            if (args.Length > 0)
                return args[0];
            return type;
        }

        public static bool Implements(this Type type, Type check)
        {
            return type.GetInterfaces().Contains(check);
        }

        public static bool IsDecimalType(this Type type)
        {
            return type.Equals(typeof(decimal)) || type.Equals(typeof(double)) || type.Equals(typeof(float));
        }

        public static bool IsIntgerType(this Type type)
        {
            return type.Equals(typeof(sbyte)) || type.Equals(typeof(byte)) || type.Equals(typeof(int)) || type.Equals(typeof(long)) || type.Equals(typeof(uint)) || type.Equals(typeof(ulong));
        }
    }
}
