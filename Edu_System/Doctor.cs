using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu_System
{
    class Doctor
    {
        public string name;
        public string username;
        public string password;
        public string email;
        static Doctor currentDoctor;
        public List<Course> teachingCourse = new List<Course>();
        public static List<Doctor> doctorInSystem = new List<Doctor>();
        Course currentCourse;

        public static Doctor IsValidUser(string username, string password)
        {
            foreach (var doctor in Doctor.doctorInSystem)
            {
                if (doctor.username == username && doctor.password == password)
                    return doctor;
            }
            return null;
        }
        public static void GetUser(Doctor dr)
        {
            currentDoctor = dr;
            currentDoctor.Menu();
        }
        public static void SignUp()
        {
            Doctor dr = new Doctor();
            Console.Write("username : ");
            dr.username = Console.ReadLine();
            Console.Write("password : ");
            dr.password = Console.ReadLine();
            Console.Write("name : ");
            dr.name = Console.ReadLine();
            Console.Write("email : ");
            dr.email = Console.ReadLine();
            Console.WriteLine("---------------------------");
            currentDoctor = dr;
            Doctor.doctorInSystem.Add(currentDoctor);
            currentDoctor.Menu();
        }
        public void Menu()
        {
            Console.WriteLine("\nPlease make a choice :");
            Console.WriteLine("\t1 - Create Courses");
            Console.WriteLine("\t2 - List Courses");
            Console.WriteLine("\t3 - Log out");
            Console.Write("Enter choice : ");
            int choice = int.Parse(Console.ReadLine());
            if (choice == 1)
                CreateCourse();
            else if (choice == 2)
                ListCourse();
            else if (choice == 3)
                UserFlowControl.LogOut();
            else
            {
                Console.WriteLine("------------------------");
                Console.WriteLine("invalid choice try again ..");
                Menu();
            }
        }
        public void CreateCourse()
        {
            Course course = new Course();
            course.doctor = currentDoctor;
            Console.Write("Code : ");
            course.code = Console.ReadLine();
            Console.Write("Name : ");
            course.name = Console.ReadLine();
            Console.Write("Password : ");
            course.password = UserFlowControl.ReadPassword();
            Console.Write("\nDoes this course has limit number for register? (Yes/No) ");
            string reply = Console.ReadLine();
            if (reply == "Yes")
            {
                Console.Write("Limit num : ");
                course.limit = int.Parse(Console.ReadLine());
            }
            else
            {
                course.limit = null;
            }
            Console.Write("Do you want pre-requisites to this course? (Yes/No) ");
            string ans = Console.ReadLine();
            if (ans == "Yes")
            {
                if (Course.courseInSystem.Count == 0)
                    Console.WriteLine("Sorry!! there are no course in system");
                else
                {
                    int pos = 0;
                    foreach (Course item in Course.courseInSystem)
                    {
                        Console.WriteLine("{0}) Course = {1} with code = {2} taught by Dr = {3}",
                            ++pos, item.name, item.code, item.doctor.name);
                    }
                    label:
                    Console.Write("Which ith [1 - {0}] Course to add? ", pos);
                    int choice = int.Parse(Console.ReadLine());
                    course.preRequisites.Add(Course.courseInSystem[choice - 1]);
                    Console.WriteLine("Done");
                    Console.WriteLine("\n1) Add another one\n2) Exit");
                    if (int.Parse(Console.ReadLine()) == 1)
                        goto label;
                }
            }


            Course.courseInSystem.Add(course);
            currentDoctor.teachingCourse.Add(course);
            Console.WriteLine("Successfuly created");

            Menu();
        }
        public void CreateAssignment()
        {
            Assignment assign = new Assignment();
            Console.Write("Content : ");
            assign.content = Console.ReadLine();
            Console.Write("grade : ");
            assign.grade = int.Parse(Console.ReadLine());
            assign.course = currentCourse;
            currentCourse.assignments.Add(assign);

            Menu();
        }
        public void ListCourse()
        {
            if (currentDoctor.teachingCourse.Count == 0)
            {
                Console.WriteLine("-----------------------");
                Console.WriteLine("Sorry no courses in list");
                Console.WriteLine("---------------------------");
                Menu();
            }
            else
            {
                int pos = 0;
                foreach (var course in currentDoctor.teachingCourse)
                {
                    Console.WriteLine("{0}) Course {1} with code {2}", ++pos, course.name, course.code);
                }
                label:
                Console.Write("Which ith [1 - {0}] course to view? ", pos);
                int choice = int.Parse(Console.ReadLine());
                if (choice > pos)
                {
                    Console.WriteLine("Error !! ...");
                    goto label;
                }
                ViewCourse(choice - 1);
            }
        }
        public void ListAssignment()
        {
            if (currentCourse.assignments.Count == 0)
            {
                Console.WriteLine("Sorry no assignments yet");
                Console.WriteLine("Please make a choice :");
                Console.WriteLine("\t1) Create assignment");
                Console.WriteLine("\t2) back to menu");
                Console.Write("\t   Enter a choice : ");
                int choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                    CreateAssignment();
                else if (choice == 2)
                    Menu();
            }
            else
            {
                int pos = 0;
                foreach (var assign in currentCourse.assignments)
                {
                    Console.WriteLine("Assignment {0} :", ++pos);
                    Console.WriteLine("Question ------> {0} - with grade {1}", assign.content, assign.grade);
                }
                Console.Write("Which [1 - {0}] assignment to view? ", currentCourse.assignments.Count);
                int choice = int.Parse(Console.ReadLine());
                ViewAssignment(choice - 1);
            }
        }
        public void ViewCourse(int choice)
        {
            currentCourse = currentDoctor.teachingCourse[choice];
            Console.WriteLine("Course {0} with Code {1} - total registered {2} students", currentCourse.name, currentCourse.code,
                currentCourse.registeredStudents.Count);
            Console.WriteLine("Course has {0} assignments", currentCourse.assignments.Count);
            label:
            Console.WriteLine("Please make a choice : ");
            Console.WriteLine("1) Create assignment");
            Console.WriteLine("2) List assignments");
            Console.Write("   Enter a choice : ");

            int choose = int.Parse(Console.ReadLine());
            if (choose == 1)
                CreateAssignment();
            else if (choose == 2)
                ListAssignment();
            else
            {
                Console.WriteLine("invalid choice try again ...");
                goto label;
            }
        }
        public void ViewAssignment(int choice)
        {
            Assignment assign = currentCourse.assignments[choice];
            Console.WriteLine("Assignment with question {0} and grade {1} total submitted {2} solution",
                assign.content, assign.grade, assign.assignSolutions.Count);
            Console.WriteLine("Grade Report : ");
            foreach (var solution in assign.assignSolutions)
            {
                if (solution.isgrade == true)
                    Console.WriteLine("Student {0} has degree {1} / {2}",
                        solution.student.name, solution.grade, assign.grade);
                else
                    Console.WriteLine("Student {0} has no dgree", solution.student.name);
            }
            Console.WriteLine("Please make a choice :");
            Console.WriteLine("\t1) View Solution");
            Console.WriteLine("\t2) Back");
            Console.WriteLine("\t  Enter a choice : ");
            int choose = int.Parse(Console.ReadLine());
            if (choose == 1)
                ViewSolution(assign);
            else if (choose == 2)
                Menu();
        }
        public void ViewSolution(Assignment assign)
        {
            Assignment currentAssign = assign;
            if (currentAssign.assignSolutions.Count == 0)
            {
                Console.WriteLine("Sorry no solution for view ...");
                Menu();
            }
            else
            {
                int pos = 0;
                foreach (var solution in currentAssign.assignSolutions)
                {
                    Console.Write("Solution {0} with answer {1} by student {2} ", ++pos, solution.answer, solution.student.name);
                    if (solution.isgrade == false)
                        Console.WriteLine("with no degree");
                    else
                        Console.WriteLine("with degree {0} / {1}", solution.grade, currentAssign.grade);
                }
                Console.WriteLine("Please make a choice :");
                Console.WriteLine("1) Set Degree");
                Console.WriteLine("2) Back");
                int choice = Convert.ToInt16(Console.ReadLine());
                if (choice == 1)
                {
                    Console.WriteLine("Which ith [1 - {0}] solution to set degree? ", currentAssign.assignSolutions.Count);
                    int choose = int.Parse(Console.ReadLine());
                    SetDegree(currentAssign.assignSolutions[choose - 1]);
                }
                else if (choice == 2)
                    Menu();
            }
        }
        public void SetDegree(Solution sol)
        {
            Solution currentSolution = sol;
            currentSolution.isgrade = true;
            Console.Write("Degree : ");
            currentSolution.grade = int.Parse(Console.ReadLine());
            ViewSolution(currentSolution.assignment);
        }
    }
}