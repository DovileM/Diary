using Diary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Diary
{
    class Program
    {
        private static string lt_date_pattern;
        private static string en_date_pattern;
        private static string name_pattern;
        private static string text_pattern;
        private static string class_pattern;

        static void Main(string[] args)
        {
            List<PersonInfo> studentInf = new List<PersonInfo>();
            List<PersonInfo> teacherInf = new List<PersonInfo>();
            List<LectureInfo> lecture = new List<LectureInfo>();
            List<Student> student = new List<Student>();
            LectureTime[] time = new LectureTime[7];
            Time(time);
            lt_date_pattern = @"^[2][0][1][6-7]-[0-1][0-9]-[0-3][0-9]$";
            en_date_pattern = @"^[0-3][0-9]/[0-1][0-9]/[2][0][1][6-7]$";
            name_pattern = @"^[\p{L}]+ [\p{L}]+$";
            text_pattern = @"^[\p{L}]+$";
            class_pattern = @"^[1]?[0-9][A-G]$";
            int indexer = -1;

            string[] days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            TeacherFileReader(teacherInf, lecture, time);
            foreach (LectureInfo l in lecture)
                l.SortTimetable();
            StudentFileReader(studentInf, student, teacherInf, lecture);

            SortByNames name = new SortByNames();
            student.Sort(name);

            bool tf1 = true;
            while (tf1)
            {
                Console.WriteLine("Choose in which direction do you want to log in: ");
                Console.WriteLine("1 - Teacher");
                Console.WriteLine("2 - Student");
                Console.WriteLine("0 - Exit");
                int no1 = ValidateNumber(Console.ReadLine());
                switch (no1)
                {
                    case 1:
                        int k = 0;
                        string tech = null;
                        while (k == 0)
                        {
                            Console.Write("Please enter a name and surname: ");
                            tech = Console.ReadLine().TrimAndReduce();
                            tech = Validate(tech, name_pattern);
                            Console.WriteLine();
                            int i = 0;
                            foreach (PersonInfo t in teacherInf)
                            {
                                if (tech.Equals(t.ToString()))
                                {
                                    Console.Clear();
                                    Console.WriteLine("\nWelcome, {0}. ", t.ToString());
                                    indexer = i;
                                    k++;
                                }
                                i++;
                            }
                            if (k == 0)
                                Console.WriteLine("Wrong connection.");
                        }
                        bool tf2 = true;
                        while (tf2)
                        {
                            Console.WriteLine("Choose what do you want to do: ");
                            Console.WriteLine("1 - See a timetable.");
                            Console.WriteLine("2 - See a lecture's info.");
                            Console.WriteLine("3 - Create a new lecture.");
                            Console.WriteLine("0 - Log out");
                            int no2 = ValidateNumber(Console.ReadLine());
                            switch (no2)
                            {
                                case 1:
                                    Console.Clear();
                                    Timetable(days, lecture[indexer]);
                                    Console.WriteLine();
                                    break;
                                case 2:
                                    Console.Clear();
                                    Console.WriteLine("Choose:");
                                    bool tf3 = true;
                                    while (tf3)
                                    {
                                        Console.WriteLine("\n1 - Write a mark. ");
                                        Console.WriteLine("2 - Write a remark or commendation. ");
                                        Console.WriteLine("3 - See all marks. ");
                                        Console.WriteLine("4 - See student's marks. ");
                                        Console.WriteLine("5 - See all remarks or commendations. ");
                                        Console.WriteLine("6 - See student's remarks or commendations. ");
                                        Console.WriteLine("0 - Exit");
                                        int no3 = ValidateNumber(Console.ReadLine());
                                        switch (no3)
                                        {
                                            case 1:
                                                EnterText(student, "mark", lecture[indexer].ToString());
                                                break;
                                            case 2:
                                                EnterText(student, "remark or commendation", lecture[indexer].ToString());
                                                break;
                                            case 3:
                                                Console.Clear();
                                                PrintMark(student, "\nALL MARKS:", lecture[indexer].ToString());
                                                break;
                                            case 4:
                                                Console.Write("Enter student's name and surname: ");
                                                string stud = Console.ReadLine().TrimAndReduce().ConvertFirst().ConvertSecond();
                                                stud = Validate(stud, name_pattern);
                                                while (!IsReal(student, stud))
                                                {
                                                    Console.WriteLine("This student doesn't exist.");
                                                    Console.Write("Enter student's name and surname: ");
                                                    stud = Validate(Console.ReadLine().TrimAndReduce().ConvertFirst().ConvertSecond(), name_pattern);
                                                    if (IsReal(student, stud))
                                                        break;
                                                }
                                                Console.Clear();
                                                foreach (Student s in student)
                                                {
                                                    if (stud.Equals(s.ToStringInfo()))
                                                    {
                                                        PrintMark(s, " marks", lecture[indexer].ToString());
                                                        break;
                                                    }
                                                }
                                                break;
                                            case 5:
                                                Console.Clear();
                                                PrintText(student, "\nALL REMARKS OR COMMENDATIONS:", lecture[indexer].ToString());
                                                break;
                                            case 6:
                                                Console.Write("Enter student's name and surname: ");
                                                stud = Console.ReadLine().TrimAndReduce().ConvertFirst().ConvertSecond();
                                                stud = Validate(stud, name_pattern);
                                                while (!IsReal(student, stud))
                                                {
                                                    Console.WriteLine("This student doesn't exist.");
                                                    Console.Write("Enter student's name and surname: ");
                                                    stud = Validate(Console.ReadLine().TrimAndReduce().ConvertFirst().ConvertSecond(), name_pattern);
                                                    if (IsReal(student, stud))
                                                        break;
                                                }
                                                Console.Clear();
                                                foreach (Student s in student)
                                                {
                                                    if (stud.Equals(s.ToStringInfo()))
                                                    {
                                                        PrintText(s, " remarks or commendations", lecture[indexer].ToString());
                                                        break;
                                                    }
                                                }
                                                break;
                                            case 0:
                                                tf3 = false;
                                                Console.Clear();
                                                break;
                                            default:
                                                Console.WriteLine("Wrong choise. Try again.");
                                                break;
                                        }
                                    }
                                    break;

                                case 3:
                                    EnterLecture(lecture[indexer], time, student, lecture);
                                    lecture[indexer].SortTimetable();
                                    break;

                                case 0:
                                    tf2 = false;
                                    Console.Clear();
                                    break;
                                default:
                                    Console.WriteLine("Wrong choise. Try again.");
                                    break;
                            }
                        }
                        break;
                    case 2:
                        k = 0;
                        while (k == 0)
                        {
                            Console.Write("Please enter a name and surname: ");
                            string stud = Console.ReadLine().TrimAndReduce(); ;
                            stud = Validate(stud, name_pattern);
                            Console.WriteLine();
                            int i = 0;
                            foreach (PersonInfo s in studentInf)
                            {
                                if (stud.Equals(s.ToString()))
                                {
                                    Console.Clear();
                                    Console.WriteLine("\nWelcome, {0}. ", s.ToString());
                                    indexer = i;
                                    k++;
                                }
                                i++;
                            }
                            if (k == 0)
                                Console.WriteLine("Wrong connection.");
                        }
                        tf2 = true;
                        while (tf2)
                        {
                            Console.WriteLine("Choose what do you want to do: ");
                            Console.WriteLine("1 - See a timetable.");
                            Console.WriteLine("2 - See all marks.");
                            Console.WriteLine("3 - See all remarks");
                            Console.WriteLine("4 - See all classmates.");
                            Console.WriteLine("0 - Log out");
                            int no3 = ValidateNumber(Console.ReadLine());
                            switch (no3)
                            {
                                case 1:
                                    Console.WriteLine();
                                    Console.Clear();
                                    TimetableStud(days, lecture, student[indexer].Category);
                                    Console.WriteLine();
                                    break;
                                case 2:
                                    Console.WriteLine("\nALL MARKS:");
                                    Console.WriteLine("     {0}:", student[indexer].Info);
                                    Console.WriteLine(student[indexer].ToStringMark());
                                    break;
                                case 3:
                                    Console.WriteLine("\nALL REMARKS OR COMMENDATIONS:");
                                    Console.WriteLine("     {0}:", student[indexer].Info);
                                    Console.WriteLine(student[indexer].ToStringText());
                                    break;
                                case 4:
                                    Console.WriteLine("{0} class:", student[indexer].Category);
                                    var classmates = from s in student
                                                     where student[indexer].Category == s.Category
                                                     select s.Info;
                                    int i = 1;
                                    foreach (var c in classmates)
                                        Console.WriteLine("{0,2}", i++ + ".  " + c);
                                    break;
                                case 0:
                                    tf2 = false;
                                    Console.Clear();
                                    break;
                                default:
                                    Console.WriteLine("Wrong choise. Try again.");
                                    break;
                            }
                        }
                        break;
                    case 0:
                        tf1 = false;
                        TeacherFileWriter(lecture);
                        StudentFileWriter(student);
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Wrong choise. Try again.");
                        break;
                }
            }
            Console.ReadKey();
        }

        #region Methods

        public static void Time(LectureTime[] time)
        {
            time[0].number = 1; time[0].start = " 8:00"; time[0].end = " 8:45";
            time[1].number = 2; time[1].start = " 8:55"; time[1].end = " 9:40";
            time[2].number = 3; time[2].start = " 9:50"; time[2].end = "10:35";
            time[3].number = 4; time[3].start = "10:55"; time[3].end = "11:40";
            time[4].number = 5; time[4].start = "12:00"; time[4].end = "12:45";
            time[5].number = 6; time[5].start = "12:55"; time[5].end = "13:40";
            time[6].number = 7; time[6].start = "13:50"; time[6].end = "14:35";

        }

        public static string Validate(string text, string pattern)
        {
            while (!Regex.IsMatch(text, pattern))
            {
                Console.WriteLine("Wrong format!".ToUpper());
                Console.Write("Write again: ");
                text = Console.ReadLine();
            }
            return text;
        }
        public static string ValidateDate(string text, string pattern1, string pattern2)
        {
            while (!Regex.IsMatch(text, pattern1) && !Regex.IsMatch(text, pattern2))
            {
                Console.WriteLine("Wrong format!".ToUpper());
                Console.Write("Write again: ");
                text = Console.ReadLine();
            }
            return text;
        }


        public static int ValidateNumber(string str)
        {
            int number;
            while (!Int32.TryParse(str, out number))
            {
                Console.WriteLine("Wrong format!".ToUpper());
                Console.Write("Write a number: ");
                str = Console.ReadLine();
            }
            return number;
        }

        public static bool IsReal(List<Student> student, string str)
        {
            foreach (Student s in student)
            {
                if (s.Info.ToString().Equals(str))
                    return true;
            }
            return false;
        }
        public static bool IsOk(List<Student> stud, List<LectureInfo> lecture, string studCathegory, string day, int num)
        {
            foreach (Student s in stud)
            {
                if (s.Category.Equals(studCathegory))
                {
                    for (int i = 0; i < lecture.Count; i++)
                    {
                        for (int j = 0; j < lecture[i].time.Count; j++)
                        {
                            if (s.Category.Equals(lecture[i].time[j].cathegory) && lecture[i].ToStringDay(j).Equals(day)
                                && lecture[i].time[j].number == num)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public static bool IsOk(LectureInfo lecture, string day, int number)
        {
            for (int i = 0; i < lecture.time.Count; i++)
            {
                if (lecture.ToStringDay(i).Equals(day) && lecture.ToIntNumber(i) == number)
                    return false;
            }
            return true;
        }

        #endregion Methods

        #region Print Methods
        public static void PrintMark(List<Student> student, string text, string teacher)
        {
            foreach (Student s in student)
            {
                int k = 0;
                for (int i = 0; i < s.mark.Count; i++)
                {
                    if (s.mark[i].teacher.Equals(teacher))
                    {
                        if (k == 0)
                        {
                            Console.WriteLine(text.ToUpper());
                            Console.WriteLine("     {0}:", s.Info);
                        }
                        Console.WriteLine(s.ToStringMark(i));
                        k++;
                    }
                }
            }
        }
        public static void PrintText(List<Student> student, string text, string teacher)
        {
            foreach (Student s in student)
            {
                int k = 0;
                for (int i = 0; i < s.text.Count; i++)
                {
                    if (s.text[i].teacher.Equals(teacher))
                    {
                        if (k == 0)
                        {
                            Console.WriteLine(text.ToUpper());
                            Console.WriteLine("     {0}:", s.Info);
                        }
                        Console.WriteLine(s.ToStringText(i));
                        k++;
                    }
                }
            }
        }

        public static void PrintMark(Student s, string text, string t)
        {
            var stud = from st in s.mark
                       select st;
            Console.WriteLine(text.ToUpper() + ":");
            foreach (Text st in stud)
                Console.WriteLine(st.ToString());
            if (stud.Count() == 0)
                Console.WriteLine("This student don't have any " + text);
        }
        public static void PrintText(Student s, string text, string t)
        {
            //var stud = from st in s.text
            //           select st;
            var stud = s.text.Select(st => st);
            Console.WriteLine(text.ToUpper() + ":");
            foreach (Text st in stud)
                Console.WriteLine(st.ToString());
            if (stud.Count() == 0)
                Console.WriteLine("This student don't have any " + text);
        }
        #endregion Print Methods

        #region Timetables
        public static void Timetable(string[] days, LectureInfo lecture)
        {
            for (int i = 0; i < days.Length; i++)
            {
                Console.WriteLine("{0}:", days[i].ToUpper());
                for (int j = 0; j < lecture.time.Count; j++)
                {
                    if (days[i].Equals(lecture.ToStringDay(j)))
                    {
                        Console.WriteLine(lecture.ToStringLecture(j));
                    }
                }
            }
        }

        public static void TimetableStud(string[] days, List<LectureInfo> lecture, string cathegory)
        {
            for (int i = 0; i < days.Length; i++)
            {
                Console.WriteLine("{0}:", days[i].ToUpper());
                for (int m = 1; m <= 7; m++)
                {
                    for (int j = 0; j < lecture.Count; j++)

                    {
                        for (int k = 0; k < lecture[j].time.Count; k++)
                        {
                            if (days[i].Equals(lecture[j].ToStringDay(k))
                            && cathegory.Equals(lecture[j].ToStringCathegory(k))
                            && m == lecture[j].time[k].Number)
                            {
                                Console.WriteLine(lecture[j].ToStringLecture(k) + lecture[j].Teacher);
                            }
                        }
                    }
                }
            }
        }
        #endregion Timetables

        #region Insert Methods
        public static void EnterText(List<Student> stud, string str, string t)
        {
            Console.Write("Enter student's name and surname: ");
            string info = Console.ReadLine().TrimAndReduce().ConvertFirst().ConvertSecond();
            info = Validate(pattern: name_pattern, text: info);
            while (!IsReal(stud, info))
            {
                Console.WriteLine("This student doesn't exist.");
                Console.Write("Enter student's name and surname: ");
                info = Validate(text: Console.ReadLine().TrimAndReduce().ConvertFirst().ConvertSecond(), pattern: name_pattern);
                if (IsReal(stud, info))
                    break;
            }
            Console.Write("Enter date: ");
            string date = Console.ReadLine();
            date = ValidateDate(pattern1: lt_date_pattern, text: date, pattern2: en_date_pattern);
            foreach (Student st in stud)
            {
                if (info.Equals(st.Info.ToString()))
                {
                    if (str.Equals("mark"))
                    {
                        Console.Write("Enter {0}: ", str);
                        string text = Console.ReadLine();
                        text = Validate(text, @"^[1]?[0-9]$");
                        st.AddMark(text, date, t);
                    }
                    else
                    {
                        Console.Write("Enter {0}: ", str);
                        string text = Console.ReadLine();
                        st.AddText(text, date, t);
                    }
                }
            }
            Console.WriteLine("SUCCSESSFULY COMPLETED");
        }

        public static void EnterLecture(LectureInfo lecture, LectureTime[] time, List<Student> stud, List<LectureInfo> t)
        {
            string teClass = String.Empty;
            int index = 0;
            string day = String.Empty;
            bool tf = true;
            while (tf)
            {
                Console.Write("Class: ");
                teClass = Console.ReadLine();
                teClass = Validate(pattern: class_pattern, text: teClass);
                Console.Write("Enter the lecture's number: ");
                index = ValidateNumber(Console.ReadLine().TrimAndReduce().ConvertFirst());
                Console.Write("Enter the day: ");
                day = Console.ReadLine();
                day = Validate(pattern: text_pattern, text: day);

                if (!IsOk(stud, t, teClass, day, index))
                {
                    Console.Clear();
                    Console.WriteLine("This class have another lecture.".ToUpperInvariant());
                    Console.WriteLine("Please, write again.");
                }
                else if (!IsOk(lecture, day, index))
                {
                    Console.Clear();
                    Console.WriteLine("This teacher has a lecture on the same date.".ToUpperInvariant());
                    Console.WriteLine("Please, write again.");
                }
                break;
            }


            Days d = (Days)Enum.Parse(typeof(Days), day, true);
            lecture.AddLecture(time[--index].start, time[index].end, ++index, teClass, d);
            Console.WriteLine("SUCCSESSFULY COMPLETED");
        }
        #endregion Insert Methods

        #region Lecture
        public static int Lecture(string d, int num, LectureInfo l)
        {
            for (int i = 0; i < l.time.Count; i++)
            {
                if (d.Equals(l.ToStringDay(i)) && num.Equals(l.ToIntNumber(i)))
                {
                    Console.WriteLine(l.ToStringLecture(i));
                    return i;
                }
            }
            return -1;
        }
        #endregion Lecture

        #region Teacher File Reader / Writer
        public static void TeacherFileReader(List<PersonInfo> teacherInf, List<LectureInfo> lecture, LectureTime[] time)
        {
            string line = "";
            string lecture1 = "";
            int c = 0;
            using (StreamReader sr = new StreamReader("Teacher.txt"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Equals("*"))
                        line = sr.ReadLine();
                    string[] str = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    teacherInf.Add(new PersonInfo(str[0], str[1]));
                    bool tf = true;
                    while ((lecture1 = sr.ReadLine()) != null)
                    {
                        if (lecture1.Equals("*"))
                            break;
                        string cathegory1 = sr.ReadLine();
                        string time1 = sr.ReadLine();
                        string[] str1 = time1.Split(new char[] { ' ', '\t', ',', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        int num1 = Int32.Parse(str1[0]);
                        string day1 = sr.ReadLine();
                        Days d = (Days)Enum.Parse(typeof(Days), day1, true);
                        if (tf)
                        {
                            lecture.Add(new LectureInfo(teacherInf[c], lecture1));
                            tf = false;
                        }
                        lecture[c].AddLecture(time[--num1].start, time[num1].end, ++num1, cathegory1, d);
                    }
                    c++;
                }
            }
        }
        public static void TeacherFileWriter(List<LectureInfo> lecture)
        {
            using (StreamWriter sw = new StreamWriter("Teacher.txt"))
            {
                foreach (LectureInfo lec in lecture)
                {
                    sw.WriteLine("*");
                    sw.WriteLine(lec.ToStringInfo());
                    for (int i = 0; i < lec.time.Count; i++)
                    {
                        sw.WriteLine(lec.ToStringLec());
                        sw.WriteLine(lec.ToStringCathegory(i));
                        sw.WriteLine(lec.ToStringTimer(i));
                        sw.WriteLine(lec.ToStringDay(i));
                    }
                }
            }
        }
        #endregion Teacher File Reader / Writer

        #region Student File Reader / Writer
        public static void StudentFileReader(List<PersonInfo> studentInf, List<Student> student, List<PersonInfo> teacher, List<LectureInfo> lecture)
        {
            using (StreamReader sr = new StreamReader("Student.txt"))
            {
                string cathegory = "";
                int c = -1;
                string info = "";
                string line = "";
                bool tf = true;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Equals("#"))
                    {
                        c++;
                        tf = true;
                        line = sr.ReadLine();
                        string[] str = line.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        studentInf.Add(new PersonInfo(str[0], str[1]));
                        info = str[0] + str[1];
                        cathegory = sr.ReadLine();

                    }
                    else if (line.Equals("*"))
                    {
                        int index = 0;
                        while (true)
                        {
                            line = sr.ReadLine();
                            string[] str = line.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (str.Length == 0)
                                break;
                            else
                            {
                                for (int j = 0; j < teacher.Count; j++)
                                    if (info.Equals(teacher[j].ToString()))
                                        index = j;
                                if (tf)
                                {
                                    student.Add(new Student(studentInf[c], cathegory));
                                    tf = false;
                                }
                                student[c].AddMark(str[4], str[0], lecture[index].ToString());
                            }
                        }
                    }
                    else if (line.Equals("^"))
                    {
                        int index = 0;
                        while (true)
                        {
                            line = sr.ReadLine();
                            string[] str = line.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (str.Length == 0)
                                break;
                            else
                            {
                                string str1 = "";
                                for (int i = 4; i < str.Length; i++)
                                    str1 += str[i] + " ";
                                for (int j = 0; j < teacher.Count; j++)
                                    if (info.Equals(teacher[j].ToString()))
                                        index = j;
                                if (tf)
                                {
                                    student.Add(new Student(studentInf[c], cathegory));
                                    tf = false;
                                }
                                student[c].AddText(str1, str[0], lecture[index].ToString());
                            }
                        }
                    }
                }
            }
        }

        public static void StudentFileWriter(List<Student> student)
        {
            using (StreamWriter sw = new StreamWriter("Student.txt"))
            {
                foreach (Student st in student)
                {
                    sw.WriteLine("#");
                    sw.WriteLine(st.ToStringInfo());
                    sw.WriteLine(st.Category);
                    if (st.mark.Count > 0)
                        sw.WriteLine("*");
                    sw.WriteLine(st.ToStringMark());
                    if (st.text.Count > 0)
                        sw.WriteLine("^");
                    sw.WriteLine(st.ToStringText());
                }
            }
        }
        #endregion Student File Reader / Writer
    }
}

