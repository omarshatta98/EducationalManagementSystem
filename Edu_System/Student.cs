using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu_System
{
    class Student
    {
        public int id;
        public string name;
        public string username;
        public string password;
        public string email;
        static Student currentStudent;
        public List<Email> myInbox = new List<Email>();
        public List<Course> registeredCourses = new List<Course>();
        public List<Solution> assignSolutions = new List<Solution>();
        public static List<Student> studentInSystem = new List<Student>();


        public void Menu()
        {
            Console.SetCursorPosition(110, Console.CursorTop);
            Console.WriteLine("---------");
            Console.SetCursorPosition(110, Console.CursorTop);
            Console.WriteLine("| Email |");
            Console.SetCursorPosition(110, Console.CursorTop);
            Console.WriteLine("---------");
            Console.WriteLine("\nPlease make a choice : ");
            Console.WriteLine("\t1)Register in course");
            Console.WriteLine("\t2)List my course");
            Console.WriteLine("\t3)Grades report");
            Console.WriteLine("\t4) View E-mail");
            Console.WriteLine("\t5)Log Out");
            Console.Write("\tEnter a choice : ");
            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine("------------------------");
            switch (choice)
            {
                case 1:
                    RegisterCourse();
                    break;
                case 2:
                    ListCourse();
                    break;
                case 3:
                    GradesReport();
                    break;
                case 4:
                    Email();
                    break;
                case 5:
                    UserFlowControl.LogOut();
                    break;
                default:
                    Console.WriteLine("Please make correct choice");
                    Console.WriteLine("--------------------------------");
                    Menu();
                    break;
            }
        }
        public static Student IsValidUser(string username, string password)
        {
            for (int i = 0; i < studentInSystem.Count; i++)
            {
                if (studentInSystem[i].username == username && studentInSystem[i].password == password)
                    return studentInSystem[i];
            }
            return null;
        }
        public static void GetUser(Student std)
        {
            currentStudent = std;
            currentStudent.Menu();
        }
        public static void SignUp()
        {
            int ID = 1000;
            Student std = new Student();
            std.id = ID++;
            Console.Write("User name : ");
            std.username = Console.ReadLine();
            Console.Write("Password : ");
            std.password = Console.ReadLine();
            Console.Write("Name : ");
            std.name = Console.ReadLine();
            Console.Write("Email : ");
            std.email = Console.ReadLine();
            Console.WriteLine("---------------------------");

            Student.studentInSystem.Add(std);
            currentStudent = std;
            currentStudent.Menu();
        }
        public void ListCourse()
        {
            if (currentStudent.registeredCourses.Count == 0)
            {
                Console.WriteLine("\nNo course for show");
                Console.WriteLine("--------------------------");
                Menu();
            }
            else
            {
                Console.WriteLine("\nMy course list : ");
                int pos = 0;
                foreach (var course in registeredCourses)
                {
                    Console.WriteLine("{0}) course {1} - Code {2}", ++pos, course.name, course.code);
                }
                label:
                Console.Write("Which [1 - {0}] course to view ? ", pos);
                int choice = int.Parse(Console.ReadLine());
                if (choice > pos)
                {
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Error !! invalid choice .....");
                    Console.WriteLine("-----------------------------");
                    goto label;
                }
                ViewCourse(choice - 1);
            }
        }
        public void RegisterCourse()
        {
            List<Course> otherCourse = Course.ComplementCourses(currentStudent.registeredCourses);
            if (otherCourse.Count == 0)
                Console.WriteLine("No avaliable course yet\n-------------------------------------");
            else
            {
                Console.WriteLine("\nAvaliable course for you : ");
                int pos = 0;
                foreach (var course in otherCourse)
                {
                    Console.WriteLine("{0}) Course {1} - Code {2}", ++pos, course.name, course.code);
                }
                label:
                Console.Write("Which ith [1 - {0}] course to register ? ", pos);
                int choice = int.Parse(Console.ReadLine());
                if (choice > pos)
                {
                    Console.WriteLine("------------------------------------");
                    Console.WriteLine("Error !! please enter correct choice ......");
                    Console.WriteLine("------------------------------------");
                    goto label;
                }
                l:
                Console.Write("Password : ");
                string pass = UserFlowControl.ReadPassword();
                if (pass == otherCourse[choice - 1].password)
                {
                    currentStudent.registeredCourses.Add(otherCourse[choice - 1]);
                    otherCourse[choice - 1].registeredStudents.Add(currentStudent);
                    Console.WriteLine("Registered Successfully");
                }
                else
                {
                    Console.WriteLine("Incorrect password !! try again...");
                    goto l;
                }
            }
            Menu();
        }
        public void ViewCourse(int choice)
        {
            Console.WriteLine("------------------------");
            Course course = currentStudent.registeredCourses[choice];
            Console.WriteLine("\ncourse {0} with code {1} - taught by Dr {2}", course.name, course.code, course.doctor.name);
            Console.WriteLine("course have {0} assignments", course.assignments.Count);
            int pos = 0;
            foreach (var assign in course.assignments)
            {
                Console.Write("Assignment {0} ", ++pos);
                Solution sol = GetAssignSolution(assign);
                if (sol == null)
                    Console.Write("Not submitted - NA");
                else
                {
                    Console.Write("Submitted ");
                    if (sol.isgrade == false)
                        Console.Write("NA");
                    else
                        Console.Write(sol.grade);
                }
                Console.Write(" / {0}", assign.grade);
                Console.WriteLine(" ");
            }
            if (course.StudentPosts.Count > 0)
            {
                Console.WriteLine("----------------------------------");
                Console.WriteLine("Course Posts : ");
                int x = 0;
                foreach (var item in course.StudentPosts)
                {
                    Console.WriteLine("{0}) {1}: {2}", ++x, item.student, item.content);
                }
                Console.WriteLine("-----------------------------------");
                Console.Write("Do you want make/see replies? (Yes/No) ");
                string ans = Console.ReadLine();
                if (ans == "Yes")
                {
                    Console.Write("Which ith [1 - {0}] post? ", x);
                    int i = int.Parse(Console.ReadLine());
                    MakeReply(course.StudentPosts[i - 1]);
                }
            }
            label:
            Console.WriteLine("Please make a choice : ");
            Console.WriteLine("\t1) Unregister from course");
            Console.WriteLine("\t2) Submitt solution");
            Console.WriteLine("\t3) Make a Post");
            Console.WriteLine("\t4) Back");
            Console.Write("Enter a choice : ");
            int choose = int.Parse(Console.ReadLine());
            if (choose == 1)
                UnregisterCourse(course);
            else if (choose == 2)
            {
                if (pos == 0)
                {
                    Console.WriteLine("sorry there are no assignment to submit...");
                    goto label;
                }
                else
                {
                    lab:
                    Console.Write("Which ith [1 - {0}] assign to submit? ", pos);
                    int num = int.Parse(Console.ReadLine());
                    if (num > pos)
                    {
                        Console.WriteLine("------------------------");
                        goto lab;
                    }
                    SubmitAssignment(course.assignments[num - 1]);
                }
            }
            else if (choose == 3)
                MakePost(course);
            else if (choose == 4)
                Menu();
            else
            {
                Console.WriteLine("invalid choice try again");
                goto label;
            }
        }
        public void UnregisterCourse(Course course)
        {
            currentStudent.registeredCourses.Remove(course);
            course.registeredStudents.Remove(currentStudent);
            Console.WriteLine("Unregister Succssesfuly !!!");
            Menu();
        }
        public void SubmitAssignment(Assignment assign)
        {
            Assignment currentAssign = assign;
            Solution sol = new Solution();
            Console.WriteLine("\nQ1) {0}", currentAssign.content);
            Console.WriteLine("Enter Solution : ");
            sol.answer = Console.ReadLine();
            sol.assignment = assign;
            sol.student = currentStudent;
            currentAssign.assignSolutions.Add(sol);
            currentStudent.assignSolutions.Add(sol);
            Menu();
        }
        public Solution GetAssignSolution(Assignment assign)
        {
            foreach (var solution in currentStudent.assignSolutions)
            {
                if (assign == solution.assignment)
                    return solution;
            }
            return null;
        }
        public void GradesReport()
        {
            if (currentStudent.assignSolutions.Count == 0)
                Console.WriteLine("there are no degree for you");
            foreach (var course in currentStudent.registeredCourses)
            {
                int totalDegree = 0, stdDegree = 0, stdSubmit = 0;
                foreach (var assign in course.assignments)
                {
                    totalDegree += assign.grade;
                    foreach (var solution in currentStudent.assignSolutions)
                    {
                        if (assign == solution.assignment)
                        {
                            stdSubmit++;
                            stdDegree += solution.grade;
                            break;
                        }
                    }
                }
                if (course.assignments.Count == 0)
                    continue;
                Console.WriteLine("\nCourse {0} with code {1} total submitted {2} assignments grade {3} / {4}",
                    course.name, course.code, stdSubmit, stdDegree, totalDegree);
            }
            Menu();
        }
        public void MakePost(Course crs)
        {
            Course currentCourse = crs;
            Post p = new Post();
            Console.Write("Content : ");
            p.content = Console.ReadLine();
            p.student = currentStudent.name;
            currentCourse.StudentPosts.Add(p);
            Menu();
        }
        public void MakeReply(Post P)
        {
            Post currentPost = P;
            Console.WriteLine("{0}: {1}", P.student, P.content);
            foreach (var item in P.reply)
            {
                Console.WriteLine("{0} : {1}", item.student, item.content);
            }
            Post reply = new Post();
            Console.Write("Reply : ");
            reply.content = Console.ReadLine();
            reply.student = currentStudent.name;
            currentPost.reply.Add(reply);
        }
        public void Email()
        {
            if (myInbox.Count > 0)
            {
                Console.WriteLine("------------------------------");
                int pos = 0;
                foreach (var item in myInbox)
                {
                    if (item.reply.Count == 0)
                        Console.WriteLine("{0}) {1}:  {2}   (+{3}) ..... ({1})",
                            ++pos, UserFlowControl.GetUserByEmail(item.from).name,
                            item.content, item.reply.Count);
                    else
                    {
                        Student std = UserFlowControl.GetUserByEmail(item.reply[item.reply.Count - 1].from);
                        Console.WriteLine("{0}) {1}:  {2}   (+{3} .... ({4}))", ++pos, std.name,
                            item.reply[item.reply.Count - 1].content, item.reply.Count,
                            UserFlowControl.GetUserByEmail(item.reply[item.reply.Count - 1].to).name);
                    }
                }

                Console.WriteLine("------------------------------");
            }
            label:
            Console.WriteLine("1) Sent");
            Console.WriteLine("2) Reply");
            Console.WriteLine("3) Back");
            int ans = int.Parse(Console.ReadLine());
            if (ans == 1)
            {
                Email e = new Email();
                e.from = currentStudent.email;
                Console.Write("TO : ");
                e.to = Console.ReadLine();
                Console.Write("Content ");
                e.content = Console.ReadLine();


                currentStudent.myInbox.Add(e);
                Student other_std = UserFlowControl.GetUserByEmail(e.to);
                other_std.myInbox.Add(e);
                Email();
            }
            else if (ans == 2)
            {
                if (myInbox.Count > 0)
                {
                    Console.WriteLine("which ith [1 - {0}] email to reply", myInbox.Count);
                    int choice = int.Parse(Console.ReadLine());
                    Console.WriteLine("------------------------------");
                    Console.WriteLine("\nAll Conversition : \n");
                    Console.WriteLine("{0}: {1}", UserFlowControl.GetUserByEmail(myInbox[choice - 1].from).name,
                        myInbox[choice - 1].content);
                    foreach (var item in myInbox[choice - 1].reply)
                    {
                        Student std = UserFlowControl.GetUserByEmail(item.from);
                        Console.WriteLine("{0}: {1}", std.name, item.content);
                    }
                    Console.WriteLine("------------------------------\n");
                    Email e = new Email();
                    e.from = currentStudent.email;
                    e.to = UserFlowControl.GetUserByEmail(myInbox[choice - 1].from).email;
                    Console.Write("Reply : ");
                    e.content = Console.ReadLine();
                    myInbox[choice - 1].reply.Add(e);
                    Email();
                }
                else
                {
                    Console.WriteLine("Sorry ther are no msg to reply");
                    goto label;
                }
            }
            else if (ans == 3)
            {
                Menu();
            }
            else
            {
                Console.WriteLine("Error!! invalid choice try again");
                goto label;
            }
        }
    }
}
