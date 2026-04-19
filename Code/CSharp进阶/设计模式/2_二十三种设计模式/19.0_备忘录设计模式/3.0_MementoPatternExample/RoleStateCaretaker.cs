using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_MementoPatternExample
{
    /// <summary>
    /// 角色状态管理者，用来操作状态保存箱
    /// </summary>
    internal class RoleStateCaretaker
    {
        public RoleStateMemento Memento { get; set; }
    }
}
