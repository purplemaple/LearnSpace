using ChatRoomTcpClient_2.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatRoomTcpClient_2.ViewModel
{
    internal class ClientViewModel : BindableBase
    {
        private bool isConnected = false;
        public DelegateCommand ConnectCommand => new DelegateCommand(() =>
        {
            if (!isConnected)
            {
                isConnected = ClientModel.CreatSocket();
            }
            else
            {
                MessageBox.Show("已经成功连接服务器，不要重复连接。");
            }

        });

        public DelegateCommand SendCommand => new DelegateCommand(() =>
        {
            string message = ClientModel.ClientInfo + ">：" + ClientModel.SendTextBox;
            if (!string.IsNullOrEmpty(message))
            {
                //调用客户端的发送方法，向服务器端发送消息
                ClientModel.SendMessage(message);

                //清空客户端的发送框
                ClientModel.SendTextBox = "";
            }
            else
            {
                MessageBox.Show("不能发送空消息！");
            }

        });


        /// <summary>
        /// 客户端1 Model
        /// </summary>
        private ClientModel _ClientModel = new();
        public ClientModel ClientModel
        {
            get => _ClientModel;
            set
            {
                SetProperty(ref _ClientModel, value);
            }
        }
    }
}
