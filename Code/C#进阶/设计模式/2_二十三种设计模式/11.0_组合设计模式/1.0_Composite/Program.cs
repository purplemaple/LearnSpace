namespace _1._0_Composite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Component 根节点    公司
            //Composite 树枝      部门
            //leaf      树叶      员工

            //创建一个公司节点
            Component component = new DepartComposite("上市大公司");

            //创建部门
            Component dep1 = new DepartComposite("总裁办");

            //创建员工
            Component boss = new Emplyee("孙全");

            //把部门添加到公司下
            component.Add(dep1);
            //把员工添加到部门下
            dep1.Add(boss);

            //打印效果 --> 只需打印根节点  -->  传入的3其实是打印时的 ' - ' 数量
            component.Display(3);

        }

        /*
         * 注: 这里用抽象类比接口更好
         * 因为父类中有很多成员需要子类去继承
         */
        public abstract class Component
        {
            public string Name { get; }

            public Component(string name)
            {
                this.Name = name;
            }

            public abstract void Add(Component component);
            public abstract void Remove(Component component);
            public abstract void Display(int depth);
        }

        /// <summary>
        /// 部门类 --> 树枝
        /// </summary>
        public class DepartComposite : Component
        {
            //继承父类的构造方法, 用于注入 name
            public DepartComposite(string name) : base(name) { }

            //存储部门或者员工
            private List<Component> listComponent = new List<Component>();

            public override void Add(Component component)
            {
                listComponent.Add(component);
            }

            public override void Remove(Component component)
            {
                listComponent.Remove(component);
            }

            public override void Display(int depth) 
            {
                Console.WriteLine(new string('-', depth) + this.Name);

                foreach(var item in listComponent)
                {
                    //用到了递归思想  -->  因为没办法一开始就知道一共有多少层
                    item.Display(depth + 4);
                }
            }
        }

        /// <summary>
        /// 员工类 --> 树叶, 树叶无法继续添加或删除子集
        /// </summary>
        public class Emplyee : Component
        {
            //继承父类的构造方法, 用于注入 name
            public Emplyee(string name) : base(name) { }

            //树叶节点不能添加也不能删除, 因此这里只继承, 不实现
            public override void Add(Component component)
            {
                throw new NotImplementedException();
            }

            public override void Remove(Component component)
            {
                throw new NotImplementedException();
            }

            public override void Display(int depth)
            {
                Console.WriteLine(new string('-', depth) + this.Name);
            }
        }
    }
}