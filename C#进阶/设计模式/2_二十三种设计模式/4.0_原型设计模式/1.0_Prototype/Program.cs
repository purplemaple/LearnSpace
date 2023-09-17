﻿namespace _1._0_Prototype
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Resume resume = new Resume("张三");
            /*
             * 浅克隆
             */
            Resume resume2 = (Resume)resume.Clone();

            resume2.Name = "李四";

            Console.WriteLine(resume.Name);     //输出结果为张三
            Console.WriteLine(resume2.Name);    //输出结果为李四
        }

        public abstract class ResumProtoType
        {
            public string Name { get; set; }

            public ResumProtoType(string name)
            {
                this.Name = name;
            }

            public abstract ResumProtoType Clone();
        }

        public class Resume : ResumProtoType
        {
            public Resume(string name) : base(name) { }

            //克隆的方法
            public override ResumProtoType Clone()
            {
                /*
                 * C# 提供的方法: 
                 * MemberwiseClone(): 属于浅克隆(浅拷贝)
                 * 
                 * 浅克隆(浅拷贝):
                 * 1. 值类型以及 string 类型: 开辟新地址, 将数据复制后放入新地址
                 * 2. 引用类型: 不开辟新地址, 将原数据的地址拿来直接用
                 * 
                 * 深克隆(深拷贝):
                 * 1. 不管什么类型: 全都开辟新地址, 复制数据到新地址
                 */
                return (ResumProtoType)this.MemberwiseClone();
            }
        }
    }
}