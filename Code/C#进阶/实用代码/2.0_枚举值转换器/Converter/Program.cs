using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Converter
{
    /*
     * 
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            Level level = Level.ISO_4;
            Console.WriteLine(level);   //输出结果为 ISO_4

            Console.WriteLine("------------");

            EnumConverter converter = new EnumConverter(typeof(Level));
            string str = converter.ConvertTo(level, typeof(string)) as string;      //这里只传入了两个必须参数
            Console.WriteLine(str);     //输出结果为 4
        }

        public class EnumConverter : System.ComponentModel.EnumConverter
        {
            public EnumConverter(Type type) : base(type)
            {

            }
            //注：参数列表中的 context 和 culture是可选参数，不赋值时默认为 null， value 和 destinationType 是必须参数
            public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
            {
                if(destinationType == typeof(string))
                {
                    if (value == null)
                    {
                        return string.Empty;
                    }
                    var attribute = value
                       .GetType()
                       .GetField(value.ToString())
                       .GetCustomAttribute<DescriptionAttribute>(false);
                    /* 第一版
                     * Description为 "" 时：       返回枚举值
                     * Description为 " " 时：      返回 " "
                     * Description为 null 时：     返回枚举值
                     * 未标注 Description 特性时：  报 NullReferenceException
                     */
                    //return string.IsNullOrEmpty(attribute.Description) ? value.ToString() : attribute.Description;

                    /* 第一版
                     * Description为 "" 时：       返回 ""
                     * Description为 " " 时：      返回 " "
                     * Description为 null 时：     返回枚举值
                     * 未标注 Description 特性时：  返回枚举值
                     */
                    //return attribute?.Description ?? value.ToString();

                    /* 第一版
                     * Description为 "" 时：       返回枚举值
                     * Description为 " " 时：      返回枚举值
                     * Description为 null 时：     返回枚举值
                     * 未标注 Description 特性时：  返回枚举值
                     * attribute?.Description：   ?.起到一个调用保护作用，当 attribute 为 null 时，会直接返回 null 而不再试图调用 Description，因而不会报空引用异常
                     */
                    return string.IsNullOrWhiteSpace(attribute?.Description) ? value.ToString() : attribute.Description;

                }          
                //传入的操作对象非字符串类型，不作处理，调用基类方法
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        [TypeConverter(typeof(EnumConverter))]
        public enum Level
        {
            [Description("1")]
            ISO_1,

            [Description("2")]
            ISO_2,

            [Description("3")]
            ISO_3,

            [Description(null)]
            ISO_4,

            [Description("5")]
            ISO_5,

            [Description("6")]
            ISO_6,

            [Description("7")]
            ISO_7,

            [Description("8")]
            ISO_8
        }
    }
}