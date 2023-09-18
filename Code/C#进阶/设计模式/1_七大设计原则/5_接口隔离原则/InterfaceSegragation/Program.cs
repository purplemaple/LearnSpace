namespace InterfaceSegragation
{
    internal class Program
    {
        /*
         *  接口隔离原则 和 单一职责原则的关系
         *  · 单一职责原则: 一个类只做一件事 / 影响类变化的原因只有一个  =>  高内聚 (要求模块内部的元素相似程度高)
         *  · 接口隔离原则: 模块之间的依赖程度要低  =>  低耦合
         */

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        /// <summary>
        /// 关于操作学生成绩的接口
        /// </summary>
        public interface IScore
        {
            //查询成绩
            void QueryScore();      //学生可以做
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
            //打印成绩单
            void PrintScore();      //学生可以做
            //发送成绩单
            void SendScore();
        }

        

        /*
        public class Teacher : IScore
        {
            //教师类实现所有接口，没问题
        }
        
        public class Student : IScore
        {
            //学生类需要实现额外的接口，不合适
        }
        */

        
    }
}