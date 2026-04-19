using _1._0_AttributeAndReflectionCase1.MyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _1._0_AttributeAndReflectionCase1.Model
{
    [Hero]
    public class 剑圣
    {
        [Skill]
        public void 疾风步()
        {
            MessageBox.Show("剑圣 - 疾风步", "提示");
        }

        [Skill]
        public void 镜像分身()
        {
            MessageBox.Show("剑圣 - 镜像分身", "提示");
        }

        [Skill]
        public void 致命一击()
        {
            MessageBox.Show("剑圣 - 致命一击", "提示");
        }

        [Skill]
        public void 剑刃风暴()
        {
            MessageBox.Show("剑圣 - 剑刃风暴", "提示");
        }
    }
}
