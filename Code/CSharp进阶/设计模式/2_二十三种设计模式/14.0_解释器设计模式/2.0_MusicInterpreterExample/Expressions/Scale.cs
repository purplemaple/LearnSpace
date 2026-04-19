using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_MusicInterpreterExample.Expressions
{
    class Scale : Expression
    {
        public override void Excute(string key, double value)
        {
            string scale = "";
            switch(Convert.ToInt32(value))
            {
                case 1:
                    scale = "低音";
                    break;
                case 2:
                    scale = "中音";
                    break;
                case 3:
                    scale = "高音";
                    break;
            }
            Console.Write(scale + " ");
        }
    }
}
