using _1._0_AttributeAndReflectionCase1.MyAttributes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _1._0_AttributeAndReflectionCase1.Model
{
    [Hero]          //添加英雄特性
    public class 段誉 
    {
        [Skill]     //添加技能特性
        public void 六脉神剑()
        {
            MessageBox.Show("段誉 - 六脉神剑", "提示");
        }

        [Skill]     //添加英雄特性
        public void 凌波微步()
        {
            MessageBox.Show("段誉 - 凌波微步", "提示");
        }
    }
}
