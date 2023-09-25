using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_MementoPatternExample
{
    /// <summary>
    /// 角色状态存储箱
    /// </summary>
    internal class RoleStateMemento
    {
        //生命力
        public int Vitality { get; set; }

        //攻击力
        public int Attack { get; set; }

        //防御力
        public int Defense { get; set; }

        public RoleStateMemento(int vit, int atk, int def)
        {
            this.Vitality = vit;
            this.Attack = atk;
            this.Defense = def;
        }
    }
}
