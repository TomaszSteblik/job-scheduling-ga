using System.Data;
using System.Globalization;
using CsvHelper;
using GeneticAlgorithm.Models;

namespace ConsoleRunner;

public static class DataReaderCsvHelper
{
    internal static Person[] GetPeopleFromCsv(string? parametersDataPathPersonel)
    {
        var people = new List<Person>();
        if (parametersDataPathPersonel is null)
            throw new ArgumentNullException(nameof(parametersDataPathPersonel),"Null path to personel");
        using (var reader = new StreamReader(parametersDataPathPersonel))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<PersonelHelper>();
            var enumerable = records.ToList();
            for (var index = 0; index < enumerable.Count; index++)
            {
                var record = enumerable[index];
                if (record.Qualifications is null)
                    throw new DataException(nameof(record));
                var person = new Person
                {
                    Id = index,
                    Name = record.Name,
                    PreferenceDays = record.DaysPreference,
                    PreferredMachineId = record.PreferredMachineId,
                    Qualifications = new List<Qualification>()
                };

                var qualifications = record.Qualifications.Split('-');
                foreach (var qualification in qualifications)
                {
                    person.Qualifications.Add(Enum.Parse<Qualification>(qualification));
                }
                people.Add(person);
            }
        }

        return people.ToArray();
    }

    internal static Machine[] GetMachinesFromCsv(string? parametersDataPathMachines)
    {
        if (parametersDataPathMachines is null)
            throw new ArgumentNullException(nameof(parametersDataPathMachines), "Null path to machines");
        using var reader = new StreamReader(parametersDataPathMachines);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Machine>();
        return records.ToArray();
    }
}