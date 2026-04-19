using _1._0_OriginalInterpreterExample.Expressions;

namespace _1._0_OriginalInterpreterExample
{
    /*
     * 解释器设计模式：
     * 给定一个语言，定义它语法的一种表示方式，并定义一个解释器，这个解释器使用该表示方式来解释语言中的句子
     * 
     * 使用情况：自定义一套语言解释语法，用于简化地解释特定语句中的复杂语法
     * 如案例2中：
     *      语法结构为 大写字母 + 空格 + 浮点数 + 空格....... ---> "T 500 O 2 E 0.5 G 0.5 A 3"
     *      解释为：字母 ---> 音符或音阶或音速；空格 ---> 分隔符；浮点数 ---> 音符或音阶或音速的值
     *      上面语句解释后为：中速     中音     Mi(半拍)     So(半拍)     La(一拍)...
     *                     (T 500)  (O 2)   (E 0.5)     (G 0.5)      (A 3) 
     * 
     */
    /*
     * 本项目演示原始的解释器案例
     */
    internal class Program
    {
        /*
         * 客户端代码，
         */
        static void Main(string[] args)
        {
            Context context = new Context();
            IList<AbstractExpression> list = new List<AbstractExpression>
            {
                new TerminalExpression(),
                new NonterminalExpression(),
                new TerminalExpression(),
                new TerminalExpression()
            };

            foreach (AbstractExpression expression in list)
            {
                expression.Interpret(context);
            }
        }
    }
}