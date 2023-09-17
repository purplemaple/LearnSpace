using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1._0_Multithreading.Model
{
    public class Book
    {
        public Book(string name, int second)
        {
            Name = name;
            Duration = second;
        }
        public string Name { get; }
        public int Duration { get; } = 0;

        private string Result(long milliseconds) 
        {
            string str = Name.PadRight(12, '-') + "用时" + Convert.ToSingle(milliseconds) / 1000 + "秒";
            return str;
        }

        public string Search()
        {
            Stopwatch sw = Stopwatch.StartNew();

            //模拟检索逻辑
            Thread.Sleep(Duration * 1000);

            sw.Stop();
            return Result(sw.ElapsedMilliseconds);
        }

    }
}
