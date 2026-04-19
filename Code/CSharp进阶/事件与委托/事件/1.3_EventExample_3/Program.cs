using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1._3_EventExample_3
{
    /*
     * 案例3：事件的拥有者同时也是事件的响应者
     */
    class Program
    {
        static void Main(string[] args)
        {
            //1. form：事件拥有者
            //3. form：事件响应者
            MyForm form = new MyForm();

            //2. Click：事件
            //5. 事件订阅关系
            form.Click += form.FormClicked;
            form.ShowDialog();
        }
    }

    class MyForm : Form
    {
        //4. 事件处理器
        internal void FormClicked(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToString();
        }
    }
}
