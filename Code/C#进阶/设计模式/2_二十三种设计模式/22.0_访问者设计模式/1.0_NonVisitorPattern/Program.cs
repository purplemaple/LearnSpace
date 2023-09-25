namespace _1._0_NonVisitorPattern
{
    internal class Program
    {
        /*
         * 本项目演示不使用访问者设计模式的情况
         */
        static void Main(string[] args)
        {
            IList<Person> persons = new List<Person>();

            Person man1 = new Man();
            man1.Action = "成功";
            persons.Add(man1);

            Person woman1 = new Woman();
            woman1.Action = "成功";
            persons.Add(woman1);

            Person man2 = new Man();
            man2.Action = "失败";
            persons.Add(man2);

            Person woman2 = new Woman();
            woman2.Action = "失败";
            persons.Add(woman2);

            Person man3 = new Man();
            man3.Action = "恋爱";
            persons.Add(man3);

            Person woman3 = new Woman();
            woman3.Action = "恋爱";
            persons.Add(woman3);

            foreach(Person person in persons)
            {
                person.GetConclusion();
            }
        }
    }
}