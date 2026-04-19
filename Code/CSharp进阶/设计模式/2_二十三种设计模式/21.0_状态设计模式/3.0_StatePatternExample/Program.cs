namespace _3._0_StatePatternExample
{
    internal class Program
    {
        /*
         * 状态模式：当一个对象的内在状态改变时允许改变其行为，这个对象看起来像是改变了其类
         * 
         * 意义：主要解决的是当控制一个对象状态转换的条件表达式过于复杂时的情况。把状态的判断逻辑转移到表示不同状态的一系列类当中，可以把复杂的判断逻辑简化
         * 
         * 优点：消除庞大的条件分支语句，状态模式通过把各种状态转移逻辑分布到 State 的子类之间，来减少相互依赖，新增或者删减时，通过增删子类可以很容易地增加新的状态和转换
         */
        static void Main(string[] args)
        {
            //紧急项目
            Work emergencyProjects = new Work();
            emergencyProjects.Hour = 9;
            emergencyProjects.WriteProgram();
            emergencyProjects.Hour = 10;
            emergencyProjects.WriteProgram();
            emergencyProjects.Hour = 12;
            emergencyProjects.WriteProgram();
            emergencyProjects.Hour = 13;
            emergencyProjects.WriteProgram();
            emergencyProjects.Hour = 14;
            emergencyProjects.WriteProgram();
            emergencyProjects.Hour = 17;
            emergencyProjects.WriteProgram();

            //emergencyProjects.TaskFinished = true;
            emergencyProjects.TaskFinished = false;

            emergencyProjects.WriteProgram();
            emergencyProjects.Hour = 19;
            emergencyProjects.WriteProgram();
            emergencyProjects.Hour = 22;
            emergencyProjects.WriteProgram();
        }
    }
}