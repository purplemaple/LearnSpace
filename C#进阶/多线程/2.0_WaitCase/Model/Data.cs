using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_WaitCase.Model
{
    public class Data
    {
        public readonly static List<Book> Books = new()
        {
            new Book("封神演义", 1),
            new Book("三国演义", 2),
            new Book("水浒传", 3),
            new Book("西游记", 4),
            new Book("红楼梦", 2),
            new Book("聊斋志异", 1),
            new Book("儒林外史", 2),
            new Book("隋唐演义", 1)
        };

        public readonly static List<string> UrlCollect = new()
        {
            "https://www.sina.com.cn/",
            "https://www.baidu.com/",
            "https://www.csdn.net/",
            "https://www.cnblogs.com/",
            "https://www.oschina.net/",
            "https://mail.163.com/",
            "https://www.aliyun.com/",
            "https://cloud.tencent.com/",
            "https://mp.weixin.qq.com/"
        };
    }
}
