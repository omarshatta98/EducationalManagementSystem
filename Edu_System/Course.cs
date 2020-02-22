using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu_System
{
    class Course
    {
        public string code;
        public string name;
        public Doctor doctor;
        public string password;
        public int? limit;
        public List<Course> preRequisites = new List<Course>();
        public List<Student> registeredStudents = new List<Student>();
        public List<Assignment> assignments = new List<Assignment>();
        public List<Post> StudentPosts = new List<Post>();
        public static List<Course> courseInSystem = new List<Course>();

        public static List<Course> ComplementCourses(List<Course> courses)
        {
            List<Course> otherCourses = new List<Course>();

            foreach (var course in courseInSystem)
            {
                bool found = false;

                foreach (var item in courses)
                {
                    if (course == item)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false && CheckPreRequisites(courses, course) &&
                    (course.registeredStudents.Count < course.limit || course.limit == null))
                    otherCourses.Add(course);
            }
            return otherCourses;
        }
        public static bool CheckPreRequisites(List<Course> std_courses, Course course)
        {
            int num = 0;
            foreach (var item in course.preRequisites)
            {
                foreach (var crs in std_courses)
                {
                    if (crs == item)
                    {
                        num++;
                        continue;
                    }
                    else
                        return false;
                }
            }
            if (num == course.preRequisites.Count)
                return true;
            else
                return false;
        }
    }
}
