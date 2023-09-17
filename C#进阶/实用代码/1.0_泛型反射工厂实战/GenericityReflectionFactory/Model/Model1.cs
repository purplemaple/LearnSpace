using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenericityReflectionFactory.MyEnums;

namespace GenericityReflectionFactory.Model
{
    [Factory(ModelType.Model_1)]
    public class Model1 : BaseModel
    {
        public Model1() : base()
        {
            OutString = "Model1 Executing...";
            Console.WriteLine(OutString);
        }
    }
}
