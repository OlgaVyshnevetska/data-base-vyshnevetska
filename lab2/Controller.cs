using static System.Console;
using System.Collections.Generic;
using Npgsql;
using System;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            string db_path = "Server=localhost; Port=5432; User Id=postgres; Password=123456; Database=db"; //@"/home/katrin/Рабочий стол/КПИ/progbase3/Progbase3.sln/ConsoleProject/data/Database.db";
            using var connection = new NpgsqlConnection(db_path);
            
            CourseRep courserep = new CourseRep(connection);
            LectureRep lecturerep = new LectureRep(connection);
            StudentRep studentrep = new StudentRep(connection);

            string command;
            string type;
            do
            {
                WriteLine("What do you want to do?: \nEnter help to see possible options.");
                command = ReadLine();
                View v = new View();
                if (command == "insert")
                {
                    WriteLine("Choose and type in: student, course or lecture");
                    type = ReadLine();
                    ProcessInsert prin = new ProcessInsert();
                    switch(type)
                    {
                        case "student": prin.ProcessInsertStudent(studentrep); break;
                        case "course": prin.ProcessInsertCourse(courserep); break;
                        case "lecture": prin.ProcessInsertLecture(lecturerep); break;
                        default: v.SendError(); break;
                    }
                }
                else if (command == "get")
                {
                    WriteLine("Choose and type in: student, course or lecture");
                    type = ReadLine();
                    NpgsqlCommand cmd = connection.CreateCommand();
                    connection.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    ProcessGet prg = new ProcessGet();
                    while (reader.Read())
                    {
                        switch(type)
                        {
                            case "student": prg.ProcessGetStudent(studentrep, reader); break;
                            case "course": prg.ProcessGetCourse(courserep, reader); break;
                            case "lecture": prg.ProcessGetLecture(lecturerep, reader); break;
                            default: v.SendError(); break;
                        }
                    }
                }
                else if (command == "update")
                {
                    WriteLine("Choose and type in: student, course or lecture");
                    type = ReadLine();
                    ProcessUpdate prup = new ProcessUpdate();
                    switch(type)
                    {
                        case "student": prup.ProcessUpdateStudent(studentrep); break;
                        case "course": prup.ProcessUpdateCourse(courserep); break;
                        case "lecture": prup.ProcessUpdateLecture(lecturerep); break;
                        default: v.SendError(); break;
                    }
                }
                else if (command == "delete")
                {
                    WriteLine("Choose and type in: student, course or lecture");
                    type = ReadLine();
                    ProcessDelete prdel = new ProcessDelete();
                    switch(type)
                    {
                        case "student": prdel.ProcessDeleteStudent(studentrep); break;
                        case "course": prdel.ProcessDeleteCourse(courserep); break;
                        case "lecture": prdel.ProcessDeleteLecture(lecturerep); break;
                        default: v.SendError(); break;
                    }
                }
                else if (command == "generate")
                {
                    ProcessGeneration pg = new ProcessGeneration(); 
                    pg.ProcessGenerate(connection);
                }
                else if(command == "help")
                {
                    v.SendHelp();
                }
                else if (command != "quit")
                {
                    WriteLine("{0} not found. Try another command.", command);
                }
            
            } while(command !=  "quit");
            WriteLine("Bye! :)");
        }
    }

    public class ProcessGeneration
    {
        public void ProcessGenerate(NpgsqlConnection connection)
        {
            WriteLine("How many do you want to generate? Enter n:");
            string temp = ReadLine();
            if(int.TryParse(temp, out int n))
            {
                GenerateData g = new GenerateData();
                g.GenerateCourse(n, connection);
            }
            else
            {
                View v = new View();
                v.SendError();
            }
        }
    }

    public class ProcessInsert
    {
        public void ProcessInsertStudent(StudentRep studentrep)
        {
            WriteLine("Type in the id of students course:");
            string temp = ReadLine();
            View v = new View(); 
            if(!(Int64.TryParse(temp, out long course)))
            {
                v.SendError();
            }
            else
            {
                WriteLine("Type in students name:");
                string name = ReadLine();
                WriteLine("Type in students surname: ");
                string surname = ReadLine();

                Student st = new Student(course, name, surname);
                long newId = studentrep.Insert(st);
                v.PrintInsertStudent(newId);
            }
        }

        public void ProcessInsertCourse(CourseRep courserep)
        {
            WriteLine("Type in name of the course:");
            string name = ReadLine();

            Course c = new Course(name);
            long newId = courserep.Insert(c);
            View v = new View();
            v.PrintInsertCourse(newId);
        }

        public void ProcessInsertLecture(LectureRep lecturerep)
        {
            WriteLine("Type in the id of course the lecture will be for:");
            string temp = ReadLine();
            View v = new View(); 
            if(!(Int64.TryParse(temp, out long course)))
            {
                v.SendError();
            }
            else
            {
                WriteLine("Type in topic of the lecture:");
                string topic = ReadLine();
                WriteLine("Type in duration of the lecture:");
                temp = ReadLine();
                if(!(Int64.TryParse(temp, out long duration)))
                {
                    v.SendError();
                }
                else
                {
                    WriteLine("Type in lectures text: ");
                    string text = ReadLine();

                    Lecture l = new Lecture(course, topic, duration, text);
                    long newId = lecturerep.Insert(l);
                    v.PrintInsertLecture(newId);
                }
            }
        }
    }

    public class ProcessGet 
    {
        public void ProcessGetStudent(StudentRep studentrep, NpgsqlDataReader reader)
        {
            Student st = studentrep.GetStudent(reader);
            View v = new View();
            v.PrintGetStudent(st);
        }
        public void ProcessGetCourse(CourseRep courserep, NpgsqlDataReader reader)
        {
            Course c = courserep.GetCourse(reader);
            View v = new View();
            v.PrintGetCourse(c);
        }
        public void ProcessGetLecture(LectureRep lecturerep, NpgsqlDataReader reader)
        {
            Lecture l = lecturerep.GetLecture(reader);
            View v = new View();
            v.PrintGetLecture(l);
        }
    }

    public class ProcessUpdate
    {
        public void ProcessUpdateStudent(StudentRep studentrep)
        {
            WriteLine("Type in the id of student you want to update:");
            string temp = ReadLine();
            View v = new View(); 
            if(!(Int64.TryParse(temp, out long id)))
            {
                v.SendError();
            }
            else
            {
                WriteLine("Type in new id of students course:");
                temp = ReadLine();
                if(!(Int64.TryParse(temp, out long course)))
                {
                    v.SendError();
                }
                else
                {
                    WriteLine("Type in new students name:");
                    string name = ReadLine();
                    WriteLine("Type in new students surname: ");
                    string surname = ReadLine();

                    Student st = new Student(course, name, surname);
                    bool check = studentrep.Update(id, st);
                    v.PrintUpdateStudent(check);
                }
            }
        }

        public void ProcessUpdateCourse(CourseRep courserep)
        {
            WriteLine("Type in the id of course you want to update:");
            string temp = ReadLine();
            View v = new View(); 
            if(!(Int64.TryParse(temp, out long id)))
            {
                v.SendError();
            }
            else
            {
                WriteLine("Type in new name of the course:");
                string name = ReadLine();

                Course c = new Course(name);
                bool check = courserep.Update(id, c);
                v.PrintUpdateCourse(check);
            }
        }

        public void ProcessUpdateLecture(LectureRep lecturerep)
        {
            WriteLine("Type in the id of lecture you want to update:");
            string temp = ReadLine();
            View v = new View(); 
            if(!(Int64.TryParse(temp, out long id)))
            {
                v.SendError();
            }
            else
            {
                WriteLine("Type in new id of course the lecture will be for:");
                temp = ReadLine();
                if(!(Int64.TryParse(temp, out long course)))
                {
                    v.SendError();
                }
                else
                {
                     WriteLine("Type in new topic of the lecture:");
                    string topic = ReadLine();
                    WriteLine("Type in new duration of the lecture:");
                    temp = ReadLine();
                    if(!(Int64.TryParse(temp, out long duration)))
                    {
                        v.SendError();
                    }
                    else
                    {
                        WriteLine("Type in new lectures text: ");
                        string text = ReadLine();

                        Lecture l = new Lecture(course, topic, duration, text);
                        bool check = lecturerep.Update(id, l);
                        v.PrintUpdateLecture(check);
                    }
                }
            }
        }
    }

    public class ProcessDelete
    {
        public void ProcessDeleteStudent(StudentRep studentrep)
        {
            WriteLine("Type in the id of student you want to delete:");
            string temp = ReadLine();
            View v = new View(); 
            if(!(Int64.TryParse(temp, out long id)))
            {
                v.SendError();
            }
            else
            {
                bool check = studentrep.DeleteById(id);
                v.PrintDeleteStudent(check);
            }
        }

        public void ProcessDeleteCourse(CourseRep courserep)
        {
            WriteLine("Type in the id of course you want to delete:");
            string temp = ReadLine();
            View v = new View(); 
            if(!(Int64.TryParse(temp, out long id)))
            {
                v.SendError();
            }
            else
            {
                bool check = courserep.DeleteById(id);
                v.PrintDeleteCourse(check);
            }
        }

        public void ProcessDeleteLecture(LectureRep lecturerep)
        {
            WriteLine("Type in the id of lecture you want to delete:");
            string temp = ReadLine();
            View v = new View(); 
            if(!(Int64.TryParse(temp, out long id)))
            {
                v.SendError();
            }
            else
            {
                bool check = lecturerep.DeleteById(id);
                v.PrintDeleteLecture(check);
            }
        }
    }
}