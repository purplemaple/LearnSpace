using GenericityReflectionFactory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenericityReflectionFactory.MyEnums;

namespace GenericityReflectionFactory.Service
{
    [Factory(ModelType.Model_2, typeof(Model2))]
    public class Calc2 : ICalcable
    {
        public Calc2(Model2 model)
        {
            
        }
        public double Calc(double x, double y)
        {
            return x - y;
        }
    }
}
