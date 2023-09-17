using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_MusicInterpreterExample.Expressions
{
    abstract class Expression
    {
        /* 解释器
         * 因为不管是 音符 Note 还是 音阶 Scale，前部分对字符串的操作都是相同的，因此这里逻辑直接写在抽象父类中，不同的部分抽成 Excute, 再交给子类实现
         */
        public void Interpret(PlayContext context)
        {
            if (context.PlayText.Length == 0) return;
            else
            {
                //从第0个字符开始(含)，往后截取1个字符
                string playKey = context.PlayText.Substring(0, 1);
                //从第3个字符开始(含)，截取之后的所有字符(这里第二个参数不写，就表示往后截取所有)
                context.PlayText = context.PlayText.Substring(2);

                //对新的字符串从0开始往。后截取到第一个空格位置前的字符(即不含空格)
                double playValue = Convert.ToDouble(context.PlayText.Substring(0, context.PlayText.IndexOf(" ")));
                context.PlayText = context.PlayText.Substring(context.PlayText.IndexOf(" ") + 1);

                Excute(playKey, playValue);
            }
        }

        public abstract void Excute(string key, double value);
    }
}
