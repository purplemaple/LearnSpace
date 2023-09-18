namespace _0._0_PrototypeIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //简历中包含: 姓名、性别、年龄、工作经验等需求: 复制三份简历对象

            Resume resume = new Resume();
            resume.SetInfo("张三", 28, '男');
            resume.SetWorkExperience("2018-2020", "腾讯.Net高级开发工程师");

            //需求: 复制两份简历, 加上原件, 这三份简历的地址不能相同

            /*
             * 这种方式不方便添加与修改 
             */
            Resume resume2 = new Resume();
            resume2.SetInfo("张三", 28, '男');
            resume2.SetWorkExperience("2018-2020", "腾讯.Net高级开发工程师");

            Resume resume3 = new Resume();
            resume3.SetInfo("张三", 28, '男');
            resume3.SetWorkExperience("2018-2020", "腾讯.Net高级开发工程师");


            resume.ShowResume();
            resume2.ShowResume();
            resume3.ShowResume();
        }

        public class Resume
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public char Gender { get; set; }
            public string TimeArea { get; set; }
            public string Company { get; set; }

            public void SetInfo(string name, int age, char gender)
            {
                this.Name = name;

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