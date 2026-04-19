using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_TemplatePatternExample.Templates
{
    //学生甲抄的试卷
    class TestPaperA : TestPaper
    {
        //只需重写父类的虚方法即可答题
        protected override string Answer1()
        {
            return "b";
        }

        protected override string Answer2()
        {
            return "c";
        }

        protected override string Answer3()
        {
            return "a";
        }
    }
}
