using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _1._0_DataFilter.Helpers
{
    /*
     * 此类是实现界面上控件显示水印(提示信息)的工具类，使用前需在 xmal 里导入
     */

    //定义一个附加属性类
    public class WatermarkHelper : DependencyObject
    {
        //注册一个附加属性Watermark，用于指定提示文本
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(WatermarkHelper), new PropertyMetadata(null));

        //获取附加属性Watermark的值
        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        //设置附加属性Watermark的值
        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }
    }
}
