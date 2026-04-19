using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1._4_EventExample_4
{
    /*
     * 最常用的组合模式
     * 案例4：事件拥有者 是 事件响应者 的子成员(反之亦然)
     */
    class Program
    {
        static void Main(string[] args)
        {
            //3. 事件响应者
            MyForm form = new MyForm();
            form.ShowDialog();
        }
    }

    class MyForm : Form
    {
        private TextBox textBox;

        //1. 事件拥有者
        private Button button;

        public MyForm()
        {
            this.textBox = new TextBox();
            this.button = new Button();
            this.Controls.Add(this.button);
            this.Controls.Add(this.textBox);
            this.button.Text = "Click me";
            this.button.Top = 20;

            //2. 事件
            //5. 事件订阅关系
            this.button.Click += this.ButtonClicked;
        }

        //4. 事件处理器
        private void ButtonClicked(object sender, EventArgs e)
        {
            this.textBox.Text = "hello, world";
        }
    }
}
