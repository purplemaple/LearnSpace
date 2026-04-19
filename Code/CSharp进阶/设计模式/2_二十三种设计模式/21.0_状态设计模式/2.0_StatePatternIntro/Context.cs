using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_StatePatternIntro
{
    /// <summary>
    /// Context类，维护一个 State 的子类实例，这个实例定义当前的状态
    /// </summary>
    class Context
    {
        private State state;
        public State State 
        {
            get => state;
            set
            {
                state = value;
                Console.WriteLine("当前状态：" + state.GetType().Name);
            } 
        }
        public Context(State state)
        {
            this.State = state;
        }

        //对请求做处理，并设置下一状态
        public void Request()
        {
            state.Handle(this);
        }
    }
}
