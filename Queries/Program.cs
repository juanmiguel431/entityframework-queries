
using System;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new PlutoContext())
            {
                //Extension methods syntax
                var courses = context.Courses
                    .Where(c => c.Name.Contains("C#"))
                    .OrderBy(c => c.Name)
                    .ToList();

                foreach (var course in courses) 
                    Console.WriteLine(course.Name);

                //LINQ Syntax
                var coursesQuery = from c in context.Courses
                    where c.Name.Contains("C#")
                    orderby c.Name
                    select c;

                foreach (var course in coursesQuery) 
                    Console.WriteLine(course.Name);
            }
        }
    }
}
