using static System.Console;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

public class View
{
    public void SendHelp()
    {
        WriteLine("Your possible options are: insert, update or delete.");
    }

    public void SendError()
    {
        WriteLine("Error! You didn't enter the correct value!");
    }

    public void PrintInsertStudent(long id)
    {
        WriteLine("Created new student with id: {0}", id);
    }
    public void PrintInsertCourse(long id)
    {
        WriteLine("Created new course with id: {0}", id);
    }
    public void PrintInsertLecture(long id)
    {
        WriteLine("Created new lecture with id: {0}", id);
    }

    public void PrintUpdateStudent(bool check)
    {
        if(check)
        {
            WriteLine("Student was successfully updated!");
        }
        else
        {
            WriteLine("Student couldn't update.");
        }
    }
    public void PrintUpdateCourse(bool check)
    {
        if(check)
        {
            WriteLine("Course was successfully updated!");
        }
        else
        {
            WriteLine("Course couldn't update.");
        }
    }
    public void PrintUpdateLecture(bool check)
    {
        if(check)
        {
            WriteLine("Lecture was successfully updated!");
        }
        else
        {
            WriteLine("Lecture couldn't update.");
        }
    }

    public void PrintDeleteStudent(bool check)
    {
        if(check)
        {
            WriteLine("Student was successfully deleted!");
        }
        else
        {
            WriteLine("Student wasn't deleted.");
        }
    }
    public void PrintDeleteCourse(bool check)
    {
        if(check)
        {
            WriteLine("Course was successfully deleted!");
        }
        else
        {
            WriteLine("Course wasn't deleted.");
        }
    }
    public void PrintDeleteLecture(bool check)
    {
        if(check)
        {
            WriteLine("Lecture was successfully deleted!");
        }
        else
        {
            WriteLine("Lecture wasn't deleted.");
        }
    }
}