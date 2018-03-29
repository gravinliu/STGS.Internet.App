using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace STGS.Internet.Tool
{

    public static class ReflectionExtensions
    {

        public static void CopyPropertiesTo<T>(this T self, T other, params string[] excepts)
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var ps = typeInfo.DeclaredProperties;
            foreach (var pi in ps)
            {
                if (!excepts.Contains(pi.Name))
                {
                    var selfValue = pi.GetValue(self);
                    pi.SetValue(other, selfValue);
                }
            }
        }

        public static void CopyPropertiesTo<T>(this IDictionary<string, object> self, T other, params string[] excepts)
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var ps = typeInfo.DeclaredProperties;
            foreach (var pi in ps)
            {
                if (!excepts.Contains(pi.Name))
                {
                    var selfValue = self[pi.Name];
                    pi.SetValue(other, selfValue);
                }
            }
        }

        public static IEnumerable<KeyValuePair<string, T>> AsParamsDictionary<T>(this object self)
        {
            return self?.GetType().GetRuntimeProperties().ToDictionary(info => info.Name, info => (T)info.GetValue(self));
        }

        public static IEnumerable<KeyValuePair<string, string>> AsParamsDictionary(this object self)
        {
            return self?.GetType().GetRuntimeProperties().ToDictionary(info => info.Name, info => info.GetValue(self).ToString());
        }

        public static IEnumerable<object> AsParamsList(this object self)
        {
            return self?.GetType().GetRuntimeProperties().Select(x => x.GetValue(self));
        }

        public static IEnumerable<string> AsParamsNames(this object self)
        {
            return self?.GetType().GetRuntimeProperties().Select(x => x.Name);
        }

        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var type = typeof(TSource);
            var typeInfo = type.GetTypeInfo();

            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda.ToString()));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda.ToString()));
            }

            // TODO: Find the altinative for ReflectedType.
            var reflectedType = propInfo.DeclaringType;
            var reflectedTypeInfo = reflectedType.GetTypeInfo();
            if (!reflectedTypeInfo.IsAssignableFrom(typeInfo))
            {
                throw new ArgumentException(string.Format("Expresion '{0}' refers to a property that is not from type {1}.", propertyLambda.ToString(), type.FullName));
            }

            return propInfo;
        }

        public static TMetaAttribute GetCustomAttribute<TEnum, TMetaAttribute>(this TEnum self) where TMetaAttribute : Attribute
        {
            var type = typeof(TEnum);
            if (type.IsSubclassOfRawGeneric(typeof(Nullable<>)))
            {
                type = type.GenericTypeArguments[0];
            }
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsEnum)
            {
                var fieldInfo = typeInfo.GetDeclaredField(Enum.GetName(type, self));
                if (fieldInfo != null)
                {
                    var attr = fieldInfo.GetCustomAttribute<TMetaAttribute>();
                    if (attr != null)
                    {
                        return attr;
                    }
                }
            }
            throw new ArgumentException($"Failed to extract text meta data from the given enum object {self} or the given object is not an enum.");
        }

        public static TMetaAttribute GetCustomAttribute<TMetaAttribute>(this object self) where TMetaAttribute : Attribute
        {
            var type = self.GetType();
            if (type.IsSubclassOfRawGeneric(typeof(Nullable<>)))
            {
                type = type.GenericTypeArguments[0];
            }
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsEnum)
            {
                var fieldInfo = typeInfo.GetDeclaredField(Enum.GetName(type, self));
                if (fieldInfo != null)
                {
                    var attr = fieldInfo.GetCustomAttribute<TMetaAttribute>();
                    if (attr != null)
                    {
                        return attr;
                    }
                }
            }
            throw new ArgumentException($"Failed to extract text meta data from the given enum object {self} or the given object is not an enum.");
        }

        public static IEnumerable<TMetaAttribute> GetCustomAttributes<TMetaAttribute>(this Type type) where TMetaAttribute : Attribute
        {
            var typeInfo = type.GetTypeInfo();
            if (type.IsSubclassOfRawGeneric(typeof(Nullable<>)))
            {
                type = type.GenericTypeArguments[0];
            }
            if (typeInfo.IsEnum)
            {
                var list = new List<TMetaAttribute>();
                foreach (var name in Enum.GetNames(type))
                {
                    var fieldInfo = typeInfo.GetDeclaredField(name);
                    if (fieldInfo != null)
                    {
                        var attr = fieldInfo.GetCustomAttribute<TMetaAttribute>();
                        if (attr != null)
                        {
                            list.Add(attr);
                        }
                    }
                }
                return list;
            }
            throw new ArgumentException($"Failed to extract text meta data from the given enum type {typeInfo.FullName} or the given type is not an enum.");
        }

        public static IEnumerable<TProperty> GetCustomAttributes<TMetaAttribute, TProperty>(this Type type, Func<TMetaAttribute, TProperty> selector) where TMetaAttribute : Attribute
        {
            if (type.IsSubclassOfRawGeneric(typeof(Nullable<>)))
            {
                type = type.GenericTypeArguments[0];
            }
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsEnum)
            {
                var list = new List<TProperty>();
                foreach (var name in Enum.GetNames(type))
                {
                    var fieldInfo = typeInfo.GetDeclaredField(name);
                    if (fieldInfo != null)
                    {
                        var attr = fieldInfo.GetCustomAttribute<TMetaAttribute>();
                        if (attr != null)
                        {
                            list.Add(selector(attr));
                        }
                    }
                }
                return list;
            }
            throw new ArgumentException($"Failed to extract text meta data from the given enum type {typeInfo.FullName} or the given type is not an enum.");
        }

        public static TEnum ToEnumValue<TEnum, TMetaAttribute>(this string self, Func<TMetaAttribute, string> selector, StringComparison options = StringComparison.CurrentCultureIgnoreCase) where TMetaAttribute : Attribute
        {
            return self.ToEnumValue<TEnum, TMetaAttribute>((attr, str) => string.Equals(selector(attr), (string)str, options));
        }

        public static TEnum ToEnumValue<TEnum, TMetaAttribute>(this object self, Func<TMetaAttribute, object, bool> comparer) where TMetaAttribute : Attribute
        {
            var type = typeof(TEnum);
            if (type.IsSubclassOfRawGeneric(typeof(Nullable<>)))
            {
                type = type.GenericTypeArguments[0];
            }
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsEnum)
            {
                var fieldInfos = typeInfo.DeclaredFields;
                foreach (var fi in fieldInfos)
                {
                    var attr = fi.GetCustomAttribute<TMetaAttribute>();
                    if (attr != null)
                    {
                        if (comparer(attr, self))
                        {
                            return (TEnum)fi.GetValue(null);
                        }
                    }
                }
            }
            throw new ArgumentException($"Failed to find the mapping enum value that matches the given meta string {self} or the given target type {type.FullName} is not an enum.");
        }

        public static object ToEnumValue<TMetaAttribute>(this object self, Func<TMetaAttribute, object, bool> comparer) where TMetaAttribute : Attribute
        {
            var type = self.GetType();
            if (type.IsSubclassOfRawGeneric(typeof(Nullable<>)))
            {
                type = type.GenericTypeArguments[0];
            }
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsEnum)
            {
                var fieldInfos = typeInfo.DeclaredFields;
                foreach (var fi in fieldInfos)
                {
                    var attr = fi.GetCustomAttribute<TMetaAttribute>();
                    if (attr != null)
                    {
                        if (comparer(attr, self))
                        {
                            return fi.GetValue(null);
                        }
                    }
                }
            }
            throw new ArgumentException($"Failed to find the mapping enum value that matches the given meta string {self} or the given target type {type.FullName} is not an enum.");
        }

        public static IDictionary<string, TValue> ToEnumDictionary<TValue>(this Type type)
        {
            return type.GetRuntimeFields().Where(x => x.IsStatic).OrderBy(x => (TValue)x.GetValue(null)).ToDictionary(x => x.Name, x => (TValue)x.GetValue(null));
        }

        public static IDictionary<string, TValue> ToEnumDictionary<TValue, TOrderKey>(this Type type, Func<KeyValuePair<string, TValue>, TOrderKey> comparerKeySelector, bool descending = false)
        {
            if (!descending)
            {
                return type.GetRuntimeFields()
                   .Where(x => x.IsStatic)
                   .Select(x => new KeyValuePair<string, TValue>(x.Name, (TValue)x.GetValue(null)))
                   .OrderBy(comparerKeySelector)
                   .ToDictionary(x => x.Key, x => x.Value);
            }
            return type.GetRuntimeFields()
                .Where(x => x.IsStatic)
                .Select(x => new KeyValuePair<string, TValue>(x.Name, (TValue)x.GetValue(null)))
                .OrderByDescending(comparerKeySelector)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.GetTypeInfo().IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.GetTypeInfo().BaseType;
            }
            return false;
        }

    }

}