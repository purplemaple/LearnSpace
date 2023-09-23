namespace _2._0_CommandPatternAbstract
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Receiver receiver = new Receiver();
            Command command = new ConcreteCommand(receiver);
            Invoke invoke = new Invoke();

            invoke.SetCommand(command);
            invoke.ExecuteCommand();
        }
    }
}