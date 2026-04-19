using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4._0_MulticastDelegateExample
{
    public class Student
    {
        public int ID { get; set; }
        public ConsoleColor PenColor { get; set; }

        public void DoHomework()
        {
            for (int i = 0; i <= 5; i++)
            {
                Console.ForegroundColor = this.PenColor;
                Console.WriteLine("Student{0} doing homework {1} hour(s).", this.ID, i);
                Thread.Sleep(1000);
            }
        }
    }
}
