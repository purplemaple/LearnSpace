using _3._0_StatePatternExample.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StatePatternExample
{
    /// <summary>
    /// 工作类
    /// </summary>
    public class Work
    {
        private State current;
        public Work()
        {
            current = new ForenoonState();
        }

        public double Hour { get; set; }

        private bool finish = false;
        public bool TaskFinished
        {
            get => finish;
            set => finish = value;
        }

        public void SetState(State s)
        {
            current = s;
        }

        public void WriteProgram()
        {
            current.WriteProgram(this);
        }
    }
}
