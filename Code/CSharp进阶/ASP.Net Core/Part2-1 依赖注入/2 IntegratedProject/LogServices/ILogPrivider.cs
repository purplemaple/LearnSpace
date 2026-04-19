using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogServices;

public interface ILogPrivider
{
    public void LogError(string msg);

    public void LogInfo(string msg);
}

public class ConsoleLogPrivider : ILogPrivider
{
    public void LogError(string msg)
    {
        Console.WriteLine("Error: " + msg);
    }

    public void LogInfo(string msg)
    {
        Console.WriteLine("Info: " + msg);
    }
}
