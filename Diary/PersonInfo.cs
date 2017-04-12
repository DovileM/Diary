namespace Diary
{
    public struct PersonInfo
    {
        public string Name{ set; get; }
        public string Surname { set; get; }
        public PersonInfo(string n, string s) : this()
        {
            Name = n;
            Surname = s;
        }

        public override string ToString()
        {
            return string.Format("{1} {0}", Name, Surname);
        }

    }

}
