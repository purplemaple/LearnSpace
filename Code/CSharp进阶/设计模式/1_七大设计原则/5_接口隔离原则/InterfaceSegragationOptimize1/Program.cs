namespace InterfaceSegragationOptimize1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        public interface IStudentScore
        {
            //查询成绩
            void QueryScore();
            //打印成绩
            void PrintScore();
        }

        public interface ITeacherScore : IStudentScore
        {
            //修改成绩
            void UpdateScore();
            //添加成绩
            void AddScore();
            //删除成绩
            void DeleteScore();
            //计算总成绩
            double GetSumScore();
            //计算班级平均成绩
            double GetAvgScore();
            //发送成绩单
            void SendScore();
        }

        /*
        public class Teacher : ITeacherScore
        {
            //教师类实现所有接口，没问题
        }

        public class Student : IStudentScore
        {
            //学生类只需实现自己的接口，没问题
        }
        */
    }
}