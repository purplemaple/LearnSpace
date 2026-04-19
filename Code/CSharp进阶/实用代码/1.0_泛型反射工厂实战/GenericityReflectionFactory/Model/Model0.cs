using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenericityReflectionFactory.MyEnums;

namespace GenericityReflectionFactory.Model
{
    [Factory(ModelType.Model_0)]
    public class Model0 : BaseModel
    {
        public Model0() : base()
        {
            OutString = "Model0 Executing...";
            Console.WriteLine(OutString);
        }
  
    }
}
