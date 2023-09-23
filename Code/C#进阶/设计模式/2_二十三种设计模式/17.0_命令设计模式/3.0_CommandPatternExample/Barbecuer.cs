using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_CommandPatternExample
{
    //烤串师傅
    public class Barbecuer
    {
        //烤羊肉串
        public void BakeMutton()
        {
            Console.WriteLine("烤羊肉串");
        }

        //烤鸡翅
        public void BakeChickWing()
        {
            Console.WriteLine("烤鸡翅");
        }
    }
}
