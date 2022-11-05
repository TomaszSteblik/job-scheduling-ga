using System;

namespace Scheduling.Models;

public class Machine : IEquatable<Machine>
{
    public string Name { get; set; }
    public Qualification RequiredQualification { get; set; }

    public bool Equals(Machine? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Machine) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public static bool operator ==(Machine? left, Machine? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Machine? left, Machine? right)
    {
        return !Equals(left, right);
    }
}