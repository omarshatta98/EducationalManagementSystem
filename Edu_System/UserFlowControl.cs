using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Edu_System
{
    class UserFlowControl
    {
        public static void Run()
        {
            Console.WriteLine("Please make a choice : ");
            Console.WriteLine("\t1) Load System");
            Console.WriteLine("\t2) New System");
            Console.Write("\tEnter a Choice : ");
            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine("---------------------------");
            if (choice == 1)
                Load();
            else if (choice == 2)
                LogOut();
            else
            {
                Console.WriteLine("Error!! Invalid Choice try again ....");
                Console.WriteLine("---------------------------------");
                Run();
            }
        }
        public static void Load()
        {
            // Student data...
            string stdFile = "StudentData.txt";
            StreamReader srStudent = new StreamReader(stdFile);
            string stdData = srStudent.ReadLine();
            while (stdData != null)
            {
                int ID = 1000;
                string[] data = stdData.Split(',');
                Student std = new Student();
                std.id = ID++;
                std.username = data[0];
                std.password = data[1];
                std.email = data[2];
                std.name = data[3];
                Student.studentInSystem.Add(std);
                stdData = srStudent.ReadLine();
            }

            // Doctor data...
            string drFile = "DoctorData.txt";
            StreamReader srDoctor = new StreamReader(drFile);
            string drData = srDoctor.ReadLine();
            while (drData != null)
            {
                string[] data = drData.Split(',');
                Doctor dr = new Doctor();
                dr.username = data[0];
                dr.password = data[1];
                dr.email = data[2];
                dr.name = data[3];
                Doctor.doctorInSystem.Add(dr);
                drData = srDoctor.ReadLine();
            }
            LogOut();
        }
        public static void LogOut()
        {
            Console.WriteLine("Please make a choice :");
            Console.WriteLine("\t1) Login");
            Console.WriteLine("\t2) SignUp");
            Console.WriteLine("\t3) Shutdown ");
            Console.Write("\tEnter a choice : ");
            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine("--------------------------");
            if (choice == 1)
                DoLogin();
            else if (choice == 2)
                DoSignUp();
            else if (choice == 3)
                Console.ReadKey();
            else
            {
                Console.WriteLine("Error !! Invalid choice try again ....");
                Console.WriteLine("---------------------------");
                LogOut();
            }
        }
        public static void DoLogin()
        {
            Console.Write("Username : ");
            string username = Console.ReadLine();
            Console.Write("Password : ");
            string password = Console.ReadLine();
            Console.WriteLine("------------------------------------");

            if (Student.IsValidUser(username, password) != null)
                Student.GetUser(Student.IsValidUser(username, password));
            else if (Doctor.IsValidUser(username, password) != null)
                Doctor.GetUser(Doctor.IsValidUser(username, password));
            else
            {
                Console.WriteLine("Error !!  invalid login try again ....\n--------------------");
                LogOut();
            }
        }
        public static void DoSignUp()
        {
            Console.WriteLine("Please make a choice : ");
            Console.WriteLine("\t1) Doctor");
            Console.WriteLine("\t2) Student");
            Console.Write("\tEnter choice : ");
            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine("-------------------------------");
            if (choice == 1)
                Doctor.SignUp();
            else if (choice == 2)
                Student.SignUp();
            else
            { Console.WriteLine("Error !! invalid choice try again ....\n-------------------"); DoSignUp(); }
        }
        public static string ReadPassword()
        {
            string pass = "";
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (pass.Length > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        pass = pass.Substring(0, pass.Length - 1);
                    }
                    else
                    {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }
                }
                else
                {
                    Console.Write("\b \b");
                    pass += key.KeyChar;
                    Console.Write("*");
                }
            }
            return pass;
        }
        public static Student GetUserByEmail(string e)
        {
            foreach (var student in Student.studentInSystem)
            {
                if (student.email == e)
                {
                    return student;
                }
            }
            return null;
        }
    }
}
