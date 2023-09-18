using System.Runtime.InteropServices;

namespace _2._0_ProtoTypeSimplify
{
    internal class Program
    {
        /*
         * 由于原型模式的父类基本没什么用, 而且在开发时应用很广泛, 所以 C# 提供统一的简化方案:
         * 需要克隆的类去实现 ICloneable 接口
         */
        static void Main(string[] args)
        {
            //1. 实现接口
            //2. 调用 MemberwiseClone() 

            Resume resume = new Resume();
            resume.SetInfo("孙全", 30, '男');
            resume.SetWorkExperience("2018-2022", "鹅厂");

            Resume resume2 = (Resume)resume.Clone();
            Resume resume3 = (Resume)resume.Clone();

            resume3.SetWorkExperience("2016-2023", "猪厂");

            resume.ShowResume();
            resume2.ShowResume();
            resume3.ShowResume();

            /*
             * 想要实现深拷贝: 使用反射和序列化法
             */
        }

        // 获取引用类型的内存地址方法
        public static string getMemory(object obj)
        {
            GCHandle handle = GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);
            IntPtr addr = GCHandle.ToIntPtr(handle);
            return $"0x{addr.ToString("X")}";
        }

        public class Resume : ICloneable
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public char Gender { get; set; }
            public string TimeArea { get; set; }
            public string Company { get; set; }

            public object Clone()
            {
                return this.MemberwiseClone();
            }

            public void SetInfo(string name, int age, char gender)
            {
                this.Name = name;
                this.Age = age;
                this.Gender = gender;
            }

            public void SetWorkExperience(string timeArea, string company)
            {
                this.TimeArea = timeArea;
                this.Company = company;
            }

            public void ShowResume()
            {
                Console.WriteLine(this.Name + this.Age + this.Gender);
                Console.WriteLine(this.TimeArea + this.Company);
            }
        }
    }
}