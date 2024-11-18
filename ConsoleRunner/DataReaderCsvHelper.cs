using System.Data;
using System.Globalization;
using CsvHelper;
using SchedulingAlgorithmModels.Models;

namespace ConsoleRunner;

public static class DataReaderCsvHelper
{
    internal static Person[] GetPeopleFromCsv(string? parametersDataPathPersonel)
    {
        var people = new List<Person>();
        if (parametersDataPathPersonel is null)
            throw new ArgumentNullException(nameof(parametersDataPathPersonel), "Null path to personel");
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

                if (record.PreferredMachineIds is null)
                    throw new DataException(nameof(reader));

                if (record.PreferredDays is null)
                    throw new DataException(nameof(reader));

                var person = new Person
                {
                    Id = index,
                    Name = record.Name,
                    PreferenceDaysCount = record.DaysPreferenceCount,
                    PreferredMachineIds = new List<int>(),
                    Qualifications = new List<string>(),
                    PreferredDays = new List<int>()
                };

                var qualifications = record.Qualifications.Split('-');
                foreach (var qualification in qualifications)
                {
                    person.Qualifications.Add(qualification);
                }

                var machines = record.PreferredMachineIds.Split('-');
                foreach (var machineId in machines.Select(int.Parse))
                {
                    person.PreferredMachineIds.Add(machineId);
                }

                var days = record.PreferredDays.Split('-');
                foreach (var dayId in days.Select(int.Parse))
                {
                    person.PreferredDays.Add(dayId);
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