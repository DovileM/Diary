using System;
using System.Collections.Generic;
using System.Text;

namespace Diary
{
    public struct LectureInfo
    {
        PersonInfo teacher;
        private string lecture;
        public List<Teacher> time;

        public LectureInfo(PersonInfo info, string lec)
        {
            
            time = new List<Teacher>();

            teacher = info;
            lecture = lec;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder(String.Empty);
            str.Append(ToStringLec() + ",  " + Teacher);
            return str.ToString();
        }
        public string ToStringInfo()
        {
            return string.Format("{0}", teacher);
        }
        public string ToStringLecture(int i)
        {
            return string.Format("     {0}    {1,10}   ", time[i], lecture);
        }
        public string ToStringCathegory(int i)
        {
            return string.Format("{0}", time[i].cathegory);
        }

        public PersonInfo Teacher { get { return teacher; } }
        public string ToStringDay(int i)
        {
            return string.Format("{0}",time[i].day);
        }
        public string ToStringLec()
        {
            return string.Format("{0}", lecture);
        }
        public string ToStringTimer(int i)
        {
            return string.Format(time[i].ToString());
        }
        public int ToIntNumber(int i)
        {
            return time[i].number;
        }

        public void SortTimetable()
        {
            time.Sort();
        }

        public void AddLecture(string start, string end, int no, string c, Days d)
        {
            time.Add(new Teacher(start, end, no, c, d));
        }
        public string ToStringTime()
        {
            StringBuilder str = new StringBuilder(String.Empty);
            int i = 0;
            foreach (Teacher t in time)
            {
                str.Append("     " + t.ToString()+ "  "+ lecture[i] +"\n");
                i++;
            }
            return str.ToString();
        }
    }

    public enum Days { Monday=1, Tuesday, Wednesday,Thursday,Friday};

    public class Teacher : IComparable<Teacher>
    {
        public string start;
        public string end;
        public int number;
        public Days day;
        public string cathegory;

        public int Number { get { return number; } }

        public int CompareTo(Teacher other)
        {
            if (day == other.day)
            {
                return number.CompareTo(other.number);
            }
            return other.day.CompareTo(day);
        }

        public Teacher(string start, string end, int num, string c, Days d)
        {
            this.start = start;
            this.end = end;
            number = num;
            cathegory = c;
            day = d;
        }

        public override string ToString()
        {
            return string.Format("{0}. {1} - {2} {3}", number, start, end, cathegory);
        }
    }

    
}
