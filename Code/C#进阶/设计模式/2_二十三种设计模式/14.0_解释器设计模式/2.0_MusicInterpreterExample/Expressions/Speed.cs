using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_MusicInterpreterExample.Expressions
{
    /*
     * 音速
     */
    class Speed : Expression
    {
        public override void Excute(string key, double value)
        {
            string speed;
            if (value < 500) speed = "快速";
            else if (value >= 1000) speed = " 慢速";
            else speed = "中速";

            Console.Write(speed + " ");
        }
    }
}
