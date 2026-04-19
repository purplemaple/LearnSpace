namespace _1._0_NonTemplatePattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("学生甲抄的试卷：");
            TestPaperA studentA = new TestPaperA();
            studentA.TestQuestion1();
            studentA.TestQuestion2();
            studentA.TestQuestion3();

            Console.WriteLine("学生乙抄的试卷：");
            TestPaperB studentB = new TestPaperB();
            studentB.TestQuestion1();
            studentB.TestQuestion2();
            studentB.TestQuestion3();
        }
    }
}