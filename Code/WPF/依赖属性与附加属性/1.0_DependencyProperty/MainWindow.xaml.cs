using System;
using System.Windows;
using System.Windows.Controls;

namespace _1._0_DependencyProperty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CustomTextBox box = new();
            //使用封装的简便赋值法
            box.IsHighlighted = false;
            //不使用封装的原始赋值法
            box.SetValue(CustomTextBox.IsHighlightedProperty, true);

        }
    }

    #region 依赖属性 Denpendency Property
    internal class CustomTextBox : TextBox
    {
        #region 依赖属性写法
        /// <summary>
        /// 其实是对依赖属性的一层封装，提供对依赖属性的简便访问方式
        /// </summary>
        public bool IsHighlighted
        {
            /*
             * 注意这里用到的 GetValue 和 SetValue 方法，是属于 DependencyObject 基类的。
             * 这就是为什么依赖属性只能存在于 DependencyObject 内部，不然就用不了这两个方法
             */
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        /// <summary>
        /// 依赖属性
        /// </summary>
        public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(
            "IsHighlighted",                //注册的名字
            typeof(bool),                   //属性的类型 --> 因为依赖属性本身是一个类型，因此不能直接用关键字注明类型。
            typeof(CustomTextBox),          //从属于的类 --> 本依赖属性写在哪个类里，这里就写哪个
            new PropertyMetadata(false, IsHightedChanged)     //默认值
        );

        //回调函数，当 IsHighted 依赖属性发生变化时执行
        private static void IsHightedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //回调函数挂载事件法
            var box = d as CustomTextBox;
            box!.TextChanged += BoxTextChanged;
        }

        private static void BoxTextChanged(object sender, TextChangedEventArgs e)
        {
            //do something
        }
        #endregion

        #region 只读依赖属性的写法

        //依赖属性
        public static readonly DependencyProperty HasTextProperty;
        //DependencyPropertyKey 用于封装依赖属性(给依赖属性再套一层壳)
        public static readonly DependencyPropertyKey HasTextPropertyKey;

        /* 要在类的静态构造里对 DependencyPropertyKey 进行赋值
         * 
         * 因为类创建后 HasTextPropertyKey 还是 null，因此不能直接赋给 HasTextProperty，需要先在静态构造中把自己注册出来才能赋给别人
         */
        static CustomTextBox()
        {
            HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
                "HasText",
                typeof(bool),
                typeof(CustomTextBox),
                new PropertyMetadata(false)
            );
            //再将套壳的数据真正赋给依赖属性 DependencyProperty
            HasTextProperty = HasTextPropertyKey.DependencyProperty;
        }

        //最后再用普通属性给依赖属性封装一下，简化读取操作
        public bool HasText => (bool)GetValue(HasTextProperty);
        #endregion
    }
    #endregion

    #region 附加属性 Attach Property
    internal class TextBoxHelper
    {
        #region 附加属性写法
        /*
                
                *//*
                 * 注：
                 *     1. 因为附加属性可以添加到多种控件上，所以在自定义时尚不知道要用在哪个控件上，因此用到依赖反转
                 *        在参数中传入 DependencyObject，然后调用这个 DependencyObject 的 GetValue 和 SetValue 即可
                 *     2. 如果要指定只能附加到某个控件上，则只能把 DependencyObject 改成该控件
                 *     
                 *//*
                public static bool GetHasText(DependencyObject obj)
                {
                    return (bool)obj.GetValue(HasTextProperty);
                }

                public static void SetHasText(DependencyObject obj, bool value)
                {
                    obj.SetValue(HasTextProperty, value);
                }

                public static readonly DependencyProperty HasTextProperty =
                    DependencyProperty.RegisterAttached(    //与依赖属性不同的地方：这里是 RegisterAttached
                        "HasText", 
                        typeof(bool), 
                        typeof(TextBoxHelper), 
                        new PropertyMetadata(false)
                    );


                #region 用于监控 HasText 的依赖属性，因为如果直接在 HasText 添加回调函数，则值改变时会循环调用，因此再套一层依赖属性
                public static bool GetMonitirTextChange(DependencyObject obj)
                {
                    return (bool)obj.GetValue(MonitirTextChangeProperty);
                }

                public static void SetMonitirTextChange(DependencyObject obj, bool value)
                {
                    obj.SetValue(MonitirTextChangeProperty, value);
                }

                public static readonly DependencyProperty MonitirTextChangeProperty =
                    DependencyProperty.RegisterAttached(
                        "MonitirTextChange", 
                        typeof(bool), 
                        typeof(TextBoxHelper), 
                        new PropertyMetadata(false, MonitorTextChangedPropertyChanged)
                    );

                private static void MonitorTextChangedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
                {
                    if (d is not TextBox box)
                        return;

                    //如果 e 的值为 true，即在前台设置 Monitor 的值为 true，则挂载事件
                    if ((bool)e.NewValue)
                    {
                        box.TextChanged += Box_TextChanged;
                        SetHasText(box!, !string.IsNullOrEmpty(box!.Text));     //提前执行一下，防止前端 Text 有默认值且语句在 Monitor 之前时出现的 BUG
                    }
                    //前台设置为 false，卸载事件
                    else
                    {
                        box.TextChanged -= Box_TextChanged;
                    }
                }

                private static void Box_TextChanged(object sender, TextChangedEventArgs e)
                {
                    //前面已经做了判断，box 肯定是 TextBox ，且不为 null，因此这里直接转
                    var box = sender as TextBox;
                    //使用 ! 可以不让程序警报
                    SetHasText(box!, !string.IsNullOrEmpty(box!.Text) );
                }
                #endregion
                
        */
        #endregion

        #region 只读附加属性写法

        /*
         * 注：
         *     1. 因为附加属性可以添加到多种控件上，所以在自定义时尚不知道要用在哪个控件上，因此用到依赖反转
         *        在参数中传入 DependencyObject，然后调用这个 DependencyObject 的 GetValue 和 SetValue 即可
         *     2. 如果要指定只能附加到某个控件上，则只能把 DependencyObject 改成该控件
         */
        public static bool GetHasText(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasTextProperty);
        }

        protected static void SetHasText(DependencyObject obj, bool value)
        {
            //注意这里是 Key 而不是 DP
            obj.SetValue(HasTextPropertyKey, value);
        }

        public static readonly DependencyProperty HasTextProperty;
        /*
         * 类似于依赖属性，再套一层 key 即可
         * 类创建时 Key 还是 null 因此同样要在静态构造中将 Key 赋给 HasTextProperty
         * 
         * 注意：因为已经改成只读了，所以对于 Set 包装器有两种处理方法
         *      1. 直接删除，要赋值时使用原始方法，对 Key 赋值
         *      2. 修饰改成 protected 并且改成对 Key 赋值而非直接对 DenpendencyProperty 赋值
         */
        public static readonly DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly(    
                "HasText",
                typeof(bool),
                typeof(TextBoxHelper),
                new PropertyMetadata(false)
            );
        //静态构造
        static TextBoxHelper()
        {
            HasTextProperty = HasTextPropertyKey.DependencyProperty;
        }


        #region 用于监控 HasText 的依赖属性，因为如果直接在 HasText 添加回调函数，则值改变时会循环调用，因此再套一层依赖属性
        public static bool GetMonitorTextChange(DependencyObject obj)
        {
            return (bool)obj.GetValue(MonitorTextChangeProperty);
        }

        public static void SetMonitorTextChange(DependencyObject obj, bool value)
        {
            obj.SetValue(MonitorTextChangeProperty, value);
        }

        public static readonly DependencyProperty MonitorTextChangeProperty =
            DependencyProperty.RegisterAttached(
                "MonitorTextChange",
                typeof(bool),
                typeof(TextBoxHelper),
                new PropertyMetadata(false, MonitorTextChangedPropertyChanged)
            );

        private static void MonitorTextChangedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox box)
                return;

            //如果 e 的值为 true，即在前台设置 Monitor 的值为 true，则挂载事件
            if ((bool)e.NewValue)
            {
                box.TextChanged += Box_TextChanged;
                /*
                 * 法一：这是只读时，直接删除 Set 包装器的写法，直接对 Key 赋值
                 */
                //box?.SetValue(HasTextPropertyKey, !string.IsNullOrEmpty(box.Text));
                /*
                 * 法二：使用修改成 Key 的Set 包装器
                 */
                SetHasText(box!, !string.IsNullOrEmpty(box!.Text));
            }
            //前台设置为 false，卸载事件
            else
            {
                box.TextChanged -= Box_TextChanged;
            }
        }

        private static void Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            //前面已经做了判断，box 肯定是 TextBox ，且不为 null，因此这里直接转
            var box = sender as TextBox;
            /*
             * 法一：这是只读时，直接删除 Set 包装器的写法，直接对 Key 赋值
             */
            //box?.SetValue(HasTextPropertyKey, !string.IsNullOrEmpty(box.Text));
            /*
             * 法二：使用修改成 Key 的Set 包装器
             */
            SetHasText(box!, !string.IsNullOrEmpty(box!.Text));
        }
        #endregion

        #endregion
    }
    #endregion
}