namespace Data.Entities;

internal class Day
{
    public int Id { get; set; }
    public int DayOfSchedule { get; set; }
    public ICollection<Person> People { get; set; }
}