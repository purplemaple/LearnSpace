using GenericityReflectionFactory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenericityReflectionFactory.MyEnums;

namespace GenericityReflectionFactory.Service
{
    [Factory(ModelType.Model_0, typeof(Model0))]
    public class Calc0 : ICalcable
    {
        public Calc0(Model0 model)
        {
            
        }
        public double Calc(double x, double y)
        {
            return x + y;
        }
    }
}
