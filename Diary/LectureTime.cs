namespace Diary
{
    public struct LectureTime
    {
        public string start;
        public string end;
        public int number;

        public int Number { get { return number; } }

        public LectureTime(string start, string end, int num)
        {
            this.start = start;
            this.end = end;
            number = num;
        }

        public override string ToString()
        {
            return string.Format("{0}. {1} - {2}", number, start, end);
        }
    }
}
