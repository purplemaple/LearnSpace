using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_TemplatePatternExample.Templates
{
    //学生乙抄的试卷
    class TestPaperB : TestPaper
    {
        //只需重写父类的虚方法即可答题
        protected override string Answer1()
        {
            return "c";
        }

        protected override string Answer2()
        {
            return "a";
        }

        protected override string Answer3()
        {
            return "a";
        }
    }
}
