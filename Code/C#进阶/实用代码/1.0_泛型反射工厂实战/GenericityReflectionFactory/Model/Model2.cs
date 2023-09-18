using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenericityReflectionFactory.MyEnums;

namespace GenericityReflectionFactory.Model
{
    [Factory(ModelType.Model_2)]
    public class Model2 : BaseModel
    {
        public Model2() : base()
        {
            OutString = "Model2 Executing...";
            Console.WriteLine(OutString);
        }
    }
}
