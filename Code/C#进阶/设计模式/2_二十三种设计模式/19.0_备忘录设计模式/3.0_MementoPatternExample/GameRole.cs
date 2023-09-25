using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_MementoPatternExample
{
    internal class GameRole
    {
        //生命力
        public int Vitality { get; set; }

        //攻击力
        public int Attack { get; set; }

        //防御力
        public int Defense { get; set; }

        //保存角色状态
        public RoleStateMemento SaveState()
        {
            return new RoleStateMemento(Vitality, Attack, Defense);
        }

        //恢复角色状态
        public void RecoveryState(RoleStateMemento memento)
        {
            Vitality = memento.Vitality;
            Attack = memento.Attack;
            Defense = memento.Defense;
        }

        //状态显示
        public void StateDisplay()
        {
            Console.WriteLine("角色当前状态");
            Console.WriteLine("体力：" + Vitality);
            Console.WriteLine("攻击力：" + Attack);
            Console.WriteLine("防御力：" + Defense);
            Console.WriteLine("");
        }

        //获得初始状态
        public void GetInitState()
        {
            this.Vitality = 100;
            this.Attack = 100;
            this.Defense = 100;
        }

        //战斗
        public void Fight()
        {
            this.Vitality = 0;
            this.Attack = 0;
            this.Defense = 0;
        }
    }
}
