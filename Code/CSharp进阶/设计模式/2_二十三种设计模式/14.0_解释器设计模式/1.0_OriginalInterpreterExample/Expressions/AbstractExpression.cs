using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_OriginalInterpreterExample.Expressions
{

    abstract class AbstractExpression
    {
        public abstract void Interpret(Context context);
    }
}
