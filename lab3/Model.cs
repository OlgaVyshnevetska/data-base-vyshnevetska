using static System.Console;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

public class DatabaseContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Lecture> Lectures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Server=localhost; Port=5432; User Id=postgres; Password=123456; Database=db3");
}

public class Course
{
    public long CourseId { get; set; }
    public string Name { get; set; }
    public ICollection<Student> Students { get; set; }
    public ICollection<Lecture> Lectures { get; set; }

    public Course()
    {
        this.CourseId = 0;
        this.Name = "";
    }

    public Course(string name)
    {
        this.Name = name;
    }

    public override string ToString()
    {
        return $"[{CourseId}] {Name}";
    }
}

public class Student 
{
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public Student()
    {
        this.StudentId = 0;
        this.CourseId = 0;
        this.Name = "";
        this.Surname = "";
    } 

    public Student(long course, string name, string surname)
    {
        this.CourseId = course;
        this.Name = name;
        this.Surname = surname;
    }

    public override string ToString()
    {
        return $"[{StudentId}] {Name} {Surname}";
    }
}

public class Lecture
{
    public long LectureId { get; set; }
    public long CourseId { get; set; }
    public string Topic { get; set; }
    public long Duration { get; set; }
    public string Text { get; set; }

    public Lecture()
    {
        this.LectureId = 0;
        this.CourseId = 0;
        this.Topic = "";
        this.Duration = 0;
        this.Text = "";
    } 

    public Lecture(long course, string topic, long duration, string text)
    {
        this.CourseId = course;
        this.Topic = topic;
        this.Duration = duration;
        this.Text = text;
    }

    public override string ToString()
    {
        return $"[{LectureId}] [{CourseId}] {Topic} - {Duration} \n{Text}";
    }
}

public class CourseRep
{
    public long Insert(Course course)//+
    {
        using (var db = new DatabaseContext())
        {
            db.Courses.Add(course);
            db.SaveChanges();
        }
        return course.CourseId;
    }

    public bool DeleteById(long id)//++
    {
        using(var db = new DatabaseContext()) 
        {
            Course course = new Course();
            foreach(var c in db.Courses)
            {
                if(c.CourseId == id)
                {
                    course = c;
                    break;
                }
            }
            db.Courses.Remove(course);
            db.SaveChanges();
        }
        return true;
    }

    public bool Update(long id, Course course)//+
    {
        using(var db = new DatabaseContext()) 
        {
            Course temp = new Course();
            foreach(var c in db.Courses)
            {
                if(c.CourseId == id)
                {
                    temp = c;
                    break;
                }
            }
            temp = course;
            db.SaveChanges();
        }
        return true;
    }    
}

public class StudentRep
{
    public long Insert(Student student)//+
    {
        using (var db = new DatabaseContext())
        {
            db.Students.Add(student);
            db.SaveChanges();
        }
        return student.StudentId;
    }

    public bool DeleteById(long id)//+
    {
        using(var db = new DatabaseContext()) 
        {
            Student student = new Student();
            foreach(var s in db.Students)
            {
                if(s.StudentId == id)
                {
                    student = s;
                    break;
                }
            }
            db.Students.Remove(student);
            db.SaveChanges();
        }
        return true;
    }

    public bool Update(long id, Student student)//+
    {
        using(var db = new DatabaseContext()) 
        {
            Student temp = new Student();
            foreach(var s in db.Students)
            {
                if(s.StudentId == id)
                {
                    temp = s;
                    break;
                }
            }
            temp = student;
            db.SaveChanges();
        }
        return true;
    }
}

public class LectureRep
{
    public long Insert(Lecture lecture)//+
    {
        using (var db = new DatabaseContext())
        {
            db.Lectures.Add(lecture);
            db.SaveChanges();
        }
        return lecture.LectureId;
    }

    public bool DeleteById(long id)//+
    {
        using(var db = new DatabaseContext()) 
        {
            Lecture lecture = new Lecture();
            foreach(var l in db.Lectures)
            {
                if(l.LectureId == id)
                {
                    lecture = l;
                    break;
                }
            }
            db.Lectures.Remove(lecture);
            db.SaveChanges();
        }
        return true;
    }

    public bool Update(long id, Lecture lecture)//+
    {
        using(var db = new DatabaseContext()) 
        {
            Lecture temp = new Lecture();
            foreach(var l in db.Lectures)
            {
                if(l.LectureId == id)
                {
                    temp = l;
                    break;
                }
            }
            temp = lecture;
            db.SaveChanges();
        }
        return true;
    }
}