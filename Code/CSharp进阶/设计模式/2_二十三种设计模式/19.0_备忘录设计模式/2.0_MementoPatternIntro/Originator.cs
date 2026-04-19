using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_MementoPatternIntro
{
    /// <summary>
    /// 发起人类(即被保存的对象类)
    /// </summary>
    internal class Originator
    {
        public string State { get; set; }

        //创建备忘录，并将要保存的数据传入
        public Memento CreateMemento()
        {
            return new Memento(State);
        }

        //使用备忘录恢复原始数据
        public void SetMemento(Memento memento)
        {
            State = memento.State;
        }

        public void Show()
        {
            Console.WriteLine("State = " + State);
        }
    }
}
