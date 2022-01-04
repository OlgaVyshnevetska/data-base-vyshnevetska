using static System.Console;
using System.Collections.Generic;
using Npgsql;
using System;


public class Course
{
    public long id;
    public string name;

    public Course()
    {
        this.id = 0;
        this.name = "";
    }

    public Course(string name)
    {
        this.name = name;
    }

    public override string ToString()
    {
        return $"[{id}] {name}";
    }
}

public class Student
{
    public long id;
    public long course;
    public string name;  
    public string surname;

    public Student()
    {
        this.id = 0;
        this.course = 0;
        this.name = "";
        this.surname = "";
    } 

    public Student(long course, string name, string surname)
    {
        this.course = course;
        this.name = name;
        this.surname = surname;
    }

    public override string ToString()
    {
        return $"[{id}] {name} {surname}";
    }
}

public class Lecture
{
    public long id;
    public long course;
    public string topic;
    public long duration;
    public string text;

    public Lecture()
    {
        this.id = 0;
        this.course = 0;
        this.topic = "";
        this.duration = 0;
        this.text = "";
    } 

    public Lecture(long course, string topic, long duration, string text)
    {
        this.course = course;
        this.topic = topic;
        this.duration = duration;
        this.text = text;
    }

    public override string ToString()
    {
        return $"[{id}] [{course}] {topic} - {duration} \n{text}";
    }
}

public class CourseRep
{
    private NpgsqlConnection connection;

    public CourseRep(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public long Insert(Course course)//+
    {
        connection.Open();

        var sql =
        @"INSERT INTO courses (name) 
        VALUES (@name);";
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@name", course.name);
        long lastId = command.ExecuteNonQuery();

        connection.Close();

        return lastId;
    }

    public bool DeleteById(long id)//++
    {
        connection.Open();

        var sql = @"DELETE FROM courses WHERE id = @id";
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        int nChanged = command.ExecuteNonQuery();
        connection.Close();
        if (nChanged == 0)
        {
            return false;
        }

        return true;
    }


    public bool Update(long id, Course course)//+
    {
        connection.Open();

        var sql = @"UPDATE courses SET name = @name WHERE id = @id";
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", course.name);
        int rowChange = command.ExecuteNonQuery();
        connection.Close();
        if (rowChange == 0)
        {
            return false;
        }

        return true;
    }

    public Course GetCourse(NpgsqlDataReader reader)//+
    {
        Course course = new Course();
        course.id = reader.GetInt32(0);
        course.name = reader.GetString(1);

        return course;
    }
    
}

public class StudentRep
{
    private NpgsqlConnection connection;
    
    public StudentRep(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public long Insert(Student student)//+
    {
        connection.Open();
        var sql =
        @"INSERT INTO students (course, name, surname) 
        VALUES (@course, @name, @surname);";

        using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("@course", student.course);
        command.Parameters.AddWithValue("@name", student.name);
        command.Parameters.AddWithValue("@surname", student.surname);

        long lastId =  command.ExecuteNonQuery();
        connection.Close();

        return lastId;
    }

    public bool DeleteById(long id)//+
    {
        connection.Open();

        var sql = @"DELETE FROM students WHERE id = @id";
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        int nChanged = command.ExecuteNonQuery();
        connection.Close();
        if (nChanged == 0)
        {
            return false;
        }

        return true;
    }

    public bool Update(long id, Student student)//+
    {
        connection.Open();

        var sql = @"UPDATE students SET course = @course, name = @name, surname = @surname  WHERE id = @id";
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", student.id);
        command.Parameters.AddWithValue("@course", student.course);
        command.Parameters.AddWithValue("@name", student.name);
        command.Parameters.AddWithValue("@surname", student.surname);
        int rowChange = command.ExecuteNonQuery();
        connection.Close();
        if (rowChange == 0)
        {
            return false;
        }

        return true;
    }

    public Student GetStudent(NpgsqlDataReader reader)//+
    {
        Student student = new Student();
        student.id = reader.GetInt32(0);
        student.course = reader.GetInt32(1);
        student.name = reader.GetString(2);
        student.surname = reader.GetString(3);
        return student;
    }
}

public class LectureRep
{
    private NpgsqlConnection connection;

    public LectureRep(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public long Insert(Lecture lecture)//+
    {
        connection.Open();
        var sql =
        @"INSERT INTO lectures (course, topic, duration, text) 
        VALUES (@course, @topic, @duration, @text);";
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@course", lecture.course);
        command.Parameters.AddWithValue("@topic", lecture.topic);
        command.Parameters.AddWithValue("@duration", lecture.duration);
        command.Parameters.AddWithValue("@text", lecture.text);
        long lastId = command.ExecuteNonQuery();
        connection.Close();

        return lastId;
    }

    public Lecture GetLecture(NpgsqlDataReader reader)//+
    {
        Lecture lecture = new Lecture();
        lecture.id = reader.GetInt32(0);
        lecture.course = reader.GetInt32(1);
        lecture.topic = reader.GetString(2);
        lecture.duration = reader.GetInt32(3);
        lecture.text = reader.GetString(4);

        return lecture;
    }

    public bool DeleteById(long id)//+
    {
        connection.Open();

        var sql = @"DELETE FROM lectures WHERE id = @id";
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        int nChanged = command.ExecuteNonQuery();
        connection.Close();

        if (nChanged == 1)
        {
            return true;
        }
        return false;
    }

    public bool Update(long id, Lecture lecture)//+
    {
        connection.Open();

        var sql = @"UPDATE lectures SET course = @course, topic = @topic, duration = @duration, text = @text WHERE id = @id";
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@course", lecture.course);
        command.Parameters.AddWithValue("@topic", lecture.topic);
        command.Parameters.AddWithValue("@duration", lecture.duration);
        command.Parameters.AddWithValue("@text", lecture.text);
        int nChanged = command.ExecuteNonQuery();
        connection.Close();
        if (nChanged == 1)
        {
            return true;
        }
        return false;
    }
}

public class GenerateData
{
    public void GenerateCourse(int number, NpgsqlConnection connection)//+
    {
        string[] names = new string[] {"Maths", "Analitics", "English", "IT", "Statistics"};
        Random random = new Random();
        CourseRep coursesRep = new CourseRep(connection);
        for (int i = 0; i < number; i++)
        {
            Course course = new Course();
            course.name = names[random.Next(0, names.Length - 1)];
            coursesRep.Insert(course);
        }

    }
}
