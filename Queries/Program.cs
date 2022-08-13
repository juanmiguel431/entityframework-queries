using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                var coursesq = context.Courses.Include(c => c.Author)
                    .Where(c => c.Name.Contains("C#") && c.Author.Id == 4)
                    .OrderBy(c => c.Name);

                var courses = coursesq.ToList();

                // foreach (var course in courses) 
                //     Console.WriteLine(course.Name);

                //LINQ Syntax
                var coursesQuery = from c in context.Courses.Include(c => c.Author)
                    where c.Name.Contains("C#")
                    orderby c.Name
                    select c;

                // foreach (var course in coursesQuery) 
                //     Console.WriteLine(course.Name);

                //Grouping
                var groupQuery = from c in context.Courses
                    group c by c.Level
                    into g
                    select g;

                var groups = groupQuery.ToList();

                // foreach (IGrouping<int, Course> group in groups)
                // {
                //     Console.WriteLine(group.Key);
                //     Console.WriteLine($"Count: {group.Count()}");
                //     foreach (var course in group)
                //     {
                //         Console.WriteLine($"\t{course.Name}");
                //     }
                // }

                //Navigation properties
                var q1 = from c in context.Courses
                    select new
                    {
                        Name = c.Name,
                        AuthorName = c.Author.Name
                    };

                var l1 = q1.ToList();

                // foreach (var i1 in l1)
                // {
                //     Console.WriteLine($"Course: {i1.Name} \t Author: {i1.AuthorName}");
                // }

                //Join
                var q2 = from c in context.Courses
                    join a in context.Authors on c.AuthorId equals a.Id
                    select new
                    {
                        Name = c.Name,
                        AuthorName = a.Name
                    };

                var l2 = q2.ToList();

                // foreach (var i2 in l2)
                // {
                //     Console.WriteLine($"Course: {i2.Name} \t Author: {i2.AuthorName}");
                // }


                var q3 = from a in context.Authors
                    join c in context.Courses on a.Id equals c.AuthorId into g
                    select new
                    {
                        AuthorName = a.Name,
                        Courses = g
                    };

                var l3 = q3.ToList();

                // foreach (var i3 in l3)
                // {
                //     Console.WriteLine($"AuthorName: {i3.AuthorName} \t Count: {i3.Courses.Count()}");
                // }

                // var coursesQ = context.Courses;

                //Cross join
                var q4 = from a1 in context.Authors
                    from c2 in context.Courses
                    select new
                    {
                        AuthorName = a1.Name,
                        CourseName = c2.Name
                    };

                var l4 = q4.ToList();

                // foreach (var i4 in l4)
                // {
                //     Console.WriteLine($"Author: {i4.AuthorName} \t Course: {i4.CourseName}");
                // }


                //Extension methods
                var q5 = context.Courses
                    .Where(c => c.Level == 1)
                    .OrderBy(c => c.Name)
                    .ThenByDescending(c => c.Level)
                    .Select(c => new
                    {
                        CourseName = c.Name,
                        AuthorName = c.Author.Name
                    });

                //Part 1 - List of list
                var q6 = context.Courses
                    .Where(c => c.Level == 1)
                    .OrderBy(c => c.Name)
                    .ThenByDescending(c => c.Level)
                    .Select(c => c.Tags);

                var l6 = q6.ToList();

                // foreach (var tags in l6)
                // {
                //     foreach (var tag in tags) 
                //         Console.WriteLine(tag.Name);
                // }

                Console.WriteLine("/////////");
                //Part 2 - One list - better - Flatter
                var q7 = context.Courses
                    .Where(c => c.Level == 1)
                    .SelectMany(c => c.Tags)
                    .OrderBy(c => c.Name)
                    .Distinct();

                var l7 = q7.ToList();

                // foreach (var tag in l7) 
                //     Console.WriteLine(tag.Name);


                var q8 = context.Courses.GroupBy(p => p.Level);
                var l8 = q8.ToList();

                // foreach (var group in l8)
                // {
                //     Console.WriteLine($"Level {group.Key}");
                //
                //     foreach (var course in group)
                //     {
                //         Console.WriteLine($"Course {course.Name}");
                //     }
                // }

                //Grouping
                var q9 = context.Courses.Join(context.Authors, c => c.AuthorId, a => a.Id,
                    (c, a) =>
                        new
                        {
                            Course = c.Name,
                            AuthorName = a.Name
                        }
                );

                var l9 = q9.ToList();

                // foreach (var i9 in l9)
                // {
                //     Console.WriteLine($"Course: {i9.Course} AuthorName: {i9.AuthorName}");
                // }
                
                //Group Join
                var q10 = context.Authors.GroupJoin(context.Courses,
                    a => a.Id,
                    c => c.AuthorId,
                    (author, courseList) => new
                    {
                        AuthorName = author.Name,
                        CoursesCount = courseList.Count()
                    });

                var l10 = q10.ToList();

                // foreach (var i10 in l10)
                // {
                //     Console.WriteLine($"Author: {i10.AuthorName} Count: {i10.CoursesCount}");
                // }
                
                
                //Cross Join
                var q11 = context.Authors.SelectMany(
                    a => context.Courses,
                    (author, course) => new
                    {
                        Author = author.Name,
                        course = course.Name
                    });

                var l11 = q11.ToList();
                
                // foreach (var i11 in l11)
                // {
                //     Console.WriteLine($"Author: {i11.Author} Course: {i11.course}");
                // }
                
                //Pagination / Partitioning.
                var pageNumber = 2;
                var pageSize = 3;

                var skip = (pageNumber - 1) * pageSize;
                var secondPageQ = context.Courses.OrderBy(p => p.Id).Skip(skip).Take(pageSize);
                var secondPage = secondPageQ.ToList();

                foreach (var course in secondPage)
                {
                    Console.WriteLine(course.Name);
                }


                var i12 = context.Courses.OrderBy(c => c.Level).FirstOrDefault(c => c.FullPrice > 100);


                IEnumerable<Course> x = context.Courses;

                x.Where(c =>
                {
                    var jmpc = "";
                    return c.Level == 1;
                });

            }
        }
    }
}