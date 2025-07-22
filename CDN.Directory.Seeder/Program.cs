using System.Globalization;
using CsvHelper;
using CDN.Directory.Infrastructure.Data;
using CDN.Directory.Core.Entities;
using Microsoft.EntityFrameworkCore;

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseMySql(
"server=localhost;port=3306;database=cdn_directory_db;user=root;password=royalvictoria03!",
ServerVersion.AutoDetect("server=localhost;port=3306;database=cdn_directory_db;user=root;password=royalvictoria03!")
);

using var context = new AppDbContext(optionsBuilder.Options);

SeedSkillsets(@"C:\Users\Alfateh Yusof\source\repos\CDN.Directory\CDN.Directory.Seeder\Dataset\skillsets.csv", context);
SeedHobbies(@"C:\Users\Alfateh Yusof\source\repos\CDN.Directory\CDN.Directory.Seeder\Dataset\hobbies.csv", context);

void SeedSkillsets(string csvFile, AppDbContext context)
{
    using var reader = new StreamReader(csvFile);
    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    var records = csv.GetRecords<SimpleName>().DistinctBy(r => r.Name.ToLower()).ToList();

    
foreach (var record in records)
    {
        if (!context.Skillsets.Any(s => s.Name.ToLower() == record.Name.ToLower()))
        {
            context.Skillsets.Add(new SkillsetMaster { Name = record.Name });
        }
    }
    context.SaveChanges();
}

void SeedHobbies(string csvFile, AppDbContext context)
{
    using var reader = new StreamReader(csvFile);
    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    var records = csv.GetRecords<SimpleName>().DistinctBy(r => r.Name.ToLower()).ToList();

    
foreach (var record in records)
    {
        if (!context.Hobbies.Any(h => h.Name.ToLower() == record.Name.ToLower()))
        {
            context.Hobbies.Add(new HobbyMaster { Name = record.Name });
        }
    }
    context.SaveChanges();
}

public class SimpleName
{
    public string Name { get; set; } = string.Empty;
}