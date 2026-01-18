using System.Reflection;
using System.Runtime.Serialization;
using ZedLive.Domain.ValueObjects.Base;

namespace ZedLive.Domain.ValueObjects;

public abstract class ValueObjects : IEquatable<ValueObjects>
{
    private List<PropertyInfo> _properties;
    private List<FieldInfo> _fields;

    public static bool operator ==(ValueObjects obj1, ValueObjects obj2)
    {
        return obj1?.Equals(obj2) ?? Equals(obj2, null);
    }

    public static bool operator !=(ValueObjects obj1, ValueObjects obj2)
    {
        return !(obj1 == obj2);
    }

    public bool Equals(ValueObjects? obj)
    {
        return Equals(obj as object);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ValueObjects)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            foreach (var prop in GetProperties())
            {
                var value = prop.GetValue(this, null);
                hash = HashValue(hash, value);
            }

            foreach (var prop in GetFields())
            {
                var value = prop.GetValue(this);
                hash = HashValue(hash, value);
            }

            return hash;
        }
    }

    private bool PropertiesAreEqual(object obj, PropertyInfo p)
    {
        return object.Equals(p.GetValue(this, null), p.GetValue(obj, null));
    }

    private bool FieldsAreEqual(object obj, FieldInfo f)
    {
        return object.Equals(f.GetValue(this), f.GetValue(obj));
    }

    private IEnumerable<PropertyInfo> GetProperties()
    {
        _properties ??= GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.GetCustomAttribute<IgnoreDataMemberAttribute>() == null)
            .ToList();

        return _properties;
    }

    private IEnumerable<FieldInfo> GetFields()
    {
        _fields ??= GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(p => p.GetCustomAttribute<IgnoreDataMemberAttribute>() == null)
            .ToList();

        return _fields;
    }

    private int HashValue(int seed, object value)
    {
        var currentHash = value?.GetHashCode() ?? 0;
        return (seed * 23) + currentHash;
    }
}