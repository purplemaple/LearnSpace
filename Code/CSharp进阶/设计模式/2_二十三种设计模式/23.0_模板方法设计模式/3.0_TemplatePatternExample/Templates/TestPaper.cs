using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_TemplatePatternExample.Templates
{
    /// <summary>
    /// 金庸小说考题试卷
    /// </summary>
    class TestPaper
    {
        //试题1
        public void TestQuestion1()
        {
            Console.WriteLine("杨过得到过，后来给了郭靖，炼成倚天剑和屠龙刀的玄铁可能是[] a.球磨铸铁 b.马口铁 c.高速合金钢 d.碳素纤维");
            Console.WriteLine("答案：" + Answer1());
        }

        /*
         * 注：精华在这，抄试卷的方法都一样，因此直接在父类中实现防止重复，而一些特性操作，如各自的答题步骤则写成虚方法或者抽象方法，延迟到子类中去实现
         * 这里采用虚方法而不采用抽象方法分原因：虚方法可以有方法体，因此在父类中可实现，这里模仿不答题则答案为空字符串的情况
         */
        protected virtual string Answer1()
        {
            return "";
        }

        //试题2
        public void TestQuestion2()
        {
            Console.WriteLine("杨过、程英、陆无双铲除了情花，造成[] a.使这种植物不再害人 b.使一种珍稀物种灭绝 c.破坏了那个生物圈的生态平衡 d.造成该地区沙漠化");
            Console.WriteLine("答案：" + Answer2());
        }

        protected virtual string Answer2()
        {
            return "";
        }

        //试题3
        public void TestQuestion3()
        {
            Console.WriteLine("蓝风凤凰致使华山师徒、桃谷六仙呕吐不止，如果你是大夫，会给他们开什么药[] a.阿司匹林 b.牛黄解毒片 c.氟哌酸 d.让他们喝大量牛奶 e.以上都不对");
            Console.WriteLine("答案：" + Answer3());
        }

        protected virtual string Answer3()
        {
            return "";
        }
    }
}
