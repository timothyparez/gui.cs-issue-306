
public class Hero
{
    public string Name {get; set;}
    public string Surname {get; set;}
    public string Nickname {get; set;}

    public Hero()
    {

    }
    public Hero(string name, string surname, string nickname)
    {
        Name = name;
        Surname = surname;
        Nickname = nickname;
    }

    public override string ToString() => $"{Name} {Surname} ({Nickname})";
}