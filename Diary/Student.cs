using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diary
{
    public class Student
    {
        private PersonInfo info;
        public List<Text> mark;
        public List<Text> text;       

        public PersonInfo Info{ get { return info; } }
        public string Category { get; set; }
        public Text GetText(int i)
        {
            return text[i];
        }
        public void AddMark(string text, string date,  string teach)
        {
            mark.Add(new Text(date, text, teach));
        }
        public void AddText(string stext, string date, string teach)
        {
            text.Add(new Text(date, stext, teach));
        }

        public Student(PersonInfo stud, string num)
        {
            text = new List<Text>();
            mark = new List<Text>();

            info = stud;
            Category = num;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", info, Category);
        }

        public string ToStringInfo()
        {
            return string.Format("{0}",info);
        }
        public string ToStringMark(int i)
        {
            return string.Format("{0}", mark[i].ToString());
        }
        public string ToStringText(int i)
        {
            return string.Format("{0}", text[i].ToString());
        }

        public string ToStringMark()
        {
            StringBuilder str = new StringBuilder(String.Empty);
            foreach (Text t in mark)
            {
                str.Append(t.ToString() + "\r\n");
            }
            return str.ToString();
        }

        public string ToStringText()
        {
            StringBuilder str = new StringBuilder(String.Empty);
            foreach(Text t in text)
            {
                str.Append(t.ToString()+"\r\n");
            }
            return str.ToString();
        }
    }



    public struct Text
    {
        public string date;
        public string text;
        public string teacher;

        public Text(string date, string text, string teacher)
        {
            this.text = text;
            this.date = date;
            this.teacher = teacher;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}, {2}    ", date, teacher, text);
        }
    }

    public class SortByNames : IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            string stud = x.ToStringInfo();
            string[] Xstr = stud.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            stud = y.ToStringInfo();
            string[] Ystr = stud.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (x.Category[0] < y.Category[0])
                return -1;
            if (x.Category[0] == y.Category[0])
            {
                if (x.Category[1] < y.Category[1])
                    return -1;
                if (x.Category[1] == y.Category[1])
                    return Xstr[0].CompareTo(Ystr[0]);
                return 1;
            }

            return 1;
        }
    }
}
