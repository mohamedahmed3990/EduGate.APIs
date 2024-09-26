using EduGate.Core.Entities;
using EduGate.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EduGate.Repositroy.Identity
{
    public static class AppDbContextSeed
    {
       
        public async static Task SeedAsync(AppDbContext dbContext)
        {
            if(dbContext.Set<Course>().Count() == 0)
            {
                var coursesData = File.ReadAllText("../EduGate.Repositroy/Data/DataSeed/courses.json");

            var courses = JsonSerializer.Deserialize<List<Course>>(coursesData);

            if(courses?.Count() > 0)
            {
                foreach (var course in courses)
                {
                    dbContext.Set<Course>().Add(course);
                }
                await dbContext.SaveChangesAsync();
            }
            }
        }

    }
}
