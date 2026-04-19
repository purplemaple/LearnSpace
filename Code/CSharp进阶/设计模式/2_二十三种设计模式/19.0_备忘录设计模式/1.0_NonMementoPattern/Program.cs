namespace _1._0_NonMementoPattern
{
    internal class Program
    {
        /*
         * 演示不使用备忘录模式的情况，整个游戏角色的细节都暴露给了客户端
         */
        static void Main(string[] args)
        {
            //大战Boss前
            GameRole role = new GameRole();
            role.GetInitState();
            role.StateDisplay();

            //保存进度
            GameRole backup = new GameRole();
            backup.Vitality = role.Vitality;
            backup.Attack = role.Attack;
            backup.Defense = role.Defense;

            //大战Boss前，损耗严重
            role.Fight();
            role.StateDisplay();

            //恢复之前的状态
            role.Vitality = backup.Vitality;
            role.Attack = backup.Attack;
            role.Defense = backup.Defense;

            role.StateDisplay();
        }
    }
}