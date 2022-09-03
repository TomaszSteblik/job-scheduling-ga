using System.Runtime.Serialization;

namespace Data.Exceptions;

[Serializable]
public class ItemNotFoundException : Exception
{
    public ItemNotFoundException() : base("Item not found in database.") { }
    
    public ItemNotFoundException(string name, int id) : 
        base($"{name} not found in database with id: {id}") { }

    protected ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}