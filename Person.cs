namespace Terminal.Gui.Issue306
{
    public class Person
    {        
        public string Name {get; set;}
        public string Surname {get; set;}
        public string Nickname {get; set;}

        public override string ToString() => $"{Name} {Surname} ({Nickname})";

        public Person(string name, string surname, string nickname) 
        {
            Name = name;
            Surname = surname;
            Nickname = nickname;
        }
    }
}
