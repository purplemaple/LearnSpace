namespace _3._0_MementoPatternExample
{
    internal class Program
    {
        /*
         * 备忘录设计模式：在不破坏封装性的前提下，捕获一个对象的内部状态，并在该对象之外制成备份，这样以后就可依靠这个备份将对象恢复到保存时的状态
         * 
         * 特性：有时一些对象的内部信息必须保存在对象以外的地方，但是必须要由对象自己读取，这时备忘录模式可以把复杂的对象内部信息对其他的对象屏蔽起来
         * 
         * 适用情况：功能比较复杂，但需要维护或记录属性历史的类，或者需要保存的属性只是众多属性中的一小部分时
         */
        static void Main(string[] args)
        {
            //大战Boss前
            GameRole role = new GameRole();
            role.GetInitState();
            role.StateDisplay();

            //保存进度
            RoleStateCaretaker stateAdmin = new RoleStateCaretaker();
            stateAdmin.Memento = role.SaveState();

            //大战Boss时，损耗严重
            role.Fight();
            role.StateDisplay();

            //恢复之前的状态
            role.RecoveryState(stateAdmin.Memento);
            role.StateDisplay();
        }
    }
}