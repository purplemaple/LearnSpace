using GenericityReflectionFactory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenericityReflectionFactory.MyEnums;

namespace GenericityReflectionFactory.Service
{
    [Factory(ModelType.Model_1, typeof(Model1))]
    public class Calc1 : ICalcable
    {
        public Calc1(Model1 model)
        {
            
        }
        public double Calc(double x, double y)
        {
            return x * y;
        }
    }
}
