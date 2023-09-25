using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_MementoPatternIntro
{
    /// <summary>
    /// 备忘录类
    /// </summary>
    internal class Memento
    {
        private string state;
        public string State
        {
            get => this.state;
        }

        public Memento(string state)
        {
            this.state = state;
        }

    }
}
