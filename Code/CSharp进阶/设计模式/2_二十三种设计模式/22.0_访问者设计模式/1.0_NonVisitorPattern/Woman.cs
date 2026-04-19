using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_NonVisitorPattern
{
    /// <summary>
    /// 女人 类
    /// </summary>
    class Woman : Person
    {
        //得到结论或反应
        public override void GetConclusion()
        {
            if (action == "成功")
            {
                Console.WriteLine("{0}{1}时，背后多半有一个不成功的男人。", this.GetType().Name, action);
            }
            else if (action == "失败")
            {
                Console.WriteLine("{0}{1}时，眼泪汪汪，谁也劝不了。", this.GetType().Name, action);
            }
            else if (action == "恋爱")
            {
                Console.WriteLine("{0}{1}时，遇事懂也装作不懂。", this.GetType().Name, action);
            }
        }
    }
}
