using System;
using System.Collections.Generic;

namespace Scheduling.Models;

public class Worker : IEquatable<Worker>
{
    public bool Equals(Worker? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FirstName == other.FirstName && LastName == other.LastName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Worker) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName);
    }

    public static bool operator ==(Worker? left, Worker? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Worker? left, Worker? right)
    {
        return !Equals(left, right);
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PreferenceDaysCount { get; set; }

    public ICollection<int>? PreferredDays { get; set; }
    public ICollection<Machine>? PreferredMachines { get; set; }
    public ICollection<Qualification> Qualifications { get; set; }
    
}