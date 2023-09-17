using _2._0_MusicInterpreterExample.Expressions;

namespace _2._0_MusicInterpreterExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayContext context = new PlayContext();
            //音乐 - 上海滩
            Console.WriteLine("上海滩：");
            context.PlayText = "T 500 O 2 E 0.5 G 0.5 A 3 E 0.5 G 0.5 D 3 E 0.5 G 0.5 A 0.5 O 3 C 1 O 2 A 0.5 G 1 C 0.5 E 0.5 D 3";
            Expression expression = null;
            try
            {
                while(context.PlayText.Length > 0)
                {
                    string str = context.PlayText.Substring(0, 1);
                    switch (str)
                    {
                        //当首字母是 O 时，则实例化音阶子类
                        case "O":
                            expression = new Scale();
                            break;
                        //当首字母是 T 时，则实例化音速子类
                        case "T":
                            expression = new Speed();
                            break;
                        //当首字母是 CDEFGAB 以及休止符 P 时，则实例化音符子类
                        case "C":
                        case "D":
                        case "E":
                        case "F":
                        case "G":
                        case "A":
                        case "B":
                        case "P":
                            expression = new Note();
                            break;
                    }
                    expression.Interpret(context);
                }
            }catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}