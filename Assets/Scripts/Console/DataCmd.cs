public class DataCmd
{
    public string name;
    public Console.Method cmd;
    public string description;
    public override string ToString() => " -> " + description;
}