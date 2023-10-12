using ChatRoomTcpServer.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatRoomTcpServer.ViewModel
{
    internal class ServerViewModel : BindableBase
    {

        public DelegateCommand StartServerCommand => new DelegateCommand(() =>
        {
            //启动服务器
            bool IsStart = ServerModel.CreatSocket();
            if (IsStart)
            {
                ServerModel.ChatTextBox1 += "启动了服务器";
            }
            else
            {
                MessageBox.Show("服务器已启动");
            }
        });


        /// <summary>
        /// 服务端Model
        /// </summary>
        private ServerModel _ServerModel = new();
        public ServerModel ServerModel
        {
            get => _ServerModel;
            set
            {
                SetProperty(ref _ServerModel, value);
            }
        }
    }
}
