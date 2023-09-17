namespace _1._0_TemplateMethod
{
    /*
     * 委托
     * 委托就是 C/C++ 函数指针的升级版
     */
    /*
     * 模板方法案例演示
     */
    class Program
    {
        /*
         * 这种写法的优势：
         * 后续还需要添加其他产品时，Product、Box、WrapProduct 都不需要改动
         * 只需要修改 ProductFactory 中生产产品的方法
         */
        static void Main(string[] args)
        {
            //产品工厂对象，用于生产不同的产品
            ProductFactory productFactory = new();
            //包装工厂对象，用于将不同产品
            WrapFactory wrapFactory = new();

            //定义委托，不同的委托含不同的产品生成方法
            Func<Product> funcPizza = new Func<Product>(productFactory.MakePizza);
            Func<Product> funcToyCar = new Func<Product>(productFactory.MakeToyCar);
            Func<Product> funcKeyBoard = new Func<Product>(productFactory.MakeKeyBoard);

            //将委托传入包装工厂的方法中，返回包装好不同产品的箱子对象
            Box boxPizza = wrapFactory.WrapProduct(funcPizza);
            Box boxToyCar = wrapFactory.WrapProduct(funcToyCar);
            Box boxKeyBoard = wrapFactory.WrapProduct(funcKeyBoard);

            Console.WriteLine(boxPizza.Product.Name);
            Console.WriteLine(boxToyCar.Product.Name);
            Console.WriteLine(boxKeyBoard.Product.Name);
        }
    }

    /// <summary>
    /// 产品类
    /// </summary>
    class Product
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// 箱子类，里面装着产品类
    /// </summary>
    class Box
    {
        public Product Product { get; set; }
    }

    /// <summary>
    /// 包装类，用于创建一个箱子类，然后接收一个产品类，再把产品类放入箱子中，最后返回箱子对象
    /// </summary>
    class WrapFactory
    {
        /// <summary>
        /// 产品打包方法
        /// </summary>
        /// <param name="getProduct">获取产品的委托，外部调用时传入，使用委托提高泛用性</param>
        /// <returns>返回已包装好产品的箱子对象</returns>
        public Box WrapProduct(Func<Product> getProduct)
        {
            Box box = new();
            Product product = getProduct();
            box.Product = product;
            return box;
        }
    }

    /// <summary>
    /// 产品工厂，提供不同的方法以返回不同的产品
    /// </summary>
    class ProductFactory
    {
        //披萨
        public Product MakePizza()
        {
            Product product = new();
            product.Name = "Pizza";
            return product;
        }

        //玩具车
        public Product MakeToyCar()
        {
            Product product = new();
            product.Name = "Toy Car";
            return product;
        }

        //键盘
        public Product MakeKeyBoard()
        {
            Product product = new();
            product.Name = "KeyBoard";
            return product;
        }
    }
}