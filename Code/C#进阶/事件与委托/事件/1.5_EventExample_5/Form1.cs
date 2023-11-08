using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1._5_EventExample_5
{
    /*
     * 案例5：
     * 
     */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            /*
             * 注：这里 button3 手写订阅关系，button1 和 button2 的订阅关系由 Form 设计器自动生成，源代码在 Form1.Designer.cs 文件里
             */
            //this.button3.Click += ButtonClicked;

            //用 -= 取消订阅事件
            //this.button3.Click -= new EventHandler(this.ButtonClicked);

            //老式挂载事件处理器的写法
            //this.button3.Click += new EventHandler(this.ButtonClicked);

            //老式委托挂载事件处理器的写法
            /*this.button3.Click += delegate (object sender, EventArgs e) {
                this.textBox1.Text = "haha!";
            };*/
            
            //Lambda表达式 + 匿名委托挂载事件处理器的写法
            this.button3.Click += (sender, e) =>
            {
                this.textBox1.Text = "hoho!";
            };
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            if(sender == this.button1)
            {
                this.textBox1.Text = "Hello!";
            }

            if(sender == this.button2)
            {
                this.textBox1.Text = "World!";
            }

            if(sender == this.button3)
            {
                this.textBox1.Text = "Mr.Okay!";
            }
        }
    }
}
